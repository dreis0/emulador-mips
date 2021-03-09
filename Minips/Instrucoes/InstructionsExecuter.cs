using Minips.Instructions.Conversions;
using Minips.Memory;
using System;
using System.Linq;

namespace Minips.Instructions
{
    public class InstructionsExecuter
    {
        private readonly MinipsMemory _memory;
        private readonly MinipsRegisters _registers;

        public int RCount;
        public int ICount;
        public int JCount;

        private readonly DateTime _startTime;

        public InstructionsExecuter(MinipsMemory memory)
        {
            _memory = memory;
            _registers = new MinipsRegisters();

            RCount = ICount = JCount = 0;
            _startTime = DateTime.Now;
        }

        public int RunInstruction(byte[] bytes, int pc)
        {
            if (bytes.Length != 4) throw new InvalidOperationException();

            var info = InstructionInfo.GetInstructionInfo(bytes);

            switch (info.Format)
            {
                case 'R':
                    return RunInstruction_R(bytes, info, pc);
                case 'I':
                    return RunInstruction_I(bytes, info, pc);
                case 'J':
                    return RunInstruction_J(bytes, info, pc);
                default:
                    throw new InvalidOperationException();
            }
        }

        public void PrintInstructionCount()
        {
            Console.WriteLine($"Instruction count: {RCount + ICount + JCount} (R: {RCount} I: {ICount} J: {JCount})");
        }

        public double GetExecutionTime() => DateTime.Now.Subtract(_startTime).TotalSeconds;

