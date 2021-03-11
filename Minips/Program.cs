using Minips.Instructions;
using Minips.Memory;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace minips
{
    class Program
    {
        const int TEXT_SECTION_START = 0x00400000;
        const int DATA_SECTION_START = 0x10010000;

        private static MinipsMemory _memory;

#if DEBUG

        static void Main(string[] args)
        {
            _memory = new MinipsMemory();

            string entrada = "08.sort";

            string dataFile = $"C:\\Users\\Miguel dos Reis\\OneDrive\\UFABC\\Arquitetura de Computadores\\Projeto\\Entradas\\{entrada}.data";
            string textFile = $"C:\\Users\\Miguel dos Reis\\OneDrive\\UFABC\\Arquitetura de Computadores\\Projeto\\Entradas\\{entrada}.text";

            CarregarInstrucoes(textFile);
            CarregarDados(dataFile);

            Decode();
            Console.WriteLine();
            Execute();
        }
#else
        static void Main(string[] args)
        {
            string tipoExecucao = args[0];
            string file = args[1];

            _memory = new MinipsMemory();

            string textFile = $"./{file}.text";
            string dataFile = $"./{file}.data";

            if (!File.Exists(textFile))
            {
                Console.WriteLine($"O arquivo {textFile} não foi encontrado");
                return;
            }

            if (!File.Exists(dataFile))
            {
                Console.WriteLine($"O arquivo {dataFile} não foi encontrado");
                return;
            }

            CarregarInstrucoes(textFile);
            CarregarDados(dataFile);

            if (tipoExecucao == "decode")
                Decode();
            else if (tipoExecucao == "run")
                Execute();
            else
                Console.WriteLine("Use: minips run arquivo / minips decode arquivo");
        }
#endif

        static void CarregarInstrucoes(string fileName)
        {
            using (var binaryReader = new BinaryReader(File.OpenRead(fileName)))
            {
                byte[] bytes = ReadBytesInOrder(binaryReader, 4);
                int address = TEXT_SECTION_START;

                while (bytes.Any())
                {
                    _memory.Write(address, bytes);
                    bytes = ReadBytesInOrder(binaryReader, 4);
                    address += 4;
                }
            }
        }

        static void CarregarDados(string fileName)
        {
            using (var binaryReader = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                byte[] bytes = ReadBytesInOrder(binaryReader, 1);
                int address = DATA_SECTION_START;

                while (bytes.Any())
                {
                    _memory.Write(address, bytes[0]);
                    bytes = ReadBytesInOrder(binaryReader, 1);
                    address++;
                }
            }
        }

        static void Decode()
        {
            int address = TEXT_SECTION_START;

            while (_memory.Read(address).Any(x => x != 0))
            {
                byte[] bytes = _memory.Read(address);
                InstructionDecoder.PrintInstruction(bytes);
                address += 4;
            }
        }

        static void Execute()
        {
            var executer = new InstructionsExecuter(_memory);
            int address = TEXT_SECTION_START;

            while (_memory.Read(address).Any(x => x != 0))
            {
                byte[] bytes = _memory.Read(address);

                //InstructionDecoder.PrintInstruction(bytes);
                address = executer.RunInstruction(bytes, address);
            }

            Console.WriteLine("Execution finished successfully");
            Console.WriteLine("------------------------------------");

            Console.WriteLine($"Instruction count: {executer.RCount + executer.ICount + executer.JCount} (R: {executer.RCount} I: {executer.ICount} J: {executer.JCount})");
            Console.WriteLine($"IPS: {executer.GetExecutionTime()/((double)(executer.RCount + executer.ICount + executer.JCount))}s");

        }

        private static byte[] ReadBytesInOrder(BinaryReader binaryReader, int count) =>
            binaryReader.ReadBytes(count)
                .Reverse()
                .ToArray();
    }
}