using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JadeDynastyServer.Network
{
    class SocketManager
    {
        public ILog Log;
        public ServerSettings Settings;

        private Socket linkSocket;
        private readonly int maxConnectNum = 2000;
        private readonly int maxConnectOneTime = 10;

        private Semaphore maxClient;
        private Dictionary<int, WorldSocket> worldSockets;

        public SocketManager(ILog log)
        {
            Log = log;

            Settings = ServerSettings.LoadSettings(Log);
        }

        public bool StartService()
        {
            try
            {
                worldSockets = new Dictionary<int, WorldSocket>();
                maxClient = new Semaphore(maxConnectNum, maxConnectNum);

                IPEndPoint local = new IPEndPoint(IPAddress.Any, Settings.ServerPort);
                linkSocket = new Socket(local.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                linkSocket.Bind(local);
                linkSocket.Listen(maxConnectOneTime);
            }
            catch (Exception e)
            {
                Log.Error("服务器启动失败，{0}", e.Message);
                return false;
            }

            Log.Message("服务器成功启动！");
            DoAccept(null);
            return true;
        }

        private void DoAccept(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += ProcessAccept;
            }
            else
                args.AcceptSocket = null;

            maxClient.WaitOne();
            if (linkSocket != null)
            {
                if (linkSocket.AcceptAsync(args))
                    ProcessAccept(null, args);
            }
        }

        private void ProcessAccept(object sender, SocketAsyncEventArgs eventArgs)
        {
            var s = eventArgs.AcceptSocket;
            if (s.Connected)
            {
                var socket = new WorldSocket(s, this);
                lock (worldSockets)
                {
                    worldSockets.Add(socket.ID, socket);
                }
                socket.Open();
                DoAccept(eventArgs); //把当前异步事件释放，等待下次连接
            }
        }

        public void StopService()
        {
            if (linkSocket != null)
            {
                linkSocket.Close();
                linkSocket = null;
            }

            if (worldSockets != null)
            {
                while (worldSockets.Count > 0)
                    worldSockets.Values.First().Close();

                worldSockets.Clear();
                worldSockets = null;
            }

            if (maxClient != null)
            {
                maxClient.Close();
                maxClient = null;
            }

            Log.Message("服务器关闭！");
        }

        public void CloseSocket(WorldSocket socket)
        {
            lock (worldSockets)
            {
                worldSockets.Remove(socket.ID);
                maxClient.Release();
            }
        }
    }
}
