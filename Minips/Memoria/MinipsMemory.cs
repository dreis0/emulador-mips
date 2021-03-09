using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void Write(int address, byte value)
        {
            _hashTable[address] = value;
        }

        public void Write(int address, byte[] value)
        {
            if (value.Length != 4)
                throw new InvalidOperationException("Uma palavra deve conter 32 bits");

            _hashTable[address] = value[0];
            _hashTable[address + 1] = value[1];
            _hashTable[address + 2] = value[2];
            _hashTable[address + 3] = value[3];
        }

        public byte ReadOneByte(int address)
        {
            return (byte)(_hashTable[address] ?? (byte)0);
        }

        public byte[] Read(int address)
        {
            var value = new int[] { address, address + 1, address + 2, address + 3 }
                .Select(x => (byte)(_hashTable[x] ?? (byte)0))
                .ToArray();

            return value ?? new byte[] { 0, 0, 0, 0 };
        }
    }
}
