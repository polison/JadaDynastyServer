using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class LinkPacket : SocketPacket
    {
        public override byte ID => 0x01;

        public override BasePacket Create()
        {
            return new LinkPacket();
        }

        public override byte[] Pack()
        {
            ByteBuffer packer = new ByteBuffer();
            var r = new Random((int)DateTime.Now.Ticks);
            OnlineHash = new byte[0x10];
            r.NextBytes(OnlineHash);
            packer.Write((byte)OnlineHash.Length);
            packer.Write(OnlineHash);
            packer.Write(Version);
            var bytes = Encoding.ASCII.GetBytes(ClientSign);
            packer.Write((byte)bytes.Length);
            packer.Write(bytes);
            packer.Write(Unknown1);
            packer.Write(Unknown2);
            packer.Write(DropBouns);
            packer.Write(Unknown3);
            packer.Write(ExpBouns);
            packer.Write(Unknown4);
            packer.Write(ServerStatus);
            packer.WriteHead(ID);
            return packer.GetBytes();
        }

        public byte[] OnlineHash;

        public byte[] Version = { 0, 4, 8, 0 };

        public string ClientSign = "100000a410000094b515a0d41a6";

        public ushort Unknown1 = 0x00;

        public byte Unknown2 = 0x00;

        public byte DropBouns = 0x00;

        public byte Unknown3 = 0x00;

        public byte ExpBouns = 0x00;

        public ushort Unknown4 = 0x00;

        public byte ServerStatus = 0x00;
    }
}
