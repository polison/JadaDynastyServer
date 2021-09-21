using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    abstract class SocketPacket : BasePacket
    {
        public abstract byte ID { get; }

        public abstract BasePacket Create();

        public virtual byte[] Pack()
        {
            throw new NotImplementedException();
        }

        public virtual void Unpack(byte[] data)
        {
            throw new NotImplementedException();
        }

        public SocketPacket()
        {
            SocketPacketManager.Instance.RegisterPacket(this);
        }
    }
}
