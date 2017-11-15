#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;

namespace SourceCode.Chasm
{
    [DebuggerDisplay("{Kind,nq,nc} {Sha1,nq,nc}")]
    public readonly struct TreeRef : IEquatable<TreeRef>
    {
        #region Properties

        public Sha1 Sha1 { get; }

        public NodeKind Kind { get; }

        public TreeMapId TreeMapId
        {
            get
            {
                if (Kind != NodeKind.Map)
                    throw new InvalidOperationException($"The node must be a {nameof(NodeKind.Map)}.");
                return new TreeMapId(Sha1);
            }
        }

        public BlobId BlobId
        {
            get
            {
                if (Kind != NodeKind.Blob)
                    throw new InvalidOperationException($"The node must be a {nameof(NodeKind.Blob)}.");
                return new BlobId(Sha1);
            }
        }

        #endregion

        #region Constructors

        public TreeRef(NodeKind kind, Sha1 sha1)
        {
            if (!Enum.IsDefined(typeof(NodeKind), kind)) throw new ArgumentOutOfRangeException(nameof(kind));
            Sha1 = sha1;
            Kind = kind;
        }

        public void Deconstruct(out NodeKind kind, out Sha1 sha1)
        {
            kind = Kind;
            sha1 = Sha1;
        }

        #endregion

        #region IEquatable

        public static bool operator ==(TreeRef ref1, TreeRef ref2) =>
            ref1.Equals(ref2);

        public static bool operator !=(TreeRef ref1, TreeRef ref2)
            => !(ref1 == ref2);

        public override bool Equals(object obj) =>
                            obj is TreeRef o && Equals(o);

        public bool Equals(TreeRef other) =>
            TreeRefComparer.Default.Equals(this, other);

        public override int GetHashCode() =>
            TreeRefComparer.Default.GetHashCode(this);

        #endregion
    }
}
