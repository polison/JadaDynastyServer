using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class GetRoleReplyPacket : SocketPacket
    {
        public override byte ID => 0x53;

        public override BasePacket Create()
        {
            return new GetRoleReplyPacket();
        }

        public override byte[] Pack()
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteFront(Unknown1);
            buffer.WriteFront(RoleIndex);
            buffer.WriteFront(AccountID);
            buffer.WriteFront(Unknown2);
            buffer.Write(HasRoleData);
            buffer.WriteFront(BindRoleID);
            buffer.WriteHead(ID);
            return buffer.GetBytes();
        }

        public uint Unknown1 = 0;

        public int RoleIndex;

        public uint AccountID;

        public uint Unknown2 = 0;

        public bool HasRoleData;

        public uint BindRoleID;
    }
}
