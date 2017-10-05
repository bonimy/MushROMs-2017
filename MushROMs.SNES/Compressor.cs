// We make the Compressor class nonstatic so it can be multithreaded (since we need a nonlocal
// copy of a SuffixTree to maintain optimization).

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Helper;

namespace MushROMs.SNES
{
    public class Compressor
    {
        private const int CHAR_BIT = 8;

        private const string DllPath = @"C:\Lunar Compress (x64).dll";

        // A static compressor class to use if operating on a single thread.
        public static readonly Compressor Default = new Compressor();

        private static readonly int[] CommandSizes = { -1, 1, 2, 1, 2 };

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static unsafe extern void* MemoryCopy(void* dest, void* src, IntPtr count);
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static unsafe extern void* MemorySet(void* dest, int c, IntPtr count);
        [DllImport(DllPath)]
        private static unsafe extern int LunarRecompress(void* source, void* dest, int size, int max, int format, int format2);

        public byte[] LunarRecompress(byte[] data, int start, int size)
        {
            unsafe
            {
                fixed (byte* src = &data[start])
                {
                    var compress = new byte[LunarRecompress(src, null, size, 0x10000, 0, 0)];
                    fixed (byte* dest = compress)
                        LunarRecompress(src, dest, size, compress.Length, 0, 0);
                    return compress;
                }
            }
        }

        private SuffixTree Tree
        {
            get;
            set;
        }
        private CompressList Commands
        {
            get;
            set;
        }

        public Compressor()
        {
            Tree = new SuffixTree();
            Commands = new CompressList(0x100);
        }

        public void Test()
        {
            const int dlen = 0x10000;
            const string dir = @"Z:\Libraries\Dropbox\Private\Emulation\Dev\Super Mario World\The Mario Project\Graphics";

            var files = Directory.GetFiles(dir);
            foreach (var file in files)
            {
                var data = File.ReadAllBytes(file);
                var lunar = new byte[dlen];
                var mushrom = new byte[dlen];
                var lunsrc = new byte[dlen];
                var mushsrc = new byte[dlen];

                unsafe
                {
                    fixed (byte* src = data)
                    fixed (byte* ldest = lunar)
                    fixed (byte* mdest = mushrom)
                    fixed (byte* lsrc = lunsrc)
                    fixed (byte* msrc = mushsrc)
                    {
                        var slen = data.Length;
                        var llen = LunarRecompress(src, ldest, slen, dlen, 0, 0);
                        var mlen = Compress(mdest, dlen, src, slen, true);

                        if (mlen == llen)
                            continue;

                        var list1 = GetCompressList(ldest, dlen);
                        var list2 = GetCompressList(mdest, dlen);

                        var dllen = Decompress(lsrc, dlen, ldest, llen);
                        var dmlen = Decompress(msrc, dlen, mdest, mlen);
                        Debug.Assert(dllen == dmlen);
                        for (int i = 0; i < dllen; i++)
                            if (lsrc[i] != msrc[i])
                                Debug.Assert(false);

                        int l1 = 0, l2 = 0, d1 = 0, d2 = 0;
                        for (int i = 0, j = 0; i < list1.Count && j < list2.Count; i++, j++)
                        {
                            l1 += GetLength(list1[i]);
                            l2 += GetLength(list2[j]);
                            d1 += list1[i].Length;
                            d2 += list2[j].Length;

                            while (d1 != d2 && i < list1.Count && j < list2.Count)
                            {
                                while (d1 < d2 && ++i < list1.Count)
                                {
                                    d1 += list1[i].Length;
                                    l1 += GetLength(list1[i]);
                                }
                                while (d1 > d2 && ++j < list2.Count)
                                {
                                    d2 += list2[j].Length;
                                    l2 += GetLength(list2[j]);
                                }
                            }
                            if (d1 != d2 || l1 != l2)
                                Debug.Assert(false);
                        }
                        Debug.Assert(l1 != llen && l2 != mlen);
                    }
                }
            }
        }

        private static int GetLength(CompressModule module)
        {
            var len = module.Length > 0x20 ? 2 : 1;
            switch (module.Command)
            {
            case Command.DirectCopy:
                return len + module.Length;
            case Command.RepeatedByte:
            case Command.IncrementingByte:
                return len + 1;
            case Command.RepeatedWord:
            case Command.CopySection:
                return len + 2;
            default:
                Debug.Assert(false);
                return 0;
            }
        }

