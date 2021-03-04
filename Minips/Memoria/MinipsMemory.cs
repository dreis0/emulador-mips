using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Minips.Memory
{
    public class MinipsMemory
    {
        /*
            Os endereços são múltiplos de 4 0x00000000 -> 0x00000004 -> 0x00000008
            Endereço inicial 0x00400000
         
         */
        private readonly Hashtable _hashTable;

        public MinipsMemory()
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
