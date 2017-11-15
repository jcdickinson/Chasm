#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Chasm.IO.Text.Wire
{
    internal static class TreeNodeMapExtensions
    {
        #region Methods

        public static string Convert(this TreeMap model)
        {
            switch (model.Count)
            {
                case 0:
                    return string.Empty;

                case 1:
                    {
                        var wire = model[0].Convert();
                        return wire;
                    }

                default:
                    {
                        var sb = new StringBuilder();
                        for (var i = 0; i < model.Count; i++)
                        {
                            var text = model[i].Convert();
                            sb.AppendLine(text);
                        }

                        var wire = sb.ToString();
                        return wire;
                    }
            }
        }

        public static TreeMap ConvertTree(this string wire)
        {
            if (string.IsNullOrWhiteSpace(wire)) return default;

            var tokens = wire.Split('\n', StringSplitOptions.None);
            if (tokens.Length == 0) return TreeMap.Empty;

            var nodes = new List<TreeMapNode>(tokens.Length);
            for (var i = 0; i < tokens.Length; i++)
            {
                var text = tokens[i].Trim();
                if (string.IsNullOrWhiteSpace(text)) break;

                var node = text.ConvertTreeNode();
                nodes.Add(node);
            }

            var model = new TreeMap(nodes);
            return model;
        }

        #endregion
    }
}
