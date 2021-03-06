﻿namespace SiRFLive.GUI.Python
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public class ListBoxStream : Stream
    {
        private static bool isFileOpen = false;
        private ListBox listbox;
        private static string logFilePath = string.Empty;
        private static StreamWriter m_streamwriter;

        public ListBoxStream(ref ListBox inbox)
        {
            this.listbox = inbox;
        }

        private void AddLBText(string text)
        {
            if (this.listbox.InvokeRequired)
            {
                SetTextCallback method = new SetTextCallback(this.AddLBText);
                this.listbox.Invoke(method, new object[] { text });
            }
            else
            {
                this.listbox.SelectedIndex = this.listbox.Items.Add(text);
                if (isFileOpen)
                {
                    m_streamwriter.Write(text + "\r\n");
                }
            }
        }

        public bool CloseFile()
        {
            try
            {
                if (isFileOpen)
                {
                    isFileOpen = false;
                    if (m_streamwriter != null)
                    {
                        m_streamwriter.Close();
                        m_streamwriter = null;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            isFileOpen = false;
            return true;
        }

        public override void Flush()
        {
        }

        public bool OpenFile()
        {
            string text = string.Empty;
            if (File.Exists(logFilePath))
            {
                m_streamwriter = File.AppendText(logFilePath);
                isFileOpen = true;
                text = string.Format("File exists -- append only -- {0}", logFilePath);
            }
            else
            {
                m_streamwriter = File.CreateText(logFilePath);
                isFileOpen = true;
                text = string.Format("File opens successfully -- {0}", logFilePath);
            }
            this.AddLBText(text);
            return isFileOpen;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return -1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return -1L;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string str = Encoding.Default.GetString(buffer, offset, count).Trim();
            if (!string.IsNullOrEmpty(str))
            {
                this.AddLBText(str);
            }
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return 0L;
            }
        }

        public string LogFilePath
        {
            get
            {
                return logFilePath;
            }
            set
            {
                logFilePath = value;
            }
        }

        public override long Position
        {
            get
            {
                return -1L;
            }
            set
            {
            }
        }

        private delegate void SetTextCallback(string text);
    }
}

