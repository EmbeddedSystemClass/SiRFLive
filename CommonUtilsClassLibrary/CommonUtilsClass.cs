﻿namespace CommonUtilsClassLibrary
{
    using CommonClassLibrary;
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class CommonUtilsClass
    {
        private int _displayBuffer = 10;
        private CommonClass.MyRichTextBox _displayWindow;
        private bool _viewPause;
        public CommonClass CC = new CommonClass();
        public int LineCount;
        public int MAX_LINE = 0x1388;

        public static string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            foreach (byte num in comByte)
            {
                builder.Append(Convert.ToString(num, 0x10).PadLeft(2, '0').PadRight(3, ' '));
            }
            return builder.ToString().ToUpper();
        }

        [STAThread]
        public void DisplayData(CommonClass.MessageType type, string msg)
        {
            EventHandler method = null;
            try
            {
                if (((this._displayWindow != null) && !this._viewPause) && !this._displayWindow.IsDisposed)
                {
                    if (this._displayWindow != null)
                    {
                        if (this._displayWindow.InvokeRequired)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.localDisplayData(type, msg);
                                };
                            }
                            this._displayWindow.BeginInvoke(method);
                        }
                        else
                        {
                            this.localDisplayData(type, msg);
                        }
                    }
                    else
                    {
                        MessageBox.Show("CommonUtilsClass: DisplayData() _displayWindow is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public static byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");
            byte[] buffer = new byte[msg.Length / 2];
            for (int i = 0; i < msg.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 0x10);
            }
            return buffer;
        }

        public static void loadLocation(Form formWindow, string top, string left, string width, string height, string state)
        {
            formWindow.Left = Convert.ToInt32(left);
            formWindow.Top = Convert.ToInt32(top);
            formWindow.Width = Convert.ToInt32(width);
            formWindow.Height = Convert.ToInt32(height);
            if (state == "Maximized")
            {
                formWindow.WindowState = FormWindowState.Maximized;
            }
            else if (state == "Minimized")
            {
                formWindow.WindowState = FormWindowState.Minimized;
            }
            else
            {
                formWindow.WindowState = FormWindowState.Normal;
            }
        }

        private void localDisplayData(CommonClass.MessageType type, string msg)
        {
            if (this.LineCount > this.MAX_LINE)
            {
                this._displayWindow.Clear();
                this.LineCount = 0;
            }
            if (type == CommonClass.MessageType.Matching)
            {
                this._displayWindow.SelectionFont = new Font(this._displayWindow.SelectionFont, FontStyle.Bold);
            }
            else
            {
                this._displayWindow.SelectionFont = new Font(this._displayWindow.SelectionFont, FontStyle.Regular);
            }
            this._displayWindow.SelectionColor = CommonClass.MessageColor[(int) type];
            this._displayWindow.AppendText(msg);
            if (!msg.Contains("\r\n"))
            {
                this._displayWindow.AppendText("\r\n");
            }
            this._displayWindow.ScrollToBottom();
        }

        public static string LogToGP2(byte[] inBytes)
        {
            StringBuilder builder = new StringBuilder();
            DateTime now = DateTime.Now;
            builder.AppendFormat("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}.{6:D3}\t({7})\t", new object[] { now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second, now.Millisecond, 0 });
            builder.Append(ByteToHex(inBytes));
            return builder.ToString().ToUpper();
        }

        public static string LogToGP2(string msg)
        {
            StringBuilder builder = new StringBuilder(msg.Length * 3);
            DateTime now = DateTime.Now;
            builder.AppendFormat("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}.{6:D3}\t({7})\t", new object[] { now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second, now.Millisecond, 0 });
            builder.Append(msg);
            return builder.ToString().ToUpper();
        }

        public static string LogToGP2(string msg, string timeStamp)
        {
            StringBuilder builder = new StringBuilder();
            if (timeStamp == string.Empty)
            {
                timeStamp = "0/0/0 0:0:0.00";
            }
            builder.Append(timeStamp + "\t(0)\t" + msg);
            return builder.ToString().ToUpper();
        }

        public static byte[] StrToByteArray(string str)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public int DisplayBuffer
        {
            get
            {
                return this._displayBuffer;
            }
            set
            {
                this._displayBuffer = value;
            }
        }

        public CommonClass.MyRichTextBox DisplayWindow
        {
            get
            {
                return this._displayWindow;
            }
            set
            {
                this._displayWindow = value;
            }
        }

        public bool viewPause
        {
            get
            {
                return this._viewPause;
            }
            set
            {
                this._viewPause = value;
            }
        }
    }
}

