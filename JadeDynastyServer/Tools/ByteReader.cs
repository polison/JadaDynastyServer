using System;
using System.Net;
using System.Text;

namespace JadeDynastyServer.Tools
{
    class ByteReader
    {
        byte[] data;

        private int offset;

        public ByteReader(byte[] packetData)
        {
            data = packetData;
            offset = 0;
        }

        public byte[] ReadBytes(int length)
        {
            var array = new byte[length];
            Array.Copy(data, offset, array, 0, length);
            offset += length;
            return array;
        }

        public byte ReadByte()
        {
            var result = data[offset];
            offset++;
            return result;
        }

        public bool ReadBool()
        {
            var result = data[offset];
            offset++;
            return result == 1;
        }

        public ushort ReadUInt16()
        {
            var result = BitConverter.ToUInt16(data, offset);
            offset += 2;
            return result;
        }

        public uint ReadUInt32()
        {
            var result = BitConverter.ToUInt32(data, offset);
            offset += 4;
            return result;
        }

        public int ReadInt32()
        {
            var result = BitConverter.ToInt32(data, offset);
            offset += 4;
            return result;
        }

        public float ReadSingle()
        {
            var result = BitConverter.ToSingle(data, offset);
            offset += 4;
            return result;
        }

        public double ReadDouble()
        {
            var result = BitConverter.ToDouble(data, offset);
            offset += 8;
            return result;
        }

        public ushort ReadFrontUInt16()
        {
            var bytes = ReadBytes(2);
            Array.Reverse(bytes);
            var result = BitConverter.ToUInt16(bytes, 0);
            return result;
        }

        public uint ReadFrontUInt32()
        {
            var bytes = ReadBytes(4);
            Array.Reverse(bytes);
            var result = BitConverter.ToUInt16(bytes, 0);
            return result;
        }

        public int ReadFrontInt32()
        {
            var bytes = ReadBytes(4);
            Array.Reverse(bytes);
            var result = BitConverter.ToUInt16(bytes, 0);
            return result;
        }

        public float ReadFrontSingle()
        {
            var bytes = ReadBytes(4);
            Array.Reverse(bytes);
            var result = BitConverter.ToUInt16(bytes, 0);
            return result;
        }

        public double ReadFrontDouble()
        {
            var bytes = ReadBytes(8);
            Array.Reverse(bytes);
            var result = BitConverter.ToUInt16(bytes, 0);
            return result;
        }

        public string ReadASCIIString()
        {
            var length = ReadByte();
            var result = Encoding.ASCII.GetString(data, offset, length);
            offset += length;
            return result;
        }

        public string ReadUnicodeString()
        {
            var length = ReadByte();
            var result = Encoding.Unicode.GetString(data, offset, length);
            offset += length;
            return result;
        }
    }
}
