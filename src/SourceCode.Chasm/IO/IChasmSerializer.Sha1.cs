﻿using SourceCode.Clay.Buffers;
using System;

namespace SourceCode.Chasm.IO
{
    partial interface IChasmSerializer // .Sha1
    {
        BufferSession Serialize(Sha1 model);

        Sha1 DeserializeSha1(ReadOnlySpan<byte> span);
    }
}
