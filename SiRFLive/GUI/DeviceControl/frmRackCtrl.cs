﻿namespace SiRFLive.GUI.DeviceControl
{
    using AxTestControl;
    using SiRFLive.DeviceControl;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmRackCtrl : Form
    {
        private AxControlHW axControlHW1;
        private IContainer components;
        private Label frmTestCtrlAttenPLabel;
        private TextBox frmTestCtrlAttenVal;
        private Label frmTestCtrlAttenValLabel;
        private Button frmTestCtrlInitBut;
        private CheckBox frmTestCtrlP1Chk;
        private CheckBox frmTestCtrlP1StatsChk;
        private CheckBox frmTestCtrlP2Chk;
        private CheckBox frmTestCtrlP2StatsChk;
        private CheckBox frmTestCtrlP3Chk;
        private CheckBox frmTestCtrlP3StatsChk;
        private CheckBox frmTestCtrlP4Chk;
        private CheckBox frmTestCtrlP4StatsChk;
        private CheckBox frmTestCtrlP5Chk;
        private CheckBox frmTestCtrlP5StatsChk;
        private CheckBox frmTestCtrlP6Chk;
        private CheckBox frmTestCtrlP6StatsChk;
        private CheckBox frmTestCtrlP7Chk;
        private CheckBox frmTestCtrlP7StatsChk;
        private CheckBox frmTestCtrlP8Chk;
        private CheckBox frmTestCtrlP8StatsChk;
        private TextBox frmTestCtrlPortVal;
        private Label frmTestCtrlPwrSettingLabel;
        private Label frmTestCtrlPwrStatusLabel;
        private Button frmTestCtrlQueryPwrBut;
        private Button frmTestCtrlSetAttenBut;
        private Button frmTestCtrlSetPwrBut;
        private TestRackMgr hwCtrl = new TestRackMgr();
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private static frmRackCtrl m_SChildform;

        public frmRackCtrl()
        {
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

        private void frmTestCtrlInitBut_Click(object sender, EventArgs e)
        {
            if (this.hwCtrl.Init() != 0)
            {
                MessageBox.Show("Error initializing HW!", "ERROR!");
            }
        }

        private void frmTestCtrlP1Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(1, this.frmTestCtrlP1Chk.Checked);
            this.frmTestCtrlP1StatsChk.Checked = this.frmTestCtrlP1Chk.Checked;
        }

        private void frmTestCtrlP2Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(2, this.frmTestCtrlP2Chk.Checked);
            this.frmTestCtrlP2StatsChk.Checked = this.frmTestCtrlP2Chk.Checked;
        }

        private void frmTestCtrlP3Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(3, this.frmTestCtrlP3Chk.Checked);
            this.frmTestCtrlP3StatsChk.Checked = this.frmTestCtrlP3Chk.Checked;
        }

        private void frmTestCtrlP4Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(4, this.frmTestCtrlP1Chk.Checked);
            this.frmTestCtrlP4StatsChk.Checked = this.frmTestCtrlP4Chk.Checked;
        }

        private void frmTestCtrlP5Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(5, this.frmTestCtrlP1Chk.Checked);
            this.frmTestCtrlP5StatsChk.Checked = this.frmTestCtrlP5Chk.Checked;
        }

        private void frmTestCtrlP6Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(6, this.frmTestCtrlP1Chk.Checked);
            this.frmTestCtrlP6StatsChk.Checked = this.frmTestCtrlP6Chk.Checked;
        }

        private void frmTestCtrlP7Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(7, this.frmTestCtrlP1Chk.Checked);
            this.frmTestCtrlP7StatsChk.Checked = this.frmTestCtrlP7Chk.Checked;
        }

        private void frmTestCtrlP8Chk_CheckedChanged(object sender, EventArgs e)
        {
            this.hwCtrl.Power(8, this.frmTestCtrlP1Chk.Checked);
            this.frmTestCtrlP8StatsChk.Checked = this.frmTestCtrlP8Chk.Checked;
        }

        private void frmTestCtrlSetAttenBut_Click(object sender, EventArgs e)
        {
            if ((this.frmTestCtrlAttenVal.Text.Length == 0) || (this.frmTestCtrlPortVal.Text.Length == 0))
            {
                MessageBox.Show("Either Port or Attenuation is blank", "ERROR!");
            }
            else
            {
                try
                {
                    this.hwCtrl.SetAtten(Convert.ToInt16(this.frmTestCtrlPortVal.Text), Convert.ToInt16(this.frmTestCtrlAttenVal.Text));
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "ERROR");
                }
            }
        }

        private void frmTestCtrlSetPwrBut_Click(object sender, EventArgs e)
        {
            int[] values = new int[9];
            int num = 0;
            values[num++] = this.frmTestCtrlP1Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP2Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP3Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP4Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP5Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP6Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP7Chk.Checked ? 1 : 0;
            values[num++] = this.frmTestCtrlP8Chk.Checked ? 1 : 0;
            this.hwCtrl.Power(values);
            this.frmTestCtrlP1StatsChk.Checked = this.frmTestCtrlP1Chk.Checked;
            this.frmTestCtrlP2StatsChk.Checked = this.frmTestCtrlP2Chk.Checked;
            this.frmTestCtrlP3StatsChk.Checked = this.frmTestCtrlP3Chk.Checked;
            this.frmTestCtrlP4StatsChk.Checked = this.frmTestCtrlP4Chk.Checked;
            this.frmTestCtrlP5StatsChk.Checked = this.frmTestCtrlP5Chk.Checked;
            this.frmTestCtrlP6StatsChk.Checked = this.frmTestCtrlP6Chk.Checked;
            this.frmTestCtrlP7StatsChk.Checked = this.frmTestCtrlP7Chk.Checked;
            this.frmTestCtrlP8StatsChk.Checked = this.frmTestCtrlP8Chk.Checked;
        }

        public static frmRackCtrl GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRackCtrl();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmRackCtrl));
            this.frmTestCtrlPwrStatusLabel = new Label();
            this.frmTestCtrlP1StatsChk = new CheckBox();
            this.frmTestCtrlP2StatsChk = new CheckBox();
            this.frmTestCtrlP3StatsChk = new CheckBox();
            this.frmTestCtrlP4StatsChk = new CheckBox();
            this.frmTestCtrlP5StatsChk = new CheckBox();
            this.frmTestCtrlP6StatsChk = new CheckBox();
            this.frmTestCtrlP7StatsChk = new CheckBox();
            this.frmTestCtrlP8StatsChk = new CheckBox();
            this.frmTestCtrlAttenValLabel = new Label();
            this.frmTestCtrlAttenVal = new TextBox();
            this.frmTestCtrlPwrSettingLabel = new Label();
            this.frmTestCtrlP1Chk = new CheckBox();
            this.frmTestCtrlP2Chk = new CheckBox();
            this.frmTestCtrlP3Chk = new CheckBox();
            this.frmTestCtrlP4Chk = new CheckBox();
            this.frmTestCtrlP5Chk = new CheckBox();
            this.frmTestCtrlP6Chk = new CheckBox();
            this.frmTestCtrlP7Chk = new CheckBox();
            this.frmTestCtrlP8Chk = new CheckBox();
            this.frmTestCtrlQueryPwrBut = new Button();
            this.frmTestCtrlSetPwrBut = new Button();
            this.frmTestCtrlSetAttenBut = new Button();
            this.frmTestCtrlInitBut = new Button();
            this.axControlHW1 = new AxControlHW();
            this.frmTestCtrlPortVal = new TextBox();
            this.frmTestCtrlAttenPLabel = new Label();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.axControlHW1.BeginInit();
            base.SuspendLayout();
            this.frmTestCtrlPwrStatusLabel.AutoSize = true;
            this.frmTestCtrlPwrStatusLabel.Location = new Point(0x40, 0x2a);
            this.frmTestCtrlPwrStatusLabel.Name = "frmTestCtrlPwrStatusLabel";
            this.frmTestCtrlPwrStatusLabel.Size = new Size(70, 13);
            this.frmTestCtrlPwrStatusLabel.TabIndex = 8;
            this.frmTestCtrlPwrStatusLabel.Text = "Power Status";
            this.frmTestCtrlP1StatsChk.AutoSize = true;
            this.frmTestCtrlP1StatsChk.Enabled = false;
            this.frmTestCtrlP1StatsChk.Location = new Point(0x9c, 0x29);
            this.frmTestCtrlP1StatsChk.Name = "frmTestCtrlP1StatsChk";
            this.frmTestCtrlP1StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP1StatsChk.TabIndex = 9;
            this.frmTestCtrlP1StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP2StatsChk.AutoSize = true;
            this.frmTestCtrlP2StatsChk.Enabled = false;
            this.frmTestCtrlP2StatsChk.Location = new Point(0xba, 0x29);
            this.frmTestCtrlP2StatsChk.Name = "frmTestCtrlP2StatsChk";
            this.frmTestCtrlP2StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP2StatsChk.TabIndex = 10;
            this.frmTestCtrlP2StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP3StatsChk.AutoSize = true;
            this.frmTestCtrlP3StatsChk.Enabled = false;
            this.frmTestCtrlP3StatsChk.Location = new Point(0xd8, 0x29);
            this.frmTestCtrlP3StatsChk.Name = "frmTestCtrlP3StatsChk";
            this.frmTestCtrlP3StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP3StatsChk.TabIndex = 11;
            this.frmTestCtrlP3StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP4StatsChk.AutoSize = true;
            this.frmTestCtrlP4StatsChk.Enabled = false;
            this.frmTestCtrlP4StatsChk.Location = new Point(0xf6, 0x29);
            this.frmTestCtrlP4StatsChk.Name = "frmTestCtrlP4StatsChk";
            this.frmTestCtrlP4StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP4StatsChk.TabIndex = 12;
            this.frmTestCtrlP4StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP5StatsChk.AutoSize = true;
            this.frmTestCtrlP5StatsChk.Enabled = false;
            this.frmTestCtrlP5StatsChk.Location = new Point(0x114, 0x29);
            this.frmTestCtrlP5StatsChk.Name = "frmTestCtrlP5StatsChk";
            this.frmTestCtrlP5StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP5StatsChk.TabIndex = 13;
            this.frmTestCtrlP5StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP6StatsChk.AutoSize = true;
            this.frmTestCtrlP6StatsChk.Enabled = false;
            this.frmTestCtrlP6StatsChk.Location = new Point(0x132, 0x29);
            this.frmTestCtrlP6StatsChk.Name = "frmTestCtrlP6StatsChk";
            this.frmTestCtrlP6StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP6StatsChk.TabIndex = 14;
            this.frmTestCtrlP6StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP7StatsChk.AutoSize = true;
            this.frmTestCtrlP7StatsChk.Enabled = false;
            this.frmTestCtrlP7StatsChk.Location = new Point(0x150, 0x29);
            this.frmTestCtrlP7StatsChk.Name = "frmTestCtrlP7StatsChk";
            this.frmTestCtrlP7StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP7StatsChk.TabIndex = 15;
            this.frmTestCtrlP7StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP8StatsChk.AutoSize = true;
            this.frmTestCtrlP8StatsChk.Enabled = false;
            this.frmTestCtrlP8StatsChk.Location = new Point(0x16e, 0x29);
            this.frmTestCtrlP8StatsChk.Name = "frmTestCtrlP8StatsChk";
            this.frmTestCtrlP8StatsChk.Size = new Size(15, 14);
            this.frmTestCtrlP8StatsChk.TabIndex = 0x10;
            this.frmTestCtrlP8StatsChk.UseVisualStyleBackColor = true;
            this.frmTestCtrlAttenValLabel.AutoSize = true;
            this.frmTestCtrlAttenValLabel.Location = new Point(0x40, 0x68);
            this.frmTestCtrlAttenValLabel.Name = "frmTestCtrlAttenValLabel";
            this.frmTestCtrlAttenValLabel.Size = new Size(0x3d, 13);
            this.frmTestCtrlAttenValLabel.TabIndex = 0x1a;
            this.frmTestCtrlAttenValLabel.Text = "Attenuation";
            this.frmTestCtrlAttenVal.Location = new Point(0x9c, 100);
            this.frmTestCtrlAttenVal.Name = "frmTestCtrlAttenVal";
            this.frmTestCtrlAttenVal.Size = new Size(0x2d, 20);
            this.frmTestCtrlAttenVal.TabIndex = 0x1b;
            this.frmTestCtrlPwrSettingLabel.AutoSize = true;
            this.frmTestCtrlPwrSettingLabel.Location = new Point(0x40, 0x47);
            this.frmTestCtrlPwrSettingLabel.Name = "frmTestCtrlPwrSettingLabel";
            this.frmTestCtrlPwrSettingLabel.Size = new Size(0x49, 13);
            this.frmTestCtrlPwrSettingLabel.TabIndex = 0x11;
            this.frmTestCtrlPwrSettingLabel.Text = "Power Setting";
            this.frmTestCtrlP1Chk.AutoSize = true;
            this.frmTestCtrlP1Chk.Location = new Point(0x9c, 70);
            this.frmTestCtrlP1Chk.Name = "frmTestCtrlP1Chk";
            this.frmTestCtrlP1Chk.Size = new Size(15, 14);
            this.frmTestCtrlP1Chk.TabIndex = 0x12;
            this.frmTestCtrlP1Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP1Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP1Chk_CheckedChanged);
            this.frmTestCtrlP2Chk.AutoSize = true;
            this.frmTestCtrlP2Chk.Location = new Point(0xba, 70);
            this.frmTestCtrlP2Chk.Name = "frmTestCtrlP2Chk";
            this.frmTestCtrlP2Chk.Size = new Size(15, 14);
            this.frmTestCtrlP2Chk.TabIndex = 0x13;
            this.frmTestCtrlP2Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP2Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP2Chk_CheckedChanged);
            this.frmTestCtrlP3Chk.AutoSize = true;
            this.frmTestCtrlP3Chk.Location = new Point(0xd8, 70);
            this.frmTestCtrlP3Chk.Name = "frmTestCtrlP3Chk";
            this.frmTestCtrlP3Chk.Size = new Size(15, 14);
            this.frmTestCtrlP3Chk.TabIndex = 20;
            this.frmTestCtrlP3Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP3Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP3Chk_CheckedChanged);
            this.frmTestCtrlP4Chk.AutoSize = true;
            this.frmTestCtrlP4Chk.Location = new Point(0xf6, 70);
            this.frmTestCtrlP4Chk.Name = "frmTestCtrlP4Chk";
            this.frmTestCtrlP4Chk.Size = new Size(15, 14);
            this.frmTestCtrlP4Chk.TabIndex = 0x15;
            this.frmTestCtrlP4Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP4Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP4Chk_CheckedChanged);
            this.frmTestCtrlP5Chk.AutoSize = true;
            this.frmTestCtrlP5Chk.Location = new Point(0x114, 70);
            this.frmTestCtrlP5Chk.Name = "frmTestCtrlP5Chk";
            this.frmTestCtrlP5Chk.Size = new Size(15, 14);
            this.frmTestCtrlP5Chk.TabIndex = 0x16;
            this.frmTestCtrlP5Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP5Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP5Chk_CheckedChanged);
            this.frmTestCtrlP6Chk.AutoSize = true;
            this.frmTestCtrlP6Chk.Location = new Point(0x132, 70);
            this.frmTestCtrlP6Chk.Name = "frmTestCtrlP6Chk";
            this.frmTestCtrlP6Chk.Size = new Size(15, 14);
            this.frmTestCtrlP6Chk.TabIndex = 0x17;
            this.frmTestCtrlP6Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP6Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP6Chk_CheckedChanged);
            this.frmTestCtrlP7Chk.AutoSize = true;
            this.frmTestCtrlP7Chk.Location = new Point(0x150, 70);
            this.frmTestCtrlP7Chk.Name = "frmTestCtrlP7Chk";
            this.frmTestCtrlP7Chk.Size = new Size(15, 14);
            this.frmTestCtrlP7Chk.TabIndex = 0x18;
            this.frmTestCtrlP7Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP7Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP7Chk_CheckedChanged);
            this.frmTestCtrlP8Chk.AutoSize = true;
            this.frmTestCtrlP8Chk.Location = new Point(0x16e, 70);
            this.frmTestCtrlP8Chk.Name = "frmTestCtrlP8Chk";
            this.frmTestCtrlP8Chk.Size = new Size(15, 14);
            this.frmTestCtrlP8Chk.TabIndex = 0x19;
            this.frmTestCtrlP8Chk.UseVisualStyleBackColor = true;
            this.frmTestCtrlP8Chk.CheckedChanged += new EventHandler(this.frmTestCtrlP8Chk_CheckedChanged);
            this.frmTestCtrlQueryPwrBut.Location = new Point(0x9c, 0x9a);
            this.frmTestCtrlQueryPwrBut.Name = "frmTestCtrlQueryPwrBut";
            this.frmTestCtrlQueryPwrBut.Size = new Size(0x69, 0x1c);
            this.frmTestCtrlQueryPwrBut.TabIndex = 30;
            this.frmTestCtrlQueryPwrBut.Text = "Query Power";
            this.frmTestCtrlQueryPwrBut.UseVisualStyleBackColor = true;
            this.frmTestCtrlSetPwrBut.Location = new Point(0x9c, 0xc9);
            this.frmTestCtrlSetPwrBut.Name = "frmTestCtrlSetPwrBut";
            this.frmTestCtrlSetPwrBut.Size = new Size(0x69, 0x1c);
            this.frmTestCtrlSetPwrBut.TabIndex = 0x20;
            this.frmTestCtrlSetPwrBut.Text = "Set Power";
            this.frmTestCtrlSetPwrBut.UseVisualStyleBackColor = true;
            this.frmTestCtrlSetPwrBut.Click += new EventHandler(this.frmTestCtrlSetPwrBut_Click);
            this.frmTestCtrlSetAttenBut.Location = new Point(0x114, 0xc9);
            this.frmTestCtrlSetAttenBut.Name = "frmTestCtrlSetAttenBut";
            this.frmTestCtrlSetAttenBut.Size = new Size(0x69, 0x1c);
            this.frmTestCtrlSetAttenBut.TabIndex = 0x21;
            this.frmTestCtrlSetAttenBut.Text = "Set Attenuation";
            this.frmTestCtrlSetAttenBut.UseVisualStyleBackColor = true;
            this.frmTestCtrlSetAttenBut.Click += new EventHandler(this.frmTestCtrlSetAttenBut_Click);
            this.frmTestCtrlInitBut.Location = new Point(0x114, 0x9a);
            this.frmTestCtrlInitBut.Name = "frmTestCtrlInitBut";
            this.frmTestCtrlInitBut.Size = new Size(0x69, 0x1c);
            this.frmTestCtrlInitBut.TabIndex = 0x1f;
            this.frmTestCtrlInitBut.Text = "Initialization";
            this.frmTestCtrlInitBut.UseVisualStyleBackColor = true;
            this.frmTestCtrlInitBut.Click += new EventHandler(this.frmTestCtrlInitBut_Click);
            this.axControlHW1.Enabled = true;
            this.axControlHW1.Location = new Point(0x21, 0xc4);
            this.axControlHW1.Name = "axControlHW1";
            this.axControlHW1.OcxState = (AxHost.State) manager.GetObject("axControlHW1.OcxState");
            this.axControlHW1.Size = new Size(0x70, 0x21);
            this.axControlHW1.TabIndex = 8;
            this.frmTestCtrlPortVal.Location = new Point(0x114, 100);
            this.frmTestCtrlPortVal.Name = "frmTestCtrlPortVal";
            this.frmTestCtrlPortVal.Size = new Size(0x2d, 20);
            this.frmTestCtrlPortVal.TabIndex = 0x1d;
            this.frmTestCtrlAttenPLabel.AutoSize = true;
            this.frmTestCtrlAttenPLabel.Location = new Point(0xf4, 0x68);
            this.frmTestCtrlAttenPLabel.Name = "frmTestCtrlAttenPLabel";
            this.frmTestCtrlAttenPLabel.Size = new Size(0x1a, 13);
            this.frmTestCtrlAttenPLabel.TabIndex = 0x1c;
            this.frmTestCtrlAttenPLabel.Text = "Port";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xb5, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(20, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "P2";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x97, 0x18);
            this.label2.Name = "label2";
            this.label2.Size = new Size(20, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "P1";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xd5, 0x18);
            this.label3.Name = "label3";
            this.label3.Size = new Size(20, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "P3";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xf1, 0x18);
            this.label4.Name = "label4";
            this.label4.Size = new Size(20, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "P4";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x111, 0x18);
            this.label5.Name = "label5";
            this.label5.Size = new Size(20, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "P5";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x12f, 0x18);
            this.label6.Name = "label6";
            this.label6.Size = new Size(20, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "P6";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x14d, 0x18);
            this.label7.Name = "label7";
            this.label7.Size = new Size(20, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "P7";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x16b, 0x18);
            this.label8.Name = "label8";
            this.label8.Size = new Size(20, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "P8";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x1a0, 0xfc);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.axControlHW1);
            base.Controls.Add(this.frmTestCtrlSetAttenBut);
            base.Controls.Add(this.frmTestCtrlSetPwrBut);
            base.Controls.Add(this.frmTestCtrlInitBut);
            base.Controls.Add(this.frmTestCtrlQueryPwrBut);
            base.Controls.Add(this.frmTestCtrlPwrSettingLabel);
            base.Controls.Add(this.frmTestCtrlPortVal);
            base.Controls.Add(this.frmTestCtrlAttenVal);
            base.Controls.Add(this.frmTestCtrlAttenPLabel);
            base.Controls.Add(this.frmTestCtrlAttenValLabel);
            base.Controls.Add(this.frmTestCtrlP8Chk);
            base.Controls.Add(this.frmTestCtrlP8StatsChk);
            base.Controls.Add(this.frmTestCtrlP7Chk);
            base.Controls.Add(this.frmTestCtrlP6Chk);
            base.Controls.Add(this.frmTestCtrlP7StatsChk);
            base.Controls.Add(this.frmTestCtrlP5Chk);
            base.Controls.Add(this.frmTestCtrlP6StatsChk);
            base.Controls.Add(this.frmTestCtrlP4Chk);
            base.Controls.Add(this.frmTestCtrlP5StatsChk);
            base.Controls.Add(this.frmTestCtrlP3Chk);
            base.Controls.Add(this.frmTestCtrlP4StatsChk);
            base.Controls.Add(this.frmTestCtrlP2Chk);
            base.Controls.Add(this.frmTestCtrlP3StatsChk);
            base.Controls.Add(this.frmTestCtrlP1Chk);
            base.Controls.Add(this.frmTestCtrlP2StatsChk);
            base.Controls.Add(this.frmTestCtrlP1StatsChk);
            base.Controls.Add(this.frmTestCtrlPwrStatusLabel);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmRackCtrl";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Test Rack Control";
            this.axControlHW1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }
    }
}

