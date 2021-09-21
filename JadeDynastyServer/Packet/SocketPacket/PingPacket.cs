using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class PingPacket : SocketPacket
    {
        public override byte ID => 0X5A;

        public override BasePacket Create()
        {
            return new PingPacket();
        }

        public override void Unpack(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            Alive = reader.ReadByte();
        }

        public override byte[] Pack()
        {
            ByteBuffer packer = new ByteBuffer();
            packer.Write(Alive);
            packer.WriteHead(ID);
            return packer.GetBytes();
        }

        public byte Alive = 0x5A;
    }
}
