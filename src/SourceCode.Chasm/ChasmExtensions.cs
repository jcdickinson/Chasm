#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Chasm
{
    public static class ChasmExtensions
    {
        #region TreeMap

        public static TreeMap? Merge(this TreeMap? first, TreeMap? second)
        {
            if (!first.HasValue) return second;
            if (!second.HasValue) return first;
            return first.Value.Merge(second.Value);
        }

        public static TreeMap? Merge(this TreeMap? first, ICollection<TreeMapNode> second)
        {
            if (!first.HasValue) return new TreeMap(second);
            return first.Value.Merge(second);
        }

        public static TreeMap? Merge(this TreeMap? first, IEnumerable<TreeMapNode> second)
        {
            if (!first.HasValue) return new TreeMap(second);
            return first.Value.Merge(second);
        }

        public static TreeMap? Merge(this TreeMap? first, TreeMapNode second)
        {
            if (!first.HasValue) return new TreeMap(second);
            return first.Value.Merge(second);
        }

        public static TreeMap? Merge(this TreeMap? first, string name, NodeKind kind, Sha1 sha1)
        {
            if (!first.HasValue) return new TreeMap(new TreeMapNode(name, kind, sha1));
            return first.Value.Merge(name, kind, sha1);
        }

        public static TreeMap? Merge(this TreeMap? first, string name, TreeRef treeRef)
        {
            if (!first.HasValue) return new TreeMap(new TreeMapNode(name, treeRef));
            return first.Value.Merge(name, treeRef);
        }

        public static bool TryGetValue(this TreeMap? map, string key, out TreeMapNode value)
        {
            if (!map.HasValue)
            {
                value = default;
                return false;
            }
            return map.Value.TryGetValue(key, out value);
        }

        public static bool TryGetValue(this TreeMap? map, string key, NodeKind kind, out TreeMapNode value)
        {
            if (!map.HasValue)
            {
                value = default;
                return false;
            }
            return map.Value.TryGetValue(key, kind, out value);
        }

        #endregion
    }
}
