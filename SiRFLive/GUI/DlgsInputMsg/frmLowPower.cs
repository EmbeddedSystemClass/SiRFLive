﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.Configuration;
    using SiRFLive.General;
    using SiRFLive.GUI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Timers;
    using System.Windows.Forms;

    public class frmLowPower : Form
    {
        private lowPowerParams _lowPowerSettings = new lowPowerParams();
        private Label apmDutyCycleLabel;
        private TextBox apmDutyCycleTxtBox;
        private TextBox apmMaxHrzErrorTxtBox;
        private Label apmMaxOffTimeLabel;
        private TextBox apmMaxOffTimeTxtBox;
        private Label apmMaxSearchTimeLabel;
        private TextBox apmMaxSearchTimeTxtBox;
        private ComboBox apmMaxVrtErrorComboBox;
        private Label apmMazHrzErrorLabel;
        private Label apmMazVrtErrorLabel;
        private Label apmNumFixesLabel;
        private TextBox apmNumFixesTxtBox;
        private ComboBox apmPriorityComboBox;
        private Label apmPriorityLabel;
        private TabPage apmTab;
        private Label apmTBFLabel;
        private TextBox apmTBFTxtBox;
        private ComboBox apmTimeAccPriorityComboBox;
        private Label apmTimeAccPriorityLabel;
        private Button btnSiRFaware;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton frmLowPowerAPMRadioBtn;
        private Button frmLowPowerBufferBtn;
        private Button frmLowPowerCancelBtn;
        private RadioButton frmLowPowerFullPowerRadioBtn;
        private GroupBox frmLowPowerModeGroupBox;
        private TabControl frmLowPowerModeTab;
        private Button frmLowPowerOkBtn;
        private RadioButton frmLowPowerPushToFixRadioBtn;
        private RadioButton frmLowPowerTricklePowerRadioBtn;
        private frmCommonSimpleInput inputForm = new frmCommonSimpleInput("Enter Data:");
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        public static string priorLowPowerSetting = "";
        private Label ptfMaxOffTimeLabel;
        private TextBox ptfMaxOffTimeTxtBox;
        private Label ptfMaxSearchTimeLabel;
        private TextBox ptfMaxSearchTimeTxtBox;
        private Label ptfPeriodLabel;
        private TextBox ptfPeriodTxtBox;
        private TabPage ptfTab;
        private TextBox textBox1;
        private TextBox textBox10;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private Label tpMaxOffTimeLabel;
        private TextBox tpMaxOffTimeTxtBox;
        private Label tpMaxSearchTimeLabel;
        private TextBox tpMaxSearchTimeTxtBox;
        private ComboBox tpOnTimeCmbBox;
        private Label tpOntimeLabel;
        private TabPage tpTab;
        private ComboBox tpUpdateRateCmbBox;
        private Label tpUpdateRateLabel;

        public frmLowPower(CommunicationManager CommWindow)
        {
            this.InitializeComponent();
            this.comm = CommWindow;
            this.inputForm.updateParent += new frmCommonSimpleInput.updateParentEventHandler(this.updateConfigList);
            this.inputForm.MdiParent = base.MdiParent;
        }

        private void btnSiRFaware_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                base.Close();
                new frmCommSiRFawareV2(this.comm).Show();
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

        private void frmLowPower_Load(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                this._lowPowerSettings = this.comm.LowPowerParams;
                this.frmLowPowerLoadSettings();
                switch (this.comm.LowPowerParams.Mode)
                {
                    case 0:
                        this.frmLowPowerFullPowerRadioBtn.Checked = true;
                        break;

                    case 1:
                        this.frmLowPowerAPMRadioBtn.Checked = true;
                        this.frmLowPowerModeTab.SelectedIndex = 0;
                        break;

                    case 3:
                        this.frmLowPowerTricklePowerRadioBtn.Checked = true;
                        this.frmLowPowerModeTab.SelectedIndex = 1;
                        break;

                    case 4:
                        this.frmLowPowerPushToFixRadioBtn.Checked = true;
                        this.frmLowPowerModeTab.SelectedIndex = 2;
                        break;

                    default:
                        this.frmLowPowerFullPowerRadioBtn.Checked = true;
                        break;
                }
                if (this.frmLowPowerFullPowerRadioBtn.Checked)
                {
                    this.frmLowPowerModeTab.Enabled = false;
                }
                else
                {
                    this.frmLowPowerModeTab.Enabled = true;
                }
            }
        }

        private void frmLowPowerAPMRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmLowPowerAPMRadioBtn.Checked)
            {
                this.frmLowPowerModeTab.SelectedIndex = 0;
            }
        }

        private void frmLowPowerBufferBtn_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                new frmLPBufferWindow(this.comm).ShowDialog();
            }
        }

        private void frmLowPowerCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmLowPowerFullPowerRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmLowPowerFullPowerRadioBtn.Checked)
            {
                this.frmLowPowerModeTab.Enabled = false;
            }
            else
            {
                this.frmLowPowerModeTab.Enabled = true;
            }
        }

        private void frmLowPowerLoadSettings()
        {
            string path = clsGlobal.InstalledDirectory + @"\scripts\SiRFLiveAutomationSetup.cfg";
            string text = "\n";
            if (!File.Exists(path))
            {
                text = "Config file not found: \n" + path + "\n";
                MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            IniHelper helper = new IniHelper(path);
            string section = string.Empty;
            string key = string.Empty;
            section = "POWER_MODE";
            key = "MODE";
            try
            {
                this.comm.LowPowerParams.Mode = Convert.ToByte(helper.IniReadValue(section, key));
            }
            catch (Exception exception)
            {
                text = text + "APM num fixes: " + exception.Message + "\n";
            }
            key = "APM_NUM_FIXES";
            try
            {
                this.comm.LowPowerParams.APMNumFixes = Convert.ToByte(helper.IniReadValue(section, key));
                this.apmNumFixesTxtBox.Text = this.comm.LowPowerParams.APMNumFixes.ToString();
            }
            catch (Exception exception2)
            {
                text = text + "APM num fixes: " + exception2.Message + "\n";
            }
            key = "APM_TBF";
            try
            {
                this.comm.LowPowerParams.APMTBF = Convert.ToByte(helper.IniReadValue(section, key));
                this.apmTBFTxtBox.Text = this.comm.LowPowerParams.APMTBF.ToString();
            }
            catch (Exception exception3)
            {
                text = text + "APM TBF: " + exception3.Message + "\n";
            }
            key = "APM_DUTY_CYCLE";
            try
            {
                this.comm.LowPowerParams.APMDutyCycle = Convert.ToByte(helper.IniReadValue(section, key));
                this.apmDutyCycleTxtBox.Text = this.comm.LowPowerParams.APMDutyCycle.ToString();
            }
            catch (Exception exception4)
            {
                text = text + "APM duty cycle: " + exception4.Message + "\n";
            }
            key = "APM_MAX_HOR_ERR";
            try
            {
                this.comm.LowPowerParams.APMMaxHrzError = Convert.ToByte(helper.IniReadValue(section, key));
                this.apmMaxHrzErrorTxtBox.Text = this.comm.LowPowerParams.APMMaxHrzError.ToString();
            }
            catch (Exception exception5)
            {
                text = text + "APM max Hrz. error: " + exception5.Message + "\n";
            }
            key = "APM_MAX_VERT_ERR";
            try
            {
                this.comm.LowPowerParams.APMMaxVrtError = Convert.ToByte(helper.IniReadValue(section, key));
                if (this.comm.LowPowerParams.APMMaxVrtError >= 8)
                {
                    this.apmMaxVrtErrorComboBox.Items.Add(this.comm.LowPowerParams.APMMaxVrtError.ToString());
                    this.apmMaxVrtErrorComboBox.Text = this.comm.LowPowerParams.APMMaxVrtError.ToString();
                }
                else
                {
                    this.apmMaxVrtErrorComboBox.SelectedIndex = this.comm.LowPowerParams.APMMaxVrtError;
                }
            }
            catch (Exception exception6)
            {
                text = text + "APM max Vrt. error: " + exception6.Message + "\n";
            }
            key = "APM_PRIORITY";
            try
            {
                this.comm.LowPowerParams.APMPriority = Convert.ToByte(helper.IniReadValue(section, key));
                if (this.comm.LowPowerParams.APMPriority > 3)
                {
                    this.apmPriorityComboBox.Items.Add(this.comm.LowPowerParams.APMPriority.ToString());
                    this.apmPriorityComboBox.Text = this.comm.LowPowerParams.APMPriority.ToString();
                }
                else
                {
                    this.apmPriorityComboBox.SelectedIndex = this.comm.LowPowerParams.APMPriority - 1;
                }
            }
            catch (Exception exception7)
            {
                text = text + "APM priority: " + exception7.Message + "\n";
            }
            key = "APM_MAX_OFFTIME";
            try
            {
                this.comm.LowPowerParams.APMMaxOffTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.apmMaxOffTimeTxtBox.Text = this.comm.LowPowerParams.APMMaxOffTime.ToString();
            }
            catch (Exception exception8)
            {
                text = text + "APM max off time: " + exception8.Message + "\n";
            }
            key = "APM_MAX_SEARCH_TIME";
            try
            {
                this.comm.LowPowerParams.APMMaxSearchTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.apmMaxSearchTimeTxtBox.Text = this.comm.LowPowerParams.APMMaxSearchTime.ToString();
            }
            catch (Exception exception9)
            {
                text = text + "APM max search time: " + exception9.Message + "\n";
            }
            key = "APM_TIME_ACC_PRIORITY";
            try
            {
                this.comm.LowPowerParams.APMTimeAccPriority = Convert.ToByte(helper.IniReadValue(section, key));
                if (this.comm.LowPowerParams.APMTimeAccPriority > 3)
                {
                    this.apmTimeAccPriorityComboBox.Items.Add(this.comm.LowPowerParams.APMTimeAccPriority.ToString());
                    this.apmTimeAccPriorityComboBox.Text = this.comm.LowPowerParams.APMTimeAccPriority.ToString();
                }
                else
                {
                    this.apmTimeAccPriorityComboBox.SelectedIndex = this.comm.LowPowerParams.APMTimeAccPriority;
                }
            }
            catch (Exception exception10)
            {
                text = text + "APM time acc priority: " + exception10.Message + "\n";
            }
            key = "TP_UPDATE";
            try
            {
                this.comm.LowPowerParams.TPUpdateRate = Convert.ToSByte(helper.IniReadValue(section, key));
                this.tpUpdateRateCmbBox.Text = this.comm.LowPowerParams.TPUpdateRate.ToString();
            }
            catch (Exception exception11)
            {
                text = text + "TP update rate: " + exception11.Message + "\n";
            }
            key = "TP_ONTIME";
            try
            {
                this.comm.LowPowerParams.TPOnTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.tpOnTimeCmbBox.Text = this.comm.LowPowerParams.TPOnTime.ToString();
            }
            catch (Exception exception12)
            {
                text = text + "TP on time: " + exception12.Message + "\n";
            }
            key = "TP_MAX_OFF_TIME";
            try
            {
                this.comm.LowPowerParams.TPMaxOffTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.tpMaxOffTimeTxtBox.Text = this.comm.LowPowerParams.TPMaxOffTime.ToString();
            }
            catch (Exception exception13)
            {
                text = text + "TP max off time: " + exception13.Message + "\n";
            }
            key = "TP_MAX_SEARCH_TIME";
            try
            {
                this.comm.LowPowerParams.TPMaxSearchTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.tpMaxSearchTimeTxtBox.Text = this.comm.LowPowerParams.TPMaxSearchTime.ToString();
            }
            catch (Exception exception14)
            {
                text = text + "TP max search time: " + exception14.Message + "\n";
            }
            key = "PTF_PERIOD";
            try
            {
                this.comm.LowPowerParams.PTFPeriod = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.ptfPeriodTxtBox.Text = this.comm.LowPowerParams.PTFPeriod.ToString();
            }
            catch (Exception exception15)
            {
                text = text + "PTF period: " + exception15.Message + "\n";
            }
            key = "PTF_MAX_SEARCH_TIME";
            try
            {
                this.comm.LowPowerParams.PTFMaxSearchTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.ptfMaxSearchTimeTxtBox.Text = this.comm.LowPowerParams.PTFMaxSearchTime.ToString();
            }
            catch (Exception exception16)
            {
                text = text + "PTF max search time: " + exception16.Message + "\n";
            }
            key = "PTF_MAX_OFF_TIME";
            try
            {
                this.comm.LowPowerParams.PTFMaxOffTime = Convert.ToUInt32(helper.IniReadValue(section, key));
                this.ptfMaxOffTimeTxtBox.Text = this.comm.LowPowerParams.PTFMaxOffTime.ToString();
            }
            catch (Exception exception17)
            {
                text = text + "PTF max off time: " + exception17.Message + "\n";
            }
        }

        private void frmLowPowerOkBtn_Click(object sender, EventArgs e)
        {
            string text = "\n";
            if (!this.frmLowPowerAPMRadioBtn.Checked)
            {
                if (this.frmLowPowerTricklePowerRadioBtn.Checked)
                {
                    this._lowPowerSettings.Mode = 3;
                    try
                    {
                        this._lowPowerSettings.TPUpdateRate = Convert.ToSByte(this.tpUpdateRateCmbBox.Text);
                        if (((this._lowPowerSettings.TPUpdateRate < 1) || (this._lowPowerSettings.TPUpdateRate > 10)) && (MessageBox.Show("Invalid Update Rate\nValid range is 1-10 seconds\nContinue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                        {
                            text = text + "Invalid TP Update Rate\n";
                        }
                    }
                    catch (Exception exception10)
                    {
                        text = text + "TP Update Rate: " + exception10.Message + "\n";
                    }
                    try
                    {
                        this._lowPowerSettings.TPOnTime = Convert.ToUInt32(this.tpOnTimeCmbBox.Text);
                        if (((this._lowPowerSettings.TPOnTime < 100) || (this._lowPowerSettings.TPOnTime > 900)) && (MessageBox.Show("Invalid range TP on time\nValid range is 100-900 msec\nContinue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                        {
                            text = text + "Invalid TP On Time\n";
                        }
                    }
                    catch (Exception exception11)
                    {
                        text = text + "TP On Time: " + exception11.Message + "\n";
                    }
                    try
                    {
                        this._lowPowerSettings.TPMaxOffTime = Convert.ToUInt32(this.tpMaxOffTimeTxtBox.Text);
                    }
                    catch (Exception exception12)
                    {
                        text = text + "TP Max Off Time: " + exception12.Message + "\n";
                    }
                    try
                    {
                        this._lowPowerSettings.TPMaxSearchTime = Convert.ToUInt32(this.tpMaxSearchTimeTxtBox.Text);
                    }
                    catch (Exception exception13)
                    {
                        text = text + "TP Max Search Time: " + exception13.Message + "\n";
                    }
                    if ((this._lowPowerSettings.TPUpdateRate == 1) && (this._lowPowerSettings.TPOnTime >= 700))
                    {
                        text = text + "1 second Update Rate does not work for OnTime values over 600\n";
                    }
                }
                else
                {
                    if (this.frmLowPowerPushToFixRadioBtn.Checked)
                    {
                        this._lowPowerSettings.Mode = 4;
                        try
                        {
                            this._lowPowerSettings.PTFPeriod = Convert.ToUInt32(this.ptfPeriodTxtBox.Text);
                            if (((this._lowPowerSettings.PTFPeriod < 10) || (this._lowPowerSettings.PTFPeriod > 0x1c20)) && (MessageBox.Show("Invalid range PTF period\nValid range is 10-7200s\nContinue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                            {
                                text = text + "Invalid PTF Period\n";
                            }
                        }
                        catch (Exception exception14)
                        {
                            text = text + "PTF Period: " + exception14.Message + "\n";
                        }
                        try
                        {
                            this._lowPowerSettings.PTFMaxSearchTime = Convert.ToUInt32(this.ptfMaxSearchTimeTxtBox.Text);
                        }
                        catch (Exception exception15)
                        {
                            text = text + "PTF Max Search Time: " + exception15.Message + "\n";
                        }
                        try
                        {
                            this._lowPowerSettings.PTFMaxOffTime = Convert.ToUInt32(this.ptfMaxOffTimeTxtBox.Text);
                            if (((this._lowPowerSettings.PTFMaxOffTime < 0x3e8) || (this._lowPowerSettings.PTFMaxOffTime > 0x2bf20)) && (MessageBox.Show("Invalid range PTF max off time\nValid range is 1000 – 180000ms\nContinue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                            {
                                text = text + "Invalid PTF Period\n";
                            }
                            goto Label_065C;
                        }
                        catch (Exception exception16)
                        {
                            text = text + "PTF Max Off Time: " + exception16.Message + "\n";
                            goto Label_065C;
                        }
                    }
                    this._lowPowerSettings.Mode = 0;
                }
                goto Label_065C;
            }
            this._lowPowerSettings.Mode = 1;
            try
            {
                this._lowPowerSettings.APMNumFixes = Convert.ToByte(this.apmNumFixesTxtBox.Text);
            }
            catch (Exception exception)
            {
                text = text + "APM Num Fixes: " + exception.Message + "\n";
            }
            try
            {
                this._lowPowerSettings.APMTBF = Convert.ToByte(this.apmTBFTxtBox.Text);
                if (((this._lowPowerSettings.APMTBF <= 0) || (this._lowPowerSettings.APMTBF > 180)) && (MessageBox.Show("Invalid range APM TBF\nValid range is 1-180s\nContinue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                {
                    text = text + "Invalid APM TBF\n";
                }
            }
            catch (Exception exception2)
            {
                text = text + "APM TBF: " + exception2.Message + "\n";
            }
            try
            {
                this._lowPowerSettings.APMDutyCycle = Convert.ToByte(this.apmDutyCycleTxtBox.Text);
                if ((this._lowPowerSettings.APMDutyCycle < 5) || (this._lowPowerSettings.APMDutyCycle > 100))
                {
                    text = text + "APM Duty Cycle min at 5% and max at 100%\n";
                }
            }
            catch (Exception exception3)
            {
                text = text + "APM Duty Cycle: " + exception3.Message + "\n";
            }
            try
            {
                this._lowPowerSettings.APMMaxHrzError = Convert.ToByte(this.apmMaxHrzErrorTxtBox.Text);
            }
            catch (Exception exception4)
            {
                text = text + "APM Max Hrz. Error: " + exception4.Message + "\n";
            }
            if ((this.apmMaxVrtErrorComboBox.SelectedIndex > 7) || (this.apmMaxVrtErrorComboBox.SelectedIndex < 0))
            {
                try
                {
                    this._lowPowerSettings.APMMaxVrtError = Convert.ToByte(this.apmMaxVrtErrorComboBox.Text);
                    goto Label_01DA;
                }
                catch (Exception exception5)
                {
                    text = text + "APM Max Vrt.Error: " + exception5.Message + "\n";
                    goto Label_01DA;
                }
            }
            this._lowPowerSettings.APMMaxVrtError = (byte) this.apmMaxVrtErrorComboBox.SelectedIndex;
        Label_01DA:
            if ((this.apmPriorityComboBox.SelectedIndex > 1) || (this.apmPriorityComboBox.SelectedIndex < 0))
            {
                try
                {
                    this._lowPowerSettings.APMPriority = Convert.ToByte(this.apmPriorityComboBox.Text.ToString());
                    goto Label_024D;
                }
                catch (Exception exception6)
                {
                    text = text + "APM Priority: " + exception6.Message + "\n";
                    goto Label_024D;
                }
            }
            this._lowPowerSettings.APMPriority = (byte) (this.apmPriorityComboBox.SelectedIndex + 1);
        Label_024D:
            try
            {
                this._lowPowerSettings.APMMaxOffTime = Convert.ToUInt32(this.apmMaxOffTimeTxtBox.Text);
            }
            catch (Exception exception7)
            {
                text = text + "APM Max Off Time: " + exception7.Message + "\n";
            }
            try
            {
                this._lowPowerSettings.APMMaxSearchTime = Convert.ToUInt32(this.apmMaxSearchTimeTxtBox.Text);
            }
            catch (Exception exception8)
            {
                text = text + "APM Max Search Time: " + exception8.Message + "\n";
            }
            if ((this.apmTimeAccPriorityComboBox.SelectedIndex > 2) || (this.apmTimeAccPriorityComboBox.SelectedIndex < 0))
            {
                try
                {
                    this._lowPowerSettings.APMTimeAccPriority = Convert.ToByte(this.apmTimeAccPriorityComboBox.Text.ToString());
                    goto Label_065C;
                }
                catch (Exception exception9)
                {
                    text = text + "APM Time Acc Priority: " + exception9.Message + "\n";
                    goto Label_065C;
                }
            }
            this._lowPowerSettings.APMTimeAccPriority = (byte) this.apmTimeAccPriorityComboBox.SelectedIndex;
        Label_065C:
            if (text != "\n")
            {
                MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (this.comm != null)
                {
                    if (this.frmLowPowerAPMRadioBtn.Checked)
                    {
                        if (clsGlobal.PerformOnAll)
                        {
                            foreach (string str2 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                            {
                                PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str2];
                                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                                {
                                    this.setAPM_AR_Parameters_4e(ref manager.comm);
                                    manager.comm.RxCtrl.ResetCtrl.Reset("HOT");
                                }
                            }
                        }
                        else
                        {
                            this.setAPM_AR_Parameters_4e(ref this.comm);
                            this.comm.RxCtrl.ResetCtrl.Reset("HOT");
                        }
                        System.Timers.Timer timer = new System.Timers.Timer();
                        timer.Elapsed += new ElapsedEventHandler(this.SendAPMTimerHandler);
                        timer.Interval = 2000.0;
                        timer.AutoReset = false;
                        timer.Start();
                    }
                    else
                    {
                        this.SendLowPower();
                    }
                }
                base.Close();
            }
        }

        private void frmLowPowerPushToFixRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmLowPowerPushToFixRadioBtn.Checked)
            {
                this.frmLowPowerModeTab.SelectedIndex = 2;
            }
        }

        private void frmLowPowerSaveSettings()
        {
            this.Cursor = Cursors.WaitCursor;
            string path = clsGlobal.InstalledDirectory + @"\scripts\SiRFLiveAutomationSetup.cfg";
            if (!File.Exists(path))
            {
                MessageBox.Show("Config file does not exist -- Not saving" + path, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if ((File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                MessageBox.Show("Config file is read only!-- Not saving\n" + path, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            IniHelper helper = new IniHelper(path);
            string section = string.Empty;
            string key = string.Empty;
            section = "POWER_MODE";
            key = "MODE";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.Mode.ToString());
            key = "APM_NUM_FIXES";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMNumFixes.ToString());
            key = "APM_TBF";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMTBF.ToString());
            key = "APM_DUTY_CYCLE";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMDutyCycle.ToString());
            key = "APM_MAX_HOR_ERR";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMMaxHrzError.ToString());
            key = "APM_MAX_VERT_ERR";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMMaxVrtError.ToString());
            key = "APM_PRIORITY";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMPriority.ToString());
            key = "APM_MAX_OFFTIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMMaxOffTime.ToString());
            key = "APM_MAX_SEARCH_TIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMMaxSearchTime.ToString());
            key = "APM_TIME_ACC_PRIORITY";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.APMTimeAccPriority.ToString());
            key = "TP_UPDATE";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.TPUpdateRate.ToString());
            key = "TP_ONTIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.TPOnTime.ToString());
            key = "TP_MAX_OFF_TIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.TPMaxOffTime.ToString());
            key = "TP_MAX_SEARCH_TIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.TPMaxSearchTime.ToString());
            key = "PTF_PERIOD";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.PTFPeriod.ToString());
            key = "PTF_MAX_SEARCH_TIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.PTFMaxSearchTime.ToString());
            key = "PTF_MAX_OFF_TIME";
            helper.IniWriteValue(section, key, this.comm.LowPowerParams.PTFMaxOffTime.ToString());
            this.Cursor = Cursors.Default;
        }

        private void frmLowPowerTricklePowerRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmLowPowerTricklePowerRadioBtn.Checked)
            {
                this.frmLowPowerModeTab.SelectedIndex = 1;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmLowPower));
            this.frmLowPowerModeGroupBox = new GroupBox();
            this.btnSiRFaware = new Button();
            this.frmLowPowerBufferBtn = new Button();
            this.frmLowPowerFullPowerRadioBtn = new RadioButton();
            this.frmLowPowerPushToFixRadioBtn = new RadioButton();
            this.frmLowPowerTricklePowerRadioBtn = new RadioButton();
            this.frmLowPowerAPMRadioBtn = new RadioButton();
            this.frmLowPowerOkBtn = new Button();
            this.frmLowPowerCancelBtn = new Button();
            this.frmLowPowerModeTab = new TabControl();
            this.apmTab = new TabPage();
            this.label16 = new Label();
            this.apmTimeAccPriorityComboBox = new ComboBox();
            this.apmTimeAccPriorityLabel = new Label();
            this.apmDutyCycleLabel = new Label();
            this.apmDutyCycleTxtBox = new TextBox();
            this.label15 = new Label();
            this.apmMazVrtErrorLabel = new Label();
            this.apmPriorityComboBox = new ComboBox();
            this.apmMaxVrtErrorComboBox = new ComboBox();
            this.apmMaxSearchTimeLabel = new Label();
            this.apmMaxOffTimeLabel = new Label();
            this.apmPriorityLabel = new Label();
            this.apmMazHrzErrorLabel = new Label();
            this.apmTBFLabel = new Label();
            this.apmNumFixesLabel = new Label();
            this.apmMaxSearchTimeTxtBox = new TextBox();
            this.apmMaxOffTimeTxtBox = new TextBox();
            this.apmMaxHrzErrorTxtBox = new TextBox();
            this.apmTBFTxtBox = new TextBox();
            this.apmNumFixesTxtBox = new TextBox();
            this.tpTab = new TabPage();
            this.tpOnTimeCmbBox = new ComboBox();
            this.tpUpdateRateCmbBox = new ComboBox();
            this.tpUpdateRateLabel = new Label();
            this.tpMaxSearchTimeLabel = new Label();
            this.tpMaxOffTimeLabel = new Label();
            this.tpMaxSearchTimeTxtBox = new TextBox();
            this.tpMaxOffTimeTxtBox = new TextBox();
            this.tpOntimeLabel = new Label();
            this.ptfTab = new TabPage();
            this.ptfMaxOffTimeLabel = new Label();
            this.ptfMaxOffTimeTxtBox = new TextBox();
            this.ptfMaxSearchTimeLabel = new Label();
            this.ptfPeriodLabel = new Label();
            this.ptfMaxSearchTimeTxtBox = new TextBox();
            this.ptfPeriodTxtBox = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.textBox1 = new TextBox();
            this.label4 = new Label();
            this.comboBox1 = new ComboBox();
            this.comboBox2 = new ComboBox();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.textBox2 = new TextBox();
            this.textBox3 = new TextBox();
            this.textBox4 = new TextBox();
            this.textBox5 = new TextBox();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label10 = new Label();
            this.textBox6 = new TextBox();
            this.label11 = new Label();
            this.comboBox3 = new ComboBox();
            this.comboBox4 = new ComboBox();
            this.label12 = new Label();
            this.label13 = new Label();
            this.label14 = new Label();
            this.textBox7 = new TextBox();
            this.textBox8 = new TextBox();
            this.textBox9 = new TextBox();
            this.textBox10 = new TextBox();
            this.frmLowPowerModeGroupBox.SuspendLayout();
            this.frmLowPowerModeTab.SuspendLayout();
            this.apmTab.SuspendLayout();
            this.tpTab.SuspendLayout();
            this.ptfTab.SuspendLayout();
            base.SuspendLayout();
            this.frmLowPowerModeGroupBox.Controls.Add(this.btnSiRFaware);
            this.frmLowPowerModeGroupBox.Controls.Add(this.frmLowPowerBufferBtn);
            this.frmLowPowerModeGroupBox.Controls.Add(this.frmLowPowerFullPowerRadioBtn);
            this.frmLowPowerModeGroupBox.Controls.Add(this.frmLowPowerPushToFixRadioBtn);
            this.frmLowPowerModeGroupBox.Controls.Add(this.frmLowPowerTricklePowerRadioBtn);
            this.frmLowPowerModeGroupBox.Controls.Add(this.frmLowPowerAPMRadioBtn);
            this.frmLowPowerModeGroupBox.Location = new Point(0x25, 30);
            this.frmLowPowerModeGroupBox.Name = "frmLowPowerModeGroupBox";
            this.frmLowPowerModeGroupBox.Size = new Size(0x11a, 0x60);
            this.frmLowPowerModeGroupBox.TabIndex = 0;
            this.frmLowPowerModeGroupBox.TabStop = false;
            this.frmLowPowerModeGroupBox.Text = "Power Mode";
            this.btnSiRFaware.Location = new Point(0xc0, 0x1b);
            this.btnSiRFaware.Name = "btnSiRFaware";
            this.btnSiRFaware.Size = new Size(0x4b, 0x17);
            this.btnSiRFaware.TabIndex = 4;
            this.btnSiRFaware.Text = "&SiRFaware";
            this.btnSiRFaware.UseVisualStyleBackColor = true;
            this.btnSiRFaware.Click += new EventHandler(this.btnSiRFaware_Click);
            this.frmLowPowerBufferBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.frmLowPowerBufferBtn.Location = new Point(0xc0, 0x3b);
            this.frmLowPowerBufferBtn.Name = "frmLowPowerBufferBtn";
            this.frmLowPowerBufferBtn.Size = new Size(0x4b, 0x17);
            this.frmLowPowerBufferBtn.TabIndex = 5;
            this.frmLowPowerBufferBtn.Text = "LP &Buffer";
            this.frmLowPowerBufferBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerBufferBtn.Click += new EventHandler(this.frmLowPowerBufferBtn_Click);
            this.frmLowPowerFullPowerRadioBtn.AutoSize = true;
            this.frmLowPowerFullPowerRadioBtn.Checked = true;
            this.frmLowPowerFullPowerRadioBtn.Location = new Point(11, 30);
            this.frmLowPowerFullPowerRadioBtn.Name = "frmLowPowerFullPowerRadioBtn";
            this.frmLowPowerFullPowerRadioBtn.Size = new Size(0x4a, 0x11);
            this.frmLowPowerFullPowerRadioBtn.TabIndex = 0;
            this.frmLowPowerFullPowerRadioBtn.TabStop = true;
            this.frmLowPowerFullPowerRadioBtn.Text = "&Full Power";
            this.frmLowPowerFullPowerRadioBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerFullPowerRadioBtn.CheckedChanged += new EventHandler(this.frmLowPowerFullPowerRadioBtn_CheckedChanged);
            this.frmLowPowerPushToFixRadioBtn.AutoSize = true;
            this.frmLowPowerPushToFixRadioBtn.Location = new Point(0x6b, 0x3e);
            this.frmLowPowerPushToFixRadioBtn.Name = "frmLowPowerPushToFixRadioBtn";
            this.frmLowPowerPushToFixRadioBtn.Size = new Size(0x51, 0x11);
            this.frmLowPowerPushToFixRadioBtn.TabIndex = 3;
            this.frmLowPowerPushToFixRadioBtn.Text = "&Push To Fix";
            this.frmLowPowerPushToFixRadioBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerPushToFixRadioBtn.CheckedChanged += new EventHandler(this.frmLowPowerPushToFixRadioBtn_CheckedChanged);
            this.frmLowPowerTricklePowerRadioBtn.AutoSize = true;
            this.frmLowPowerTricklePowerRadioBtn.Location = new Point(11, 0x3e);
            this.frmLowPowerTricklePowerRadioBtn.Name = "frmLowPowerTricklePowerRadioBtn";
            this.frmLowPowerTricklePowerRadioBtn.Size = new Size(90, 0x11);
            this.frmLowPowerTricklePowerRadioBtn.TabIndex = 1;
            this.frmLowPowerTricklePowerRadioBtn.Text = "&Trickle Power";
            this.frmLowPowerTricklePowerRadioBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerTricklePowerRadioBtn.CheckedChanged += new EventHandler(this.frmLowPowerTricklePowerRadioBtn_CheckedChanged);
            this.frmLowPowerAPMRadioBtn.AutoSize = true;
            this.frmLowPowerAPMRadioBtn.Location = new Point(0x6b, 30);
            this.frmLowPowerAPMRadioBtn.Name = "frmLowPowerAPMRadioBtn";
            this.frmLowPowerAPMRadioBtn.Size = new Size(0x30, 0x11);
            this.frmLowPowerAPMRadioBtn.TabIndex = 2;
            this.frmLowPowerAPMRadioBtn.Text = "&APM";
            this.frmLowPowerAPMRadioBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerAPMRadioBtn.CheckedChanged += new EventHandler(this.frmLowPowerAPMRadioBtn_CheckedChanged);
            this.frmLowPowerOkBtn.Location = new Point(0x5c, 0x1db);
            this.frmLowPowerOkBtn.Name = "frmLowPowerOkBtn";
            this.frmLowPowerOkBtn.Size = new Size(0x4b, 0x17);
            this.frmLowPowerOkBtn.TabIndex = 11;
            this.frmLowPowerOkBtn.Text = "&OK";
            this.frmLowPowerOkBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerOkBtn.Click += new EventHandler(this.frmLowPowerOkBtn_Click);
            this.frmLowPowerCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmLowPowerCancelBtn.Location = new Point(0xbd, 0x1da);
            this.frmLowPowerCancelBtn.Name = "frmLowPowerCancelBtn";
            this.frmLowPowerCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmLowPowerCancelBtn.TabIndex = 12;
            this.frmLowPowerCancelBtn.Text = "&Cancel";
            this.frmLowPowerCancelBtn.UseVisualStyleBackColor = true;
            this.frmLowPowerCancelBtn.Click += new EventHandler(this.frmLowPowerCancelBtn_Click);
            this.frmLowPowerModeTab.Controls.Add(this.apmTab);
            this.frmLowPowerModeTab.Controls.Add(this.tpTab);
            this.frmLowPowerModeTab.Controls.Add(this.ptfTab);
            this.frmLowPowerModeTab.Location = new Point(0x25, 0x8a);
            this.frmLowPowerModeTab.Name = "frmLowPowerModeTab";
            this.frmLowPowerModeTab.SelectedIndex = 0;
            this.frmLowPowerModeTab.Size = new Size(0x11a, 310);
            this.frmLowPowerModeTab.TabIndex = 13;
            this.apmTab.Controls.Add(this.label16);
            this.apmTab.Controls.Add(this.apmTimeAccPriorityComboBox);
            this.apmTab.Controls.Add(this.apmTimeAccPriorityLabel);
            this.apmTab.Controls.Add(this.apmDutyCycleLabel);
            this.apmTab.Controls.Add(this.apmDutyCycleTxtBox);
            this.apmTab.Controls.Add(this.label15);
            this.apmTab.Controls.Add(this.apmMazVrtErrorLabel);
            this.apmTab.Controls.Add(this.apmPriorityComboBox);
            this.apmTab.Controls.Add(this.apmMaxVrtErrorComboBox);
            this.apmTab.Controls.Add(this.apmMaxSearchTimeLabel);
            this.apmTab.Controls.Add(this.apmMaxOffTimeLabel);
            this.apmTab.Controls.Add(this.apmPriorityLabel);
            this.apmTab.Controls.Add(this.apmMazHrzErrorLabel);
            this.apmTab.Controls.Add(this.apmTBFLabel);
            this.apmTab.Controls.Add(this.apmNumFixesLabel);
            this.apmTab.Controls.Add(this.apmMaxSearchTimeTxtBox);
            this.apmTab.Controls.Add(this.apmMaxOffTimeTxtBox);
            this.apmTab.Controls.Add(this.apmMaxHrzErrorTxtBox);
            this.apmTab.Controls.Add(this.apmTBFTxtBox);
            this.apmTab.Controls.Add(this.apmNumFixesTxtBox);
            this.apmTab.Location = new Point(4, 0x16);
            this.apmTab.Name = "apmTab";
            this.apmTab.Padding = new Padding(3);
            this.apmTab.Size = new Size(0x112, 0x11c);
            this.apmTab.TabIndex = 0;
            this.apmTab.Text = "APM";
            this.apmTab.UseVisualStyleBackColor = true;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x18, 0x1d);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x54, 13);
            this.label16.TabIndex = 10;
            this.label16.Text = "(0 = Continuous)";
            this.apmTimeAccPriorityComboBox.FormattingEnabled = true;
            this.apmTimeAccPriorityComboBox.Items.AddRange(new object[] { "No Priority", "Max Search Time", "Max Horz. Error", "User Defined" });
            this.apmTimeAccPriorityComboBox.Location = new Point(150, 0xf4);
            this.apmTimeAccPriorityComboBox.Name = "apmTimeAccPriorityComboBox";
            this.apmTimeAccPriorityComboBox.Size = new Size(100, 0x15);
            this.apmTimeAccPriorityComboBox.TabIndex = 8;
            this.apmTimeAccPriorityLabel.AutoSize = true;
            this.apmTimeAccPriorityLabel.Location = new Point(0x18, 0xf8);
            this.apmTimeAccPriorityLabel.Name = "apmTimeAccPriorityLabel";
            this.apmTimeAccPriorityLabel.Size = new Size(0x56, 13);
            this.apmTimeAccPriorityLabel.TabIndex = 8;
            this.apmTimeAccPriorityLabel.Text = "Time Acc Priority";
            this.apmDutyCycleLabel.AutoSize = true;
            this.apmDutyCycleLabel.Location = new Point(0x18, 0x4e);
            this.apmDutyCycleLabel.Name = "apmDutyCycleLabel";
            this.apmDutyCycleLabel.Size = new Size(0x4b, 13);
            this.apmDutyCycleLabel.TabIndex = 7;
            this.apmDutyCycleLabel.Text = "Duty Cycle (%)";
            this.apmDutyCycleTxtBox.Location = new Point(150, 0x4a);
            this.apmDutyCycleTxtBox.Name = "apmDutyCycleTxtBox";
            this.apmDutyCycleTxtBox.Size = new Size(100, 20);
            this.apmDutyCycleTxtBox.TabIndex = 2;
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x18, 0x71);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x44, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "(0 = No Max)";
            this.apmMazVrtErrorLabel.AutoSize = true;
            this.apmMazVrtErrorLabel.Location = new Point(0x18, 0x86);
            this.apmMazVrtErrorLabel.Name = "apmMazVrtErrorLabel";
            this.apmMazVrtErrorLabel.Size = new Size(0x58, 13);
            this.apmMazVrtErrorLabel.TabIndex = 3;
            this.apmMazVrtErrorLabel.Text = "Max Vrt. Error (m)";
            this.apmPriorityComboBox.FormattingEnabled = true;
            this.apmPriorityComboBox.Items.AddRange(new object[] { "Time", "Power", "User Defined" });
            this.apmPriorityComboBox.Location = new Point(150, 0x9f);
            this.apmPriorityComboBox.Name = "apmPriorityComboBox";
            this.apmPriorityComboBox.Size = new Size(100, 0x15);
            this.apmPriorityComboBox.TabIndex = 5;
            this.apmMaxVrtErrorComboBox.FormattingEnabled = true;
            this.apmMaxVrtErrorComboBox.Items.AddRange(new object[] { "< 1 meter", "< 5 meters", "< 10 meters", "< 20 meters", "< 40 meters", "< 80 meters", "< 160 meters", "No Maximum", "User Defined" });
            this.apmMaxVrtErrorComboBox.Location = new Point(150, 130);
            this.apmMaxVrtErrorComboBox.Name = "apmMaxVrtErrorComboBox";
            this.apmMaxVrtErrorComboBox.Size = new Size(100, 0x15);
            this.apmMaxVrtErrorComboBox.TabIndex = 4;
            this.apmMaxSearchTimeLabel.AutoSize = true;
            this.apmMaxSearchTimeLabel.Location = new Point(0x18, 220);
            this.apmMaxSearchTimeLabel.Name = "apmMaxSearchTimeLabel";
            this.apmMaxSearchTimeLabel.Size = new Size(0x70, 13);
            this.apmMaxSearchTimeLabel.TabIndex = 1;
            this.apmMaxSearchTimeLabel.Text = "Max Search Time (ms)";
            this.apmMaxOffTimeLabel.AutoSize = true;
            this.apmMaxOffTimeLabel.Location = new Point(0x18, 0xc0);
            this.apmMaxOffTimeLabel.Name = "apmMaxOffTimeLabel";
            this.apmMaxOffTimeLabel.Size = new Size(0x5c, 13);
            this.apmMaxOffTimeLabel.TabIndex = 1;
            this.apmMaxOffTimeLabel.Text = "Max Off Time (ms)";
            this.apmPriorityLabel.AutoSize = true;
            this.apmPriorityLabel.Location = new Point(0x18, 0xa3);
            this.apmPriorityLabel.Name = "apmPriorityLabel";
            this.apmPriorityLabel.Size = new Size(0x26, 13);
            this.apmPriorityLabel.TabIndex = 1;
            this.apmPriorityLabel.Text = "Priority";
            this.apmMazHrzErrorLabel.AutoSize = true;
            this.apmMazHrzErrorLabel.Location = new Point(0x18, 100);
            this.apmMazHrzErrorLabel.Name = "apmMazHrzErrorLabel";
            this.apmMazHrzErrorLabel.Size = new Size(0x5b, 13);
            this.apmMazHrzErrorLabel.TabIndex = 1;
            this.apmMazHrzErrorLabel.Text = "Max Hrz. Error (m)";
            this.apmTBFLabel.AutoSize = true;
            this.apmTBFLabel.Location = new Point(0x18, 50);
            this.apmTBFLabel.Name = "apmTBFLabel";
            this.apmTBFLabel.Size = new Size(0x74, 13);
            this.apmTBFLabel.TabIndex = 1;
            this.apmTBFLabel.Text = "Time Between Fixes (s)";
            this.apmNumFixesLabel.AutoSize = true;
            this.apmNumFixesLabel.Location = new Point(0x18, 0x10);
            this.apmNumFixesLabel.Name = "apmNumFixesLabel";
            this.apmNumFixesLabel.Size = new Size(0x38, 13);
            this.apmNumFixesLabel.TabIndex = 1;
            this.apmNumFixesLabel.Text = "Num Fixes";
            this.apmMaxSearchTimeTxtBox.Location = new Point(150, 0xd8);
            this.apmMaxSearchTimeTxtBox.Name = "apmMaxSearchTimeTxtBox";
            this.apmMaxSearchTimeTxtBox.Size = new Size(100, 20);
            this.apmMaxSearchTimeTxtBox.TabIndex = 7;
            this.apmMaxOffTimeTxtBox.Location = new Point(150, 0xbc);
            this.apmMaxOffTimeTxtBox.Name = "apmMaxOffTimeTxtBox";
            this.apmMaxOffTimeTxtBox.Size = new Size(100, 20);
            this.apmMaxOffTimeTxtBox.TabIndex = 6;
            this.apmMaxHrzErrorTxtBox.Location = new Point(150, 0x66);
            this.apmMaxHrzErrorTxtBox.Name = "apmMaxHrzErrorTxtBox";
            this.apmMaxHrzErrorTxtBox.Size = new Size(100, 20);
            this.apmMaxHrzErrorTxtBox.TabIndex = 3;
            this.apmTBFTxtBox.Location = new Point(150, 0x2e);
            this.apmTBFTxtBox.Name = "apmTBFTxtBox";
            this.apmTBFTxtBox.Size = new Size(100, 20);
            this.apmTBFTxtBox.TabIndex = 1;
            this.apmNumFixesTxtBox.Location = new Point(150, 0x12);
            this.apmNumFixesTxtBox.Name = "apmNumFixesTxtBox";
            this.apmNumFixesTxtBox.Size = new Size(100, 20);
            this.apmNumFixesTxtBox.TabIndex = 0;
            this.tpTab.Controls.Add(this.tpOnTimeCmbBox);
            this.tpTab.Controls.Add(this.tpUpdateRateCmbBox);
            this.tpTab.Controls.Add(this.tpUpdateRateLabel);
            this.tpTab.Controls.Add(this.tpMaxSearchTimeLabel);
            this.tpTab.Controls.Add(this.tpMaxOffTimeLabel);
            this.tpTab.Controls.Add(this.tpMaxSearchTimeTxtBox);
            this.tpTab.Controls.Add(this.tpMaxOffTimeTxtBox);
            this.tpTab.Controls.Add(this.tpOntimeLabel);
            this.tpTab.Location = new Point(4, 0x16);
            this.tpTab.Name = "tpTab";
            this.tpTab.Size = new Size(0x112, 0x11c);
            this.tpTab.TabIndex = 1;
            this.tpTab.Text = "TP";
            this.tpTab.UseVisualStyleBackColor = true;
            this.tpOnTimeCmbBox.FormattingEnabled = true;
            this.tpOnTimeCmbBox.Items.AddRange(new object[] { "100", "200", "300", "400", "500", "600", "700", "800", "900" });
            this.tpOnTimeCmbBox.Location = new Point(0x93, 0x3f);
            this.tpOnTimeCmbBox.MaxDropDownItems = 9;
            this.tpOnTimeCmbBox.Name = "tpOnTimeCmbBox";
            this.tpOnTimeCmbBox.Size = new Size(100, 0x15);
            this.tpOnTimeCmbBox.TabIndex = 2;
            this.tpUpdateRateCmbBox.FormattingEnabled = true;
            this.tpUpdateRateCmbBox.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.tpUpdateRateCmbBox.Location = new Point(0x93, 0x24);
            this.tpUpdateRateCmbBox.MaxDropDownItems = 10;
            this.tpUpdateRateCmbBox.Name = "tpUpdateRateCmbBox";
            this.tpUpdateRateCmbBox.Size = new Size(100, 0x15);
            this.tpUpdateRateCmbBox.TabIndex = 1;
            this.tpUpdateRateLabel.AutoSize = true;
            this.tpUpdateRateLabel.Location = new Point(0x1c, 40);
            this.tpUpdateRateLabel.Name = "tpUpdateRateLabel";
            this.tpUpdateRateLabel.Size = new Size(0x5e, 13);
            this.tpUpdateRateLabel.TabIndex = 10;
            this.tpUpdateRateLabel.Text = "Update Rate (sec)";
            this.tpMaxSearchTimeLabel.AutoSize = true;
            this.tpMaxSearchTimeLabel.Location = new Point(0x1c, 0x77);
            this.tpMaxSearchTimeLabel.Name = "tpMaxSearchTimeLabel";
            this.tpMaxSearchTimeLabel.Size = new Size(0x70, 13);
            this.tpMaxSearchTimeLabel.TabIndex = 8;
            this.tpMaxSearchTimeLabel.Text = "Max Search Time (ms)";
            this.tpMaxOffTimeLabel.AutoSize = true;
            this.tpMaxOffTimeLabel.Location = new Point(0x1c, 0x5d);
            this.tpMaxOffTimeLabel.Name = "tpMaxOffTimeLabel";
            this.tpMaxOffTimeLabel.Size = new Size(0x5c, 13);
            this.tpMaxOffTimeLabel.TabIndex = 9;
            this.tpMaxOffTimeLabel.Text = "Max Off Time (ms)";
            this.tpMaxSearchTimeTxtBox.Location = new Point(0x93, 0x73);
            this.tpMaxSearchTimeTxtBox.Name = "tpMaxSearchTimeTxtBox";
            this.tpMaxSearchTimeTxtBox.Size = new Size(100, 20);
            this.tpMaxSearchTimeTxtBox.TabIndex = 4;
            this.tpMaxOffTimeTxtBox.Location = new Point(0x93, 0x59);
            this.tpMaxOffTimeTxtBox.Name = "tpMaxOffTimeTxtBox";
            this.tpMaxOffTimeTxtBox.Size = new Size(100, 20);
            this.tpMaxOffTimeTxtBox.TabIndex = 3;
            this.tpOntimeLabel.AutoSize = true;
            this.tpOntimeLabel.Location = new Point(0x1b, 0x43);
            this.tpOntimeLabel.Name = "tpOntimeLabel";
            this.tpOntimeLabel.Size = new Size(0x45, 13);
            this.tpOntimeLabel.TabIndex = 4;
            this.tpOntimeLabel.Text = "On Time (ms)";
            this.ptfTab.Controls.Add(this.ptfMaxOffTimeLabel);
            this.ptfTab.Controls.Add(this.ptfMaxOffTimeTxtBox);
            this.ptfTab.Controls.Add(this.ptfMaxSearchTimeLabel);
            this.ptfTab.Controls.Add(this.ptfPeriodLabel);
            this.ptfTab.Controls.Add(this.ptfMaxSearchTimeTxtBox);
            this.ptfTab.Controls.Add(this.ptfPeriodTxtBox);
            this.ptfTab.Location = new Point(4, 0x16);
            this.ptfTab.Name = "ptfTab";
            this.ptfTab.Size = new Size(0x112, 0x11c);
            this.ptfTab.TabIndex = 2;
            this.ptfTab.Text = "PTF";
            this.ptfTab.UseVisualStyleBackColor = true;
            this.ptfMaxOffTimeLabel.AutoSize = true;
            this.ptfMaxOffTimeLabel.Location = new Point(0x18, 0x5e);
            this.ptfMaxOffTimeLabel.Name = "ptfMaxOffTimeLabel";
            this.ptfMaxOffTimeLabel.Size = new Size(0x5c, 13);
            this.ptfMaxOffTimeLabel.TabIndex = 15;
            this.ptfMaxOffTimeLabel.Text = "Max Off Time (ms)";
            this.ptfMaxOffTimeTxtBox.Location = new Point(150, 90);
            this.ptfMaxOffTimeTxtBox.Name = "ptfMaxOffTimeTxtBox";
            this.ptfMaxOffTimeTxtBox.Size = new Size(100, 20);
            this.ptfMaxOffTimeTxtBox.TabIndex = 2;
            this.ptfMaxSearchTimeLabel.AutoSize = true;
            this.ptfMaxSearchTimeLabel.Location = new Point(0x18, 0x44);
            this.ptfMaxSearchTimeLabel.Name = "ptfMaxSearchTimeLabel";
            this.ptfMaxSearchTimeLabel.Size = new Size(0x70, 13);
            this.ptfMaxSearchTimeLabel.TabIndex = 12;
            this.ptfMaxSearchTimeLabel.Text = "Max Search Time (ms)";
            this.ptfPeriodLabel.AutoSize = true;
            this.ptfPeriodLabel.Location = new Point(0x18, 0x2a);
            this.ptfPeriodLabel.Name = "ptfPeriodLabel";
            this.ptfPeriodLabel.Size = new Size(110, 13);
            this.ptfPeriodLabel.TabIndex = 13;
            this.ptfPeriodLabel.Text = "Push To Fix Period (s)";
            this.ptfMaxSearchTimeTxtBox.Location = new Point(150, 0x40);
            this.ptfMaxSearchTimeTxtBox.Name = "ptfMaxSearchTimeTxtBox";
            this.ptfMaxSearchTimeTxtBox.Size = new Size(100, 20);
            this.ptfMaxSearchTimeTxtBox.TabIndex = 1;
            this.ptfPeriodTxtBox.Location = new Point(150, 0x26);
            this.ptfPeriodTxtBox.Name = "ptfPeriodTxtBox";
            this.ptfPeriodTxtBox.Size = new Size(100, 20);
            this.ptfPeriodTxtBox.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x17, 0xbd);
            this.label1.Name = "label1";
            this.label1.Size = new Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Max Search Time";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x17, 0xa3);
            this.label2.Name = "label2";
            this.label2.Size = new Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max Off Time";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x17, 0x39);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x66, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Time Between Fixes";
            this.textBox1.Location = new Point(0x87, 0xb9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x17, 0x6d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x47, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Max Vrt. Error";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "Time", "Power", "User Defined" });
            this.comboBox1.Location = new Point(0x87, 0x84);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(100, 0x15);
            this.comboBox1.TabIndex = 2;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] { "< 1 meter", "< 5 meters", "< 10 meters", "< 20 meters", "< 40 meters", "< 80 meters", "< 160 meters", "No Maximum", "User Defined" });
            this.comboBox2.Location = new Point(0x87, 0x69);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(100, 0x15);
            this.comboBox2.TabIndex = 2;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x17, 0x88);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x26, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Priority";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x17, 0x53);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x4a, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Max Hrz. Error";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x17, 0x1f);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x38, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Num Fixes";
            this.textBox2.Location = new Point(0x87, 0x9f);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(100, 20);
            this.textBox2.TabIndex = 0;
            this.textBox3.Location = new Point(0x87, 0x4f);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Size(100, 20);
            this.textBox3.TabIndex = 0;
            this.textBox4.Location = new Point(0x87, 0x35);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Size(100, 20);
            this.textBox4.TabIndex = 0;
            this.textBox5.Location = new Point(0x87, 0x1b);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Size(100, 20);
            this.textBox5.TabIndex = 0;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x17, 0xbd);
            this.label8.Name = "label8";
            this.label8.Size = new Size(90, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Max Search Time";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x17, 0xa3);
            this.label9.Name = "label9";
            this.label9.Size = new Size(70, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Max Off Time";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x17, 0x39);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x66, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Time Between Fixes";
            this.textBox6.Location = new Point(0x87, 0xb9);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Size(100, 20);
            this.textBox6.TabIndex = 0;
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x17, 0x6d);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x47, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Max Vrt. Error";
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] { "Time", "Power", "User Defined" });
            this.comboBox3.Location = new Point(0x87, 0x84);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new Size(100, 0x15);
            this.comboBox3.TabIndex = 2;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] { "< 1 meter", "< 5 meters", "< 10 meters", "< 20 meters", "< 40 meters", "< 80 meters", "< 160 meters", "No Maximum", "User Defined" });
            this.comboBox4.Location = new Point(0x87, 0x69);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new Size(100, 0x15);
            this.comboBox4.TabIndex = 2;
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x17, 0x88);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x26, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Priority";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x17, 0x53);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x4a, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Max Hrz. Error";
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x17, 0x1f);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x38, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Num Fixes";
            this.textBox7.Location = new Point(0x87, 0x9f);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Size(100, 20);
            this.textBox7.TabIndex = 0;
            this.textBox8.Location = new Point(0x87, 0x4f);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Size(100, 20);
            this.textBox8.TabIndex = 0;
            this.textBox9.Location = new Point(0x87, 0x35);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Size(100, 20);
            this.textBox9.TabIndex = 0;
            this.textBox10.Location = new Point(0x87, 0x1b);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Size(100, 20);
            this.textBox10.TabIndex = 0;
            base.AcceptButton = this.frmLowPowerOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmLowPowerCancelBtn;
            base.ClientSize = new Size(0x164, 0x21b);
            base.Controls.Add(this.frmLowPowerModeTab);
            base.Controls.Add(this.frmLowPowerCancelBtn);
            base.Controls.Add(this.frmLowPowerOkBtn);
            base.Controls.Add(this.frmLowPowerModeGroupBox);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmLowPower";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Power Mode";
            base.Load += new EventHandler(this.frmLowPower_Load);
            this.frmLowPowerModeGroupBox.ResumeLayout(false);
            this.frmLowPowerModeGroupBox.PerformLayout();
            this.frmLowPowerModeTab.ResumeLayout(false);
            this.apmTab.ResumeLayout(false);
            this.apmTab.PerformLayout();
            this.tpTab.ResumeLayout(false);
            this.tpTab.PerformLayout();
            this.ptfTab.ResumeLayout(false);
            this.ptfTab.PerformLayout();
            base.ResumeLayout(false);
        }

        private void SendAPMTimerHandler(object source, ElapsedEventArgs e)
        {
            this.SendLowPower();
        }

        private void SendLowPower()
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            manager.comm.LowPowerParams = this._lowPowerSettings;
                            manager.comm.RxCtrl.SetPowerMode(true);
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.comm.LowPowerParams = this._lowPowerSettings;
                this.comm.RxCtrl.SetPowerMode(true);
                this.frmLowPowerSaveSettings();
            }
            this.frmLowPowerSaveSettings();
        }

        private void setAPM_AR_Parameters_4e(ref CommunicationManager targetComm)
        {
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoReplyHWCfg = true;
            targetComm.AutoReplyCtrl.HWCfgCtrl.Reply = true;
            targetComm.AutoReplyCtrl.HWCfgCtrl.PreciseTimeEnabled = Convert.ToByte("0");
            targetComm.AutoReplyCtrl.HWCfgCtrl.PreciseTimeDirection = Convert.ToByte("1");
            targetComm.AutoReplyCtrl.HWCfgCtrl.FreqAidEnabled = Convert.ToByte("0");
            targetComm.AutoReplyCtrl.HWCfgCtrl.RTCAvailabe = Convert.ToByte("1");
            targetComm.AutoReplyCtrl.HWCfgCtrl.RTCSource = Convert.ToByte("1");
            targetComm.AutoReplyCtrl.HWCfgCtrl.CoarseTimeEnabled = Convert.ToByte("0");
            targetComm.AutoReplyCtrl.FreqTransferCtrl.Reply = false;
            targetComm.AutoReplyCtrl.FreqTransferCtrl.Reject = true;
            targetComm.AutoReplyCtrl.ApproxPositionCtrl.Reply = true;
            targetComm.AutoReplyCtrl.ApproxPositionCtrl.Reject = true;
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoReplyApproxPos = true;
            targetComm.AutoReplyCtrl.TimeTransferCtrl.Reply = true;
            targetComm.AutoReplyCtrl.TimeTransferCtrl.Reject = true;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = Convert.ToByte("1");
            targetComm.AutoReplyCtrl.PositionRequestCtrl.NumFixes = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.TimeBtwFixes = 1;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.VertErrMax = 7;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority = Convert.ToByte(0);
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromTTB = false;
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromFile = false;
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoAid_ExtEph_fromFile = false;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.EphSource = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.EphReply = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.AcqAssistSource = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.AcqAssistReply = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.AlmSource = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.AlmReply = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.NavBitSource = 0;
            targetComm.AutoReplyCtrl.PositionRequestCtrl.NavBitReply = 0;
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoPosReq = true;
            targetComm.AutoReplyCtrl.AutoReplyParams.AutoReply = true;
        }

        internal void updateConfigList(string updatedData)
        {
            string str;
            if ((((str = this.inputForm.UpdateType) == null) || (str == "POWER_DUTY_CYCLE")) || (!(str == "TIME_DUTY_PRIORITY") && !(str == "MAX_ON_TIME")))
            {
            }
        }
    }
}

