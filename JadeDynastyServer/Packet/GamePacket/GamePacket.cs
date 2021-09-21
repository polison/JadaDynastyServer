using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    abstract class GamePacket : BasePacket
    {
        public abstract ushort ID { get; }

        public abstract BasePacket Create();

        public virtual byte[] Pack()
        {
            throw new NotImplementedException();
        }

        public virtual void Unpack(byte[] data)
        {
            throw new NotImplementedException();
        }
    }

    abstract class GameServerPacket : GamePacket
    {
        public sealed override void Unpack(byte[] data)
        {
            base.Unpack(data);
        }

        public GameServerPacket()
        {
            GamePacketManager.Instance.RegisterServerPacket(this);
        }
    }

    abstract class GameClientPacket : GamePacket
    {
        public sealed override byte[] Pack()
        {
            return base.Pack();
        }

        public GameClientPacket()
        {
            GamePacketManager.Instance.RegisterClientPacket(this);
        }
    }
}
