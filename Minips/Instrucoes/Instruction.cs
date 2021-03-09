using System;
using System.Collections;
using System.Linq;

namespace Minips.Instructions
{
    public abstract class BaseInstruction
    {
        public int Opcode { get; set; }
        public InstructionInfo Info { get; set; }
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
}
