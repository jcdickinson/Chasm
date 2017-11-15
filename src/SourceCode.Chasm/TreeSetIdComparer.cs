#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Chasm
{
    /// <summary>
    /// Represents a way to compare different <see cref="TreeSetId"/> values.
    /// </summary>
    public abstract class TreeSetIdComparer : IEqualityComparer<TreeSetId>, IComparer<TreeSetId>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="TreeSetIdComparer"/> that compares all fields of a <see cref="TreeSetId"/> value.
        /// </summary>
        public static TreeSetIdComparer Default { get; } = new DefaultComparer();

        #endregion

        #region Constructors

        private TreeSetIdComparer()
        { }

        #endregion

        #region IComparer

        /// <inheritdoc/>
        public abstract int Compare(TreeSetId x, TreeSetId y);

        #endregion

        #region IEqualityComparer

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        [Obsolete("Use GetHashCode(TreeId) to get the hash code for a TreeId.", true)]
        public override int GetHashCode() =>
            throw new InvalidOperationException();

        [Obsolete("Use Equals(TreeId, TreeId) to check two TreeId values for equality.")]
        public override bool Equals(object obj) =>
            throw new InvalidOperationException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        public abstract bool Equals(TreeSetId x, TreeSetId y);

        /// <inheritdoc/>
        public abstract int GetHashCode(TreeSetId obj);

        #endregion

        #region Concrete

        private sealed class DefaultComparer : TreeSetIdComparer
        {
            #region Methods

            public override int Compare(TreeSetId x, TreeSetId y) => Sha1Comparer.Default.Compare(x.Sha1, y.Sha1);

            public override bool Equals(TreeSetId x, TreeSetId y) => Sha1Comparer.Default.Equals(x.Sha1, y.Sha1);

            public override int GetHashCode(TreeSetId obj) => Sha1Comparer.Default.GetHashCode(obj.Sha1);

            #endregion
        }

        #endregion
    }
}
