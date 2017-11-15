#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Chasm.Tests
{
    public static class TreeNodeTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNode_is_empty))]
        public static void TreeNode_is_empty()
        {
            var noData = new TreeMapNode();
            var nullData = new TreeMapNode("a", NodeKind.Blob, Sha1.Zero);

            // Name
            Assert.Null(TreeMapNode.Empty.Name);
            Assert.Null(noData.Name);
            Assert.Equal("a", nullData.Name);
            Assert.Throws<ArgumentNullException>(() => new TreeMapNode(null, NodeKind.Blob, Sha1.Zero));
            Assert.Throws<ArgumentNullException>(() => new TreeMapNode(null, new BlobId()));
            Assert.Throws<ArgumentNullException>(() => new TreeMapNode(null, new TreeMapId()));

            // NodeKind
            Assert.Equal(NodeKind.Blob, default);
            Assert.Equal(default, TreeMapNode.Empty.TreeRef.Kind);
            Assert.Equal(default, noData.TreeRef.Kind);
            Assert.Equal(default, nullData.TreeRef.Kind);
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeMapNode("a", (NodeKind)2, Sha1.Zero));

            // Sha1
            Assert.Equal(default, TreeMapNode.Empty.TreeRef.Sha1);
            Assert.Equal(default, noData.TreeRef.Sha1);
            Assert.Equal(default, nullData.TreeRef.Sha1);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNode_equality))]
        public static void TreeNode_equality()
        {
            var tree1 = new TreeMapNode("a", NodeKind.Map, Sha1.Hash("abc"));
            var tree2 = new TreeMapNode("a", NodeKind.Map, Sha1.Hash("abc"));

            Assert.True(tree1 == tree2);
            Assert.False(tree1 != tree2);
            Assert.True(tree1.Equals((object)tree2));

            // Equal
            var actual = new TreeMapNode(tree1.Name, tree1.TreeRef.Kind, tree1.TreeRef.Sha1);
            Assert.Equal(tree1, actual);
            Assert.Equal(tree1.GetHashCode(), actual.GetHashCode());

            // Name
            actual = new TreeMapNode("b", tree1.TreeRef.Kind, tree1.TreeRef.Sha1);
            Assert.NotEqual(tree1, actual);
            Assert.NotEqual(tree1.GetHashCode(), actual.GetHashCode());

            actual = new TreeMapNode("ab", tree1.TreeRef.Kind, tree1.TreeRef.Sha1);
            Assert.NotEqual(tree1, actual);
            Assert.NotEqual(tree1.GetHashCode(), actual.GetHashCode());

            actual = new TreeMapNode(tree1.Name.ToUpperInvariant(), tree1.TreeRef.Kind, tree1.TreeRef.Sha1);
            Assert.NotEqual(tree1, actual);
            Assert.NotEqual(tree1.GetHashCode(), actual.GetHashCode());

            // Kind
            actual = new TreeMapNode(tree1.Name, default, tree1.TreeRef.Sha1);
            Assert.NotEqual(tree1, actual);
            Assert.NotEqual(tree1.GetHashCode(), actual.GetHashCode());

            actual = new TreeMapNode(tree1.Name, NodeKind.Blob, tree1.TreeRef.Sha1);
            Assert.NotEqual(tree1, actual);
            Assert.NotEqual(tree1.GetHashCode(), actual.GetHashCode());

            // Sha1
            actual = new TreeMapNode(tree1.Name, tree1.TreeRef.Kind, Sha1.Hash("def"));
            Assert.NotEqual(tree1, actual);
            Assert.NotEqual(tree1.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNode_Deconstruct))]
        public static void TreeNode_Deconstruct()
        {
            var expected = new TreeMapNode("a", NodeKind.Blob, Sha1.Hash("abc"));

            var (name, treeRef) = expected;
            var actual = new TreeMapNode(name, treeRef);

            Assert.Equal(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNode_Compare))]
        public static void TreeNode_Compare()
        {
            var comparer = TreeMapNodeComparer.Default;

            var tree1 = new TreeMapNode("a", NodeKind.Blob, Sha1.Hash("abc"));
            var tree2 = new TreeMapNode("a", NodeKind.Blob, Sha1.Hash("abc"));
            var tree3 = new TreeMapNode("d", NodeKind.Blob, Sha1.Hash("def"));
            var list = new[] { tree1, tree2, tree3 };

            Assert.True(TreeMapNode.Empty < tree1);
            Assert.True(tree1 > TreeMapNode.Empty);

            Assert.True(tree1.CompareTo(tree2) == 0);
            Assert.True(tree1.CompareTo(tree3) != 0);

            Array.Sort(list, comparer.Compare);

            Assert.True(list[0] <= list[1]);
            Assert.True(list[2] >= list[1]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNode_Constructor_String_TreeId))]
        public static void TreeNode_Constructor_String_TreeId()
        {
            // Arrange
            var expectedTreeId = new TreeMapId();
            var expectedName = Guid.NewGuid().ToString();

            // Action
            var actual = new TreeMapNode(expectedName, expectedTreeId);

            // Assert
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedTreeId.Sha1, actual.TreeRef.Sha1);
        }

        #endregion
    }
}