        private int RunInstruction_R(byte[] bytes, InstructionInfo info, int pc)
        {
            RCount++;

            var instruction = bytes.AsInstruction_R(info);

            if (IsSyscall(instruction))
                return RunSyscall(instruction, pc);

            switch (instruction.Info.Type)
            {
                case InstructionType.add:
                    Add(instruction);
                    pc += 4;
                    break;
                case InstructionType.addu:
                    Addu(instruction);
                    pc += 4;
                    break;
                case InstructionType.slt:
                    Slt(instruction);
                    pc += 4;
                    break;
                case InstructionType.jr:
                    pc = Jr(instruction);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return pc;
        }

        private int RunInstruction_I(byte[] bytes, InstructionInfo info, int pc)
        {
            ICount++;

            var instruction = bytes.AsInstruction_I(info);

            switch (instruction.Info.Type)
            {
                case InstructionType.lui:
                    Lui(instruction);
                    pc += 4;
                    break;
                case InstructionType.ori:
                    Ori(instruction);
                    pc += 4;
                    break;
                case InstructionType.addiu:
                    Addiu(instruction);
                    pc += 4;
                    break;
                case InstructionType.addi:
                    Addi(instruction);
                    pc += 4;
                    break;
                case InstructionType.beq:
                    pc = Beq(instruction, pc);
                    break;
                case InstructionType.bne:
                    pc = Bne(instruction, pc);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return pc;
        }

        private int RunInstruction_J(byte[] bytes, InstructionInfo info, int pc)
        {
            JCount++;

            var instruction = bytes.AsInstruction_J(info);

            switch (info.Type)
            {
                case InstructionType.j:
                    pc = J(instruction, pc);
                    break;
                case InstructionType.jal:
                    pc = Jal(instruction, pc);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return pc;
        }

        #region Syscalls

        private bool IsSyscall(Instruction_R instruction) => instruction.Funct == 0xC;
        private int RunSyscall(Instruction_R instruction, int pc)
        {
            int v0 = _registers.Read(2).AsInt();

            switch (v0)
            {
                case 1: //Imprime o inteiro contido em $a0
                    Console.Write(_registers.Read(4).AsInt());
                    break;
                case 4: //Imprime String (terminada por \0) apontada pelo endereço em $a0
                    byte[] a0 = _registers.Read(4);
                    int endereco = a0.AsInt();
                    while (true)
                    {
                        string print = System.Text.Encoding.Latin1.GetString(new byte[] { _memory.ReadOneByte(endereco) });

                        if (print == "\0")
                            break;

                        Console.Write(print);
                        endereco++;
                    }
                    break;
                case 5: //Lê um inteiro e o coloca em $v0
                    var input = Console.ReadKey();
                    var asInt = Convert.ToInt32(char.GetNumericValue(input.KeyChar));
                    var bytes = asInt.AsBytes();
                    _registers.Write(2, bytes);
                    Console.WriteLine();
                    break;
                case 10: //Termina a execução do programa
                    return int.MaxValue;
                case 11: //Imprime o caractere em $a0
                    char[] c = System.Text.Encoding.UTF8.GetChars(_registers.Read(4).Reverse().ToArray());
                    Console.Write(c[0]);
                    break;
            }

            return pc + 4;
        }

        #endregion

        #region R Type

        private void Add(Instruction_R instruction)
        {
            int r1 = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
            int r2 = _registers.Read(instruction.SegundoRegistradorFonte).AsInt();

            _registers.Write(instruction.RegistradorDestino, (r1 + r2).AsBytes());
        }

        private void Addu(Instruction_R instruction)
        {
            int r1 = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
            int r2 = _registers.Read(instruction.SegundoRegistradorFonte).AsInt();

            _registers.Write(instruction.RegistradorDestino, (r1 + r2).AsBytes());
        }

        private void Slt(Instruction_R instruction)
        {
            int r1 = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
            int r2 = _registers.Read(instruction.SegundoRegistradorFonte).AsInt();

            if (r1 < r2)
                _registers.Write(instruction.RegistradorDestino, 1.AsBytes());
            else
                _registers.Write(instruction.RegistradorDestino, 0.AsBytes());
        }

        private int Jr(Instruction_R instruction)
        {
            return _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
        }

        #endregion

        #region I Type

        private void Lui(Instruction_I instruction)
        {
            int toLoad = instruction.Immediate << 16;
            var bytes = toLoad.AsBytes();

            _registers.Write(instruction.SegundoRegistradorFonte, bytes);
        }

        private void Ori(Instruction_I instruction)
        {
            var register = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();

            var result = register | instruction.Immediate;
            var bytes = result.AsBytes();
            
            _registers.Write(instruction.SegundoRegistradorFonte, bytes);
        }

        private void Addiu(Instruction_I instruction)
        {
            int result = instruction.PrimeiroRegistradorFonte + instruction.Immediate;
            _registers.Write(instruction.SegundoRegistradorFonte, result.AsBytes());
        }

        private void Addi(Instruction_I instruction)
        {
            int registerValue = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
            int imm = instruction.Immediate;

            var result = (registerValue + imm).AsBytes().Take(4).ToArray();

            _registers.Write(instruction.SegundoRegistradorFonte, result);
        }

        private int Beq(Instruction_I instruction, int pc)
        {
            int r1 = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
            int r2 = _registers.Read(instruction.SegundoRegistradorFonte).AsInt();

            if (r1 == r2)
                return pc + 4 + instruction.Immediate * 4;

            return pc + 4;
        }

        private int Bne(Instruction_I instruction, int pc)
        {
            int r1 = _registers.Read(instruction.PrimeiroRegistradorFonte).AsInt();
            int r2 = _registers.Read(instruction.SegundoRegistradorFonte).AsInt();

            if (r1 != r2)
                return pc + 4 + instruction.Immediate * 4;

            return pc + 4;
        }

        #endregion

        #region J Type

        private int J(Instruction_J instruction, int pc)
        {
            return (int)(pc & 0xf0000000) | instruction.Address << 2;
        }

        private int Jal(Instruction_J instruction, int pc)
        {
            _registers.Write(31, (pc + 4).AsBytes());
            return J(instruction, pc);
        }

        #endregion
    }
}
