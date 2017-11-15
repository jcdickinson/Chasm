#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceCode.Chasm
{
#pragma warning disable CA1710 // Identifiers should have correct suffix

    public readonly struct TreeList : IReadOnlyList<TreeRef>, IEquatable<TreeList>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        #region Constants

        /// <summary>
        /// A singleton representing an empty <see cref="TreeList"/> value.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static TreeList Empty { get; }

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

        private TreeList(bool sentinel, ReadOnlyMemory<TreeRef> nodes)
        {
            _nodes = nodes;
        }

        public TreeList(params TreeRef[] nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null || nodes.Length == 0)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            var copy = new TreeRef[nodes.Length];
            nodes.CopyTo(copy);
            _nodes = copy;
        }

        public TreeList(IEnumerable<TreeRef> nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            _nodes = nodes.ToArray();
        }

        public TreeList(ICollection<TreeRef> nodes)
        {
            // We choose to coerce empty & null, so de/serialization round-trips with fidelity
            if (nodes == null || nodes.Count == 0)
            {
                _nodes = default; // ie, same as default struct ctor
                return;
            }

            // Copy
            var array = new TreeRef[nodes.Count];
            nodes.CopyTo(array, 0);
            _nodes = array;
        }

        #endregion

        #region IEquatable

        public static bool operator ==(TreeList list1, TreeList list2)
            => list1.Equals(list2);

        public static bool operator !=(TreeList list1, TreeList list2)
            => !(list1 == list2);

        public override bool Equals(object obj)
                            => obj is TreeList o
            && TreeListComparer.Default.Equals(this, o);

        public bool Equals(TreeList other)
            => TreeListComparer.Default.Equals(this, other);

        public override int GetHashCode()
            => TreeListComparer.Default.GetHashCode(this);

        #endregion

        #region Collection

        public TreeList Add(TreeRef treeRef) => Insert(_nodes.Length, treeRef);

        public TreeList Insert(int index, TreeRef treeRef)
        {
            if (index > _nodes.Length) throw new IndexOutOfRangeException();
            if (_nodes.IsEmpty) return new TreeList(true, new TreeRef[] { treeRef });

            var arr = new TreeRef[_nodes.Length + 1];
            var span = arr.AsSpan();

            if (index > 0)
            {
                _nodes.Span.Slice(0, index).CopyTo(span);
            }

            span[index] = treeRef;

            if (index < _nodes.Length)
            {
                _nodes.Span
                    .Slice(index)
                    .CopyTo(span.Slice(index + 1));
            }

            return new TreeList(true, arr);
        }

        public TreeList RemoveAt(int index)
        {
            if (index >= _nodes.Length) throw new IndexOutOfRangeException();

            var arr = new TreeRef[_nodes.Length - 1];
            var span = arr.AsSpan();

            if (index > 0)
            {
                _nodes.Span.Slice(0, index).CopyTo(span);
            }

            if (index < _nodes.Length)
            {
                _nodes.Span
                    .Slice(index + 1)
                    .CopyTo(span.Slice(index));
            }

            return new TreeList(true, arr);
        }

        public TreeList Remove(TreeRef treeRef)
        {
            var i = IndexOf(treeRef);
            if (i >= 0) return RemoveAt(i);
            return this;
        }

        public TreeList RemoveWhere(Func<TreeRef, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (_nodes.Length == 0) return this;

            var copy = new TreeRef[_nodes.Length - 1];
            var span = _nodes.Span;
            var j = 0;
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (!predicate(span[i]))
                    copy[j++] = span[i];
            }

            if (j == _nodes.Length)
                return this;

            return new TreeList(true, new ReadOnlyMemory<TreeRef>(copy, 0, j));
        }

        public int IndexOf(TreeRef treeRef)
        {
            var span = _nodes.Span;
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i] == treeRef) return i;
            }
            return -1;
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
