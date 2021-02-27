using Minips.Instructions;
using Minips.Memory;
using System;
using System.IO;

namespace minips
{
    class Program
    {
        const int TEXT_SECTION_START = 0x00400000;
        const int DATA_SECTION_START = 0x10010000;

        private static MinipsMemory _memory;
        private static MinipsMemory _registers;

        static void Main(string[] args)
        {
            _memory = new MinipsMemory();
            _registers = new MinipsMemory();

            string textFile = "C:\\Users\\Miguel dos Reis\\OneDrive\\UFABC\\Arquitetura de Computadores\\Projeto\\01.soma.text";
            string dataFile = "C:\\Users\\Miguel dos Reis\\OneDrive\\UFABC\\Arquitetura de Computadores\\Projeto\\01.soma.data";

            // Loads .text
            using (var binaryReader = new BinaryReader(File.Open(textFile, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                byte[] bytes = binaryReader.ReadBytes(4);
                int address = TEXT_SECTION_START;

                while (bytes != null && bytes.Length == 4)
                {
                    _memory.Write(address, bytes);

                    bytes = binaryReader.ReadBytes(4);
                    address += 4;
                }
            }

            // Loads .data
            using (var binaryReader = new BinaryReader(File.Open(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                byte[] bytes = binaryReader.ReadBytes(4); ;
                int address = DATA_SECTION_START;

                while (bytes != null && bytes.Length == 4)
                {
                    _memory.Write(address, bytes);

                    bytes = binaryReader.ReadBytes(4);
                    address += 4;
                } 
            }
        }
    }
}