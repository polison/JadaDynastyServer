using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class AccountInfoPacket : SocketPacket
    {
        public override byte ID => 0x04;

        public override BasePacket Create()
        {
            return new AccountInfoPacket();
        }

        public override byte[] Pack()
        {
            ByteBuffer packer = new ByteBuffer();
            packer.WriteFront(AccountId);
            packer.WriteFront(SessionId);            
            packer.WriteFront(Unknown1);
            packer.WriteFront(ServerId);
            packer.WriteFront(Unknown2);
            packer.WriteFront(Unknown3);
            packer.WriteFront(Unknown4);
            packer.WriteHead(ID);
            return packer.GetBytes();
        }

        public uint AccountId;

        public int ServerId;

        public int SessionId;

        public uint Unknown1 = 1;
        public uint Unknown2 = 2;
        public uint Unknown3 = 3;
        public uint Unknown4 = 4;
    }
}
