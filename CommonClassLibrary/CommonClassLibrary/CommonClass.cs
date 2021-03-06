﻿namespace CommonClassLibrary
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class CommonClass
    {
        public static Color[] MessageColor = new Color[] { Color.Black, Color.Green, Color.Black, Color.Orange, Color.Blue, Color.Red };

        public class BinarycheckBox : CheckBox
        {
            private Brush _backgroundbrush = new SolidBrush(Color.Green);
            private static Brush bluebrush = new SolidBrush(Color.Black);
            private Pen bluepen = new Pen(bluebrush);
            private Font font = new Font("Ariel", 8f);
            private StringFormat strFormat = new StringFormat();

            public BinarycheckBox()
            {
                base.Paint += new PaintEventHandler(this.PaintHandler);
            }

            private void PaintHandler(object sender, PaintEventArgs pe)
            {
                Point point = new Point();
                if (base.CheckAlign == ContentAlignment.BottomCenter)
                {
                    point.X = (base.Width / 2) - 4;
                    point.Y = base.Height - 11;
                }
                if (base.CheckAlign == ContentAlignment.BottomLeft)
                {
                    point.X = 3;
                    point.Y = base.Height - 11;
                }
                if (base.CheckAlign == ContentAlignment.BottomRight)
                {
                    point.X = base.Width - 11;
                    point.Y = base.Height - 11;
                }
                if (base.CheckAlign == ContentAlignment.MiddleCenter)
                {
                    point.X = (base.Width / 2) - 4;
                    point.Y = (base.Height / 2) - 4;
                }
                if (base.CheckAlign == ContentAlignment.MiddleLeft)
                {
                    point.X = 3;
                    point.Y = (base.Height / 2) - 4;
                }
                if (base.CheckAlign == ContentAlignment.MiddleRight)
                {
                    point.X = base.Width - 11;
                    point.Y = (base.Height / 2) - 5;
                }
                if (base.CheckAlign == ContentAlignment.TopCenter)
                {
                    point.X = (base.Width / 2) - 1;
                    point.Y = 3;
                }
                if (base.CheckAlign == ContentAlignment.TopLeft)
                {
                    point.X = 3;
                    point.Y = 3;
                }
                if (base.CheckAlign == ContentAlignment.TopRight)
                {
                    point.X = base.Width - 11;
                    point.Y = 3;
                }
                this.SetBackgroundColor(pe.Graphics, this._backgroundbrush, new RectangleF(point.X - 0.45f, point.Y - 0.35f, 10.5f, 10.5f));
            }

            public void SetBackgroundColor(Graphics g, Brush b, RectangleF r)
            {
                g.FillRectangle(b, r);
            }

            public Brush BackgroundBrush
            {
                get
                {
                    return this._backgroundbrush;
                }
                set
                {
                    this._backgroundbrush = value;
                }
            }
        }

        public enum InputDeviceModes
        {
            RS232,
            TCP_Client,
            TCP_Server,
            FilePlayBack,
            I2C
        }

        public enum MessageSource
        {
            RX_INPUT = 1,
            RX_OUTPUT = 0,
            UNDEFINED = -1,
            USER_TEXT = 0xff
        }

        public enum MessageType
        {
            Incoming,
            Outgoing,
            Normal,
            Warning,
            Matching,
            Error
        }

        public class MyRichTextBox : RichTextBox
        {
            private const int SB_BOTTOM = 7;
            private const int SB_LINEDOWN = 1;
            private const int SB_LINEUP = 0;
            private const int SB_TOP = 6;
            private const int WM_VSCROLL = 0x115;

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("user32.dll", SetLastError=true)]
            private static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
            public void ScrollLineDown()
            {
                HandleRef hWnd = new HandleRef(this, base.Handle);
                PostMessage(hWnd, 0x115, new IntPtr(1), new IntPtr(0));
            }

            public void ScrollLineUp()
            {
                HandleRef hWnd = new HandleRef(this, base.Handle);
                PostMessage(hWnd, 0x115, new IntPtr(0), new IntPtr(0));
            }

            public void ScrollToBottom()
            {
                HandleRef hWnd = new HandleRef(this, base.Handle);
                PostMessage(hWnd, 0x115, new IntPtr(7), new IntPtr(0));
            }

            public void ScrollToTop()
            {
                HandleRef hWnd = new HandleRef(this, base.Handle);
                PostMessage(hWnd, 0x115, new IntPtr(6), new IntPtr(0));
            }
        }

        public enum ProductType
        {
            GSD4e = 1,
            GSD4t = 0,
            GSW = 2,
            SLC = 3,
            UNDEFINED = -1
        }

        public enum TransmissionType
        {
            Text,
            Hex,
            SSB,
            GP2,
            GPS,
            Bin
        }
    }
}

