#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;

namespace SourceCode.Chasm
{
    [DebuggerDisplay("{Sha1,nq,ac}")]
    public readonly struct TreeSetId : IEquatable<TreeSetId>, IComparable<TreeSetId>
    {
        #region Properties

        public Sha1 Sha1 { get; }

        #endregion

        #region Constructors

        public TreeSetId(Sha1 sha1)
        {
            Sha1 = sha1;
        }

        #endregion

        #region IEquatable

        public bool Equals(TreeSetId other) => TreeSetIdComparer.Default.Equals(this, other);

        public override bool Equals(object obj)
            => obj is TreeSetId blobId
            && TreeSetIdComparer.Default.Equals(this, blobId);

        public override int GetHashCode() => TreeSetIdComparer.Default.GetHashCode(this);

        #endregion

        #region IComparable

        public int CompareTo(TreeSetId other) => TreeSetIdComparer.Default.Compare(this, other);

        #endregion

        #region Operators

        public static bool operator ==(TreeSetId x, TreeSetId y) => TreeSetIdComparer.Default.Equals(x, y);

        public static bool operator !=(TreeSetId x, TreeSetId y) => !(x == y);

        public static bool operator >=(TreeSetId x, TreeSetId y) => TreeSetIdComparer.Default.Compare(x, y) >= 0;

        public static bool operator >(TreeSetId x, TreeSetId y) => TreeSetIdComparer.Default.Compare(x, y) > 0;

        public static bool operator <=(TreeSetId x, TreeSetId y) => TreeSetIdComparer.Default.Compare(x, y) <= 0;

        public static bool operator <(TreeSetId x, TreeSetId y) => TreeSetIdComparer.Default.Compare(x, y) < 0;

        public override string ToString() => Sha1.ToString("N"); // Used by callsites as a proxy for .Sha1.ToString()

        #endregion
    }
}
