using JadeDynastyServer.Network;
using JadeDynastyServer.Packet;
using JadeDynastyServer.Tools;

namespace JadeDynastyServer
{
    public class JadeDynastyServer
    {
        private ILog Log;
        private SocketManager socketManager;

        public JadeDynastyServer(ILog log)
        {
            Log = log;
        }

        private void InitializePackets()
        {
            //new LinkPacket(); //-01
            //new AuthPacket(); //-02
            new LoginPacket(); //-03
            //new AccountInfoPacket(); //-04
            new GetRolePacket(); //0x52
            new CreateRolePacket();// 0x54


            new PingPacket(); //-0x5A            
        }

        public bool StartServer()
        {
            InitializePackets();
            socketManager = new SocketManager(Log);
            return socketManager.StartService();
        }

        public void StopServer()
        {
            if (socketManager != null)
                socketManager.StopService();

            socketManager = null;
        }
    }
}
