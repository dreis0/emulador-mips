using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minips.Instructions
{
    public class Instruction
    {
        public int[] Opcode { get; set; }
        public int[] PrimeiroRegistradorFonte { get; set; }
        public int[] SegundoRegistradorFonte { get; set; }
        public int[] RegistradorDestino { get; set; }
        public int[] Shamt { get; set; }
        public int[] Funct { get; set; }
    }

    public static class ConvertionExtensios
    {
        public static Instruction AsInstruction(this byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .Select(x => x.Reverse())
                .SelectMany(x => x);

            return new Instruction
            {
                Opcode = bits.Take(6).ToArray(),
                PrimeiroRegistradorFonte = bits.Skip(6).Take(5).ToArray(),
                SegundoRegistradorFonte = bits.Skip(11).Take(5).ToArray(),
                RegistradorDestino = bits.Skip(16).Take(5).ToArray(),
                Shamt = bits.Skip(21).Take(5).ToArray(),
                Funct = bits.Skip(26).Take(6).ToArray()
            };
        }

        //Método de conversão adaptado de: https://stackoverflow.com/questions/6758196/convert-int-to-a-bit-array-in-net
        public static int[] AsBitArray(this byte bt) =>
             new BitArray(new byte[] { bt })
                .Cast<bool>()
                .Select(bit => bit ? 1 : 0)
                .ToArray();
    }

}
