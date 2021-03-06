﻿namespace SiRFLive.GUI
{
    using SiRFLive.Communication;
    using SiRFLive.MessageHandling;
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class frmMapIt : Form
    {
        private string _html_file;
        private StringBuilder _htmlend;
        private StringBuilder _htmlFixes;
        private StringBuilder _htmlhead;
        private double _last_lat;
        private double _last_lon;
        private string _lat;
        private double[] _lats;
        private int _length;
        private string _lon;
        private double[] _lons;
        private CommunicationManager comm;
        private IContainer components;
        private ToolStripButton frmMapitMapBtn;
        private ToolStripButton frmMapitStreetViewBtn;
        private ToolStripButton frmMapitTrackBtn;
        private bool IsRealTime;
        private static frmMapIt m_SChildform;
        private PageSetupDialog pageSetupDialog1;
        private Panel panel1;
        private ToolStrip toolStrip1;
        private WebBrowser webBrowser1;

        public frmMapIt()
        {
            this._html_file = string.Empty;
            this.IsRealTime = true;
            this.InitializeComponent();
        }

        public frmMapIt(CommunicationManager mainComWin)
        {
            this._html_file = string.Empty;
            this.IsRealTime = true;
            this.InitializeComponent();
            this.CommWindow = mainComWin;
        }

        public frmMapIt(int length, double[] lats, double[] lons)
        {
            this._html_file = string.Empty;
            this.IsRealTime = false;
            this._length = length;
            this._lats = new double[length];
            this._lons = new double[length];
            for (int i = 0; i < length; i++)
            {
                this._lats[i] = lats[i];
                this._lons[i] = lons[i];
            }
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMapIt_Load(object sender, EventArgs e)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                MessageBox.Show("No internet connection");
            }
            else if (this.IsRealTime)
            {
                this.frmMapitMapBtn.Enabled = true;
                this.frmMapitStreetViewBtn.Enabled = true;
                this.frmMapitTrackBtn.Enabled = true;
                int count = this.comm.dataGui.Positions.PositionList.Count;
                if (count > 0)
                {
                    double latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1]).Latitude;
                    double longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1]).Longitude;
                    if ((Math.Abs(latitude) > 0.05) && (Math.Abs(longitude) > 0.05))
                    {
                        this.mapIt(latitude.ToString(), longitude.ToString());
                    }
                }
            }
            else
            {
                this.frmMapitMapBtn.Enabled = false;
                this.frmMapitStreetViewBtn.Enabled = false;
                this.frmMapitTrackBtn.Enabled = false;
                this.plotTrack();
            }
        }

        private void frmMapIt_Resize(object sender, EventArgs e)
        {
            this.panel1.Width = base.Width - 20;
            this.panel1.Height = base.Height - 0x4d;
        }

        private void frmMapitMapBtn_Click(object sender, EventArgs e)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                MessageBox.Show("No internet connection !", "Info:", MessageBoxButtons.OK);
            }
            else
            {
                int count = this.comm.dataGui.Positions.PositionList.Count;
                if (count > 0)
                {
                    double latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1]).Latitude;
                    double longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1]).Longitude;
                    if ((Math.Abs(latitude) > 0.05) && (Math.Abs(longitude) > 0.05))
                    {
                        this.mapIt(latitude.ToString(), longitude.ToString());
                    }
                }
            }
        }

        private void frmMapitStreetViewBtn_Click(object sender, EventArgs e)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                MessageBox.Show("No internet connection !", "Info:", MessageBoxButtons.OK);
            }
            else
            {
                int count = this.comm.dataGui.Positions.PositionList.Count;
                if (count > 1)
                {
                    double latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1]).Latitude;
                    double longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1]).Longitude;
                    double headingDegrees = this.comm.dataGui.HeadingDegrees;
                    double num5 = this.comm.dataGui.PitchDegrees * -1.0;
                    string path = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\html\pov.html";
                    string newValue = string.Format("myLOC = new GLatLng({0:F7},{1:F7})", latitude, longitude);
                    string str3 = "myPOV = {" + string.Format("yaw:{0:F7},pitch:{1:F7}", headingDegrees, num5) + "}";
                    string str4 = string.Empty;
                    try
                    {
                        StringBuilder builder = new StringBuilder();
                        StreamReader reader = File.OpenText(path);
                        builder.Append(reader.ReadToEnd());
                        reader.Close();
                        string input = builder.ToString();
                        Regex regex = new Regex(@"myLOC = new GLatLng\([+-]*[0-9]+.[0-9]+,[+-]*[0-9]+.[0-9]+\)");
                        string oldValue = regex.Match(input).ToString();
                        str4 = input.Replace(oldValue, newValue);
                        regex = new Regex(@"myPOV = \{yaw:[+-]*[0-9]+.[0-9]+,pitch:[+-]*[0-9]+.[0-9]+\}");
                        oldValue = regex.Match(input).ToString();
                        str4 = str4.Replace(oldValue, str3);
                        StreamWriter writer = new StreamWriter(path);
                        writer.Write(str4);
                        writer.Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message.ToString(), "Error");
                    }
                    try
                    {
                        StringBuilder builder2 = new StringBuilder();
                        builder2.Append(path);
                        this.webBrowser1.Navigate(builder2.ToString());
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show(exception2.Message.ToString(), "Error");
                    }
                }
            }
        }

        private void frmMapitTrackBtn_Click(object sender, EventArgs e)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                MessageBox.Show("No internet connection !", "Info:", MessageBoxButtons.OK);
            }
            else
            {
                this.plotTrack();
            }
        }

        public static frmMapIt GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmMapIt();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMapIt));
            this.panel1 = new Panel();
            this.webBrowser1 = new WebBrowser();
            this.pageSetupDialog1 = new PageSetupDialog();
            this.toolStrip1 = new ToolStrip();
            this.frmMapitMapBtn = new ToolStripButton();
            this.frmMapitStreetViewBtn = new ToolStripButton();
            this.frmMapitTrackBtn = new ToolStripButton();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Location = new Point(12, 0x2b);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x394, 0x273);
            this.panel1.TabIndex = 0;
            this.webBrowser1.Dock = DockStyle.Fill;
            this.webBrowser1.Location = new Point(0, 0);
            this.webBrowser1.MinimumSize = new Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new Size(0x394, 0x273);
            this.webBrowser1.TabIndex = 0;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.frmMapitMapBtn, this.frmMapitStreetViewBtn, this.frmMapitTrackBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(940, 0x19);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            this.frmMapitMapBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmMapitMapBtn.Image = Resources.mapIt;
            this.frmMapitMapBtn.ImageTransparentColor = Color.Magenta;
            this.frmMapitMapBtn.Name = "frmMapitMapBtn";
            this.frmMapitMapBtn.Size = new Size(0x17, 0x16);
            this.frmMapitMapBtn.Text = "Show Map";
            this.frmMapitMapBtn.Click += new EventHandler(this.frmMapitMapBtn_Click);
            this.frmMapitStreetViewBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmMapitStreetViewBtn.Image = Resources.streetViewHS;
            this.frmMapitStreetViewBtn.ImageTransparentColor = Color.Magenta;
            this.frmMapitStreetViewBtn.Name = "frmMapitStreetViewBtn";
            this.frmMapitStreetViewBtn.Size = new Size(0x17, 0x16);
            this.frmMapitStreetViewBtn.Text = "Street View - click to refresh";
            this.frmMapitStreetViewBtn.Click += new EventHandler(this.frmMapitStreetViewBtn_Click);
            this.frmMapitTrackBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmMapitTrackBtn.Image = Resources.TrackHS;
            this.frmMapitTrackBtn.ImageTransparentColor = Color.Magenta;
            this.frmMapitTrackBtn.Name = "frmMapitTrackBtn";
            this.frmMapitTrackBtn.Size = new Size(0x17, 0x16);
            this.frmMapitTrackBtn.Text = "Track View";
            this.frmMapitTrackBtn.Click += new EventHandler(this.frmMapitTrackBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(940, 0x2aa);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.panel1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmMapIt";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "frmMapIt";
            base.Resize += new EventHandler(this.frmMapIt_Resize);
            base.Load += new EventHandler(this.frmMapIt_Load);
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void mapIt()
        {
            int count = 0;
            if (this.IsRealTime)
            {
                count = this.comm.dataGui.Positions.PositionList.Count;
            }
            else
            {
                count = this._length;
            }
            double latitude = 0.0;
            double longitude = 0.0;
            if (count >= 1)
            {
                this._htmlFixes = new StringBuilder();
                this._htmlFixes.Append("var point = new GLatLng();\n");
                int index = 0;
                if (this.IsRealTime)
                {
                    latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[index]).Latitude;
                    longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[index]).Longitude;
                }
                else
                {
                    latitude = this._lats[index];
                    longitude = this._lons[index];
                }
                while (((Math.Abs(latitude) < 0.05) || (Math.Abs(longitude) < 0.05)) && (index < (count - 1)))
                {
                    if (this.IsRealTime)
                    {
                        latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[index]).Latitude;
                        longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[index]).Longitude;
                    }
                    else
                    {
                        latitude = this._lats[index];
                        longitude = this._lons[index];
                    }
                    index++;
                }
                if (index < count)
                {
                    this._htmlFixes.AppendFormat("point = new GLatLng({0},{1});\n", latitude, longitude);
                    this._htmlFixes.Append("map.setCenter(point, 18);\n");
                    this._htmlFixes.Append("map.addOverlay(new GMarker(point, Icon));\n");
                    this._last_lat = latitude;
                    this._last_lon = longitude;
                    for (int i = index; i < count; i++)
                    {
                        if (this.IsRealTime)
                        {
                            latitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[i]).Latitude;
                            longitude = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[i]).Longitude;
                        }
                        else
                        {
                            latitude = this._lats[i];
                            longitude = this._lons[i];
                        }
                        if ((Math.Abs((double) (latitude - this._last_lat)) > 5E-05) || (Math.Abs((double) (longitude - this._last_lon)) > 5E-05))
                        {
                            this._last_lat = latitude;
                            this._last_lon = longitude;
                            this._htmlFixes.AppendFormat("point = new GLatLng({0},{1});\n", latitude, longitude);
                            this._htmlFixes.Append("map.addOverlay(new GMarker(point, Icon));\n");
                        }
                    }
                    string str = this._htmlhead.ToString() + this._htmlFixes.ToString() + this._htmlend.ToString();
                    try
                    {
                        StreamWriter writer = new StreamWriter(this._html_file);
                        writer.Write(str);
                        writer.Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message.ToString(), "Error");
                    }
                    try
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(this._html_file);
                        this.webBrowser1.Navigate(builder.ToString());
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show(exception2.Message.ToString(), "Error");
                    }
                }
            }
        }

        public void mapIt(string lat, string lon)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("http://maps.google.com/maps?q=");
                if (lat != string.Empty)
                {
                    builder.Append(lat + "%2C");
                }
                if (lon != string.Empty)
                {
                    builder.Append(lon);
                }
                this.webBrowser1.Navigate(builder.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString(), "Error");
            }
        }

        private void plotTrack()
        {
            if (this.IsRealTime)
            {
                if (this.comm.dataGui.Positions.PositionList.Count <= 1)
                {
                    return;
                }
            }
            else if (this._length <= 1)
            {
                return;
            }
            string path = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\html\write1.TXT";
            this._html_file = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\html\track.html";
            try
            {
                this._htmlhead = new StringBuilder();
                StreamReader reader = File.OpenText(path);
                this._htmlhead.Append(reader.ReadToEnd());
                reader.Close();
                this._htmlend = new StringBuilder();
                this._htmlend.Append("\n}\n</script>\n</head>\n<body onload=\"initialize()\" onunload=\"GUnload()\">");
                this._htmlend.AppendFormat("\n<div id=\"map_canvas\" style=\"width: {0}px; height:{1}px\"></div>\n </body>\n</html>", this.webBrowser1.Width, this.webBrowser1.Height);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString(), "Error");
            }
            this.mapIt();
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
                this.Text = this.comm.sourceDeviceName + "Google Map";
            }
        }

        public string Latitude
        {
            get
            {
                return this._lat;
            }
            set
            {
                this._lat = value;
            }
        }

        public string Longitude
        {
            get
            {
                return this._lon;
            }
            set
            {
                this._lon = value;
            }
        }
    }
}

