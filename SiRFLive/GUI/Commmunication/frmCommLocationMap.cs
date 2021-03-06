﻿namespace SiRFLive.GUI.Commmunication
{
    using GPSUtils;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.GUI;
    using SiRFLive.GUI.DlgsInputMsg;
    using SiRFLive.MessageHandling;
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class frmCommLocationMap : Form
    {
        private string _lat = string.Empty;
        private double _locationMapRadius = 1.0;
        private string _lon = string.Empty;
        private static int _numberOpen;
        private string _persistedWindowName = "Input Command Window";
        internal frmMapIt _SiRFMap;
        private double CenterLatitude;
        private double CenterLongitude;
        private CommunicationManager comm;
        private IContainer components;
        private double EnteredFixedLatitude;
        private double EnteredFixedLongitude;
        private bool FixedLocationFlag;
        private GroupBox groupBox7;
        private Label label_Altitude;
        private Label label_fix;
        private Label label_gpsTime;
        private Label label_HDOP;
        private Label label_heading;
        private Label label_Latitude;
        private Label label_Longitude;
        private Label label_mode;
        private Label label_ReceiverTime;
        private Label label_Speed;
        private Label label_totalSVsUsed;
        private Label LatLongLabel;
        private ToolStripStatusLabel locationViewStatusLabel;
        private ToolStripButton locationViewToolbarClearBtn;
        private ToolStripButton locationViewToolbarMapitBtn;
        private ToolStripButton locationViewToolbarSetRadiusBtn;
        private ToolStrip locationViewToolStrip;
        private ContextMenuStrip MapRightClickContextMenuStrip;
        private MyPanel panel_Location;
        private int PanelCircleRadius;
        private int PanelHeightWidth;
        private int RightMouseClick_X;
        private int RightMouseClick_Y;
        private ToolStripMenuItem setMapCenterLocationHereToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripButton toolStripButton_setRefLocation;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommLocationMap()
        {
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "Map " + _numberOpen.ToString();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        private void button_MapIt_Click(object sender, EventArgs e)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                MessageBox.Show("No internet connection!", "Info", MessageBoxButtons.OK);
            }
            else if (this.comm.dataGui.Positions.PositionList.Count >= 1)
            {
                if (MessageBox.Show("\"Location data\" is about to be sent to Google Maps... Proceed?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this._SiRFMap = this.CreateGoogleMapWindow();
                }
            }
        }

        private void ConvertMousePositionToLatLong(int X, int Y, out double Lat, out double Long)
        {
            double num;
            double num2;
            double num3;
            double num4;
            bool flag = false;
            GPSUtilsClass class2 = new GPSUtilsClass();
            int zone = class2.ComputeZone(this.CenterLatitude, this.CenterLongitude);
            if (this.CenterLongitude < 0.0)
            {
                flag = true;
            }
            class2.ConvertGEO2UTM(this.CenterLatitude, this.CenterLongitude, zone, out num3, out num4);
            this.ConvertXandYToNorthAndEastBasedOnScreenCenter(X, Y, this.PanelCircleRadius, this.PanelHeightWidth, num3, num4, out num, out num2);
            class2.ConvertUTM2GEO(num, num2, zone, out Lat, out Long);
            if (flag)
            {
                Long = 0.0 - Long;
            }
        }

        private void ConvertXandYToNorthAndEastBasedOnScreenCenter(int x, int y, int radius, int panelHeightWidth, double centerNorth, double centerEast, out double North, out double East)
        {
            double num = (radius * 2.0) / ((double) panelHeightWidth);
            int num2 = panelHeightWidth / 2;
            North = centerNorth;
            East = centerEast;
            if ((x >= num2) && (y < num2))
            {
                North += Math.Abs((int) (y - num2)) * num;
                East += Math.Abs((int) (x - num2)) * num;
            }
            else if ((x >= num2) && (y >= num2))
            {
                North -= Math.Abs((int) (num2 - y)) * num;
                East += Math.Abs((int) (x - num2)) * num;
            }
            else if ((x < num2) && (y >= num2))
            {
                North -= Math.Abs((int) (y - num2)) * num;
                East -= Math.Abs((int) (num2 - x)) * num;
            }
            else if ((x < num2) && (y < num2))
            {
                North += Math.Abs((int) (num2 - y)) * num;
                East -= Math.Abs((int) (num2 - x)) * num;
            }
        }

        internal frmMapIt CreateGoogleMapWindow()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (this.comm != null)
                {
                    if (method == null)
                    {
                        method = delegate {
                            string str = this.comm.sourceDeviceName + ": GoogleMap";
                            if ((this._SiRFMap == null) || this._SiRFMap.IsDisposed)
                            {
                                this._SiRFMap = new frmMapIt(this.comm);
                                this._SiRFMap.MdiParent = base.MdiParent;
                                this._SiRFMap.Show();
                            }
                            this._SiRFMap.Text = str;
                            this._SiRFMap.BringToFront();
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    MessageBox.Show("COM window not initialized!", "Information");
                }
            }
            return this._SiRFMap;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommLocationMap_Load(object sender, EventArgs e)
        {
            this.FixedLocationFlag = false;
            base.Top = this.WinTop;
            base.Left = this.WinLeft;
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
        }

        private void frmCommLocationMap_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommLocationMap_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommLocationMapClearBtn_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                if (MessageBox.Show("Are you sure you want to clear all nav points from the map view?", "Clear Data", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    bool enableLocationMapView = this.comm.EnableLocationMapView;
                    this.comm.EnableLocationMapView = false;
                    this.comm.dataGui.Positions.PositionList.Clear();
                    this.comm.EnableLocationMapView = enableLocationMapView;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommLocationMap));
            this.groupBox7 = new GroupBox();
            this.label_heading = new Label();
            this.label_gpsTime = new Label();
            this.label_mode = new Label();
            this.label_fix = new Label();
            this.label_totalSVsUsed = new Label();
            this.label_Speed = new Label();
            this.label_HDOP = new Label();
            this.label_Altitude = new Label();
            this.label_Latitude = new Label();
            this.label_Longitude = new Label();
            this.label_ReceiverTime = new Label();
            this.MapRightClickContextMenuStrip = new ContextMenuStrip(this.components);
            this.setMapCenterLocationHereToolStripMenuItem = new ToolStripMenuItem();
            this.LatLongLabel = new Label();
            this.locationViewToolStrip = new ToolStrip();
            this.locationViewToolbarMapitBtn = new ToolStripButton();
            this.locationViewToolbarSetRadiusBtn = new ToolStripButton();
            this.locationViewToolbarClearBtn = new ToolStripButton();
            this.toolStripButton_setRefLocation = new ToolStripButton();
            this.statusStrip1 = new StatusStrip();
            this.locationViewStatusLabel = new ToolStripStatusLabel();
            this.panel_Location = new MyPanel();
            this.groupBox7.SuspendLayout();
            this.MapRightClickContextMenuStrip.SuspendLayout();
            this.locationViewToolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox7.BackColor = SystemColors.ControlLight;
            this.groupBox7.Controls.Add(this.label_heading);
            this.groupBox7.Controls.Add(this.label_gpsTime);
            this.groupBox7.Controls.Add(this.label_mode);
            this.groupBox7.Controls.Add(this.label_fix);
            this.groupBox7.Controls.Add(this.label_totalSVsUsed);
            this.groupBox7.Controls.Add(this.label_Speed);
            this.groupBox7.Controls.Add(this.label_HDOP);
            this.groupBox7.Controls.Add(this.label_Altitude);
            this.groupBox7.Controls.Add(this.label_Latitude);
            this.groupBox7.Controls.Add(this.label_Longitude);
            this.groupBox7.Controls.Add(this.label_ReceiverTime);
            this.groupBox7.Location = new Point(0x16, 0x22);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(0x1b9, 0x97);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Position Infomation";
            this.label_heading.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_heading.AutoSize = true;
            this.label_heading.Location = new Point(0x135, 70);
            this.label_heading.Name = "label_heading";
            this.label_heading.Size = new Size(50, 13);
            this.label_heading.TabIndex = 11;
            this.label_heading.Text = "Heading:";
            this.label_gpsTime.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_gpsTime.AutoSize = true;
            this.label_gpsTime.Location = new Point(0xb2, 0x15);
            this.label_gpsTime.Name = "label_gpsTime";
            this.label_gpsTime.Size = new Size(0x5c, 13);
            this.label_gpsTime.TabIndex = 10;
            this.label_gpsTime.Text = "TOW:        Week:";
            this.label_mode.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_mode.AutoSize = true;
            this.label_mode.Location = new Point(0xb2, 0x61);
            this.label_mode.Name = "label_mode";
            this.label_mode.Size = new Size(0x25, 13);
            this.label_mode.TabIndex = 9;
            this.label_mode.Text = "Mode:";
            this.label_fix.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_fix.AutoSize = true;
            this.label_fix.Location = new Point(7, 0x61);
            this.label_fix.Name = "label_fix";
            this.label_fix.Size = new Size(0x17, 13);
            this.label_fix.TabIndex = 8;
            this.label_fix.Text = "Fix:";
            this.label_fix.TextAlign = ContentAlignment.MiddleRight;
            this.label_fix.Visible = false;
            this.label_totalSVsUsed.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_totalSVsUsed.AutoSize = true;
            this.label_totalSVsUsed.Location = new Point(7, 0x7a);
            this.label_totalSVsUsed.Name = "label_totalSVsUsed";
            this.label_totalSVsUsed.Size = new Size(0x6b, 13);
            this.label_totalSVsUsed.TabIndex = 7;
            this.label_totalSVsUsed.Text = "Number of SVs used:";
            this.label_Speed.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_Speed.AutoSize = true;
            this.label_Speed.Location = new Point(0xb2, 70);
            this.label_Speed.Name = "label_Speed";
            this.label_Speed.Size = new Size(0x29, 13);
            this.label_Speed.TabIndex = 6;
            this.label_Speed.Text = "Speed:";
            this.label_HDOP.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_HDOP.AutoSize = true;
            this.label_HDOP.Location = new Point(7, 70);
            this.label_HDOP.Name = "label_HDOP";
            this.label_HDOP.Size = new Size(0x29, 13);
            this.label_HDOP.TabIndex = 5;
            this.label_HDOP.Text = "HDOP:";
            this.label_Altitude.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_Altitude.AutoSize = true;
            this.label_Altitude.Location = new Point(0x135, 0x2c);
            this.label_Altitude.Name = "label_Altitude";
            this.label_Altitude.Size = new Size(0x2d, 13);
            this.label_Altitude.TabIndex = 3;
            this.label_Altitude.Text = "Altitude:";
            this.label_Latitude.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_Latitude.AutoSize = true;
            this.label_Latitude.Location = new Point(7, 0x2c);
            this.label_Latitude.Name = "label_Latitude";
            this.label_Latitude.Size = new Size(0x30, 13);
            this.label_Latitude.TabIndex = 2;
            this.label_Latitude.Text = "Latitude:";
            this.label_Longitude.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_Longitude.AutoSize = true;
            this.label_Longitude.Location = new Point(0xb2, 0x2c);
            this.label_Longitude.Name = "label_Longitude";
            this.label_Longitude.Size = new Size(0x39, 13);
            this.label_Longitude.TabIndex = 1;
            this.label_Longitude.Text = "Longitude:";
            this.label_ReceiverTime.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label_ReceiverTime.AutoSize = true;
            this.label_ReceiverTime.Location = new Point(7, 0x15);
            this.label_ReceiverTime.Name = "label_ReceiverTime";
            this.label_ReceiverTime.Size = new Size(0x4f, 13);
            this.label_ReceiverTime.TabIndex = 0;
            this.label_ReceiverTime.Text = "Receiver Time:";
            this.MapRightClickContextMenuStrip.Items.AddRange(new ToolStripItem[] { this.setMapCenterLocationHereToolStripMenuItem });
            this.MapRightClickContextMenuStrip.Name = "MapRightClickContextMenuStrip";
            this.MapRightClickContextMenuStrip.Size = new Size(0xcb, 0x1a);
            this.setMapCenterLocationHereToolStripMenuItem.Name = "setMapCenterLocationHereToolStripMenuItem";
            this.setMapCenterLocationHereToolStripMenuItem.Size = new Size(0xca, 0x16);
            this.setMapCenterLocationHereToolStripMenuItem.Text = "Switch to Manual Center";
            this.setMapCenterLocationHereToolStripMenuItem.Click += new EventHandler(this.setMapCenterLocationHereToolStripMenuItem_Click);
            this.LatLongLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.LatLongLabel.AutoSize = true;
            this.LatLongLabel.Location = new Point(0x1b, 0xc3);
            this.LatLongLabel.Name = "LatLongLabel";
            this.LatLongLabel.Size = new Size(0x3a, 13);
            this.LatLongLabel.TabIndex = 15;
            this.LatLongLabel.Text = "Lat, Long: ";
            this.locationViewToolStrip.Items.AddRange(new ToolStripItem[] { this.locationViewToolbarMapitBtn, this.locationViewToolbarSetRadiusBtn, this.locationViewToolbarClearBtn, this.toolStripButton_setRefLocation });
            this.locationViewToolStrip.Location = new Point(0, 0);
            this.locationViewToolStrip.Name = "locationViewToolStrip";
            this.locationViewToolStrip.Size = new Size(0x1e5, 0x19);
            this.locationViewToolStrip.TabIndex = 0x12;
            this.locationViewToolStrip.Text = "toolStrip1";
            this.locationViewToolbarMapitBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.locationViewToolbarMapitBtn.Image = Resources.mapIt;
            this.locationViewToolbarMapitBtn.ImageTransparentColor = Color.Magenta;
            this.locationViewToolbarMapitBtn.Name = "locationViewToolbarMapitBtn";
            this.locationViewToolbarMapitBtn.Size = new Size(0x17, 0x16);
            this.locationViewToolbarMapitBtn.Text = "Map Position";
            this.locationViewToolbarMapitBtn.Click += new EventHandler(this.button_MapIt_Click);
            this.locationViewToolbarSetRadiusBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.locationViewToolbarSetRadiusBtn.Image = Resources.Config;
            this.locationViewToolbarSetRadiusBtn.ImageTransparentColor = Color.Magenta;
            this.locationViewToolbarSetRadiusBtn.Name = "locationViewToolbarSetRadiusBtn";
            this.locationViewToolbarSetRadiusBtn.Size = new Size(0x17, 0x16);
            this.locationViewToolbarSetRadiusBtn.Text = "Configuration";
            this.locationViewToolbarSetRadiusBtn.Click += new EventHandler(this.locationViewToolbarSetRadiusBtn_Click);
            this.locationViewToolbarClearBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.locationViewToolbarClearBtn.Image = Resources.DeleteHS;
            this.locationViewToolbarClearBtn.ImageTransparentColor = Color.Magenta;
            this.locationViewToolbarClearBtn.Name = "locationViewToolbarClearBtn";
            this.locationViewToolbarClearBtn.Size = new Size(0x17, 0x16);
            this.locationViewToolbarClearBtn.Text = "Clear Data";
            this.locationViewToolbarClearBtn.Click += new EventHandler(this.frmCommLocationMapClearBtn_Click);
            this.toolStripButton_setRefLocation.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton_setRefLocation.Image = Resources.RefLocation;
            this.toolStripButton_setRefLocation.ImageTransparentColor = Color.Magenta;
            this.toolStripButton_setRefLocation.Name = "toolStripButton_setRefLocation";
            this.toolStripButton_setRefLocation.Size = new Size(0x17, 0x16);
            this.toolStripButton_setRefLocation.Text = "Set Reference Location";
            this.toolStripButton_setRefLocation.Click += new EventHandler(this.toolStripButton_setRefLocation_Click);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.locationViewStatusLabel });
            this.statusStrip1.Location = new Point(0, 0x1b9);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x1e5, 0x16);
            this.statusStrip1.TabIndex = 0x13;
            this.statusStrip1.Text = "Status";
            this.locationViewStatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.locationViewStatusLabel.Name = "locationViewStatusLabel";
            this.locationViewStatusLabel.Size = new Size(0x26, 0x11);
            this.locationViewStatusLabel.Text = "Status";
            this.panel_Location.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel_Location.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_Location.Location = new Point(0x16, 0xd9);
            this.panel_Location.Name = "panel_Location";
            this.panel_Location.Size = new Size(0x1b9, 210);
            this.panel_Location.TabIndex = 7;
            this.panel_Location.Paint += new PaintEventHandler(this.panel_Location_Paint);
            this.panel_Location.MouseMove += new MouseEventHandler(this.panel_Location_MouseMove);
            this.panel_Location.MouseClick += new MouseEventHandler(this.panel_Location_MouseClick);
            this.panel_Location.Resize += new EventHandler(this.panel_Location_Resize);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = SystemColors.Control;
            base.ClientSize = new Size(0x1e5, 0x1cf);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.locationViewToolStrip);
            base.Controls.Add(this.groupBox7);
            base.Controls.Add(this.panel_Location);
            base.Controls.Add(this.LatLongLabel);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmCommLocationMap";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Location View";
            base.Load += new EventHandler(this.frmCommLocationMap_Load);
            base.LocationChanged += new EventHandler(this.frmCommLocationMap_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommLocationMap_ResizeEnd);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.MapRightClickContextMenuStrip.ResumeLayout(false);
            this.locationViewToolStrip.ResumeLayout(false);
            this.locationViewToolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void locationViewToolbarSetRadiusBtn_Click(object sender, EventArgs e)
        {
            frmConfigLocationView view = new frmConfigLocationView();
            view.UpdateParentParams += new frmConfigLocationView.UpdateWindowEventHandler(this.updateConfiguration);
            view.IsAutoCenter = !this.FixedLocationFlag;
            view.Radius = (uint) this.comm.LocationMapRadius;
            view.IsIMUAvailable = this.comm.IMUPositionAvailable;
            view.IMUFilepath = this.comm.IMUFilePath;
            view.Lat = this.EnteredFixedLatitude;
            view.Lon = this.EnteredFixedLongitude;
            view.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.EnableLocationMapView = false;
                if (this._SiRFMap != null)
                {
                    this._SiRFMap.Close();
                }
                if (this.updateMainWindow != null)
                {
                    this.updateMainWindow(base.Name);
                }
                if (this.UpdatePortManager != null)
                {
                    this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
                }
            }
            base.OnClosed(e);
        }

        private void panel_Location_MouseClick(object sender, MouseEventArgs e)
        {
            double num;
            double num2;
            this.RightMouseClick_X = e.X;
            this.RightMouseClick_Y = e.Y;
            this.ConvertMousePositionToLatLong(e.X, e.Y, out num, out num2);
            if (e.Button == MouseButtons.Right)
            {
                this.EnteredFixedLatitude = num;
                this.EnteredFixedLongitude = num2;
                this.MapRightClickContextMenuStrip.Show(this.panel_Location, e.X, e.Y);
                if (this.FixedLocationFlag)
                {
                    this.setMapCenterLocationHereToolStripMenuItem.Text = "Switch to Auto Center";
                }
                else
                {
                    this.setMapCenterLocationHereToolStripMenuItem.Text = "Switch to Manual Center";
                }
            }
            else if (!this.FixedLocationFlag)
            {
                this.EnteredFixedLatitude = num;
                this.EnteredFixedLongitude = num2;
            }
            this.updateStatusString();
        }

        private void panel_Location_MouseMove(object sender, MouseEventArgs e)
        {
            double num;
            double num2;
            this.ConvertMousePositionToLatLong(e.X, e.Y, out num, out num2);
            string str = string.Format("Lat:{0:F6}, Lng:{1:F6}", num, num2);
            this.LatLongLabel.Text = str;
        }

        private void panel_Location_Paint(object sender, PaintEventArgs e)
        {
            int height = this.panel_Location.Height;
            int width = this.panel_Location.Width;
            int y = height;
            int x = width;
            if (width > height)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            this.PanelHeightWidth = height;
            using (BufferedGraphics graphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
            {
                Graphics graphics2 = graphics.Graphics;
                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(x, y), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 230));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0, 0, x, y);
                graphics2.DrawLine(Pens.Gray, 0, height / 2, width, height / 2);
                graphics2.DrawLine(Pens.Gray, width / 2, 0, width / 2, height);
                graphics2.DrawEllipse(Pens.Gray, 0, 0, width, height);
                graphics2.DrawEllipse(Pens.Gray, (int) (width / 4), (int) (height / 4), (int) (width / 2), (int) (height / 2));
                if ((this.comm.dataGui.Positions.PositionList.Count > 0) || (this.comm.dataGui.TruePositions.PositionList.Count > 0))
                {
                    double num5 = 0.0;
                    double d = 0.0;
                    double num7 = 1.0;
                    double num8 = 0.0;
                    double num9 = 0.0;
                    if (this.comm.dataGui.TruePositions.PositionList.Count > 0)
                    {
                        num5 = (((PositionInfo.PositionStruct) this.comm.dataGui.TruePositions.PositionList[this.comm.dataGui.TruePositions.PositionList.Count - 1]).Longitude * 3.1415926535897931) / 180.0;
                        d = (((PositionInfo.PositionStruct) this.comm.dataGui.TruePositions.PositionList[this.comm.dataGui.TruePositions.PositionList.Count - 1]).Latitude * 3.1415926535897931) / 180.0;
                    }
                    if (this.comm.dataGui.Positions.PositionList.Count > 0)
                    {
                        if (!this.FixedLocationFlag)
                        {
                            if (num5 != 0.0)
                            {
                                this.CenterLongitude = num5;
                            }
                            else
                            {
                                this.CenterLongitude = (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Longitude * 3.1415926535897931) / 180.0;
                            }
                            if (d != 0.0)
                            {
                                this.CenterLatitude = d;
                            }
                            else
                            {
                                this.CenterLatitude = (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Latitude * 3.1415926535897931) / 180.0;
                            }
                        }
                        else
                        {
                            this.CenterLatitude = (this.EnteredFixedLatitude * 3.1415926535897931) / 180.0;
                            this.CenterLongitude = (this.EnteredFixedLongitude * 3.1415926535897931) / 180.0;
                        }
                    }
                    num7 = this._locationMapRadius;
                    StringFormat format = new StringFormat();
                    format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    graphics2.DrawString(string.Format("R = {0:F2}m", num7), new Font("Segoe UI", 9f), Brushes.Gray, width * 1f, ((float) height) / 2f, format);
                    for (int i = 0; i < this.comm.dataGui.TruePositions.PositionList.Count; i++)
                    {
                        try
                        {
                            if (num7 == 0.0)
                            {
                                num7 = 1.0;
                            }
                            num8 = (((PositionInfo.PositionStruct) this.comm.dataGui.TruePositions.PositionList[i]).Longitude * 3.1415926535897931) / 180.0;
                            num9 = (((PositionInfo.PositionStruct) this.comm.dataGui.TruePositions.PositionList[i]).Latitude * 3.1415926535897931) / 180.0;
                            float num11 = ((float) (((((6378137.0 * Math.Acos(((Math.Cos(d) * Math.Cos(d)) * Math.Cos(num8 - num5)) + (Math.Sin(d) * Math.Sin(d)))) * Math.Sign((double) (num8 - num5))) * 0.5) * width) / num7)) - 1f;
                            float num12 = ((float) (((((6378137.0 * Math.Acos((Math.Cos(num9) * Math.Cos(d)) + (Math.Sin(num9) * Math.Sin(d)))) * Math.Sign((double) (num9 - d))) * 0.5) * height) / num7)) - 1f;
                            graphics2.FillEllipse(Brushes.Black, (float) ((width / 2) + num11), (float) ((height / 2) - num12), (float) 2f, (float) 2f);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    for (int j = 0; j < this.comm.dataGui.Positions.PositionList.Count; j++)
                    {
                        try
                        {
                            if (num7 == 0.0)
                            {
                                num7 = 1.0;
                            }
                            num8 = (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[j]).Longitude * 3.1415926535897931) / 180.0;
                            num9 = (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[j]).Latitude * 3.1415926535897931) / 180.0;
                            float num14 = ((float) (((((6378137.0 * Math.Acos(((Math.Cos(this.CenterLatitude) * Math.Cos(this.CenterLatitude)) * Math.Cos(num8 - this.CenterLongitude)) + (Math.Sin(this.CenterLatitude) * Math.Sin(this.CenterLatitude)))) * Math.Sign((double) (num8 - this.CenterLongitude))) * 0.5) * width) / num7)) - 1f;
                            float num15 = ((float) (((((6378137.0 * Math.Acos((Math.Cos(num9) * Math.Cos(this.CenterLatitude)) + (Math.Sin(num9) * Math.Sin(this.CenterLatitude)))) * Math.Sign((double) (num9 - this.CenterLatitude))) * 0.5) * height) / num7)) - 1f;
                            if (j == (this.comm.dataGui.Positions.PositionList.Count - 1))
                            {
                                graphics2.FillEllipse(Brushes.Red, (float) ((width / 2) + num14), (float) ((height / 2) - num15), (float) 2f, (float) 2f);
                            }
                            else
                            {
                                graphics2.FillEllipse(Brushes.Blue, (float) ((width / 2) + num14), (float) ((height / 2) - num15), (float) 2f, (float) 2f);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    this.PanelCircleRadius = (int) num7;
                    if (this.comm.dataGui.Positions.PositionList.Count > 0)
                    {
                        int num16 = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).RxTime_Hour;
                        int num17 = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).RxTime_Minute;
                        ushort num18 = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).RxTime_second;
                        double longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Longitude;
                        double latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Latitude;
                        double altitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Altitude;
                        uint satellitesUsed = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).SatellitesUsed;
                        double hDOP = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).HDOP;
                        double speed = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Speed;
                        double heading = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).Heading;
                        ushort navValid = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavValid;
                        ushort navType = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavType;
                        double tOW = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).TOW;
                        ushort extWeek = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).ExtWeek;
                        this._lat = latitude.ToString();
                        this._lon = longitude.ToString();
                        this.label_ReceiverTime.Text = string.Format("Receiver Time(UTC): {0:00}:{1:00}:{2:00}", num16, num17, ((double) num18) / 1000.0);
                        this.label_Latitude.Text = string.Format("Latitude: {0:F6}", latitude);
                        this.label_Longitude.Text = string.Format("Longitude:{0:F6}", longitude);
                        this.label_Altitude.Text = string.Format("Altitude: {0:F2} m", altitude);
                        int num29 = 0;
                        if (this.comm._rxType == CommunicationManager.ReceiverType.NMEA)
                        {
                            this.label_totalSVsUsed.Text = string.Format("Number of SVs used in Fix: {0}", satellitesUsed);
                        }
                        else
                        {
                            StringBuilder builder = new StringBuilder();
                            for (int k = 1; k < this.comm.dataGui.PRN_Arr_PRNforSolution.Length; k++)
                            {
                                if (this.comm.dataGui.PRN_Arr_PRNforSolution[k] > 0)
                                {
                                    num29++;
                                    builder.AppendFormat("{0} ", k);
                                }
                            }
                            if (num29 > 0)
                            {
                                this.label_totalSVsUsed.Text = string.Format("Number of SVs used in Fix: {0,-11}( {1})", num29, builder.ToString());
                            }
                            else
                            {
                                this.label_totalSVsUsed.Text = string.Format("Number of SVs used in Fix: {0}", num29);
                            }
                        }
                        string str = "N/A";
                        string str2 = "N/A";
                        if (tOW != 0.0)
                        {
                            str = string.Format("{0:F2}", tOW / 1000.0);
                        }
                        if (extWeek != 0)
                        {
                            str2 = extWeek.ToString();
                        }
                        this.label_gpsTime.Text = string.Format("TOW: {0,-25}Ext. Week: {1}", str, str2);
                        this.label_HDOP.Text = string.Format("HDOP: {0:F2}", hDOP);
                        this.label_Speed.Text = string.Format("Speed: {0:F2} m/s", speed);
                        if (navValid == 0)
                        {
                            this.label_fix.Text = "Fix Valid";
                        }
                        else
                        {
                            this.label_fix.Text = "Fix Invalid";
                        }
                        this.label_heading.Text = string.Format("Heading: {0:F2}\x00b0", heading);
                        this.label_mode.Text = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavModeString;
                    }
                    graphics.Render(e.Graphics);
                }
            }
        }

        private void panel_Location_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_Location.Refresh();
        }

        private void setMapCenterLocationHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FixedLocationFlag = !this.FixedLocationFlag;
            if (this.FixedLocationFlag)
            {
                double num;
                double num2;
                this.ConvertMousePositionToLatLong(this.RightMouseClick_X, this.RightMouseClick_Y, out num, out num2);
                this.CenterLatitude = num * 0.017453292519944444;
                this.CenterLongitude = num2 * 0.017453292519944444;
            }
            this.updateStatusString();
        }

        private void toolStripButton_setRefLocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.comm.IsSourceDeviceOpen())
                {
                    frmSetReferenceLocation location = new frmSetReferenceLocation();
                    location.CommWindow = this.comm;
                    location.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not open -- action failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void updateConfiguration(uint type, uint radius, double lat, double lon, bool isAutoCenter, bool isIMUAvailable, string imuPath)
        {
            switch (type)
            {
                case 1:
                    this.LocationMapRadius = radius;
                    this.comm.LocationMapRadius = radius;
                    break;

                case 2:
                    this.FixedLocationFlag = !isAutoCenter;
                    if (!isAutoCenter)
                    {
                        this.EnteredFixedLatitude = lat;
                        this.EnteredFixedLongitude = lon;
                    }
                    break;

                case 3:
                    this.comm.IMUPositionAvailable = isIMUAvailable;
                    if (isIMUAvailable)
                    {
                        this.comm.IMUFilePath = imuPath;
                    }
                    break;
            }
            this.updateStatusString();
        }

        private void updateStatusString()
        {
            string str = "Status";
            if (!this.FixedLocationFlag)
            {
                str = "Auto Center";
            }
            else
            {
                str = string.Format("Center at (Lat:{0:F6},Lon:{1:F6})", this.EnteredFixedLatitude, this.EnteredFixedLongitude);
            }
            if (this.comm.IMUPositionAvailable)
            {
                str = str + "-- IMU data available";
            }
            this.locationViewStatusLabel.Text = str;
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
                this.comm.DisplayPanelLocation = this.panel_Location;
                this.comm.EnableLocationMapView = true;
                if (this.comm.IMUPositionAvailable)
                {
                    this.comm.GetIMUDataForGUI();
                }
                this.Text = this.comm.sourceDeviceName + ": Location View";
                this._locationMapRadius = this.comm.LocationMapRadius;
                this.updateStatusString();
            }
        }

        public double LocationMapRadius
        {
            get
            {
                return this._locationMapRadius;
            }
            set
            {
                this._locationMapRadius = value;
                if (this._locationMapRadius == 0.0)
                {
                    this._locationMapRadius = 1.0;
                }
            }
        }

        public string PersistedWindowName
        {
            get
            {
                return this._persistedWindowName;
            }
            set
            {
                this._persistedWindowName = value;
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

