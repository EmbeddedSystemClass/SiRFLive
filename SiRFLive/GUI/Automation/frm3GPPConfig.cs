﻿namespace SiRFLive.GUI.Automation
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.Configuration;
    using SiRFLive.General;
    using SiRFLive.GUI.DlgsInputMsg;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frm3GPPConfig : Form
    {
        private string _configFilePath = string.Empty;
        private int[] _numSamplesPass = new int[] { 
            0x4d, 0x6a, 0x83, 0x9a, 0xb0, 0xc5, 0xda, 0xee, 0x101, 0x115, 0x127, 0x13a, 0x14d, 0x15f, 0x171, 0x183, 
            0x195, 0x1a6, 440, 0x1c9, 0x1da, 0x1ec, 0x1fd, 0x20e, 0x21f, 560, 0x241, 0x251, 610, 0x273, 0x283, 660, 
            0x2a4, 0x2b5, 0x2c5, 0x2d5, 0x2e6, 0x2f6, 0x306, 790, 0x327, 0x337, 0x347, 0x357, 0x367, 0x377, 0x387, 0x397, 
            0x3a7, 0x3b7, 0x3c7, 0x3d6, 0x3e6, 0x3f6, 0x406, 0x416, 0x425, 0x435, 0x454, 0x464, 0x474, 0x483, 0x493, 0x4a2, 
            0x4b2, 0x4c1, 0x4d1, 0x4e0, 0x4f0, 0x4ff, 0x50f, 0x51e, 0x52e, 0x53d, 0x54d, 0x55c, 0x56b, 0x57b, 0x58a, 0x599, 
            0x5a9, 0x5b8, 0x5c7, 0x5d7, 0x5e6, 0x5f5, 0x604, 0x614, 0x623, 0x632, 0x641, 0x651, 0x660, 0x66f, 0x67e, 0x68d, 
            0x69c, 0x6ac, 0x6bb, 0x6ca, 0x6d9, 0x6e8, 0x6f7, 0x706, 0x715, 0x724, 0x734, 0x743, 0x752, 0x761, 0x770, 0x77f, 
            0x78e, 0x79d, 0x7ac, 0x7bb, 0x7ca, 0x7d9, 0x7e8, 0x7f7, 0x806, 0x815, 0x824, 0x833, 0x842, 0x850, 0x85f, 0x86e, 
            0x87d, 0x88c, 0x89b, 0x8aa, 0x8b9, 0x8c8, 0x8d7, 0x8e5, 0x8f4, 0x903, 0x912, 0x921, 0x930, 0x93f, 0x94d, 0x95c, 
            0x96b, 0x97a, 0x989, 0x998, 0x9a6, 0x9b5, 0x9c4, 0x9d3, 0x9e2, 0x9f0, 0x9ff, 0xa0e, 0xa1d, 0xa2b, 0xa3a, 0xa49, 
            0xa58, 0xa66, 0xa75, 0xa84, 0xa8e, 0xaa1, 0xab0, 0xabf, 0xacd
         };
        private IContainer components;
        private GroupBox frm3GPPConfigAidingTypeGrpBox;
        private TextBox frm3GPPConfigCableLossTxtBox;
        private Button frm3GPPConfigCancelBtn;
        private RadioButton frm3GPPConfigCounterRadioBtn;
        private Label frm3GPPConfigEstTimeLabel;
        private GroupBox frm3GPPConfigFreqTransferModeGrpBox;
        private Label frm3GPPConfigHrzQoSLabel;
        private CheckBox frm3GPPConfigMsAB_A1ChkBox;
        private TextBox frm3GPPConfigMsAB_A1MarginTxtBox;
        private CheckBox frm3GPPConfigMsAB_A2ChkBox;
        private TextBox frm3GPPConfigMsAB_A2MarginTxtBox;
        private CheckBox frm3GPPConfigMsAB_BChkBox;
        private TextBox frm3GPPConfigMsAB_BMarginTxtBox;
        private CheckBox frm3GPPConfigMsAssist1ChkBox;
        private Label frm3GPPConfigMsAssist1MarginLabel;
        private TextBox frm3GPPConfigMsAssist1MarginTxtBox;
        private CheckBox frm3GPPConfigMsAssist2ChkBox;
        private Label frm3GPPConfigMsAssist2MarginLabel;
        private TextBox frm3GPPConfigMsAssist2MarginTxtBox;
        private CheckBox frm3GPPConfigMsBasedChkBox;
        private Label frm3GPPConfigMsBasedMarginLabel;
        private TextBox frm3GPPConfigMsBasedMarginTxtBox;
        private RadioButton frm3GPPConfigNoFreqReqRadioBtn;
        private RadioButton frm3GPPConfigNonCounterRadioBtn;
        private Label frm3GPPConfigNumberCyclesLabel;
        private Button frm3GPPConfigOkBtn;
        private Label frm3GPPConfigPriorityLabel;
        private GroupBox frm3GPPConfigProfileSettingsGrpBox;
        private Label frm3GPPConfigRelFreqAccLabel;
        private Label frm3GPPConfigRespTimeMaxLabel;
        private Label frm3GPPConfigRFFreqOffsetLabel;
        private Label frm3GPPConfigSignalAttnLabel;
        private CheckBox frm3GPPConfigTest1ChkBox;
        private ComboBox frm3GPPConfigTest1CyclesComboBox;
        private TextBox frm3GPPConfigTest1HrzQoSTxtBox;
        private TextBox frm3GPPConfigTest1MaxErrorTxtBox;
        private TextBox frm3GPPConfigTest1PriorityTxtBox;
        private TextBox frm3GPPConfigTest1RelFreqAccTxtBox;
        private TextBox frm3GPPConfigTest1RespMaxTimeTxtBox;
        private TextBox frm3GPPConfigTest1SignalAttnTxtBox;
        private Label frm3GPPConfigTest1TimeLabel;
        private TextBox frm3GPPConfigTest1VrtQoSTxtBox;
        private CheckBox frm3GPPConfigTest2ChkBox;
        private ComboBox frm3GPPConfigTest2CyclesComboBox;
        private TextBox frm3GPPConfigTest2HrzQoSTxtBox;
        private TextBox frm3GPPConfigTest2MaxErrorTxtBox;
        private TextBox frm3GPPConfigTest2PriorityTxtBox;
        private TextBox frm3GPPConfigTest2RelFreqAccTxtBox;
        private TextBox frm3GPPConfigTest2RespMaxTimeTxtBox;
        private TextBox frm3GPPConfigTest2SignalAttnTxtBox;
        private Label frm3GPPConfigTest2TimeLabel;
        private TextBox frm3GPPConfigTest2VrtQoSTxtBox;
        private CheckBox frm3GPPConfigTest3ChkBox;
        private ComboBox frm3GPPConfigTest3CyclesComboBox;
        private TextBox frm3GPPConfigTest3HrzQoSTxtBox;
        private TextBox frm3GPPConfigTest3MaxErrorTxtBox;
        private TextBox frm3GPPConfigTest3PriorityTxtBox;
        private TextBox frm3GPPConfigTest3RelFreqAccTxtBox;
        private TextBox frm3GPPConfigTest3RespMaxTimeTxtBox;
        private TextBox frm3GPPConfigTest3SignalAttnTxtBox;
        private Label frm3GPPConfigTest3TimeLabel;
        private TextBox frm3GPPConfigTest3VrtQoSTxtBox;
        private CheckBox frm3GPPConfigTest4ChkBox;
        private ComboBox frm3GPPConfigTest4CyclesComboBox;
        private TextBox frm3GPPConfigTest4HrzQoSTxtBox;
        private TextBox frm3GPPConfigTest4MaxErrorTxtBox;
        private TextBox frm3GPPConfigTest4PriorityTxtBox;
        private TextBox frm3GPPConfigTest4RelFreqAccTxtBox;
        private TextBox frm3GPPConfigTest4RespMaxTimeTxtBox;
        private TextBox frm3GPPConfigTest4SignalAttnTxtBox;
        private Label frm3GPPConfigTest4TimeLabel;
        private TextBox frm3GPPConfigTest4VrtQoSTxtBox;
        private CheckBox frm3GPPConfigTest5ChkBox;
        private ComboBox frm3GPPConfigTest5CyclesComboBox;
        private TextBox frm3GPPConfigTest5HrzQoSTxtBox;
        private TextBox frm3GPPConfigTest5MaxErrorTxtBox;
        private TextBox frm3GPPConfigTest5PriorityTxtBox;
        private TextBox frm3GPPConfigTest5RelFreqAccTxtBox;
        private TextBox frm3GPPConfigTest5RespMaxTimeTxtBox;
        private TextBox frm3GPPConfigTest5SignalAttnTxtBox;
        private Label frm3GPPConfigTest5TimeLabel;
        private TextBox frm3GPPConfigTest5VrtQoSTxtBox;
        private Label frm3GPPConfigTestSelectionLabel;
        private Label frm3GPPConfigTotalLabel;
        private Label frm3GPPConfigTotalTimeLabel;
        private Label frm3GPPConfigVrtQoSLabel;
        private Button frmEditConfigurationAutoReplyBtn;
        private Button frmEditConfigurationLoadDefautlBtn;
        private frmCommonSimpleInput inputForm = new frmCommonSimpleInput("Number of Cycles:");
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
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label3;
        private Label label30;
        private Label label31;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;

        public frm3GPPConfig(string configFilePath)
        {
            this.InitializeComponent();
            if (configFilePath != string.Empty)
            {
                this._configFilePath = configFilePath.Replace(".py", ".cfg");
            }
            this.inputForm.updateParent += new frmCommonSimpleInput.updateParentEventHandler(this.updateConfigData);
            this.inputForm.MdiParent = base.MdiParent;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frm3GPPConfig_Load(object sender, EventArgs e)
        {
            if (this._configFilePath.Contains("3GPP"))
            {
                this.Text = "3GPP Configuraton";
            }
            else if (this._configFilePath.Contains("TIA916"))
            {
                this.Text = "TIA916 Configuration";
            }
            this.updateLastSettings(this._configFilePath);
        }

        private void frm3GPPConfigCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frm3GPPConfigOkBtn_Click(object sender, EventArgs e)
        {
            if (this.saveToConfigFile() == 0)
            {
                this.updateTestsToRunList();
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void frm3GPPConfigTest1ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.updateEstimatedTime();
        }

        private void frm3GPPConfigTest1CyclesComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.updateMaxError(1);
            }
        }

        private void frm3GPPConfigTest1CyclesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.frm3GPPConfigTest1CyclesComboBox.SelectedItem.ToString() != "USER_DEFINED")
            {
                this.frm3GPPConfigTest1CyclesComboBox.Text = this.frm3GPPConfigTest1CyclesComboBox.SelectedItem.ToString();
            }
            else
            {
                this.inputForm.UpdateType = "CYCLES_TEST1";
                this.inputForm.ShowDialog();
            }
            this.updateEstimatedTime();
            this.updateMaxError(1);
        }

        private void frm3GPPConfigTest2ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.updateEstimatedTime();
        }

        private void frm3GPPConfigTest2CyclesComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.updateMaxError(2);
            }
        }

        private void frm3GPPConfigTest2CyclesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.frm3GPPConfigTest2CyclesComboBox.SelectedItem.ToString() != "USER_DEFINED")
            {
                this.frm3GPPConfigTest2CyclesComboBox.Text = this.frm3GPPConfigTest2CyclesComboBox.SelectedItem.ToString();
            }
            else
            {
                this.inputForm.UpdateType = "CYCLES_TEST2";
                this.inputForm.ShowDialog();
            }
            this.updateEstimatedTime();
            this.updateMaxError(2);
        }

        private void frm3GPPConfigTest3ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.updateEstimatedTime();
        }

        private void frm3GPPConfigTest3CyclesComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.updateMaxError(3);
            }
        }

        private void frm3GPPConfigTest3CyclesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.frm3GPPConfigTest3CyclesComboBox.SelectedItem.ToString() != "USER_DEFINED")
            {
                this.frm3GPPConfigTest3CyclesComboBox.Text = this.frm3GPPConfigTest3CyclesComboBox.SelectedItem.ToString();
            }
            else
            {
                this.inputForm.UpdateType = "CYCLES_TEST3";
                this.inputForm.ShowDialog();
            }
            this.updateEstimatedTime();
            this.updateMaxError(3);
        }

        private void frm3GPPConfigTest4ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.updateEstimatedTime();
        }

        private void frm3GPPConfigTest4CyclesComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.updateMaxError(4);
            }
        }

        private void frm3GPPConfigTest4CyclesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.frm3GPPConfigTest4CyclesComboBox.SelectedItem.ToString() != "USER_DEFINED")
            {
                this.frm3GPPConfigTest4CyclesComboBox.Text = this.frm3GPPConfigTest4CyclesComboBox.SelectedItem.ToString();
            }
            else
            {
                this.inputForm.UpdateType = "CYCLES_TEST4";
                this.inputForm.ShowDialog();
            }
            this.updateEstimatedTime();
            this.updateMaxError(4);
        }

        private void frm3GPPConfigTest5ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.updateEstimatedTime();
        }

        private void frm3GPPConfigTest5CyclesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void frmEditConfigurationAutoReplyBtn_Click(object sender, EventArgs e)
        {
            string path = this._configFilePath;
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Configuration file not found!\n {0}", path), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                frmAutoReply reply = new frmAutoReply();
                reply.AutoReplyConfigFilePath = path;
                reply.CommWindow = new CommunicationManager();
                reply.CommWindow.ReadAutoReplyData(path);
                if (reply.ShowDialog() == DialogResult.Cancel)
                {
                    reply.CommWindow.Dispose();
                    reply.CommWindow = null;
                    reply.Dispose();
                    reply = null;
                }
                else
                {
                    if (reply.CommWindow.AutoReplyCtrl.HWCfgCtrl.FreqAidEnabled == 0)
                    {
                        this.frm3GPPConfigNoFreqReqRadioBtn.Checked = true;
                    }
                    else if (reply.CommWindow.AutoReplyCtrl.HWCfgCtrl.FreqAidMethod == 0)
                    {
                        this.frm3GPPConfigCounterRadioBtn.Checked = true;
                    }
                    else
                    {
                        this.frm3GPPConfigNonCounterRadioBtn.Checked = true;
                    }
                    if (this.frm3GPPConfigTest1ChkBox.Checked)
                    {
                        this.frm3GPPConfigTest1RelFreqAccTxtBox.Text = reply.CommWindow.AutoReplyCtrl.FreqTransferCtrl.Accuracy.ToString();
                        this.frm3GPPConfigTest1HrzQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax.ToString();
                        this.frm3GPPConfigTest1VrtQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.VertErrMax.ToString();
                        this.frm3GPPConfigTest1RespMaxTimeTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax.ToString();
                        if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 0)
                        {
                            this.frm3GPPConfigTest1PriorityTxtBox.Text = "No Priority";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 1)
                        {
                            this.frm3GPPConfigTest1PriorityTxtBox.Text = "Response Time";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 2)
                        {
                            this.frm3GPPConfigTest1PriorityTxtBox.Text = "Position Error";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 3)
                        {
                            this.frm3GPPConfigTest1PriorityTxtBox.Text = "Use Entire Response Time";
                        }
                        else
                        {
                            this.frm3GPPConfigTest1PriorityTxtBox.Text = "Response Time";
                        }
                    }
                    if (this.frm3GPPConfigTest2ChkBox.Checked)
                    {
                        this.frm3GPPConfigTest2RelFreqAccTxtBox.Text = reply.CommWindow.AutoReplyCtrl.FreqTransferCtrl.Accuracy.ToString();
                        this.frm3GPPConfigTest2HrzQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax.ToString();
                        this.frm3GPPConfigTest2VrtQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.VertErrMax.ToString();
                        this.frm3GPPConfigTest2RespMaxTimeTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax.ToString();
                        if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 0)
                        {
                            this.frm3GPPConfigTest2PriorityTxtBox.Text = "No Priority";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 1)
                        {
                            this.frm3GPPConfigTest2PriorityTxtBox.Text = "Response Time";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 2)
                        {
                            this.frm3GPPConfigTest2PriorityTxtBox.Text = "Position Error";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 3)
                        {
                            this.frm3GPPConfigTest2PriorityTxtBox.Text = "Use Entire Response Time";
                        }
                        else
                        {
                            this.frm3GPPConfigTest2PriorityTxtBox.Text = "Response Time";
                        }
                    }
                    if (this.frm3GPPConfigTest3ChkBox.Checked)
                    {
                        this.frm3GPPConfigTest3RelFreqAccTxtBox.Text = reply.CommWindow.AutoReplyCtrl.FreqTransferCtrl.Accuracy.ToString();
                        this.frm3GPPConfigTest3HrzQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax.ToString();
                        this.frm3GPPConfigTest3VrtQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.VertErrMax.ToString();
                        this.frm3GPPConfigTest3RespMaxTimeTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax.ToString();
                        if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 0)
                        {
                            this.frm3GPPConfigTest3PriorityTxtBox.Text = "No Priority";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 1)
                        {
                            this.frm3GPPConfigTest3PriorityTxtBox.Text = "Response Time";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 2)
                        {
                            this.frm3GPPConfigTest3PriorityTxtBox.Text = "Position Error";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 3)
                        {
                            this.frm3GPPConfigTest3PriorityTxtBox.Text = "Use Entire Response Time";
                        }
                        else
                        {
                            this.frm3GPPConfigTest3PriorityTxtBox.Text = "Response Time";
                        }
                    }
                    if (this.frm3GPPConfigTest4ChkBox.Checked)
                    {
                        this.frm3GPPConfigTest4RelFreqAccTxtBox.Text = reply.CommWindow.AutoReplyCtrl.FreqTransferCtrl.Accuracy.ToString();
                        this.frm3GPPConfigTest4HrzQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax.ToString();
                        this.frm3GPPConfigTest4VrtQoSTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.VertErrMax.ToString();
                        this.frm3GPPConfigTest4RespMaxTimeTxtBox.Text = reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax.ToString();
                        if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 0)
                        {
                            this.frm3GPPConfigTest4PriorityTxtBox.Text = "No Priority";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 1)
                        {
                            this.frm3GPPConfigTest4PriorityTxtBox.Text = "Response Time";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 2)
                        {
                            this.frm3GPPConfigTest4PriorityTxtBox.Text = "Position Error";
                        }
                        else if (reply.CommWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority == 3)
                        {
                            this.frm3GPPConfigTest4PriorityTxtBox.Text = "Use Entire Response Time";
                        }
                        else
                        {
                            this.frm3GPPConfigTest4PriorityTxtBox.Text = "Response Time";
                        }
                    }
                    reply.CommWindow.Dispose();
                    reply.CommWindow = null;
                    reply.Dispose();
                    reply = null;
                }
            }
        }

        private void frmEditConfigurationLoadDefautlBtn_Click(object sender, EventArgs e)
        {
            this.frm3GPPConfigTest1CyclesComboBox.Items.Clear();
            this.frm3GPPConfigTest2CyclesComboBox.Items.Clear();
            this.frm3GPPConfigTest3CyclesComboBox.Items.Clear();
            this.frm3GPPConfigTest4CyclesComboBox.Items.Clear();
            this.frm3GPPConfigTest5CyclesComboBox.Items.Clear();
            this.updateDefaultSettings(clsGlobal.InstalledDirectory + @"\scripts\3GPP\default3GPP.cfg");
        }

        private int getMaxError(int maxSamples)
        {
            try
            {
                if (maxSamples < 0x4d)
                {
                    return 1;
                }
                for (int i = 0; i < this._numSamplesPass.Length; i++)
                {
                    if (i == (this._numSamplesPass.Length - 1))
                    {
                        return i;
                    }
                    if (maxSamples == this._numSamplesPass[i])
                    {
                        return (i + 1);
                    }
                    if ((maxSamples > this._numSamplesPass[i]) && (maxSamples < this._numSamplesPass[i + 1]))
                    {
                        return (i + 1);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("3GPP Error\n{0}", exception.Message), "3GPP Config Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return 0;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frm3GPPConfig));
            this.frm3GPPConfigTestSelectionLabel = new Label();
            this.frm3GPPConfigNumberCyclesLabel = new Label();
            this.frm3GPPConfigSignalAttnLabel = new Label();
            this.frm3GPPConfigRelFreqAccLabel = new Label();
            this.frm3GPPConfigHrzQoSLabel = new Label();
            this.frm3GPPConfigVrtQoSLabel = new Label();
            this.frm3GPPConfigRespTimeMaxLabel = new Label();
            this.frm3GPPConfigPriorityLabel = new Label();
            this.frm3GPPConfigEstTimeLabel = new Label();
            this.frm3GPPConfigTest1ChkBox = new CheckBox();
            this.frm3GPPConfigTest2ChkBox = new CheckBox();
            this.frm3GPPConfigTest3ChkBox = new CheckBox();
            this.frm3GPPConfigTest4ChkBox = new CheckBox();
            this.frm3GPPConfigTest5ChkBox = new CheckBox();
            this.frm3GPPConfigTest1CyclesComboBox = new ComboBox();
            this.frm3GPPConfigTest2CyclesComboBox = new ComboBox();
            this.frm3GPPConfigTest3CyclesComboBox = new ComboBox();
            this.frm3GPPConfigTest4CyclesComboBox = new ComboBox();
            this.frm3GPPConfigTest5CyclesComboBox = new ComboBox();
            this.frm3GPPConfigTest1SignalAttnTxtBox = new TextBox();
            this.frm3GPPConfigTest2SignalAttnTxtBox = new TextBox();
            this.frm3GPPConfigTest3SignalAttnTxtBox = new TextBox();
            this.frm3GPPConfigTest4SignalAttnTxtBox = new TextBox();
            this.frm3GPPConfigTest5SignalAttnTxtBox = new TextBox();
            this.frm3GPPConfigTest1RelFreqAccTxtBox = new TextBox();
            this.frm3GPPConfigTest2RelFreqAccTxtBox = new TextBox();
            this.frm3GPPConfigTest3RelFreqAccTxtBox = new TextBox();
            this.frm3GPPConfigTest4RelFreqAccTxtBox = new TextBox();
            this.frm3GPPConfigTest5RelFreqAccTxtBox = new TextBox();
            this.frm3GPPConfigTest1HrzQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest2HrzQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest3HrzQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest4HrzQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest5HrzQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest1VrtQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest2VrtQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest3VrtQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest4VrtQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest5VrtQoSTxtBox = new TextBox();
            this.frm3GPPConfigTest1RespMaxTimeTxtBox = new TextBox();
            this.frm3GPPConfigTest2RespMaxTimeTxtBox = new TextBox();
            this.frm3GPPConfigTest3RespMaxTimeTxtBox = new TextBox();
            this.frm3GPPConfigTest4RespMaxTimeTxtBox = new TextBox();
            this.frm3GPPConfigTest5RespMaxTimeTxtBox = new TextBox();
            this.frm3GPPConfigTest1PriorityTxtBox = new TextBox();
            this.frm3GPPConfigTest2PriorityTxtBox = new TextBox();
            this.frm3GPPConfigTest3PriorityTxtBox = new TextBox();
            this.frm3GPPConfigTest4PriorityTxtBox = new TextBox();
            this.frm3GPPConfigTest5PriorityTxtBox = new TextBox();
            this.frm3GPPConfigFreqTransferModeGrpBox = new GroupBox();
            this.frm3GPPConfigNonCounterRadioBtn = new RadioButton();
            this.frm3GPPConfigNoFreqReqRadioBtn = new RadioButton();
            this.frm3GPPConfigCounterRadioBtn = new RadioButton();
            this.frm3GPPConfigProfileSettingsGrpBox = new GroupBox();
            this.frm3GPPConfigCableLossTxtBox = new TextBox();
            this.frm3GPPConfigRFFreqOffsetLabel = new Label();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label10 = new Label();
            this.frm3GPPConfigOkBtn = new Button();
            this.frm3GPPConfigCancelBtn = new Button();
            this.label11 = new Label();
            this.label12 = new Label();
            this.label13 = new Label();
            this.label14 = new Label();
            this.label15 = new Label();
            this.label16 = new Label();
            this.label17 = new Label();
            this.label18 = new Label();
            this.label19 = new Label();
            this.label20 = new Label();
            this.label21 = new Label();
            this.label22 = new Label();
            this.label23 = new Label();
            this.label24 = new Label();
            this.label25 = new Label();
            this.frm3GPPConfigTest1TimeLabel = new Label();
            this.frm3GPPConfigTest2TimeLabel = new Label();
            this.frm3GPPConfigTest3TimeLabel = new Label();
            this.frm3GPPConfigTest4TimeLabel = new Label();
            this.frm3GPPConfigTest5TimeLabel = new Label();
            this.frm3GPPConfigTotalLabel = new Label();
            this.frm3GPPConfigTotalTimeLabel = new Label();
            this.frm3GPPConfigAidingTypeGrpBox = new GroupBox();
            this.label31 = new Label();
            this.label30 = new Label();
            this.label29 = new Label();
            this.label28 = new Label();
            this.frm3GPPConfigMsAssist2MarginLabel = new Label();
            this.frm3GPPConfigMsAssist1MarginLabel = new Label();
            this.frm3GPPConfigMsBasedMarginLabel = new Label();
            this.frm3GPPConfigMsAB_A2ChkBox = new CheckBox();
            this.frm3GPPConfigMsAB_A1ChkBox = new CheckBox();
            this.frm3GPPConfigMsAB_BChkBox = new CheckBox();
            this.frm3GPPConfigMsAssist2ChkBox = new CheckBox();
            this.frm3GPPConfigMsAssist1ChkBox = new CheckBox();
            this.frm3GPPConfigMsBasedChkBox = new CheckBox();
            this.frm3GPPConfigMsAB_A2MarginTxtBox = new TextBox();
            this.frm3GPPConfigMsAB_A1MarginTxtBox = new TextBox();
            this.frm3GPPConfigMsAB_BMarginTxtBox = new TextBox();
            this.frm3GPPConfigMsAssist2MarginTxtBox = new TextBox();
            this.frm3GPPConfigMsAssist1MarginTxtBox = new TextBox();
            this.frm3GPPConfigMsBasedMarginTxtBox = new TextBox();
            this.frmEditConfigurationAutoReplyBtn = new Button();
            this.label26 = new Label();
            this.frm3GPPConfigTest1MaxErrorTxtBox = new TextBox();
            this.frm3GPPConfigTest2MaxErrorTxtBox = new TextBox();
            this.frm3GPPConfigTest3MaxErrorTxtBox = new TextBox();
            this.frm3GPPConfigTest4MaxErrorTxtBox = new TextBox();
            this.frm3GPPConfigTest5MaxErrorTxtBox = new TextBox();
            this.label27 = new Label();
            this.frmEditConfigurationLoadDefautlBtn = new Button();
            this.frm3GPPConfigFreqTransferModeGrpBox.SuspendLayout();
            this.frm3GPPConfigProfileSettingsGrpBox.SuspendLayout();
            this.frm3GPPConfigAidingTypeGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.frm3GPPConfigTestSelectionLabel.AutoSize = true;
            this.frm3GPPConfigTestSelectionLabel.Location = new Point(0x12, 0x19);
            this.frm3GPPConfigTestSelectionLabel.Name = "frm3GPPConfigTestSelectionLabel";
            this.frm3GPPConfigTestSelectionLabel.Size = new Size(0x4b, 13);
            this.frm3GPPConfigTestSelectionLabel.TabIndex = 0;
            this.frm3GPPConfigTestSelectionLabel.Text = "Test Selection";
            this.frm3GPPConfigNumberCyclesLabel.AutoSize = true;
            this.frm3GPPConfigNumberCyclesLabel.Location = new Point(0xb5, 0x19);
            this.frm3GPPConfigNumberCyclesLabel.Name = "frm3GPPConfigNumberCyclesLabel";
            this.frm3GPPConfigNumberCyclesLabel.Size = new Size(0x4d, 13);
            this.frm3GPPConfigNumberCyclesLabel.TabIndex = 1;
            this.frm3GPPConfigNumberCyclesLabel.Text = "Max # of Trials";
            this.frm3GPPConfigSignalAttnLabel.AutoSize = true;
            this.frm3GPPConfigSignalAttnLabel.Location = new Point(0x159, 0x19);
            this.frm3GPPConfigSignalAttnLabel.Name = "frm3GPPConfigSignalAttnLabel";
            this.frm3GPPConfigSignalAttnLabel.Size = new Size(0x3a, 13);
            this.frm3GPPConfigSignalAttnLabel.TabIndex = 2;
            this.frm3GPPConfigSignalAttnLabel.Text = "Signal Attn";
            this.frm3GPPConfigRelFreqAccLabel.AutoSize = true;
            this.frm3GPPConfigRelFreqAccLabel.Location = new Point(0x1a5, 0x19);
            this.frm3GPPConfigRelFreqAccLabel.Name = "frm3GPPConfigRelFreqAccLabel";
            this.frm3GPPConfigRelFreqAccLabel.Size = new Size(0x4b, 13);
            this.frm3GPPConfigRelFreqAccLabel.TabIndex = 3;
            this.frm3GPPConfigRelFreqAccLabel.Text = "Rel_Freq_Acc";
            this.frm3GPPConfigHrzQoSLabel.AutoSize = true;
            this.frm3GPPConfigHrzQoSLabel.Location = new Point(0x202, 0x19);
            this.frm3GPPConfigHrzQoSLabel.Name = "frm3GPPConfigHrzQoSLabel";
            this.frm3GPPConfigHrzQoSLabel.Size = new Size(50, 13);
            this.frm3GPPConfigHrzQoSLabel.TabIndex = 4;
            this.frm3GPPConfigHrzQoSLabel.Text = "Hrz. QoS";
            this.frm3GPPConfigVrtQoSLabel.AutoSize = true;
            this.frm3GPPConfigVrtQoSLabel.Location = new Point(0x257, 0x19);
            this.frm3GPPConfigVrtQoSLabel.Name = "frm3GPPConfigVrtQoSLabel";
            this.frm3GPPConfigVrtQoSLabel.Size = new Size(0x2f, 13);
            this.frm3GPPConfigVrtQoSLabel.TabIndex = 5;
            this.frm3GPPConfigVrtQoSLabel.Text = "Vrt. QoS";
            this.frm3GPPConfigRespTimeMaxLabel.AutoSize = true;
            this.frm3GPPConfigRespTimeMaxLabel.Location = new Point(0x293, 0x19);
            this.frm3GPPConfigRespTimeMaxLabel.Name = "frm3GPPConfigRespTimeMaxLabel";
            this.frm3GPPConfigRespTimeMaxLabel.Size = new Size(0x68, 13);
            this.frm3GPPConfigRespTimeMaxLabel.TabIndex = 6;
            this.frm3GPPConfigRespTimeMaxLabel.Text = "Response Time Max";
            this.frm3GPPConfigPriorityLabel.AutoSize = true;
            this.frm3GPPConfigPriorityLabel.Location = new Point(0x308, 0x19);
            this.frm3GPPConfigPriorityLabel.Name = "frm3GPPConfigPriorityLabel";
            this.frm3GPPConfigPriorityLabel.Size = new Size(0x26, 13);
            this.frm3GPPConfigPriorityLabel.TabIndex = 7;
            this.frm3GPPConfigPriorityLabel.Text = "Priority";
            this.frm3GPPConfigEstTimeLabel.AutoSize = true;
            this.frm3GPPConfigEstTimeLabel.Location = new Point(840, 0x19);
            this.frm3GPPConfigEstTimeLabel.Name = "frm3GPPConfigEstTimeLabel";
            this.frm3GPPConfigEstTimeLabel.Size = new Size(0x4f, 13);
            this.frm3GPPConfigEstTimeLabel.TabIndex = 8;
            this.frm3GPPConfigEstTimeLabel.Text = "Est. Time(secs)";
            this.frm3GPPConfigTest1ChkBox.AutoSize = true;
            this.frm3GPPConfigTest1ChkBox.Location = new Point(0x15, 0x2e);
            this.frm3GPPConfigTest1ChkBox.Name = "frm3GPPConfigTest1ChkBox";
            this.frm3GPPConfigTest1ChkBox.Size = new Size(0x70, 0x11);
            this.frm3GPPConfigTest1ChkBox.TabIndex = 0;
            this.frm3GPPConfigTest1ChkBox.Text = "Test &1 - Sensitivity";
            this.frm3GPPConfigTest1ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigTest1ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigTest2ChkBox.AutoSize = true;
            this.frm3GPPConfigTest2ChkBox.Location = new Point(0x15, 0x44);
            this.frm3GPPConfigTest2ChkBox.Name = "frm3GPPConfigTest2ChkBox";
            this.frm3GPPConfigTest2ChkBox.Size = new Size(0x97, 0x11);
            this.frm3GPPConfigTest2ChkBox.TabIndex = 1;
            this.frm3GPPConfigTest2ChkBox.Text = "Test &2 - Nominal Accuracy";
            this.frm3GPPConfigTest2ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigTest2ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest2ChkBox_CheckedChanged);
            this.frm3GPPConfigTest3ChkBox.AutoSize = true;
            this.frm3GPPConfigTest3ChkBox.Location = new Point(0x15, 90);
            this.frm3GPPConfigTest3ChkBox.Name = "frm3GPPConfigTest3ChkBox";
            this.frm3GPPConfigTest3ChkBox.Size = new Size(0x8d, 0x11);
            this.frm3GPPConfigTest3ChkBox.TabIndex = 2;
            this.frm3GPPConfigTest3ChkBox.Text = "Test &3 - Dynamic Range";
            this.frm3GPPConfigTest3ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigTest3ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest3ChkBox_CheckedChanged);
            this.frm3GPPConfigTest4ChkBox.AutoSize = true;
            this.frm3GPPConfigTest4ChkBox.Location = new Point(0x15, 0x70);
            this.frm3GPPConfigTest4ChkBox.Name = "frm3GPPConfigTest4ChkBox";
            this.frm3GPPConfigTest4ChkBox.Size = new Size(0x69, 0x11);
            this.frm3GPPConfigTest4ChkBox.TabIndex = 3;
            this.frm3GPPConfigTest4ChkBox.Text = "Test &4 -Multipath";
            this.frm3GPPConfigTest4ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigTest4ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest4ChkBox_CheckedChanged);
            this.frm3GPPConfigTest5ChkBox.AutoSize = true;
            this.frm3GPPConfigTest5ChkBox.Location = new Point(0x15, 0x86);
            this.frm3GPPConfigTest5ChkBox.Name = "frm3GPPConfigTest5ChkBox";
            this.frm3GPPConfigTest5ChkBox.Size = new Size(0x8e, 0x11);
            this.frm3GPPConfigTest5ChkBox.TabIndex = 4;
            this.frm3GPPConfigTest5ChkBox.Text = "Test &5 -Moving Scenario";
            this.frm3GPPConfigTest5ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigTest5ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest5ChkBox_CheckedChanged);
            this.frm3GPPConfigTest1CyclesComboBox.FormattingEnabled = true;
            this.frm3GPPConfigTest1CyclesComboBox.Location = new Point(0xb9, 0x2c);
            this.frm3GPPConfigTest1CyclesComboBox.Name = "frm3GPPConfigTest1CyclesComboBox";
            this.frm3GPPConfigTest1CyclesComboBox.Size = new Size(0x45, 0x15);
            this.frm3GPPConfigTest1CyclesComboBox.TabIndex = 5;
            this.frm3GPPConfigTest1CyclesComboBox.SelectedIndexChanged += new EventHandler(this.frm3GPPConfigTest1CyclesComboBox_SelectedIndexChanged);
            this.frm3GPPConfigTest1CyclesComboBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frm3GPPConfigTest1CyclesComboBox_PreviewKeyDown);
            this.frm3GPPConfigTest2CyclesComboBox.FormattingEnabled = true;
            this.frm3GPPConfigTest2CyclesComboBox.Location = new Point(0xb9, 0x42);
            this.frm3GPPConfigTest2CyclesComboBox.Name = "frm3GPPConfigTest2CyclesComboBox";
            this.frm3GPPConfigTest2CyclesComboBox.Size = new Size(0x45, 0x15);
            this.frm3GPPConfigTest2CyclesComboBox.TabIndex = 12;
            this.frm3GPPConfigTest2CyclesComboBox.SelectedIndexChanged += new EventHandler(this.frm3GPPConfigTest2CyclesComboBox_SelectedIndexChanged);
            this.frm3GPPConfigTest2CyclesComboBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frm3GPPConfigTest2CyclesComboBox_PreviewKeyDown);
            this.frm3GPPConfigTest3CyclesComboBox.FormattingEnabled = true;
            this.frm3GPPConfigTest3CyclesComboBox.Location = new Point(0xb9, 0x58);
            this.frm3GPPConfigTest3CyclesComboBox.Name = "frm3GPPConfigTest3CyclesComboBox";
            this.frm3GPPConfigTest3CyclesComboBox.Size = new Size(0x45, 0x15);
            this.frm3GPPConfigTest3CyclesComboBox.TabIndex = 0x13;
            this.frm3GPPConfigTest3CyclesComboBox.SelectedIndexChanged += new EventHandler(this.frm3GPPConfigTest3CyclesComboBox_SelectedIndexChanged);
            this.frm3GPPConfigTest3CyclesComboBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frm3GPPConfigTest3CyclesComboBox_PreviewKeyDown);
            this.frm3GPPConfigTest4CyclesComboBox.FormattingEnabled = true;
            this.frm3GPPConfigTest4CyclesComboBox.Location = new Point(0xb9, 110);
            this.frm3GPPConfigTest4CyclesComboBox.Name = "frm3GPPConfigTest4CyclesComboBox";
            this.frm3GPPConfigTest4CyclesComboBox.Size = new Size(0x45, 0x15);
            this.frm3GPPConfigTest4CyclesComboBox.TabIndex = 0x1a;
            this.frm3GPPConfigTest4CyclesComboBox.SelectedIndexChanged += new EventHandler(this.frm3GPPConfigTest4CyclesComboBox_SelectedIndexChanged);
            this.frm3GPPConfigTest4CyclesComboBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frm3GPPConfigTest4CyclesComboBox_PreviewKeyDown);
            this.frm3GPPConfigTest5CyclesComboBox.Enabled = false;
            this.frm3GPPConfigTest5CyclesComboBox.FormattingEnabled = true;
            this.frm3GPPConfigTest5CyclesComboBox.Location = new Point(0xb9, 0x84);
            this.frm3GPPConfigTest5CyclesComboBox.Name = "frm3GPPConfigTest5CyclesComboBox";
            this.frm3GPPConfigTest5CyclesComboBox.Size = new Size(0x45, 0x15);
            this.frm3GPPConfigTest5CyclesComboBox.TabIndex = 0x21;
            this.frm3GPPConfigTest5CyclesComboBox.SelectedIndexChanged += new EventHandler(this.frm3GPPConfigTest5CyclesComboBox_SelectedIndexChanged);
            this.frm3GPPConfigTest1SignalAttnTxtBox.Enabled = false;
            this.frm3GPPConfigTest1SignalAttnTxtBox.Location = new Point(0x15c, 0x2d);
            this.frm3GPPConfigTest1SignalAttnTxtBox.Name = "frm3GPPConfigTest1SignalAttnTxtBox";
            this.frm3GPPConfigTest1SignalAttnTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest1SignalAttnTxtBox.TabIndex = 6;
            this.frm3GPPConfigTest2SignalAttnTxtBox.Enabled = false;
            this.frm3GPPConfigTest2SignalAttnTxtBox.Location = new Point(0x15c, 0x43);
            this.frm3GPPConfigTest2SignalAttnTxtBox.Name = "frm3GPPConfigTest2SignalAttnTxtBox";
            this.frm3GPPConfigTest2SignalAttnTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest2SignalAttnTxtBox.TabIndex = 13;
            this.frm3GPPConfigTest3SignalAttnTxtBox.Enabled = false;
            this.frm3GPPConfigTest3SignalAttnTxtBox.Location = new Point(0x15c, 0x59);
            this.frm3GPPConfigTest3SignalAttnTxtBox.Name = "frm3GPPConfigTest3SignalAttnTxtBox";
            this.frm3GPPConfigTest3SignalAttnTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest3SignalAttnTxtBox.TabIndex = 20;
            this.frm3GPPConfigTest4SignalAttnTxtBox.Enabled = false;
            this.frm3GPPConfigTest4SignalAttnTxtBox.Location = new Point(0x15c, 0x6f);
            this.frm3GPPConfigTest4SignalAttnTxtBox.Name = "frm3GPPConfigTest4SignalAttnTxtBox";
            this.frm3GPPConfigTest4SignalAttnTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest4SignalAttnTxtBox.TabIndex = 0x1b;
            this.frm3GPPConfigTest5SignalAttnTxtBox.Enabled = false;
            this.frm3GPPConfigTest5SignalAttnTxtBox.Location = new Point(0x15c, 0x85);
            this.frm3GPPConfigTest5SignalAttnTxtBox.Name = "frm3GPPConfigTest5SignalAttnTxtBox";
            this.frm3GPPConfigTest5SignalAttnTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest5SignalAttnTxtBox.TabIndex = 0x22;
            this.frm3GPPConfigTest1RelFreqAccTxtBox.Enabled = false;
            this.frm3GPPConfigTest1RelFreqAccTxtBox.Location = new Point(0x1ad, 0x2d);
            this.frm3GPPConfigTest1RelFreqAccTxtBox.Name = "frm3GPPConfigTest1RelFreqAccTxtBox";
            this.frm3GPPConfigTest1RelFreqAccTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest1RelFreqAccTxtBox.TabIndex = 7;
            this.frm3GPPConfigTest2RelFreqAccTxtBox.Enabled = false;
            this.frm3GPPConfigTest2RelFreqAccTxtBox.Location = new Point(0x1ad, 0x43);
            this.frm3GPPConfigTest2RelFreqAccTxtBox.Name = "frm3GPPConfigTest2RelFreqAccTxtBox";
            this.frm3GPPConfigTest2RelFreqAccTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest2RelFreqAccTxtBox.TabIndex = 14;
            this.frm3GPPConfigTest3RelFreqAccTxtBox.Enabled = false;
            this.frm3GPPConfigTest3RelFreqAccTxtBox.Location = new Point(0x1ad, 0x59);
            this.frm3GPPConfigTest3RelFreqAccTxtBox.Name = "frm3GPPConfigTest3RelFreqAccTxtBox";
            this.frm3GPPConfigTest3RelFreqAccTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest3RelFreqAccTxtBox.TabIndex = 0x15;
            this.frm3GPPConfigTest4RelFreqAccTxtBox.Enabled = false;
            this.frm3GPPConfigTest4RelFreqAccTxtBox.Location = new Point(0x1ad, 0x6f);
            this.frm3GPPConfigTest4RelFreqAccTxtBox.Name = "frm3GPPConfigTest4RelFreqAccTxtBox";
            this.frm3GPPConfigTest4RelFreqAccTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest4RelFreqAccTxtBox.TabIndex = 0x1c;
            this.frm3GPPConfigTest5RelFreqAccTxtBox.Enabled = false;
            this.frm3GPPConfigTest5RelFreqAccTxtBox.Location = new Point(0x1ad, 0x85);
            this.frm3GPPConfigTest5RelFreqAccTxtBox.Name = "frm3GPPConfigTest5RelFreqAccTxtBox";
            this.frm3GPPConfigTest5RelFreqAccTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest5RelFreqAccTxtBox.TabIndex = 0x23;
            this.frm3GPPConfigTest1HrzQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest1HrzQoSTxtBox.Location = new Point(0x1ff, 0x2d);
            this.frm3GPPConfigTest1HrzQoSTxtBox.Name = "frm3GPPConfigTest1HrzQoSTxtBox";
            this.frm3GPPConfigTest1HrzQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest1HrzQoSTxtBox.TabIndex = 8;
            this.frm3GPPConfigTest2HrzQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest2HrzQoSTxtBox.Location = new Point(0x1ff, 0x43);
            this.frm3GPPConfigTest2HrzQoSTxtBox.Name = "frm3GPPConfigTest2HrzQoSTxtBox";
            this.frm3GPPConfigTest2HrzQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest2HrzQoSTxtBox.TabIndex = 15;
            this.frm3GPPConfigTest3HrzQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest3HrzQoSTxtBox.Location = new Point(0x1ff, 0x59);
            this.frm3GPPConfigTest3HrzQoSTxtBox.Name = "frm3GPPConfigTest3HrzQoSTxtBox";
            this.frm3GPPConfigTest3HrzQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest3HrzQoSTxtBox.TabIndex = 0x16;
            this.frm3GPPConfigTest4HrzQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest4HrzQoSTxtBox.Location = new Point(0x1ff, 0x6f);
            this.frm3GPPConfigTest4HrzQoSTxtBox.Name = "frm3GPPConfigTest4HrzQoSTxtBox";
            this.frm3GPPConfigTest4HrzQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest4HrzQoSTxtBox.TabIndex = 0x1d;
            this.frm3GPPConfigTest5HrzQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest5HrzQoSTxtBox.Location = new Point(0x1ff, 0x85);
            this.frm3GPPConfigTest5HrzQoSTxtBox.Name = "frm3GPPConfigTest5HrzQoSTxtBox";
            this.frm3GPPConfigTest5HrzQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest5HrzQoSTxtBox.TabIndex = 0x24;
            this.frm3GPPConfigTest1VrtQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest1VrtQoSTxtBox.Location = new Point(0x254, 0x2d);
            this.frm3GPPConfigTest1VrtQoSTxtBox.Name = "frm3GPPConfigTest1VrtQoSTxtBox";
            this.frm3GPPConfigTest1VrtQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest1VrtQoSTxtBox.TabIndex = 9;
            this.frm3GPPConfigTest2VrtQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest2VrtQoSTxtBox.Location = new Point(0x254, 0x43);
            this.frm3GPPConfigTest2VrtQoSTxtBox.Name = "frm3GPPConfigTest2VrtQoSTxtBox";
            this.frm3GPPConfigTest2VrtQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest2VrtQoSTxtBox.TabIndex = 0x10;
            this.frm3GPPConfigTest3VrtQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest3VrtQoSTxtBox.Location = new Point(0x254, 0x59);
            this.frm3GPPConfigTest3VrtQoSTxtBox.Name = "frm3GPPConfigTest3VrtQoSTxtBox";
            this.frm3GPPConfigTest3VrtQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest3VrtQoSTxtBox.TabIndex = 0x17;
            this.frm3GPPConfigTest4VrtQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest4VrtQoSTxtBox.Location = new Point(0x254, 0x6f);
            this.frm3GPPConfigTest4VrtQoSTxtBox.Name = "frm3GPPConfigTest4VrtQoSTxtBox";
            this.frm3GPPConfigTest4VrtQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest4VrtQoSTxtBox.TabIndex = 30;
            this.frm3GPPConfigTest5VrtQoSTxtBox.Enabled = false;
            this.frm3GPPConfigTest5VrtQoSTxtBox.Location = new Point(0x254, 0x85);
            this.frm3GPPConfigTest5VrtQoSTxtBox.Name = "frm3GPPConfigTest5VrtQoSTxtBox";
            this.frm3GPPConfigTest5VrtQoSTxtBox.Size = new Size(50, 20);
            this.frm3GPPConfigTest5VrtQoSTxtBox.TabIndex = 0x25;
            this.frm3GPPConfigTest1RespMaxTimeTxtBox.Enabled = false;
            this.frm3GPPConfigTest1RespMaxTimeTxtBox.Location = new Point(0x2af, 0x2d);
            this.frm3GPPConfigTest1RespMaxTimeTxtBox.Name = "frm3GPPConfigTest1RespMaxTimeTxtBox";
            this.frm3GPPConfigTest1RespMaxTimeTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest1RespMaxTimeTxtBox.TabIndex = 10;
            this.frm3GPPConfigTest2RespMaxTimeTxtBox.Enabled = false;
            this.frm3GPPConfigTest2RespMaxTimeTxtBox.Location = new Point(0x2af, 0x43);
            this.frm3GPPConfigTest2RespMaxTimeTxtBox.Name = "frm3GPPConfigTest2RespMaxTimeTxtBox";
            this.frm3GPPConfigTest2RespMaxTimeTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest2RespMaxTimeTxtBox.TabIndex = 0x11;
            this.frm3GPPConfigTest3RespMaxTimeTxtBox.Enabled = false;
            this.frm3GPPConfigTest3RespMaxTimeTxtBox.Location = new Point(0x2af, 0x59);
            this.frm3GPPConfigTest3RespMaxTimeTxtBox.Name = "frm3GPPConfigTest3RespMaxTimeTxtBox";
            this.frm3GPPConfigTest3RespMaxTimeTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest3RespMaxTimeTxtBox.TabIndex = 0x18;
            this.frm3GPPConfigTest4RespMaxTimeTxtBox.Enabled = false;
            this.frm3GPPConfigTest4RespMaxTimeTxtBox.Location = new Point(0x2af, 0x6f);
            this.frm3GPPConfigTest4RespMaxTimeTxtBox.Name = "frm3GPPConfigTest4RespMaxTimeTxtBox";
            this.frm3GPPConfigTest4RespMaxTimeTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest4RespMaxTimeTxtBox.TabIndex = 0x1f;
            this.frm3GPPConfigTest5RespMaxTimeTxtBox.Enabled = false;
            this.frm3GPPConfigTest5RespMaxTimeTxtBox.Location = new Point(0x2af, 0x85);
            this.frm3GPPConfigTest5RespMaxTimeTxtBox.Name = "frm3GPPConfigTest5RespMaxTimeTxtBox";
            this.frm3GPPConfigTest5RespMaxTimeTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest5RespMaxTimeTxtBox.TabIndex = 0x26;
            this.frm3GPPConfigTest1PriorityTxtBox.Enabled = false;
            this.frm3GPPConfigTest1PriorityTxtBox.Location = new Point(0x2f9, 0x2d);
            this.frm3GPPConfigTest1PriorityTxtBox.Name = "frm3GPPConfigTest1PriorityTxtBox";
            this.frm3GPPConfigTest1PriorityTxtBox.Size = new Size(0x45, 20);
            this.frm3GPPConfigTest1PriorityTxtBox.TabIndex = 11;
            this.frm3GPPConfigTest2PriorityTxtBox.Enabled = false;
            this.frm3GPPConfigTest2PriorityTxtBox.Location = new Point(0x2f9, 0x43);
            this.frm3GPPConfigTest2PriorityTxtBox.Name = "frm3GPPConfigTest2PriorityTxtBox";
            this.frm3GPPConfigTest2PriorityTxtBox.Size = new Size(0x45, 20);
            this.frm3GPPConfigTest2PriorityTxtBox.TabIndex = 0x12;
            this.frm3GPPConfigTest3PriorityTxtBox.Enabled = false;
            this.frm3GPPConfigTest3PriorityTxtBox.Location = new Point(0x2f9, 0x59);
            this.frm3GPPConfigTest3PriorityTxtBox.Name = "frm3GPPConfigTest3PriorityTxtBox";
            this.frm3GPPConfigTest3PriorityTxtBox.Size = new Size(0x45, 20);
            this.frm3GPPConfigTest3PriorityTxtBox.TabIndex = 0x19;
            this.frm3GPPConfigTest4PriorityTxtBox.Enabled = false;
            this.frm3GPPConfigTest4PriorityTxtBox.Location = new Point(0x2f9, 0x6f);
            this.frm3GPPConfigTest4PriorityTxtBox.Name = "frm3GPPConfigTest4PriorityTxtBox";
            this.frm3GPPConfigTest4PriorityTxtBox.Size = new Size(0x45, 20);
            this.frm3GPPConfigTest4PriorityTxtBox.TabIndex = 0x20;
            this.frm3GPPConfigTest5PriorityTxtBox.Enabled = false;
            this.frm3GPPConfigTest5PriorityTxtBox.Location = new Point(0x2f9, 0x85);
            this.frm3GPPConfigTest5PriorityTxtBox.Name = "frm3GPPConfigTest5PriorityTxtBox";
            this.frm3GPPConfigTest5PriorityTxtBox.Size = new Size(0x45, 20);
            this.frm3GPPConfigTest5PriorityTxtBox.TabIndex = 0x27;
            this.frm3GPPConfigFreqTransferModeGrpBox.Controls.Add(this.frm3GPPConfigNonCounterRadioBtn);
            this.frm3GPPConfigFreqTransferModeGrpBox.Controls.Add(this.frm3GPPConfigNoFreqReqRadioBtn);
            this.frm3GPPConfigFreqTransferModeGrpBox.Controls.Add(this.frm3GPPConfigCounterRadioBtn);
            this.frm3GPPConfigFreqTransferModeGrpBox.Location = new Point(420, 0xd7);
            this.frm3GPPConfigFreqTransferModeGrpBox.Name = "frm3GPPConfigFreqTransferModeGrpBox";
            this.frm3GPPConfigFreqTransferModeGrpBox.Size = new Size(0x93, 0x61);
            this.frm3GPPConfigFreqTransferModeGrpBox.TabIndex = 0x29;
            this.frm3GPPConfigFreqTransferModeGrpBox.TabStop = false;
            this.frm3GPPConfigFreqTransferModeGrpBox.Text = "Freq Transfer Mode";
            this.frm3GPPConfigNonCounterRadioBtn.AutoSize = true;
            this.frm3GPPConfigNonCounterRadioBtn.Enabled = false;
            this.frm3GPPConfigNonCounterRadioBtn.Location = new Point(0x12, 0x2b);
            this.frm3GPPConfigNonCounterRadioBtn.Name = "frm3GPPConfigNonCounterRadioBtn";
            this.frm3GPPConfigNonCounterRadioBtn.Size = new Size(0x55, 0x11);
            this.frm3GPPConfigNonCounterRadioBtn.TabIndex = 0;
            this.frm3GPPConfigNonCounterRadioBtn.TabStop = true;
            this.frm3GPPConfigNonCounterRadioBtn.Text = "Non-Counter";
            this.frm3GPPConfigNonCounterRadioBtn.UseVisualStyleBackColor = true;
            this.frm3GPPConfigNoFreqReqRadioBtn.AutoSize = true;
            this.frm3GPPConfigNoFreqReqRadioBtn.Enabled = false;
            this.frm3GPPConfigNoFreqReqRadioBtn.Location = new Point(0x12, 0x44);
            this.frm3GPPConfigNoFreqReqRadioBtn.Name = "frm3GPPConfigNoFreqReqRadioBtn";
            this.frm3GPPConfigNoFreqReqRadioBtn.Size = new Size(0x6a, 0x11);
            this.frm3GPPConfigNoFreqReqRadioBtn.TabIndex = 0;
            this.frm3GPPConfigNoFreqReqRadioBtn.TabStop = true;
            this.frm3GPPConfigNoFreqReqRadioBtn.Text = "No Freq Request";
            this.frm3GPPConfigNoFreqReqRadioBtn.UseVisualStyleBackColor = true;
            this.frm3GPPConfigCounterRadioBtn.AutoSize = true;
            this.frm3GPPConfigCounterRadioBtn.Enabled = false;
            this.frm3GPPConfigCounterRadioBtn.Location = new Point(0x12, 20);
            this.frm3GPPConfigCounterRadioBtn.Name = "frm3GPPConfigCounterRadioBtn";
            this.frm3GPPConfigCounterRadioBtn.Size = new Size(0x3e, 0x11);
            this.frm3GPPConfigCounterRadioBtn.TabIndex = 0;
            this.frm3GPPConfigCounterRadioBtn.TabStop = true;
            this.frm3GPPConfigCounterRadioBtn.Text = "Counter";
            this.frm3GPPConfigCounterRadioBtn.UseVisualStyleBackColor = true;
            this.frm3GPPConfigProfileSettingsGrpBox.Controls.Add(this.frm3GPPConfigCableLossTxtBox);
            this.frm3GPPConfigProfileSettingsGrpBox.Controls.Add(this.frm3GPPConfigRFFreqOffsetLabel);
            this.frm3GPPConfigProfileSettingsGrpBox.Location = new Point(420, 0x13e);
            this.frm3GPPConfigProfileSettingsGrpBox.Name = "frm3GPPConfigProfileSettingsGrpBox";
            this.frm3GPPConfigProfileSettingsGrpBox.Size = new Size(0x93, 0x4c);
            this.frm3GPPConfigProfileSettingsGrpBox.TabIndex = 0x2a;
            this.frm3GPPConfigProfileSettingsGrpBox.TabStop = false;
            this.frm3GPPConfigProfileSettingsGrpBox.Text = "Profile Settings";
            this.frm3GPPConfigCableLossTxtBox.Location = new Point(0x17, 0x2b);
            this.frm3GPPConfigCableLossTxtBox.Name = "frm3GPPConfigCableLossTxtBox";
            this.frm3GPPConfigCableLossTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigCableLossTxtBox.TabIndex = 0x2b;
            this.frm3GPPConfigRFFreqOffsetLabel.AutoSize = true;
            this.frm3GPPConfigRFFreqOffsetLabel.Location = new Point(0x21, 0x18);
            this.frm3GPPConfigRFFreqOffsetLabel.Name = "frm3GPPConfigRFFreqOffsetLabel";
            this.frm3GPPConfigRFFreqOffsetLabel.Size = new Size(0x51, 13);
            this.frm3GPPConfigRFFreqOffsetLabel.TabIndex = 0x60;
            this.frm3GPPConfigRFFreqOffsetLabel.Text = "Cable Loss (dB)";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x183, 0x31);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1a, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "(dB)";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x183, 0x47);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1a, 13);
            this.label2.TabIndex = 0x1a;
            this.label2.Text = "(dB)";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x183, 0x5d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1a, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "(dB)";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x183, 0x73);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1a, 13);
            this.label4.TabIndex = 0x36;
            this.label4.Text = "(dB)";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x183, 0x89);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1a, 13);
            this.label5.TabIndex = 0x44;
            this.label5.Text = "(dB)";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x1d5, 0x31);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x21, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "(ppm)";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x1d5, 0x47);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x21, 13);
            this.label7.TabIndex = 0x1c;
            this.label7.Text = "(ppm)";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x1d5, 0x5d);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x21, 13);
            this.label8.TabIndex = 0x2a;
            this.label8.Text = "(ppm)";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x1d5, 0x73);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x21, 13);
            this.label9.TabIndex = 0x38;
            this.label9.Text = "(ppm)";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x1d5, 0x89);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x21, 13);
            this.label10.TabIndex = 70;
            this.label10.Text = "(ppm)";
            this.frm3GPPConfigOkBtn.Location = new Point(690, 0x12a);
            this.frm3GPPConfigOkBtn.Name = "frm3GPPConfigOkBtn";
            this.frm3GPPConfigOkBtn.Size = new Size(0x7f, 0x17);
            this.frm3GPPConfigOkBtn.TabIndex = 0x2d;
            this.frm3GPPConfigOkBtn.Text = "&OK";
            this.frm3GPPConfigOkBtn.UseVisualStyleBackColor = true;
            this.frm3GPPConfigOkBtn.Click += new EventHandler(this.frm3GPPConfigOkBtn_Click);
            this.frm3GPPConfigCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frm3GPPConfigCancelBtn.Location = new Point(690, 0x149);
            this.frm3GPPConfigCancelBtn.Name = "frm3GPPConfigCancelBtn";
            this.frm3GPPConfigCancelBtn.Size = new Size(0x7f, 0x17);
            this.frm3GPPConfigCancelBtn.TabIndex = 0x2e;
            this.frm3GPPConfigCancelBtn.Text = "&Cancel";
            this.frm3GPPConfigCancelBtn.UseVisualStyleBackColor = true;
            this.frm3GPPConfigCancelBtn.Click += new EventHandler(this.frm3GPPConfigCancelBtn_Click);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x233, 0x31);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x15, 13);
            this.label11.TabIndex = 0x10;
            this.label11.Text = "(m)";
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x233, 0x47);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x15, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "(m)";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x233, 0x5d);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x15, 13);
            this.label13.TabIndex = 0x2c;
            this.label13.Text = "(m)";
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x233, 0x73);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x15, 13);
            this.label14.TabIndex = 0x3a;
            this.label14.Text = "(m)";
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x233, 0x89);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x15, 13);
            this.label15.TabIndex = 0x48;
            this.label15.Text = "(m)";
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x288, 0x31);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x15, 13);
            this.label16.TabIndex = 0x12;
            this.label16.Text = "(m)";
            this.label17.AutoSize = true;
            this.label17.Location = new Point(0x288, 0x47);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x15, 13);
            this.label17.TabIndex = 0x20;
            this.label17.Text = "(m)";
            this.label18.AutoSize = true;
            this.label18.Location = new Point(0x288, 0x5d);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x15, 13);
            this.label18.TabIndex = 0x2e;
            this.label18.Text = "(m)";
            this.label19.AutoSize = true;
            this.label19.Location = new Point(0x288, 0x73);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x15, 13);
            this.label19.TabIndex = 60;
            this.label19.Text = "(m)";
            this.label20.AutoSize = true;
            this.label20.Location = new Point(0x288, 0x89);
            this.label20.Name = "label20";
            this.label20.Size = new Size(0x15, 13);
            this.label20.TabIndex = 0x4a;
            this.label20.Text = "(m)";
            this.label21.AutoSize = true;
            this.label21.Location = new Point(0x2d6, 0x31);
            this.label21.Name = "label21";
            this.label21.Size = new Size(30, 13);
            this.label21.TabIndex = 20;
            this.label21.Text = "(sec)";
            this.label22.AutoSize = true;
            this.label22.Location = new Point(0x2d6, 0x47);
            this.label22.Name = "label22";
            this.label22.Size = new Size(30, 13);
            this.label22.TabIndex = 0x22;
            this.label22.Text = "(sec)";
            this.label23.AutoSize = true;
            this.label23.Location = new Point(0x2d6, 0x5d);
            this.label23.Name = "label23";
            this.label23.Size = new Size(30, 13);
            this.label23.TabIndex = 0x30;
            this.label23.Text = "(sec)";
            this.label24.AutoSize = true;
            this.label24.Location = new Point(0x2d6, 0x73);
            this.label24.Name = "label24";
            this.label24.Size = new Size(30, 13);
            this.label24.TabIndex = 0x3e;
            this.label24.Text = "(sec)";
            this.label25.AutoSize = true;
            this.label25.Location = new Point(0x2d6, 0x89);
            this.label25.Name = "label25";
            this.label25.Size = new Size(30, 13);
            this.label25.TabIndex = 0x4c;
            this.label25.Text = "(sec)";
            this.frm3GPPConfigTest1TimeLabel.AutoSize = true;
            this.frm3GPPConfigTest1TimeLabel.Location = new Point(0x35b, 0x31);
            this.frm3GPPConfigTest1TimeLabel.Name = "frm3GPPConfigTest1TimeLabel";
            this.frm3GPPConfigTest1TimeLabel.Size = new Size(0x10, 13);
            this.frm3GPPConfigTest1TimeLabel.TabIndex = 0x16;
            this.frm3GPPConfigTest1TimeLabel.Text = "---";
            this.frm3GPPConfigTest2TimeLabel.AutoSize = true;
            this.frm3GPPConfigTest2TimeLabel.Location = new Point(0x35b, 0x47);
            this.frm3GPPConfigTest2TimeLabel.Name = "frm3GPPConfigTest2TimeLabel";
            this.frm3GPPConfigTest2TimeLabel.Size = new Size(0x10, 13);
            this.frm3GPPConfigTest2TimeLabel.TabIndex = 0x24;
            this.frm3GPPConfigTest2TimeLabel.Text = "---";
            this.frm3GPPConfigTest3TimeLabel.AutoSize = true;
            this.frm3GPPConfigTest3TimeLabel.Location = new Point(0x35b, 0x5d);
            this.frm3GPPConfigTest3TimeLabel.Name = "frm3GPPConfigTest3TimeLabel";
            this.frm3GPPConfigTest3TimeLabel.Size = new Size(0x10, 13);
            this.frm3GPPConfigTest3TimeLabel.TabIndex = 50;
            this.frm3GPPConfigTest3TimeLabel.Text = "---";
            this.frm3GPPConfigTest4TimeLabel.AutoSize = true;
            this.frm3GPPConfigTest4TimeLabel.Location = new Point(0x35b, 0x73);
            this.frm3GPPConfigTest4TimeLabel.Name = "frm3GPPConfigTest4TimeLabel";
            this.frm3GPPConfigTest4TimeLabel.Size = new Size(0x10, 13);
            this.frm3GPPConfigTest4TimeLabel.TabIndex = 0x40;
            this.frm3GPPConfigTest4TimeLabel.Text = "---";
            this.frm3GPPConfigTest5TimeLabel.AutoSize = true;
            this.frm3GPPConfigTest5TimeLabel.Location = new Point(0x35b, 0x89);
            this.frm3GPPConfigTest5TimeLabel.Name = "frm3GPPConfigTest5TimeLabel";
            this.frm3GPPConfigTest5TimeLabel.Size = new Size(0x10, 13);
            this.frm3GPPConfigTest5TimeLabel.TabIndex = 0x4e;
            this.frm3GPPConfigTest5TimeLabel.Text = "---";
            this.frm3GPPConfigTotalLabel.AutoSize = true;
            this.frm3GPPConfigTotalLabel.Location = new Point(0x301, 0xab);
            this.frm3GPPConfigTotalLabel.Name = "frm3GPPConfigTotalLabel";
            this.frm3GPPConfigTotalLabel.Size = new Size(0x39, 13);
            this.frm3GPPConfigTotalLabel.TabIndex = 0x4f;
            this.frm3GPPConfigTotalLabel.Text = "Total Time";
            this.frm3GPPConfigTotalTimeLabel.AutoSize = true;
            this.frm3GPPConfigTotalTimeLabel.Location = new Point(0x35b, 0xab);
            this.frm3GPPConfigTotalTimeLabel.Name = "frm3GPPConfigTotalTimeLabel";
            this.frm3GPPConfigTotalTimeLabel.Size = new Size(0x10, 13);
            this.frm3GPPConfigTotalTimeLabel.TabIndex = 80;
            this.frm3GPPConfigTotalTimeLabel.Text = "---";
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.label31);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.label30);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.label29);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.label28);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAssist2MarginLabel);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAssist1MarginLabel);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsBasedMarginLabel);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAB_A2ChkBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAB_A1ChkBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAB_BChkBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAssist2ChkBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAssist1ChkBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsBasedChkBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAB_A2MarginTxtBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAB_A1MarginTxtBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAB_BMarginTxtBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAssist2MarginTxtBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsAssist1MarginTxtBox);
            this.frm3GPPConfigAidingTypeGrpBox.Controls.Add(this.frm3GPPConfigMsBasedMarginTxtBox);
            this.frm3GPPConfigAidingTypeGrpBox.Location = new Point(0x15, 0xd7);
            this.frm3GPPConfigAidingTypeGrpBox.Name = "frm3GPPConfigAidingTypeGrpBox";
            this.frm3GPPConfigAidingTypeGrpBox.Size = new Size(0x16c, 0xb3);
            this.frm3GPPConfigAidingTypeGrpBox.TabIndex = 40;
            this.frm3GPPConfigAidingTypeGrpBox.TabStop = false;
            this.frm3GPPConfigAidingTypeGrpBox.Text = "Aiding Type";
            this.label31.AutoSize = true;
            this.label31.Location = new Point(0x128, 0x21);
            this.label31.Name = "label31";
            this.label31.Size = new Size(20, 13);
            this.label31.TabIndex = 0x5b;
            this.label31.Text = "dB";
            this.label30.AutoSize = true;
            this.label30.Location = new Point(0x128, 0x99);
            this.label30.Name = "label30";
            this.label30.Size = new Size(20, 13);
            this.label30.TabIndex = 90;
            this.label30.Text = "dB";
            this.label29.AutoSize = true;
            this.label29.Location = new Point(0x128, 0x81);
            this.label29.Name = "label29";
            this.label29.Size = new Size(20, 13);
            this.label29.TabIndex = 90;
            this.label29.Text = "dB";
            this.label28.AutoSize = true;
            this.label28.Location = new Point(0x128, 0x69);
            this.label28.Name = "label28";
            this.label28.Size = new Size(20, 13);
            this.label28.TabIndex = 90;
            this.label28.Text = "dB";
            this.frm3GPPConfigMsAssist2MarginLabel.AutoSize = true;
            this.frm3GPPConfigMsAssist2MarginLabel.Location = new Point(0x128, 0x51);
            this.frm3GPPConfigMsAssist2MarginLabel.Name = "frm3GPPConfigMsAssist2MarginLabel";
            this.frm3GPPConfigMsAssist2MarginLabel.Size = new Size(20, 13);
            this.frm3GPPConfigMsAssist2MarginLabel.TabIndex = 90;
            this.frm3GPPConfigMsAssist2MarginLabel.Text = "dB";
            this.frm3GPPConfigMsAssist1MarginLabel.AutoSize = true;
            this.frm3GPPConfigMsAssist1MarginLabel.Location = new Point(0x128, 0x39);
            this.frm3GPPConfigMsAssist1MarginLabel.Name = "frm3GPPConfigMsAssist1MarginLabel";
            this.frm3GPPConfigMsAssist1MarginLabel.Size = new Size(20, 13);
            this.frm3GPPConfigMsAssist1MarginLabel.TabIndex = 0x57;
            this.frm3GPPConfigMsAssist1MarginLabel.Text = "dB";
            this.frm3GPPConfigMsBasedMarginLabel.AutoSize = true;
            this.frm3GPPConfigMsBasedMarginLabel.Location = new Point(0xe2, 12);
            this.frm3GPPConfigMsBasedMarginLabel.Name = "frm3GPPConfigMsBasedMarginLabel";
            this.frm3GPPConfigMsBasedMarginLabel.Size = new Size(0x27, 13);
            this.frm3GPPConfigMsBasedMarginLabel.TabIndex = 0x54;
            this.frm3GPPConfigMsBasedMarginLabel.Text = "Margin";
            this.frm3GPPConfigMsAB_A2ChkBox.AutoSize = true;
            this.frm3GPPConfigMsAB_A2ChkBox.Location = new Point(0x15, 0x97);
            this.frm3GPPConfigMsAB_A2ChkBox.Name = "frm3GPPConfigMsAB_A2ChkBox";
            this.frm3GPPConfigMsAB_A2ChkBox.Size = new Size(0x91, 0x11);
            this.frm3GPPConfigMsAB_A2ChkBox.TabIndex = 2;
            this.frm3GPPConfigMsAB_A2ChkBox.Text = "MS-AB with MSA2 Aiding";
            this.frm3GPPConfigMsAB_A2ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigMsAB_A2ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigMsAB_A1ChkBox.AutoSize = true;
            this.frm3GPPConfigMsAB_A1ChkBox.Location = new Point(0x15, 0x7f);
            this.frm3GPPConfigMsAB_A1ChkBox.Name = "frm3GPPConfigMsAB_A1ChkBox";
            this.frm3GPPConfigMsAB_A1ChkBox.Size = new Size(0x91, 0x11);
            this.frm3GPPConfigMsAB_A1ChkBox.TabIndex = 2;
            this.frm3GPPConfigMsAB_A1ChkBox.Text = "MS-AB with MSA1 Aiding";
            this.frm3GPPConfigMsAB_A1ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigMsAB_A1ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigMsAB_BChkBox.AutoSize = true;
            this.frm3GPPConfigMsAB_BChkBox.Location = new Point(0x15, 0x67);
            this.frm3GPPConfigMsAB_BChkBox.Name = "frm3GPPConfigMsAB_BChkBox";
            this.frm3GPPConfigMsAB_BChkBox.Size = new Size(0x8b, 0x11);
            this.frm3GPPConfigMsAB_BChkBox.TabIndex = 2;
            this.frm3GPPConfigMsAB_BChkBox.Text = "MS-AB with MSB Aiding";
            this.frm3GPPConfigMsAB_BChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigMsAB_BChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigMsAssist2ChkBox.AutoSize = true;
            this.frm3GPPConfigMsAssist2ChkBox.Location = new Point(0x15, 0x4f);
            this.frm3GPPConfigMsAssist2ChkBox.Name = "frm3GPPConfigMsAssist2ChkBox";
            this.frm3GPPConfigMsAssist2ChkBox.Size = new Size(0x4e, 0x11);
            this.frm3GPPConfigMsAssist2ChkBox.TabIndex = 2;
            this.frm3GPPConfigMsAssist2ChkBox.Text = "MS-Assist2";
            this.frm3GPPConfigMsAssist2ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigMsAssist2ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigMsAssist1ChkBox.AutoSize = true;
            this.frm3GPPConfigMsAssist1ChkBox.Location = new Point(0x15, 0x37);
            this.frm3GPPConfigMsAssist1ChkBox.Name = "frm3GPPConfigMsAssist1ChkBox";
            this.frm3GPPConfigMsAssist1ChkBox.Size = new Size(0x4e, 0x11);
            this.frm3GPPConfigMsAssist1ChkBox.TabIndex = 1;
            this.frm3GPPConfigMsAssist1ChkBox.Text = "MS-Assist1";
            this.frm3GPPConfigMsAssist1ChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigMsAssist1ChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigMsBasedChkBox.AutoSize = true;
            this.frm3GPPConfigMsBasedChkBox.Location = new Point(0x15, 0x1f);
            this.frm3GPPConfigMsBasedChkBox.Name = "frm3GPPConfigMsBasedChkBox";
            this.frm3GPPConfigMsBasedChkBox.Size = new Size(0x4b, 0x11);
            this.frm3GPPConfigMsBasedChkBox.TabIndex = 0;
            this.frm3GPPConfigMsBasedChkBox.Text = "MS-Based";
            this.frm3GPPConfigMsBasedChkBox.UseVisualStyleBackColor = true;
            this.frm3GPPConfigMsBasedChkBox.CheckedChanged += new EventHandler(this.frm3GPPConfigTest1ChkBox_CheckedChanged);
            this.frm3GPPConfigMsAB_A2MarginTxtBox.Location = new Point(0xc2, 0x95);
            this.frm3GPPConfigMsAB_A2MarginTxtBox.Name = "frm3GPPConfigMsAB_A2MarginTxtBox";
            this.frm3GPPConfigMsAB_A2MarginTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigMsAB_A2MarginTxtBox.TabIndex = 5;
            this.frm3GPPConfigMsAB_A2MarginTxtBox.Text = "0";
            this.frm3GPPConfigMsAB_A1MarginTxtBox.Location = new Point(0xc2, 0x7d);
            this.frm3GPPConfigMsAB_A1MarginTxtBox.Name = "frm3GPPConfigMsAB_A1MarginTxtBox";
            this.frm3GPPConfigMsAB_A1MarginTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigMsAB_A1MarginTxtBox.TabIndex = 5;
            this.frm3GPPConfigMsAB_A1MarginTxtBox.Text = "0";
            this.frm3GPPConfigMsAB_BMarginTxtBox.Location = new Point(0xc2, 0x65);
            this.frm3GPPConfigMsAB_BMarginTxtBox.Name = "frm3GPPConfigMsAB_BMarginTxtBox";
            this.frm3GPPConfigMsAB_BMarginTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigMsAB_BMarginTxtBox.TabIndex = 5;
            this.frm3GPPConfigMsAB_BMarginTxtBox.Text = "0";
            this.frm3GPPConfigMsAssist2MarginTxtBox.Location = new Point(0xc2, 0x4d);
            this.frm3GPPConfigMsAssist2MarginTxtBox.Name = "frm3GPPConfigMsAssist2MarginTxtBox";
            this.frm3GPPConfigMsAssist2MarginTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigMsAssist2MarginTxtBox.TabIndex = 5;
            this.frm3GPPConfigMsAssist2MarginTxtBox.Text = "0";
            this.frm3GPPConfigMsAssist1MarginTxtBox.Location = new Point(0xc2, 0x35);
            this.frm3GPPConfigMsAssist1MarginTxtBox.Name = "frm3GPPConfigMsAssist1MarginTxtBox";
            this.frm3GPPConfigMsAssist1MarginTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigMsAssist1MarginTxtBox.TabIndex = 4;
            this.frm3GPPConfigMsAssist1MarginTxtBox.Text = "0";
            this.frm3GPPConfigMsBasedMarginTxtBox.Location = new Point(0xc2, 0x1d);
            this.frm3GPPConfigMsBasedMarginTxtBox.Name = "frm3GPPConfigMsBasedMarginTxtBox";
            this.frm3GPPConfigMsBasedMarginTxtBox.Size = new Size(100, 20);
            this.frm3GPPConfigMsBasedMarginTxtBox.TabIndex = 3;
            this.frm3GPPConfigMsBasedMarginTxtBox.Text = "0";
            this.frmEditConfigurationAutoReplyBtn.Location = new Point(690, 0xec);
            this.frmEditConfigurationAutoReplyBtn.Name = "frmEditConfigurationAutoReplyBtn";
            this.frmEditConfigurationAutoReplyBtn.Size = new Size(0x7f, 0x17);
            this.frmEditConfigurationAutoReplyBtn.TabIndex = 0x2c;
            this.frmEditConfigurationAutoReplyBtn.Text = "Configure &AutoReply";
            this.frmEditConfigurationAutoReplyBtn.UseVisualStyleBackColor = true;
            this.frmEditConfigurationAutoReplyBtn.Click += new EventHandler(this.frmEditConfigurationAutoReplyBtn_Click);
            this.label26.AutoSize = true;
            this.label26.Location = new Point(0x12, 0xbf);
            this.label26.Name = "label26";
            this.label26.Size = new Size(0x185, 13);
            this.label26.TabIndex = 0x65;
            this.label26.Text = "If margin is negative, signal will be adjusted with the value in the Signal Atten field";
            this.frm3GPPConfigTest1MaxErrorTxtBox.Enabled = false;
            this.frm3GPPConfigTest1MaxErrorTxtBox.Location = new Point(0x11c, 0x2d);
            this.frm3GPPConfigTest1MaxErrorTxtBox.Name = "frm3GPPConfigTest1MaxErrorTxtBox";
            this.frm3GPPConfigTest1MaxErrorTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest1MaxErrorTxtBox.TabIndex = 0x66;
            this.frm3GPPConfigTest1MaxErrorTxtBox.Text = "2765";
            this.frm3GPPConfigTest2MaxErrorTxtBox.Enabled = false;
            this.frm3GPPConfigTest2MaxErrorTxtBox.Location = new Point(0x11c, 0x43);
            this.frm3GPPConfigTest2MaxErrorTxtBox.Name = "frm3GPPConfigTest2MaxErrorTxtBox";
            this.frm3GPPConfigTest2MaxErrorTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest2MaxErrorTxtBox.TabIndex = 0x67;
            this.frm3GPPConfigTest2MaxErrorTxtBox.Text = "2765";
            this.frm3GPPConfigTest3MaxErrorTxtBox.Enabled = false;
            this.frm3GPPConfigTest3MaxErrorTxtBox.Location = new Point(0x11c, 0x59);
            this.frm3GPPConfigTest3MaxErrorTxtBox.Name = "frm3GPPConfigTest3MaxErrorTxtBox";
            this.frm3GPPConfigTest3MaxErrorTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest3MaxErrorTxtBox.TabIndex = 0x68;
            this.frm3GPPConfigTest3MaxErrorTxtBox.Text = "2765";
            this.frm3GPPConfigTest4MaxErrorTxtBox.Enabled = false;
            this.frm3GPPConfigTest4MaxErrorTxtBox.Location = new Point(0x11c, 0x6f);
            this.frm3GPPConfigTest4MaxErrorTxtBox.Name = "frm3GPPConfigTest4MaxErrorTxtBox";
            this.frm3GPPConfigTest4MaxErrorTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest4MaxErrorTxtBox.TabIndex = 0x69;
            this.frm3GPPConfigTest4MaxErrorTxtBox.Text = "2765";
            this.frm3GPPConfigTest5MaxErrorTxtBox.Enabled = false;
            this.frm3GPPConfigTest5MaxErrorTxtBox.Location = new Point(0x11c, 0x85);
            this.frm3GPPConfigTest5MaxErrorTxtBox.Name = "frm3GPPConfigTest5MaxErrorTxtBox";
            this.frm3GPPConfigTest5MaxErrorTxtBox.Size = new Size(0x25, 20);
            this.frm3GPPConfigTest5MaxErrorTxtBox.TabIndex = 0x6a;
            this.frm3GPPConfigTest5MaxErrorTxtBox.Text = "2765";
            this.label27.AutoSize = true;
            this.label27.Location = new Point(0x10a, 0x19);
            this.label27.Name = "label27";
            this.label27.Size = new Size(0x48, 13);
            this.label27.TabIndex = 0x6b;
            this.label27.Text = "Max # Misses";
            this.frmEditConfigurationLoadDefautlBtn.Location = new Point(690, 0x10b);
            this.frmEditConfigurationLoadDefautlBtn.Name = "frmEditConfigurationLoadDefautlBtn";
            this.frmEditConfigurationLoadDefautlBtn.Size = new Size(0x7f, 0x17);
            this.frmEditConfigurationLoadDefautlBtn.TabIndex = 0x6c;
            this.frmEditConfigurationLoadDefautlBtn.Text = "Load Default";
            this.frmEditConfigurationLoadDefautlBtn.UseVisualStyleBackColor = true;
            this.frmEditConfigurationLoadDefautlBtn.Click += new EventHandler(this.frmEditConfigurationLoadDefautlBtn_Click);
            base.AcceptButton = this.frm3GPPConfigOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.frm3GPPConfigCancelBtn;
            base.ClientSize = new Size(0x3b2, 0x1a9);
            base.Controls.Add(this.frmEditConfigurationLoadDefautlBtn);
            base.Controls.Add(this.label27);
            base.Controls.Add(this.frm3GPPConfigTest5MaxErrorTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4MaxErrorTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3MaxErrorTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2MaxErrorTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1MaxErrorTxtBox);
            base.Controls.Add(this.label26);
            base.Controls.Add(this.frmEditConfigurationAutoReplyBtn);
            base.Controls.Add(this.frm3GPPConfigAidingTypeGrpBox);
            base.Controls.Add(this.frm3GPPConfigTotalLabel);
            base.Controls.Add(this.frm3GPPConfigTotalTimeLabel);
            base.Controls.Add(this.frm3GPPConfigTest5TimeLabel);
            base.Controls.Add(this.frm3GPPConfigTest4TimeLabel);
            base.Controls.Add(this.frm3GPPConfigTest3TimeLabel);
            base.Controls.Add(this.frm3GPPConfigTest2TimeLabel);
            base.Controls.Add(this.frm3GPPConfigTest1TimeLabel);
            base.Controls.Add(this.frm3GPPConfigCancelBtn);
            base.Controls.Add(this.frm3GPPConfigOkBtn);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label15);
            base.Controls.Add(this.label14);
            base.Controls.Add(this.label13);
            base.Controls.Add(this.label12);
            base.Controls.Add(this.label20);
            base.Controls.Add(this.label19);
            base.Controls.Add(this.label18);
            base.Controls.Add(this.label17);
            base.Controls.Add(this.label25);
            base.Controls.Add(this.label24);
            base.Controls.Add(this.label23);
            base.Controls.Add(this.label22);
            base.Controls.Add(this.label21);
            base.Controls.Add(this.label16);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.frm3GPPConfigProfileSettingsGrpBox);
            base.Controls.Add(this.frm3GPPConfigFreqTransferModeGrpBox);
            base.Controls.Add(this.frm3GPPConfigTest5SignalAttnTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4SignalAttnTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3SignalAttnTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2SignalAttnTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest5PriorityTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4PriorityTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest5RespMaxTimeTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4RespMaxTimeTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest5VrtQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4VrtQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3PriorityTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest5HrzQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3RespMaxTimeTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4HrzQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3VrtQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2PriorityTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest5RelFreqAccTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2RespMaxTimeTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3HrzQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2VrtQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1PriorityTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest4RelFreqAccTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1RespMaxTimeTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2HrzQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1VrtQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest3RelFreqAccTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1HrzQoSTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest2RelFreqAccTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1RelFreqAccTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest1SignalAttnTxtBox);
            base.Controls.Add(this.frm3GPPConfigTest5CyclesComboBox);
            base.Controls.Add(this.frm3GPPConfigTest4CyclesComboBox);
            base.Controls.Add(this.frm3GPPConfigTest3CyclesComboBox);
            base.Controls.Add(this.frm3GPPConfigTest2CyclesComboBox);
            base.Controls.Add(this.frm3GPPConfigTest1CyclesComboBox);
            base.Controls.Add(this.frm3GPPConfigTest5ChkBox);
            base.Controls.Add(this.frm3GPPConfigTest4ChkBox);
            base.Controls.Add(this.frm3GPPConfigTest3ChkBox);
            base.Controls.Add(this.frm3GPPConfigTest2ChkBox);
            base.Controls.Add(this.frm3GPPConfigTest1ChkBox);
            base.Controls.Add(this.frm3GPPConfigEstTimeLabel);
            base.Controls.Add(this.frm3GPPConfigPriorityLabel);
            base.Controls.Add(this.frm3GPPConfigRespTimeMaxLabel);
            base.Controls.Add(this.frm3GPPConfigVrtQoSLabel);
            base.Controls.Add(this.frm3GPPConfigHrzQoSLabel);
            base.Controls.Add(this.frm3GPPConfigRelFreqAccLabel);
            base.Controls.Add(this.frm3GPPConfigSignalAttnLabel);
            base.Controls.Add(this.frm3GPPConfigNumberCyclesLabel);
            base.Controls.Add(this.frm3GPPConfigTestSelectionLabel);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frm3GPPConfig";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "3GPP Test Setup";
            base.Load += new EventHandler(this.frm3GPPConfig_Load);
            this.frm3GPPConfigFreqTransferModeGrpBox.ResumeLayout(false);
            this.frm3GPPConfigFreqTransferModeGrpBox.PerformLayout();
            this.frm3GPPConfigProfileSettingsGrpBox.ResumeLayout(false);
            this.frm3GPPConfigProfileSettingsGrpBox.PerformLayout();
            this.frm3GPPConfigAidingTypeGrpBox.ResumeLayout(false);
            this.frm3GPPConfigAidingTypeGrpBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void loadConfigFile(string configFilePath)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!File.Exists(configFilePath))
            {
                MessageBox.Show("Configuration File does not exist", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Cursor = Cursors.Default;
            }
            else
            {
                IniHelper helper = new IniHelper(configFilePath);
                List<string> sections = helper.GetSections();
                string key = string.Empty;
                string str2 = string.Empty;
                List<string> list2 = new List<string>();
                char[] trimChars = new char[] { '\n', '\r', '\t', '\0' };
                int num = 0;
                int num2 = 4;
                foreach (string str3 in sections)
                {
                    int num3;
                    string str4 = str3;
                    if (str4 != null)
                    {
                        if (!(str4 == "TESTS"))
                        {
                            if (str4 == "TEST_SETUP")
                            {
                                goto Label_01C0;
                            }
                            if (str4 == "AIDING_PARAMS")
                            {
                                goto Label_062C;
                            }
                        }
                        else
                        {
                            key = "SENSITIVITY_TEST1";
                            str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                            this.frm3GPPConfigTest1ChkBox.Checked = str2 == "1";
                            key = "NOMINAL_ACCURACY_TEST2";
                            str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                            this.frm3GPPConfigTest2ChkBox.Checked = str2 == "1";
                            key = "DYNAMIC_RANGE_TEST3";
                            str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                            this.frm3GPPConfigTest3ChkBox.Checked = str2 == "1";
                            key = "MULTIPATH_TEST4";
                            str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                            this.frm3GPPConfigTest4ChkBox.Checked = str2 == "1";
                            key = "MOVING_SCENARIO_TEST5";
                            str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                            this.frm3GPPConfigTest5ChkBox.Checked = str2 == "1";
                        }
                    }
                    continue;
                Label_01C0:
                    num3 = 1;
                    while (num3 <= 5)
                    {
                        key = "CYCLES_TEST" + num3.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                        num3++;
                    }
                    if (!this.frm3GPPConfigTest1CyclesComboBox.Items.Contains(list2[num]))
                    {
                        this.frm3GPPConfigTest1CyclesComboBox.Items.Add(list2[num]);
                    }
                    this.frm3GPPConfigTest1CyclesComboBox.Text = list2[num++];
                    if (!this.frm3GPPConfigTest2CyclesComboBox.Items.Contains(list2[num]))
                    {
                        this.frm3GPPConfigTest2CyclesComboBox.Items.Add(list2[num]);
                    }
                    this.frm3GPPConfigTest2CyclesComboBox.Text = list2[num++];
                    if (!this.frm3GPPConfigTest3CyclesComboBox.Items.Contains(list2[num]))
                    {
                        this.frm3GPPConfigTest3CyclesComboBox.Items.Add(list2[num]);
                    }
                    this.frm3GPPConfigTest3CyclesComboBox.Text = list2[num++];
                    if (!this.frm3GPPConfigTest4CyclesComboBox.Items.Contains(list2[num]))
                    {
                        this.frm3GPPConfigTest4CyclesComboBox.Items.Add(list2[num]);
                    }
                    this.frm3GPPConfigTest4CyclesComboBox.Text = list2[num++];
                    num = 0;
                    list2.Clear();
                    for (int i = 1; i <= num2; i++)
                    {
                        key = "SIGNAL_ATTN_TEST" + i.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                    }
                    this.frm3GPPConfigTest1SignalAttnTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest2SignalAttnTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest3SignalAttnTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest4SignalAttnTxtBox.Text = list2[num++];
                    key = "MS_BASED_TEST";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsBasedChkBox.Checked = str2 == "1";
                    key = "MS_ASSIST1_TEST";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAssist1ChkBox.Checked = str2 == "1";
                    key = "MS_ASSIST2_TEST";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAssist2ChkBox.Checked = str2 == "1";
                    key = "MSAB_B_TEST";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAB_BChkBox.Checked = str2 == "1";
                    key = "MSAB_A1_TEST";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAB_A1ChkBox.Checked = str2 == "1";
                    key = "MSAB_A2_TEST";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAB_A2ChkBox.Checked = str2 == "1";
                    key = "CABLE_LOSS";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigCableLossTxtBox.Text = str2;
                    key = "MS_BASED_MARGIN";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsBasedMarginTxtBox.Text = str2;
                    key = "MS_ASSIST1_MARGIN";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAssist1MarginTxtBox.Text = str2;
                    key = "MS_ASSIST2_MARGIN";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAssist2MarginTxtBox.Text = str2;
                    key = "MSAB_B_MARGIN";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAB_BMarginTxtBox.Text = str2;
                    key = "MSAB_A1_MARGIN";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAB_A1MarginTxtBox.Text = str2;
                    key = "MSAB_A2_MARGIN";
                    str2 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    this.frm3GPPConfigMsAB_A2MarginTxtBox.Text = str2;
                    continue;
                Label_062C:
                    key = "FREQ_TRANFER_METHOD";
                    string str5 = helper.GetIniFileString(str3, key, "").TrimEnd(trimChars);
                    if (str5 != null)
                    {
                        if (!(str5 == "Counter"))
                        {
                            if (str5 == "NonCounter")
                            {
                                goto Label_0688;
                            }
                            if (str5 == "NoFreqReq")
                            {
                                goto Label_0696;
                            }
                        }
                        else
                        {
                            this.frm3GPPConfigCounterRadioBtn.Checked = true;
                        }
                    }
                    goto Label_06A2;
                Label_0688:
                    this.frm3GPPConfigNonCounterRadioBtn.Checked = true;
                    goto Label_06A2;
                Label_0696:
                    this.frm3GPPConfigNoFreqReqRadioBtn.Checked = true;
                Label_06A2:
                    num = 0;
                    list2.Clear();
                    for (int j = 1; j <= num2; j++)
                    {
                        key = "REL_FREQ_ACC_TEST" + j.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                    }
                    this.frm3GPPConfigTest1RelFreqAccTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest2RelFreqAccTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest3RelFreqAccTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest4RelFreqAccTxtBox.Text = list2[num++];
                    num = 0;
                    list2.Clear();
                    for (int k = 1; k <= num2; k++)
                    {
                        key = "HRZ_QOS_TEST" + k.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                    }
                    this.frm3GPPConfigTest1HrzQoSTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest2HrzQoSTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest3HrzQoSTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest4HrzQoSTxtBox.Text = list2[num++];
                    num = 0;
                    list2.Clear();
                    for (int m = 1; m <= num2; m++)
                    {
                        key = "VRT_QOS_TEST" + m.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                    }
                    this.frm3GPPConfigTest1VrtQoSTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest2VrtQoSTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest3VrtQoSTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest4VrtQoSTxtBox.Text = list2[num++];
                    num = 0;
                    list2.Clear();
                    for (int n = 1; n <= num2; n++)
                    {
                        key = "RESPONSE_TIME_MAX_TEST" + n.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                    }
                    this.frm3GPPConfigTest1RespMaxTimeTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest2RespMaxTimeTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest3RespMaxTimeTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest4RespMaxTimeTxtBox.Text = list2[num++];
                    num = 0;
                    list2.Clear();
                    for (int num9 = 1; num9 <= num2; num9++)
                    {
                        key = "PRIORITY_TEST" + num9.ToString();
                        list2.Add(helper.GetIniFileString(str3, key, "").TrimEnd(trimChars));
                    }
                    this.frm3GPPConfigTest1PriorityTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest2PriorityTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest3PriorityTxtBox.Text = list2[num++];
                    this.frm3GPPConfigTest4PriorityTxtBox.Text = list2[num++];
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void loadDefaultCycles()
        {
            string[] items = new string[] { "77", "106", "131", "154", "176", "197", "218", "238", "257", "277", "USER_DEFINED" };
            this.frm3GPPConfigTest1CyclesComboBox.Items.AddRange(items);
            this.frm3GPPConfigTest2CyclesComboBox.Items.AddRange(items);
            this.frm3GPPConfigTest3CyclesComboBox.Items.AddRange(items);
            this.frm3GPPConfigTest4CyclesComboBox.Items.AddRange(items);
            this.frm3GPPConfigTest5CyclesComboBox.Items.AddRange(items);
            this.frm3GPPConfigTest5CyclesComboBox.Enabled = false;
        }

        private int saveToConfigFile()
        {
            this.Cursor = Cursors.WaitCursor;
            IniHelper helper = new IniHelper(this._configFilePath);
            if (File.Exists(this._configFilePath))
            {
                if ((File.GetAttributes(this._configFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("Config File is read only!\nPlease change property and retry\n\n{0}", this._configFilePath), "Error");
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                string section = "TESTS";
                string key = "SENSITIVITY_TEST1";
                int num = 1;
                int num2 = 0;
                if (this.frm3GPPConfigTest1ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "NOMINAL_ACCURACY_TEST2";
                if (this.frm3GPPConfigTest2ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "DYNAMIC_RANGE_TEST3";
                if (this.frm3GPPConfigTest3ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MULTIPATH_TEST4";
                if (this.frm3GPPConfigTest4ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MOVING_SCENARIO_TEST5";
                if (this.frm3GPPConfigTest5ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                section = "TEST_SETUP";
                key = "CYCLES_TEST" + num.ToString();
                try
                {
                    num2 = Convert.ToInt32(this.frm3GPPConfigTest1CyclesComboBox.Text.ToString());
                    helper.IniWriteValue(section, key, num2.ToString());
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Test " + num.ToString() + "# Cycles: " + exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "CYCLES_TEST" + num.ToString();
                try
                {
                    num2 = Convert.ToInt32(this.frm3GPPConfigTest2CyclesComboBox.Text.ToString());
                    helper.IniWriteValue(section, key, num2.ToString());
                }
                catch (Exception exception2)
                {
                    MessageBox.Show("Test " + num.ToString() + "# Cycles: " + exception2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "CYCLES_TEST" + num.ToString();
                try
                {
                    num2 = Convert.ToInt32(this.frm3GPPConfigTest3CyclesComboBox.Text.ToString());
                    helper.IniWriteValue(section, key, num2.ToString());
                }
                catch (Exception exception3)
                {
                    MessageBox.Show("Test " + num.ToString() + "# Cycles: " + exception3.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "CYCLES_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, Convert.ToInt32(this.frm3GPPConfigTest4CyclesComboBox.Text.ToString()).ToString());
                }
                catch (Exception exception4)
                {
                    MessageBox.Show("Test " + num.ToString() + "# Cycles: " + exception4.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                key = "MS_BASED_TEST";
                if (this.frm3GPPConfigMsBasedChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MS_ASSIST1_TEST";
                if (this.frm3GPPConfigMsAssist1ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MS_ASSIST2_TEST";
                if (this.frm3GPPConfigMsAssist2ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MSAB_B_TEST";
                if (this.frm3GPPConfigMsAB_BChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MSAB_A1_TEST";
                if (this.frm3GPPConfigMsAB_A1ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                key = "MSAB_A2_TEST";
                if (this.frm3GPPConfigMsAB_A2ChkBox.Checked)
                {
                    helper.IniWriteValue(section, key, "1");
                }
                else
                {
                    helper.IniWriteValue(section, key, "0");
                }
                List<string> list = new List<string>();
                list.Add(this.frm3GPPConfigMsBasedMarginTxtBox.Text);
                list.Add(this.frm3GPPConfigMsAssist1MarginTxtBox.Text);
                list.Add(this.frm3GPPConfigMsAssist2MarginTxtBox.Text);
                list.Add(this.frm3GPPConfigMsAB_BMarginTxtBox.Text);
                list.Add(this.frm3GPPConfigMsAB_A1MarginTxtBox.Text);
                list.Add(this.frm3GPPConfigMsAB_A2MarginTxtBox.Text);
                foreach (string str4 in list)
                {
                    string[] strArray = str4.Split(new char[] { ',' });
                    double num3 = 0.0;
                    try
                    {
                        if (strArray.Length > 1)
                        {
                            foreach (string str5 in strArray)
                            {
                                num3 = Convert.ToDouble(str5);
                                if ((num3 > 9.9) || (num3 < -9.9))
                                {
                                    MessageBox.Show("Margin: " + num3.ToString() + " is not valid (range: -9.9 to 9.9)", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    this.Cursor = Cursors.Default;
                                    return 1;
                                }
                            }
                        }
                        else
                        {
                            num3 = Convert.ToDouble(strArray[0]);
                            if ((num3 > 9.9) || (num3 < -9.9))
                            {
                                MessageBox.Show("Margin: " + num3.ToString() + " is not valid (range: -9.9 to 9.9)", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                this.Cursor = Cursors.Default;
                                return 1;
                            }
                        }
                    }
                    catch (Exception exception5)
                    {
                        MessageBox.Show("Margin: " + exception5.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.Cursor = Cursors.Default;
                        return 1;
                    }
                }
                key = "MS_BASED_MARGIN";
                helper.IniWriteValue(section, key, this.frm3GPPConfigMsBasedMarginTxtBox.Text);
                key = "MS_ASSIST1_MARGIN";
                helper.IniWriteValue(section, key, this.frm3GPPConfigMsAssist1MarginTxtBox.Text);
                key = "MS_ASSIST2_MARGIN";
                helper.IniWriteValue(section, key, this.frm3GPPConfigMsAssist2MarginTxtBox.Text);
                key = "MSAB_B_MARGIN";
                helper.IniWriteValue(section, key, this.frm3GPPConfigMsAB_BMarginTxtBox.Text);
                key = "MSAB_A1_MARGIN";
                helper.IniWriteValue(section, key, this.frm3GPPConfigMsAB_A1MarginTxtBox.Text);
                key = "MSAB_A2_MARGIN";
                helper.IniWriteValue(section, key, this.frm3GPPConfigMsAB_A2MarginTxtBox.Text);
                key = "CABLE_LOSS";
                try
                {
                    helper.IniWriteValue(section, key, Convert.ToDouble(this.frm3GPPConfigCableLossTxtBox.Text.ToString()).ToString());
                }
                catch (Exception exception6)
                {
                    MessageBox.Show("Cable Loss: " + exception6.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                section = "AIDING_PARAMS";
                key = "FREQ_TRANFER_METHOD";
                string str6 = "FREQ_AIDING";
                string str7 = "FREQ_METHOD";
                if (this.frm3GPPConfigCounterRadioBtn.Checked)
                {
                    helper.IniWriteValue(section, key, "Counter");
                    helper.IniWriteValue(str6, str7, "0");
                    str7 = "USE_FREQ_AIDING";
                    helper.IniWriteValue(str6, str7, "0");
                }
                else if (this.frm3GPPConfigNonCounterRadioBtn.Checked)
                {
                    helper.IniWriteValue(section, key, "NonCounter");
                    helper.IniWriteValue(str6, str7, "1");
                    str7 = "USE_FREQ_AIDING";
                    helper.IniWriteValue(str6, str7, "0");
                }
                else if (this.frm3GPPConfigNoFreqReqRadioBtn.Checked)
                {
                    str7 = "USE_FREQ_AIDING";
                    helper.IniWriteValue(section, key, "NoFreqReq");
                    helper.IniWriteValue(str6, str7, "2");
                }
                else
                {
                    helper.IniWriteValue(section, key, "NoFreqReq");
                }
                num = 1;
                key = "REL_FREQ_ACC_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest1RelFreqAccTxtBox.Text);
                }
                catch (Exception exception7)
                {
                    MessageBox.Show("REL_FREQ_ACC_TEST " + num.ToString() + ": " + exception7.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "REL_FREQ_ACC_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest2RelFreqAccTxtBox.Text);
                }
                catch (Exception exception8)
                {
                    MessageBox.Show("REL_FREQ_ACC_TEST " + num.ToString() + ": " + exception8.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "REL_FREQ_ACC_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest3RelFreqAccTxtBox.Text);
                }
                catch (Exception exception9)
                {
                    MessageBox.Show("REL_FREQ_ACC_TEST " + num.ToString() + ": " + exception9.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "REL_FREQ_ACC_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest4RelFreqAccTxtBox.Text);
                }
                catch (Exception exception10)
                {
                    MessageBox.Show("REL_FREQ_ACC_TEST " + num.ToString() + ": " + exception10.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num = 1;
                key = "HRZ_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest1HrzQoSTxtBox.Text);
                }
                catch (Exception exception11)
                {
                    MessageBox.Show("HRZ_QOS_TEST " + num.ToString() + ": " + exception11.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "HRZ_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest2HrzQoSTxtBox.Text);
                }
                catch (Exception exception12)
                {
                    MessageBox.Show("HRZ_QOS_TEST " + num.ToString() + ": " + exception12.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "HRZ_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest3HrzQoSTxtBox.Text);
                }
                catch (Exception exception13)
                {
                    MessageBox.Show("HRZ_QOS_TEST " + num.ToString() + ": " + exception13.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "HRZ_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest4HrzQoSTxtBox.Text);
                }
                catch (Exception exception14)
                {
                    MessageBox.Show("HRZ_QOS_TEST " + num.ToString() + ": " + exception14.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num = 1;
                key = "VRT_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest1VrtQoSTxtBox.Text);
                }
                catch (Exception exception15)
                {
                    MessageBox.Show("VRT_QOS_TEST " + num.ToString() + ": " + exception15.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "VRT_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest2VrtQoSTxtBox.Text);
                }
                catch (Exception exception16)
                {
                    MessageBox.Show("VRT_QOS_TEST " + num.ToString() + ": " + exception16.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "VRT_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest3VrtQoSTxtBox.Text);
                }
                catch (Exception exception17)
                {
                    MessageBox.Show("VRT_QOS_TEST " + num.ToString() + ": " + exception17.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "VRT_QOS_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest4VrtQoSTxtBox.Text);
                }
                catch (Exception exception18)
                {
                    MessageBox.Show("VRT_QOS_TEST " + num.ToString() + ": " + exception18.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num = 1;
                key = "RESPONSE_TIME_MAX_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest1RespMaxTimeTxtBox.Text);
                }
                catch (Exception exception19)
                {
                    MessageBox.Show("RESPONSE_TIME_MAX_TEST " + num.ToString() + ": " + exception19.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "RESPONSE_TIME_MAX_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest2RespMaxTimeTxtBox.Text);
                }
                catch (Exception exception20)
                {
                    MessageBox.Show("RESPONSE_TIME_MAX_TEST " + num.ToString() + ": " + exception20.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "RESPONSE_TIME_MAX_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest3RespMaxTimeTxtBox.Text);
                }
                catch (Exception exception21)
                {
                    MessageBox.Show("RESPONSE_TIME_MAX_TEST " + num.ToString() + ": " + exception21.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "RESPONSE_TIME_MAX_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest4RespMaxTimeTxtBox.Text);
                }
                catch (Exception exception22)
                {
                    MessageBox.Show("RESPONSE_TIME_MAX_TEST " + num.ToString() + ": " + exception22.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num = 1;
                key = "PRIORITY_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest1PriorityTxtBox.Text);
                }
                catch (Exception exception23)
                {
                    MessageBox.Show("PRIORITY_TEST " + num.ToString() + ": " + exception23.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "PRIORITY_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest2PriorityTxtBox.Text);
                }
                catch (Exception exception24)
                {
                    MessageBox.Show("PRIORITY_TEST " + num.ToString() + ": " + exception24.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "PRIORITY_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest3PriorityTxtBox.Text);
                }
                catch (Exception exception25)
                {
                    MessageBox.Show("PRIORITY_TEST " + num.ToString() + ": " + exception25.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
                num++;
                key = "PRIORITY_TEST" + num.ToString();
                try
                {
                    helper.IniWriteValue(section, key, this.frm3GPPConfigTest4PriorityTxtBox.Text);
                }
                catch (Exception exception26)
                {
                    MessageBox.Show("PRIORITY_TEST " + num.ToString() + ": " + exception26.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Cursor = Cursors.Default;
                    return 1;
                }
            }
            this.Cursor = Cursors.Default;
            return 0;
        }

        internal void updateConfigData(string updatedData)
        {
            string updateType = this.inputForm.UpdateType;
            if (updateType != null)
            {
                if (!(updateType == "CYCLES_TEST1"))
                {
                    if (!(updateType == "CYCLES_TEST2"))
                    {
                        if (!(updateType == "CYCLES_TEST3"))
                        {
                            if (updateType == "CYCLES_TEST4")
                            {
                                this.frm3GPPConfigTest4CyclesComboBox.Items.Add(updatedData);
                                this.frm3GPPConfigTest4CyclesComboBox.Text = updatedData;
                            }
                            return;
                        }
                        this.frm3GPPConfigTest3CyclesComboBox.Items.Add(updatedData);
                        this.frm3GPPConfigTest3CyclesComboBox.Text = updatedData;
                        return;
                    }
                }
                else
                {
                    this.frm3GPPConfigTest1CyclesComboBox.Items.Add(updatedData);
                    this.frm3GPPConfigTest1CyclesComboBox.Text = updatedData;
                    return;
                }
                this.frm3GPPConfigTest2CyclesComboBox.Items.Add(updatedData);
                this.frm3GPPConfigTest2CyclesComboBox.Text = updatedData;
            }
        }

        private void updateDefaultSettings(string configFilePath)
        {
            this.loadDefaultCycles();
            this.loadConfigFile(configFilePath);
            this.updateEstimatedTime();
            this.updateMaxError(-1);
        }

        private void updateEstimatedTime()
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            try
            {
                if ((this.frm3GPPConfigTest1CyclesComboBox.Text.Length != 0) && this.frm3GPPConfigTest1ChkBox.Checked)
                {
                    num = Convert.ToInt32(this.frm3GPPConfigTest1CyclesComboBox.Text) * 60;
                }
                if ((this.frm3GPPConfigTest2CyclesComboBox.Text.Length != 0) && this.frm3GPPConfigTest2ChkBox.Checked)
                {
                    num2 = Convert.ToInt32(this.frm3GPPConfigTest2CyclesComboBox.Text) * 60;
                }
                if ((this.frm3GPPConfigTest3CyclesComboBox.Text.Length != 0) && this.frm3GPPConfigTest3ChkBox.Checked)
                {
                    num3 = Convert.ToInt32(this.frm3GPPConfigTest3CyclesComboBox.Text) * 60;
                }
                if ((this.frm3GPPConfigTest4CyclesComboBox.Text.Length != 0) && this.frm3GPPConfigTest4ChkBox.Checked)
                {
                    num4 = Convert.ToInt32(this.frm3GPPConfigTest4CyclesComboBox.Text) * 60;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.frm3GPPConfigTest1TimeLabel.Text = num.ToString();
            this.frm3GPPConfigTest2TimeLabel.Text = num2.ToString();
            this.frm3GPPConfigTest3TimeLabel.Text = num3.ToString();
            this.frm3GPPConfigTest4TimeLabel.Text = num4.ToString();
            this.frm3GPPConfigTotalTimeLabel.Text = (((num + num2) + num3) + num4).ToString();
        }

        private void updateLastSettings(string configFilePath)
        {
            this.loadDefaultCycles();
            this.loadConfigFile(configFilePath);
            this.updateEstimatedTime();
            this.updateMaxError(-1);
        }

        private void updateMaxError(int testNum)
        {
            try
            {
                switch (testNum)
                {
                    case 1:
                        this.frm3GPPConfigTest1MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest1CyclesComboBox.Text)).ToString();
                        return;

                    case 2:
                        this.frm3GPPConfigTest2MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest2CyclesComboBox.Text)).ToString();
                        return;

                    case 3:
                        this.frm3GPPConfigTest3MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest3CyclesComboBox.Text)).ToString();
                        return;

                    case 4:
                        this.frm3GPPConfigTest4MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest4CyclesComboBox.Text)).ToString();
                        return;
                }
                this.frm3GPPConfigTest1MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest1CyclesComboBox.Text)).ToString();
                this.frm3GPPConfigTest2MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest2CyclesComboBox.Text)).ToString();
                this.frm3GPPConfigTest3MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest3CyclesComboBox.Text)).ToString();
                this.frm3GPPConfigTest4MaxErrorTxtBox.Text = this.getMaxError(Convert.ToInt32(this.frm3GPPConfigTest4CyclesComboBox.Text)).ToString();
                this.frm3GPPConfigTest5MaxErrorTxtBox.Text = "";
            }
            catch
            {
            }
        }

        private void updateTestsToRunList()
        {
            if (this.frm3GPPConfigTest1ChkBox.Checked)
            {
                foreach (string str in this.frm3GPPConfigMsBasedMarginTxtBox.Text.Split(new char[] { ',' }))
                {
                    if (this.frm3GPPConfigMsBasedChkBox.Checked)
                    {
                        clsGlobal.TestsToRun.Add(string.Format("MSB SENSITIVITY_TEST1({0}): Sensitivity Margin: {1}", this.frm3GPPConfigTest1CyclesComboBox.Text.ToString(), str.Trim()));
                    }
                }
                foreach (string str2 in this.frm3GPPConfigMsAssist1MarginTxtBox.Text.Split(new char[] { ',' }))
                {
                    if (this.frm3GPPConfigMsAssist1ChkBox.Checked)
                    {
                        clsGlobal.TestsToRun.Add(string.Format("MSA-1 SENSITIVITY_TEST1({0}): Sensitivity Margin: {1}", this.frm3GPPConfigTest1CyclesComboBox.Text.ToString(), str2.Trim()));
                    }
                }
                foreach (string str3 in this.frm3GPPConfigMsAssist2MarginTxtBox.Text.Split(new char[] { ',' }))
                {
                    if (this.frm3GPPConfigMsAssist2ChkBox.Checked)
                    {
                        clsGlobal.TestsToRun.Add(string.Format("MSA-2 SENSITIVITY_TEST1({0}): Sensitivity Margin: {1}", this.frm3GPPConfigTest1CyclesComboBox.Text.ToString(), str3.Trim()));
                    }
                }
            }
            if (this.frm3GPPConfigTest2ChkBox.Checked)
            {
                if (this.frm3GPPConfigMsBasedChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSB NOMINAL_ACCURACY_TEST2({0}):  Nominal Accuracy", this.frm3GPPConfigTest2CyclesComboBox.Text.ToString()));
                }
                if (this.frm3GPPConfigMsAssist1ChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSA-1 NOMINAL_ACCURACY_TEST2({0}):  Nominal Accuracy", this.frm3GPPConfigTest2CyclesComboBox.Text.ToString()));
                }
                if (this.frm3GPPConfigMsAssist2ChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSA-2 NOMINAL_ACCURACY_TEST2({0}):  Nominal Accuracy", this.frm3GPPConfigTest2CyclesComboBox.Text.ToString()));
                }
            }
            if (this.frm3GPPConfigTest3ChkBox.Checked)
            {
                if (this.frm3GPPConfigMsBasedChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSB DYNAMIC_RANGE_TEST3({0}):  Dynamic Range", this.frm3GPPConfigTest3CyclesComboBox.Text.ToString()));
                }
                if (this.frm3GPPConfigMsAssist1ChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSA-1 DYNAMIC_RANGE_TEST3({0}):  Dynamic Range", this.frm3GPPConfigTest3CyclesComboBox.Text.ToString()));
                }
                if (this.frm3GPPConfigMsAssist2ChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSA-2 DYNAMIC_RANGE_TEST3({0}):  Dynamic Range", this.frm3GPPConfigTest3CyclesComboBox.Text.ToString()));
                }
            }
            if (this.frm3GPPConfigTest4ChkBox.Checked)
            {
                if (this.frm3GPPConfigMsBasedChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSB MULTIPATH_TEST4({0}):  Multipath", this.frm3GPPConfigTest4CyclesComboBox.Text.ToString()));
                }
                if (this.frm3GPPConfigMsAssist1ChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSA-1 MULTIPATH_TEST4({0}):  Multipath", this.frm3GPPConfigTest4CyclesComboBox.Text.ToString()));
                }
                if (this.frm3GPPConfigMsAssist2ChkBox.Checked)
                {
                    clsGlobal.TestsToRun.Add(string.Format("MSA-2 MULTIPATH_TEST4({0}):  Multipath", this.frm3GPPConfigTest4CyclesComboBox.Text.ToString()));
                }
            }
            if (this.frm3GPPConfigTest5ChkBox.Checked)
            {
                clsGlobal.TestsToRun.Add("MOVING_SCENARIO_TEST5");
            }
        }
    }
}