        private static unsafe List<CompressModule> GetCompressList(byte* src, int slen)
        {
            var list = new List<CompressModule>();

            int sindex = 0, dindex = 0, clen;

            while (sindex < slen)
            {
                if (src[sindex] == 0xFF)
                    return list;

                // Command is three most significant bits
                var command = (Command)(src[sindex] >> 5);

                // Signifies extended length copy.
                if (command == Command.LongCommand)
                {
                    // Get new command
                    command = (Command)((src[sindex] >> 2) & 0x07);
                    if (command == Command.LongCommand)
                        return null;

                    // Length is ten least significant bits.
                    clen = ((src[sindex] & 0x03) << CHAR_BIT);
                    clen |= src[++sindex];
                }
                else
                    clen = src[sindex] & 0x1F;

                clen++;
                sindex++;

                var value = 0;
                switch (command)
                {
                case Command.RepeatedByte:
                case Command.IncrementingByte:
                    value = src[sindex];
                    break;
                case Command.RepeatedWord:
                case Command.CopySection:
                    value = src[sindex] | (src[sindex + 1] << 8);
                    break;
                }

                list.Add(new CompressModule(command, value, dindex, clen));

                switch (command)
                {
                case Command.DirectCopy: // Direct byte copy
                    dindex += clen;
                    sindex += clen;
                    continue;
                case Command.RepeatedByte: // Fill with one byte repeated
                    dindex += clen;
                    sindex++;
                    continue;
                case Command.RepeatedWord: // Fill with two alternating bytes
                    dindex += clen;
                    sindex += 2;
                    continue;
                case Command.IncrementingByte: // Fill with incrementing byte value
                    dindex += clen;
                    sindex++;
                    continue;
                case Command.CopySection: // Copy data from previous section
                    dindex += clen;
                    sindex += 2;
                    continue;
                case (Command)5:
                case (Command)6:
                    return null;
                default:
                    Debug.Assert(false);
                    return null;
                }
            }
            return null;
        }

