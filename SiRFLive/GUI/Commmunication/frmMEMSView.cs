﻿namespace SiRFLive.GUI.Commmunication
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmMEMSView : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label_MEMSView;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label lblXvalue_Acc;
        private Label lblXvalue_Mag;
        private Label lblYvalue_Acc;
        private Label lblYvalue_Mag;
        private Label lblZvalue_Acc;
        private Label lblZvalue_Mag;
        private MyPanel myPanel;
        private MyPanel panel_MEMS;
        public int tb_maxRange;
        public int tb_minRange;
        private TrackBar trackBarXvalue_Acc;
        private TrackBar trackBarXvalue_Mag;
        private TrackBar trackBarYvalue_Acc;
        private TrackBar trackBarYvalue_Mag;
        private TrackBar trackBarZvalue_Acc;
        private TrackBar trackBarZvalue_Mag;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;
        private int xvalue_Acc;
        private int xvalue_Mag;
        private int yvalue_Acc;
        private int yvalue_Mag;
        private int zvalue_Acc;
        private int zvalue_Mag;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmMEMSView()
        {
            this.tb_minRange = -2048;
            this.tb_maxRange = 0x7ff;
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        public frmMEMSView(CommunicationManager mainComWin)
        {
            this.tb_minRange = -2048;
            this.tb_maxRange = 0x7ff;
            this.MEMS_setup();
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
            this.CommWindow = mainComWin;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public static short convertHiLow2value(byte hiValue, byte lowValue)
        {
            if ((hiValue & 8) != 0)
            {
                hiValue = (byte) (hiValue | 240);
            }
            return (short) ((hiValue << 8) | lowValue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMEMSView_Load(object sender, EventArgs e)
        {
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
            this.updateMEMS_XYZvalues();
        }

        private void frmMEMSView_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmMEMSView_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMEMSView));
            this.label_MEMSView = new Label();
            this.trackBarXvalue_Mag = new TrackBar();
            this.trackBarYvalue_Mag = new TrackBar();
            this.trackBarZvalue_Mag = new TrackBar();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.lblXvalue_Mag = new Label();
            this.lblYvalue_Mag = new Label();
            this.lblZvalue_Mag = new Label();
            this.groupBox1 = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.lblZvalue_Acc = new Label();
            this.lblYvalue_Acc = new Label();
            this.lblXvalue_Acc = new Label();
            this.trackBarZvalue_Acc = new TrackBar();
            this.trackBarYvalue_Acc = new TrackBar();
            this.trackBarXvalue_Acc = new TrackBar();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.panel_MEMS = new MyPanel();
            this.label8 = new Label();
            this.label7 = new Label();
            this.trackBarXvalue_Mag.BeginInit();
            this.trackBarYvalue_Mag.BeginInit();
            this.trackBarZvalue_Mag.BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.trackBarZvalue_Acc.BeginInit();
            this.trackBarYvalue_Acc.BeginInit();
            this.trackBarXvalue_Acc.BeginInit();
            this.panel_MEMS.SuspendLayout();
            base.SuspendLayout();
            this.label_MEMSView.AutoSize = true;
            this.label_MEMSView.Location = new Point(0x5d, 6);
            this.label_MEMSView.Name = "label_MEMSView";
            this.label_MEMSView.Size = new Size(0x23, 13);
            this.label_MEMSView.TabIndex = 0;
            this.label_MEMSView.Text = "State:";
            this.trackBarXvalue_Mag.Anchor = AnchorStyles.None;
            this.trackBarXvalue_Mag.BackColor = SystemColors.Control;
            this.trackBarXvalue_Mag.Enabled = false;
            this.trackBarXvalue_Mag.LargeChange = 0;
            this.trackBarXvalue_Mag.Location = new Point(0x24, 0x16);
            this.trackBarXvalue_Mag.Maximum = 0x7ff;
            this.trackBarXvalue_Mag.Minimum = -2048;
            this.trackBarXvalue_Mag.Name = "trackBarXvalue_Mag";
            this.trackBarXvalue_Mag.Size = new Size(0xd6, 0x2a);
            this.trackBarXvalue_Mag.TabIndex = 2;
            this.trackBarXvalue_Mag.TabStop = false;
            this.trackBarXvalue_Mag.TickFrequency = 0x800;
            this.trackBarYvalue_Mag.Anchor = AnchorStyles.None;
            this.trackBarYvalue_Mag.BackColor = SystemColors.Control;
            this.trackBarYvalue_Mag.Enabled = false;
            this.trackBarYvalue_Mag.LargeChange = 0;
            this.trackBarYvalue_Mag.Location = new Point(0x24, 0x3b);
            this.trackBarYvalue_Mag.Maximum = 0x7ff;
            this.trackBarYvalue_Mag.Minimum = -2048;
            this.trackBarYvalue_Mag.Name = "trackBarYvalue_Mag";
            this.trackBarYvalue_Mag.Size = new Size(0xd6, 0x2a);
            this.trackBarYvalue_Mag.TabIndex = 3;
            this.trackBarYvalue_Mag.TabStop = false;
            this.trackBarYvalue_Mag.TickFrequency = 0x800;
            this.trackBarZvalue_Mag.Anchor = AnchorStyles.None;
            this.trackBarZvalue_Mag.BackColor = SystemColors.Control;
            this.trackBarZvalue_Mag.Enabled = false;
            this.trackBarZvalue_Mag.LargeChange = 0;
            this.trackBarZvalue_Mag.Location = new Point(0x24, 0x63);
            this.trackBarZvalue_Mag.Maximum = 0x7ff;
            this.trackBarZvalue_Mag.Minimum = -2048;
            this.trackBarZvalue_Mag.Name = "trackBarZvalue_Mag";
            this.trackBarZvalue_Mag.Size = new Size(0xd6, 0x2a);
            this.trackBarZvalue_Mag.TabIndex = 4;
            this.trackBarZvalue_Mag.TabStop = false;
            this.trackBarZvalue_Mag.TickFrequency = 0x800;
            this.label1.Anchor = AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x12, 0x1f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(14, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "X";
            this.label2.Anchor = AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            this.label3.Anchor = AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x12, 0x6c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(14, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Z";
            this.lblXvalue_Mag.Anchor = AnchorStyles.None;
            this.lblXvalue_Mag.AutoSize = true;
            this.lblXvalue_Mag.Location = new Point(0xfe, 0x1f);
            this.lblXvalue_Mag.Name = "lblXvalue_Mag";
            this.lblXvalue_Mag.Size = new Size(13, 13);
            this.lblXvalue_Mag.TabIndex = 8;
            this.lblXvalue_Mag.Text = "0";
            this.lblYvalue_Mag.Anchor = AnchorStyles.None;
            this.lblYvalue_Mag.AutoSize = true;
            this.lblYvalue_Mag.Location = new Point(0xfe, 70);
            this.lblYvalue_Mag.Name = "lblYvalue_Mag";
            this.lblYvalue_Mag.Size = new Size(13, 13);
            this.lblYvalue_Mag.TabIndex = 9;
            this.lblYvalue_Mag.Text = "0";
            this.lblZvalue_Mag.Anchor = AnchorStyles.None;
            this.lblZvalue_Mag.AutoSize = true;
            this.lblZvalue_Mag.Location = new Point(0xfe, 0x6c);
            this.lblZvalue_Mag.Name = "lblZvalue_Mag";
            this.lblZvalue_Mag.Size = new Size(13, 13);
            this.lblZvalue_Mag.TabIndex = 10;
            this.lblZvalue_Mag.Text = "0";
            this.groupBox1.Controls.Add(this.trackBarXvalue_Mag);
            this.groupBox1.Controls.Add(this.lblZvalue_Mag);
            this.groupBox1.Controls.Add(this.trackBarYvalue_Mag);
            this.groupBox1.Controls.Add(this.lblYvalue_Mag);
            this.groupBox1.Controls.Add(this.trackBarZvalue_Mag);
            this.groupBox1.Controls.Add(this.lblXvalue_Mag);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(10, 0x20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x12d, 0x93);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Magnetometer*";
            this.groupBox2.Controls.Add(this.lblZvalue_Acc);
            this.groupBox2.Controls.Add(this.lblYvalue_Acc);
            this.groupBox2.Controls.Add(this.lblXvalue_Acc);
            this.groupBox2.Controls.Add(this.trackBarZvalue_Acc);
            this.groupBox2.Controls.Add(this.trackBarYvalue_Acc);
            this.groupBox2.Controls.Add(this.trackBarXvalue_Acc);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new Point(0x13d, 0x20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x12d, 0x93);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accelerometer*";
            this.lblZvalue_Acc.Anchor = AnchorStyles.None;
            this.lblZvalue_Acc.AutoSize = true;
            this.lblZvalue_Acc.Location = new Point(0xfe, 0x6c);
            this.lblZvalue_Acc.Name = "lblZvalue_Acc";
            this.lblZvalue_Acc.Size = new Size(13, 13);
            this.lblZvalue_Acc.TabIndex = 0x11;
            this.lblZvalue_Acc.Text = "0";
            this.lblYvalue_Acc.Anchor = AnchorStyles.None;
            this.lblYvalue_Acc.AutoSize = true;
            this.lblYvalue_Acc.Location = new Point(0xfe, 70);
            this.lblYvalue_Acc.Name = "lblYvalue_Acc";
            this.lblYvalue_Acc.Size = new Size(13, 13);
            this.lblYvalue_Acc.TabIndex = 0x10;
            this.lblYvalue_Acc.Text = "0";
            this.lblXvalue_Acc.Anchor = AnchorStyles.None;
            this.lblXvalue_Acc.AutoSize = true;
            this.lblXvalue_Acc.Location = new Point(0xfe, 0x1f);
            this.lblXvalue_Acc.Name = "lblXvalue_Acc";
            this.lblXvalue_Acc.Size = new Size(13, 13);
            this.lblXvalue_Acc.TabIndex = 15;
            this.lblXvalue_Acc.Text = "0";
            this.trackBarZvalue_Acc.Anchor = AnchorStyles.None;
            this.trackBarZvalue_Acc.BackColor = SystemColors.Control;
            this.trackBarZvalue_Acc.Enabled = false;
            this.trackBarZvalue_Acc.LargeChange = 0;
            this.trackBarZvalue_Acc.Location = new Point(0x24, 0x63);
            this.trackBarZvalue_Acc.Maximum = 0x7ff;
            this.trackBarZvalue_Acc.Minimum = -2048;
            this.trackBarZvalue_Acc.Name = "trackBarZvalue_Acc";
            this.trackBarZvalue_Acc.Size = new Size(0xd6, 0x2a);
            this.trackBarZvalue_Acc.TabIndex = 14;
            this.trackBarZvalue_Acc.TabStop = false;
            this.trackBarZvalue_Acc.TickFrequency = 0x800;
            this.trackBarYvalue_Acc.Anchor = AnchorStyles.None;
            this.trackBarYvalue_Acc.BackColor = SystemColors.Control;
            this.trackBarYvalue_Acc.Enabled = false;
            this.trackBarYvalue_Acc.LargeChange = 0;
            this.trackBarYvalue_Acc.Location = new Point(0x24, 0x3b);
            this.trackBarYvalue_Acc.Maximum = 0x7ff;
            this.trackBarYvalue_Acc.Minimum = -2048;
            this.trackBarYvalue_Acc.Name = "trackBarYvalue_Acc";
            this.trackBarYvalue_Acc.Size = new Size(0xd6, 0x2a);
            this.trackBarYvalue_Acc.TabIndex = 13;
            this.trackBarYvalue_Acc.TabStop = false;
            this.trackBarYvalue_Acc.TickFrequency = 0x800;
            this.trackBarXvalue_Acc.Anchor = AnchorStyles.None;
            this.trackBarXvalue_Acc.BackColor = SystemColors.Control;
            this.trackBarXvalue_Acc.Enabled = false;
            this.trackBarXvalue_Acc.LargeChange = 0;
            this.trackBarXvalue_Acc.Location = new Point(0x24, 0x16);
            this.trackBarXvalue_Acc.Maximum = 0x7ff;
            this.trackBarXvalue_Acc.Minimum = -2048;
            this.trackBarXvalue_Acc.Name = "trackBarXvalue_Acc";
            this.trackBarXvalue_Acc.Size = new Size(0xd6, 0x2a);
            this.trackBarXvalue_Acc.TabIndex = 12;
            this.trackBarXvalue_Acc.TabStop = false;
            this.trackBarXvalue_Acc.TickFrequency = 0x800;
            this.label6.Anchor = AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x12, 0x6c);
            this.label6.Name = "label6";
            this.label6.Size = new Size(14, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Z";
            this.label5.Anchor = AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x12, 70);
            this.label5.Name = "label5";
            this.label5.Size = new Size(14, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Y";
            this.label4.Anchor = AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x12, 0x1f);
            this.label4.Name = "label4";
            this.label4.Size = new Size(14, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "X";
            this.panel_MEMS.Controls.Add(this.label8);
            this.panel_MEMS.Controls.Add(this.label7);
            this.panel_MEMS.Controls.Add(this.label_MEMSView);
            this.panel_MEMS.Controls.Add(this.groupBox2);
            this.panel_MEMS.Controls.Add(this.groupBox1);
            this.panel_MEMS.Location = new Point(3, 2);
            this.panel_MEMS.Name = "panel_MEMS";
            this.panel_MEMS.Size = new Size(0x274, 0xe1);
            this.panel_MEMS.TabIndex = 13;
            this.panel_MEMS.Paint += new PaintEventHandler(this.panel_MEMS_Paint);
            this.panel_MEMS.Resize += new EventHandler(this.panel_MEMS_Resize);
            this.label8.AutoSize = true;
            this.label8.ForeColor = SystemColors.ActiveCaption;
            this.label8.Location = new Point(0x15, 0xca);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0xa2, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Please see spec sheet for details";
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label7.ForeColor = SystemColors.ActiveCaption;
            this.label7.Location = new Point(13, 0xbb);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0xae, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "* Availability is hardware dependent";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x277, 0xe5);
            base.Controls.Add(this.panel_MEMS);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x275, 0xd7);
            base.Name = "frmMEMSView";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "MEMS Sensor Data";
            base.Load += new EventHandler(this.frmMEMSView_Load);
            base.Paint += new PaintEventHandler(this.panel_MEMS_Paint);
            base.LocationChanged += new EventHandler(this.frmMEMSView_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmMEMSView_ResizeEnd);
            this.trackBarXvalue_Mag.EndInit();
            this.trackBarYvalue_Mag.EndInit();
            this.trackBarZvalue_Mag.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.trackBarZvalue_Acc.EndInit();
            this.trackBarYvalue_Acc.EndInit();
            this.trackBarXvalue_Acc.EndInit();
            this.panel_MEMS.ResumeLayout(false);
            this.panel_MEMS.PerformLayout();
            base.ResumeLayout(false);
        }

        public void MEMS_setup()
        {
            TrackBar bar = new TrackBar();
            TrackBar bar2 = new TrackBar();
            TrackBar bar3 = new TrackBar();
            Label label = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            bar.SetRange(this.tb_minRange, this.tb_maxRange);
            bar.Value = this.Xvalue_Mag;
            bar2.SetRange(this.tb_minRange, this.tb_maxRange);
            bar2.Value = this.Yvalue_Mag;
            bar3.SetRange(this.tb_minRange, this.tb_maxRange);
            bar3.Value = this.Zvalue_Mag;
            label.Text = this.Xvalue_Mag.ToString();
            label2.Text = this.Yvalue_Mag.ToString();
            label3.Text = this.Zvalue_Mag.ToString();
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

        private void panel_MEMS_Paint(object sender, PaintEventArgs e)
        {
            this.updateMEMS_XYZvalues();
            string str = "State: ";
            switch (this.comm.dataGui.MEMS_State)
            {
                case -1:
                    str = str + "Disabled";
                    this.label_MEMSView.BackColor = SystemColors.Control;
                    clsGlobal.MemStat = false;
                    this.setMEMSToZero();
                    break;

                case 0:
                    str = str + "Unknown";
                    this.label_MEMSView.BackColor = Color.Khaki;
                    clsGlobal.MemStat = true;
                    break;

                case 1:
                    str = str + "Stationary";
                    this.label_MEMSView.BackColor = Color.LightSteelBlue;
                    clsGlobal.MemStat = true;
                    break;

                case 2:
                    str = str + "Moving";
                    this.label_MEMSView.BackColor = Color.YellowGreen;
                    clsGlobal.MemStat = true;
                    break;
            }
            this.label_MEMSView.Text = str;
        }

        private void panel_MEMS_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_MEMS.Refresh();
        }

        private void setMEMSToZero()
        {
            this.lblXvalue_Mag.Text = this.xvalue_Mag.ToString();
            this.lblYvalue_Mag.Text = this.yvalue_Mag.ToString();
            this.lblZvalue_Mag.Text = this.zvalue_Mag.ToString();
            this.lblXvalue_Acc.Text = this.xvalue_Acc.ToString();
            this.lblYvalue_Acc.Text = this.yvalue_Acc.ToString();
            this.lblZvalue_Acc.Text = this.zvalue_Acc.ToString();
            this.trackBarXvalue_Mag.Value = this.xvalue_Mag;
            this.trackBarYvalue_Mag.Value = this.yvalue_Mag;
            this.trackBarZvalue_Mag.Value = this.zvalue_Mag;
            this.trackBarXvalue_Acc.Value = this.xvalue_Acc;
            this.trackBarYvalue_Acc.Value = this.yvalue_Acc;
            this.trackBarZvalue_Acc.Value = this.zvalue_Acc;
        }

        private void updateMEMS_XYZvalues()
        {
            switch (this.comm.dataGui.sensID)
            {
                case 14:
                    this.lblXvalue_Mag.Text = this.comm.dataGui.XValue_Mag.ToString();
                    this.lblYvalue_Mag.Text = this.comm.dataGui.YValue_Mag.ToString();
                    this.lblZvalue_Mag.Text = this.comm.dataGui.ZValue_Mag.ToString();
                    this.trackBarXvalue_Mag.Value = this.comm.dataGui.XValue_Mag;
                    this.trackBarYvalue_Mag.Value = this.comm.dataGui.YValue_Mag;
                    this.trackBarZvalue_Mag.Value = this.comm.dataGui.ZValue_Mag;
                    return;

                case 15:
                    this.lblXvalue_Acc.Text = this.comm.dataGui.XValue_Acc.ToString();
                    this.lblYvalue_Acc.Text = this.comm.dataGui.YValue_Acc.ToString();
                    this.lblZvalue_Acc.Text = this.comm.dataGui.ZValue_Acc.ToString();
                    this.trackBarXvalue_Acc.Value = this.comm.dataGui.XValue_Acc;
                    this.trackBarYvalue_Acc.Value = this.comm.dataGui.YValue_Acc;
                    this.trackBarZvalue_Acc.Value = this.comm.dataGui.ZValue_Acc;
                    return;
            }
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
                this.comm.DisplayPanelMEMS = this.panel_MEMS;
                this.myPanel = this.panel_MEMS;
                this.comm.EnableMEMSView = true;
                this.Text = this.comm.sourceDeviceName + ": MEMS Sensor Data";
            }
        }

        public int Xvalue_Acc
        {
            get
            {
                return this.xvalue_Acc;
            }
            set
            {
                this.xvalue_Acc = value;
            }
        }

        public int Xvalue_Mag
        {
            get
            {
                return this.xvalue_Mag;
            }
            set
            {
                this.xvalue_Mag = value;
            }
        }

        public int Yvalue_Acc
        {
            get
            {
                return this.yvalue_Acc;
            }
            set
            {
                this.yvalue_Acc = value;
            }
        }

        public int Yvalue_Mag
        {
            get
            {
                return this.yvalue_Mag;
            }
            set
            {
                this.yvalue_Mag = value;
            }
        }

        public int Zvalue_Acc
        {
            get
            {
                return this.zvalue_Acc;
            }
            set
            {
                this.zvalue_Acc = value;
            }
        }

        public int Zvalue_Mag
        {
            get
            {
                return this.zvalue_Mag;
            }
            set
            {
                this.zvalue_Mag = value;
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

