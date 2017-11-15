#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay;
using System;
using System.Collections.Generic;

namespace SourceCode.Chasm
{
    /// <summary>
    /// Represents a way to compare different <see cref="TreeMapNode"/> values.
    /// </summary>
    public abstract class TreeMapNodeComparer : IEqualityComparer<TreeMapNode>, IComparer<TreeMapNode>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="TreeMapNodeComparer"/> that compares all fields of a <see cref="TreeMapNode"/> value.
        /// </summary>
        public static TreeMapNodeComparer Default { get; } = new DefaultComparer();

        /// <summary>
        /// Gets a <see cref="TreeMapNodeComparer"/> that only compares the <see cref="TreeMapNode.Name"/> field of a <see cref="TreeMapNode"/> value.
        /// </summary>
        public static TreeMapNodeComparer NameOnly { get; } = new NameOnlyComparer();

        #endregion

        #region Constructors

        private TreeMapNodeComparer()
        { }

        #endregion

        #region IComparer

        /// <inheritdoc/>
        public abstract int Compare(TreeMapNode x, TreeMapNode y);

        #endregion

        #region IEqualityComparer

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        [Obsolete("Use GetHashCode(TreeMapNode) to get the hash code for a TreeMapNode.", true)]
        public override int GetHashCode() =>
            throw new InvalidOperationException();

        [Obsolete("Use Equals(TreeMapNode, TreeMapNode) to check two TreeMapNode values for equality.")]
        public override bool Equals(object obj) =>
            throw new InvalidOperationException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        public abstract bool Equals(TreeMapNode x, TreeMapNode y);

        /// <inheritdoc/>
        public abstract int GetHashCode(TreeMapNode obj);

        #endregion

        #region Concrete

        private sealed class DefaultComparer : TreeMapNodeComparer
        {
            #region Methods

            public override int Compare(TreeMapNode x, TreeMapNode y)
            {
                // Nodes are always sorted by Name first (see TreeMap)
                var cmp = string.CompareOrdinal(x.Name, y.Name);
                if (cmp != 0) return cmp;

                // Then by Sha1 (in order to detect duplicate)
                cmp = Sha1Comparer.Default.Compare(x.TreeRef.Sha1, y.TreeRef.Sha1);
                if (cmp != 0) return cmp;

                // And lastly by Kind
                cmp = ((int)x.TreeRef.Kind).CompareTo((int)y.TreeRef.Kind);
                return cmp;
            }

            public override bool Equals(TreeMapNode x, TreeMapNode y)
            {
                if (!TreeRefComparer.Default.Equals(x.TreeRef, y.TreeRef)) return false;
                if (!StringComparer.Ordinal.Equals(x.Name, y.Name)) return false;

                return true;
            }

            public override int GetHashCode(TreeMapNode obj) => HashCode.Combine(
                StringComparer.Ordinal.GetHashCode(obj.Name ?? string.Empty),
                TreeRefComparer.Default.GetHashCode(obj.TreeRef)
            );

            #endregion
        }

        private sealed class NameOnlyComparer : TreeMapNodeComparer
        {
            #region Methods

            public override int Compare(TreeMapNode x, TreeMapNode y) => string.CompareOrdinal(x.Name, y.Name);

            public override bool Equals(TreeMapNode x, TreeMapNode y) => StringComparer.Ordinal.Equals(x.Name, y.Name);

            public override int GetHashCode(TreeMapNode obj) => HashCode.Combine(
                StringComparer.Ordinal.GetHashCode(obj.Name ?? string.Empty)
            );

            #endregion
        }

        #endregion
    }
}
