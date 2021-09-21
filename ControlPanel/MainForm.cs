using JadeDynastyServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlPanel
{
    public partial class MainForm : Form
    {
        private FormLog formLog;
        private ServerSettings serverSettings;
        private JadeDynastyServer.JadeDynastyServer dynastyServer;

        public delegate void LogTextCallBack(string str);
        public LogTextCallBack logTextCB;

        public void LogTextAddLog(string str)
        {
            logTextBox.Text = logTextBox.Text.Insert(0, str);
            logTextBox.SelectionStart = 0;
        }

        public MainForm()
        {
            InitializeComponent();

            ChangeButtonState(true);

            formLog = new FormLog(this);
            logTextCB = new LogTextCallBack(LogTextAddLog);
            dynastyServer = new JadeDynastyServer.JadeDynastyServer(formLog);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            if (dynastyServer.StartServer())
                ChangeButtonState(false);
            else
                ChangeButtonState(true);
        }

        private void ChangeButtonState(bool enable)
        {
            button1.Enabled = enable;
            button2.Enabled = !enable;
            button3.Enabled = !enable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            dynastyServer.StopServer();
            ChangeButtonState(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            button1_Click(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serverSettings.DBHost = txtServerPort.Text;
            serverSettings.DBPort = int.Parse(txtDBPort.Text);
            serverSettings.DBUser = txtDBUser.Text;
            serverSettings.DBPassword = txtDBPassword.Text;
            serverSettings.DBName = txtDBName.Text;
            serverSettings.ServerID = int.Parse(txtServerID.Text);
            serverSettings.ServerPort = int.Parse(txtServerPort.Text);
            serverSettings.ServerName = txtServerName.Text;
            serverSettings.ShowDetail = chkDebugDetail.Checked;
            serverSettings.SaveSettings(formLog);
        }

        private void chkDebugDetail_CheckedChanged(object sender, EventArgs e)
        {
            serverSettings.ShowDetail = chkDebugDetail.Checked;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            serverSettings = ServerSettings.LoadSettings(formLog);
            txtServerPort.Text = serverSettings.DBHost;
            txtDBPort.Text = serverSettings.DBPort.ToString();
            txtDBUser.Text = serverSettings.DBUser;
            txtDBPassword.Text = serverSettings.DBPassword;
            txtDBName.Text = serverSettings.DBName;
            txtServerID.Text = serverSettings.ServerID.ToString();
            txtServerPort.Text = serverSettings.ServerPort.ToString();
            txtServerName.Text = serverSettings.ServerName;
            chkDebugDetail.Checked = serverSettings.ShowDetail;

            button1_Click(sender, e);
        }
    }
}
