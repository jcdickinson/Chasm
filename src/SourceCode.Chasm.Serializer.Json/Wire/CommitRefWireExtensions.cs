#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json;
using System.Json;

namespace SourceCode.Chasm.IO.Json.Wire
{
    internal static class CommitRefWireExtensions
    {
        #region Constants

        private const string _sha1 = "sha1";

        #endregion

        #region Methods

        public static JsonObject Convert(this CommitRef model)
        {
            if (model == CommitRef.Empty) return default; // null

            var wire = new JsonObject
            {
                [_sha1] = model.CommitId.Sha1.ToString("N")
            };

            return wire;
        }

        public static CommitRef ConvertCommitRef(this JsonObject wire)
        {
            if (wire == null) return default;

            // Sha1
            var jv = wire.GetValue(_sha1, JsonType.String, false);
            var sha1 = Sha1.Parse(jv);

            var commitId = new CommitId(sha1);

            var model = new CommitRef(commitId);
            return model;
        }

        public static CommitRef ParseCommitRef(this string json)
        {
            var wire = json.ParseJsonObject();

            var model = ConvertCommitRef(wire);
            return model;
        }

        #endregion
    }
}
