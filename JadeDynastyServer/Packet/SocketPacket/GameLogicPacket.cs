using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JadeDynastyServer.Tools;

namespace JadeDynastyServer.Packet
{
    class GameLogicPacket : SocketPacket
    {
        public override byte ID => 0x00;

        public override BasePacket Create()
        {
            return new GameLogicPacket();
        }

        public override byte[] Pack()
        {
            ByteBuffer buffer = new ByteBuffer();
            foreach(var p in packets)
                buffer.Write(p.Pack());

            buffer.WriteHead(ID);
            return buffer.GetBytes();
        }

        private List<GameServerPacket> packets = new List<GameServerPacket>();
        public void AddPacket(GameServerPacket packet)
        {
            packets.Add(packet);
        }
    }
}
