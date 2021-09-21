using JadeDynastyServer.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Tools
{
    class GamePacketManager
    {
        #region Instance
        private static GamePacketManager manager;

		public static GamePacketManager Instance
		{
			get
			{
				if (manager == null)
				{
					manager = new GamePacketManager();
				}
				return manager;
			}
		}
		#endregion

		private Dictionary<ushort, GamePacket> GameServerPackets = new Dictionary<ushort, GamePacket>();
		private Dictionary<ushort, GamePacket> GameClientPackets = new Dictionary<ushort, GamePacket>();

		public GamePacket FindServerPacket(ushort id)
		{
			if (GameServerPackets.ContainsKey(id))
			{
				return GameServerPackets[id].Create() as GamePacket;
			}
			return null;
		}

		public void RegisterServerPacket(GamePacket p)
		{
			if (!GameServerPackets.ContainsKey(p.ID))
			{
				GameServerPackets.Add(p.ID, p);
			}
		}

		public GamePacket FindClientPacket(ushort id)
		{
			if (GameClientPackets.ContainsKey(id))
			{
				return GameClientPackets[id].Create() as GamePacket;
			}
			return null;
		}

		public void RegisterClientPacket(GamePacket p)
		{
			if (!GameClientPackets.ContainsKey(p.ID))
			{
				GameClientPackets.Add(p.ID, p);
			}
		}
	}
}
