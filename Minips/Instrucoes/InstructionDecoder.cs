using System;
using Minips.Instructions.Conversions;

namespace Minips.Instructions
{
    public static class InstructionDecoder
    {
        public static void PrintInstruction(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException("Instrucao inválida");

            InstructionInfo i = InstructionInfo.GetInstructionInfo(bytes);

            switch (i.Format)
            {
                case 'R':
                    PrintInstruction_R(bytes, i);
                    break;
                case 'I':
                    PrintInstruction_I(bytes, i);
                    break;
                case 'J':
                    PrintInstruction_J(bytes, i);
                    break;
            }
        }

        private static void PrintInstruction_R(byte[] bytes, InstructionInfo info)
        {
            var instruction = bytes.AsInstruction_R(info);

            if (info.Funct == 0xC)
            {
                Console.WriteLine("syscall");
            }
            else
            {
                Console.WriteLine($"{info.Mnemonic} ${GetRegisterAlias(instruction.RegistradorDestino)}, ${GetRegisterAlias(instruction.PrimeiroRegistradorFonte)}, ${GetRegisterAlias(instruction.SegundoRegistradorFonte)}");
            }
        }

        private static void PrintInstruction_I(byte[] bytes, InstructionInfo info)
        {
            var instruction = bytes.AsInstruction_I(info);
            Console.WriteLine($"{info.Mnemonic}, ${GetRegisterAlias(instruction.SegundoRegistradorFonte)}, ${GetRegisterAlias(instruction.PrimeiroRegistradorFonte)}, {instruction.Immediate}");
        }

        private static void PrintInstruction_J(byte[] bytes, InstructionInfo info)
        {
            var instruction = bytes.AsInstruction_J(info);
            Console.WriteLine($"{info.Mnemonic}, 0x{instruction.Address.ToString("X")}");
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
