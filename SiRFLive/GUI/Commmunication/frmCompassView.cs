﻿namespace SiRFLive.GUI.Commmunication
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCompassView : Form
    {
        private const int accelCal_magnCal = 0x11;
        private const int accelCal_magnCalReq = 0x12;
        private const int accelCal_magnDisturb = 0x13;
        private const int accelCal_magnUnk = 0x10;
        private const int accelCalReq_magnCal = 0x21;
        private const int accelCalReq_magnCalReq = 0x22;
        private const int accelCalReq_magnDisturb = 0x23;
        private const int accelCalReq_magnUnk = 0x20;
        private const int accelDisturb_magnCal = 0x31;
        private const int accelDisturb_magnCalReq = 50;
        private const int accelDisturb_magnDisturb = 0x33;
        private const int accelDisturb_magnUnk = 0x30;
        private const int accelUnk_magnCal = 1;
        private const int accelUnk_magnCalReq = 2;
        private const int accelUnk_magnDisturb = 3;
        private const int accelUnk_magnUnk = 0;
        private int callightHeight;
        private int callightWidth;
        private int callightX;
        private int callightYbottom;
        private int callightYtop;
        private int calrectHeight;
        private int calrectWidth;
        private int calrectX;
        private int calrectY;
        private CommunicationManager comm;
        private IContainer components;
        private MyPanel myPanel;
        private MyPanel panel_Compass;
        private MyPanel panel_Pitch;
        private MyPanel panel_Roll;
        private int pitch_lightHeight;
        private int pitch_lightWidth;
        private int pitch_lightX;
        private int pitch_lightYbottom;
        private int pitch_lightYtop;
        private int pitch_rectHeight;
        private int pitch_rectWidth;
        private int pitch_rectX;
        private int pitch_rectY;
        private int roll_lightHeight;
        private int roll_lightWidth;
        private int roll_lightX;
        private int roll_lightYbottom;
        private int roll_lightYtop;
        private int roll_rectHeight;
        private int roll_rectWidth;
        private int roll_rectX;
        private int roll_rectY;
        private ToolTip toolTipCompassView;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCompassView()
        {
            this.calrectX = 8;
            this.calrectY = 0x10;
            this.calrectWidth = 12;
            this.calrectHeight = 0x1c;
            this.callightX = 10;
            this.callightYtop = 20;
            this.callightYbottom = 0x20;
            this.callightWidth = 8;
            this.callightHeight = 8;
            this.pitch_rectX = 8;
            this.pitch_rectY = 8;
            this.pitch_rectWidth = 8;
            this.pitch_rectHeight = 0x10;
            this.pitch_lightX = 10;
            this.pitch_lightYtop = 11;
            this.pitch_lightYbottom = 0x11;
            this.pitch_lightWidth = 4;
            this.pitch_lightHeight = 4;
            this.roll_rectX = 8;
            this.roll_rectY = 8;
            this.roll_rectWidth = 8;
            this.roll_rectHeight = 0x10;
            this.roll_lightX = 10;
            this.roll_lightYtop = 11;
            this.roll_lightYbottom = 0x11;
            this.roll_lightWidth = 4;
            this.roll_lightHeight = 4;
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        public frmCompassView(CommunicationManager mainComWin)
        {
            this.calrectX = 8;
            this.calrectY = 0x10;
            this.calrectWidth = 12;
            this.calrectHeight = 0x1c;
            this.callightX = 10;
            this.callightYtop = 20;
            this.callightYbottom = 0x20;
            this.callightWidth = 8;
            this.callightHeight = 8;
            this.pitch_rectX = 8;
            this.pitch_rectY = 8;
            this.pitch_rectWidth = 8;
            this.pitch_rectHeight = 0x10;
            this.pitch_lightX = 10;
            this.pitch_lightYtop = 11;
            this.pitch_lightYbottom = 0x11;
            this.pitch_lightWidth = 4;
            this.pitch_lightHeight = 4;
            this.roll_rectX = 8;
            this.roll_rectY = 8;
            this.roll_rectWidth = 8;
            this.roll_rectHeight = 0x10;
            this.roll_lightX = 10;
            this.roll_lightYtop = 11;
            this.roll_lightYbottom = 0x11;
            this.roll_lightWidth = 4;
            this.roll_lightHeight = 4;
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
            this.CommWindow = mainComWin;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void CompassPanelMeasurements()
        {
            if (this.WinTop != 0)
            {
                base.Top = this.WinTop;
            }
            if (this.WinLeft != 0)
            {
                base.Left = this.WinLeft;
            }
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
            this.CompassPanelResizeMeas();
        }

        private void CompassPanelResizeMeas()
        {
            int num = 0x1c;
            int num2 = base.Height - num;
            int width = base.Width;
            this.panel_Compass.Height = num2;
            this.panel_Compass.Width = (int) (width * 0.67);
            this.panel_Pitch.Height = (int) (num2 * 0.5);
            this.panel_Pitch.Width = ((int) (width * 0.33)) - 7;
            this.panel_Roll.Height = (int) (num2 * 0.5);
            this.panel_Roll.Width = ((int) (width * 0.33)) - 7;
            this.panel_Pitch.Left = this.panel_Compass.Location.X + this.panel_Compass.Width;
            this.panel_Roll.Left = this.panel_Compass.Location.X + this.panel_Compass.Width;
            this.panel_Pitch.Top = this.panel_Compass.Location.Y;
            this.panel_Roll.Top = this.panel_Pitch.Location.Y + this.panel_Pitch.Height;
            this.panel_Compass.Invalidate();
            this.panel_Pitch.Invalidate();
            this.panel_Roll.Invalidate();
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private double DegreeToRadian(double angle)
        {
            return ((3.1415926535897931 * angle) / 180.0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCompassView_Load(object sender, EventArgs e)
        {
            this.CompassPanelMeasurements();
        }

        private void frmCompassView_LocationChanged(object sender, EventArgs e)
        {
            this.CompassPanelResizeMeas();
        }

        private void frmCompassView_ResizeEnd(object sender, EventArgs e)
        {
            this.CompassPanelResizeMeas();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCompassView));
            this.toolTipCompassView = new ToolTip(this.components);
            this.panel_Roll = new MyPanel();
            this.panel_Pitch = new MyPanel();
            this.panel_Compass = new MyPanel();
            base.SuspendLayout();
            this.panel_Roll.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.panel_Roll.Location = new Point(270, 0x83);
            this.panel_Roll.Margin = new Padding(1);
            this.panel_Roll.Name = "panel_Roll";
            this.panel_Roll.Size = new Size(0x87, 0x87);
            this.panel_Roll.TabIndex = 2;
            this.panel_Roll.DoubleClick += new EventHandler(this.panel_Roll_DoubleClick);
            this.panel_Roll.Paint += new PaintEventHandler(this.panel_Roll_Paint);
            this.panel_Roll.Resize += new EventHandler(this.panel_Roll_Resize);
            this.panel_Pitch.Location = new Point(270, 1);
            this.panel_Pitch.Margin = new Padding(1);
            this.panel_Pitch.Name = "panel_Pitch";
            this.panel_Pitch.Size = new Size(0x87, 0x87);
            this.panel_Pitch.TabIndex = 1;
            this.panel_Pitch.DoubleClick += new EventHandler(this.panel_Pitch_DoubleClick);
            this.panel_Pitch.Paint += new PaintEventHandler(this.panel_Pitch_Paint);
            this.panel_Pitch.Resize += new EventHandler(this.panel_Pitch_Resize);
            this.panel_Compass.Location = new Point(1, 1);
            this.panel_Compass.Margin = new Padding(1);
            this.panel_Compass.Name = "panel_Compass";
            this.panel_Compass.Size = new Size(270, 270);
            this.panel_Compass.TabIndex = 0;
            this.panel_Compass.DoubleClick += new EventHandler(this.panel_Compass_DoubleClick);
            this.panel_Compass.Paint += new PaintEventHandler(this.panel_Compass_Paint);
            this.panel_Compass.Resize += new EventHandler(this.panel_Compass_Resize);
            this.panel_Compass.MouseHover += new EventHandler(this.panel_Compass_MouseHover);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x189, 0x10c);
            base.Controls.Add(this.panel_Roll);
            base.Controls.Add(this.panel_Pitch);
            base.Controls.Add(this.panel_Compass);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Location = new Point(50, 50);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmCompassView";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "MEMS Compass View";
            base.Load += new EventHandler(this.frmCompassView_Load);
            base.LocationChanged += new EventHandler(this.frmCompassView_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCompassView_ResizeEnd);
            base.ResumeLayout(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow(base.Name);
            }
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            base.OnClosed(e);
        }

        private void panel_Compass_DoubleClick(object sender, EventArgs e)
        {
            this.CompassPanelMeasurements();
        }

        private void panel_Compass_MouseHover(object sender, EventArgs e)
        {
            string text = "Double click to reset to default window size." + Environment.NewLine + Environment.NewLine + "Calibration Status:" + Environment.NewLine + "   - Gray:       Unknown/Disabled" + Environment.NewLine + "   - Orange:   Disturbed" + Environment.NewLine + "   - Red:         Required" + Environment.NewLine + "   - Green:     Calibrated";
            this.toolTipCompassView.Show(text, this.panel_Compass, 0x7530);
        }

        private void panel_Compass_Paint(object sender, PaintEventArgs e)
        {
            float height = this.panel_Compass.Height;
            float width = this.panel_Compass.Width;
            float num3 = height;
            float num4 = width;
            if (width > height)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            using (BufferedGraphics graphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
            {
                Graphics graphics2 = graphics.Graphics;
                graphics2.SmoothingMode = SmoothingMode.AntiAlias;
                graphics2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point((int) num4, (int) num3), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xf5, 0xea));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0f, 0f, num4, num3);
                int num5 = 3;
                Pen pen = new Pen(Color.DarkGray, (float) num5);
                Pen pen2 = new Pen(Color.Red, (float) (num5 - 1));
                Pen pen3 = new Pen(Color.LightGray, (float) num5);
                Pen pen4 = new Pen(Color.LightGray, (float) (num5 - 1));
                Brush brush2 = new SolidBrush(Color.White);
                Brush brush3 = new SolidBrush(Color.Red);
                Brush brush4 = new SolidBrush(Color.Black);
                Brush brush5 = new SolidBrush(Color.Lime);
                Brush brush6 = new SolidBrush(Color.SeaShell);
                Brush brush7 = new SolidBrush(Color.OrangeRed);
                Brush brush8 = new SolidBrush(Color.Gray);
                double num6 = (width - (width / 6f)) / 2f;
                double num7 = (width - (width / 4f)) / 2f;
                int num8 = (int) (width / 2f);
                int num9 = (int) (height / 2f);
                graphics2.FillEllipse(brush4, (float) num5, (float) num5, width - (2 * num5), height - (2 * num5));
                graphics2.DrawEllipse(pen, (float) num5, (float) num5, width - (2 * num5), height - (2 * num5));
                graphics2.DrawEllipse(pen3, (float) (2 * num5), (float) (2 * num5), width - (4 * num5), height - (4 * num5));
                for (int i = 0; i < 0x10; i++)
                {
                    double angle = (i * 22.5) + this.comm.dataGui.HeadingDegrees;
                    double num12 = Math.Sin(this.DegreeToRadian(angle)) * num7;
                    double num13 = Math.Cos(this.DegreeToRadian(angle)) * num7;
                    double num14 = Math.Sin(this.DegreeToRadian(angle)) * (num7 - (2f * this.Font.Size));
                    double num15 = Math.Cos(this.DegreeToRadian(angle)) * (num7 - (2f * this.Font.Size));
                    graphics2.DrawLine(pen4, (int) (((int) num12) + num8), (int) (((int) num13) + num9), (int) (((int) num14) + num8), (int) (((int) num15) + num9));
                }
                Font font = new Font("Microsoft Sans Serif", 9f);
                float num16 = font.Size / 2f;
                double num17 = Math.Sin(this.DegreeToRadian(this.comm.dataGui.HeadingDegrees)) * num6;
                double num18 = Math.Cos(this.DegreeToRadian(this.comm.dataGui.HeadingDegrees)) * num6;
                graphics2.DrawString("N", font, brush3, (float) (((float) (num8 - num17)) - num16), (float) (((float) (num9 - num18)) - num16));
                graphics2.DrawString("S", font, brush3, (float) (((float) (num8 + num17)) - num16), (float) (((float) (num9 + num18)) - num16));
                graphics2.DrawString("W", font, brush3, (float) (((float) (num8 - num18)) - num16), (float) (((float) (num9 + num17)) - num16));
                graphics2.DrawString("E", font, brush3, (float) (((float) (num8 + num18)) - num16), (float) (((float) (num9 - num17)) - num16));
                graphics2.DrawLine(pen2, num8, 8, num8, num9 - (num9 / 2));
                Font font2 = new Font("Arial", 16f);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graphics2.DrawString(string.Format("{0}\x00b0", this.comm.dataGui.HeadingDegrees), font2, brush2, width / 2f, (height / 2f) - (font2.Size / 2f), format);
                Font font3 = new Font("Microsoft Sans Serif", 7f);
                graphics2.DrawString("CAL", font3, brush4, (float) 3f, (float) 4f);
                graphics2.FillRectangle(brush4, this.calrectX, this.calrectY, this.calrectWidth, this.calrectHeight);
                switch (this.comm.dataGui.CalStatus)
                {
                    case 0:
                    case 0x10:
                    case 0x20:
                    case 0x30:
                        graphics2.FillEllipse(brush8, this.callightX, this.callightYtop, this.callightWidth, this.callightHeight);
                        graphics2.FillEllipse(brush6, this.callightX, this.callightYbottom, this.callightWidth, this.callightHeight);
                        break;

                    case 1:
                    case 0x11:
                    case 0x21:
                    case 0x31:
                        graphics2.FillEllipse(brush5, this.callightX, this.callightYtop, this.callightWidth, this.callightHeight);
                        graphics2.FillEllipse(brush6, this.callightX, this.callightYbottom, this.callightWidth, this.callightHeight);
                        break;

                    case 2:
                    case 0x12:
                    case 0x22:
                    case 50:
                        graphics2.FillEllipse(brush6, this.callightX, this.callightYtop, this.callightWidth, this.callightHeight);
                        graphics2.FillEllipse(brush3, this.callightX, this.callightYbottom, this.callightWidth, this.callightHeight);
                        break;

                    case 3:
                    case 0x13:
                    case 0x23:
                    case 0x33:
                        graphics2.FillEllipse(brush7, this.callightX, this.callightYtop, this.callightWidth, this.callightHeight);
                        graphics2.FillEllipse(brush6, this.callightX, this.callightYbottom, this.callightWidth, this.callightHeight);
                        break;

                    default:
                        graphics2.FillEllipse(brush8, this.callightX, this.callightYtop, this.callightWidth, this.callightHeight);
                        graphics2.FillEllipse(brush6, this.callightX, this.callightYbottom, this.callightWidth, this.callightHeight);
                        break;
                }
                graphics.Render(e.Graphics);
            }
        }

        private void panel_Compass_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_Compass.Refresh();
        }

        private void panel_Pitch_DoubleClick(object sender, EventArgs e)
        {
            this.CompassPanelMeasurements();
        }

        private void panel_Pitch_Paint(object sender, PaintEventArgs e)
        {
            float height = this.panel_Pitch.Height;
            float width = this.panel_Pitch.Width;
            float num3 = height;
            float num4 = width;
            if (width > height)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            using (BufferedGraphics graphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
            {
                Graphics graphics2 = graphics.Graphics;
                graphics2.SmoothingMode = SmoothingMode.AntiAlias;
                graphics2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point((int) num4, (int) num3), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xf5, 0xea));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0f, 0f, num4, num3);
                int num5 = 3;
                Pen pen = new Pen(Color.DarkGray, (float) num5);
                Pen pen2 = new Pen(Color.Red, (float) (num5 - 1));
                Pen pen3 = new Pen(Color.LightGray, (float) num5);
                Pen pen4 = new Pen(Color.LightGray, (float) (num5 - 1));
                Brush brush2 = new SolidBrush(Color.White);
                Brush brush3 = new SolidBrush(Color.Red);
                Brush brush4 = new SolidBrush(Color.Black);
                Brush brush5 = new SolidBrush(Color.Lime);
                Brush brush6 = new SolidBrush(Color.SeaShell);
                Brush brush7 = new SolidBrush(Color.Orange);
                Brush brush8 = new SolidBrush(Color.Gray);
                double num6 = (width - (width / 6f)) / 2f;
                double num7 = (width - (width / 4f)) / 2f;
                int num8 = (int) (width / 2f);
                int num9 = (int) (height / 2f);
                graphics2.FillEllipse(brush4, (float) num5, (float) num5, width - (2 * num5), height - (2 * num5));
                graphics2.DrawEllipse(pen, (float) num5, (float) num5, width - (2 * num5), height - (2 * num5));
                graphics2.DrawEllipse(pen3, (float) (2 * num5), (float) (2 * num5), width - (4 * num5), height - (4 * num5));
                for (int i = 0; i < 0x24; i++)
                {
                    double num11 = i * 10;
                    double num12 = Math.Sin(this.DegreeToRadian(num11)) * num7;
                    double num13 = Math.Cos(this.DegreeToRadian(num11)) * num7;
                    double num14 = Math.Sin(this.DegreeToRadian(num11)) * (num7 - (2f * this.Font.Size));
                    double num15 = Math.Cos(this.DegreeToRadian(num11)) * (num7 - (2f * this.Font.Size));
                    graphics2.DrawLine(pen4, (int) (((int) num12) + num8), (int) (((int) num13) + num9), (int) (((int) num14) + num8), (int) (((int) num15) + num9));
                }
                Font font = new Font("Microsoft Sans Serif", 7f);
                float num16 = font.Size / 2f;
                double num17 = Math.Sin(0.0) * num6;
                double num18 = Math.Cos(0.0) * num6;
                graphics2.DrawString("90\x00b0", font, brush3, (float) (((float) (num8 - num17)) - num16), (float) (((float) (num9 - num18)) - num16));
                graphics2.DrawString("-90\x00b0", font, brush3, (float) (((float) (num8 + num17)) - num16), (float) (((float) (num9 + num18)) - num16));
                graphics2.DrawString("0\x00b0", font, brush3, (float) (((float) (num8 - num18)) - num16), (float) (((float) (num9 + num17)) - num16));
                double num19 = (width - (width / 6f)) / 2f;
                double num20 = (width - (width / 4f)) / 2f;
                Math.Sin(this.DegreeToRadian(this.comm.dataGui.PitchDegrees));
                Math.Cos(this.DegreeToRadian(this.comm.dataGui.PitchDegrees));
                double angle = (this.comm.dataGui.PitchDegrees * -1.0) - 90.0;
                double num22 = Math.Sin(this.DegreeToRadian(angle)) * num19;
                double num23 = Math.Cos(this.DegreeToRadian(angle)) * num19;
                double num24 = Math.Sin(this.DegreeToRadian(angle)) * (num20 - (4f * this.Font.Size));
                double num25 = Math.Cos(this.DegreeToRadian(angle)) * (num20 - (4f * this.Font.Size));
                graphics2.DrawLine(pen2, (int) (((int) num22) + num8), (int) (((int) num23) + num9), (int) (((int) num24) + num8), (int) (((int) num25) + num9));
                Font font2 = new Font("Arial", 10f);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graphics2.DrawString(string.Format("{0}\x00b0", this.comm.dataGui.PitchDegrees), font2, brush2, width / 2f, (height / 2f) - (font2.Size / 2f), format);
                font2 = new Font("Arial", 9f);
                graphics2.DrawString("Pitch", font2, brush3, width / 2f, ((height / 2f) + ((float) (font2.Size * 1.5))) - (font2.Size / 2f), format);
                graphics2.FillRectangle(brush4, this.pitch_rectX, this.pitch_rectY, this.pitch_rectWidth, this.pitch_rectHeight);
                switch (this.comm.dataGui.CalStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        graphics2.FillEllipse(brush8, this.pitch_lightX, this.pitch_lightYtop, this.pitch_lightWidth, this.pitch_lightHeight);
                        graphics2.FillEllipse(brush6, this.pitch_lightX, this.pitch_lightYbottom, this.pitch_lightWidth, this.pitch_lightHeight);
                        break;

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                        graphics2.FillEllipse(brush5, this.pitch_lightX, this.pitch_lightYtop, this.pitch_lightWidth, this.pitch_lightHeight);
                        graphics2.FillEllipse(brush6, this.pitch_lightX, this.pitch_lightYbottom, this.pitch_lightWidth, this.pitch_lightHeight);
                        break;

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                        graphics2.FillEllipse(brush6, this.pitch_lightX, this.pitch_lightYtop, this.pitch_lightWidth, this.pitch_lightHeight);
                        graphics2.FillEllipse(brush3, this.pitch_lightX, this.pitch_lightYbottom, this.pitch_lightWidth, this.pitch_lightHeight);
                        break;

                    case 0x30:
                    case 0x31:
                    case 50:
                    case 0x33:
                        graphics2.FillEllipse(brush7, this.pitch_lightX, this.pitch_lightYtop, this.pitch_lightWidth, this.pitch_lightHeight);
                        graphics2.FillEllipse(brush6, this.pitch_lightX, this.pitch_lightYbottom, this.pitch_lightWidth, this.pitch_lightHeight);
                        break;

                    default:
                        graphics2.FillEllipse(brush8, this.pitch_lightX, this.pitch_lightYtop, this.pitch_lightWidth, this.pitch_lightHeight);
                        graphics2.FillEllipse(brush6, this.pitch_lightX, this.pitch_lightYbottom, this.pitch_lightWidth, this.pitch_lightHeight);
                        break;
                }
                graphics.Render(e.Graphics);
            }
        }

        private void panel_Pitch_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_Pitch.Refresh();
        }

        private void panel_Roll_DoubleClick(object sender, EventArgs e)
        {
            this.CompassPanelMeasurements();
        }

        private void panel_Roll_Paint(object sender, PaintEventArgs e)
        {
            float height = this.panel_Roll.Height;
            float width = this.panel_Roll.Width;
            float num3 = height;
            float num4 = width;
            if (width > height)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            using (BufferedGraphics graphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
            {
                Graphics graphics2 = graphics.Graphics;
                graphics2.SmoothingMode = SmoothingMode.AntiAlias;
                graphics2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point((int) num4, (int) num3), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xf5, 0xea));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0f, 0f, num4, num3);
                int num5 = 3;
                Pen pen = new Pen(Color.DarkGray, (float) num5);
                Pen pen2 = new Pen(Color.Red, (float) (num5 - 1));
                Pen pen3 = new Pen(Color.LightGray, (float) num5);
                Pen pen4 = new Pen(Color.LightGray, (float) (num5 - 1));
                Brush brush2 = new SolidBrush(Color.White);
                Brush brush3 = new SolidBrush(Color.Red);
                Brush brush4 = new SolidBrush(Color.Black);
                Brush brush5 = new SolidBrush(Color.Lime);
                Brush brush6 = new SolidBrush(Color.SeaShell);
                Brush brush7 = new SolidBrush(Color.Orange);
                Brush brush8 = new SolidBrush(Color.Gray);
                double num6 = (width - (width / 6f)) / 2f;
                double num7 = (width - (width / 4f)) / 2f;
                int num8 = (int) (width / 2f);
                int num9 = (int) (height / 2f);
                graphics2.FillEllipse(brush4, (float) num5, (float) num5, width - (2 * num5), height - (2 * num5));
                graphics2.DrawEllipse(pen, (float) num5, (float) num5, width - (2 * num5), height - (2 * num5));
                graphics2.DrawEllipse(pen3, (float) (2 * num5), (float) (2 * num5), width - (4 * num5), height - (4 * num5));
                for (int i = 0; i < 0x24; i++)
                {
                    double angle = (i * 10) + this.comm.dataGui.RollDegrees;
                    double num12 = Math.Sin(this.DegreeToRadian(angle)) * num7;
                    double num13 = Math.Cos(this.DegreeToRadian(angle)) * num7;
                    double num14 = Math.Sin(this.DegreeToRadian(angle)) * (num7 - (2f * this.Font.Size));
                    double num15 = Math.Cos(this.DegreeToRadian(angle)) * (num7 - (2f * this.Font.Size));
                    switch (i)
                    {
                        case 9:
                        case 0x1b:
                            graphics2.DrawLine(pen4, ((int) num12) + num8, ((int) num13) + num9, num8, num9);
                            break;

                        default:
                            graphics2.DrawLine(pen4, (int) (((int) num12) + num8), (int) (((int) num13) + num9), (int) (((int) num14) + num8), (int) (((int) num15) + num9));
                            break;
                    }
                }
                Font font = new Font("Microsoft Sans Serif", 7f);
                float num16 = font.Size / 2f;
                double num17 = Math.Sin(this.DegreeToRadian(this.comm.dataGui.RollDegrees)) * num6;
                double num18 = Math.Cos(this.DegreeToRadian(this.comm.dataGui.RollDegrees)) * num6;
                graphics2.DrawString("90\x00b0", font, brush3, (float) (((float) (num8 - num17)) - num16), (float) (((float) (num9 - num18)) - num16));
                graphics2.DrawString("-90\x00b0", font, brush3, (float) (((float) (num8 + num17)) - num16), (float) (((float) (num9 + num18)) - num16));
                graphics2.DrawString("0\x00b0", font, brush3, (float) (((float) (num8 - num18)) - num16), (float) (((float) (num9 + num17)) - num16));
                graphics2.DrawLine(pen2, 8f, (float) num9, (float) (((double) num8) / 1.5), (float) num8);
                graphics2.DrawLine(pen2, (float) ((((double) num8) / 1.5) * 2.0), (float) num9, width - 8f, (float) num9);
                Font font2 = new Font("Arial", 10f);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graphics2.FillRectangle(brush4, (float) (width * 0.4f), (float) (height * 0.46f), (float) (width * 0.2f), (float) (width * 0.2f));
                graphics2.DrawString(string.Format("{0}\x00b0", this.comm.dataGui.RollDegrees), font2, brush2, width / 2f, (height / 2f) - (font2.Size / 2f), format);
                font2 = new Font("Arial", 9f);
                graphics2.DrawString("Roll", font2, brush3, width / 2f, ((height / 2f) + ((float) (font2.Size * 1.5))) - (font2.Size / 2f), format);
                graphics2.FillRectangle(brush4, this.roll_rectX, this.roll_rectY, this.roll_rectWidth, this.roll_rectHeight);
                switch (this.comm.dataGui.CalStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        graphics2.FillEllipse(brush8, this.roll_lightX, this.roll_lightYtop, this.roll_lightWidth, this.roll_lightHeight);
                        graphics2.FillEllipse(brush6, this.roll_lightX, this.roll_lightYbottom, this.roll_lightWidth, this.roll_lightHeight);
                        break;

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                        graphics2.FillEllipse(brush5, this.roll_lightX, this.roll_lightYtop, this.roll_lightWidth, this.roll_lightHeight);
                        graphics2.FillEllipse(brush6, this.roll_lightX, this.roll_lightYbottom, this.roll_lightWidth, this.roll_lightHeight);
                        break;

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                        graphics2.FillEllipse(brush6, this.roll_lightX, this.roll_lightYtop, this.roll_lightWidth, this.roll_lightHeight);
                        graphics2.FillEllipse(brush3, this.roll_lightX, this.roll_lightYbottom, this.roll_lightWidth, this.roll_lightHeight);
                        break;

                    case 0x30:
                    case 0x31:
                    case 50:
                    case 0x33:
                        graphics2.FillEllipse(brush7, this.roll_lightX, this.roll_lightYtop, this.roll_lightWidth, this.roll_lightHeight);
                        graphics2.FillEllipse(brush6, this.roll_lightX, this.roll_lightYbottom, this.roll_lightWidth, this.roll_lightHeight);
                        break;

                    default:
                        graphics2.FillEllipse(brush8, this.roll_lightX, this.roll_lightYtop, this.roll_lightWidth, this.roll_lightHeight);
                        graphics2.FillEllipse(brush6, this.roll_lightX, this.roll_lightYbottom, this.roll_lightWidth, this.roll_lightHeight);
                        break;
                }
                graphics.Render(e.Graphics);
            }
        }

        private void panel_Roll_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_Roll.Refresh();
        }

        private double RadianToDegree(double angle)
        {
            return (angle * 57.295779513082323);
        }

        public CommunicationManager CommWindow
        {
            get
            {
                return this.comm;
            }
            set
            {
                this.comm = value;
                this.comm.DisplayPanelCompass = this.panel_Compass;
                this.comm.DisplayPanelPitch = this.panel_Pitch;
                this.comm.DisplayPanelRoll = this.panel_Roll;
                this.myPanel = this.panel_Compass;
                this.comm.EnableCompassView = true;
                this.Text = this.comm.sourceDeviceName + ": MEMS Compass View";
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

