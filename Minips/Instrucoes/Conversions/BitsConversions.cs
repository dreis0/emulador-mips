using System;
using System.Collections;
using System.Linq;

namespace Minips.Instructions.Conversions
{
    public static class BitsConversions
    {
        //Método de conversão adaptado de: https://stackoverflow.com/questions/6758196/convert-int-to-a-bit-array-in-net
        public static int[] AsBitArray(this byte bt) =>
             new BitArray(new byte[] { bt })
                .Cast<bool>()
                .Select(bit => bit ? 1 : 0)
                .Reverse()
                .ToArray();

        public static int GetOpCode(this byte[] instruction)
        {
            var bits = instruction
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return bits.Take(6).ToArray().AsInt();
        }

        public static int GetFunct(this byte[] instruction)
        {
            var bits = instruction
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return bits.Skip(26).Take(6).ToArray().AsInt();
        }

        public static int AsInt(this int[] bits)
        {
            int value = 0;
            var reversed = bits.Reverse().ToArray();

            for (int i = 0; i < bits.Length; i++)
            {
                if (reversed[i] == 1)
                    value += 1 << i;
            }

            return value;
        }

        public static byte[] AsBytes(this int num) =>
            BitConverter.GetBytes(num).Reverse().ToArray();

        public static int AsInt(this byte[] bytes) =>
            bytes.SelectMany(x => x.AsBitArray()).ToArray().AsInt();

        public static byte[] AsBytes(this int[] bitArray)
        {
            int[] bytes = new int[4];
            for (int i = 0; i < 4; i++)
            {
                var b = bitArray.Skip(i * 8).Take(8).ToArray();
                bytes[i] = b.AsInt();
            }

            var asBytes = bytes.Select(x => x.AsBytes().Last());

            return asBytes.ToArray();
        }
    }
}
