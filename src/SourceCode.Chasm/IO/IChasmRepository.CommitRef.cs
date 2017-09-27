﻿using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Chasm.IO
{
    partial interface IChasmRepository // .CommitRef
    {
        #region Read

        CommitId ReadCommitRef(string repo, string name);

        ValueTask<CommitId> ReadCommitRefAsync(string repo, string name, CancellationToken cancellationToken);

        #endregion

        #region Write

        /// <summary>
        /// Write a <see cref="CommitRef"/> to the repository using the provided values.
        /// Note that the underlying store should use pessimistic concurrency control to prevent data loss.
        /// </summary>
        /// <param name="repo">The repository name.</param>
        /// <param name="name">The name of the <see cref="CommitRef"/></param>
        /// <param name="previousCommitId">The previous <see cref="CommitId"/> that the caller used for reading.</param>
        /// <param name="newCommitId">The new <see cref="CommitId"/> that represents the content being written.</param>
        /// <returns></returns>
        void WriteCommitRef(CommitId? previousCommitId, string repo, string name, CommitId newCommitId);

        /// <summary>
        /// Write a <see cref="CommitRef"/> to the repository using the provided values.
        /// Note that the underlying store should use pessimistic concurrency control to prevent data loss.
        /// </summary>
        /// <param name="repo">The repository name.</param>
        /// <param name="name">The name of the <see cref="CommitRef"/></param>
        /// <param name="previousCommitId">The previous <see cref="CommitId"/> that the caller used for reading.</param>
        /// <param name="newCommitId">The new <see cref="CommitId"/> that represents the content being written.</param>
        /// <param name="cancellationToken">Allows the <see cref="Task"/> to be cancelled.</param>
        /// <returns></returns>
        Task WriteCommitRefAsync(CommitId? previousCommitId, string repo, string name, CommitId newCommitId, CancellationToken cancellationToken);

        #endregion
    }
}
