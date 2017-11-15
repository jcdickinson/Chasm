#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay;
using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Chasm
{
    /// <summary>
    /// Represents a way to compare different <see cref="TreeMap"/> values.
    /// </summary>
    public abstract class TreeMapComparer : IEqualityComparer<TreeMap>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="TreeMapComparer"/> that compares all fields of a <see cref="TreeMap"/> value.
        /// </summary>
        public static TreeMapComparer Default { get; } = new DefaultComparer();

        #endregion

        #region Constructors

        private TreeMapComparer()
        { }

        #endregion

        #region IEqualityComparer

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        [Obsolete("Use GetHashCode(TreeNodeMap) to get the hash code for a TreeNodeMap.", true)]
        public override int GetHashCode() =>
            throw new InvalidOperationException();

        [Obsolete("Use Equals(TreeNodeMap, TreeNodeMap) to check two TreeNodeMap values for equality.")]
        public override bool Equals(object obj) =>
            throw new InvalidOperationException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        public abstract bool Equals(TreeMap x, TreeMap y);

        /// <inheritdoc/>
        public abstract int GetHashCode(TreeMap obj);

        #endregion

        #region Concrete

        private sealed class DefaultComparer : TreeMapComparer
        {
            #region Methods

            public override bool Equals(TreeMap x, TreeMap y)
            {
                if (x._nodes.Length != y._nodes.Length) return false;

                var sx = x._nodes.Span;
                var sy = y._nodes.Span;
                for (var i = 0; i < sx.Length; i++)
                {
                    if (!TreeMapNodeComparer.Default.Equals(sx[i], sy[i]))
                        return false;
                }

                return true;
            }

            public override int GetHashCode(TreeMap obj)
            {
                var hc = new HashCode();

                hc.Add(obj._nodes.Length);

                return hc.ToHashCode();
            }

            #endregion
        }

        #endregion
    }
}
