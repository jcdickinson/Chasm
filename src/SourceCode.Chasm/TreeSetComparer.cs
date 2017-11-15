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
    /// Represents a way to compare different <see cref="TreeSet"/> values.
    /// </summary>
    public abstract class TreeSetComparer : IEqualityComparer<TreeSet>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="TreeSetComparer"/> that compares all fields of a <see cref="TreeSet"/> value.
        /// </summary>
        public static TreeSetComparer Default { get; } = new DefaultComparer();

        #endregion

        #region Constructors

        private TreeSetComparer()
        { }

        #endregion

        #region IEqualityComparer

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        [Obsolete("Use GetHashCode(TreeSet) to get the hash code for a TreeSet.", true)]
        public override int GetHashCode() =>
            throw new InvalidOperationException();

        [Obsolete("Use Equals(TreeSet, TreeSet) to check two TreeSet values for equality.")]
        public override bool Equals(object obj) =>
            throw new InvalidOperationException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        public abstract bool Equals(TreeSet x, TreeSet y);

        /// <inheritdoc/>
        public abstract int GetHashCode(TreeSet obj);

        #endregion

        #region Concrete

        private sealed class DefaultComparer : TreeSetComparer
        {
            #region Methods

            public override bool Equals(TreeSet x, TreeSet y)
                => x._nodes.MemoryEquals(y._nodes);

            public override int GetHashCode(TreeSet obj)
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
