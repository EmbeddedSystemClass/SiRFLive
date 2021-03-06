﻿namespace SiRFLive.GUI.DeviceControl
{
    using SiRFLive.DeviceControl;
    using SiRFLive.TestAutomation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmRFSynthesizer : Form
    {
        private int _AGC1;
        private int _AGC2;
        private int _inputDevice;
        private bool _manual_AGC;
        private bool _one_Pass;
        private int _outputDevice;
        private int _PortAddress = 0x278;
        private Button button_Exit;
        private Button button_ProgramAGCAndSyn;
        private Button button_ProgramSyn;
        private Button button_SetAGCAuto;
        private Button button_SetAGCManually;
        private Button button_SetAuxFreq;
        private Button button_setClk;
        private Button button_setMainFreq;
        private Button button_UpdateDsp;
        private ColumnHeader columnHeader1;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox groupBox8;
        private GroupBox groupBox9;
        private Label label_AGCValSet;
        private Label label_SynLocked;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private ListView listView1;
        private static frmRFSynthesizer m_SChildform;
        private NumericUpDown numericUpDown_AGCManually;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private RadioButton radioButton_1_Channel;
        private RadioButton radioButton_2_Channels;
        public RFPlaybackInterface syncCtrl = new RFPlaybackInterface();
        private TextBox textBox_AuxFreq;
        private TextBox textBox_Ch1MaxDataVal;
        private TextBox textBox_Ch2MaxDataValue;
        private TextBox textBox_Clk;
        private TextBox textBox_ExcaqCardAddress;
        private TextBox textBox_mainFreq;
        private TextBox textBox_ParallelPortAddress;

        public frmRFSynthesizer()
        {
            this.InitializeComponent();
        }

        private void AGC_number_to_string(int AGC1, int AGC2)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            for (int i = 4; i >= 0; i--)
            {
                if ((this._AGC1 & ((int) Math.Pow(2.0, (double) i))) != 0)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if ((this._AGC2 & ((int) Math.Pow(2.0, (double) i))) != 0)
                {
                    str2 = str2 + "1";
                }
                else
                {
                    str2 = str2 + "0";
                }
            }
            str = str + "0";
            str2 = str2 + "0";
            this.Transmit_AGC(str, str2);
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_ProgramAGCAndSyn_Click(object sender, EventArgs e)
        {
            this.command2_ProgramSyn();
            this.command3_SetAGC();
        }

        private void button_ProgramSyn_Click(object sender, EventArgs e)
        {
            this.command2_ProgramSyn();
        }

        private void button_SetAGCAuto_Click(object sender, EventArgs e)
        {
            this.command3_SetAGC();
        }

        private void button_SetAGCManually_Click(object sender, EventArgs e)
        {
            this._manual_AGC = true;
            this.command3_SetAGC();
            this._manual_AGC = false;
        }

        private void button_SetAuxFreq_Click(object sender, EventArgs e)
        {
            if (this.textBox_AuxFreq.Text != string.Empty)
            {
                string data = string.Empty;
                double num = Convert.ToDouble(this.textBox_AuxFreq.Text);
                double num2 = 5.0;
                double a = num / num2;
                int num4 = (int) (Math.Round(a) - 32.0);
                string str2 = "0011000" + this.convertIntToBinaryStr(9, num4);
                int num5 = 0x400;
                int num6 = (int) Math.Round((double) (num5 * ((a - num4) - 32.0)));
                string str3 = "010000" + this.convertIntToBinaryStr(10, num6);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = "0111001000000000";
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = str2;
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = str3;
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
            }
        }

        private void button_setClk_Click(object sender, EventArgs e)
        {
            if (this.textBox_Clk.Text != string.Empty)
            {
                string data = string.Empty;
                double num = Convert.ToDouble(this.textBox_Clk.Text);
                double num2 = 5.0;
                double a = num / num2;
                int num4 = (int) (Math.Round(a) - 32.0);
                string str2 = "0011000" + this.convertIntToBinaryStr(9, num4);
                int num5 = 0x400;
                int num6 = (int) Math.Round((double) (num5 * ((a - num4) - 32.0)));
                string str3 = "010000" + this.convertIntToBinaryStr(10, num6);
                PortAccessAPI.Output(this._PortAddress, 4);
                data = "0111001000000110";
                this.Transmit_clk(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 4);
                data = str2;
                this.Transmit_clk(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 4);
                data = str3;
                this.Transmit_clk(data);
                PortAccessAPI.Output(this._PortAddress, 5);
            }
        }

        private void button_setMainFreq_Click(object sender, EventArgs e)
        {
            if (this.textBox_mainFreq.Text != string.Empty)
            {
                string data = string.Empty;
                double num = Convert.ToDouble(this.textBox_mainFreq.Text);
                double num2 = 10.0;
                double a = num / num2;
                int num4 = (int) (Math.Round(a) - 32.0);
                string str2 = "0000000" + this.convertIntToBinaryStr(9, num4);
                int num5 = 0x40000;
                int num6 = (int) Math.Round((double) (num5 * ((a - num4) - 32.0)));
                string str3 = this.convertIntToBinaryStr(0x12, num6);
                string str4 = "000100" + str3.Substring(0, 10);
                string str5 = "00100000" + str3.Substring(10, 8);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = "0111001000000000";
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = str2;
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = str5;
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
                PortAccessAPI.Output(this._PortAddress, 1);
                data = str4;
                this.Transmit(data);
                PortAccessAPI.Output(this._PortAddress, 5);
            }
        }

        private void button_UpdateDsp_Click(object sender, EventArgs e)
        {
            this._one_Pass = true;
            this.command3_SetAGC();
            this._one_Pass = false;
        }

        private void command2_ProgramSyn()
        {
            string data = string.Empty;
            this._PortAddress = Convert.ToInt32(this.textBox_ParallelPortAddress.Text, 0x10);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0111001000000000";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0000000001110111";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0010000000001001";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0001001000101011";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0011000000010101";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0100001100110011";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0101000000100000";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "0110011111011111";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "1000000000000000";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 1);
            data = "1001000000000000";
            this.Transmit(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            this.listView1.Items.Add("Programmed main synthesizers");
            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
            if (this.radioButton_1_Channel.Checked)
            {
                PortAccessAPI.Output(this._PortAddress + 2, 1);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress + 2, 0);
            }
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0111000000000110";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0111001000000110";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            if (this.radioButton_1_Channel.Checked)
            {
                this.listView1.Items.Add("clock output = 40 MHz");
                this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
            }
            else
            {
                this.listView1.Items.Add("clock output = 80 MHz");
                this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
            }
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0111001000000110";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0011000000100000";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0100000000000000";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0101000000100000";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "0110011111000000";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "1000000000000000";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            PortAccessAPI.Output(this._PortAddress, 4);
            data = "1001000000000000";
            this.Transmit_clk(data);
            PortAccessAPI.Output(this._PortAddress, 5);
            this.listView1.Items.Add("Programmed clock synthesizer");
            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
            int num = PortAccessAPI.Input(this._PortAddress + 1);
            string str2 = "";
            for (int i = 7; i >= 0; i--)
            {
                if ((num & ((int) Math.Pow(2.0, (double) i))) != 0)
                {
                    str2 = str2 + "1";
                }
                else
                {
                    str2 = str2 + "0";
                }
            }
            if (str2.Substring(0, 1) == "1")
            {
                this.label_SynLocked.BackColor = Color.LightGreen;
            }
            else
            {
                this.label_SynLocked.BackColor = Color.Red;
            }
        }

        private void command3_SetAGC()
        {
            int[] numArray = new int[2];
            double[] numArray2 = new double[2];
            short[] buffer = new short[0x2000];
            double actual = 0.0;
            int num9 = 0;
            this._inputDevice = Convert.ToInt32(this.textBox_ExcaqCardAddress.Text);
            numArray[0] = 0;
            numArray[1] = 1;
            numArray2[0] = 0.1;
            numArray2[1] = 0.1;
            double num10 = 2.0 * numArray2[0];
            this.listView1.Items.Add("voltage range = " + num10.ToString() + " Vpp");
            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
            double rate = 20000000.0;
            double num3 = 0.0;
            Graphics graphics = this.pictureBox1.CreateGraphics();
            graphics.Clear(Color.White);
            Graphics graphics2 = this.pictureBox2.CreateGraphics();
            graphics2.Clear(Color.White);
            if (!this._one_Pass)
            {
                this._AGC1 = (int) this.numericUpDown_AGCManually.Value;
                this._AGC2 = (int) this.numericUpDown_AGCManually.Value;
            }
            this._PortAddress = Convert.ToInt32(this.textBox_ParallelPortAddress.Text, 0x10);
            this.label_AGCValSet.BackColor = Color.Red;
            int num = fnCPBK_XDA_Board_Init(this._inputDevice, 0);
            this.ErrCheck((long) num, "fnCPBK_XDA_Board_Init");
            num = fnCPBK_XDA_Ain_SetSequence(this._inputDevice, 2, ref numArray[0], ref numArray2[0]);
            this.ErrCheck((long) num, "fnCPBK_XDA_Ain_SetSequence");
            num = fnCPBK_XDA_Ain_SetClock(this._inputDevice, rate, ref actual);
            this.ErrCheck((long) num, "fnCPBK_XDA_Ain_SetClock");
            num = fnCPBK_XDA_Ain_SetScanRate(this._inputDevice, num3, ref actual);
            this.ErrCheck((long) num, "fnCPBK_XDA_Ain_SetScanRate");
            num = fnCPBK_XDA_Ain_SetBuffer(this._inputDevice, 0x2000, buffer);
            this.ErrCheck((long) num, "fnCPBK_XDA_Ain_SetBuffer");
            if (!this._manual_AGC)
            {
                num9 = 0x1f;
            }
            if (this._manual_AGC)
            {
                num9 = 1;
            }
            if (this._one_Pass)
            {
                num9 = 1;
            }
            if (!this._one_Pass)
            {
                this.AGC_number_to_string(this._AGC1, this._AGC2);
            }
            Pen pen = new Pen(Color.Blue);
            for (int i = 1; i <= num9; i++)
            {
                double num5 = 0.0;
                double num6 = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                num = fnCPBK_XDA_Ain_Start(this._inputDevice, 0, 0x2000);
                this.ErrCheck((long) num, "fnCPBK_XDA_Ain_Start");
                graphics.Clear(Color.White);
                graphics2.Clear(Color.White);
                for (int j = 0; j < 0x1fff; j += 2)
                {
                    if (buffer[j] > num5)
                    {
                        num5 = buffer[j];
                    }
                    if (buffer[j] < num7)
                    {
                        num7 = buffer[j];
                    }
                }
                for (int k = 1; k < 0x2000; k += 2)
                {
                    if (buffer[k] > num6)
                    {
                        num6 = buffer[k];
                    }
                    if (buffer[k] < num8)
                    {
                        num8 = buffer[k];
                    }
                }
                int width = this.pictureBox1.Width;
                int height = this.pictureBox1.Height;
                int num16 = this.pictureBox2.Width;
                int num17 = this.pictureBox2.Height;
                Point[] points = new Point[0x1000];
                Point[] pointArray2 = new Point[0x1000];
                Point[] pointArray3 = new Point[0x1000];
                for (int m = 0; m < 0x1fff; m += 2)
                {
                    int x = (m * width) / 0x1000;
                    int index = m / 2;
                    int num21 = (int) (((buffer[m] - num7) * (height - 0x10)) / (num5 - num7));
                    int y = (8 + (height - 0x10)) - num21;
                    pointArray3[index] = new Point(x, y);
                    points[index] = new Point(x, height - 8);
                    pointArray2[index] = new Point(x, 8);
                }
                graphics.DrawLines(pen, pointArray3);
                graphics.DrawLines(pen, points);
                graphics.DrawLines(pen, pointArray2);
                if (!this.radioButton_1_Channel.Checked)
                {
                    for (int n = 1; n < 0x2000; n += 2)
                    {
                        int num24 = (n * num16) / 0x1000;
                        int num25 = (n - 1) / 2;
                        int num26 = (int) (((buffer[n] - num8) * (num17 - 0x10)) / (num6 - num8));
                        int num27 = (8 + (num17 - 0x10)) - num26;
                        pointArray3[num25] = new Point(num24, num27);
                        points[num25] = new Point(num24, num17 - 8);
                        pointArray2[num25] = new Point(num24, 8);
                    }
                    graphics2.DrawLines(pen, pointArray3);
                    graphics2.DrawLines(pen, points);
                    graphics2.DrawLines(pen, pointArray2);
                }
                if (!this._one_Pass)
                {
                    if (!this._manual_AGC)
                    {
                        if (num5 > 512.0)
                        {
                            this._AGC1++;
                        }
                        else
                        {
                            this._AGC1--;
                        }
                        if (this._AGC1 < 0)
                        {
                            this._AGC1 = 0;
                        }
                        if (this._AGC1 > 0x1f)
                        {
                            this._AGC1 = 0x1f;
                        }
                        if (num6 > 512.0)
                        {
                            this._AGC2++;
                        }
                        else
                        {
                            this._AGC2--;
                        }
                        if (this._AGC2 < 0)
                        {
                            this._AGC2 = 0;
                        }
                        if (this._AGC2 > 0x1f)
                        {
                            this._AGC2 = 0x1f;
                        }
                    }
                    this.textBox_Ch1MaxDataVal.Text = " CH1 max sample value = " + num5.ToString();
                    this.textBox_Ch2MaxDataValue.Text = " CH2 max sample value = " + num6.ToString();
                    this.AGC_number_to_string(this._AGC1, this._AGC2);
                }
            }
            if (this.radioButton_1_Channel.Checked)
            {
                this._AGC2 = this._AGC1;
            }
            this.button_Exit.Enabled = true;
            if (!this._one_Pass)
            {
                this.listView1.Items.Add("AGC1 value = " + this._AGC1.ToString());
                this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                this.listView1.Items.Add("AGC2 value = " + this._AGC2.ToString());
                this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
            }
            this.label_AGCValSet.BackColor = Color.LightGreen;
            this.button_Exit.Enabled = true;
            num = fnCPBK_XDA_Ain_SetBuffer(this._inputDevice, 0, buffer);
            this.ErrCheck((long) num, "fnCPBK_XDA_Ain_SetBuffer");
            num = fnCPBK_XDA_Board_Cleanup(this._inputDevice, 0);
            this.ErrCheck((long) num, "fnCPBK_XDA_Board_Cleanup");
        }

        private uint convertBinaryStrToInt(string str)
        {
            int length = str.Length;
            uint num2 = 0;
            uint num3 = 0;
            for (int i = length - 1; i >= 0; i--)
            {
                num3 = Convert.ToUInt32(str.Substring(i, 1));
                num2 += num3 * ((uint) Math.Pow(2.0, (double) ((length - 1) - i)));
            }
            return num2;
        }

        private string convertIntToBinaryStr(int numBits, int num)
        {
            string str = string.Empty;
            int num2 = Math.Abs(num);
            for (int i = numBits - 1; i >= 0; i--)
            {
                if ((num2 & ((int) Math.Pow(2.0, (double) i))) != 0)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
            }
            if (num < 0)
            {
                string str2 = string.Empty;
                int length = str.Length;
                for (int j = 0; j < length; j++)
                {
                    if (str.Substring(j, 1) == "0")
                    {
                        str2 = str2 + "1";
                    }
                    else
                    {
                        str2 = str2 + "0";
                    }
                }
                uint num6 = this.convertBinaryStrToInt(str2) + 1;
                str = string.Empty;
                for (int k = numBits - 1; k >= 0; k--)
                {
                    if ((num6 & ((int) Math.Pow(2.0, (double) k))) != 0L)
                    {
                        str = str + "1";
                    }
                    else
                    {
                        str = str + "0";
                    }
                }
            }
            return str;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ErrCheck(long ErrCode, string WhatFunction)
        {
            if (ErrCode > 0L)
            {
                MessageBox.Show("Error " + ErrCode.ToString() + " " + WhatFunction + " - Warning");
            }
            else if (ErrCode < 0L)
            {
                MessageBox.Show("Error " + ErrCode.ToString() + " " + WhatFunction + " - Critical Error");
                if (this._outputDevice == this._inputDevice)
                {
                    ErrCode = fnCPBK_XDA_Board_Cleanup(this._outputDevice, 0);
                }
                else
                {
                    ErrCode = fnCPBK_XDA_Board_Cleanup(this._outputDevice, 0);
                    ErrCode = fnCPBK_XDA_Board_Cleanup(this._inputDevice, 0);
                }
            }
        }

        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Ain_SetBuffer(int deviceID, int count, [In, Out] short[] buffer);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Ain_SetClock(int deviceID, double rate, ref double actual);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Ain_SetScanRate(int deviceID, double rate, ref double actual);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Ain_SetSequence(int deviceID, int count, ref int channel, ref double range);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Ain_Start(int deviceID, int modeflags, int count);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Board_Cleanup(int deviceID, int rsvd);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnCPBK_XDA_Board_Init(int deviceID, int rsvd);
        public static frmRFSynthesizer GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRFSynthesizer();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmRFSynthesizer));
            this.button_SetAGCManually = new Button();
            this.button_ProgramSyn = new Button();
            this.button_SetAGCAuto = new Button();
            this.textBox_ExcaqCardAddress = new TextBox();
            this.label1 = new Label();
            this.listView1 = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.label4 = new Label();
            this.textBox_ParallelPortAddress = new TextBox();
            this.button_UpdateDsp = new Button();
            this.label_SynLocked = new Label();
            this.label_AGCValSet = new Label();
            this.textBox_Ch1MaxDataVal = new TextBox();
            this.textBox_Ch2MaxDataValue = new TextBox();
            this.label5 = new Label();
            this.label6 = new Label();
            this.groupBox1 = new GroupBox();
            this.pictureBox1 = new PictureBox();
            this.groupBox2 = new GroupBox();
            this.pictureBox2 = new PictureBox();
            this.groupBox3 = new GroupBox();
            this.radioButton_2_Channels = new RadioButton();
            this.radioButton_1_Channel = new RadioButton();
            this.groupBox4 = new GroupBox();
            this.groupBox5 = new GroupBox();
            this.groupBox6 = new GroupBox();
            this.groupBox7 = new GroupBox();
            this.numericUpDown_AGCManually = new NumericUpDown();
            this.groupBox8 = new GroupBox();
            this.button_Exit = new Button();
            this.button_ProgramAGCAndSyn = new Button();
            this.textBox_mainFreq = new TextBox();
            this.textBox_AuxFreq = new TextBox();
            this.button_setMainFreq = new Button();
            this.button_SetAuxFreq = new Button();
            this.label2 = new Label();
            this.label3 = new Label();
            this.textBox_Clk = new TextBox();
            this.label7 = new Label();
            this.button_setClk = new Button();
            this.groupBox9 = new GroupBox();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize) this.pictureBox2).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.numericUpDown_AGCManually.BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            base.SuspendLayout();
            this.button_SetAGCManually.Location = new Point(0x20, 0x48);
            this.button_SetAGCManually.Name = "button_SetAGCManually";
            this.button_SetAGCManually.Size = new Size(0x8e, 0x17);
            this.button_SetAGCManually.TabIndex = 1;
            this.button_SetAGCManually.Text = "Set AGC Manually";
            this.button_SetAGCManually.UseVisualStyleBackColor = true;
            this.button_SetAGCManually.Click += new EventHandler(this.button_SetAGCManually_Click);
            this.button_ProgramSyn.Location = new Point(0x1a, 0x16);
            this.button_ProgramSyn.Name = "button_ProgramSyn";
            this.button_ProgramSyn.Size = new Size(0x8e, 0x17);
            this.button_ProgramSyn.TabIndex = 1;
            this.button_ProgramSyn.Text = "Program Synthesizers";
            this.button_ProgramSyn.UseVisualStyleBackColor = true;
            this.button_ProgramSyn.Click += new EventHandler(this.button_ProgramSyn_Click);
            this.button_SetAGCAuto.Location = new Point(0x1a, 0x41);
            this.button_SetAGCAuto.Name = "button_SetAGCAuto";
            this.button_SetAGCAuto.Size = new Size(0x8e, 0x17);
            this.button_SetAGCAuto.TabIndex = 1;
            this.button_SetAGCAuto.Text = "Set AGC Automatically";
            this.button_SetAGCAuto.UseVisualStyleBackColor = true;
            this.button_SetAGCAuto.Click += new EventHandler(this.button_SetAGCAuto_Click);
            this.textBox_ExcaqCardAddress.Location = new Point(0x84, 0x23);
            this.textBox_ExcaqCardAddress.Name = "textBox_ExcaqCardAddress";
            this.textBox_ExcaqCardAddress.Size = new Size(100, 20);
            this.textBox_ExcaqCardAddress.TabIndex = 3;
            this.textBox_ExcaqCardAddress.Text = "1";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x17, 0x23);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Excaq Card Address";
            this.listView1.Alignment = ListViewAlignment.Left;
            this.listView1.AutoArrange = false;
            this.listView1.BackColor = SystemColors.Window;
            this.listView1.BorderStyle = BorderStyle.None;
            this.listView1.Columns.AddRange(new ColumnHeader[] { this.columnHeader1 });
            this.listView1.ForeColor = SystemColors.WindowText;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new Point(5, 0x13);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new Size(0xe1, 210);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 0x100;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xf8, 0x23);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x68, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Parallel Port Address";
            this.textBox_ParallelPortAddress.Location = new Point(370, 0x23);
            this.textBox_ParallelPortAddress.Name = "textBox_ParallelPortAddress";
            this.textBox_ParallelPortAddress.Size = new Size(100, 20);
            this.textBox_ParallelPortAddress.TabIndex = 12;
            this.textBox_ParallelPortAddress.Text = "378";
            this.button_UpdateDsp.Location = new Point(0x1a, 0x6c);
            this.button_UpdateDsp.Name = "button_UpdateDsp";
            this.button_UpdateDsp.Size = new Size(0x8e, 0x17);
            this.button_UpdateDsp.TabIndex = 1;
            this.button_UpdateDsp.Text = "Update Display";
            this.button_UpdateDsp.UseVisualStyleBackColor = true;
            this.button_UpdateDsp.Click += new EventHandler(this.button_UpdateDsp_Click);
            this.label_SynLocked.BorderStyle = BorderStyle.Fixed3D;
            this.label_SynLocked.Font = new Font("Microsoft Sans Serif", 36f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label_SynLocked.Location = new Point(15, 0x17);
            this.label_SynLocked.Name = "label_SynLocked";
            this.label_SynLocked.Size = new Size(0xcd, 0x3e);
            this.label_SynLocked.TabIndex = 13;
            this.label_AGCValSet.BorderStyle = BorderStyle.Fixed3D;
            this.label_AGCValSet.Font = new Font("Microsoft Sans Serif", 36f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label_AGCValSet.Location = new Point(15, 0x17);
            this.label_AGCValSet.Name = "label_AGCValSet";
            this.label_AGCValSet.Size = new Size(0xcf, 0x3e);
            this.label_AGCValSet.TabIndex = 13;
            this.textBox_Ch1MaxDataVal.Location = new Point(0xa9, 0x90);
            this.textBox_Ch1MaxDataVal.Name = "textBox_Ch1MaxDataVal";
            this.textBox_Ch1MaxDataVal.Size = new Size(0xa9, 20);
            this.textBox_Ch1MaxDataVal.TabIndex = 3;
            this.textBox_Ch1MaxDataVal.Text = "1";
            this.textBox_Ch2MaxDataValue.Location = new Point(0xa9, 0x8f);
            this.textBox_Ch2MaxDataValue.Name = "textBox_Ch2MaxDataValue";
            this.textBox_Ch2MaxDataValue.Size = new Size(0xa9, 20);
            this.textBox_Ch2MaxDataValue.TabIndex = 3;
            this.textBox_Ch2MaxDataValue.Text = "1";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(12, 0x93);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x83, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "CH1 Maximum Data Value";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(15, 0x92);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x83, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "CH2 Maximum Data Value";
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.textBox_Ch1MaxDataVal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new Point(0x13, 0xb7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x179, 0xb8);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel 1 - used in 1 and 2 channel mode to set the AGC level";
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox1.Location = new Point(0x12, 0x19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x155, 0x6a);
            this.pictureBox1.TabIndex = 0x18;
            this.pictureBox1.TabStop = false;
            this.groupBox2.Controls.Add(this.textBox_Ch2MaxDataValue);
            this.groupBox2.Controls.Add(this.pictureBox2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new Point(0x13, 0x18e);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x179, 0xb8);
            this.groupBox2.TabIndex = 0x10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel2 - AGC slaved to Channel 1 - not used in 1 channel mode ";
            this.pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox2.Location = new Point(0x12, 0x1a);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(0x155, 0x69);
            this.pictureBox2.TabIndex = 0x19;
            this.pictureBox2.TabStop = false;
            this.groupBox3.Controls.Add(this.radioButton_2_Channels);
            this.groupBox3.Controls.Add(this.radioButton_1_Channel);
            this.groupBox3.Location = new Point(0x13, 0x48);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x73, 0x62);
            this.groupBox3.TabIndex = 0x11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "2x or 4x Output";
            this.radioButton_2_Channels.AutoSize = true;
            this.radioButton_2_Channels.Location = new Point(15, 0x38);
            this.radioButton_2_Channels.Name = "radioButton_2_Channels";
            this.radioButton_2_Channels.Size = new Size(0x4e, 0x11);
            this.radioButton_2_Channels.TabIndex = 0;
            this.radioButton_2_Channels.Text = "2 Channels";
            this.radioButton_2_Channels.UseVisualStyleBackColor = true;
            this.radioButton_1_Channel.AutoSize = true;
            this.radioButton_1_Channel.Checked = true;
            this.radioButton_1_Channel.Location = new Point(15, 0x1c);
            this.radioButton_1_Channel.Name = "radioButton_1_Channel";
            this.radioButton_1_Channel.Size = new Size(0x49, 0x11);
            this.radioButton_1_Channel.TabIndex = 0;
            this.radioButton_1_Channel.TabStop = true;
            this.radioButton_1_Channel.Text = "1 Channel";
            this.radioButton_1_Channel.UseVisualStyleBackColor = true;
            this.groupBox4.Controls.Add(this.label_SynLocked);
            this.groupBox4.Location = new Point(0xa4, 0x48);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0xed, 0x62);
            this.groupBox4.TabIndex = 0x12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sythesizers Locked";
            this.groupBox5.Controls.Add(this.label_AGCValSet);
            this.groupBox5.Location = new Point(0x1a7, 0x48);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0xed, 0x62);
            this.groupBox5.TabIndex = 0x13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AGC Value Set";
            this.groupBox6.Controls.Add(this.button_ProgramSyn);
            this.groupBox6.Controls.Add(this.button_SetAGCAuto);
            this.groupBox6.Controls.Add(this.button_UpdateDsp);
            this.groupBox6.Location = new Point(0x11, 0xf1);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(200, 0x9e);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Subroutines";
            this.groupBox7.Controls.Add(this.numericUpDown_AGCManually);
            this.groupBox7.Controls.Add(this.button_SetAGCManually);
            this.groupBox7.Location = new Point(0x11, 0x19e);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(200, 0x6a);
            this.groupBox7.TabIndex = 0x15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Set AGC Manually";
            this.numericUpDown_AGCManually.Location = new Point(0x2b, 0x21);
            int[] bits = new int[4];
            bits[0] = 0x1f;
            this.numericUpDown_AGCManually.Maximum = new decimal(bits);
            this.numericUpDown_AGCManually.Name = "numericUpDown_AGCManually";
            this.numericUpDown_AGCManually.Size = new Size(120, 20);
            this.numericUpDown_AGCManually.TabIndex = 0x16;
            this.groupBox8.Controls.Add(this.groupBox6);
            this.groupBox8.Controls.Add(this.groupBox7);
            this.groupBox8.Controls.Add(this.listView1);
            this.groupBox8.Location = new Point(0x1a7, 0xb7);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new Size(0xed, 0x21e);
            this.groupBox8.TabIndex = 0x16;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Technical Console";
            this.button_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Exit.Location = new Point(0x12d, 0x2e0);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new Size(0x54, 0x17);
            this.button_Exit.TabIndex = 1;
            this.button_Exit.Text = "E&xit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new EventHandler(this.button_Exit_Click);
            this.button_ProgramAGCAndSyn.Location = new Point(3, 0x2d4);
            this.button_ProgramAGCAndSyn.Name = "button_ProgramAGCAndSyn";
            this.button_ProgramAGCAndSyn.Size = new Size(0xa2, 0x23);
            this.button_ProgramAGCAndSyn.TabIndex = 0;
            this.button_ProgramAGCAndSyn.Text = "Program AGC and Synthesizers";
            this.button_ProgramAGCAndSyn.UseVisualStyleBackColor = true;
            this.button_ProgramAGCAndSyn.Click += new EventHandler(this.button_ProgramAGCAndSyn_Click);
            this.textBox_mainFreq.Location = new Point(50, 0x10);
            this.textBox_mainFreq.Name = "textBox_mainFreq";
            this.textBox_mainFreq.Size = new Size(100, 20);
            this.textBox_mainFreq.TabIndex = 0x17;
            this.textBox_AuxFreq.Location = new Point(50, 0x2b);
            this.textBox_AuxFreq.Name = "textBox_AuxFreq";
            this.textBox_AuxFreq.Size = new Size(100, 20);
            this.textBox_AuxFreq.TabIndex = 0x17;
            this.button_setMainFreq.Location = new Point(0x8f, 13);
            this.button_setMainFreq.Name = "button_setMainFreq";
            this.button_setMainFreq.Size = new Size(0x3f, 0x17);
            this.button_setMainFreq.TabIndex = 0x18;
            this.button_setMainFreq.Text = "Set Main";
            this.button_setMainFreq.UseVisualStyleBackColor = true;
            this.button_setMainFreq.Click += new EventHandler(this.button_setMainFreq_Click);
            this.button_SetAuxFreq.Location = new Point(0x8e, 0x2a);
            this.button_SetAuxFreq.Name = "button_SetAuxFreq";
            this.button_SetAuxFreq.Size = new Size(0x3f, 0x17);
            this.button_SetAuxFreq.TabIndex = 0x18;
            this.button_SetAuxFreq.Text = "Set Aux";
            this.button_SetAuxFreq.UseVisualStyleBackColor = true;
            this.button_SetAuxFreq.Click += new EventHandler(this.button_SetAuxFreq_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(30, 13);
            this.label2.TabIndex = 0x19;
            this.label2.Text = "Main";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(13, 0x2f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x19, 13);
            this.label3.TabIndex = 0x19;
            this.label3.Text = "Aux";
            this.textBox_Clk.Location = new Point(50, 0x4c);
            this.textBox_Clk.Name = "textBox_Clk";
            this.textBox_Clk.Size = new Size(100, 20);
            this.textBox_Clk.TabIndex = 0x1a;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x10, 80);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x16, 13);
            this.label7.TabIndex = 0x1b;
            this.label7.Text = "Clk";
            this.button_setClk.Location = new Point(0x8e, 0x48);
            this.button_setClk.Name = "button_setClk";
            this.button_setClk.Size = new Size(0x40, 0x17);
            this.button_setClk.TabIndex = 0x1c;
            this.button_setClk.Text = "Set Clk";
            this.button_setClk.UseVisualStyleBackColor = true;
            this.button_setClk.Click += new EventHandler(this.button_setClk_Click);
            this.groupBox9.Controls.Add(this.button_SetAuxFreq);
            this.groupBox9.Controls.Add(this.label7);
            this.groupBox9.Controls.Add(this.button_setClk);
            this.groupBox9.Controls.Add(this.textBox_Clk);
            this.groupBox9.Controls.Add(this.label3);
            this.groupBox9.Controls.Add(this.button_setMainFreq);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.textBox_mainFreq);
            this.groupBox9.Controls.Add(this.textBox_AuxFreq);
            this.groupBox9.Location = new Point(0x67, 0x255);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new Size(0xd4, 0x6a);
            this.groupBox9.TabIndex = 0x1d;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Set Frequency";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Exit;
            base.ClientSize = new Size(0x2af, 0x303);
            base.Controls.Add(this.groupBox9);
            base.Controls.Add(this.button_ProgramAGCAndSyn);
            base.Controls.Add(this.button_Exit);
            base.Controls.Add(this.groupBox8);
            base.Controls.Add(this.groupBox5);
            base.Controls.Add(this.groupBox4);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.textBox_ParallelPortAddress);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.textBox_ExcaqCardAddress);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmRFSynthesizer";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Playback Setup Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize) this.pictureBox1).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((ISupportInitialize) this.pictureBox2).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.numericUpDown_AGCManually.EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void Send(int IntBufData, int PortAddress)
        {
            if (IntBufData == 1)
            {
                PortAccessAPI.Output(this._PortAddress, 9);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress, 1);
            }
            if (IntBufData == 1)
            {
                PortAccessAPI.Output(this._PortAddress, 11);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress, 3);
            }
            if (IntBufData == 1)
            {
                PortAccessAPI.Output(this._PortAddress, 9);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress, 1);
            }
        }

        private void Send_clk(int IntBufData, int PortAddress)
        {
            if (IntBufData == 1)
            {
                PortAccessAPI.Output(this._PortAddress, 12);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress, 4);
            }
            if (IntBufData == 1)
            {
                PortAccessAPI.Output(this._PortAddress, 14);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress, 6);
            }
            if (IntBufData == 1)
            {
                PortAccessAPI.Output(this._PortAddress, 12);
            }
            else
            {
                PortAccessAPI.Output(this._PortAddress, 4);
            }
        }

        private void Transmit(string data)
        {
            int intBufData = 0;
            PortAccessAPI.Output(this._PortAddress, 1);
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                switch (data.Substring(i, 1))
                {
                    case "0":
                        intBufData = 0;
                        break;

                    case "1":
                        intBufData = 1;
                        break;
                }
                this.Send(intBufData, this._PortAddress);
            }
            PortAccessAPI.Output(this._PortAddress, 5);
        }

        private void Transmit_AGC(string data1, string data2)
        {
            int num = 0;
            PortAccessAPI.Output(this._PortAddress, 5);
            for (int i = 0; i < 6; i++)
            {
                switch (data1.Substring(i, 1))
                {
                    case "0":
                        num = 0;
                        break;

                    case "1":
                        num = 1;
                        break;
                }
                if (num == 1)
                {
                    PortAccessAPI.Output(this._PortAddress, 0x25);
                }
                else
                {
                    PortAccessAPI.Output(this._PortAddress, 5);
                }
                if (num == 1)
                {
                    PortAccessAPI.Output(this._PortAddress, 0x35);
                }
                else
                {
                    PortAccessAPI.Output(this._PortAddress, 0x15);
                }
                if (num == 1)
                {
                    PortAccessAPI.Output(this._PortAddress, 0x25);
                }
                else
                {
                    PortAccessAPI.Output(this._PortAddress, 5);
                }
            }
            PortAccessAPI.Output(this._PortAddress, 0x85);
            PortAccessAPI.Output(this._PortAddress, 5);
            for (int j = 0; j < 6; j++)
            {
                switch (data2.Substring(j, 1))
                {
                    case "0":
                        num = 0;
                        break;

                    case "1":
                        num = 1;
                        break;
                }
                if (num == 1)
                {
                    PortAccessAPI.Output(this._PortAddress, 0x25);
                }
                else
                {
                    PortAccessAPI.Output(this._PortAddress, 5);
                }
                if (num == 1)
                {
                    PortAccessAPI.Output(this._PortAddress, 0x35);
                }
                else
                {
                    PortAccessAPI.Output(this._PortAddress, 0x15);
                }
                if (num == 1)
                {
                    PortAccessAPI.Output(this._PortAddress, 0x25);
                }
                else
                {
                    PortAccessAPI.Output(this._PortAddress, 5);
                }
            }
            PortAccessAPI.Output(this._PortAddress, 0x45);
            PortAccessAPI.Output(this._PortAddress, 5);
        }

        private void Transmit_clk(string data)
        {
            int intBufData = 0;
            PortAccessAPI.Output(this._PortAddress, 4);
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                switch (data.Substring(i, 1))
                {
                    case "0":
                        intBufData = 0;
                        break;

                    case "1":
                        intBufData = 1;
                        break;
                }
                this.Send_clk(intBufData, this._PortAddress);
            }
            PortAccessAPI.Output(this._PortAddress, 5);
        }
    }
}

