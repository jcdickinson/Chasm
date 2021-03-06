#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Chasm.IO.Json;
using SourceCode.Chasm.IO.Proto;
using SourceCode.Chasm.IO.Text;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Chasm.IO.Tests
{
    internal sealed class TestData : IEnumerable<object[]>
    {
        #region Constants

        private readonly List<object[]> _data = new List<object[]>
        {
            // Json Serializer
            new object[]{ new JsonChasmSerializer() },

            // Proto Serializer
            new object[]{ new ProtoChasmSerializer() },

            // Text Serializer
            new object[]{ new TextChasmSerializer() }
        };

        // https://en.wikipedia.org/wiki/Special:CiteThisPage?page=Rick_Astley
        public const string LongStr = @"From Wikipedia: Astley was born on 6 February 1966 in Newton-le-Willows in Lancashire, the fourth child of his family. His parents divorced when he was five, and Astley was brought up by his father.[9] His musical career started when he was ten, singing in the local church choir.[10] During his schooldays, Astley formed and played the drums in a number of local bands, where he met guitarist David Morris.[2][11] After leaving school at sixteen, Astley was employed during the day as a driver in his father's market-gardening business and played drums on the Northern club circuit at night in bands such as Give Way – specialising in covering Beatles and Shadows songs – and FBI, which won several local talent competitions.[10]";

        public const string SurrogatePair = "\uD869\uDE01";

        #endregion

        #region Methods

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