        public static int GetDecompressLength(byte[] compressedData)
        {
            return GetDecompressLength(compressedData, 0, compressedData.Length);
        }
        public static int GetDecompressLength(byte[] compressedData, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > compressedData.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            unsafe
            {
                fixed (byte* ptr = &compressedData[startIndex])
                    return Decompress(null, 0, ptr, length);
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            return Decompress(compressedData, 0, compressedData.Length);
        }
        public static byte[] Decompress(byte[] compressedData, int startIndex, int length)
        {
            var dlen = GetDecompressLength(compressedData);
            if (dlen == 0)
                return null;

            return Decompress(dlen, compressedData, startIndex, length);
        }

        public static byte[] Decompress(int decompressLength, byte[] compressedData)
        {
            return Decompress(decompressLength, compressedData, 0, compressedData.Length);
        }
        public static byte[] Decompress(int decompressLength, byte[] compressedData, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > compressedData.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (decompressLength < 0)
                throw new ArgumentOutOfRangeException(nameof(decompressLength));

            var dest = new byte[decompressLength];

            unsafe
            {
                fixed (byte* ptr = &compressedData[startIndex])
                fixed (byte* decompress = dest)
                    if (Decompress(decompress, decompressLength, ptr, length) == 0)
                        return null;
            }

            return dest;
        }

        private static unsafe int Decompress(byte* dest, int dlen, byte* src, int slen)
        {
            int sindex = 0, dindex = 0, clen;

            while (sindex < slen)
            {
                if (src[sindex] == 0xFF)
                    return dindex;

                // Command is three most significant bits
                var command = (Command)(src[sindex] >> 5);

                // Signifies extended length copy.
                if (command == Command.LongCommand)
                {
                    // Get new command
                    command = (Command)((src[sindex] >> 2) & 0x07);
                    if (command == Command.LongCommand)
                        return 0;

                    // Length is ten least significant bits.
                    clen = ((src[sindex] & 0x03) << CHAR_BIT);
                    clen |= src[++sindex];
                }
                else
                    clen = src[sindex] & 0x1F;

                clen++;
                sindex++;

                switch (command)
                {
                case Command.DirectCopy: // Direct byte copy
                    if (dest != null)
                    {
                        if (dindex + clen > dlen)
                            return 0;
                        if (sindex + clen > slen)
                            return 0;
                        MemoryCopy(dest + dindex, src + sindex, (IntPtr)clen);
                    }
                    dindex += clen;
                    sindex += clen;
                    continue;
                case Command.RepeatedByte: // Fill with one byte repeated
                    if (dest != null)
                    {
                        if (dindex + clen > dlen)
                            return 0;
                        if (sindex >= slen)
                            return 0;
                        MemorySet(dest + dindex, src[sindex], (IntPtr)clen);
                    }
                    dindex += clen;
                    sindex++;
                    continue;
                case Command.RepeatedWord: // Fill with two alternating bytes
                    if (dest != null)
                    {
                        if (dindex + clen > dlen)
                            return 0;
                        if (sindex + 1 >= slen)
                            return 0;
                        MemorySet(dest + dindex, src[sindex], (IntPtr)clen);
                        for (int i = 1, j = src[sindex + 1]; i < clen; i += 2)
                            dest[dindex + i] = (byte)j;
                    }
                    dindex += clen;
                    sindex += 2;
                    continue;
                case Command.IncrementingByte: // Fill with incrementing byte value
                    if (dest != null)
                    {
                        if (dindex + clen > dlen)
                            return 0;
                        if (sindex >= slen)
                            return 0;
                        for (int i = 0, j = src[sindex]; i < clen; i++, j++)
                            dest[dindex + i] = (byte)j;
                    }
                    dindex += clen;
                    sindex++;
                    continue;
                case Command.CopySection: // Copy data from previous section
                    if (dest != null)
                    {
                        if (dindex + clen > dlen)
                            return 0;
                        if (sindex + 1 >= slen)
                            return 0;

                        // We have to manually do this copy in case of overlapping regions (memmove does not work).
                        var write = dest + dindex;
                        var read = dest + ((src[sindex + 1] << CHAR_BIT) | src[sindex]);
                        for (int i = 0; i < clen; i++)
                            write[i] = read[i];
                    }
                    dindex += clen;
                    sindex += 2;
                    continue;
                case (Command)5:
                case (Command)6:
                    return 0;
                default:
                    Debug.Assert(false);
                    return 0;
                }
            }
            return 0;
        }

        public int GetCompressLength(byte[] data)
        {
            return GetCompressLength(data, 0, data.Length);
        }
        public int GetCompressLength(byte[] data, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > data.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            unsafe
            {
                fixed (byte* ptr = &data[startIndex])
                    return GetCompressLength(ptr, length);
            }
        }

        private unsafe int GetCompressLength(byte* data, int length)
        {
            return Compress(null, 0, data, length, true);
        }

        public byte[] Compress(byte[] data)
        {
            return Compress(data, 0, data.Length);
        }
        public byte[] Compress(byte[] data, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > data.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));


            unsafe
            {
                fixed (byte* ptr = &data[startIndex])
                {
                    var compressLength = GetCompressLength(ptr, length);
                    var dest = new byte[compressLength];
                    fixed (byte* compress = dest)
                        if (Compress(compress, compressLength, ptr, length, false) == 0)
                            return null;
                    return dest;
                }
            }
        }

        public byte[] Compress(int compressLength, byte[] data)
        {
            return Compress(compressLength, data, 0, data.Length);
        }
        public byte[] Compress(int compressLength, byte[] data, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > data.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (compressLength < 0)
                throw new ArgumentOutOfRangeException(nameof(compressLength));

            var dest = new byte[compressLength];

            unsafe
            {
                fixed (byte* ptr = &data[startIndex])
                fixed (byte* compress = dest)
                    if (Compress(compress, compressLength, ptr, length, true) == 0)
                        return null;
            }

            return dest;
        }

