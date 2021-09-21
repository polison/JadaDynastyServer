using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Packet
{
    interface BasePacket
    {
        BasePacket Create();

        byte[] Pack();

        void Unpack(byte[] data);
    }
}
