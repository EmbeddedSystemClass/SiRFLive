﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using OpenNETCF.IO.Serial;
    using SiRFLive.Communication;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmTTBOpen : Form
    {
        private Button button_Cancel;
        private Button button_OK;
        private ComboBox cboBaud;
        private ComboBox cboData;
        private ComboBox cboParity;
        private ComboBox cboPort;
        private ComboBox cboStop;
        private CommunicationManager comm;
        private IContainer components;
        private Label Label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        public frmTTBOpen()
        {
            this.InitializeComponent();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.cboPort.Text == "NONE")
            {
                this.comm.TTBPort.Close();
                base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                try
                {
                    if (this.comm.TTBPort.IsOpen)
                    {
                        this.comm.TTBPort.Close();
                        this.comm.TTBPort = new CommWrapper();
                    }
                    this.comm.TTBPort.PortName = this.cboPort.Text;
                    this.comm.TTBPort.BaudRate = int.Parse(this.cboBaud.Text);
                    this.comm.TTBPort.DataBits = int.Parse(this.cboData.Text);
                    this.comm.TTBPort.Parity = (Parity) Enum.Parse(typeof(Parity), this.cboParity.Text);
                    this.comm.TTBPort.StopBits = (StopBits) Enum.Parse(typeof(StopBits), this.cboStop.Text);
                    this.comm.TTBPortNum = this.comm.TTBPort.PortName.Trim(new char[] { 'C', 'c', 'O', 'c', 'M', 'm' });
                    this.comm.TTBPort.Open();
                    base.DialogResult = DialogResult.OK;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmTTBOpen_Load(object sender, EventArgs e)
        {
            this.cboPort.Items.Clear();
            this.cboParity.Items.Clear();
            this.cboStop.Items.Clear();
            this.cboPort.Items.Add("NONE");
            this.LoadValues();
            if ((this.comm.TTBPort != null) && this.comm.TTBPort.IsOpen)
            {
                this.cboPort.Text = this.comm.TTBPort.PortName;
                this.cboBaud.Text = this.comm.TTBPort.BaudRate.ToString();
                this.cboParity.Text = this.comm.TTBPort.Parity.ToString();
                this.cboStop.Text = this.comm.TTBPort.StopBits.ToString();
                this.cboData.Text = this.comm.TTBPort.DataBits.ToString();
            }
            else
            {
                this.SetDefaults();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmTTBOpen));
            this.label5 = new Label();
            this.cboData = new ComboBox();
            this.label4 = new Label();
            this.cboStop = new ComboBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.cboParity = new ComboBox();
            this.Label1 = new Label();
            this.cboBaud = new ComboBox();
            this.cboPort = new ComboBox();
            this.button_OK = new Button();
            this.button_Cancel = new Button();
            base.SuspendLayout();
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x4b, 0xbd);
            this.label5.Name = "label5";
            this.label5.Size = new Size(50, 13);
            this.label5.TabIndex = 0x13;
            this.label5.Text = "Data Bits";
            this.cboData.FormattingEnabled = true;
            this.cboData.Items.AddRange(new object[] { "7", "8", "9" });
            this.cboData.Location = new Point(0x8b, 0xb9);
            this.cboData.Name = "cboData";
            this.cboData.Size = new Size(0x4c, 0x15);
            this.cboData.TabIndex = 4;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x4b, 0x95);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x31, 13);
            this.label4.TabIndex = 0x12;
            this.label4.Text = "Stop Bits";
            this.cboStop.FormattingEnabled = true;
            this.cboStop.Location = new Point(0x8b, 0x91);
            this.cboStop.Name = "cboStop";
            this.cboStop.Size = new Size(0x4c, 0x15);
            this.cboStop.TabIndex = 3;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x4b, 0x6d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x21, 13);
            this.label3.TabIndex = 0x11;
            this.label3.Text = "Parity";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x4b, 0x45);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3a, 13);
            this.label2.TabIndex = 0x10;
            this.label2.Text = "Baud Rate";
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Location = new Point(0x8b, 0x69);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new Size(0x4c, 0x15);
            this.cboParity.TabIndex = 2;
            this.Label1.AutoSize = true;
            this.Label1.Location = new Point(0x4b, 0x1d);
            this.Label1.Name = "Label1";
            this.Label1.Size = new Size(0x37, 13);
            this.Label1.TabIndex = 15;
            this.Label1.Text = "Serial Port";
            this.cboBaud.FormattingEnabled = true;
            this.cboBaud.Items.AddRange(new object[] { "300", "600", "1200", "2400", "4800", "9600", "14400", "28800", "38400", "57600", "115200", "230400", "460800" });
            this.cboBaud.Location = new Point(0x8b, 0x41);
            this.cboBaud.Name = "cboBaud";
            this.cboBaud.Size = new Size(0x4c, 0x15);
            this.cboBaud.TabIndex = 1;
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new Point(0x8b, 0x19);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new Size(0x4c, 0x15);
            this.cboPort.TabIndex = 0;
            this.button_OK.Anchor = AnchorStyles.Bottom;
            this.button_OK.Location = new Point(0x3b, 0xe0);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new Size(0x4b, 0x17);
            this.button_OK.TabIndex = 5;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new EventHandler(this.button_OK_Click);
            this.button_Cancel.Anchor = AnchorStyles.Bottom;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(0x9c, 0xe0);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            base.AcceptButton = this.button_OK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(290, 0x111);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.cboData);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.button_OK);
            base.Controls.Add(this.cboStop);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cboPort);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cboBaud);
            base.Controls.Add(this.cboParity);
            base.Controls.Add(this.Label1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmTTBOpen";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Open TTB Port";
            base.Load += new EventHandler(this.frmTTBOpen_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadValues()
        {
            this.comm.SetPortNameValues(this.cboPort);
            this.comm.SetParityValues(this.cboParity);
            this.comm.SetStopBitValues(this.cboStop);
        }

        private void SetDefaults()
        {
            this.cboPort.SelectedText = "COM10";
            this.cboBaud.SelectedText = "57600";
            this.cboParity.SelectedIndex = 0;
            this.cboStop.SelectedIndex = 0;
            this.cboData.SelectedIndex = 1;
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
            }
        }
    }
}

