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
    /// Represents a way to compare different <see cref="TreeList"/> values.
    /// </summary>
    public abstract class TreeListComparer : IEqualityComparer<TreeList>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="TreeListComparer"/> that compares all fields of a <see cref="TreeList"/> value.
        /// </summary>
        public static TreeListComparer Default { get; } = new DefaultComparer();

        #endregion

        #region Constructors

        private TreeListComparer()
        { }

        #endregion

        #region IEqualityComparer

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        [Obsolete("Use GetHashCode(TreeList) to get the hash code for a TreeList.", true)]
        public override int GetHashCode() =>
            throw new InvalidOperationException();

        [Obsolete("Use Equals(TreeList, TreeList) to check two TreeList values for equality.")]
        public override bool Equals(object obj) =>
            throw new InvalidOperationException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        public abstract bool Equals(TreeList x, TreeList y);

        /// <inheritdoc/>
        public abstract int GetHashCode(TreeList obj);

        #endregion

        #region Concrete

        private sealed class DefaultComparer : TreeListComparer
        {
            #region Methods

            public override bool Equals(TreeList x, TreeList y)
                => x._nodes.MemoryEquals(y._nodes);

            public override int GetHashCode(TreeList obj)
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
