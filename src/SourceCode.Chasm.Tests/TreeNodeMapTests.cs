#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SourceCode.Chasm.Tests
{
    public static class TreeNodeMapTests
    {
        #region Constants

        private static readonly TreeMapNode Node0 = new TreeMapNode(nameof(Node0), NodeKind.Map, Sha1.Hash(nameof(Node0)));
        private static readonly TreeMapNode Node0Blob = new TreeMapNode(nameof(Node0), NodeKind.Blob, Sha1.Hash(nameof(Node0Blob)));
        private static readonly TreeMapNode Node1 = new TreeMapNode(nameof(Node1), NodeKind.Blob, Sha1.Hash(nameof(Node1)));
        private static readonly TreeMapNode Node2 = new TreeMapNode(nameof(Node2), NodeKind.Map, Sha1.Hash(nameof(Node2)));
        private static readonly TreeMapNode Node3 = new TreeMapNode(nameof(Node3), NodeKind.Blob, Sha1.Hash(nameof(Node3)));

        #endregion

        #region Methods

        private static void AssertEmpty(TreeMap treeMap)
        {
            Assert.Empty(treeMap);
            Assert.Equal(TreeMap.Empty, treeMap); // By design
            Assert.Equal(TreeMap.Empty.GetHashCode(), treeMap.GetHashCode());
            Assert.Empty(treeMap.Keys);

            Assert.Throws<IndexOutOfRangeException>(() => treeMap[0]);
            Assert.Throws<KeyNotFoundException>(() => treeMap["x"]);
            Assert.False(treeMap.TryGetValue("x", out _));
            Assert.False(treeMap.TryGetValue("x", NodeKind.Blob, out _));

            Assert.False(treeMap.Equals(new object()));
            Assert.Contains("Count: 0", treeMap.ToString());
            Assert.Equal(-1, treeMap.IndexOf(Guid.NewGuid().ToString()));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Empty))]
        public static void TreeNodeMap_Empty()
        {
            var noData = new TreeMap();
            AssertEmpty(noData);

            var nullData = new TreeMap(null);
            AssertEmpty(nullData);

            var collData = new TreeMap((IList<TreeMapNode>)null);
            AssertEmpty(collData);

            var emptyData = new TreeMap(Array.Empty<TreeMapNode>());
            AssertEmpty(emptyData);

            Assert.Empty(TreeMap.Empty);
            Assert.Equal(default, TreeMap.Empty);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Sorting))]
        public static void TreeNodeMap_Sorting()
        {
            var nodes = new[] { Node0, Node1 };
            var tree0 = new TreeMap(nodes.OrderBy(n => n.TreeRef.Sha1).ToArray());
            var tree1 = new TreeMap(nodes.OrderByDescending(n => n.TreeRef.Sha1).ToList()); // ICollection<T>

            Assert.Equal(tree0[0], tree1[0]);
            Assert.Equal(tree0[1], tree1[1]);

            Assert.True(tree1[Node0.Name] == Node0);
            Assert.True(tree1[Node1.Name] == Node1);
            Assert.False(tree1.ContainsKey("x"));
            Assert.True(tree1.ContainsKey(Node0.Name));
            Assert.True(tree1.ContainsKey(Node1.Name));
            Assert.False(tree1.TryGetValue("x", out _));
            Assert.True(tree1.TryGetValue(Node0.Name, out var v20) && v20 == Node0);
            Assert.True(tree1.TryGetValue(Node1.Name, out var v21) && v21 == Node1);
            Assert.False(tree1.TryGetValue(Node0.Name, NodeKind.Blob, out _));
            Assert.True(tree1.TryGetValue(Node0.Name, Node0.TreeRef.Kind, out _));

            nodes = new[] { Node0, Node1, Node2 };
            tree0 = new TreeMap(nodes.OrderBy(n => n.TreeRef.Sha1).ToArray());
            tree1 = new TreeMap(nodes.OrderByDescending(n => n.TreeRef.Sha1).ToList()); // ICollection<T>

            Assert.True(tree1[Node0.Name] == Node0);
            Assert.True(tree1[Node1.Name] == Node1);
            Assert.True(tree1[Node2.Name] == Node2);
            Assert.False(tree1.ContainsKey("x"));
            Assert.True(tree1.ContainsKey(Node0.Name));
            Assert.True(tree1.ContainsKey(Node1.Name));
            Assert.True(tree1.ContainsKey(Node2.Name));
            Assert.False(tree1.TryGetValue("x", out _));
            Assert.True(tree1.TryGetValue(Node0.Name, out var v30) && v30 == Node0);
            Assert.True(tree1.TryGetValue(Node1.Name, out var v31) && v31 == Node1);
            Assert.True(tree1.TryGetValue(Node2.Name, out var v32) && v32 == Node2);

            Assert.Equal(tree0[0], tree1[0]);
            Assert.Equal(tree0[1], tree1[1]);
            Assert.Equal(tree0[2], tree1[2]);

            nodes = new[] { Node0, Node1, Node2, Node3 };
            tree0 = new TreeMap(nodes.OrderBy(n => n.TreeRef.Sha1).ToArray());
            tree1 = new TreeMap(nodes.OrderByDescending(n => n.TreeRef.Sha1).ToList()); // ICollection<T>

            Assert.True(tree1[Node0.Name] == Node0);
            Assert.True(tree1[Node1.Name] == Node1);
            Assert.True(tree1[Node2.Name] == Node2);
            Assert.True(tree1[Node3.Name] == Node3);
            Assert.False(tree1.ContainsKey("x"));
            Assert.True(tree1.ContainsKey(Node0.Name));
            Assert.True(tree1.ContainsKey(Node1.Name));
            Assert.True(tree1.ContainsKey(Node2.Name));
            Assert.True(tree1.ContainsKey(Node3.Name));
            Assert.False(tree1.TryGetValue("x", out _));
            Assert.True(tree1.TryGetValue(Node0.Name, out var v40) && v40 == Node0);
            Assert.True(tree1.TryGetValue(Node1.Name, out var v41) && v41 == Node1);
            Assert.True(tree1.TryGetValue(Node2.Name, out var v42) && v42 == Node2);
            Assert.True(tree1.TryGetValue(Node3.Name, out var v43) && v43 == Node3);

            Assert.Equal(tree0[0], tree1[0]);
            Assert.Equal(tree0[1], tree1[1]);
            Assert.Equal(tree0[2], tree1[2]);
            Assert.Equal(tree0[3], tree1[3]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Full_2))]
        public static void TreeNodeMap_Duplicate_Full_2()
        {
            var nodes = new[] { Node0, Node0 };

            var tree = new TreeMap(nodes);
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0));

            tree = new TreeMap(nodes.ToList()); // ICollection<T>
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Full_3))]
        public static void TreeNodeMap_Duplicate_Full_3()
        {
            var nodes = new[] { Node0, Node1, Node0 }; // Shuffled

            var tree = new TreeMap(nodes);
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0), n => Assert.Equal(n, Node1));

            tree = new TreeMap(nodes.ToList()); // ICollection<T>
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0), n => Assert.Equal(n, Node1));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Full_2_Exception))]
        public static void TreeNodeMap_Duplicate_Full_2_Exception()
        {
            // Arrange
            var nodes = new[] { Node0, Node0Blob }; // Shuffled

            // Action
            var ex = Assert.Throws<ArgumentException>(() => new TreeMap(nodes));

            // Assert
            Assert.Contains(Node0.Name, ex.Message);
            Assert.Contains(Node0.TreeRef.Sha1.ToString(), ex.Message);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Full_3_Exception))]
        public static void TreeNodeMap_Duplicate_Full_3_Exception()
        {
            // Arrange
            var nodes = new[] { Node0, Node0Blob, Node1 }; // Shuffled

            // Action
            var ex = Assert.Throws<ArgumentException>(() => new TreeMap(nodes));

            // Assert
            Assert.Contains(Node0.Name, ex.Message);
            Assert.Contains(Node0.TreeRef.Sha1.ToString(), ex.Message);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Full_4))]
        public static void TreeNodeMap_Duplicate_Full_4()
        {
            var nodes = new[] { Node0, Node2, Node1, Node0 }; // Shuffled

            var tree = new TreeMap(nodes);
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0), n => Assert.Equal(n, Node1), n => Assert.Equal(n, Node2));

            tree = new TreeMap(nodes.ToList()); // ICollection<T>
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0), n => Assert.Equal(n, Node1), n => Assert.Equal(n, Node2));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Full_N))]
        public static void TreeNodeMap_Duplicate_Full_N()
        {
            var nodes = new[] { Node3, Node1, Node2, Node0, Node3, Node0, Node1, Node0, Node1, Node2, Node0, Node3 }; // Shuffled

            var tree = new TreeMap(nodes);
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0), n => Assert.Equal(n, Node1), n => Assert.Equal(n, Node2), n => Assert.Equal(n, Node3));

            tree = new TreeMap(nodes.ToList()); // ICollection<T>
            Assert.Collection<TreeMapNode>(tree, n => Assert.Equal(n, Node0), n => Assert.Equal(n, Node1), n => Assert.Equal(n, Node2), n => Assert.Equal(n, Node3));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Name))]
        public static void TreeNodeMap_Duplicate_Name()
        {
            var nodes = new[] { new TreeMapNode(Node0.Name, NodeKind.Map, Node1.TreeRef.Sha1), Node0 }; // Reversed

            Assert.Throws<ArgumentException>(() => new TreeMap(nodes));
            Assert.Throws<ArgumentException>(() => new TreeMap(nodes.ToList())); // ICollection<T>
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Duplicate_Sha1))]
        public static void TreeNodeMap_Duplicate_Sha1()
        {
            var nodes = new[] { new TreeMapNode(Node1.Name, NodeKind.Map, Node0.TreeRef.Sha1), Node0 }; // Reversed

            var tree0 = new TreeMap(nodes);
            Assert.Collection<TreeMapNode>(tree0, n => Assert.Equal(n, Node0), n => Assert.Equal(n, nodes[0]));

            tree0 = new TreeMap(nodes.ToList()); // ICollection<T>
            Assert.Collection<TreeMapNode>(tree0, n => Assert.Equal(n, Node0), n => Assert.Equal(n, nodes[0]));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Equality))]
        public static void TreeNodeMap_Equality()
        {
            var expected = new TreeMap(new[] { new TreeMapNode("c1", NodeKind.Blob, Sha1.Hash("c1")), new TreeMapNode("c2", NodeKind.Map, Sha1.Hash("c2")) });
            var node3 = new TreeMapNode("c3", NodeKind.Map, Sha1.Hash("c3"));

            // Equal
            var actual = new TreeMap().Merge(expected);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.True(actual.Equals((object)expected));
            Assert.True(expected == actual);
            Assert.False(expected != actual);

            // Less Nodes
            actual = new TreeMap().Merge(expected[0]);
            Assert.NotEqual(expected, actual);
            Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
            Assert.False(actual.Equals((object)expected));
            Assert.False(expected == actual);
            Assert.True(expected != actual);

            // More Nodes
            actual = new TreeMap().Merge(expected).Merge(node3);
            Assert.NotEqual(expected, actual);
            Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
            Assert.False(actual.Equals((object)expected));
            Assert.False(expected == actual);
            Assert.True(expected != actual);

            // Different Nodes
            actual = new TreeMap().Merge(expected[0]).Merge(node3);
            Assert.NotEqual(expected, actual); // hashcode is the same (node count)
            Assert.False(actual.Equals((object)expected));
            Assert.False(expected == actual);
            Assert.True(expected != actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_IndexOf))]
        public static void TreeNodeMap_IndexOf()
        {
            // Arrange
            var actual = new TreeMap(new[] { Node0, Node1 });

            // Action/Assert
            Assert.Equal(-1, actual.IndexOf(null));
            Assert.True(actual.IndexOf(Guid.NewGuid().ToString()) < 0);
            Assert.Equal(0, actual.IndexOf(Node0.Name));
            Assert.Equal(1, actual.IndexOf(Node1.Name));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Merge_Empty))]
        public static void TreeNodeMap_Merge_Empty()
        {
            var emptyTreeNodeMap = new TreeMap();
            var node = new TreeMapNode("b", NodeKind.Blob, Sha1.Hash("Test1"));
            var list = new TreeMap(node);

            // TreeNodeMap
            var merged = list.Merge(emptyTreeNodeMap);
            Assert.Equal(list, merged);

            merged = emptyTreeNodeMap.Merge(list);
            Assert.Equal(list, merged);

            // ICollection
            merged = list.Merge(Array.Empty<TreeMapNode>());
            Assert.Equal(list, merged);

            merged = emptyTreeNodeMap.Merge(list.Values.ToArray());
            Assert.Equal(list, merged);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Merge_Null))]
        public static void TreeNodeMap_Merge_Null()
        {
            // Arrange
            var list = new TreeMap(Node0);

            // Action
            var merged = list.Merge(null);

            // Assert
            Assert.Equal(list, merged);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Merge_Single))]
        public static void TreeNodeMap_Merge_Single()
        {
            var list = new TreeMap();

            list = list.Merge(new TreeMapNode("b", NodeKind.Blob, Sha1.Hash("Test1")));
            list = list.Merge(new TreeMapNode("a", NodeKind.Map, Sha1.Hash("Test2")));
            list = list.Merge(new TreeMapNode("c", NodeKind.Blob, Sha1.Hash("Test3")));
            list = list.Merge(new TreeMapNode("d", NodeKind.Map, Sha1.Hash("Test4")));
            list = list.Merge(new TreeMapNode("g", NodeKind.Blob, Sha1.Hash("Test5")));
            list = list.Merge(new TreeMapNode("e", NodeKind.Map, Sha1.Hash("Test6")));
            list = list.Merge(new TreeMapNode("f", NodeKind.Blob, Sha1.Hash("Test7")));

            var prev = list.Keys.First();
            foreach (var cur in list.Keys.Skip(1))
            {
                Assert.True(string.CompareOrdinal(cur, prev) > 0);
                prev = cur;
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Merge_Single_Exist))]
        public static void TreeNodeMap_Merge_Single_Exist()
        {
            // Arrange
            var list = new TreeMap();
            var expectedName = Guid.NewGuid().ToString();
            var expectedKind = NodeKind.Map;
            var expectedSha1 = Sha1.Hash(Guid.NewGuid().ToString());

            list = list.Merge(new TreeMapNode(expectedName, NodeKind.Blob, Sha1.Hash("Test1")));

            // Action
            var actual = list.Merge(new TreeMapNode(expectedName, expectedKind, expectedSha1));
            var actualNode = actual[expectedName];

            // Assert
            Assert.Equal(expectedName, actualNode.Name);
            Assert.Equal(expectedKind, actualNode.TreeRef.Kind);
            Assert.Equal(expectedSha1, actualNode.TreeRef.Sha1);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Merge_TreeNodeMap))]
        public static void TreeNodeMap_Merge_TreeNodeMap()
        {
            var list1 = new TreeMap();

            list1 = list1.Merge(new TreeMapNode("d", NodeKind.Map, Sha1.Hash("Test4")));
            list1 = list1.Merge(new TreeMapNode("e", NodeKind.Map, Sha1.Hash("Test5")));
            list1 = list1.Merge(new TreeMapNode("f", NodeKind.Blob, Sha1.Hash("Test6")));
            list1 = list1.Merge(new TreeMapNode("g", NodeKind.Blob, Sha1.Hash("Test7")));

            var list2 = new TreeMap();

            list2 = list2.Merge(new TreeMapNode("a", NodeKind.Map, Sha1.Hash("Test1")));
            list2 = list2.Merge(new TreeMapNode("b", NodeKind.Blob, Sha1.Hash("Test2")));
            list2 = list2.Merge(new TreeMapNode("c", NodeKind.Blob, Sha1.Hash("Test3")));
            list2 = list2.Merge(new TreeMapNode("d", NodeKind.Map, Sha1.Hash("Test4 Replace")));
            list2 = list2.Merge(new TreeMapNode("g", NodeKind.Blob, Sha1.Hash("Test5 Replace")));
            list2 = list2.Merge(new TreeMapNode("q", NodeKind.Map, Sha1.Hash("Test8")));
            list2 = list2.Merge(new TreeMapNode("r", NodeKind.Blob, Sha1.Hash("Test9")));

            var list3 = list1.Merge(list2);

            Assert.Equal(9, list3.Count);

            Assert.Equal("a", list3[0].Name);
            Assert.Equal("b", list3[1].Name);
            Assert.Equal("c", list3[2].Name);
            Assert.Equal("d", list3[3].Name);
            Assert.Equal("e", list3[4].Name);
            Assert.Equal("f", list3[5].Name);
            Assert.Equal("g", list3[6].Name);
            Assert.Equal("q", list3[7].Name);
            Assert.Equal("r", list3[8].Name);

            Assert.Equal(list2[0].TreeRef.Sha1, list3[0].TreeRef.Sha1);
            Assert.Equal(list2[1].TreeRef.Sha1, list3[1].TreeRef.Sha1);
            Assert.Equal(list2[2].TreeRef.Sha1, list3[2].TreeRef.Sha1);
            Assert.Equal(list2[3].TreeRef.Sha1, list3[3].TreeRef.Sha1);
            Assert.Equal(list1[1].TreeRef.Sha1, list3[4].TreeRef.Sha1);
            Assert.Equal(list1[2].TreeRef.Sha1, list3[5].TreeRef.Sha1);
            Assert.Equal(list2[4].TreeRef.Sha1, list3[6].TreeRef.Sha1);
            Assert.Equal(list2[5].TreeRef.Sha1, list3[7].TreeRef.Sha1);
            Assert.Equal(list2[6].TreeRef.Sha1, list3[8].TreeRef.Sha1);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Merge_Collection))]
        public static void TreeNodeMap_Merge_Collection()
        {
            var list1 = new TreeMap();

            list1 = list1.Merge(new TreeMapNode("d", NodeKind.Map, Sha1.Hash("Test4")));
            list1 = list1.Merge(new TreeMapNode("e", NodeKind.Map, Sha1.Hash("Test5")));
            list1 = list1.Merge(new TreeMapNode("f", NodeKind.Blob, Sha1.Hash("Test6")));
            list1 = list1.Merge(new TreeMapNode("g", NodeKind.Blob, Sha1.Hash("Test7")));

            var list2 = new[]
            {
                new TreeMapNode("c", NodeKind.Blob, Sha1.Hash("Test3")),
                new TreeMapNode("a", NodeKind.Map, Sha1.Hash("Test1")),
                new TreeMapNode("b", NodeKind.Blob, Sha1.Hash("Test2")),
                new TreeMapNode("d", NodeKind.Map, Sha1.Hash("Test4 Replace")),
                new TreeMapNode("g", NodeKind.Blob, Sha1.Hash("Test5 Replace")),
                new TreeMapNode("q", NodeKind.Map, Sha1.Hash("Test8")),
                new TreeMapNode("r", NodeKind.Blob, Sha1.Hash("Test9")),
            };

            var list3 = list1.Merge(list2);

            Assert.Equal(9, list3.Count);

            Assert.Equal("a", list3[0].Name);
            Assert.Equal("b", list3[1].Name);
            Assert.Equal("c", list3[2].Name);
            Assert.Equal("d", list3[3].Name);
            Assert.Equal("e", list3[4].Name);
            Assert.Equal("f", list3[5].Name);
            Assert.Equal("g", list3[6].Name);
            Assert.Equal("q", list3[7].Name);
            Assert.Equal("r", list3[8].Name);

            Assert.Equal(list2[1].TreeRef.Sha1, list3[0].TreeRef.Sha1);
            Assert.Equal(list2[2].TreeRef.Sha1, list3[1].TreeRef.Sha1);
            Assert.Equal(list2[0].TreeRef.Sha1, list3[2].TreeRef.Sha1);
            Assert.Equal(list2[3].TreeRef.Sha1, list3[3].TreeRef.Sha1);
            Assert.Equal(list1[1].TreeRef.Sha1, list3[4].TreeRef.Sha1);
            Assert.Equal(list1[2].TreeRef.Sha1, list3[5].TreeRef.Sha1);
            Assert.Equal(list2[4].TreeRef.Sha1, list3[6].TreeRef.Sha1);
            Assert.Equal(list2[5].TreeRef.Sha1, list3[7].TreeRef.Sha1);
            Assert.Equal(list2[6].TreeRef.Sha1, list3[8].TreeRef.Sha1);

            var dupes = new[]
            {
                new TreeMapNode(list2[0].Name, list2[0].TreeRef.Kind, list2[1].TreeRef.Sha1),
                new TreeMapNode(list2[1].Name, list2[1].TreeRef.Kind, list2[2].TreeRef.Sha1),
                new TreeMapNode(list2[2].Name, list2[2].TreeRef.Kind, list2[3].TreeRef.Sha1),
                new TreeMapNode(list2[3].Name, list2[3].TreeRef.Kind, list2[0].TreeRef.Sha1)
            };

            list3 = list3.Merge(dupes);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Delete))]
        public static void TreeNodeMap_Delete()
        {
            var sut = new TreeMap(
                new TreeMapNode("a", NodeKind.Blob, Sha1.Hash("a")),
                new TreeMapNode("b", NodeKind.Blob, Sha1.Hash("b")),
                new TreeMapNode("c", NodeKind.Blob, Sha1.Hash("c"))
            );

            var removed = sut.Remove("a");
            Assert.Equal(2, removed.Count);
            Assert.Equal("b", removed[0].Name);
            Assert.Equal("c", removed[1].Name);

            removed = sut.Remove("b");
            Assert.Equal(2, removed.Count);
            Assert.Equal("a", removed[0].Name);
            Assert.Equal("c", removed[1].Name);

            removed = sut.Remove("c");
            Assert.Equal(2, removed.Count);
            Assert.Equal("a", removed[0].Name);
            Assert.Equal("b", removed[1].Name);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_Delete_Predicate))]
        public static void TreeNodeMap_Delete_Predicate()
        {
            var sut = new TreeMap(
                new TreeMapNode("a", NodeKind.Blob, Sha1.Hash("a")),
                new TreeMapNode("b", NodeKind.Blob, Sha1.Hash("b")),
                new TreeMapNode("c", NodeKind.Blob, Sha1.Hash("c"))
            );

            var set = new HashSet<string>(StringComparer.Ordinal)
            { "a", "b", "d" };

            var removed = sut.RemoveWhere(set.Contains);
            Assert.Single(removed);
            Assert.Equal("c", removed[0].Name);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_IReadOnlyDictionary_Empty_GetEnumerator))]
        public static void TreeNodeMap_IReadOnlyDictionary_Empty_GetEnumerator()
        {
            // Arrange
            var treeNodeMap = new TreeMap();
            var readOnlyDictionary = treeNodeMap as IReadOnlyDictionary<string, TreeMapNode>;

            // Action
            var enumerator = readOnlyDictionary.GetEnumerator();

            // Assert
            Assert.False(enumerator.MoveNext());

            var current = enumerator.Current;
            Assert.Null(current.Key);
            Assert.Equal(TreeMapNode.Empty, current.Value);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(TreeNodeMap_IReadOnlyDictionary_GetEnumerator))]
        public static void TreeNodeMap_IReadOnlyDictionary_GetEnumerator()
        {
            // Arrange
            var nodes = new[] { Node0, Node1 };
            var treeNodeMap = new TreeMap(nodes);
            var readOnlyDictionary = treeNodeMap as IReadOnlyDictionary<string, TreeMapNode>;

            // Action
            var enumerator = readOnlyDictionary.GetEnumerator();

            // Assert
            Assert.True(enumerator.MoveNext());
            Assert.Equal(Node0, enumerator.Current.Value);

            Assert.True(enumerator.MoveNext());
            Assert.Equal(Node1, enumerator.Current.Value);

            Assert.False(enumerator.MoveNext());
            Assert.Equal(Node1, enumerator.Current.Value);
        }

        #endregion
    }
}
