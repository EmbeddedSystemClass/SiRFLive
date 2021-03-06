﻿namespace SiRFLive.GUI
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommSVAvgCNo : Form
    {
        private float[,] _avgcnos;
        private Color[] _color;
        private static int _numberOpen;
        private string _persistedWindowName;
        private Button button_0To0;
        private Button button_0To8;
        private Button button_10To12;
        private Button button_12To14;
        private Button button_14To16;
        private Button button_16To18;
        private Button button_18To20;
        private Button button_20To22;
        private Button button_22To24;
        private Button button_24To26;
        private Button button_26To28;
        private Button button_28To30;
        private Button button_30To32;
        private Button button_32To34;
        private Button button_34To36;
        private Button button_36To38;
        private Button button_38To40;
        private Button button_40To42;
        private Button button_42To44;
        private Button button_44To46;
        private Button button_46To48;
        private Button button_48To50;
        private Button button_50To;
        private Button button_8To10;
        private ColorDialog colorDialog1;
        private CommunicationManager comm;
        private IContainer components;
        private bool IsRealTime;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private SplitContainer splitContainer1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommSVAvgCNo()
        {
            this._avgcnos = new float[9, 8];
            this._persistedWindowName = "SV Average CNo Window";
            this._color = new Color[] { 
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 220), Color.FromArgb(0xcc, 0xff, 0xcc), Color.FromArgb(0xff, 0xff, 0x99), Color.FromArgb(0xff, 0xff, 0x99), Color.FromArgb(0xff, 0xff, 0x99), Color.FromArgb(0xcc, 0xff, 0x99), Color.FromArgb(0xcc, 0xff, 0x99), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xcc, 0xff, 0), Color.FromArgb(0xff, 0xca, 0), Color.FromArgb(0xcc, 0xcc, 0), Color.FromArgb(0xff, 0x99, 0), 
                Color.FromArgb(0xcc, 0x99, 0), Color.FromArgb(0xff, 0x66, 0), Color.FromArgb(0xcc, 0x66, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xff, 0, 0)
             };
            this.IsRealTime = true;
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "SV CNo Window " + _numberOpen.ToString();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        public frmCommSVAvgCNo(float[,] avgcnos)
        {
            this._avgcnos = new float[9, 8];
            this._persistedWindowName = "SV Average CNo Window";
            this._color = new Color[] { 
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 220), Color.FromArgb(0xcc, 0xff, 0xcc), Color.FromArgb(0xff, 0xff, 0x99), Color.FromArgb(0xff, 0xff, 0x99), Color.FromArgb(0xff, 0xff, 0x99), Color.FromArgb(0xcc, 0xff, 0x99), Color.FromArgb(0xcc, 0xff, 0x99), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0), Color.FromArgb(0xcc, 0xff, 0), Color.FromArgb(0xff, 0xca, 0), Color.FromArgb(0xcc, 0xcc, 0), Color.FromArgb(0xff, 0x99, 0), 
                Color.FromArgb(0xcc, 0x99, 0), Color.FromArgb(0xff, 0x66, 0), Color.FromArgb(0xcc, 0x66, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xcc, 0x33, 0), Color.FromArgb(0xff, 0, 0)
             };
            this.IsRealTime = false;
            this.InitializeComponent();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this._avgcnos[i, j] = avgcnos[i, j];
                }
            }
        }

        private void button_0To0_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_0To0.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_0To8_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_0To8.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_10To12_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_10To12.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_12To14_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_12To14.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_14To16_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_14To16.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_16To18_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_16To18.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_18To20_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_18To20.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_20To22_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_20To22.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_22To24_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_22To24.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_24To26_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_24To26.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_26To28_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_26To28.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_28To30_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_28To30.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_30To32_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_30To32.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_32To34_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_32To34.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_34To36_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_34To36.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_36To38_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_36To38.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_38To40_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_38To40.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_40To42_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_40To42.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_42To44_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_42To44.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_44To46_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_44To46.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_46To48_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_46To48.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_48To50_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_48To50.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_50To_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_50To.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        private void button_8To10_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_8To10.BackColor = this.colorDialog1.Color;
                this.splitContainer1.Panel2.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommSVAvgCNo_Load(object sender, EventArgs e)
        {
            this.button_0To0.BackColor = this._color[0];
            this.button_0To8.BackColor = this._color[1];
            this.button_8To10.BackColor = this._color[2];
            this.button_10To12.BackColor = this._color[3];
            this.button_12To14.BackColor = this._color[4];
            this.button_14To16.BackColor = this._color[5];
            this.button_16To18.BackColor = this._color[6];
            this.button_18To20.BackColor = this._color[7];
            this.button_20To22.BackColor = this._color[8];
            this.button_22To24.BackColor = this._color[9];
            this.button_24To26.BackColor = this._color[10];
            this.button_26To28.BackColor = this._color[11];
            this.button_28To30.BackColor = this._color[12];
            this.button_30To32.BackColor = this._color[13];
            this.button_32To34.BackColor = this._color[14];
            this.button_34To36.BackColor = this._color[15];
            this.button_36To38.BackColor = this._color[0x10];
            this.button_38To40.BackColor = this._color[0x11];
            this.button_40To42.BackColor = this._color[0x12];
            this.button_42To44.BackColor = this._color[0x13];
            this.button_44To46.BackColor = this._color[20];
            this.button_46To48.BackColor = this._color[0x15];
            this.button_48To50.BackColor = this._color[0x16];
            this.button_50To.BackColor = this._color[0x17];
        }

        private void frmCommSVAvgCNo_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommSVAvgCNo_ResizeEnd(object sender, EventArgs e)
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommSVAvgCNo));
            this.button_50To = new Button();
            this.label24 = new Label();
            this.button_48To50 = new Button();
            this.label23 = new Label();
            this.button_46To48 = new Button();
            this.label22 = new Label();
            this.button_44To46 = new Button();
            this.label21 = new Label();
            this.button_42To44 = new Button();
            this.label20 = new Label();
            this.button_40To42 = new Button();
            this.label19 = new Label();
            this.button_38To40 = new Button();
            this.label18 = new Label();
            this.splitContainer1 = new SplitContainer();
            this.button_36To38 = new Button();
            this.label17 = new Label();
            this.button_34To36 = new Button();
            this.label16 = new Label();
            this.button_32To34 = new Button();
            this.label15 = new Label();
            this.button_30To32 = new Button();
            this.label14 = new Label();
            this.button_28To30 = new Button();
            this.label13 = new Label();
            this.button_26To28 = new Button();
            this.label12 = new Label();
            this.button_24To26 = new Button();
            this.label11 = new Label();
            this.button_22To24 = new Button();
            this.label10 = new Label();
            this.button_20To22 = new Button();
            this.label9 = new Label();
            this.button_18To20 = new Button();
            this.label8 = new Label();
            this.button_16To18 = new Button();
            this.label7 = new Label();
            this.button_14To16 = new Button();
            this.label6 = new Label();
            this.button_12To14 = new Button();
            this.label5 = new Label();
            this.button_10To12 = new Button();
            this.label4 = new Label();
            this.button_8To10 = new Button();
            this.label3 = new Label();
            this.button_0To8 = new Button();
            this.label2 = new Label();
            this.button_0To0 = new Button();
            this.label1 = new Label();
            this.colorDialog1 = new ColorDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            base.SuspendLayout();
            this.button_50To.FlatAppearance.BorderColor = Color.Silver;
            this.button_50To.FlatStyle = FlatStyle.Flat;
            this.button_50To.Location = new Point(80, 490);
            this.button_50To.Name = "button_50To";
            this.button_50To.Size = new Size(0x4b, 13);
            this.button_50To.TabIndex = 0x2e;
            this.button_50To.UseVisualStyleBackColor = true;
            this.button_50To.Click += new EventHandler(this.button_50To_Click);
            this.label24.BorderStyle = BorderStyle.FixedSingle;
            this.label24.Location = new Point(7, 0x1e6);
            this.label24.Name = "label24";
            this.label24.Size = new Size(0x9d, 0x15);
            this.label24.TabIndex = 0x2f;
            this.label24.Text = "50<CNO";
            this.button_48To50.FlatAppearance.BorderColor = Color.Silver;
            this.button_48To50.FlatStyle = FlatStyle.Flat;
            this.button_48To50.Location = new Point(80, 470);
            this.button_48To50.Name = "button_48To50";
            this.button_48To50.Size = new Size(0x4b, 13);
            this.button_48To50.TabIndex = 0x2c;
            this.button_48To50.UseVisualStyleBackColor = true;
            this.button_48To50.Click += new EventHandler(this.button_48To50_Click);
            this.label23.BorderStyle = BorderStyle.FixedSingle;
            this.label23.Location = new Point(7, 0x1d2);
            this.label23.Name = "label23";
            this.label23.Size = new Size(0x9d, 0x15);
            this.label23.TabIndex = 0x2d;
            this.label23.Text = "48<CNO<=50";
            this.button_46To48.FlatAppearance.BorderColor = Color.Silver;
            this.button_46To48.FlatStyle = FlatStyle.Flat;
            this.button_46To48.Location = new Point(80, 450);
            this.button_46To48.Name = "button_46To48";
            this.button_46To48.Size = new Size(0x4b, 13);
            this.button_46To48.TabIndex = 0x2a;
            this.button_46To48.UseVisualStyleBackColor = true;
            this.button_46To48.Click += new EventHandler(this.button_46To48_Click);
            this.label22.BorderStyle = BorderStyle.FixedSingle;
            this.label22.Location = new Point(7, 0x1be);
            this.label22.Name = "label22";
            this.label22.Size = new Size(0x9d, 0x15);
            this.label22.TabIndex = 0x2b;
            this.label22.Text = "46<CNO<=48";
            this.button_44To46.FlatAppearance.BorderColor = Color.Silver;
            this.button_44To46.FlatStyle = FlatStyle.Flat;
            this.button_44To46.Location = new Point(80, 430);
            this.button_44To46.Name = "button_44To46";
            this.button_44To46.Size = new Size(0x4b, 13);
            this.button_44To46.TabIndex = 40;
            this.button_44To46.UseVisualStyleBackColor = true;
            this.button_44To46.Click += new EventHandler(this.button_44To46_Click);
            this.label21.BorderStyle = BorderStyle.FixedSingle;
            this.label21.Location = new Point(7, 0x1aa);
            this.label21.Name = "label21";
            this.label21.Size = new Size(0x9d, 0x15);
            this.label21.TabIndex = 0x29;
            this.label21.Text = "44<CNO<=46";
            this.button_42To44.FlatAppearance.BorderColor = Color.Silver;
            this.button_42To44.FlatStyle = FlatStyle.Flat;
            this.button_42To44.Location = new Point(80, 410);
            this.button_42To44.Name = "button_42To44";
            this.button_42To44.Size = new Size(0x4b, 13);
            this.button_42To44.TabIndex = 0x26;
            this.button_42To44.UseVisualStyleBackColor = true;
            this.button_42To44.Click += new EventHandler(this.button_42To44_Click);
            this.label20.BorderStyle = BorderStyle.FixedSingle;
            this.label20.Location = new Point(7, 0x196);
            this.label20.Name = "label20";
            this.label20.Size = new Size(0x9d, 0x15);
            this.label20.TabIndex = 0x27;
            this.label20.Text = "42<CNO<=44";
            this.button_40To42.FlatAppearance.BorderColor = Color.Silver;
            this.button_40To42.FlatStyle = FlatStyle.Flat;
            this.button_40To42.Location = new Point(80, 390);
            this.button_40To42.Name = "button_40To42";
            this.button_40To42.Size = new Size(0x4b, 13);
            this.button_40To42.TabIndex = 0x24;
            this.button_40To42.UseVisualStyleBackColor = true;
            this.button_40To42.Click += new EventHandler(this.button_40To42_Click);
            this.label19.BorderStyle = BorderStyle.FixedSingle;
            this.label19.Location = new Point(7, 0x182);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x9d, 0x15);
            this.label19.TabIndex = 0x25;
            this.label19.Text = "40<CNO<=42";
            this.button_38To40.FlatAppearance.BorderColor = Color.Silver;
            this.button_38To40.FlatStyle = FlatStyle.Flat;
            this.button_38To40.Location = new Point(80, 370);
            this.button_38To40.Name = "button_38To40";
            this.button_38To40.Size = new Size(0x4b, 13);
            this.button_38To40.TabIndex = 0x22;
            this.button_38To40.UseVisualStyleBackColor = true;
            this.button_38To40.Click += new EventHandler(this.button_38To40_Click);
            this.label18.BorderStyle = BorderStyle.FixedSingle;
            this.label18.Location = new Point(7, 0x16e);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x9d, 0x15);
            this.label18.TabIndex = 0x23;
            this.label18.Text = "38<CNO<=40";
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.button_50To);
            this.splitContainer1.Panel1.Controls.Add(this.label24);
            this.splitContainer1.Panel1.Controls.Add(this.button_48To50);
            this.splitContainer1.Panel1.Controls.Add(this.label23);
            this.splitContainer1.Panel1.Controls.Add(this.button_46To48);
            this.splitContainer1.Panel1.Controls.Add(this.label22);
            this.splitContainer1.Panel1.Controls.Add(this.button_44To46);
            this.splitContainer1.Panel1.Controls.Add(this.label21);
            this.splitContainer1.Panel1.Controls.Add(this.button_42To44);
            this.splitContainer1.Panel1.Controls.Add(this.label20);
            this.splitContainer1.Panel1.Controls.Add(this.button_40To42);
            this.splitContainer1.Panel1.Controls.Add(this.label19);
            this.splitContainer1.Panel1.Controls.Add(this.button_38To40);
            this.splitContainer1.Panel1.Controls.Add(this.label18);
            this.splitContainer1.Panel1.Controls.Add(this.button_36To38);
            this.splitContainer1.Panel1.Controls.Add(this.label17);
            this.splitContainer1.Panel1.Controls.Add(this.button_34To36);
            this.splitContainer1.Panel1.Controls.Add(this.label16);
            this.splitContainer1.Panel1.Controls.Add(this.button_32To34);
            this.splitContainer1.Panel1.Controls.Add(this.label15);
            this.splitContainer1.Panel1.Controls.Add(this.button_30To32);
            this.splitContainer1.Panel1.Controls.Add(this.label14);
            this.splitContainer1.Panel1.Controls.Add(this.button_28To30);
            this.splitContainer1.Panel1.Controls.Add(this.label13);
            this.splitContainer1.Panel1.Controls.Add(this.button_26To28);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.button_24To26);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.button_22To24);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.button_20To22);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.button_18To20);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.button_16To18);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.button_14To16);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.button_12To14);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.button_10To12);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.button_8To10);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.button_0To8);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.button_0To0);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Paint += new PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new Size(0x310, 0x249);
            this.splitContainer1.SplitterDistance = 0xb0;
            this.splitContainer1.TabIndex = 1;
            this.button_36To38.FlatAppearance.BorderColor = Color.Silver;
            this.button_36To38.FlatStyle = FlatStyle.Flat;
            this.button_36To38.Location = new Point(80, 350);
            this.button_36To38.Name = "button_36To38";
            this.button_36To38.Size = new Size(0x4b, 13);
            this.button_36To38.TabIndex = 0x20;
            this.button_36To38.UseVisualStyleBackColor = true;
            this.button_36To38.Click += new EventHandler(this.button_36To38_Click);
            this.label17.BorderStyle = BorderStyle.FixedSingle;
            this.label17.Location = new Point(7, 0x15a);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x9d, 0x15);
            this.label17.TabIndex = 0x21;
            this.label17.Text = "36<CNO<=38";
            this.button_34To36.FlatAppearance.BorderColor = Color.Silver;
            this.button_34To36.FlatStyle = FlatStyle.Flat;
            this.button_34To36.Location = new Point(80, 330);
            this.button_34To36.Name = "button_34To36";
            this.button_34To36.Size = new Size(0x4b, 13);
            this.button_34To36.TabIndex = 30;
            this.button_34To36.UseVisualStyleBackColor = true;
            this.button_34To36.Click += new EventHandler(this.button_34To36_Click);
            this.label16.BorderStyle = BorderStyle.FixedSingle;
            this.label16.Location = new Point(7, 0x146);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x9d, 0x15);
            this.label16.TabIndex = 0x1f;
            this.label16.Text = "34<CNO<=36";
            this.button_32To34.FlatAppearance.BorderColor = Color.Silver;
            this.button_32To34.FlatStyle = FlatStyle.Flat;
            this.button_32To34.Location = new Point(80, 310);
            this.button_32To34.Name = "button_32To34";
            this.button_32To34.Size = new Size(0x4b, 13);
            this.button_32To34.TabIndex = 0x1c;
            this.button_32To34.UseVisualStyleBackColor = true;
            this.button_32To34.Click += new EventHandler(this.button_32To34_Click);
            this.label15.BorderStyle = BorderStyle.FixedSingle;
            this.label15.Location = new Point(7, 0x132);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x9d, 0x15);
            this.label15.TabIndex = 0x1d;
            this.label15.Text = "32<CNO<=34";
            this.button_30To32.FlatAppearance.BorderColor = Color.Silver;
            this.button_30To32.FlatStyle = FlatStyle.Flat;
            this.button_30To32.Location = new Point(80, 290);
            this.button_30To32.Name = "button_30To32";
            this.button_30To32.Size = new Size(0x4b, 13);
            this.button_30To32.TabIndex = 0x1a;
            this.button_30To32.UseVisualStyleBackColor = true;
            this.button_30To32.Click += new EventHandler(this.button_30To32_Click);
            this.label14.BorderStyle = BorderStyle.FixedSingle;
            this.label14.Location = new Point(7, 0x11e);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x9d, 0x15);
            this.label14.TabIndex = 0x1b;
            this.label14.Text = "30<CNO<=32";
            this.button_28To30.FlatAppearance.BorderColor = Color.Silver;
            this.button_28To30.FlatStyle = FlatStyle.Flat;
            this.button_28To30.Location = new Point(80, 270);
            this.button_28To30.Name = "button_28To30";
            this.button_28To30.Size = new Size(0x4b, 13);
            this.button_28To30.TabIndex = 0x18;
            this.button_28To30.UseVisualStyleBackColor = true;
            this.button_28To30.Click += new EventHandler(this.button_28To30_Click);
            this.label13.BorderStyle = BorderStyle.FixedSingle;
            this.label13.Location = new Point(7, 0x10a);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x9d, 0x15);
            this.label13.TabIndex = 0x19;
            this.label13.Text = "28<CNO<=30";
            this.button_26To28.FlatAppearance.BorderColor = Color.Silver;
            this.button_26To28.FlatStyle = FlatStyle.Flat;
            this.button_26To28.Location = new Point(80, 250);
            this.button_26To28.Name = "button_26To28";
            this.button_26To28.Size = new Size(0x4b, 13);
            this.button_26To28.TabIndex = 0x16;
            this.button_26To28.UseVisualStyleBackColor = true;
            this.button_26To28.Click += new EventHandler(this.button_26To28_Click);
            this.label12.BorderStyle = BorderStyle.FixedSingle;
            this.label12.Location = new Point(7, 0xf6);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x9d, 0x15);
            this.label12.TabIndex = 0x17;
            this.label12.Text = "26<CNO<=28";
            this.button_24To26.FlatAppearance.BorderColor = Color.Silver;
            this.button_24To26.FlatStyle = FlatStyle.Flat;
            this.button_24To26.Location = new Point(80, 230);
            this.button_24To26.Name = "button_24To26";
            this.button_24To26.Size = new Size(0x4b, 13);
            this.button_24To26.TabIndex = 20;
            this.button_24To26.UseVisualStyleBackColor = true;
            this.button_24To26.Click += new EventHandler(this.button_24To26_Click);
            this.label11.BorderStyle = BorderStyle.FixedSingle;
            this.label11.Location = new Point(7, 0xe2);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x9d, 0x15);
            this.label11.TabIndex = 0x15;
            this.label11.Text = "24<CNO<=26";
            this.button_22To24.FlatAppearance.BorderColor = Color.Silver;
            this.button_22To24.FlatStyle = FlatStyle.Flat;
            this.button_22To24.Location = new Point(80, 210);
            this.button_22To24.Name = "button_22To24";
            this.button_22To24.Size = new Size(0x4b, 13);
            this.button_22To24.TabIndex = 0x12;
            this.button_22To24.UseVisualStyleBackColor = true;
            this.button_22To24.Click += new EventHandler(this.button_22To24_Click);
            this.label10.BorderStyle = BorderStyle.FixedSingle;
            this.label10.Location = new Point(7, 0xce);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x9d, 0x15);
            this.label10.TabIndex = 0x13;
            this.label10.Text = "22<CNO<=24";
            this.button_20To22.FlatAppearance.BorderColor = Color.Silver;
            this.button_20To22.FlatStyle = FlatStyle.Flat;
            this.button_20To22.Location = new Point(80, 190);
            this.button_20To22.Name = "button_20To22";
            this.button_20To22.Size = new Size(0x4b, 13);
            this.button_20To22.TabIndex = 0x10;
            this.button_20To22.UseVisualStyleBackColor = true;
            this.button_20To22.Click += new EventHandler(this.button_20To22_Click);
            this.label9.BorderStyle = BorderStyle.FixedSingle;
            this.label9.Location = new Point(7, 0xba);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x9d, 0x15);
            this.label9.TabIndex = 0x11;
            this.label9.Text = "20<CNO<=22";
            this.button_18To20.FlatAppearance.BorderColor = Color.Silver;
            this.button_18To20.FlatStyle = FlatStyle.Flat;
            this.button_18To20.Location = new Point(80, 170);
            this.button_18To20.Name = "button_18To20";
            this.button_18To20.Size = new Size(0x4b, 13);
            this.button_18To20.TabIndex = 14;
            this.button_18To20.UseVisualStyleBackColor = true;
            this.button_18To20.Click += new EventHandler(this.button_18To20_Click);
            this.label8.BorderStyle = BorderStyle.FixedSingle;
            this.label8.Location = new Point(7, 0xa6);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x9d, 0x15);
            this.label8.TabIndex = 15;
            this.label8.Text = "18<CNO<=20";
            this.button_16To18.FlatAppearance.BorderColor = Color.Silver;
            this.button_16To18.FlatStyle = FlatStyle.Flat;
            this.button_16To18.Location = new Point(80, 150);
            this.button_16To18.Name = "button_16To18";
            this.button_16To18.Size = new Size(0x4b, 13);
            this.button_16To18.TabIndex = 12;
            this.button_16To18.UseVisualStyleBackColor = true;
            this.button_16To18.Click += new EventHandler(this.button_16To18_Click);
            this.label7.BorderStyle = BorderStyle.FixedSingle;
            this.label7.Location = new Point(7, 0x92);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x9d, 0x15);
            this.label7.TabIndex = 13;
            this.label7.Text = "16<CNO<=18";
            this.button_14To16.FlatAppearance.BorderColor = Color.Silver;
            this.button_14To16.FlatStyle = FlatStyle.Flat;
            this.button_14To16.Location = new Point(80, 130);
            this.button_14To16.Name = "button_14To16";
            this.button_14To16.Size = new Size(0x4b, 13);
            this.button_14To16.TabIndex = 10;
            this.button_14To16.UseVisualStyleBackColor = true;
            this.button_14To16.Click += new EventHandler(this.button_14To16_Click);
            this.label6.BorderStyle = BorderStyle.FixedSingle;
            this.label6.Location = new Point(7, 0x7e);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x9d, 0x15);
            this.label6.TabIndex = 11;
            this.label6.Text = "14<CNO<=16";
            this.button_12To14.FlatAppearance.BorderColor = Color.Silver;
            this.button_12To14.FlatStyle = FlatStyle.Flat;
            this.button_12To14.Location = new Point(80, 110);
            this.button_12To14.Name = "button_12To14";
            this.button_12To14.Size = new Size(0x4b, 13);
            this.button_12To14.TabIndex = 8;
            this.button_12To14.UseVisualStyleBackColor = true;
            this.button_12To14.Click += new EventHandler(this.button_12To14_Click);
            this.label5.BorderStyle = BorderStyle.FixedSingle;
            this.label5.Location = new Point(7, 0x6a);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x9d, 0x15);
            this.label5.TabIndex = 9;
            this.label5.Text = "12<CNO<=14";
            this.button_10To12.FlatAppearance.BorderColor = Color.Silver;
            this.button_10To12.FlatStyle = FlatStyle.Flat;
            this.button_10To12.Location = new Point(80, 90);
            this.button_10To12.Name = "button_10To12";
            this.button_10To12.Size = new Size(0x4b, 13);
            this.button_10To12.TabIndex = 6;
            this.button_10To12.UseVisualStyleBackColor = true;
            this.button_10To12.Click += new EventHandler(this.button_10To12_Click);
            this.label4.BorderStyle = BorderStyle.FixedSingle;
            this.label4.Location = new Point(7, 0x56);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x9d, 0x15);
            this.label4.TabIndex = 7;
            this.label4.Text = "10<CNO<=12";
            this.button_8To10.FlatAppearance.BorderColor = Color.Silver;
            this.button_8To10.FlatStyle = FlatStyle.Flat;
            this.button_8To10.Location = new Point(80, 70);
            this.button_8To10.Name = "button_8To10";
            this.button_8To10.Size = new Size(0x4b, 13);
            this.button_8To10.TabIndex = 4;
            this.button_8To10.UseVisualStyleBackColor = true;
            this.button_8To10.Click += new EventHandler(this.button_8To10_Click);
            this.label3.BorderStyle = BorderStyle.FixedSingle;
            this.label3.Location = new Point(7, 0x42);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x9d, 0x15);
            this.label3.TabIndex = 5;
            this.label3.Text = "8<CNO<=10";
            this.button_0To8.FlatAppearance.BorderColor = Color.Silver;
            this.button_0To8.FlatStyle = FlatStyle.Flat;
            this.button_0To8.Location = new Point(80, 50);
            this.button_0To8.Name = "button_0To8";
            this.button_0To8.Size = new Size(0x4b, 13);
            this.button_0To8.TabIndex = 2;
            this.button_0To8.UseVisualStyleBackColor = true;
            this.button_0To8.Click += new EventHandler(this.button_0To8_Click);
            this.label2.BorderStyle = BorderStyle.FixedSingle;
            this.label2.Location = new Point(7, 0x2e);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x9d, 0x15);
            this.label2.TabIndex = 3;
            this.label2.Text = "0<CNO<=8";
            this.button_0To0.FlatAppearance.BorderColor = Color.Silver;
            this.button_0To0.FlatStyle = FlatStyle.Flat;
            this.button_0To0.Location = new Point(80, 0x20);
            this.button_0To0.Name = "button_0To0";
            this.button_0To0.Size = new Size(0x4b, 13);
            this.button_0To0.TabIndex = 0;
            this.button_0To0.UseVisualStyleBackColor = true;
            this.button_0To0.Click += new EventHandler(this.button_0To0_Click);
            this.label1.BorderStyle = BorderStyle.FixedSingle;
            this.label1.Location = new Point(7, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x9d, 0x15);
            this.label1.TabIndex = 1;
            this.label1.Text = "CNO=0dBHz";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x310, 0x249);
            base.Controls.Add(this.splitContainer1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommSVAvgCNo";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "frmCommSVAvgCNo";
            base.Load += new EventHandler(this.frmCommSVAvgCNo_Load);
            base.LocationChanged += new EventHandler(this.frmCommSVAvgCNo_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommSVAvgCNo_ResizeEnd);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.IsRealTime)
            {
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

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            float height = this.splitContainer1.Panel2.Height;
            float width = this.splitContainer1.Panel2.Width;
            int num3 = 9;
            float num4 = height;
            float num5 = width;
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
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point((int) num5, (int) num4), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xf5, 0xea));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0f, 0f, num5, num4);
                graphics2.FillEllipse(Brushes.White, (float) 0f, (float) 0f, (float) (width - 1f), (float) (height - 1f));
                SolidBrush brush2 = new SolidBrush(Color.FromArgb(0xcc, 0xcc, 0));
                for (int i = 0; i < 9; i++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        float num8 = 0f;
                        if (this.IsRealTime)
                        {
                            if (this.comm.dataPlot != null)
                            {
                                num8 = this.comm.dataPlot.Avg_CNo[i, k];
                            }
                        }
                        else
                        {
                            num8 = this._avgcnos[i, k];
                        }
                        if ((num8 >= 0f) && (num8 < 0.0001))
                        {
                            brush2.Color = this.button_0To0.BackColor;
                        }
                        else if ((num8 > 0f) && (num8 <= 8f))
                        {
                            brush2.Color = this.button_0To8.BackColor;
                        }
                        else if ((num8 > 8f) && (num8 <= 10f))
                        {
                            brush2.Color = this.button_8To10.BackColor;
                        }
                        else if ((num8 > 10f) && (num8 <= 12f))
                        {
                            brush2.Color = this.button_10To12.BackColor;
                        }
                        else if ((num8 > 12f) && (num8 <= 14f))
                        {
                            brush2.Color = this.button_12To14.BackColor;
                        }
                        else if ((num8 > 14f) && (num8 <= 16f))
                        {
                            brush2.Color = this.button_14To16.BackColor;
                        }
                        else if ((num8 > 16f) && (num8 <= 18f))
                        {
                            brush2.Color = this.button_16To18.BackColor;
                        }
                        else if ((num8 > 18f) && (num8 <= 20f))
                        {
                            brush2.Color = this.button_18To20.BackColor;
                        }
                        else if ((num8 > 20f) && (num8 <= 22f))
                        {
                            brush2.Color = this.button_20To22.BackColor;
                        }
                        else if ((num8 > 22f) && (num8 <= 24f))
                        {
                            brush2.Color = this.button_22To24.BackColor;
                        }
                        else if ((num8 > 24f) && (num8 < 26f))
                        {
                            brush2.Color = this.button_24To26.BackColor;
                        }
                        else if ((num8 > 26f) && (num8 < 28f))
                        {
                            brush2.Color = this.button_26To28.BackColor;
                        }
                        else if ((num8 > 28f) && (num8 < 30f))
                        {
                            brush2.Color = this.button_28To30.BackColor;
                        }
                        else if ((num8 > 30f) && (num8 < 32f))
                        {
                            brush2.Color = this.button_30To32.BackColor;
                        }
                        else if ((num8 > 32f) && (num8 <= 34f))
                        {
                            brush2.Color = this.button_32To34.BackColor;
                        }
                        else if ((num8 > 34f) && (num8 <= 36f))
                        {
                            brush2.Color = this.button_34To36.BackColor;
                        }
                        else if ((num8 > 36f) && (num8 <= 38f))
                        {
                            brush2.Color = this.button_36To38.BackColor;
                        }
                        else if ((num8 > 38f) && (num8 <= 40f))
                        {
                            brush2.Color = this.button_38To40.BackColor;
                        }
                        else if ((num8 > 40f) && (num8 <= 42f))
                        {
                            brush2.Color = this.button_40To42.BackColor;
                        }
                        else if ((num8 > 42f) && (num8 <= 44f))
                        {
                            brush2.Color = this.button_42To44.BackColor;
                        }
                        else if ((num8 > 44f) && (num8 <= 46f))
                        {
                            brush2.Color = this.button_44To46.BackColor;
                        }
                        else if ((num8 > 46f) && (num8 <= 48f))
                        {
                            brush2.Color = this.button_46To48.BackColor;
                        }
                        else if ((num8 > 48f) && (num8 <= 50f))
                        {
                            brush2.Color = this.button_48To50.BackColor;
                        }
                        else if (num8 > 50f)
                        {
                            brush2.Color = this.button_50To.BackColor;
                        }
                        Rectangle rect = new Rectangle((int) ((i * width) / 18f), (int) ((i * height) / 18f), (int) ((width * (9 - i)) / 9f), (int) ((height * (9 - i)) / 9f));
                        int num9 = (k * 0x2d) - 90;
                        if (num9 < 0)
                        {
                            num9 += 360;
                        }
                        graphics2.FillPie(brush2, rect, (float) num9, 45f);
                        int num10 = 0;
                        int num11 = 0;
                        int num12 = (int) ((width * 1f) / 72f);
                        float num13 = (width / 2f) - ((width * i) / 18f);
                        switch (k)
                        {
                            case 0:
                                num10 = (((int) (width / 2f)) + ((int) (num13 * Math.Sin(0.39269908169872414)))) - num12;
                                num11 = ((int) (height / 2f)) - ((int) (num13 * Math.Cos(0.39269908169872414)));
                                break;

                            case 1:
                                num10 = (((int) (width / 2f)) + ((int) (num13 * Math.Cos(0.39269908169872414)))) - (2 * num12);
                                num11 = ((int) (height / 2f)) - ((int) (num13 * Math.Sin(0.39269908169872414)));
                                break;

                            case 2:
                                num10 = (((int) (width / 2f)) + ((int) (num13 * Math.Cos(0.39269908169872414)))) - ((int) (2.5 * num12));
                                num11 = (((int) (height / 2f)) + ((int) (num13 * Math.Sin(0.39269908169872414)))) - (2 * num12);
                                break;

                            case 3:
                                num10 = (((int) (width / 2f)) + ((int) (num13 * Math.Sin(0.39269908169872414)))) - num12;
                                num11 = (((int) (height / 2f)) + ((int) (num13 * Math.Cos(0.39269908169872414)))) - (3 * num12);
                                break;

                            case 4:
                                num10 = ((int) (width / 2f)) - ((int) (num13 * Math.Sin(0.39269908169872414)));
                                num11 = (((int) (width / 2f)) + ((int) (num13 * Math.Cos(0.39269908169872414)))) - (3 * num12);
                                break;

                            case 5:
                                num10 = (((int) (width / 2f)) - ((int) (num13 * Math.Cos(0.39269908169872414)))) + num12;
                                num11 = (((int) (height / 2f)) + ((int) (num13 * Math.Sin(0.39269908169872414)))) - num12;
                                break;

                            case 6:
                                num10 = ((int) (width / 2f)) - ((int) (num13 * Math.Cos(0.39269908169872414)));
                                num11 = ((int) (height / 2f)) - ((int) (num13 * Math.Sin(0.39269908169872414)));
                                break;

                            case 7:
                                num10 = ((int) (width / 2f)) - ((int) (num13 * Math.Sin(0.39269908169872414)));
                                num11 = ((int) (height / 2f)) - ((int) (num13 * Math.Cos(0.39269908169872414)));
                                break;
                        }
                        if (i != 8)
                        {
                            graphics2.DrawString(string.Format("{0:F0}", num8), new Font("Segoe UI", (float) num3), Brushes.Black, (float) num10, (float) num11);
                        }
                    }
                }
                graphics2.DrawLine(Pens.Black, (float) 0f, (float) (height / 2f), (float) (width * 1f), (float) (height / 2f));
                graphics2.DrawLine(Pens.Black, (float) (width / 2f), (float) 0f, (float) (width / 2f), (float) (height * 1f));
                graphics2.DrawLine(Pens.Black, (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))));
                graphics2.DrawLine(Pens.Black, (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))));
                graphics2.DrawString("N", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (width / 2f), (float) 0f);
                graphics2.DrawString("S", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (width / 2f), (float) ((height * 1f) - 15f));
                graphics2.DrawString("E", new Font("Segoe UI", (float) num3), Brushes.Black, (float) ((width * 1f) - 12f), (float) (height / 2f));
                graphics2.DrawString("W", new Font("Segoe UI", (float) num3), Brushes.Black, (float) 0f, (float) (height / 2f));
                graphics2.DrawString("45", new Font("Segoe UI", (float) num3), Brushes.Black, (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), ((float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828)))) - 10f);
                graphics2.DrawString("135", new Font("Segoe UI", (float) num3), Brushes.Black, (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))));
                graphics2.DrawString("225", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))) - 15.0), ((float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828)))) + 5f);
                graphics2.DrawString("315", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (((float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828)))) - 15f), (float) (((float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828)))) - 20f));
                graphics2.DrawEllipse(Pens.Black, (float) 0f, (float) 0f, (float) (width - 1f), (float) (height - 1f));
                for (int j = 1; j < 10; j++)
                {
                    graphics2.DrawEllipse(Pens.Black, (float) ((width * j) / 18f), (float) ((height * j) / 18f), (float) ((width * (9 - j)) / 9f), (float) ((height * (9 - j)) / 9f));
                }
                graphics.Render(e.Graphics);
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
                this.Text = this.comm.sourceDeviceName + ": SV CNo ";
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

