using Minips.Instructions.Annotations;
using Minips.Instructions.Conversions;
using System;
using System.Linq;
using System.Reflection;

namespace Minips.Instructions
{
    public class InstructionInfo
    {
        public char Format { get; set; }

        public int OpCode { get; set; }

        public int? Funct { get; set; }

        public string Mnemonic { get; set; }

        public InstructionType Type { get; set; }

        public static InstructionInfo GetInstructionInfo(byte[] bytes)
        {
            int opCode = bytes.GetOpCode();
            int funct = bytes.GetFunct();

            var instructions = Enum.GetValues<InstructionType>()
                .Select(x =>
                {
                    string mnemonic = x.ToString();

                    var instructionType = (typeof(InstructionType)).GetMember(x.ToString()).Single();

                    int opCode = instructionType.GetCustomAttribute<Opcode>().Code;
                    char format = instructionType.GetCustomAttribute<Format>().InstructionFormat;
                    int? funct = instructionType.GetCustomAttribute<Funct>()?.FunctionCode;

                    return new InstructionInfo { Format = format, OpCode = opCode, Funct = funct, Mnemonic = mnemonic, Type = x };
                });

            var sameOpCode = instructions
                .Where(x => x.OpCode == opCode);

            return sameOpCode.SingleOrDefault(x => !x.Funct.HasValue || x.Funct == funct);
        }
    }
}
