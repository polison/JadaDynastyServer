using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class LoginPacket : SocketPacket
    {
        public override byte ID => 0x03;

        public override BasePacket Create()
        {
            return new LoginPacket();
        }

        public override void Unpack(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            UserName = reader.ReadASCIIString();
            var hashLength = reader.ReadByte();
            LoginHash = reader.ReadBytes(hashLength);
        }

        public string UserName;

        public byte[] LoginHash;
    }
}
