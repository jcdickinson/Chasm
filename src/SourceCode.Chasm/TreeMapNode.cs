#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;

namespace SourceCode.Chasm
{
    [DebuggerDisplay("{ToString(),nq,ac}")]
    public readonly struct TreeMapNode : IEquatable<TreeMapNode>, IComparable<TreeMapNode>
    {
        #region Constants

        /// <summary>
        /// A singleton representing an empty <see cref="TreeMapNode"/> value.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static TreeMapNode Empty { get; }

        #endregion

        #region Properties

        public string Name { get; }

        public TreeRef TreeRef { get; }

        public TreeMapId TreeMapId
        {
            get
            {
                if (TreeRef.Kind != NodeKind.Map)
                    throw new InvalidOperationException($"The node {Name} must be a {nameof(NodeKind.Map)}.");
                return new TreeMapId(TreeRef.Sha1);
            }
        }

        public TreeListId TreeListId
        {
            get
            {
                if (TreeRef.Kind != NodeKind.Map)
                    throw new InvalidOperationException($"The node {Name} must be a {nameof(NodeKind.List)}.");
                return new TreeListId(TreeRef.Sha1);
            }
        }

        public TreeSetId TreeSetId
        {
            get
            {
                if (TreeRef.Kind != NodeKind.Set)
                    throw new InvalidOperationException($"The node {Name} must be a {nameof(NodeKind.Set)}.");
                return new TreeSetId(TreeRef.Sha1);
            }
        }

        public BlobId BlobId
        {
            get
            {
                if (TreeRef.Kind != NodeKind.Blob)
                    throw new InvalidOperationException($"The node {Name} must be a {nameof(NodeKind.Blob)}.");
                return new BlobId(TreeRef.Sha1);
            }
        }

        #endregion

        #region De/Constructors

        public TreeMapNode(string name, TreeRef treeRef)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            TreeRef = treeRef;
        }

        public TreeMapNode(string name, NodeKind kind, Sha1 sha1)
            : this(name, new TreeRef(kind, sha1))
        { }

        public TreeMapNode(string name, BlobId blobId)
            : this(name, NodeKind.Blob, blobId.Sha1)
        { }

        public TreeMapNode(string name, TreeMapId treeMapId)
            : this(name, NodeKind.Map, treeMapId.Sha1)
        { }

        public void Deconstruct(out string name, out TreeRef treeRef)
        {
            name = Name;
            treeRef = TreeRef;
        }

        #endregion

        #region IEquatable

        public bool Equals(TreeMapNode other) => TreeMapNodeComparer.Default.Equals(this, other);

        public override bool Equals(object obj)
            => obj is TreeMapNode node
            && TreeMapNodeComparer.Default.Equals(this, node);

        public override int GetHashCode() => TreeMapNodeComparer.Default.GetHashCode(this);

        #endregion

        #region IComparable

        public int CompareTo(TreeMapNode other) => TreeMapNodeComparer.Default.Compare(this, other);

        #endregion

        #region Operators

        public static bool operator ==(TreeMapNode x, TreeMapNode y) => TreeMapNodeComparer.Default.Equals(x, y);

        public static bool operator !=(TreeMapNode x, TreeMapNode y) => !(x == y);

        public static bool operator >=(TreeMapNode x, TreeMapNode y) => TreeMapNodeComparer.Default.Compare(x, y) >= 0;

        public static bool operator >(TreeMapNode x, TreeMapNode y) => TreeMapNodeComparer.Default.Compare(x, y) > 0;

        public static bool operator <=(TreeMapNode x, TreeMapNode y) => TreeMapNodeComparer.Default.Compare(x, y) <= 0;

        public static bool operator <(TreeMapNode x, TreeMapNode y) => TreeMapNodeComparer.Default.Compare(x, y) < 0;

        public override string ToString() => $"{Name}: {TreeRef.Kind} ({TreeRef.Sha1:D})";

        #endregion
    }
}
