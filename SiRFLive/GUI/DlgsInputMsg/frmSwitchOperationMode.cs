﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmSwitchOperationMode : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton frmSwitchOperationMode1RadioBtn;
        private RadioButton frmSwitchOperationMode2RadioBtn;
        private RadioButton frmSwitchOperationMode3RadioBtn;
        private RadioButton frmSwitchOperationMode4RadioBtn;
        private RadioButton frmSwitchOperationMode5Stage1RadioBtn;
        private RadioButton frmSwitchOperationMode5Stage2RadioBtn;
        private RadioButton frmSwitchOperationMode6RadioBtn;
        private RadioButton frmSwitchOperationMode7RadioBtn;
        private RadioButton frmSwitchOperationMode8RadioBtn;
        private RadioButton frmSwitchOperationModeNormalRadioBtn;
        private TextBox frmSwitchOPerationModeOutputText;
        private TextBox frmSwitchOperationModePeriodTxtBox;
        private TextBox frmSwitchOperationSVNumTxtBox;
        private Button frmTransmitSerialCancelBtn;
        private GroupBox frmTransmitSerialMessageOperationModeGroupbox;
        private Button frmTransmitSerialMessageSendBtn;
        private Label label_Period;
        private Label label_Seconds;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        public frmSwitchOperationMode(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
        }

        private string ConvertInputDataToSend(int mode, int period, int svNum, int tm5Ready)
        {
            string messageName = "Switch Operating Mode";
            StringBuilder builder = new StringBuilder();
            string csvMessage = string.Empty;
            if ((this.comm._rxType == CommunicationManager.ReceiverType.SLC) && (this.comm.MessageProtocol != "OSP"))
            {
                builder.Append("238,");
            }
            builder.Append("150,");
            switch (mode)
            {
                case 0:
                    builder.Append("0,");
                    break;

                case 1:
                    builder.Append("7761,");
                    break;

                case 2:
                    builder.Append("7762,");
                    break;

                case 3:
                    builder.Append("7763,");
                    break;

                case 4:
                    builder.Append("7764,");
                    break;

                case 5:
                    builder.Append("7765,");
                    break;

                case 6:
                    builder.Append("7766,");
                    break;

                case 7:
                    builder.Append("7767,");
                    break;

                case 8:
                    builder.Append("7768,");
                    break;

                default:
                    builder.Append("0,");
                    break;
            }
            builder.Append(svNum.ToString());
            builder.Append(",");
            builder.Append(period.ToString());
            builder.Append(",");
            builder.Append(tm5Ready.ToString());
            csvMessage = builder.ToString();
            return this.comm.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, "SSB");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSwitchOperationMode_Load(object sender, EventArgs e)
        {
            this.frmSwitchOperationMode1RadioBtn.Enabled = false;
            this.frmSwitchOperationMode2RadioBtn.Enabled = false;
        }

        private void frmSwitchOperationMode1RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode2RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode3RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode4RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode5Stage1RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode5Stage2RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode6RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = true;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = true;
        }

        private void frmSwitchOperationMode7RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = false;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = false;
        }

        private void frmSwitchOperationMode8RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = false;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = false;
        }

        private void frmSwitchOperationModeNormalRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.frmSwitchOperationSVNumTxtBox.Enabled = false;
            this.frmSwitchOperationModePeriodTxtBox.Enabled = false;
        }

        private void frmTransmitSerialCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmTransmitSerialMessageSendBtn_Click(object sender, EventArgs e)
        {
            int period = 0;
            int svNum = 0;
            int mode = 0;
            int num4 = 0;
            if (this.frmSwitchOperationModeNormalRadioBtn.Checked)
            {
                mode = 0;
            }
            else if (this.frmSwitchOperationMode1RadioBtn.Checked)
            {
                mode = 1;
            }
            else if (this.frmSwitchOperationMode2RadioBtn.Checked)
            {
                mode = 2;
            }
            else if (this.frmSwitchOperationMode3RadioBtn.Checked)
            {
                mode = 3;
            }
            else if (this.frmSwitchOperationMode4RadioBtn.Checked)
            {
                mode = 4;
            }
            else if (this.frmSwitchOperationMode5Stage1RadioBtn.Checked)
            {
                mode = 5;
            }
            else if (this.frmSwitchOperationMode5Stage2RadioBtn.Checked)
            {
                mode = 5;
                num4 = 1;
            }
            else if (this.frmSwitchOperationMode6RadioBtn.Checked)
            {
                mode = 6;
            }
            else if (this.frmSwitchOperationMode7RadioBtn.Checked)
            {
                mode = 7;
                svNum = 1;
            }
            else if (this.frmSwitchOperationMode8RadioBtn.Checked)
            {
                mode = 8;
                svNum = 1;
            }
            switch (mode)
            {
                case 7:
                case 8:
                    break;

                case 0:
                    period = 0;
                    svNum = 0;
                    break;

                default:
                    try
                    {
                        period = Convert.ToInt16(this.frmSwitchOperationModePeriodTxtBox.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Error in 'Period' field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        svNum = Convert.ToUInt16(this.frmSwitchOperationSVNumTxtBox.Text);
                        if ((svNum < 1) || (svNum > 0x20))
                        {
                            MessageBox.Show("'SV#' field out of range (1-32)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error in 'SV#' field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    break;
            }
            this.comm.RxCtrl.ResetCtrl.ResetType = "COLD";
            this.frmSwitchOPerationModeOutputText.Text = this.ConvertInputDataToSend(mode, period, svNum, num4);
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                        if (manager != null)
                        {
                            manager.comm.WriteData(this.frmSwitchOPerationModeOutputText.Text);
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.comm.WriteData(this.frmSwitchOPerationModeOutputText.Text);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSwitchOperationMode));
            this.frmTransmitSerialMessageOperationModeGroupbox = new GroupBox();
            this.frmSwitchOperationMode8RadioBtn = new RadioButton();
            this.frmSwitchOperationMode7RadioBtn = new RadioButton();
            this.frmSwitchOperationMode6RadioBtn = new RadioButton();
            this.frmSwitchOperationMode5Stage2RadioBtn = new RadioButton();
            this.frmSwitchOperationMode5Stage1RadioBtn = new RadioButton();
            this.frmSwitchOperationModeNormalRadioBtn = new RadioButton();
            this.frmSwitchOperationMode4RadioBtn = new RadioButton();
            this.frmSwitchOperationMode1RadioBtn = new RadioButton();
            this.frmSwitchOperationMode3RadioBtn = new RadioButton();
            this.frmSwitchOperationMode2RadioBtn = new RadioButton();
            this.frmTransmitSerialCancelBtn = new Button();
            this.frmTransmitSerialMessageSendBtn = new Button();
            this.frmSwitchOPerationModeOutputText = new TextBox();
            this.label_Period = new Label();
            this.frmSwitchOperationModePeriodTxtBox = new TextBox();
            this.label_Seconds = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.frmSwitchOperationSVNumTxtBox = new TextBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.frmTransmitSerialMessageOperationModeGroupbox.SuspendLayout();
            base.SuspendLayout();
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode8RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode7RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode6RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode5Stage2RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode5Stage1RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationModeNormalRadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode4RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode1RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode3RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Controls.Add(this.frmSwitchOperationMode2RadioBtn);
            this.frmTransmitSerialMessageOperationModeGroupbox.Location = new Point(0x1d, 0x16);
            this.frmTransmitSerialMessageOperationModeGroupbox.Name = "frmTransmitSerialMessageOperationModeGroupbox";
            this.frmTransmitSerialMessageOperationModeGroupbox.Size = new Size(0xa3, 0xfc);
            this.frmTransmitSerialMessageOperationModeGroupbox.TabIndex = 0;
            this.frmTransmitSerialMessageOperationModeGroupbox.TabStop = false;
            this.frmTransmitSerialMessageOperationModeGroupbox.Text = "Switch Operation Mode";
            this.frmSwitchOperationMode8RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode8RadioBtn.Location = new Point(0x18, 220);
            this.frmSwitchOperationMode8RadioBtn.Name = "frmSwitchOperationMode8RadioBtn";
            this.frmSwitchOperationMode8RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode8RadioBtn.TabIndex = 0x10;
            this.frmSwitchOperationMode8RadioBtn.TabStop = true;
            this.frmSwitchOperationMode8RadioBtn.Text = "Test Mode &8";
            this.frmSwitchOperationMode8RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode8RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode8RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode7RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode7RadioBtn.Location = new Point(0x18, 200);
            this.frmSwitchOperationMode7RadioBtn.Name = "frmSwitchOperationMode7RadioBtn";
            this.frmSwitchOperationMode7RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode7RadioBtn.TabIndex = 7;
            this.frmSwitchOperationMode7RadioBtn.TabStop = true;
            this.frmSwitchOperationMode7RadioBtn.Text = "Test Mode &7";
            this.frmSwitchOperationMode7RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode7RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode7RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode6RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode6RadioBtn.Location = new Point(0x18, 0xb2);
            this.frmSwitchOperationMode6RadioBtn.Name = "frmSwitchOperationMode6RadioBtn";
            this.frmSwitchOperationMode6RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode6RadioBtn.TabIndex = 6;
            this.frmSwitchOperationMode6RadioBtn.TabStop = true;
            this.frmSwitchOperationMode6RadioBtn.Text = "Test Mode &6";
            this.frmSwitchOperationMode6RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode6RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode6RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode5Stage2RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode5Stage2RadioBtn.Location = new Point(0x17, 0x9c);
            this.frmSwitchOperationMode5Stage2RadioBtn.Name = "frmSwitchOperationMode5Stage2RadioBtn";
            this.frmSwitchOperationMode5Stage2RadioBtn.Size = new Size(0x7d, 0x11);
            this.frmSwitchOperationMode5Stage2RadioBtn.TabIndex = 5;
            this.frmSwitchOperationMode5Stage2RadioBtn.TabStop = true;
            this.frmSwitchOperationMode5Stage2RadioBtn.Text = "Test Mode &5 Stage 2";
            this.frmSwitchOperationMode5Stage2RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode5Stage2RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode5Stage2RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode5Stage1RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode5Stage1RadioBtn.Location = new Point(0x18, 0x86);
            this.frmSwitchOperationMode5Stage1RadioBtn.Name = "frmSwitchOperationMode5Stage1RadioBtn";
            this.frmSwitchOperationMode5Stage1RadioBtn.Size = new Size(0x7d, 0x11);
            this.frmSwitchOperationMode5Stage1RadioBtn.TabIndex = 5;
            this.frmSwitchOperationMode5Stage1RadioBtn.TabStop = true;
            this.frmSwitchOperationMode5Stage1RadioBtn.Text = "Test Mode &5 Stage 1";
            this.frmSwitchOperationMode5Stage1RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode5Stage1RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode5Stage1RadioBtn_CheckedChanged);
            this.frmSwitchOperationModeNormalRadioBtn.AutoSize = true;
            this.frmSwitchOperationModeNormalRadioBtn.Location = new Point(0x18, 0x18);
            this.frmSwitchOperationModeNormalRadioBtn.Name = "frmSwitchOperationModeNormalRadioBtn";
            this.frmSwitchOperationModeNormalRadioBtn.Size = new Size(0x3a, 0x11);
            this.frmSwitchOperationModeNormalRadioBtn.TabIndex = 0;
            this.frmSwitchOperationModeNormalRadioBtn.TabStop = true;
            this.frmSwitchOperationModeNormalRadioBtn.Text = "&Normal";
            this.frmSwitchOperationModeNormalRadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationModeNormalRadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationModeNormalRadioBtn_CheckedChanged);
            this.frmSwitchOperationMode4RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode4RadioBtn.Location = new Point(0x18, 0x70);
            this.frmSwitchOperationMode4RadioBtn.Name = "frmSwitchOperationMode4RadioBtn";
            this.frmSwitchOperationMode4RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode4RadioBtn.TabIndex = 4;
            this.frmSwitchOperationMode4RadioBtn.TabStop = true;
            this.frmSwitchOperationMode4RadioBtn.Text = "Test Mode &4";
            this.frmSwitchOperationMode4RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode4RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode4RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode1RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode1RadioBtn.Location = new Point(0x18, 0x2e);
            this.frmSwitchOperationMode1RadioBtn.Name = "frmSwitchOperationMode1RadioBtn";
            this.frmSwitchOperationMode1RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode1RadioBtn.TabIndex = 1;
            this.frmSwitchOperationMode1RadioBtn.TabStop = true;
            this.frmSwitchOperationMode1RadioBtn.Text = "Test Mode &1";
            this.frmSwitchOperationMode1RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode1RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode1RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode3RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode3RadioBtn.Location = new Point(0x18, 90);
            this.frmSwitchOperationMode3RadioBtn.Name = "frmSwitchOperationMode3RadioBtn";
            this.frmSwitchOperationMode3RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode3RadioBtn.TabIndex = 3;
            this.frmSwitchOperationMode3RadioBtn.TabStop = true;
            this.frmSwitchOperationMode3RadioBtn.Text = "Test Mode &3";
            this.frmSwitchOperationMode3RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode3RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode3RadioBtn_CheckedChanged);
            this.frmSwitchOperationMode2RadioBtn.AutoSize = true;
            this.frmSwitchOperationMode2RadioBtn.Location = new Point(0x18, 0x44);
            this.frmSwitchOperationMode2RadioBtn.Name = "frmSwitchOperationMode2RadioBtn";
            this.frmSwitchOperationMode2RadioBtn.Size = new Size(0x55, 0x11);
            this.frmSwitchOperationMode2RadioBtn.TabIndex = 2;
            this.frmSwitchOperationMode2RadioBtn.TabStop = true;
            this.frmSwitchOperationMode2RadioBtn.Text = "Test Mode &2";
            this.frmSwitchOperationMode2RadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchOperationMode2RadioBtn.CheckedChanged += new EventHandler(this.frmSwitchOperationMode2RadioBtn_CheckedChanged);
            this.frmTransmitSerialCancelBtn.Anchor = AnchorStyles.Bottom;
            this.frmTransmitSerialCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmTransmitSerialCancelBtn.Location = new Point(0x106, 0x123);
            this.frmTransmitSerialCancelBtn.Name = "frmTransmitSerialCancelBtn";
            this.frmTransmitSerialCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmTransmitSerialCancelBtn.TabIndex = 4;
            this.frmTransmitSerialCancelBtn.Text = "&Cancel";
            this.frmTransmitSerialCancelBtn.UseVisualStyleBackColor = true;
            this.frmTransmitSerialCancelBtn.Click += new EventHandler(this.frmTransmitSerialCancelBtn_Click);
            this.frmTransmitSerialMessageSendBtn.Anchor = AnchorStyles.Bottom;
            this.frmTransmitSerialMessageSendBtn.Location = new Point(0xab, 0x123);
            this.frmTransmitSerialMessageSendBtn.Name = "frmTransmitSerialMessageSendBtn";
            this.frmTransmitSerialMessageSendBtn.Size = new Size(0x4b, 0x17);
            this.frmTransmitSerialMessageSendBtn.TabIndex = 3;
            this.frmTransmitSerialMessageSendBtn.Text = "&Send";
            this.frmTransmitSerialMessageSendBtn.UseVisualStyleBackColor = true;
            this.frmTransmitSerialMessageSendBtn.Click += new EventHandler(this.frmTransmitSerialMessageSendBtn_Click);
            this.frmSwitchOPerationModeOutputText.Location = new Point(0xc9, 0x97);
            this.frmSwitchOPerationModeOutputText.Multiline = true;
            this.frmSwitchOPerationModeOutputText.Name = "frmSwitchOPerationModeOutputText";
            this.frmSwitchOPerationModeOutputText.ReadOnly = true;
            this.frmSwitchOPerationModeOutputText.Size = new Size(0x11c, 0x7b);
            this.frmSwitchOPerationModeOutputText.TabIndex = 8;
            this.label_Period.AutoSize = true;
            this.label_Period.Location = new Point(0xc6, 0x20);
            this.label_Period.Name = "label_Period";
            this.label_Period.Size = new Size(40, 13);
            this.label_Period.TabIndex = 9;
            this.label_Period.Text = "Period:";
            this.frmSwitchOperationModePeriodTxtBox.Location = new Point(0x101, 0x1c);
            this.frmSwitchOperationModePeriodTxtBox.Name = "frmSwitchOperationModePeriodTxtBox";
            this.frmSwitchOperationModePeriodTxtBox.Size = new Size(0x3b, 20);
            this.frmSwitchOperationModePeriodTxtBox.TabIndex = 1;
            this.label_Seconds.AutoSize = true;
            this.label_Seconds.Location = new Point(0x142, 0x20);
            this.label_Seconds.Name = "label_Seconds";
            this.label_Seconds.Size = new Size(0x2f, 13);
            this.label_Seconds.TabIndex = 11;
            this.label_Seconds.Text = "seconds";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xc6, 0x3a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x9c, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Use all 12 channels to track SV";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xc6, 0x53);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x22, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "SV #:";
            this.frmSwitchOperationSVNumTxtBox.Location = new Point(0x100, 0x4f);
            this.frmSwitchOperationSVNumTxtBox.Name = "frmSwitchOperationSVNumTxtBox";
            this.frmSwitchOperationSVNumTxtBox.Size = new Size(0x3b, 20);
            this.frmSwitchOperationSVNumTxtBox.TabIndex = 2;
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0xd3, 0x74);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x111, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Messages can be viewed in the Response View window";
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0xd4, 0x85);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x9e, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "(Menu: View -> Response View)";
            base.AcceptButton = this.frmTransmitSerialMessageSendBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmTransmitSerialCancelBtn;
            base.ClientSize = new Size(0x1fd, 0x146);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.frmSwitchOperationSVNumTxtBox);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label_Seconds);
            base.Controls.Add(this.frmSwitchOperationModePeriodTxtBox);
            base.Controls.Add(this.label_Period);
            base.Controls.Add(this.frmSwitchOPerationModeOutputText);
            base.Controls.Add(this.frmTransmitSerialCancelBtn);
            base.Controls.Add(this.frmTransmitSerialMessageSendBtn);
            base.Controls.Add(this.frmTransmitSerialMessageOperationModeGroupbox);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmSwitchOperationMode";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Switch Operation Mode";
            base.Load += new EventHandler(this.frmSwitchOperationMode_Load);
            this.frmTransmitSerialMessageOperationModeGroupbox.ResumeLayout(false);
            this.frmTransmitSerialMessageOperationModeGroupbox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

