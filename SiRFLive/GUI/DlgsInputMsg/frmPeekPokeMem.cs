﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public class frmPeekPokeMem : Form
    {
        private long _pokeFileSize;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton frmPeekPokeMem16bitRadioBtn;
        private RadioButton frmPeekPokeMem32bitRadioBtn;
        private RadioButton frmPeekPokeMem8bitRadioBtn;
        private Label frmPeekPokeMemAddrLabel;
        private TextBox frmPeekPokeMemAddrTextBox;
        private Button frmPeekPokeMemBrowserBtn;
        private Button frmPeekPokeMemCancelBtn;
        private Label frmPeekPokeMemDataCountLabel;
        private TextBox frmPeekPokeMemDataCountTextBox;
        private TextBox frmPeekPokeMemFilePathTextBox;
        private RadioButton frmPeekPokeMemMultiPeekRadioBtn;
        private RadioButton frmPeekPokeMemMultiPokeRadioBtn;
        private Button frmPeekPokeMemOkBtn;
        private RadioButton frmPeekPokeMemPeekRadioBtn;
        private RadioButton frmPeekPokeMemPokeRadioBtn;
        private Label frmPeekPokeMemValueLabel;
        private TextBox frmPeekPokeMemValueTextBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label7;
        private Label label8;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmPeekPokeMem(CommunicationManager commMgr)
        {
            this.InitializeComponent();
            this.comm = commMgr;
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmPeekPokeMem_Load(object sender, EventArgs e)
        {
            switch (this.comm.TrackerICCtrl.PeekPoke)
            {
                case 0:
                    this.frmPeekPokeMemPeekRadioBtn.Checked = true;
                    this.frmPeekPokeMemFilePathTextBox.ReadOnly = true;
                    this.frmPeekPokeMemBrowserBtn.Enabled = false;
                    break;

                case 1:
                    this.frmPeekPokeMemPokeRadioBtn.Checked = true;
                    this.frmPeekPokeMemFilePathTextBox.ReadOnly = true;
                    this.frmPeekPokeMemBrowserBtn.Enabled = false;
                    break;

                case 2:
                    this.frmPeekPokeMemMultiPeekRadioBtn.Checked = true;
                    this.frmPeekPokeMemFilePathTextBox.ReadOnly = true;
                    this.frmPeekPokeMemBrowserBtn.Enabled = false;
                    break;

                case 3:
                    this.frmPeekPokeMemMultiPokeRadioBtn.Checked = true;
                    this.frmPeekPokeMemFilePathTextBox.ReadOnly = false;
                    this.frmPeekPokeMemBrowserBtn.Enabled = true;
                    break;

                default:
                    this.frmPeekPokeMemPeekRadioBtn.Checked = true;
                    this.frmPeekPokeMemFilePathTextBox.ReadOnly = true;
                    this.frmPeekPokeMemBrowserBtn.Enabled = false;
                    break;
            }
            switch (this.comm.TrackerICCtrl.Access)
            {
                case 1:
                    this.frmPeekPokeMem8bitRadioBtn.Checked = true;
                    break;

                case 2:
                    this.frmPeekPokeMem16bitRadioBtn.Checked = true;
                    break;

                case 3:
                    this.frmPeekPokeMem32bitRadioBtn.Checked = true;
                    break;

                default:
                    this.frmPeekPokeMem32bitRadioBtn.Checked = true;
                    break;
            }
            this.frmPeekPokeMemAddrTextBox.Text = this.comm.TrackerICCtrl.Address.ToString("X");
            this.frmPeekPokeMemDataCountTextBox.Text = this.comm.TrackerICCtrl.NumBytes.ToString();
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
        }

        private void frmPeekPokeMemBrowserBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Specify Poke File Name:";
            dialog.InitialDirectory = @"C:\";
            dialog.Filter = "Binary files(*.bin)|*.bin|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.CheckPathExists = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.frmPeekPokeMemFilePathTextBox.Text = dialog.FileName;
                FileInfo info = new FileInfo(this.frmPeekPokeMemFilePathTextBox.Text);
                this._pokeFileSize = info.Length;
                this.frmPeekPokeMemDataCountTextBox.Text = this._pokeFileSize.ToString();
            }
        }

        private void frmPeekPokeMemCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmPeekPokeMemMultiPeekRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmPeekPokeMemMultiPeekRadioBtn.Checked)
            {
                this.frmPeekPokeMemValueTextBox.ReadOnly = true;
                this.frmPeekPokeMemFilePathTextBox.ReadOnly = true;
                this.frmPeekPokeMemFilePathTextBox.Text = @"C:\MultiPeek.bin";
                this.frmPeekPokeMemBrowserBtn.Enabled = false;
            }
        }

        private void frmPeekPokeMemMultiPokeRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmPeekPokeMemMultiPokeRadioBtn.Checked)
            {
                this.frmPeekPokeMemFilePathTextBox.ReadOnly = false;
                this.frmPeekPokeMemBrowserBtn.Enabled = true;
                this.frmPeekPokeMemValueTextBox.ReadOnly = true;
                this.frmPeekPokeMemDataCountTextBox.ReadOnly = true;
                bool flag = false;
                if (this.frmPeekPokeMemFilePathTextBox.Text != string.Empty)
                {
                    if (!File.Exists(this.frmPeekPokeMemFilePathTextBox.Text))
                    {
                        flag = true;
                    }
                    else
                    {
                        FileInfo info = new FileInfo(this.frmPeekPokeMemFilePathTextBox.Text);
                        this._pokeFileSize = info.Length;
                        this.frmPeekPokeMemDataCountTextBox.Text = this._pokeFileSize.ToString();
                    }
                }
                else
                {
                    flag = true;
                }
                if (flag)
                {
                    MessageBox.Show(string.Format("File does not exist: \n {0}", this.frmPeekPokeMemFilePathTextBox.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                this.frmPeekPokeMemFilePathTextBox.ReadOnly = true;
                this.frmPeekPokeMemBrowserBtn.Enabled = false;
            }
        }

        private void frmPeekPokeMemOkBtn_Click(object sender, EventArgs e)
        {
            string str21;
            string str = string.Empty;
            if (this.frmPeekPokeMem8bitRadioBtn.Checked)
            {
                this.comm.TrackerICCtrl.Access = 1;
            }
            else if (this.frmPeekPokeMem16bitRadioBtn.Checked)
            {
                this.comm.TrackerICCtrl.Access = 2;
            }
            else if (this.frmPeekPokeMem32bitRadioBtn.Checked)
            {
                this.comm.TrackerICCtrl.Access = 4;
            }
            else
            {
                this.comm.TrackerICCtrl.Access = 4;
            }
            if (this.frmPeekPokeMemPeekRadioBtn.Checked)
            {
                this.frmPeekPokeMem32bitRadioBtn.Checked = true;
                this.comm.TrackerICCtrl.PeekPoke = 0;
                try
                {
                    this.comm.TrackerICCtrl.Address = Convert.ToUInt32(this.frmPeekPokeMemAddrTextBox.Text, 0x10);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Address: " + exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                StringBuilder builder = new StringBuilder();
                builder.Append("B203");
                builder.Append(this.comm.TrackerICCtrl.PeekPoke.ToString("X2"));
                builder.Append(this.comm.TrackerICCtrl.Access.ToString("X2"));
                builder.Append(this.comm.TrackerICCtrl.Address.ToString("X8"));
                builder.Append("00000000");
                string message = builder.ToString();
                str = ((message.Length / 2)).ToString("X4") + message + this.comm.m_Protocols.GetChecksum(message, false);
            }
            else
            {
                if (this.frmPeekPokeMemPokeRadioBtn.Checked)
                {
                    this.frmPeekPokeMem32bitRadioBtn.Checked = true;
                    this.comm.TrackerICCtrl.PeekPoke = 1;
                    try
                    {
                        this.comm.TrackerICCtrl.Address = Convert.ToUInt32(this.frmPeekPokeMemAddrTextBox.Text, 0x10);
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show("Address: " + exception2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        this.comm.TrackerICCtrl.Data = Convert.ToUInt32(this.frmPeekPokeMemValueTextBox.Text, 0x10).ToString("X8");
                    }
                    catch (Exception exception3)
                    {
                        MessageBox.Show("Value: " + exception3.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    StringBuilder builder2 = new StringBuilder();
                    builder2.Append("B203");
                    builder2.Append(this.comm.TrackerICCtrl.PeekPoke.ToString("X2"));
                    builder2.Append(this.comm.TrackerICCtrl.Access.ToString("X2"));
                    builder2.Append(this.comm.TrackerICCtrl.Address.ToString("X8"));
                    builder2.Append(this.comm.TrackerICCtrl.Data);
                    string str6 = builder2.ToString();
                    try
                    {
                        str = ((str6.Length / 2)).ToString("X4") + str6 + this.comm.m_Protocols.GetChecksum(str6, false);
                        goto Label_0963;
                    }
                    catch (Exception exception4)
                    {
                        MessageBox.Show("Value: " + exception4.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
                if (this.frmPeekPokeMemMultiPeekRadioBtn.Checked)
                {
                    this.comm.TrackerICCtrl.PeekPoke = 2;
                    try
                    {
                        this.comm.TrackerICCtrl.Address = Convert.ToUInt32(this.frmPeekPokeMemAddrTextBox.Text, 0x10);
                    }
                    catch (Exception exception5)
                    {
                        MessageBox.Show("Address: " + exception5.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        int num;
                        Math.DivRem(this.frmPeekPokeMemValueTextBox.Text.Length, 2, out num);
                        if (num == 1)
                        {
                            this.comm.TrackerICCtrl.Data = "0" + this.frmPeekPokeMemValueTextBox.Text;
                        }
                        else
                        {
                            this.comm.TrackerICCtrl.Data = this.frmPeekPokeMemValueTextBox.Text;
                        }
                    }
                    catch (Exception exception6)
                    {
                        MessageBox.Show("Value: " + exception6.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        this.comm.TrackerICCtrl.NumBytes = Convert.ToUInt16(this.frmPeekPokeMemDataCountTextBox.Text);
                        if ((this.comm.TrackerICCtrl.NumBytes < 0) || (this.comm.TrackerICCtrl.NumBytes > 0x3e8))
                        {
                            string text = "Data Count: out of range (0-1000)";
                            MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                    catch (Exception exception7)
                    {
                        MessageBox.Show("Data Count: " + exception7.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    StringBuilder builder3 = new StringBuilder();
                    builder3.Append("B203");
                    builder3.Append(this.comm.TrackerICCtrl.PeekPoke.ToString("X2"));
                    builder3.Append(this.comm.TrackerICCtrl.Access.ToString("X2"));
                    builder3.Append(this.comm.TrackerICCtrl.Address.ToString("X8"));
                    builder3.Append(this.comm.TrackerICCtrl.NumBytes.ToString("X4"));
                    string str12 = builder3.ToString();
                    try
                    {
                        str = ((str12.Length / 2)).ToString("X4") + str12 + this.comm.m_Protocols.GetChecksum(str12, false);
                        goto Label_0963;
                    }
                    catch (Exception exception8)
                    {
                        MessageBox.Show("Value: " + exception8.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
                if (this.frmPeekPokeMemMultiPokeRadioBtn.Checked)
                {
                    this.comm.TrackerICCtrl.PeekPoke = 3;
                    try
                    {
                        this.comm.TrackerICCtrl.Address = Convert.ToUInt32(this.frmPeekPokeMemAddrTextBox.Text, 0x10);
                    }
                    catch (Exception exception9)
                    {
                        MessageBox.Show("Address: " + exception9.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        int num2;
                        Math.DivRem(this.frmPeekPokeMemValueTextBox.Text.Length, 2, out num2);
                        if (num2 == 1)
                        {
                            this.comm.TrackerICCtrl.Data = "0" + this.frmPeekPokeMemValueTextBox.Text;
                        }
                        else
                        {
                            this.comm.TrackerICCtrl.Data = this.frmPeekPokeMemValueTextBox.Text;
                        }
                    }
                    catch (Exception exception10)
                    {
                        MessageBox.Show("Value: " + exception10.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    bool flag = false;
                    if (this.frmPeekPokeMemFilePathTextBox.Text != string.Empty)
                    {
                        if (!File.Exists(this.frmPeekPokeMemFilePathTextBox.Text))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        MessageBox.Show(string.Format("File does not exist: \n {0}", this.frmPeekPokeMemFilePathTextBox.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        this.comm.TrackerICCtrl.NumBytes = Convert.ToUInt16(this.frmPeekPokeMemDataCountTextBox.Text);
                        if ((this.comm.TrackerICCtrl.NumBytes < 0) || (this.comm.TrackerICCtrl.NumBytes > 0x3e8))
                        {
                            string str16 = "Data Count: out of range (0-1000)";
                            MessageBox.Show(str16, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                    catch (Exception exception11)
                    {
                        MessageBox.Show("Data Count: " + exception11.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    byte[] input = File.ReadAllBytes(this.frmPeekPokeMemFilePathTextBox.Text);
                    this.frmPeekPokeMemDataCountTextBox.Text = input.Length.ToString();
                    this.comm.TrackerICCtrl.Data = HelperFunctions.ByteToHex(input).Replace(" ", "");
                    StringBuilder builder4 = new StringBuilder();
                    builder4.Append("B203");
                    builder4.Append(this.comm.TrackerICCtrl.PeekPoke.ToString("X2"));
                    builder4.Append(this.comm.TrackerICCtrl.Access.ToString("X2"));
                    builder4.Append(this.comm.TrackerICCtrl.Address.ToString("X8"));
                    builder4.Append(this.comm.TrackerICCtrl.NumBytes.ToString("X4"));
                    builder4.Append(this.comm.TrackerICCtrl.Data);
                    string str19 = builder4.ToString();
                    try
                    {
                        str = ((str19.Length / 2)).ToString("X4") + str19 + this.comm.m_Protocols.GetChecksum(str19, false);
                    }
                    catch (Exception exception12)
                    {
                        MessageBox.Show("Value: " + exception12.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
            }
        Label_0963:
            str21 = "A0A2" + str + "B0B3";
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str22 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str22 == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str22];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            manager.comm.WriteData(str21);
                        }
                    }
                }
            }
            else
            {
                this.comm.WriteData(str21);
            }
            base.Close();
        }

        private void frmPeekPokeMemPeekRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmPeekPokeMemPeekRadioBtn.Checked)
            {
                this.frmPeekPokeMemValueTextBox.ReadOnly = true;
                this.frmPeekPokeMemDataCountTextBox.ReadOnly = true;
                this.frmPeekPokeMem32bitRadioBtn.Checked = true;
            }
            else
            {
                this.frmPeekPokeMemValueTextBox.ReadOnly = false;
                this.frmPeekPokeMemDataCountTextBox.ReadOnly = false;
            }
        }

        private void frmPeekPokeMemPokeRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmPeekPokeMemPokeRadioBtn.Checked)
            {
                this.frmPeekPokeMemValueTextBox.ReadOnly = false;
                this.frmPeekPokeMemDataCountTextBox.ReadOnly = true;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmPeekPokeMem));
            this.groupBox1 = new GroupBox();
            this.frmPeekPokeMemMultiPokeRadioBtn = new RadioButton();
            this.frmPeekPokeMemMultiPeekRadioBtn = new RadioButton();
            this.frmPeekPokeMemPokeRadioBtn = new RadioButton();
            this.frmPeekPokeMemPeekRadioBtn = new RadioButton();
            this.groupBox2 = new GroupBox();
            this.frmPeekPokeMem32bitRadioBtn = new RadioButton();
            this.frmPeekPokeMem16bitRadioBtn = new RadioButton();
            this.frmPeekPokeMem8bitRadioBtn = new RadioButton();
            this.label8 = new Label();
            this.label7 = new Label();
            this.frmPeekPokeMemAddrTextBox = new TextBox();
            this.frmPeekPokeMemValueTextBox = new TextBox();
            this.frmPeekPokeMemValueLabel = new Label();
            this.frmPeekPokeMemAddrLabel = new Label();
            this.frmPeekPokeMemDataCountTextBox = new TextBox();
            this.frmPeekPokeMemDataCountLabel = new Label();
            this.label1 = new Label();
            this.frmPeekPokeMemFilePathTextBox = new TextBox();
            this.frmPeekPokeMemBrowserBtn = new Button();
            this.frmPeekPokeMemOkBtn = new Button();
            this.frmPeekPokeMemCancelBtn = new Button();
            this.label2 = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.frmPeekPokeMemMultiPokeRadioBtn);
            this.groupBox1.Controls.Add(this.frmPeekPokeMemMultiPeekRadioBtn);
            this.groupBox1.Controls.Add(this.frmPeekPokeMemPokeRadioBtn);
            this.groupBox1.Controls.Add(this.frmPeekPokeMemPeekRadioBtn);
            this.groupBox1.Location = new Point(0x17, 0x18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x12d, 0x38);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action";
            this.frmPeekPokeMemMultiPokeRadioBtn.AutoSize = true;
            this.frmPeekPokeMemMultiPokeRadioBtn.Location = new Point(0xd0, 20);
            this.frmPeekPokeMemMultiPokeRadioBtn.Name = "frmPeekPokeMemMultiPokeRadioBtn";
            this.frmPeekPokeMemMultiPokeRadioBtn.Size = new Size(0x4b, 0x11);
            this.frmPeekPokeMemMultiPokeRadioBtn.TabIndex = 0;
            this.frmPeekPokeMemMultiPokeRadioBtn.TabStop = true;
            this.frmPeekPokeMemMultiPokeRadioBtn.Text = "Multi Poke";
            this.frmPeekPokeMemMultiPokeRadioBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemMultiPokeRadioBtn.CheckedChanged += new EventHandler(this.frmPeekPokeMemMultiPokeRadioBtn_CheckedChanged);
            this.frmPeekPokeMemMultiPeekRadioBtn.AutoSize = true;
            this.frmPeekPokeMemMultiPeekRadioBtn.Location = new Point(0x80, 20);
            this.frmPeekPokeMemMultiPeekRadioBtn.Name = "frmPeekPokeMemMultiPeekRadioBtn";
            this.frmPeekPokeMemMultiPeekRadioBtn.Size = new Size(0x4b, 0x11);
            this.frmPeekPokeMemMultiPeekRadioBtn.TabIndex = 0;
            this.frmPeekPokeMemMultiPeekRadioBtn.TabStop = true;
            this.frmPeekPokeMemMultiPeekRadioBtn.Text = "Multi Peek";
            this.frmPeekPokeMemMultiPeekRadioBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemMultiPeekRadioBtn.CheckedChanged += new EventHandler(this.frmPeekPokeMemMultiPeekRadioBtn_CheckedChanged);
            this.frmPeekPokeMemPokeRadioBtn.AutoSize = true;
            this.frmPeekPokeMemPokeRadioBtn.Location = new Point(0x49, 20);
            this.frmPeekPokeMemPokeRadioBtn.Name = "frmPeekPokeMemPokeRadioBtn";
            this.frmPeekPokeMemPokeRadioBtn.Size = new Size(50, 0x11);
            this.frmPeekPokeMemPokeRadioBtn.TabIndex = 0;
            this.frmPeekPokeMemPokeRadioBtn.TabStop = true;
            this.frmPeekPokeMemPokeRadioBtn.Text = "Poke";
            this.frmPeekPokeMemPokeRadioBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemPokeRadioBtn.CheckedChanged += new EventHandler(this.frmPeekPokeMemPokeRadioBtn_CheckedChanged);
            this.frmPeekPokeMemPeekRadioBtn.AutoSize = true;
            this.frmPeekPokeMemPeekRadioBtn.Location = new Point(0x12, 20);
            this.frmPeekPokeMemPeekRadioBtn.Name = "frmPeekPokeMemPeekRadioBtn";
            this.frmPeekPokeMemPeekRadioBtn.Size = new Size(50, 0x11);
            this.frmPeekPokeMemPeekRadioBtn.TabIndex = 0;
            this.frmPeekPokeMemPeekRadioBtn.TabStop = true;
            this.frmPeekPokeMemPeekRadioBtn.Text = "Peek";
            this.frmPeekPokeMemPeekRadioBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemPeekRadioBtn.CheckedChanged += new EventHandler(this.frmPeekPokeMemPeekRadioBtn_CheckedChanged);
            this.groupBox2.Controls.Add(this.frmPeekPokeMem32bitRadioBtn);
            this.groupBox2.Controls.Add(this.frmPeekPokeMem16bitRadioBtn);
            this.groupBox2.Controls.Add(this.frmPeekPokeMem8bitRadioBtn);
            this.groupBox2.Location = new Point(0x17, 0x5c);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x55, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Size";
            this.frmPeekPokeMem32bitRadioBtn.AutoSize = true;
            this.frmPeekPokeMem32bitRadioBtn.Location = new Point(0x10, 0x41);
            this.frmPeekPokeMem32bitRadioBtn.Name = "frmPeekPokeMem32bitRadioBtn";
            this.frmPeekPokeMem32bitRadioBtn.Size = new Size(0x34, 0x11);
            this.frmPeekPokeMem32bitRadioBtn.TabIndex = 0;
            this.frmPeekPokeMem32bitRadioBtn.TabStop = true;
            this.frmPeekPokeMem32bitRadioBtn.Text = "32 Bit";
            this.frmPeekPokeMem32bitRadioBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMem16bitRadioBtn.AutoSize = true;
            this.frmPeekPokeMem16bitRadioBtn.Location = new Point(0x10, 0x2a);
            this.frmPeekPokeMem16bitRadioBtn.Name = "frmPeekPokeMem16bitRadioBtn";
            this.frmPeekPokeMem16bitRadioBtn.Size = new Size(0x34, 0x11);
            this.frmPeekPokeMem16bitRadioBtn.TabIndex = 0;
            this.frmPeekPokeMem16bitRadioBtn.TabStop = true;
            this.frmPeekPokeMem16bitRadioBtn.Text = "16 Bit";
            this.frmPeekPokeMem16bitRadioBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMem8bitRadioBtn.AutoSize = true;
            this.frmPeekPokeMem8bitRadioBtn.Location = new Point(0x10, 0x13);
            this.frmPeekPokeMem8bitRadioBtn.Name = "frmPeekPokeMem8bitRadioBtn";
            this.frmPeekPokeMem8bitRadioBtn.Size = new Size(0x2e, 0x11);
            this.frmPeekPokeMem8bitRadioBtn.TabIndex = 0;
            this.frmPeekPokeMem8bitRadioBtn.TabStop = true;
            this.frmPeekPokeMem8bitRadioBtn.Text = "8 Bit";
            this.frmPeekPokeMem8bitRadioBtn.UseVisualStyleBackColor = true;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(200, 140);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x12, 13);
            this.label8.TabIndex = 0x1a;
            this.label8.Text = "0x";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(200, 0x6c);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x12, 13);
            this.label7.TabIndex = 0x1b;
            this.label7.Text = "0x";
            this.frmPeekPokeMemAddrTextBox.Location = new Point(0xe0, 0x68);
            this.frmPeekPokeMemAddrTextBox.Name = "frmPeekPokeMemAddrTextBox";
            this.frmPeekPokeMemAddrTextBox.Size = new Size(100, 20);
            this.frmPeekPokeMemAddrTextBox.TabIndex = 2;
            this.frmPeekPokeMemValueTextBox.Location = new Point(0xe0, 0x88);
            this.frmPeekPokeMemValueTextBox.Name = "frmPeekPokeMemValueTextBox";
            this.frmPeekPokeMemValueTextBox.Size = new Size(100, 20);
            this.frmPeekPokeMemValueTextBox.TabIndex = 3;
            this.frmPeekPokeMemValueLabel.AutoSize = true;
            this.frmPeekPokeMemValueLabel.Location = new Point(0x89, 140);
            this.frmPeekPokeMemValueLabel.Name = "frmPeekPokeMemValueLabel";
            this.frmPeekPokeMemValueLabel.Size = new Size(0x25, 13);
            this.frmPeekPokeMemValueLabel.TabIndex = 0x15;
            this.frmPeekPokeMemValueLabel.Text = "Value:";
            this.frmPeekPokeMemAddrLabel.AutoSize = true;
            this.frmPeekPokeMemAddrLabel.Location = new Point(0x8a, 0x6c);
            this.frmPeekPokeMemAddrLabel.Name = "frmPeekPokeMemAddrLabel";
            this.frmPeekPokeMemAddrLabel.Size = new Size(0x30, 13);
            this.frmPeekPokeMemAddrLabel.TabIndex = 20;
            this.frmPeekPokeMemAddrLabel.Text = "Address:";
            this.frmPeekPokeMemDataCountTextBox.Location = new Point(0xe0, 0xab);
            this.frmPeekPokeMemDataCountTextBox.Name = "frmPeekPokeMemDataCountTextBox";
            this.frmPeekPokeMemDataCountTextBox.Size = new Size(100, 20);
            this.frmPeekPokeMemDataCountTextBox.TabIndex = 4;
            this.frmPeekPokeMemDataCountLabel.AutoSize = true;
            this.frmPeekPokeMemDataCountLabel.Location = new Point(0x8b, 0xab);
            this.frmPeekPokeMemDataCountLabel.Name = "frmPeekPokeMemDataCountLabel";
            this.frmPeekPokeMemDataCountLabel.Size = new Size(0x40, 13);
            this.frmPeekPokeMemDataCountLabel.TabIndex = 0x15;
            this.frmPeekPokeMemDataCountLabel.Text = "Data Count:";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x17, 0xcc);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x6f, 13);
            this.label1.TabIndex = 0x1c;
            this.label1.Text = "Multi Bytes File Name:";
            this.frmPeekPokeMemFilePathTextBox.Location = new Point(0x17, 0xde);
            this.frmPeekPokeMemFilePathTextBox.Name = "frmPeekPokeMemFilePathTextBox";
            this.frmPeekPokeMemFilePathTextBox.Size = new Size(0x12d, 20);
            this.frmPeekPokeMemFilePathTextBox.TabIndex = 5;
            this.frmPeekPokeMemBrowserBtn.Location = new Point(330, 0xdd);
            this.frmPeekPokeMemBrowserBtn.Name = "frmPeekPokeMemBrowserBtn";
            this.frmPeekPokeMemBrowserBtn.Size = new Size(0x21, 0x17);
            this.frmPeekPokeMemBrowserBtn.TabIndex = 6;
            this.frmPeekPokeMemBrowserBtn.Text = "...";
            this.frmPeekPokeMemBrowserBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemBrowserBtn.Click += new EventHandler(this.frmPeekPokeMemBrowserBtn_Click);
            this.frmPeekPokeMemOkBtn.Location = new Point(0x63, 0x111);
            this.frmPeekPokeMemOkBtn.Name = "frmPeekPokeMemOkBtn";
            this.frmPeekPokeMemOkBtn.Size = new Size(0x4b, 0x17);
            this.frmPeekPokeMemOkBtn.TabIndex = 7;
            this.frmPeekPokeMemOkBtn.Text = "&OK";
            this.frmPeekPokeMemOkBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemOkBtn.Click += new EventHandler(this.frmPeekPokeMemOkBtn_Click);
            this.frmPeekPokeMemCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmPeekPokeMemCancelBtn.Location = new Point(0xcb, 0x111);
            this.frmPeekPokeMemCancelBtn.Name = "frmPeekPokeMemCancelBtn";
            this.frmPeekPokeMemCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmPeekPokeMemCancelBtn.TabIndex = 8;
            this.frmPeekPokeMemCancelBtn.Text = "&Cancel";
            this.frmPeekPokeMemCancelBtn.UseVisualStyleBackColor = true;
            this.frmPeekPokeMemCancelBtn.Click += new EventHandler(this.frmPeekPokeMemCancelBtn_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x19, 250);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xbd, 13);
            this.label2.TabIndex = 0x20;
            this.label2.Text = @"(Multi Peek saved at C:\MultiPeek.bin)";
            base.AcceptButton = this.frmPeekPokeMemOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmPeekPokeMemCancelBtn;
            base.ClientSize = new Size(0x182, 0x139);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.frmPeekPokeMemCancelBtn);
            base.Controls.Add(this.frmPeekPokeMemOkBtn);
            base.Controls.Add(this.frmPeekPokeMemBrowserBtn);
            base.Controls.Add(this.frmPeekPokeMemFilePathTextBox);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.frmPeekPokeMemAddrTextBox);
            base.Controls.Add(this.frmPeekPokeMemDataCountTextBox);
            base.Controls.Add(this.frmPeekPokeMemValueTextBox);
            base.Controls.Add(this.frmPeekPokeMemDataCountLabel);
            base.Controls.Add(this.frmPeekPokeMemValueLabel);
            base.Controls.Add(this.frmPeekPokeMemAddrLabel);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmPeekPokeMem";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Tracker IC Peek/Poke Memory";
            base.Load += new EventHandler(this.frmPeekPokeMem_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            base.OnClosed(e);
        }

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

