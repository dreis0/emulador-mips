using Minips.Instructions;
using System;
using System.Reflection;
using System.Linq;
using Minips.Instrucoes.Annotations;

namespace Minips.Instrucoes
{
    public static class InstructionDecoder
    {
        public static void PrintInstruction(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Instrucao inválida");

            int opCode = bytes.GetOpCode();

            var instructions = Enum.GetValues<InstructionType>()
                .Select(x =>
                {
                    var instructionType = (typeof(InstructionType)).GetMember(x.ToString()).Single();

                    int opCode = instructionType.GetCustomAttribute<Opcode>().Code;
                    char format = instructionType.GetCustomAttribute<Format>().InstructionFormat;

                    return new { opCode, format };
                })
                .Where(x => x.opCode == opCode);

            var i = instructions.FirstOrDefault();

            switch (i.format)
            {
                case 'R':
                    PrintInstruction_R(bytes);
                    break;
                case 'I':
                    PrintInstruction_I(bytes);
                    break;
                case 'J':
                    PrintInstruction_J(bytes);
                    break;
                default:
                    Console.WriteLine("syscall");
                    break;
            }
        }

        private static void PrintInstruction_R(byte[] bytes)
        {
            var instruction = bytes.AsInstruction_R();
            var instructionGlossary = Enum.GetValues<InstructionType>()
                .Select(x =>
                {
                    string mnemonic = x.ToString();

                    var instructionType = (typeof(InstructionType)).GetMember(x.ToString()).Single();

                    int opCode = instructionType.GetCustomAttribute<Opcode>().Code;
                    char format = instructionType.GetCustomAttribute<Format>().InstructionFormat;
                    int? funct = instructionType.GetCustomAttribute<Funct>()?.FunctionCode;

                    return new { opCode, mnemonic, format, funct };
                });

            var i = instructionGlossary
                .Where(x => x.opCode == instruction.Opcode && ((x.funct == instruction.Funct)) || (!x.funct.HasValue && instruction.Funct == 0))
                .SingleOrDefault();

            if (i.funct == 0xC)
            {
                Console.WriteLine("syscall");
            }
            else
            {
                Console.WriteLine($"{i.mnemonic} ${GetRegisterAlias(instruction.RegistradorDestino)}, ${GetRegisterAlias(instruction.PrimeiroRegistradorFonte)}, ${GetRegisterAlias(instruction.SegundoRegistradorFonte)}");
            }
        }

        private static void PrintInstruction_I(byte[] bytes)
        {
            var instruction = bytes.AsInstruction_I();
            var instructionGlossary = Enum.GetValues<InstructionType>()
                .Select(x =>
                {
                    string mnemonic = x.ToString();

                    var instructionType = (typeof(InstructionType)).GetMember(x.ToString()).Single();

                    int opCode = instructionType.GetCustomAttribute<Opcode>().Code;
                    char format = instructionType.GetCustomAttribute<Format>().InstructionFormat;
                    int? funct = instructionType.GetCustomAttribute<Funct>()?.FunctionCode;

                    return new { opCode, mnemonic, format, funct };
                });

            var i = instructionGlossary
                .Where(x => x.opCode == instruction.Opcode)
                .SingleOrDefault();


            Console.WriteLine($"{i.mnemonic}, ${GetRegisterAlias(instruction.SegundoRegistradorFonte)}, ${GetRegisterAlias(instruction.PrimeiroRegistradorFonte)}, {instruction.Immediate}");
        }

        private static void PrintInstruction_J(byte[] bytes)
        {
            var instruction = bytes.AsInstruction_J();
            var instructionGlossary = Enum.GetValues<InstructionType>()
                .Select(x =>
                {
                    string mnemonic = x.ToString();

                    var instructionType = (typeof(InstructionType)).GetMember(x.ToString()).Single();

                    int opCode = instructionType.GetCustomAttribute<Opcode>().Code;
                    char format = instructionType.GetCustomAttribute<Format>().InstructionFormat;
                    int? funct = instructionType.GetCustomAttribute<Funct>()?.FunctionCode;

                    return new { opCode, mnemonic, format, funct };
                });

            var i = instructionGlossary
                .Where(x => x.opCode == instruction.Opcode)
                .SingleOrDefault();


            Console.WriteLine($"{i.mnemonic}, ${GetRegisterAlias(instruction.Address)}");
        }

        private static string GetRegisterAlias(int register)
        {
            if (register == 0)
                return "zero";

            if (register == 1)
                return "at";

            if (register <= 3)
                return $"v{register - 2}";

            if (register <= 7)
                return $"a{register - 4}";

            if (register <= 15)
                return $"t{register - 8}";

            if (register <= 23)
                return $"s{register - 16}";

            if (register <= 25)
                return $"t{register - 16}";

            if (register <= 27)
                return $"k{register - 26}";

            if (register == 28)
                return "gp";

            if (register == 29)
                return "sp";

            if (register == 30)
                return "fp";

            if (register == 31)
                return "ra";

            throw new InvalidOperationException("Número de registrador inválido");
        }
    }
}
