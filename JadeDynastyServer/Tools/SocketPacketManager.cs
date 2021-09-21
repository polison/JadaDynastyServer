using JadeDynastyServer.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Tools
{
    class SocketPacketManager
    {
		#region Instance
		private static SocketPacketManager manager;

		public static SocketPacketManager Instance
		{
			get
			{
				if (manager == null)
				{
					manager = new SocketPacketManager();
				}
				return manager;
			}
		}
		#endregion

		private Dictionary<byte, SocketPacket> SocketPackets = new Dictionary<byte, SocketPacket>();

		public SocketPacket FindPacket(byte id)
		{
			if (SocketPackets.ContainsKey(id))
			{
				return SocketPackets[id].Create() as SocketPacket;
			}
			return null;
		}

		public void RegisterPacket(SocketPacket p)
		{
			if (!SocketPackets.ContainsKey(p.ID))
			{
				SocketPackets.Add(p.ID, p);
			}
		}
	}
}
