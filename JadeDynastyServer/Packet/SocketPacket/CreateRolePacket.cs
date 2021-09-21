using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class CreateRolePacket : SocketPacket
    {
        public override byte ID => 0x54;

        public override BasePacket Create()
        {
            return new CreateRolePacket();
        }

        public override void Unpack(byte[] data)
        {
            var reader = new Tools.ByteReader(data);
            AccountId = reader.ReadFrontUInt32();
            RoleIndex = reader.ReadFrontInt32();
            reader.ReadBytes(4);
            RoleSex = reader.ReadByte();
            RoleFace = reader.ReadByte();
            RoleHair = reader.ReadByte();
            RoleEar = reader.ReadByte();
            RoleTail = reader.ReadByte();
            RoleClass = reader.ReadByte();
            RoleLevel = reader.ReadFrontUInt32();
            RoleName = reader.ReadUnicodeString();
            reader.ReadBytes(36);
            BaseMode = reader.ReadByte();
            reader.ReadByte();
            RoleTeam = reader.ReadByte();
            FashionHead = reader.ReadFrontUInt32();
            reader.ReadBytes(12);
            FashionCloth = reader.ReadFrontUInt32();
            reader.ReadBytes(12);
            FashionShoes = reader.ReadFrontUInt32();
            reader.ReadBytes(8);
            FashionWeapon = reader.ReadFrontUInt32();
            BindRoleID_Long = reader.ReadUnicodeString();
        }

        public uint AccountId;

        public int RoleIndex;

        public byte RoleSex, RoleFace, RoleHair, RoleEar, RoleTail, RoleClass;

        public uint RoleLevel;

        public string RoleName;

        public byte BaseMode;

        public byte RoleTeam;

        public uint FashionHead, FashionCloth, FashionShoes, FashionWeapon;

        public string BindRoleID_Long;
    }
}
