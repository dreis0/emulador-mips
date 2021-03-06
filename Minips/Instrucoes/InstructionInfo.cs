namespace Minips.Instructions
{
    public class InstructionInfo
    {
        public char Format { get; set; }

        public int OpCode { get; set; }

        public int? Funct { get; set; }

        public string Mnemonic { get; set; }

        public InstructionType Type { get; set; }
    }
}
