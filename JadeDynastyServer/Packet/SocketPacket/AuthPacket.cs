using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class AuthPacket : SocketPacket
    {
        public override byte ID => 0x02;

        public override BasePacket Create()
        {
            return new AuthPacket();
        }

        public override byte[] Pack()
        {
            ByteBuffer packer = new ByteBuffer();
            var r = new Random((int)DateTime.Now.Ticks);
            AuthHash = new byte[0x10];
            r.NextBytes(AuthHash);
            packer.Write((byte)AuthHash.Length);
            packer.Write(AuthHash);
            packer.Write(0x00);
            packer.WriteHead(ID);
            return packer.GetBytes();
        }

        public override void Unpack(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            var authlength= reader.ReadByte();
            AuthHash = reader.ReadBytes(authlength);
            ForceLogin = reader.ReadBool();
        }

        public bool ForceLogin = false;

        public byte[] AuthHash;
    }
}
