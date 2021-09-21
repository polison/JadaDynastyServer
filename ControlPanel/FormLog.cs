using JadeDynastyServer;
using System;
using System.Text;

namespace ControlPanel
{
    class FormLog : ILog
    {
        #region ILog

        public void Error(string format, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("E:{0}-{1},", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
            sb.AppendFormat(format, args);
            sb.AppendLine();
            form.Invoke(form.logTextCB, sb.ToString());
        }

        public void Message(string format, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("M:{0}-{1},", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
            sb.AppendFormat(format, args);
            sb.AppendLine();
            form.Invoke(form.logTextCB, sb.ToString());
        }

        public void Warning(string format, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("W:{0}-{1},", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
            sb.AppendFormat(format, args);
            sb.AppendLine();
            form.Invoke(form.logTextCB, sb.ToString());
        }

        #endregion

        private MainForm form;

        public FormLog(MainForm mainForm)
        {
            form = mainForm;
        }
    }
}

