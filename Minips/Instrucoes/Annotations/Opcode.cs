using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minips.Instrucoes.Annotations
{
    public class Opcode : Attribute
    {
        public int Code { get; set; }

        public Opcode(int opcode)
        {
            Code = opcode;
        }
    }
}
