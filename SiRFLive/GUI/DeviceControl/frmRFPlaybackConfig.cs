﻿namespace SiRFLive.GUI.DeviceControl
{
    using SiRFLive.TestAutomation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmRFPlaybackConfig : Form
    {
        private Label captureDMASizeLabel;
        private TextBox captureDMASizeVal;
        private Label chan1VolLabel;
        private ComboBox chan1VolVal;
        private Label Chan2VolLabel;
        private ComboBox chan2VolVal;
        private Label chanLabel;
        private ComboBox chanVal;
        private Label clkLabel;
        private ComboBox clkVal;
        private IContainer components;
        private Label deviceIdLabel;
        private TextBox deviceIdVal;
        private static frmRFPlaybackConfig m_SChildform;
        public RFPlaybackInterface pbConfig = new RFPlaybackInterface();
        private Label PlaybackDMASizeLabel;
        private TextBox playbackDMASizeVal;
        private Button RFPlaybackUpdateConfig;
        private Button RFReplayCancelConfig;
        private Label sampleRateLabel;
        private TextBox sampleRateVal;

        public frmRFPlaybackConfig()
        {
            this.InitializeComponent();
        }

        private void chanVal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.chanVal.SelectedIndex == 0)
            {
                this.chan1VolVal.Enabled = true;
                this.chan2VolVal.Enabled = false;
            }
            else if (this.chanVal.SelectedIndex == 1)
            {
                this.chan2VolVal.Enabled = true;
                this.chan1VolVal.Enabled = false;
            }
            else
            {
                this.chan2VolVal.Enabled = true;
                this.chan1VolVal.Enabled = true;
            }
        }

        public void CloseRFReplayConfigWindow()
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
                base.Close();
                m_SChildform = null;
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static frmRFPlaybackConfig GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRFPlaybackConfig();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmRFPlaybackConfig));
            this.sampleRateVal = new TextBox();
            this.sampleRateLabel = new Label();
            this.RFPlaybackUpdateConfig = new Button();
            this.deviceIdLabel = new Label();
            this.deviceIdVal = new TextBox();
            this.captureDMASizeLabel = new Label();
            this.captureDMASizeVal = new TextBox();
            this.PlaybackDMASizeLabel = new Label();
            this.playbackDMASizeVal = new TextBox();
            this.chanVal = new ComboBox();
            this.chanLabel = new Label();
            this.clkLabel = new Label();
            this.clkVal = new ComboBox();
            this.chan1VolLabel = new Label();
            this.chan1VolVal = new ComboBox();
            this.Chan2VolLabel = new Label();
            this.chan2VolVal = new ComboBox();
            this.RFReplayCancelConfig = new Button();
            base.SuspendLayout();
            this.sampleRateVal.Location = new Point(0x9b, 0x40);
            this.sampleRateVal.Name = "sampleRateVal";
            this.sampleRateVal.Size = new Size(0x39, 20);
            this.sampleRateVal.TabIndex = 5;
            this.sampleRateVal.Text = "20";
            this.sampleRateLabel.AutoSize = true;
            this.sampleRateLabel.Location = new Point(0x27, 0x44);
            this.sampleRateLabel.Name = "sampleRateLabel";
            this.sampleRateLabel.Size = new Size(0x60, 13);
            this.sampleRateLabel.TabIndex = 4;
            this.sampleRateLabel.Text = "SampleRate (MHz)";
            this.RFPlaybackUpdateConfig.Location = new Point(0x9e, 0xbb);
            this.RFPlaybackUpdateConfig.Name = "RFPlaybackUpdateConfig";
            this.RFPlaybackUpdateConfig.Size = new Size(0x4b, 0x17);
            this.RFPlaybackUpdateConfig.TabIndex = 0x10;
            this.RFPlaybackUpdateConfig.Text = "&Update";
            this.RFPlaybackUpdateConfig.UseVisualStyleBackColor = true;
            this.RFPlaybackUpdateConfig.Click += new EventHandler(this.RFPlaybackUpdateConfig_Click);
            this.deviceIdLabel.AutoSize = true;
            this.deviceIdLabel.Location = new Point(0x27, 30);
            this.deviceIdLabel.Name = "deviceIdLabel";
            this.deviceIdLabel.Size = new Size(0x37, 13);
            this.deviceIdLabel.TabIndex = 0;
            this.deviceIdLabel.Text = "Device ID";
            this.deviceIdVal.Location = new Point(0x9b, 0x1a);
            this.deviceIdVal.Name = "deviceIdVal";
            this.deviceIdVal.Size = new Size(0x39, 20);
            this.deviceIdVal.TabIndex = 1;
            this.deviceIdVal.Text = "1";
            this.captureDMASizeLabel.AutoSize = true;
            this.captureDMASizeLabel.Location = new Point(0x27, 0x6a);
            this.captureDMASizeLabel.Name = "captureDMASizeLabel";
            this.captureDMASizeLabel.Size = new Size(0x5e, 13);
            this.captureDMASizeLabel.TabIndex = 8;
            this.captureDMASizeLabel.Text = "Capture DMA Size";
            this.captureDMASizeVal.Location = new Point(0x9b, 0x66);
            this.captureDMASizeVal.Name = "captureDMASizeVal";
            this.captureDMASizeVal.Size = new Size(0x39, 20);
            this.captureDMASizeVal.TabIndex = 9;
            this.captureDMASizeVal.Text = "10";
            this.PlaybackDMASizeLabel.AutoSize = true;
            this.PlaybackDMASizeLabel.Location = new Point(0x27, 0x90);
            this.PlaybackDMASizeLabel.Name = "PlaybackDMASizeLabel";
            this.PlaybackDMASizeLabel.Size = new Size(0x65, 13);
            this.PlaybackDMASizeLabel.TabIndex = 12;
            this.PlaybackDMASizeLabel.Text = "Playback DMA Size";
            this.playbackDMASizeVal.Location = new Point(0x9b, 140);
            this.playbackDMASizeVal.Name = "playbackDMASizeVal";
            this.playbackDMASizeVal.Size = new Size(0x39, 20);
            this.playbackDMASizeVal.TabIndex = 13;
            this.playbackDMASizeVal.Text = "2";
            this.chanVal.AllowDrop = true;
            this.chanVal.FormattingEnabled = true;
            this.chanVal.Items.AddRange(new object[] { "Channel 1", "Channel 2", "Both" });
            this.chanVal.Location = new Point(0x173, 0x19);
            this.chanVal.Name = "chanVal";
            this.chanVal.Size = new Size(0x4c, 0x15);
            this.chanVal.TabIndex = 3;
            this.chanVal.Text = "Channel 1";
            this.chanVal.SelectedIndexChanged += new EventHandler(this.chanVal_SelectedIndexChanged);
            this.chanLabel.AutoSize = true;
            this.chanLabel.Location = new Point(0x138, 0x1d);
            this.chanLabel.Name = "chanLabel";
            this.chanLabel.Size = new Size(0x2e, 13);
            this.chanLabel.TabIndex = 2;
            this.chanLabel.Text = "Channel";
            this.clkLabel.AutoSize = true;
            this.clkLabel.Location = new Point(0x149, 0x43);
            this.clkLabel.Name = "clkLabel";
            this.clkLabel.Size = new Size(0x22, 13);
            this.clkLabel.TabIndex = 6;
            this.clkLabel.Text = "Clock";
            this.clkVal.AllowDrop = true;
            this.clkVal.FormattingEnabled = true;
            this.clkVal.Items.AddRange(new object[] { "External", "Internal" });
            this.clkVal.Location = new Point(0x173, 0x3f);
            this.clkVal.Name = "clkVal";
            this.clkVal.Size = new Size(0x4c, 0x15);
            this.clkVal.TabIndex = 7;
            this.clkVal.Text = "External";
            this.chan1VolLabel.AutoSize = true;
            this.chan1VolLabel.Location = new Point(0xea, 0x69);
            this.chan1VolLabel.Name = "chan1VolLabel";
            this.chan1VolLabel.Size = new Size(0x81, 13);
            this.chan1VolLabel.TabIndex = 10;
            this.chan1VolLabel.Text = "Channel 1 Voltage Range";
            this.chan1VolVal.AllowDrop = true;
            this.chan1VolVal.FormattingEnabled = true;
            this.chan1VolVal.Items.AddRange(new object[] { "5 Vp-p", "2 Vp-p", "1 Vp-p", "0.5 Vp-p", "0.2 Vp-p", "0.1 Vp-p", "0.05 Vp-p" });
            this.chan1VolVal.Location = new Point(0x173, 0x65);
            this.chan1VolVal.Name = "chan1VolVal";
            this.chan1VolVal.Size = new Size(0x4c, 0x15);
            this.chan1VolVal.TabIndex = 11;
            this.chan1VolVal.Text = "0.2 Vp-p";
            this.Chan2VolLabel.AutoSize = true;
            this.Chan2VolLabel.Location = new Point(0xea, 0x8f);
            this.Chan2VolLabel.Name = "Chan2VolLabel";
            this.Chan2VolLabel.Size = new Size(0x81, 13);
            this.Chan2VolLabel.TabIndex = 14;
            this.Chan2VolLabel.Text = "Channel 2 Voltage Range";
            this.chan2VolVal.AllowDrop = true;
            this.chan2VolVal.Enabled = false;
            this.chan2VolVal.FormattingEnabled = true;
            this.chan2VolVal.Items.AddRange(new object[] { "5 Vp-p", "2 Vp-p", "1 Vp-p", "0.5 Vp-p", "0.2 Vp-p", "0.1 Vp-p", "0.05 Vp-p" });
            this.chan2VolVal.Location = new Point(0x173, 0x8b);
            this.chan2VolVal.Name = "chan2VolVal";
            this.chan2VolVal.Size = new Size(0x4c, 0x15);
            this.chan2VolVal.TabIndex = 15;
            this.chan2VolVal.Text = "5 Vp-p";
            this.RFReplayCancelConfig.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.RFReplayCancelConfig.Location = new Point(0xfe, 0xba);
            this.RFReplayCancelConfig.Name = "RFReplayCancelConfig";
            this.RFReplayCancelConfig.Size = new Size(0x4b, 0x17);
            this.RFReplayCancelConfig.TabIndex = 0x11;
            this.RFReplayCancelConfig.Text = "&Cancel";
            this.RFReplayCancelConfig.UseVisualStyleBackColor = true;
            this.RFReplayCancelConfig.Click += new EventHandler(this.RFReplayCancelConfig_Click);
            base.AcceptButton = this.RFPlaybackUpdateConfig;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.RFReplayCancelConfig;
            base.ClientSize = new Size(0x1e6, 0xeb);
            base.Controls.Add(this.RFReplayCancelConfig);
            base.Controls.Add(this.Chan2VolLabel);
            base.Controls.Add(this.chan2VolVal);
            base.Controls.Add(this.chan1VolLabel);
            base.Controls.Add(this.chan1VolVal);
            base.Controls.Add(this.clkLabel);
            base.Controls.Add(this.clkVal);
            base.Controls.Add(this.chanLabel);
            base.Controls.Add(this.chanVal);
            base.Controls.Add(this.PlaybackDMASizeLabel);
            base.Controls.Add(this.playbackDMASizeVal);
            base.Controls.Add(this.captureDMASizeLabel);
            base.Controls.Add(this.captureDMASizeVal);
            base.Controls.Add(this.deviceIdLabel);
            base.Controls.Add(this.deviceIdVal);
            base.Controls.Add(this.RFPlaybackUpdateConfig);
            base.Controls.Add(this.sampleRateLabel);
            base.Controls.Add(this.sampleRateVal);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmRFPlaybackConfig";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "RF Player System Configuration";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void RFPlaybackUpdateConfig_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int[] configParams = new int[] { 20, 10, 2, 0, 200, 200, 0 };
            int[] numArray2 = new int[] { 0x1388, 0x7d0, 0x3e8, 500, 200, 100, 50 };
            if ((((this.deviceIdVal.Text.Length == 0) | (this.sampleRateVal.Text.Length == 0)) | (this.captureDMASizeVal.Text.Length == 0)) | (this.playbackDMASizeVal.Text.Length == 0))
            {
                MessageBox.Show("Invalid Input Parameters", "ERROR!");
                this.Cursor = Cursors.Default;
            }
            else
            {
                int num;
                try
                {
                    num = Convert.ToInt32(this.deviceIdVal.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "ERROR!");
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (num > 0)
                {
                    try
                    {
                        configParams[0] = Convert.ToInt32(this.sampleRateVal.Text);
                        configParams[1] = Convert.ToInt32(this.captureDMASizeVal.Text);
                        configParams[2] = Convert.ToInt32(this.playbackDMASizeVal.Text);
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show(exception2.Message, "ERROR!");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        string[] strArray = new string[] { "Sample Rate", "Capture DMA", "Playback DMA" };
                        if ((configParams[i] < 1) || (configParams[i] > 20))
                        {
                            MessageBox.Show("Invalid " + strArray[i] + "\n Value range: 1 - 20", "ERROR!");
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    configParams[3] = this.chanVal.SelectedIndex + 1;
                    configParams[4] = numArray2[this.chan1VolVal.SelectedIndex];
                    configParams[5] = numArray2[this.chan2VolVal.SelectedIndex];
                    configParams[6] = this.clkVal.SelectedIndex + 1;
                    this.pbConfig.Init(num);
                    int num3 = this.pbConfig.Config(configParams);
                    if ((num3 != 1) && this.pbConfig.IsManual)
                    {
                        MessageBox.Show("Configure Error: " + Convert.ToInt32(num3), "ERROR!");
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Invalid Device ID", "ERROR!");
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void RFReplayCancelConfig_Click(object sender, EventArgs e)
        {
            base.Close();
            m_SChildform = null;
        }
    }
}

