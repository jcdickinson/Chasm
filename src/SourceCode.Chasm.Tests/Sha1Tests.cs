﻿using SourceCode.Clay.Buffers;
using System;
using System.Text;
using Xunit;

namespace SourceCode.Chasm.Tests
{
    public static class Sha1Tests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_null_sha1))]
        public static void When_create_null_sha1()
        {
            var expected = Sha1.Empty;
            Assert.Equal(default, expected);

            // Null string
            var actual = Sha1.Hash((string)null);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Null bytes
            actual = Sha1.Hash((byte[])null);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            actual = Sha1.Hash(null, 0, 0);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Null segment
            actual = Sha1.Hash(default(ArraySegment<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Roundtrip string
            actual = Sha1.Parse(expected.ToString());
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Roundtrip formatted
            actual = Sha1.Parse(expected.ToString("D"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            actual = Sha1.Parse(expected.ToString("S"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // With hex specifier
            actual = Sha1.Parse("0x" + expected.ToString("D"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            actual = Sha1.Parse("0x" + expected.ToString("S"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Bytes))]
        public static void When_construct_sha1_from_Bytes()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLen + 10];

            // Construct Byte[]
            expected.CopyTo(buffer, 0);
            var actual = new Sha1(buffer);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Byte[] with offset 0
            expected.CopyTo(buffer, 0);
            actual = new Sha1(buffer, 0);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Byte[] with offset N
            expected.CopyTo(buffer, 5);
            actual = new Sha1(buffer, 5);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_ArraySegment))]
        public static void When_construct_sha1_from_ArraySegment()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLen + 10];

            // Construct Byte[] with offset N
            expected.CopyTo(buffer, 5);
            var actual = new Sha1(buffer, 5);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Segment
            expected.CopyTo(buffer, 0);
            var seg = new ArraySegment<byte>(buffer);
            actual = new Sha1(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Segment with offset 0
            expected.CopyTo(buffer, 0);
            seg = new ArraySegment<byte>(buffer, 0, Sha1.ByteLen);
            actual = new Sha1(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Segment with offset N
            expected.CopyTo(buffer, 5);
            seg = new ArraySegment<byte>(buffer, 5, Sha1.ByteLen);
            actual = new Sha1(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Memory))]
        public static void When_construct_sha1_from_Memory()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLen + 10];

            // Construct Memory
            expected.CopyTo(buffer, 0);
            var mem = new Memory<byte>(buffer);
            var actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Memory with offset 0
            expected.CopyTo(buffer, 0);
            mem = new Memory<byte>(buffer, 0, Sha1.ByteLen);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Memory with offset N
            expected.CopyTo(buffer, 5);
            mem = new Memory<byte>(buffer, 5, Sha1.ByteLen);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_ReadOnlyMemory))]
        public static void When_construct_sha1_from_ReadOnlyMemory()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLen + 10];

            // Construct ReadOnlyMemory
            expected.CopyTo(buffer, 0);
            var mem = new ReadOnlyMemory<byte>(buffer);
            var actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct ReadOnlyMemory with offset 0
            expected.CopyTo(buffer, 0);
            mem = new ReadOnlyMemory<byte>(buffer, 0, Sha1.ByteLen);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct ReadOnlyMemory with offset N
            expected.CopyTo(buffer, 5);
            mem = new ReadOnlyMemory<byte>(buffer, 5, Sha1.ByteLen);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Span))]
        public static void When_construct_sha1_from_Span()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLen + 10];

            // Construct Span
            expected.CopyTo(buffer, 0);
            var span = new Span<byte>(buffer);
            var actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Span with offset 0
            expected.CopyTo(buffer, 0);
            span = new Span<byte>(buffer, 0, Sha1.ByteLen);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct Span with offset N
            expected.CopyTo(buffer, 5);
            span = new Span<byte>(buffer, 5, Sha1.ByteLen);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_ReadOnlySpan))]
        public static void When_construct_sha1_from_ReadOnlySpan()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLen + 10];

            // Construct ReadOnlySpan
            expected.CopyTo(buffer, 0);
            var span = new ReadOnlySpan<byte>(buffer);
            var actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct ReadOnlySpan with offset 0
            expected.CopyTo(buffer, 0);
            span = new ReadOnlySpan<byte>(buffer, 0, Sha1.ByteLen);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);

            // Construct ReadOnlySpan with offset N
            expected.CopyTo(buffer, 5);
            span = new ReadOnlySpan<byte>(buffer, 5, Sha1.ByteLen);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            Assert.Equal(expected.Memory.Span, actual.Memory.Span, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_test_vector_1))]
        public static void When_create_test_vector_1()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html

            const string expected = "da39a3ee5e6b4b0d3255bfef95601890afd80709";
            var sha1 = Sha1.Parse(expected);
            {
                const string input = "";

                // String
                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);

                // Empty array
                actual = Sha1.Hash(Array.Empty<byte>()).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Hash(Array.Empty<byte>(), 0, 0).ToString();
                Assert.Equal(expected, actual);

                // Empty segment
                actual = Sha1.Hash(new ArraySegment<byte>(Array.Empty<byte>())).ToString();
                Assert.Equal(expected, actual);

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLen];
                sha1.CopyTo(buffer, 0);

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Buffer
                //actual = new Sha1(new ReadOnlyMemory<byte>(buffer)).ToString();
                //Assert.Equal(expected, actual);
                //Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_test_vector_2))]
        public static void When_create_test_vector_2()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html
            const string expected = "a9993e364706816aba3e25717850c26c9cd0d89d";
            var sha1 = Sha1.Parse(expected);
            {
                const string input = "abc";

                // String
                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha1.Hash(bytes, 0, bytes.Length).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLen];
                sha1.CopyTo(buffer, 0);

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_test_vector_3))]
        public static void When_create_test_vector_3()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html
            const string expected = "84983e441c3bd26ebaae4aa1f95129e5e54670f1";
            var sha1 = Sha1.Parse(expected);
            {
                const string input = "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq";

                // String
                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Hash(bytes, 0, bytes.Length).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLen];
                sha1.CopyTo(buffer, 0);

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_test_vector_4))]
        public static void When_create_test_vector_4()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html
            const string expected = "a49b2446a02c645bf419f995b67091253a04a259";
            var sha1 = Sha1.Parse(expected);
            {
                const string input = "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu";

                // String
                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Hash(bytes, 0, bytes.Length).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLen];
                sha1.CopyTo(buffer, 0);

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_test_vector_5))]
        public static void When_create_test_vector_5()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html
            const string expected = "34aa973cd4c4daa4f61eeb2bdbad27316534016f";
            var sha1 = Sha1.Parse(expected);
            {
                // String
                var input = new string('a', 1000_000);

                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Hash(bytes, 0, bytes.Length).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLen];
                sha1.CopyTo(buffer, 0);

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_narrow_string))]
        public static void When_create_sha_from_narrow_string()
        {
            for (var i = 1; i < 200; i++)
            {
                var str = new string(Char.MinValue, i);
                var sha1 = Sha1.Hash(str);

                Assert.NotEqual(Sha1.Empty, sha1);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_wide_string))]
        public static void When_create_sha_from_wide_string()
        {
            for (var i = 1; i < 200; i++)
            {
                var str = new string(Char.MaxValue, i);
                var sha1 = Sha1.Hash(str);

                Assert.NotEqual(Sha1.Empty, sha1);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_format_sha1))]
        public static void When_format_sha1()
        {
            const string expected_N = "a9993e364706816aba3e25717850c26c9cd0d89d";
            var sha1 = Sha1.Parse(expected_N);

            // ("N")
            {
                var actual = sha1.ToString(); // Default format
                Assert.Equal(expected_N, actual);

                actual = sha1.ToString("N");
                Assert.Equal(expected_N, actual);

                actual = sha1.ToString("n");
                Assert.Equal(expected_N, actual);
            }

            // ("D")
            {
                const string expected_D = "a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d";

                var actual = sha1.ToString("D");
                Assert.Equal(expected_D, actual);

                actual = sha1.ToString("d");
                Assert.Equal(expected_D, actual);
            }

            // ("S")
            {
                const string expected_S = "a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d";

                var actual = sha1.ToString("S");
                Assert.Equal(expected_S, actual);

                actual = sha1.ToString("s");
                Assert.Equal(expected_S, actual);
            }

            // (null)
            {
                Assert.Throws<FormatException>(() => sha1.ToString(null));
            }

            // ("")
            {
                Assert.Throws<FormatException>(() => sha1.ToString(""));
            }

            // (" ")
            {
                Assert.Throws<FormatException>(() => sha1.ToString(" "));
            }

            // ("x")
            {
                Assert.Throws<FormatException>(() => sha1.ToString("x"));
            }

            // ("ss")
            {
                Assert.Throws<FormatException>(() => sha1.ToString("nn"));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_parse_sha1))]
        public static void When_parse_sha1()
        {
            const string expected_N = "a9993e364706816aba3e25717850c26c9cd0d89d";
            var sha1 = Sha1.Parse(expected_N);

            // "N"
            {
                var actual = Sha1.Parse(sha1.ToString()); // Default format
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse(sha1.ToString("N"));
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("N"));
                Assert.Equal(sha1, actual);

                Assert.Throws<FormatException>(() => Sha1.Parse(sha1.ToString("N") + "a"));
            }

            // "D"
            {
                var actual = Sha1.Parse(sha1.ToString("D"));
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("D"));
                Assert.Equal(sha1, actual);

                Assert.Throws<FormatException>(() => Sha1.Parse(sha1.ToString("D") + "a"));
            }

            // "S"
            {
                var actual = Sha1.Parse(sha1.ToString("S"));
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S"));
                Assert.Equal(sha1, actual);

                Assert.Throws<FormatException>(() => Sha1.Parse(sha1.ToString("S") + "a"));
            }

            // null
            {
                Assert.Throws<FormatException>(() => Sha1.Parse(null));
            }

            // Empty
            {
                Assert.Throws<FormatException>(() => Sha1.Parse(""));
            }

            // Whitespace
            {
                Assert.Throws<FormatException>(() => Sha1.Parse(" "));
                Assert.Throws<FormatException>(() => Sha1.Parse("\t"));
                Assert.Throws<FormatException>(() => Sha1.Parse(" " + expected_N));
                Assert.Throws<FormatException>(() => Sha1.Parse(expected_N + " "));
            }

            // "0x"
            {
                Assert.Throws<FormatException>(() => Sha1.Parse("0x"));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_lexographic_compare_sha1))]
        public static void When_lexographic_compare_sha1()
        {
            var byt1 = new byte[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var sha1 = new Sha1(byt1);
            var str1 = sha1.ToString();
            Assert.Equal(@"000102030405060708090a0b0c0d0e0f10111213", str1);

            // Bump blit0
            var sha2 = new Sha1(sha1.Blit0 + 1, sha1.Blit1, sha1.Blit2);
            var str2 = sha2.ToString();
            Assert.Equal(@"_01_0102030405060708090a0b0c0d0e0f10111213".Replace("_", ""), str2);

            for (ushort i = 1; i < ushort.MaxValue; i++)
            {
                sha2 = new Sha1(sha1.Blit0 + i, sha1.Blit1, sha1.Blit2);
                Assert.True(sha2 > sha1);
            }

            // Bump blit1
            sha2 = new Sha1(sha1.Blit0, sha1.Blit1 + 1, sha1.Blit2);
            str2 = sha2.ToString();
            Assert.Equal(@"0001020304050607_09_090a0b0c0d0e0f10111213".Replace("_", ""), str2);

            for (ushort i = 1; i < ushort.MaxValue; i++)
            {
                sha2 = new Sha1(sha1.Blit0, sha1.Blit1 + i, sha1.Blit2);
                Assert.True(sha2 > sha1);
            }

            // Bump blit2
            sha2 = new Sha1(sha1.Blit0, sha1.Blit1, sha1.Blit2 + 1);
            str2 = sha2.ToString();
            Assert.Equal(@"000102030405060708090a0b0c0d0e0f_11_111213".Replace("_", ""), str2);

            for (ushort i = 1; i < ushort.MaxValue; i++)
            {
                sha2 = new Sha1(sha1.Blit0, sha1.Blit1, sha1.Blit2 + i);
                Assert.True(sha2 > sha1);
            }
        }
    }
}