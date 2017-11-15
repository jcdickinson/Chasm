#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Buffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Chasm
{
#pragma warning disable CA1710 // Identifiers should have correct suffix

    public readonly struct TreeSet : IReadOnlyList<TreeRef>, IEquatable<TreeSet>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        #region Constants

        /// <summary>
        /// A singleton representing an empty <see cref="TreeSet"/> value.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static TreeSet Empty { get; }

        #endregion

        #region Fields

        internal readonly ReadOnlyMemory<TreeRef> _nodes;

        #endregion

        #region Properties

        public int Count => _nodes.Length;

        public TreeRef this[int index]
        {
            get
            {
                if (_nodes.IsEmpty)
                    return Array.Empty<TreeRef>()[index]; // Throw underlying exception

                var span = _nodes.Span;
                return span[index];
            }
        }

        #endregion

        #region Constructors

        private TreeSet(bool sentinel, ReadOnlyMemory<TreeRef> nodes)
        {
            _nodes = nodes;
        }

        public TreeSet(params TreeRef[] nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null || nodes.Length == 0)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            var copy = new TreeRef[nodes.Length];
            nodes.CopyTo(copy);
            _nodes = DistinctSort(copy);
        }

        public TreeSet(IEnumerable<TreeRef> nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            _nodes = DistinctSort(nodes.ToArray());
        }

        public TreeSet(ICollection<TreeRef> nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null || nodes.Count == 0)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            var array = new TreeRef[nodes.Count];
            nodes.CopyTo(array, 0);
            _nodes = DistinctSort(array);
        }

        #endregion

        #region Helpers

        private static ReadOnlyMemory<TreeRef> DistinctSort(Memory<TreeRef> memory)
        {
            if (memory.IsEmpty) return memory;
            var span = memory.Span;

            switch (span.Length)
            {
                case 1:
                    return memory;

                case 2:
                    if (TreeRefComparer.Default.Compare(span[0], span[1]) > 0)
                        return new TreeRef[2] { span[1], span[0] };
                    else
                        return memory;

                default:
                    span.Sort(TreeRefComparer.Default.Compare);

                    // Distinct
                    var j = 1;
                    for (var i = 1; i < span.Length; i++)
                    {
                        if (TreeRefComparer.Default.Equals(span[i - 1], span[i]))
                            continue;
                        span[j++] = span[i];
                    }

                    return memory.Slice(0, j);
            }
        }

        #endregion

        #region IEquatable

        public static bool operator ==(TreeSet list1, TreeSet list2)
            => list1.Equals(list2);

        public static bool operator !=(TreeSet list1, TreeSet list2)
            => !(list1 == list2);

        public override bool Equals(object obj)
                            => obj is TreeSet o
            && TreeSetComparer.Default.Equals(this, o);

        public bool Equals(TreeSet other)
            => TreeSetComparer.Default.Equals(this, other);

        public override int GetHashCode()
            => TreeSetComparer.Default.GetHashCode(this);

        #endregion

        #region Union

        private static ReadOnlyMemory<TreeRef> UnionWith(ReadOnlyMemory<TreeRef> firstMemory, ReadOnlyMemory<TreeRef> secondMemory)
        {
            if (firstMemory.IsEmpty) return secondMemory;
            if (secondMemory.IsEmpty) return firstMemory;

            var first = firstMemory.Span;
            var second = secondMemory.Span;

            var newArray = new TreeRef[first.Length + second.Length];

            var i = 0;
            var aIndex = 0;
            var bIndex = 0;
            for (; aIndex < first.Length || bIndex < second.Length; i++)
            {
                if (aIndex >= first.Length)
                {
                    newArray[i] = second[bIndex++];
                }
                else if (bIndex >= second.Length)
                {
                    newArray[i] = first[aIndex++];
                }
                else
                {
                    var a = first[aIndex];
                    var b = second[bIndex];
                    var cmp = TreeRefComparer.Default.Compare(a, b);

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

            return new ReadOnlyMemory<TreeRef>(newArray, 0, i);
        }

        public TreeSet Add(TreeRef node)
        {
            if (_nodes.IsEmpty) return new TreeSet(node);

            var index = IndexOf(node);

            var span = _nodes.Span;

            TreeRef[] array;
            if (index >= 0) return this;

            index = ~index;
            array = new TreeRef[_nodes.Length + 1];

            var j = 0;
            for (var i = 0; i < array.Length; i++)
                array[i] = i == index ? node : span[j++];

            return new TreeSet(true, array);
        }

        public TreeSet Add(NodeKind kind, Sha1 sha1) => Add(new TreeRef(kind, sha1));

        public TreeSet UnionWith(TreeSet nodes)
        {
            if (nodes == default || nodes.Count == 0)
                return this;

            if (_nodes.IsEmpty || _nodes.Length == 0)
                return nodes;

            var set = UnionWith(_nodes, nodes._nodes);

            var tree = new TreeSet(true, set);
            return tree;
        }

        #endregion

        #region Exclude

        private static ReadOnlyMemory<TreeRef> ExceptWith(ReadOnlyMemory<TreeRef> firstMemory, ReadOnlyMemory<TreeRef> secondMemory)
        {
            if (firstMemory.IsEmpty) return firstMemory;
            if (secondMemory.IsEmpty) return firstMemory;

            var first = firstMemory.Span;
            var second = secondMemory.Span;

            var newArray = new TreeRef[first.Length];

            var i = 0;
            var j = 0;
            var k = 0;
            while (i < first.Length)
            {
                if (j >= second.Length)
                {
                    newArray[k++] = first[i++];
                    continue;
                }

                var cmp = TreeRefComparer.Default.Compare(first[i], second[j]);
                if (cmp < 0)
                {
                    newArray[k++] = first[i++];
                }
                else if (cmp > 0)
                {
                    j++;
                }
                else if (cmp == 0)
                {
                    i++;
                    j++;
                }
            }

            return new ReadOnlyMemory<TreeRef>(newArray, 0, k);
        }

        public TreeSet Remove(TreeRef treeRef)
        {
            var index = IndexOf(treeRef);
            if (index < 0) return this;

            var copy = new TreeRef[_nodes.Length - 1];
            var span = copy.AsSpan();

            if (index > 0)
            {
                _nodes.Span.Slice(0, index).CopyTo(span);
            }

            if (index < _nodes.Length)
            {
                _nodes.Span.Slice(index + 1).CopyTo(span.Slice(index));
            }

            return new TreeSet(true, copy);
        }

        public TreeSet Remove(NodeKind kind, Sha1 sha1) => Remove(new TreeRef(kind, sha1));

        public TreeSet ExceptWith(TreeSet treeSet)
        {
            var hs = new HashSet<int>();

            if (_nodes.IsEmpty) return this;
            if (treeSet._nodes.IsEmpty) return this;

            var set = new TreeSet(true, ExceptWith(_nodes, treeSet._nodes));
            return set;
        }

        #endregion

        #region IntersectWith

        private static ReadOnlyMemory<TreeRef> IntersectWith(ReadOnlyMemory<TreeRef> firstMemory, ReadOnlyMemory<TreeRef> secondMemory)
        {
            if (firstMemory.IsEmpty) return firstMemory;
            if (secondMemory.IsEmpty) return secondMemory;

            var first = firstMemory.Span;
            var second = secondMemory.Span;

            var newArray = new TreeRef[Math.Max(first.Length, second.Length)];

            var i = 0;
            var j = 0;
            var k = 0;
            while (i < first.Length && j < second.Length)
            {
                var cmp = TreeRefComparer.Default.Compare(first[i], second[j]);
                if (cmp < 0)
                {
                    i++;
                }
                else if (cmp > 0)
                {
                    j++;
                }
                else if (cmp == 0)
                {
                    newArray[k++] = first[i];
                    i++;
                    j++;
                }
            }

            return new ReadOnlyMemory<TreeRef>(newArray, 0, k);
        }

        public bool Contains(TreeRef treeRef) => IndexOf(treeRef) >= 0;

        public bool Contains(NodeKind kind, Sha1 sha1) => Contains(new TreeRef(kind, sha1));

        public TreeSet IntersectWith(TreeSet treeSet)
        {
            if (_nodes.IsEmpty) return this;
            if (treeSet._nodes.IsEmpty) return treeSet;

            var set = new TreeSet(true, IntersectWith(_nodes, treeSet._nodes));
            return set;
        }

        #endregion

        #region IsSetOf

        private static (int Left, int Right, int Count) GetSetDiscriminator(ReadOnlySpan<TreeRef> left, ReadOnlySpan<TreeRef> right)
        {
            var i = 0;
            var j = 0;
            var k = 0;
            while (i < left.Length && j < right.Length)
            {
                var cmp = TreeRefComparer.Default.Compare(left[i], right[j]);
                if (cmp == 0)
                {
                    i++;
                    j++;
                    k++;
                }
                else if (cmp < 0)
                {
                    i++;
                }
                else if (cmp > 0)
                {
                    j++;
                }
            }

            return (i, j, k);
        }

        public bool IsSubsetOf(TreeSet treeSet)
        {
            if (treeSet._nodes.IsEmpty) return _nodes.IsEmpty;
            if (_nodes.IsEmpty) return true;

            var (left, right, count) = GetSetDiscriminator(_nodes.Span, treeSet._nodes.Span);
            return count == _nodes.Length && left <= right;
        }

        public bool IsProperSubsetOf(TreeSet treeSet)
        {
            if (treeSet._nodes.IsEmpty) return false;
            if (_nodes.IsEmpty) return true;

            var (left, right, count) = GetSetDiscriminator(_nodes.Span, treeSet._nodes.Span);
            return count == _nodes.Length && left < right;
        }

        public bool IsSuperSetOf(TreeSet treeSet)
        {
            if (_nodes.IsEmpty) return treeSet._nodes.IsEmpty;
            if (treeSet._nodes.IsEmpty) return true;

            var (left, right, count) = GetSetDiscriminator(_nodes.Span, treeSet._nodes.Span);
            return count == treeSet._nodes.Length && left >= right;
        }

        public bool IsProperSuperSetOf(TreeSet treeSet)
        {
            if (_nodes.IsEmpty) return false;
            if (treeSet._nodes.IsEmpty) return true;

            var (left, right, count) = GetSetDiscriminator(_nodes.Span, treeSet._nodes.Span);
            return count == treeSet._nodes.Length && left > right;
        }

        public bool Overlaps(TreeSet treeSet)
        {
            if (_nodes.IsEmpty || treeSet._nodes.IsEmpty) return false;

            var (left, right, count) = GetSetDiscriminator(_nodes.Span, treeSet._nodes.Span);
            return left != 0 && right != 0;
        }

        #endregion

        #region SymmetricExceptWith

        private static ReadOnlyMemory<TreeRef> SymmetricExceptWith(ReadOnlySpan<TreeRef> first, ReadOnlySpan<TreeRef> second)
        {
            var newArray = new TreeRef[first.Length + second.Length];

            var i = 0;
            var aIndex = 0;
            var bIndex = 0;
            while (aIndex < first.Length || bIndex < second.Length)
            {
                if (aIndex >= first.Length)
                {
                    newArray[i++] = second[bIndex++];
                }
                else if (bIndex >= second.Length)
                {
                    newArray[i++] = first[aIndex++];
                }
                else
                {
                    var a = first[aIndex];
                    var b = second[bIndex];
                    var cmp = TreeRefComparer.Default.Compare(a, b);

                    if (cmp == 0)
                    {
                        ++bIndex;
                        ++aIndex;
                    }
                    else if (cmp < 0)
                    {
                        newArray[i++] = a;
                        ++aIndex;
                    }
                    else
                    {
                        newArray[i++] = b;
                        ++bIndex;
                    }
                }
            }

            return new ReadOnlyMemory<TreeRef>(newArray, 0, i);
        }

        public TreeSet SymmetricExceptWith(TreeSet treeSet)
        {
            if (treeSet._nodes.IsEmpty) return this;
            if (_nodes.IsEmpty) return treeSet;

            var set = new TreeSet(true, SymmetricExceptWith(_nodes.Span, treeSet._nodes.Span));
            return set;
        }

        #endregion

        #region IndexOf

        public int IndexOf(TreeRef treeRef)
        {
            if (_nodes.IsEmpty) return -1;

            var l = 0;
            var r = _nodes.Length - 1;
            var i = r / 2;

            var span = _nodes.Span;

            while (r >= l)
            {
                var cmp = TreeRefComparer.Default.Compare(span[i], treeRef);
                if (cmp == 0) return i;
                else if (cmp > 0) r = i - 1;
                else l = i + 1;

                i = l + (r - l) / 2;
            }

            return ~i;
        }

        #endregion

        #region IEnumerable

        public IEnumerator<TreeRef> GetEnumerator()
        {
            if (_nodes.IsEmpty)
                yield break;

            for (var i = 0; i < _nodes.Length; i++)
                yield return _nodes.Span[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
