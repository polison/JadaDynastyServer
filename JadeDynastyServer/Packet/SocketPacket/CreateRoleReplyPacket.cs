using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class CreateRoleReplyPacket : SocketPacket
    {
        public override byte ID => 0x55;

        public override BasePacket Create()
        {
            return new CreateRoleReplyPacket();
        }

        public override byte[] Pack()
        {
            var buffer = new Tools.ByteBuffer();
            buffer.WriteFront(ErrorId);
            buffer.WriteFront(RoleId);
            buffer.WriteFront(AccountId);
            buffer.WriteFront(RoleId);
            buffer.Write(RoleSex);
            buffer.Write(RoleFace);
            buffer.Write(RoleHair);
            buffer.Write(RoleEar);
            buffer.Write(RoleTail);
            buffer.Write(RoleClass);
            buffer.WriteFront(RoleLevel);
            buffer.Write(RoleName);
            buffer.Write(EquipCount);
            buffer.Write(RoleStatus);
            buffer.WriteFront(dt);
            buffer.WriteFront(ct);
            buffer.WriteFront(lt);
            buffer.Write(posx);
            buffer.Write(posh);
            buffer.Write(posy);
            buffer.WriteFront(mapid);
            buffer.Write(emptyByte);
            buffer.Write(FashionMode);
            buffer.Write(emptyUint);
            buffer.Write(BaseMode);
            buffer.Write(emptyByte);
            buffer.Write(RoleTeam);
            buffer.WriteFront(FashionHead);
            buffer.Write(emptyUint);
            buffer.Write(emptyUint);
            buffer.Write(emptyUint);
            buffer.WriteFront(FashionCloth);
            buffer.Write(emptyUint);
            buffer.Write(emptyUint);
            buffer.Write(emptyUint);
            buffer.WriteFront(FashionShoes);
            buffer.Write(emptyUint);
            buffer.Write(emptyUint);
            buffer.WriteFront(FashionWeapon);
            buffer.WriteFront(BindRoleID);
            buffer.Write(emptyUint);
            buffer.WriteHead(ID);
            return buffer.GetBytes();
        }

        public uint AccountId;

        public uint RoleId;

        public int RoleIndex;

        public byte RoleSex, RoleFace, RoleHair, RoleEar, RoleTail, RoleClass;

        public uint RoleLevel;

        public string RoleName;

        public byte EquipCount = 0;

        public byte RoleStatus = 1;

        public byte BaseMode;

        public byte RoleTeam;

        public byte FashionMode = 0;

        public uint FashionHead, FashionCloth, FashionShoes, FashionWeapon;

        public uint BindRoleID;

        public uint ErrorId = 0;

        public float posx, posh, posy;
        public uint mapid;
        public uint dt, ct, lt;
        public byte emptyByte = 0;
        public uint emptyUint = 0;
    }
}
