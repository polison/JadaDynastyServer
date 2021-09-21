using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class LinePacket : SocketPacket
    {
        public override byte ID => 0x25;

        public override BasePacket Create()
        {
            return new LinePacket();
        }

        public override byte[] Pack()
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write(LineCount);
            buffer.Write(LineID);
            buffer.Write(LineName);
            buffer.Write(Unknown1);
            buffer.Write(LinePlayer);
            buffer.Write(Unknown2);
            buffer.Write(Unknown3);
            buffer.Write(Unknown4);
            buffer.Write(Unknown5);
            buffer.Write(Unknown6);
            buffer.WriteHead(ID);
            return buffer.GetBytes();
        }

        public uint LineCount = 1;

        public byte LineID = 1;

        public string LineName;

        public byte Unknown1 = 0x08;

        public ushort LinePlayer = 0;

        public ushort Unknown2 = 0;

        public uint Unknown3 = 0;

        public ushort Unknown4 = 0;

        public ushort Unknown5 = 0;

        public byte Unknown6 = 0;
    }
}
