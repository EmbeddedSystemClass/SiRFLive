﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmDRSetPollGenericSensParams : Form
    {
        private Button button_Done;
        private Button button_Poll;
        private Button button_RestoreDefault;
        private Button button_set;
        private CommunicationManager comm;
        private IContainer components;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox textBox_MillivoltsPer0;
        private TextBox textBox_MillivoltsPer1;
        private TextBox textBox_MillivoltsPer2;
        private TextBox textBox_MillivoltsPer3;
        private TextBox textBox_MillivoltsPer4;
        private TextBox textBox_RefVolt0;
        private TextBox textBox_RefVolt1;
        private TextBox textBox_RefVolt2;
        private TextBox textBox_RefVolt3;
        private TextBox textBox_RefVolt4;
        private TextBox textBox_ZeroRateVolts0;
        private TextBox textBox_ZeroRateVolts1;
        private TextBox textBox_ZeroRateVolts2;
        private TextBox textBox_ZeroRateVolts3;
        private TextBox textBox_ZeroRateVolts4;

        public frmDRSetPollGenericSensParams(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
        }

        private void button_Done_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_Poll_Click(object sender, EventArgs e)
        {
            string messageName = "DR Poll Generic Sensor Param";
            StringBuilder builder = new StringBuilder();
            builder.Append("172,13");
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "SSB");
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str3 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str3 == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str3];
                        if (manager != null)
                        {
                            manager.comm.WriteData(msg);
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.comm.WriteData(msg);
            }
        }

        private void button_RestoreDefault_Click(object sender, EventArgs e)
        {
            string messageName = "DR Set Generic Sensor Param";
            string csvMessage = "172,12,1,0,0,0,1,0,0,0,2,0,0,0,2,0,0,0,2,0,0,0";
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, "SSB");
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str4 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str4 == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str4];
                        if (manager != null)
                        {
                            manager.comm.WriteData(msg);
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.comm.WriteData(msg);
            }
        }

        private void button_set_Click(object sender, EventArgs e)
        {
            string messageName = "DR Set Generic Sensor Param";
            ushort num = 0;
            ushort num2 = 0;
            ushort num3 = 0;
            ushort num4 = 0;
            ushort num5 = 0;
            ushort num6 = 0;
            ushort num7 = 0;
            ushort num8 = 0;
            ushort num9 = 0;
            ushort num10 = 0;
            ushort num11 = 0;
            ushort num12 = 0;
            ushort num13 = 0;
            ushort num14 = 0;
            ushort num15 = 0;
            try
            {
                num = Convert.ToUInt16(this.textBox_ZeroRateVolts0.Text);
                num2 = Convert.ToUInt16(this.textBox_MillivoltsPer0.Text);
                num3 = Convert.ToUInt16(this.textBox_RefVolt0.Text);
                num4 = Convert.ToUInt16(this.textBox_ZeroRateVolts1.Text);
                num5 = Convert.ToUInt16(this.textBox_MillivoltsPer1.Text);
                num6 = Convert.ToUInt16(this.textBox_RefVolt1.Text);
                num7 = Convert.ToUInt16(this.textBox_ZeroRateVolts2.Text);
                num8 = Convert.ToUInt16(this.textBox_MillivoltsPer2.Text);
                num9 = Convert.ToUInt16(this.textBox_RefVolt2.Text);
                num10 = Convert.ToUInt16(this.textBox_ZeroRateVolts3.Text);
                num11 = Convert.ToUInt16(this.textBox_MillivoltsPer3.Text);
                num12 = Convert.ToUInt16(this.textBox_RefVolt3.Text);
                num13 = Convert.ToUInt16(this.textBox_ZeroRateVolts4.Text);
                num14 = Convert.ToUInt16(this.textBox_MillivoltsPer4.Text);
                num15 = Convert.ToUInt16(this.textBox_RefVolt4.Text);
            }
            catch
            {
                MessageBox.Show("Invalid Input!", "Error", MessageBoxButtons.OK);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("172,12,{0},{1},{2},{3},", new object[] { 1, num, num2, num3 }));
            builder.Append(string.Format("{0},{1},{2},{3},", new object[] { 1, num4, num5, num6 }));
            builder.Append(string.Format("{0},{1},{2},{3},", new object[] { 2, num7, num8, num9 }));
            builder.Append(string.Format("{0},{1},{2},{3},", new object[] { 2, num10, num11, num12 }));
            builder.Append(string.Format("{0},{1},{2},{3}", new object[] { 2, num13, num14, num15 }));
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "SSB");
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str3 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str3 == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str3];
                        if (manager != null)
                        {
                            manager.comm.WriteData(msg);
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.comm.WriteData(msg);
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

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmDRSetPollGenericSensParams));
            this.textBox_ZeroRateVolts0 = new TextBox();
            this.textBox_ZeroRateVolts1 = new TextBox();
            this.textBox_ZeroRateVolts2 = new TextBox();
            this.textBox_ZeroRateVolts3 = new TextBox();
            this.textBox_ZeroRateVolts4 = new TextBox();
            this.textBox_MillivoltsPer0 = new TextBox();
            this.textBox_MillivoltsPer4 = new TextBox();
            this.textBox_RefVolt0 = new TextBox();
            this.textBox_RefVolt4 = new TextBox();
            this.textBox_MillivoltsPer1 = new TextBox();
            this.textBox_MillivoltsPer2 = new TextBox();
            this.textBox_MillivoltsPer3 = new TextBox();
            this.textBox_RefVolt1 = new TextBox();
            this.textBox_RefVolt2 = new TextBox();
            this.textBox_RefVolt3 = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label11 = new Label();
            this.button_set = new Button();
            this.button_Poll = new Button();
            this.button_RestoreDefault = new Button();
            this.button_Done = new Button();
            this.label10 = new Label();
            this.label12 = new Label();
            this.label13 = new Label();
            base.SuspendLayout();
            this.textBox_ZeroRateVolts0.Location = new Point(0x69, 0x3b);
            this.textBox_ZeroRateVolts0.Name = "textBox_ZeroRateVolts0";
            this.textBox_ZeroRateVolts0.Size = new Size(100, 20);
            this.textBox_ZeroRateVolts0.TabIndex = 0;
            this.textBox_ZeroRateVolts0.Text = "0";
            this.textBox_ZeroRateVolts1.Location = new Point(0x69, 0x6a);
            this.textBox_ZeroRateVolts1.Name = "textBox_ZeroRateVolts1";
            this.textBox_ZeroRateVolts1.Size = new Size(100, 20);
            this.textBox_ZeroRateVolts1.TabIndex = 0;
            this.textBox_ZeroRateVolts1.Text = "0";
            this.textBox_ZeroRateVolts2.Location = new Point(0x69, 0x98);
            this.textBox_ZeroRateVolts2.Name = "textBox_ZeroRateVolts2";
            this.textBox_ZeroRateVolts2.Size = new Size(100, 20);
            this.textBox_ZeroRateVolts2.TabIndex = 0;
            this.textBox_ZeroRateVolts2.Text = "0";
            this.textBox_ZeroRateVolts3.Location = new Point(0x69, 0xbd);
            this.textBox_ZeroRateVolts3.Name = "textBox_ZeroRateVolts3";
            this.textBox_ZeroRateVolts3.Size = new Size(100, 20);
            this.textBox_ZeroRateVolts3.TabIndex = 0;
            this.textBox_ZeroRateVolts3.Text = "0";
            this.textBox_ZeroRateVolts4.Location = new Point(0x69, 0xe8);
            this.textBox_ZeroRateVolts4.Name = "textBox_ZeroRateVolts4";
            this.textBox_ZeroRateVolts4.Size = new Size(100, 20);
            this.textBox_ZeroRateVolts4.TabIndex = 0;
            this.textBox_ZeroRateVolts4.Text = "0";
            this.textBox_MillivoltsPer0.Location = new Point(240, 0x3b);
            this.textBox_MillivoltsPer0.Name = "textBox_MillivoltsPer0";
            this.textBox_MillivoltsPer0.Size = new Size(100, 20);
            this.textBox_MillivoltsPer0.TabIndex = 0;
            this.textBox_MillivoltsPer0.Text = "0";
            this.textBox_MillivoltsPer4.Location = new Point(240, 0xe8);
            this.textBox_MillivoltsPer4.Name = "textBox_MillivoltsPer4";
            this.textBox_MillivoltsPer4.Size = new Size(100, 20);
            this.textBox_MillivoltsPer4.TabIndex = 0;
            this.textBox_MillivoltsPer4.Text = "0";
            this.textBox_RefVolt0.Location = new Point(410, 0x3b);
            this.textBox_RefVolt0.Name = "textBox_RefVolt0";
            this.textBox_RefVolt0.Size = new Size(100, 20);
            this.textBox_RefVolt0.TabIndex = 0;
            this.textBox_RefVolt0.Text = "0";
            this.textBox_RefVolt4.Location = new Point(410, 0xe8);
            this.textBox_RefVolt4.Name = "textBox_RefVolt4";
            this.textBox_RefVolt4.Size = new Size(100, 20);
            this.textBox_RefVolt4.TabIndex = 0;
            this.textBox_RefVolt4.Text = "0";
            this.textBox_MillivoltsPer1.Location = new Point(240, 0x6a);
            this.textBox_MillivoltsPer1.Name = "textBox_MillivoltsPer1";
            this.textBox_MillivoltsPer1.Size = new Size(100, 20);
            this.textBox_MillivoltsPer1.TabIndex = 0;
            this.textBox_MillivoltsPer1.Text = "0";
            this.textBox_MillivoltsPer2.Location = new Point(240, 0x98);
            this.textBox_MillivoltsPer2.Name = "textBox_MillivoltsPer2";
            this.textBox_MillivoltsPer2.Size = new Size(100, 20);
            this.textBox_MillivoltsPer2.TabIndex = 0;
            this.textBox_MillivoltsPer2.Text = "0";
            this.textBox_MillivoltsPer3.Location = new Point(240, 0xbd);
            this.textBox_MillivoltsPer3.Name = "textBox_MillivoltsPer3";
            this.textBox_MillivoltsPer3.Size = new Size(100, 20);
            this.textBox_MillivoltsPer3.TabIndex = 0;
            this.textBox_MillivoltsPer3.Text = "0";
            this.textBox_RefVolt1.Location = new Point(410, 0x6a);
            this.textBox_RefVolt1.Name = "textBox_RefVolt1";
            this.textBox_RefVolt1.Size = new Size(100, 20);
            this.textBox_RefVolt1.TabIndex = 0;
            this.textBox_RefVolt1.Text = "0";
            this.textBox_RefVolt2.Location = new Point(410, 0x98);
            this.textBox_RefVolt2.Name = "textBox_RefVolt2";
            this.textBox_RefVolt2.Size = new Size(100, 20);
            this.textBox_RefVolt2.TabIndex = 0;
            this.textBox_RefVolt2.Text = "0";
            this.textBox_RefVolt3.Location = new Point(410, 0xbd);
            this.textBox_RefVolt3.Name = "textBox_RefVolt3";
            this.textBox_RefVolt3.Size = new Size(100, 20);
            this.textBox_RefVolt3.TabIndex = 0;
            this.textBox_RefVolt3.Text = "0";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x26, 0x3b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Roll Gyro";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x26, 0x6d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "X-Axis";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x26, 0x9b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Y-Axis";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x26, 0xc4);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x24, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Z-Axis";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x26, 0xef);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x38, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Pitch Gyro";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x6d, 0x1d);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x51, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Zero Rate Volts";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0xee, 0x1d);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x41, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Millivolts Per";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x19b, 0x1d);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x3f, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Ref Voltage";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x15a, 0x3e);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x23, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "deg/s";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x15a, 0x9b);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x22, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "mv/G";
            this.button_set.Location = new Point(0x22f, 0x31);
            this.button_set.Name = "button_set";
            this.button_set.Size = new Size(0x6b, 0x17);
            this.button_set.TabIndex = 5;
            this.button_set.Text = "Set";
            this.button_set.UseVisualStyleBackColor = true;
            this.button_set.Click += new EventHandler(this.button_set_Click);
            this.button_Poll.Location = new Point(0x22f, 80);
            this.button_Poll.Name = "button_Poll";
            this.button_Poll.Size = new Size(0x6b, 0x17);
            this.button_Poll.TabIndex = 5;
            this.button_Poll.Text = "Poll";
            this.button_Poll.UseVisualStyleBackColor = true;
            this.button_Poll.Click += new EventHandler(this.button_Poll_Click);
            this.button_RestoreDefault.Location = new Point(0x22f, 0x76);
            this.button_RestoreDefault.Name = "button_RestoreDefault";
            this.button_RestoreDefault.Size = new Size(0x6b, 0x17);
            this.button_RestoreDefault.TabIndex = 5;
            this.button_RestoreDefault.Text = "Restore Default";
            this.button_RestoreDefault.UseVisualStyleBackColor = true;
            this.button_RestoreDefault.Click += new EventHandler(this.button_RestoreDefault_Click);
            this.button_Done.Location = new Point(0x250, 230);
            this.button_Done.Name = "button_Done";
            this.button_Done.Size = new Size(0x4a, 0x17);
            this.button_Done.TabIndex = 5;
            this.button_Done.Text = "Exit";
            this.button_Done.UseVisualStyleBackColor = true;
            this.button_Done.Click += new EventHandler(this.button_Done_Click);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x15a, 0x6d);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x23, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "deg/s";
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x15a, 0xc0);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x22, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "mv/G";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x15a, 0xeb);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x22, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "mv/G";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2ab, 0x115);
            base.Controls.Add(this.button_Done);
            base.Controls.Add(this.button_RestoreDefault);
            base.Controls.Add(this.button_Poll);
            base.Controls.Add(this.button_set);
            base.Controls.Add(this.label13);
            base.Controls.Add(this.label12);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.textBox_RefVolt4);
            base.Controls.Add(this.textBox_MillivoltsPer4);
            base.Controls.Add(this.textBox_ZeroRateVolts4);
            base.Controls.Add(this.textBox_RefVolt3);
            base.Controls.Add(this.textBox_MillivoltsPer3);
            base.Controls.Add(this.textBox_ZeroRateVolts3);
            base.Controls.Add(this.textBox_RefVolt2);
            base.Controls.Add(this.textBox_MillivoltsPer2);
            base.Controls.Add(this.textBox_ZeroRateVolts2);
            base.Controls.Add(this.textBox_RefVolt1);
            base.Controls.Add(this.textBox_RefVolt0);
            base.Controls.Add(this.textBox_MillivoltsPer1);
            base.Controls.Add(this.textBox_MillivoltsPer0);
            base.Controls.Add(this.textBox_ZeroRateVolts1);
            base.Controls.Add(this.textBox_ZeroRateVolts0);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmDRSetPollGenericSensParams";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DR Generic Sensor Parameters";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

