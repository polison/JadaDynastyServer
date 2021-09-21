using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    class HeadInfoPacket : GameServerPacket
    {
        public override ushort ID => 0x26;

        public override BasePacket Create()
        {
            return new HeadInfoPacket();
        }

        public override byte[] Pack()
        {
            return base.Pack();
        }

        public ushort DengJi =0;

        public ushort YuanShenDengJi = 0;

        public byte ZhanDouZhuangTai = 0;

        public byte Unknown = 0;

        public uint DangQianQiXueZhi = 0;

        public uint QiXueZhiShangeXian = 0;

        public uint DangQianMoFaZhi = 0;

        public uint MoFaZhiShangXian = 0;

        public uint DangQianYuanLiZhi = 0;

        public uint YuanLiZhiShangXian = 0;

        public uint DangQianFaBaoJingLiZhi = 0;

        public double JingYanZhi = 0;

        public double YuanShenJingYanZhi = 0;

        public uint NuQiZhi = 0;//英招专用？
    }
}
