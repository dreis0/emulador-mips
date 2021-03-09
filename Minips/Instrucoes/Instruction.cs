namespace Minips.Instructions
{
    public abstract class BaseInstruction
    {
        public int Opcode { get; set; }
        public InstructionInfo Info { get; set; }
    }

    public class Instruction_R : BaseInstruction
    {
        public int RS { get; set; }
        public int RT { get; set; }
        public int RD { get; set; }
        public int Shamt { get; set; }
        public int Funct { get; set; }
    }

    public class Instruction_I : BaseInstruction
    {
        public int RS { get; set; }
        public int RT { get; set; }
        public int Immediate { get; set; }
    }

    public class Instruction_J : BaseInstruction
    {
        public int Address { get; set; }
    }
}