        private unsafe int Compress(byte* dest, int dlen, byte* src, int slen, bool init)
        {
            /*
             * To maximize compressed data space and decompression time, we need to examine
             * several edge cases. For example, the minimum number of bytes to reduce space
             * for a repeating byte sequence is three
             * 
             * data:     00 00 00
             * compress: 12 00       - Using repeating byte compression
             * altcomp:  02 00 00    - Direct copy costs space
             * 
             * However, if there is an uncompressable byte before and after the data, we get
             * 
             * data:     01 00 00 00 10
             * compress: 00 01|12 00|00 10 - Direct copy, repeating, and direct copy
             * altcomp:  04 01 00 00 00 10 - One continuous direct copy
             * 
             * Both methods take the same amount of space, but the single direct copy would be
             * preferred since it reduces computation time when decompressing.
             * 
             * data:     00 00 00 10
             * compress: 12 00|00 10
             * altcomp:  03 00 00 00 10
             * 
             * data:     01 00 00 00
             * compress: 00 01|12 00
             * altcomp:  03 01 00 00 00
             * 
             * data:     00 00 00 01 00 00 00 02
             * compress: 12 00|00 01|12 00|00 01
             * altcomp:  12 00|04 01 00 00 00 02
             * 
             * We run into more edge case pain when one or both sides of uncompressable data is over 32 bytes
             * data:     [n <= 0x20 uncompressable bytes] 00 00 00 [m > 0x20 - n uncompressable bytes]
             * compress: 0n [n bytes]|12 00|0m [m bytes] - n + m + 4 bytes
             * altcomp:  xx xx [n + m + 3 bytes] - n + m + 5 bytes
             * 
             * data:     [n > 0x20] 00 00 00 [m <= 0x20]
             * compress: xx xx [n bytes]|12 00|0m [m bytes] - n + m + 5 bytes
             * altcomp:  xx xx [n + m + 3 bytes] - n + m + 5 bytes
             * 
             * data:     [n > 0x20] 00 00 00 [m > 0x20]
             * compress: xx xx [n bytes]|12 00|xx xx [m bytes] - n + m + 6 bytes
             * altcomp:  xx xx [n + m + 3 bytes] - n + m + 5 bytes
             * 
             * We might have a corner case where uncompressed data is over 0x400 bytes, but this will only
             * cost us one byte (maybe). And seems incredibly hard to implement efficiently. So we will
             * leave it alone, but keep it mind that it exists.
             */

            // Current source and position index
            int sindex = 0, dindex = 0, len = 0, last = 0, lastdindex = 0;

            if (!init)
                goto _begin;

            Tree.CreateTree((IntPtr)src, slen);
            Commands.Clear();

            var prev = new CompressModule(Command.DirectCopy, 0, 0, 0);

            // Find all acceptable compression commands
            while (sindex < slen)
            {
                // Get the current byte
                var val0 = src[sindex];

                // See if we have a repeating substring.
                var substring = Tree.GetLongestInternalSubstring(sindex);

                // Ensure we can read a second byte
                if (sindex + 1 < slen)
                {
                    // Get next byte
                    var val1 = src[sindex + 1];

                    // Ensure we can read the third byte
                    if (sindex + 2 < slen)
                    {
                        // Get third byte
                        var val2 = src[sindex + 2];

                        // We have a repeated word (or maybe three repeated bytes)
                        if (val2 == val0)
                        {
                            // We know for sure the command length is at least three bytes
                            len = 3;

                            // If first and second byte are equal, then this is a repeated byte copy
                            if (val0 == val1)
                                goto _repeatedByteCopy;

                            // Determine how long the repeated word copy goes
                            for (; sindex + len < slen; len++)
                            {
                                if (src[sindex + len] != val1)
                                    break;
                                if (sindex + ++len >= slen)
                                    break;
                                if (src[sindex + len] != val0)
                                    break;
                            }

                            if (len > 3)
                            {
                                if (substring.Length > len)
                                {
                                    Commands.Add(Command.CopySection, substring.Start, sindex, substring.Length);
                                    sindex += substring.Length;
                                    last = sindex;
                                    continue;
                                }
                                // Get the repeated word value
                                Commands.Add(Command.RepeatedWord, val0 + (val1 << CHAR_BIT), sindex, len);
                                sindex += len;
                                last = sindex;
                                continue;
                            }
                        } // val2 != val0
                    } // sindex + 2 >= slen

                    // See if we have a repeating byte sequence
                    if (val1 != val0)
                        goto _incrementedByteCheck;

                    len = 2;

                    // Note that this label is called when len = 3.
                    _repeatedByteCopy:
                    for (; sindex + len < slen; len++)
                        if (src[sindex + len] != val0)
                            break;

                    if (len > 2)
                    {
                        if (substring.Length > len + 1)
                        {
                            Commands.Add(Command.CopySection, substring.Start, sindex, substring.Length);
                            sindex += substring.Length;
                            last = sindex;
                            continue;
                        }
                        // Set repeated byte value
                        Commands.Add(Command.RepeatedByte, val0, sindex, len);
                        sindex += len;
                        last = sindex;
                        continue;
                    }

                    // See if we have an incrementing byte sequence.
                    _incrementedByteCheck:
                    if (val1 == (byte)(val0 + 1))
                    {
                        // Determine how long the incrementing byte sequence goes for.
                        for (len = 2; sindex + len < slen; len++)
                            if (src[sindex + len] != (byte)(val0 + len))
                                break;

                        if (len > 2)
                        {
                            if (substring.Length > len + 1)
                            {
                                Commands.Add(Command.CopySection, substring.Start, sindex, substring.Length);
                                sindex += substring.Length;
                                last = sindex;
                                continue;
                            }
                            // Set incrementing byte value.
                            Commands.Add(Command.IncrementingByte, val0, sindex, len);
                            sindex += len;
                            last = sindex;
                            continue;
                        }
                    } // val1 != (byte)(val0 + 1)
                } // sindex + 1 >= slen

                if (substring.Length > 3)
                {
                    Commands.Add(Command.CopySection, substring.Start, sindex, substring.Length);
                    sindex += substring.Length;
                    last = sindex;
                    continue;
                }

                sindex++;
                continue;
            }

            // Add the last direct copy if we must
            if (sindex != last)
                Commands.Add(Command.DirectCopy, 0, last, sindex - last);

            // Now we write the compression commands
            dindex = 0;
            lastdindex = 0;

            _begin:
            int i = 0, count = Commands.Count;
            CompressModule current, next;
            var previous = new CompressModule((Command)(-1), 0, 0, 0);

            _loop:
            while (i < count - 1)
            {
                current = Commands[i];
                next = Commands[i + 1];

                if (previous.Command == Command.DirectCopy && next.Command == Command.DirectCopy)
                {
                    var tlen = next.End - previous.Index;
                    var add = (previous.Length > 0x20 && next.Length > 0x20) ? 1 : 0;
                    var edge = (previous.Length <= 0x20 && next.Length <= 0x20) && tlen > 0x20;

                    switch (current.Command)
                    {
                    case Command.RepeatedByte:
                    case Command.IncrementingByte:
                        if (edge && current.Length == 3)
                            break;
                        if (current.Length > 3 + add)
                            break;
                        Commands[++i] = new CompressModule(Command.DirectCopy, 0, previous.Index, tlen);
                        dindex = lastdindex;
                        continue;
                    case Command.RepeatedWord:
                    case Command.CopySection:
                        if (edge && current.Length == 4)
                            break;
                        if (current.Length > 4 + add)
                            break;
                        Commands[++i] = new CompressModule(Command.DirectCopy, 0, previous.Index, tlen);
                        dindex = lastdindex;
                        continue;
                    }
                }

                goto _write;
            }
            if (i < count)
            {
                current = Commands[i];

                if (previous.Command == Command.DirectCopy)
                {
                    switch (current.Command)
                    {
                    case Command.RepeatedByte:
                    case Command.IncrementingByte:
                        if (current.Length == 3)
                        {
                            current = new CompressModule(Command.DirectCopy, 0, previous.Index, current.End - previous.Index);
                            dindex = lastdindex;
                            break;
                        }
                        break;
                    case Command.RepeatedWord:
                    case Command.CopySection:
                        if (current.Length == 4)
                        {
                            current = new CompressModule(Command.DirectCopy, 0, previous.Index, current.End - previous.Index);
                            dindex = lastdindex;
                            break;
                        }
                        break;
                    }
                }
                goto _write;
            }
            goto _end;

            _write:
            var command = current.Command;
            for (len = current.Length; len > 0;)
            {
                var sublen = Math.Min(len, 0x400);
                if (dest != null)
                {
                    if (sublen > 0x20)
                    {
                        if (command == Command.DirectCopy)
                        {
                            if (dindex + 2 + sublen > dlen)
                                return 0;
                        }
                        else if (dindex + 2 + CommandSizes[(int)command] + 1 > dlen)
                            return 0;

                        lastdindex = dindex;
                        dest[dindex++] = (byte)(((int)Command.LongCommand << (CHAR_BIT - 3)) | ((int)command << 2) | (--sublen >> CHAR_BIT));
                        dest[dindex++] = (byte)sublen++;
                    }
                    else
                    {
                        if (command == Command.DirectCopy)
                        {
                            if (dindex + 1 + sublen > dlen)
                                return 0;
                        }
                        else if (dindex + 1 + CommandSizes[(int)command] + 1 > dlen)
                            return 0;
                        lastdindex = dindex;
                        dest[dindex++] = (byte)(((int)command) << (CHAR_BIT - 3) | (sublen - 1));
                    }
                    switch (command)
                    {
                    case Command.RepeatedByte:
                    case Command.IncrementingByte:
                        dest[dindex++] = (byte)current.Value;
                        break;
                    case Command.RepeatedWord:
                        dest[dindex++] = (byte)current.Value;
                        dest[dindex++] = (byte)(current.Value >> CHAR_BIT);
                        break;
                    case Command.CopySection:
                        dest[dindex++] = (byte)current.Value;
                        dest[dindex++] = (byte)(current.Value >> CHAR_BIT);
                        break;
                    case Command.DirectCopy:
                        MemoryCopy(dest + dindex, src + current.Index, (IntPtr)sublen);
                        dindex += sublen;
                        break;
                    default:
                        Debug.Assert(false);
                        return 0;
                    }
                }
                else
                {
                    lastdindex = dindex;
                    if (sublen > 0x20)
                        dindex += 2;
                    else
                        dindex++;

                    if (command == Command.DirectCopy)
                        dindex += sublen;
                    else
                        dindex += CommandSizes[(int)current.Command];
                }

                len -= sublen;
            }
            previous = Commands[i++];
            goto _loop;

            _end:
            if (dest != null)
                dest[dindex] = 0xFF;

            return ++dindex;
        }

