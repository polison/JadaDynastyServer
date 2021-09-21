using JadeDynastyServer.Packet;
using JadeDynastyServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace JadeDynastyServer.Network
{
    class WorldSocket
    {
        private int id;
        public int ID => id;

        private SocketManager socketManager;
        private Socket connSocket;
        private SocketAsyncEventArgs readArgs, writeArgs;
        private byte[] readData;
        private ByteBuffer readBytes;

        private RC4 s2cEncrypt, c2sEncrypt;
        private MPPC compressor;
        private bool needCompress;
        private bool needEncrypt;
        private LoginPacket loginPacket;
        private LinkPacket linkPacket;
        private bool isSending = false;

        private List<BasePacket> packets;

        public WorldSocket(Socket socket, SocketManager manager)
        {
            connSocket = socket;
            socketManager = manager;

            id = socket.Handle.ToInt32();

            compressor = new MPPC();
            needCompress = false;
            needEncrypt = false;

            packets = new List<BasePacket>();
        }

        #region socket

        private void IO_Completed(object sender, SocketAsyncEventArgs args)
        {
            WorldSocket client = args.UserToken as WorldSocket;
            lock (client)
            {
                switch (args.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        client.ProcessRead();
                        break;
                    case SocketAsyncOperation.Send:
                        client.ProcessSend();
                        break;
                    default:
                        client.Close();
                        break;
                }
            }
        }

        private void ProcessRead()
        {
            if (readArgs.BytesTransferred > 0 && readArgs.SocketError == SocketError.Success)
            {
                ReadPacket();
                if (!connSocket.ReceiveAsync(readArgs))
                    ProcessRead();
            }
            else
                Close();
        }

        private void ProcessSend()
        {
            if (writeArgs.SocketError == SocketError.Success)
            {
                isSending = false;
                if (packets.Count > 0)
                    SendPacket();
                return;
            }

            Close();
        }

        public void Open()
        {
            socketManager.Log.Message("客户端[{1}]{0}连接……", connSocket.RemoteEndPoint, id);
            readData = new byte[0x2000];

            readBytes = new ByteBuffer();

            readArgs = new SocketAsyncEventArgs();
            readArgs.UserToken = this;
            readArgs.SetBuffer(readData, 0, readData.Length);
            readArgs.AcceptSocket = connSocket;

            writeArgs = new SocketAsyncEventArgs();
            writeArgs.UserToken = this;
            writeArgs.AcceptSocket = connSocket;

            readArgs.Completed += IO_Completed;
            writeArgs.Completed += IO_Completed;
            if (!connSocket.ReceiveAsync(readArgs))
                ProcessRead();

            linkPacket = new LinkPacket();
            SendPacket(linkPacket);
        }

        public void Close()
        {
            if (connSocket == null)
                return;

            socketManager.Log.Message("客户端[{1}]{0}断开……", connSocket.RemoteEndPoint, id);
            socketManager.CloseSocket(this);
            try
            {
                connSocket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                connSocket.Close();
            }
            connSocket = null;
        }

        #endregion

        #region Packet

        private void ReadPacket()
        {
            if (needEncrypt)
                readBytes.Write(readArgs, c2sEncrypt);
            else
                readBytes.Write(readArgs);

            while (readBytes.GetLength() > sizeof(ushort))
            {
                var cmdId = readBytes.ReadByte();
                var packetLength = readBytes.ReadPacketLength();
                var packetData = readBytes.ReadBytes(packetLength);
                if (socketManager.Settings.ShowDetail)
                {
                    socketManager.Log.Message("内容:{0}", BitConverter.ToString(packetData));
                    socketManager.Log.Message("长度:{0}", packetLength);
                    socketManager.Log.Message("客户端[{2}]{0}接收到:{1:X2}", connSocket.RemoteEndPoint, cmdId, id);
                }
                BasePacket packet = SocketPacketManager.Instance.FindPacket(cmdId);
                if (packet != null)
                {
                    packet.Unpack(packetData);
                    HandlePacket(packet);
                }
                readBytes.ClearReadPacket();
            }
        }

        private void HandlePacket(BasePacket packet)
        {
            if (packet is LoginPacket)
                DoLogin((LoginPacket)packet);
            else if (packet is AuthPacket)
                DoAuth((AuthPacket)packet);
            else if (packet is PingPacket)
                SendPacket(packet);
            else if (packet is GetRolePacket)
                DoGetRole((GetRolePacket)packet);
            else if (packet is CreateRolePacket)
                DoCreateRole((CreateRolePacket)packet);
        }

        private void DoCreateRole(CreateRolePacket packet)
        {
            var replyPacket = new CreateRoleReplyPacket();
            replyPacket.AccountId = packet.AccountId;
            replyPacket.RoleClass = packet.RoleClass;
            replyPacket.RoleFace = packet.RoleFace;
            replyPacket.RoleHair = packet.RoleHair;
            replyPacket.RoleEar = packet.RoleEar;
            replyPacket.RoleTail = packet.RoleTail;
            replyPacket.RoleTeam = packet.RoleTeam;
            replyPacket.RoleName = packet.RoleName;
            replyPacket.RoleSex = packet.RoleSex;
            replyPacket.BaseMode = packet.BaseMode;
            replyPacket.EquipCount = 0;
            replyPacket.ErrorId = 0;
            replyPacket.FashionCloth = packet.FashionCloth;
            replyPacket.FashionHead = packet.FashionHead;
            replyPacket.FashionShoes = packet.FashionShoes;
            replyPacket.FashionWeapon = packet.FashionWeapon;
            replyPacket.mapid = 2;
            replyPacket.RoleId = 1024;
            replyPacket.RoleIndex = packet.RoleIndex;
            replyPacket.RoleLevel = packet.RoleLevel;
            SendPacket(replyPacket);
        }

        private void DoGetRole(GetRolePacket packet)
        {
            var replyPacket = new GetRoleReplyPacket();
            replyPacket.AccountID = packet.AccountID;
            replyPacket.HasRoleData = false;
            replyPacket.RoleIndex = -1;
            replyPacket.BindRoleID = 0;
            SendPacket(replyPacket);
        }

        private void DoAuth(AuthPacket packet)
        {
            var s2cKey = ComputeKey(packet.AuthHash);
            s2cEncrypt = new RC4(s2cKey);
            needCompress = true;

            var linePacket = new LinePacket();
            linePacket.LineName = socketManager.Settings.ServerName;
            SendPacket(linePacket);

            var account = new AccountInfoPacket();
            account.ServerId = socketManager.Settings.ServerID;
            account.SessionId = ID;
            account.AccountId = 1;
            SendPacket(account);
        }

        private byte[] ComputeKey(byte[] authHash)
        {
            var hmacmd5 = new HMACMD5(Encoding.ASCII.GetBytes(loginPacket.UserName));
            var macHash = loginPacket.LoginHash.Concat(authHash).ToArray();
            var key = hmacmd5.ComputeHash(macHash);
            return key;
        }

        private void DoLogin(LoginPacket packet)
        {
            socketManager.Log.Message("客户端[{2}]{1}请求登录用户{0}！", packet.UserName, connSocket.RemoteEndPoint, id);
            loginPacket = packet;
            var authPacket = new AuthPacket();
            SendPacket(authPacket);
            var c2sKey = ComputeKey(authPacket.AuthHash);
            c2sEncrypt = new RC4(c2sKey);
            needEncrypt = true;
        }

        private void SendPacket(BasePacket packet = null)
        {
            if (packet != null)
                packets.Add(packet);
            if (!isSending)
            {
                var p = packets[0];
                packets.RemoveAt(0);
                isSending = true;
                var bytes = p.Pack();
                if (needCompress)
                {
                    bytes = compressor.Compress(bytes);
                    bytes = s2cEncrypt.Encrypt(bytes);
                }
                writeArgs.SetBuffer(bytes, 0, bytes.Length);
                if (!connSocket.SendAsync(writeArgs))
                    ProcessSend();
            }
        }

        #endregion
    }
}
