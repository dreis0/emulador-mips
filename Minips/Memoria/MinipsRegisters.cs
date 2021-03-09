using System;
using System.Collections;

namespace Minips.Memory
{
    public class MinipsRegisters
    {
        private readonly Hashtable _hashTable;

        public MinipsRegisters()
        {
            _hashTable = new Hashtable();
        }

        public void Write(int address, byte[] value)
        {
            if (value.Length != 4)
                throw new InvalidOperationException("Uma palavra deve conter 32 bits");

            _hashTable[address] = value;
        }

        public byte[] Read(int address)
        {
            var value = _hashTable[address] as byte[];
            return value ?? new byte[] { 0, 0, 0, 0 };
        }
    }
}