        [DebuggerDisplay("Index = {Index}, Length = {Length}, Command = {Command}")]
        private struct CompressModule
        {
            public static readonly CompressModule Null = new CompressModule();

            public Command Command
            {
                get;
                set;
            }
            public int Value
            {
                get;
                set;
            }
            public int Index
            {
                get;
                set;
            }
            public int Length
            {
                get;
                set;
            }
            public int End => Index + Length;
            public bool IsNull => Length == 0;
            public CompressModule(Command command, int value, int index, int length)
            {
                Command = command;
                Value = value;
                Index = index;
                Length = length;
            }

            public static bool operator ==(CompressModule left, CompressModule right)
            {
                return left.Command == right.Command &&
                    left.Value == right.Value &&
                    left.Index == right.Index &&
                    left.Length == right.Length;
            }
            public static bool operator !=(CompressModule left, CompressModule right)
            {
                return !(left == right);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is CompressModule))
                    return false;
                return (CompressModule)obj == this;
            }
            public override int GetHashCode()
            {
                return Hash.Generate((int)Command, Value, Index, Length);
            }
        }

        private class CompressList : List<CompressModule>
        {
            public CompressList() : base()
            {
            }
            public CompressList(int capacity) : base(capacity)
            {
            }
            public CompressList(IEnumerable<CompressModule> collection) : base(collection)
            {
            }

            public void Add(Command command, int value, int index, int length)
            {
                Add(new CompressModule(command, value, index, length));
            }

            public new void Add(CompressModule module)
            {
                var command = module.Command;
                if (command != Command.DirectCopy)
                {
                    if (Count > 0)
                    {
                        var last = this[Count - 1];
                        if (last.End != module.Index)
                            base.Add(new CompressModule(Command.DirectCopy, 0, last.End, module.Index - last.End));
                    }
                    else if (module.Index > 0)
                        base.Add(new CompressModule(Command.DirectCopy, 0, 0, module.Index));
                }
                base.Add(module);
            }
        }

        private enum Command
        {
            DirectCopy = 0,
            RepeatedByte = 1,
            RepeatedWord = 2,
            IncrementingByte = 3,
            CopySection = 4,
            LongCommand = 7
        }
    }
}
