using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace JadeDynastyServer.Tools
{
    class ByteBuffer
    {
        List<byte> data;

        int wpos, rpos;

        public ByteBuffer()
        {
            data = new List<byte>();
            wpos = 0;
            rpos = 0;
        }

        public byte[] GetBytes()
        {
            return data.ToArray();
        }

        public void Write(SocketAsyncEventArgs args, RC4 encrypt = null)
        {
            var bytes = new byte[args.BytesTransferred];
            Array.Copy(args.Buffer, args.Offset, bytes, 0, bytes.Length);
            if (encrypt != null)
                bytes = encrypt.Encrypt(bytes);
            Write(bytes);
        }

        public void Write(byte[] value, int length)
        {
            data.AddRange(value.Take(length));
            wpos += length;
        }

        public void Write(byte[] value)
        {
            data.AddRange(value);
            wpos += value.Length;
        }

        public void Write(byte value)
        {
            data.Add(value);
            wpos++;
        }

        public void Write(bool value)
        {
            if (value)
                Write((byte)1);
            else
                Write((byte)0);
        }

        public void Write(uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Write(bytes);
        }

        public void Write(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Write(bytes);
        }

        public void Write(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Write(bytes);
        }

        public void Write(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Write(bytes);
        }

        public void Write(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            Write((byte)bytes.Length);
            //Array.Reverse(bytes);
            Write(bytes);
        }

        public void WriteFront(uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes);
        }

        public void WriteFront(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes);
        }

        public void WriteFront(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes);
        }

        public void WriteFront(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes);
        }

        public void WriteFront(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            Write((byte)bytes.Length);
            Array.Reverse(bytes);
            Write(bytes);
        }

        //id 01 00
        public void WriteHead(byte ID)
        {
            WriteLength();
            data.Insert(0, ID);
            wpos++;
        }

        //22 03 02 02 00
        public void WriteHead(ushort ID)
        {
            var bytes = BitConverter.GetBytes(ID);
            data.Insert(0, bytes[1]);
            data.Insert(0, bytes[0]);
            WriteLength();
            WriteLength();
            data.Insert(0, 0x22);
            wpos += 3;
        }

        private void WriteLength()
        {
            var length = data.Count;
            if (length < 0x80)
            {
                data.Insert(0, (byte)length);
                wpos++;
            }
            else
            {
                data.Insert(0, (byte)length);
                data.Insert(0, (byte)((length >> 8 & 0x7F) | 0x80));
                wpos += 2;
            }
        }

        public byte ReadByte()
        {
            return data[rpos++];
        }

        public int ReadPacketLength()
        {
            var result = 0;
            var b = ReadByte();
            if (b < 0x80)
                result = b;
            else
            {
                var b1 = ReadByte();
                result = ((b - 0x80) << 8) + b1;
            }
            return result;
        }

        public ushort ReadUInt16()
        {
            var result = BitConverter.ToUInt16(GetBytes(), rpos);
            rpos += 2;
            return result;
        }

        public uint ReadUInt32()
        {
            var result = BitConverter.ToUInt32(GetBytes(), rpos);
            rpos += 4;
            return result;
        }

        public ulong ReadUInt64()
        {
            var result = BitConverter.ToUInt64(GetBytes(), rpos);
            rpos += 8;
            return result;
        }

        public float ReadSingle()
        {
            var result = BitConverter.ToSingle(GetBytes(), rpos);
            rpos += 4;
            return result;
        }

        public double ReadDouble()
        {
            var result = BitConverter.ToDouble(GetBytes(), rpos);
            rpos += 8;
            return result;
        }

        public byte[] ReadBytes(int length)
        {
            var result = data.GetRange(rpos, length);
            rpos += length;
            return result.ToArray();
        }

        public void ClearReadPacket()
        {
            data.RemoveRange(0, rpos);
            wpos -= rpos;
            rpos = 0;
        }

        public int GetLength()
        {
            return data.Count;
        }
    }
}
