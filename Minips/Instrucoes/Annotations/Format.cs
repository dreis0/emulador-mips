using System;
using System.Linq;

namespace Minips.Instrucoes.Annotations
{
    public class Format : Attribute
    {
        public char InstructionFormat { get; set; }

        public Format(char instructionFormat)
        {
            var valid = new char[] { 'R', 'I', 'J', };

            if (!valid.Contains(instructionFormat))
                throw new InvalidOperationException($"{instructionFormat} não é um formato válido");

            InstructionFormat = instructionFormat;
        }
    }

}
