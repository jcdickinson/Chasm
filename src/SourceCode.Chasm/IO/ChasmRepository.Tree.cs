#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Chasm.IO
{
    partial class ChasmRepository // .Tree
    {
        #region Read

        public virtual async ValueTask<TreeMap?> ReadTreeAsync(TreeMapId treeMapId, CancellationToken cancellationToken)
        {
            // Read bytes
            var buffer = await ReadObjectAsync(treeMapId.Sha1, cancellationToken).ConfigureAwait(false);
            if (buffer == null) return default;

            // Deserialize
            var tree = Serializer.DeserializeTreeMap(buffer.Value.Span);
            return tree;
        }

        public virtual async ValueTask<IReadOnlyDictionary<TreeMapId, TreeMap>> ReadTreeBatchAsync(IEnumerable<TreeMapId> treeIds, CancellationToken cancellationToken)
        {
            if (treeIds == null) return ReadOnlyDictionary.Empty<TreeMapId, TreeMap>();

            // Read bytes in batch
            var sha1s = System.Linq.Enumerable.Select(treeIds, n => n.Sha1);
            var kvps = await ReadObjectBatchAsync(sha1s, cancellationToken).ConfigureAwait(false);

            // Deserialize batch
            if (kvps.Count == 0) return ReadOnlyDictionary.Empty<TreeMapId, TreeMap>();

            var dict = new Dictionary<TreeMapId, TreeMap>(kvps.Count);

            foreach (var kvp in kvps)
            {
                var tree = Serializer.DeserializeTreeMap(kvp.Value.Span);

                var treeId = new TreeMapId(kvp.Key);
                dict[treeId] = tree;
            }

            return dict;
        }

        public virtual async ValueTask<TreeMap?> ReadTreeAsync(string branch, string commitRefName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(branch)) throw new ArgumentNullException(nameof(branch));
            if (string.IsNullOrWhiteSpace(commitRefName)) throw new ArgumentNullException(nameof(commitRefName));

            // CommitRef
            var commitRef = await ReadCommitRefAsync(branch, commitRefName, cancellationToken).ConfigureAwait(false);

            // NotFound
            if (commitRef == null) return default;

            // Tree
            var tree = await ReadTreeAsync(commitRef.Value.CommitId, cancellationToken).ConfigureAwait(false);
            return tree;
        }

        public virtual async ValueTask<TreeMap?> ReadTreeAsync(CommitId commitId, CancellationToken cancellationToken)
        {
            // Commit
            var commit = await ReadCommitAsync(commitId, cancellationToken).ConfigureAwait(false);
            if (commit == null) return default;

            // Tree
            if (commit.Value.TreeId == null) return default;
            var tree = await ReadTreeAsync(commit.Value.TreeId.Value, cancellationToken).ConfigureAwait(false);

            return tree;
        }

        #endregion

        #region Write

        public virtual async ValueTask<TreeMapId> WriteTreeAsync(TreeMap tree, CancellationToken cancellationToken)
        {
            using (var session = Serializer.Serialize(tree))
            {
                var sha1 = Sha1.Hash(session.Result);

                await WriteObjectAsync(sha1, session.Result, false, cancellationToken).ConfigureAwait(false);

                var model = new TreeMapId(sha1);
                return model;
            }
        }

        public virtual async ValueTask<CommitId> WriteTreeAsync(IReadOnlyList<CommitId> parents, TreeMap tree, Audit author, Audit committer, string message, CancellationToken cancellationToken)
        {
            var treeId = await WriteTreeAsync(tree, cancellationToken).ConfigureAwait(false);
            var commit = new Commit(parents, treeId, author, committer, message);
            var commitId = await WriteCommitAsync(commit, cancellationToken).ConfigureAwait(false);

            return commitId;
        }

        #endregion
    }
}
