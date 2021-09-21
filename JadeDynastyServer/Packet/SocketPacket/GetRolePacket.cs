using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class GetRolePacket : SocketPacket
    {
        public override byte ID => 0x52;

        public override BasePacket Create()
        {
            return new GetRolePacket();
        }

        public override void Unpack(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            AccountID = reader.ReadFrontUInt32();
            reader.ReadUInt32();
            RoleIndex = reader.ReadFrontInt32();
        }

        public uint AccountID;

        public int RoleIndex;
    }
}
