using System;
using System.Collections.Generic;
using System.Text;

namespace Minips.Instrucoes.Annotations
{
    public class Funct : Attribute
    {
        public int FunctionCode { get; set; }

        public Funct(int functionCode)
        {
            FunctionCode = functionCode;
        }
    }
}
