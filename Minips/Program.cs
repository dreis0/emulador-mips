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
        private static MinipsMemory _registers;

#if DEBUG

        static void Main(string[] args)
        {
            _memory = new MinipsMemory();
            _registers = new MinipsMemory();

            string textFile = "C:\\Users\\Miguel dos Reis\\OneDrive\\UFABC\\Arquitetura de Computadores\\Projeto\\Entradas\\02.hello.text";
            string dataFile = "C:\\Users\\Miguel dos Reis\\OneDrive\\UFABC\\Arquitetura de Computadores\\Projeto\\Entradas\\02.hello.data";

            CarregarInstrucoes(textFile);
            CarregarDados(dataFile);

            Decode();
        }
#else
            static void Main(string[] args)
        {
            string tipoExecucao = args[0];
            string file = args[1];

            _memory = new MinipsMemory();
            _registers = new MinipsMemory();

            string textFile = $"./{file}.text";
            string dataFile = $"./{file}.data";

            CarregarInstrucoes(textFile);
            CarregarDados(dataFile);
        }
#endif

        static void CarregarInstrucoes(string fileName)
        {
            using (var binaryReader = new BinaryReader(File.OpenRead(fileName)))
            {
                byte[] bytes = ReadBytesInOrder(binaryReader, 4);
                int address = TEXT_SECTION_START;

                while (bytes != null && bytes.Length == 4)
                {
                    _registers.Write(address, bytes);
                    bytes = ReadBytesInOrder(binaryReader, 4);
                    address += 4;
                }
            }
        }

        static void CarregarDados(string fileName)
        {
            using (var binaryReader = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                byte[] bytes = ReadBytesInOrder(binaryReader, 4);
                int address = DATA_SECTION_START;

                while (bytes != null && bytes.Length == 4)
                {
                    _memory.Write(address, bytes);
                    bytes = ReadBytesInOrder(binaryReader, 4);
                    address += 4;
                }
            }
        }

        static void Decode()
        {
            int address = TEXT_SECTION_START;

            while(_registers.Read(address).Any(x => x != 0))
            {
                byte[] bytes = _registers.Read(address);
                InstructionDecoder.PrintInstruction(bytes);
                address += 4;
            }
        }

        private static byte[] ReadBytesInOrder(BinaryReader binaryReader, int count) =>
            binaryReader.ReadBytes(count)
                .Reverse()
                .ToArray();
    }
}