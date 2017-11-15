#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SourceCode.Chasm
{
    [DebuggerDisplay("{ToString(),nq,ac}")]
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public readonly struct TreeMap : IReadOnlyDictionary<string, TreeMapNode>, IReadOnlyList<TreeMapNode>, IReadOnlyDictionary<string, TreeRef>, IReadOnlyList<TreeRef>, IEquatable<TreeMap>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        #region Constants

        /// <summary>
        /// A singleton representing an empty <see cref="TreeMap"/> value.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static TreeMap Empty { get; }

        #endregion

        #region Fields

        internal readonly ReadOnlyMemory<TreeMapNode> _nodes;

        #endregion

        #region Properties

        public int Count => _nodes.Length;

        TreeRef IReadOnlyList<TreeRef>.this[int index]
        {
            get
            {
                if (_nodes.IsEmpty)
                    return Array.Empty<TreeRef>()[index]; // Throw underlying exception

                var span = _nodes.Span;

                return span[index].TreeRef;
            }
        }

        TreeRef IReadOnlyDictionary<string, TreeRef>.this[string key]
        {
            get
            {
                if (!TryGetValue(key, out var node))
                    throw new KeyNotFoundException(nameof(key));

                return node.TreeRef;
            }
        }

        public TreeMapNode this[int index]
        {
            get
            {
                if (_nodes.IsEmpty)
                    return Array.Empty<TreeMapNode>()[index]; // Throw underlying exception

                var span = _nodes.Span;

                return span[index];
            }
        }

        public TreeMapNode this[string key]
        {
            get
            {
                if (!TryGetValue(key, out var node))
                    throw new KeyNotFoundException(nameof(key));

                return node;
            }
        }

        #endregion

        #region Constructors

        public TreeMap(params TreeMapNode[] nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null || nodes.Length == 0)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            // Sort & de-duplicate
            _nodes = DistinctSort(nodes, false);
        }

        public TreeMap(IEnumerable<TreeMapNode> nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            var copy = nodes.ToArray();

            if (copy == null)
            {
                _nodes = default;
                return;
            }

            // Sort & de-duplicate
            _nodes = DistinctSort(copy, true);
        }

        public TreeMap(ICollection<TreeMapNode> nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null || nodes.Count == 0)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            // Copy
            var array = new TreeMapNode[nodes.Count];
            nodes.CopyTo(array, 0);

            // Sort & de-duplicate
            _nodes = DistinctSort(array, true);
        }

        private TreeMap(bool sentinel, ReadOnlyMemory<TreeMapNode> nodes)
        {
            _nodes = nodes;
        }

        #endregion

        #region Methods

        private static ReadOnlyMemory<TreeMapNode> Merge(TreeMap first, TreeMap second)
        {
            var newArray = new TreeMapNode[first.Count + second.Count];

            var i = 0;
            var aIndex = 0;
            var bIndex = 0;
            for (; aIndex < first.Count || bIndex < second.Count; i++)
            {
                if (aIndex >= first.Count)
                {
                    newArray[i] = second[bIndex++];
                }
                else if (bIndex >= second.Count)
                {
                    newArray[i] = first[aIndex++];
                }
                else
                {
                    var a = first[aIndex];
                    var b = second[bIndex];
                    var cmp = string.CompareOrdinal(a.Name, b.Name);

                    if (cmp == 0)
                    {
                        newArray[i] = b;
                        ++bIndex;
                        ++aIndex;
                    }
                    else if (cmp < 0)
                    {
                        newArray[i] = a;
                        ++aIndex;
                    }
                    else
                    {
                        newArray[i] = b;
                        ++bIndex;
                    }
                }
            }

            return new ReadOnlyMemory<TreeMapNode>(newArray, 0, i);
        }

        public TreeMap Merge(TreeMapNode node)
        {
            if (_nodes.IsEmpty) return new TreeMap(node);

            var index = IndexOf(node.Name);

            var span = _nodes.Span;

            TreeMapNode[] array;
            if (index >= 0)
            {
                array = new TreeMapNode[_nodes.Length];
                span.CopyTo(array);
                array[index] = node;
            }
            else
            {
                index = ~index;
                array = new TreeMapNode[_nodes.Length + 1];

                var j = 0;
                for (var i = 0; i < array.Length; i++)
                    array[i] = i == index ? node : span[j++];
            }

            return new TreeMap(array);
        }

        public TreeMap Merge(string name, TreeRef treeRef) => Merge(new TreeMapNode(name, treeRef));

        public TreeMap Merge(string name, NodeKind kind, Sha1 sha1) => Merge(new TreeMapNode(name, kind, sha1));

        public TreeMap Merge(TreeMap nodes)
        {
            if (nodes == default || nodes.Count == 0)
                return this;

            if (_nodes.IsEmpty || _nodes.Length == 0)
                return nodes;

            var set = Merge(this, nodes);

            var tree = new TreeMap(true, set);
            return tree;
        }

        public TreeMap Merge(ICollection<TreeMapNode> nodes) => Merge(new TreeMap(nodes));

        public TreeMap Merge(IEnumerable<TreeMapNode> nodes) => Merge(new TreeMap(nodes));

        public TreeMap Remove(string key)
        {
            if (_nodes.Length == 0) return this;

            var copy = new TreeMapNode[_nodes.Length - 1];
            var span = _nodes.Span;
            var found = false;
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (found)
                {
                    copy[i - 1] = span[i];
                }
                else
                {
                    if (i < _nodes.Length - 1)
                        copy[i] = span[i];
                    found = StringComparer.Ordinal.Equals(span[i].Name, key);
                }
            }

            if (found)
                return new TreeMap(true, new ReadOnlyMemory<TreeMapNode>(copy));

            return this;
        }

        public TreeMap RemoveWhere(Func<string, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (_nodes.Length == 0) return this;

            var copy = new TreeMapNode[_nodes.Length - 1];
            var span = _nodes.Span;
            var j = 0;
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (!predicate(span[i].Name))
                    copy[j++] = span[i];
            }

            if (j == _nodes.Length)
                return this;

            return new TreeMap(true, new ReadOnlyMemory<TreeMapNode>(copy, 0, j));
        }

        public int IndexOf(string key)
        {
            if (_nodes.IsEmpty || key == null) return -1;

            var l = 0;
            var r = _nodes.Length - 1;
            var i = r / 2;
            var ks = key;

            var span = _nodes.Span;

            while (r >= l)
            {
                var cmp = string.CompareOrdinal(span[i].Name, ks);
                if (cmp == 0) return i;
                else if (cmp > 0) r = i - 1;
                else l = i + 1;

                i = l + (r - l) / 2;
            }

            return ~i;
        }

        public bool ContainsKey(string key) => IndexOf(key) >= 0;

        public bool TryGetValue(string key, out TreeMapNode value)
        {
            value = default;

            if (_nodes.IsEmpty || key == null)
                return false;

            var l = 0;
            var r = _nodes.Length - 1;
            var i = r / 2;
            var ks = key;

            var span = _nodes.Span;

            while (r >= l)
            {
                value = span[i];

                var cmp = string.CompareOrdinal(value.Name, ks);
                if (cmp == 0) return true;

                if (cmp > 0) r = i - 1;
                else l = i + 1;

                i = l + (r - l) / 2;
            }

            value = default;
            return false;
        }

        bool IReadOnlyDictionary<string, TreeRef>.TryGetValue(string key, out TreeRef value)
        {
            if (TryGetValue(key, out var node))
            {
                value = node.TreeRef;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryGetValue(string key, NodeKind kind, out TreeMapNode value)
        {
            value = default;

            if (!TryGetValue(key, out var node))
                return false;

            if (node.TreeRef.Kind != kind)
                return false;

            value = node;
            return true;
        }

        #endregion

        #region Helpers

        private static ReadOnlyMemory<TreeMapNode> DistinctSort(TreeMapNode[] array, bool alreadyCopied)
        {
            Debug.Assert(array != null); // Already checked at callsites

            // Optimize for common cases 0, 1, 2, N
            ReadOnlyMemory<TreeMapNode> result;
            switch (array.Length)
            {
                case 0:
                    return default;

                case 1:
                    // Always copy. The callsite may change the array after creating the TreeMap.
                    result = new TreeMapNode[1] { array[0] };
                    return result;

                case 2:
                    // If the Name (alone) is duplicated
                    if (StringComparer.Ordinal.Equals(array[0].Name, array[1].Name))
                    {
                        // If it's a complete duplicate, silently skip
                        if (TreeMapNodeComparer.Default.Equals(array[0], array[1]))
                            result = new TreeMapNode[1] { array[0] };
                        else
                            throw CreateDuplicateException(array[0]);
                    }
                    // In the wrong order
                    else if (TreeMapNodeComparer.Default.Compare(array[0], array[1]) > 0)
                    {
                        result = new TreeMapNode[2] { array[1], array[0] };
                    }
                    else if (alreadyCopied)
                    {
                        result = array;
                    }
                    else
                    {
                        result = new TreeMapNode[2] { array[0], array[1] };
                    }
                    return result;

                default:

                    // If callsite did not already copy, do so before mutating
                    var nodes = array;
                    if (!alreadyCopied)
                    {
                        nodes = new TreeMapNode[array.Length];
                        array.CopyTo(nodes, 0);
                    }

                    // Sort: Delegate dispatch faster than interface (https://github.com/dotnet/coreclr/pull/8504)
                    Array.Sort(nodes, TreeMapNodeComparer.Default.Compare);

                    // Distinct
                    var j = 1;
                    for (var i = 1; i < nodes.Length; i++)
                    {
                        // If the Name (alone) is duplicated
                        if (StringComparer.Ordinal.Equals(nodes[i - 1].Name, nodes[i].Name))
                        {
                            // If it's a complete duplicate, silently skip
                            if (TreeMapNodeComparer.Default.Equals(nodes[i - 1], nodes[i]))
                                continue;

                            throw CreateDuplicateException(nodes[0]);
                        }
                        nodes[j++] = nodes[i]; // Increment target index if distinct
                    }

                    var span = new ReadOnlyMemory<TreeMapNode>(nodes, 0, j);
                    return span;
            }

            ArgumentException CreateDuplicateException(TreeMapNode node)
                => new ArgumentException($"Duplicate {nameof(TreeMapNode)} arguments passed to {nameof(TreeMap)}: ({node})");
        }

        #endregion

        #region IEnumerable

        public IEnumerable<string> Keys
        {
            get
            {
                if (_nodes.IsEmpty)
                    yield break;

                for (var i = 0; i < _nodes.Length; i++)
                    yield return _nodes.Span[i].Name;
            }
        }

        public IEnumerable<TreeMapNode> Values
        {
            get
            {
                if (_nodes.IsEmpty)
                    yield break;

                for (var i = 0; i < _nodes.Length; i++)
                    yield return _nodes.Span[i];
            }
        }

        IEnumerable<TreeRef> IReadOnlyDictionary<string, TreeRef>.Values
        {
            get
            {
                if (_nodes.IsEmpty)
                    yield break;

                for (var i = 0; i < _nodes.Length; i++)
                    yield return _nodes.Span[i].TreeRef;
            }
        }

        public IEnumerator<TreeMapNode> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<TreeRef> IEnumerable<TreeRef>.GetEnumerator()
        {
            if (_nodes.IsEmpty)
                yield break;

            for (var i = 0; i < _nodes.Length; i++)
                yield return _nodes.Span[i].TreeRef;
        }

        IEnumerator<KeyValuePair<string, TreeMapNode>> IEnumerable<KeyValuePair<string, TreeMapNode>>.GetEnumerator()
        {
            if (_nodes.IsEmpty)
                yield break;

            for (var i = 0; i < _nodes.Length; i++)
                yield return new KeyValuePair<string, TreeMapNode>(_nodes.Span[i].Name, _nodes.Span[i]);
        }

        IEnumerator<KeyValuePair<string, TreeRef>> IEnumerable<KeyValuePair<string, TreeRef>>.GetEnumerator()
        {
            if (_nodes.IsEmpty)
                yield break;

            for (var i = 0; i < _nodes.Length; i++)
                yield return new KeyValuePair<string, TreeRef>(_nodes.Span[i].Name, _nodes.Span[i].TreeRef);
        }

        #endregion

        #region IEquatable

        public bool Equals(TreeMap other) => TreeMapComparer.Default.Equals(this, other);

        public override bool Equals(object obj)
            => obj is TreeMap tree
            && TreeMapComparer.Default.Equals(this, tree);

        public override int GetHashCode() => TreeMapComparer.Default.GetHashCode(this);

        #endregion

        #region Operators

        public static bool operator ==(TreeMap x, TreeMap y) => TreeMapComparer.Default.Equals(x, y);

        public static bool operator !=(TreeMap x, TreeMap y) => !(x == y);

        public override string ToString() => $"{nameof(Count)}: {_nodes.Length}";

        #endregion
    }
}
