using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minips.Instructions.Conversions
{
    public static class InstructionConversionsExtensions
    {
        public static Instruction_R AsInstruction_R(this byte[] bytes, InstructionInfo info)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return new Instruction_R
            {
                Opcode = bits.Take(6).ToArray().AsInt(),
                RS = bits.Skip(6).Take(5).ToArray().AsInt(),
                RT = bits.Skip(11).Take(5).ToArray().AsInt(),
                RD = bits.Skip(16).Take(5).ToArray().AsInt(),
                Shamt = bits.Skip(21).Take(5).ToArray().AsInt(),
                Funct = bits.Skip(26).Take(6).ToArray().AsInt(),
                Info = info
            };
        }

        public static Instruction_I AsInstruction_I(this byte[] bytes, InstructionInfo info)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return new Instruction_I
            {
                Opcode = bits.Take(6).ToArray().AsInt(),
                RS = bits.Skip(6).Take(5).ToArray().AsInt(),
                RT = bits.Skip(11).Take(5).ToArray().AsInt(),
                Immediate = bits.Skip(16).Take(16).ToArray().AsTwoComplementInt(),
                Info = info
            };
        }

        public static Instruction_J AsInstruction_J(this byte[] bytes, InstructionInfo info)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Uma instrução deve conter quatro bytes");

            var bits = bytes
                .Select(x => x.AsBitArray())
                .SelectMany(x => x);

            return new Instruction_J
            {
                Opcode = bits.Take(6).ToArray().AsInt(),
                Address = bits.Skip(6).Take(26).ToArray().AsInt(),
                Info = info
            };
        }
    }
}
