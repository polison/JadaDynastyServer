using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JadeDynastyServer
{
    [Serializable]
    public class ServerSettings
    {
        #region LoadSetting
        private static ServerSettings settings;

        public static ServerSettings LoadSettings(ILog log)
        {
            if (settings == null)
            {
                try
                {
                    Deserialize();
                }
                catch(FileNotFoundException)
                {
                    log.Warning("配置文件未找到，创建一个！");
                    settings = new ServerSettings();
                    Serialize();
                }
            }

            return settings;
        }

        public static void Deserialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerSettings));

            using (TextReader textReader = new StreamReader("JadeDynastyServer.xml"))
            {
                settings = (ServerSettings)xmlSerializer.Deserialize(textReader);
            }
        }

        public static void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerSettings));
            using (TextWriter textWriter = new StreamWriter("JadeDynastyServer.xml"))
            {
                xmlSerializer.Serialize(textWriter, settings);
            }
        }
        #endregion

        public void SaveSettings(ILog log)
        {
            Serialize();
            log.Message("成功保存配置！");
        }

        #region Settings

        /// <summary>
        /// 数据库地址
        /// </summary>
        private string dbHost = "127.0.0.1";
        public string DBHost
        {
            get => dbHost;
            set => dbHost = value;
        }

        /// <summary>
        /// 数据库端口
        /// </summary>
        private int dbPort = 3306;
        public int DBPort
        {
            get => dbPort; 
            set => dbPort = value;
        }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        private string dbUser = "root";
        public string DBUser
        {
            get => dbUser;
            set => dbUser = value;
        }

        /// <summary>
        /// 数据库密码
        /// </summary>
        private string dbPassword = "123456";
        public string DBPassword
        {
            get => dbPassword;
            set => dbPassword = value;
        }

        /// <summary>
        /// 数据库名
        /// </summary>
        private string dbName = "elementworld";
        public string DBName
        {
            get => dbName;
            set => dbName = value;
        }

        /// <summary>
        /// 服务器id
        /// </summary>
        private int serverId = 500;
        public int ServerID
        {
            get => serverId;
            set => serverId = value;
        }

        /// <summary>
        /// 服务器端口
        /// </summary>
        private int serverPort = 29000;
        public int ServerPort
        {
            get => serverPort;
            set => serverPort = value;
        }

        /// <summary>
        /// 服务器显示名-线路1名称
        /// </summary>
        private string serverName = "单机诛仙一线";
        public string ServerName
        {
            get => serverName;
            set => serverName = value;
        }

        /// <summary>
        /// 显示客户端封包详细信息
        /// </summary>
        private bool showDetail = true;
        public bool ShowDetail
        {
            get => showDetail;
            set => showDetail = value;
        }

        #endregion
    }
}
