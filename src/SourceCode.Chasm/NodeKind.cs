#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Chasm
{
#pragma warning disable CA1028 // Enum Storage should be Int32

    public enum NodeKind : byte
    {
        Blob = 0, // Default

        Map = 1,

        List = 2,

        Set = 3
    }

#pragma warning restore CA1028 // Enum Storage should be Int32
}
