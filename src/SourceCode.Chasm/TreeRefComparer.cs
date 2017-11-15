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
    /// Represents a way to compare different <see cref="TreeRef"/> values.
    /// </summary>
    public abstract class TreeRefComparer : IEqualityComparer<TreeRef>, IComparer<TreeRef>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="TreeRefComparer"/> that compares all fields of a <see cref="TreeRef"/> value.
        /// </summary>
        public static TreeRefComparer Default { get; } = new DefaultComparer();

        #endregion

        #region Constructors

        private TreeRefComparer()
        { }

        #endregion

        #region IComparer

        /// <inheritdoc/>
        public abstract int Compare(TreeRef x, TreeRef y);

        #endregion

        #region IEqualityComparer

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        [Obsolete("Use GetHashCode(TreeRef) to get the hash code for a TreeRef.", true)]
        public override int GetHashCode() =>
            throw new InvalidOperationException();

        [Obsolete("Use Equals(TreeRef, TreeRef) to check two TreeRef values for equality.")]
        public override bool Equals(object obj) =>
            throw new InvalidOperationException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        public abstract bool Equals(TreeRef x, TreeRef y);

        /// <inheritdoc/>
        public abstract int GetHashCode(TreeRef obj);

        #endregion

        #region Concrete

        private sealed class DefaultComparer : TreeRefComparer
        {
            #region Methods

            public override int Compare(TreeRef x, TreeRef y)
            {
                // Then by Sha1 (in order to detect duplicate)
                var cmp = Sha1Comparer.Default.Compare(x.Sha1, y.Sha1);
                if (cmp != 0) return cmp;

                // And lastly by Kind
                cmp = ((int)x.Kind).CompareTo((int)y.Kind);
                return cmp;
            }

            public override bool Equals(TreeRef x, TreeRef y)
            {
                if (x.Kind != y.Kind) return false;
                if (x.Sha1 != y.Sha1) return false;

                return true;
            }

            public override int GetHashCode(TreeRef obj) => HashCode.Combine(
                obj.Kind,
                obj.Sha1
            );

            #endregion
        }

        #endregion
    }
}
