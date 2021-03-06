using System;
using System.Collections;
using System.Linq;

namespace Minips.Instructions
{
    public abstract class BaseInstruction
    {
        public int Opcode { get; set; }
        public InstructionType Type { get; set; }
    }

    public class Instruction_R : BaseInstruction
    {
        public int PrimeiroRegistradorFonte { get; set; }
        public int SegundoRegistradorFonte { get; set; }
        public int RegistradorDestino { get; set; }
        public int Shamt { get; set; }
        public int Funct { get; set; }
    }

    public class Instruction_I : BaseInstruction
    {
        public int PrimeiroRegistradorFonte { get; set; }
        public int SegundoRegistradorFonte { get; set; }
        public int Immediate { get; set; }
    }

    public class Instruction_J : BaseInstruction
    {
        public int Address { get; set; }
    }

    public static class ConvertionExtensios
    {
        public static Instruction_R AsInstruction_R(this byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return new Instruction_R
            {
                Opcode = bits.Take(6).ToArray().AsInt(),
                PrimeiroRegistradorFonte = bits.Skip(6).Take(5).ToArray().AsInt(),
                SegundoRegistradorFonte = bits.Skip(11).Take(5).ToArray().AsInt(),
                RegistradorDestino = bits.Skip(16).Take(5).ToArray().AsInt(),
                Shamt = bits.Skip(21).Take(5).ToArray().AsInt(),
                Funct = bits.Skip(26).Take(6).ToArray().AsInt()
            };
        }

        public static Instruction_I AsInstruction_I(this byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return new Instruction_I
            {
                Opcode = bits.Take(6).ToArray().AsInt(),
                PrimeiroRegistradorFonte = bits.Skip(6).Take(5).ToArray().AsInt(),
                SegundoRegistradorFonte = bits.Skip(11).Take(5).ToArray().AsInt(),
                Immediate = bits.Skip(16).Take(16).ToArray().AsInt(),
            };
        }

        public static Instruction_J AsInstruction_J(this byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return new Instruction_J
            {
                Opcode = bits.Take(6).ToArray().AsInt(),
                Address = bits.Take(26).ToArray().AsInt()
            };
        }

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

        private static int AsInt(this int[] bits)
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
    }

}
