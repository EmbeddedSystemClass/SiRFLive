﻿namespace SiRFLive.GUI.Commmunication
{
    using AardvarkI2CClassLibrary;
    using CommMgrClassLibrary;
    using CommonClassLibrary;
    using CommonUtilsClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class frmCommSettings : Form
    {
        private bool _advancedRS232SettingsFlag;
        private bool _advancedRxSettingsFlag;
        private ArrayList _backupTabPages = new ArrayList();
        private bool _errorDetected;
        private Label ai3Version;
        private CheckBox autoBaudChkBox;
        private ComboBox cboAI3Version;
        private ComboBox cboBaud;
        private ComboBox cboData;
        private ComboBox cboFlowControl;
        private ComboBox cboFVersion;
        private ComboBox cboParity;
        private ComboBox cboPort;
        private ComboBox cboProtocols;
        private ComboBox cboReadBuffer;
        private ComboBox cboStop;
        private CheckBox checkBox_SlaveMode;
        public CommMgrClass CMC;
        private TextBox COMClientIPAddrTextBox;
        private GroupBox COMClientIPGroupBox;
        private TabControl COMDeviceTabControl;
        private CommunicationManager comm;
        private IContainer components;
        private GroupBox COMServerGroupBox;
        private TextBox COMServerIPTextBox;
        private RadioButton COMTcpIPClientRadioButton;
        private GroupBox COMTcpIpModeGroupBox;
        private RadioButton COMTcpIPServerRadioButton;
        private GroupBox EEgroupBoxCGEE;
        private GroupBox EEgroupBoxSGEE;
        private Label EElabel_AuthCode;
        private Label EElabel_Banktime;
        private Label EElabel_Daynum;
        private Label EElabel_Select;
        private Label EElabel_ServerName;
        private Label EElabel_ServerPort;
        private ComboBox extEphComboBox_EEDay;
        private ComboBox extEphComboBox_Select;
        private ComboBox extEphComboBox_ServerName;
        private TextBox extEphEditBox_AuthCode;
        private TextBox extEphEditBox_BankTime;
        private TextBox extEphEditBox_ServerPort;
        private TabPage ExtEphTabPage;
        private Label frmCommOpenBufferSizeLabel;
        private TextBox frmCommOpenBufferSizeTxtBox;
        private Label frmCommOpenProtocolsLabel;
        private GroupBox frmCommOpenRxTypeGrpBox;
        private Button frmCommSettigsRS232AdvancedSettingsBtn;
        private Button frmCommSettingsAdvancedHostAppBtn;
        private Button frmCommSettingsAdvancedRxSettingsBtn;
        private Button frmCommSettingsCancelBtn;
        private GroupBox frmCommSettingsConTypeGrpBox;
        private Button frmCommSettingsOkBtn;
        private ComboBox frmCommSettingsProductFamilyComboBox;
        private GroupBox frmCommSettingsRS232AdvanceSettingsGrpBox;
        private Label frmCommSettingsRxNameLabel;
        private TextBox frmCommSettingsRxNameTxtBox;
        private GroupBox frmCommSettingsRxParamsGrpBox;
        private RadioButton frmComSettingsGSWRadioBtn;
        private RadioButton frmComSettingsSLCRadioBtn;
        private RadioButton frmComSettingsTTBRadioBtn;
        private Label fVersion;
        private GroupBox groupBox3;
        private ComboBox gsd4eOnOffPortComboBox;
        private Label gsd4eOnOffPortLabel;
        private ComboBox hostAppCboHostPair1;
        private ComboBox hostAppCboHostPair2;
        private ComboBox hostAppCboResetPort;
        private ComboBox hostAppCboServerTCPIPPort;
        private ComboBox hostAppCboTCPIPClientPort;
        private ComboBox hostAppCboTrackerPort;
        private CheckBox hostAppEEChkBox;
        private RadioButton hostAppI2CRadioBtn;
        private ComboBox hostAppLDOModeComboBox;
        private Label hostAppLDOModeLabel;
        private ComboBox hostAppLnaComboBox;
        private Label hostAppLnaTypeLabel;
        private Label hostAppPairLabel;
        private Label hostAppResetPortLabel;
        private RadioButton hostAppRS232RadioBtn;
        private CheckBox hostAppRunHostChkBox;
        private Button hostAppSWDirBrowseBtn;
        private Label hostAppSWDirLabel;
        private TextBox hostAppSWDirTxtBox;
        private TabPage HostAppTabPage;
        private RadioButton hostAppTCPIPRadioBtn;
        private ComboBox hostAppTCXOFreqComboBox;
        private Label hostAppTCXOFreqLabel;
        private Label hostAppTrackerPortLabel;
        private CheckBox hostAppVersionCheckBox;
        private Button I2C_DetectBtn;
        private TextBox I2C_MasterTxtBx;
        private TextBox I2C_PortTxtBox;
        private TextBox I2C_PortTxtBoxMaster;
        private Label I2C_SerialNum;
        private Label I2C_SerialNumMaster;
        private TextBox I2C_SlaveTxtBx;
        private TabPage I2CTabPage;
        private Label Label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ComboBox onOffActionComboBox;
        private Label pulseOnOffPortLabel;
        private RadioButton rdoCSV;
        private RadioButton rdoGP2;
        private RadioButton rdoHex;
        private RadioButton rdoSSB;
        private RadioButton rdoText;
        private TabPage RS232TabPage;
        private TabPage TcpIpTabPage;

        public frmCommSettings(ref CommunicationManager parentCommWindow)
        {
            CommonUtilsClass class2 = new CommonUtilsClass();
            this.InitializeComponent();
            this.CommWindow = parentCommWindow;
            if (this.comm != null)
            {
                this.frmCommOpenBufferSizeTxtBox.Text = class2.DisplayBuffer.ToString();
                this.CMC = this.comm.CMC;
            }
        }

        private void addRemoveTabpage(bool b_addtab, int tIdx, string tName)
        {
            if (!b_addtab)
            {
                if (this.COMDeviceTabControl.TabPages.ContainsKey(tName))
                {
                    this.COMDeviceTabControl.TabPages.RemoveByKey(tName);
                }
            }
            else
            {
                int num = tIdx - 1;
                if (num < 0)
                {
                    num = 0;
                }
                if (!this.COMDeviceTabControl.TabPages.ContainsKey(tName))
                {
                    this.COMDeviceTabControl.SelectedIndex = num;
                    this.COMDeviceTabControl.TabPages.Add((TabPage) this._backupTabPages[tIdx]);
                }
            }
        }

        private void cboFlowControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.FlowControl = this.cboFlowControl.SelectedIndex;
            }
        }

        private void cboPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.hostAppCboHostPair2.SelectedIndex = this.cboPort.SelectedIndex;
        }

        private void cboProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = this.cboProtocols.SelectedItem.ToString();
            if (str != null)
            {
                if (!(str == "SSB"))
                {
                    if (str == "NMEA")
                    {
                        this.frmComSettingsSLCRadioBtn.Checked = true;
                        this.frmComSettingsSLCRadioBtn.Enabled = false;
                        this.frmComSettingsTTBRadioBtn.Enabled = false;
                        this.frmComSettingsGSWRadioBtn.Enabled = false;
                        this.cboFVersion.Enabled = false;
                        this.cboAI3Version.Enabled = false;
                        this.cboFVersion.SelectedIndex = 1;
                        this.cboAI3Version.SelectedIndex = 2;
                    }
                    else if (str == "OSP")
                    {
                        this.frmComSettingsSLCRadioBtn.Checked = true;
                        this.frmComSettingsSLCRadioBtn.Enabled = false;
                        this.frmComSettingsTTBRadioBtn.Enabled = false;
                        this.frmComSettingsGSWRadioBtn.Enabled = false;
                        this.cboFVersion.Enabled = false;
                        this.cboAI3Version.Enabled = false;
                        this.cboFVersion.Text = "1.0";
                        this.cboAI3Version.Text = "1.0";
                        this.cboFVersion.SelectedIndex = 0;
                        this.cboAI3Version.SelectedIndex = 0;
                    }
                }
                else if (!clsGlobal.IsMarketingUser())
                {
                    this.frmComSettingsSLCRadioBtn.Enabled = true;
                    this.frmComSettingsTTBRadioBtn.Enabled = true;
                    this.frmComSettingsGSWRadioBtn.Enabled = true;
                    this.cboFVersion.Enabled = true;
                    this.cboAI3Version.Enabled = true;
                    this.cboFVersion.SelectedIndex = 1;
                    this.cboAI3Version.SelectedIndex = 2;
                }
            }
            if (this.cboProtocols.SelectedItem.ToString() == "NMEA")
            {
                this.rdoText.Checked = true;
            }
            else
            {
                this.rdoCSV.Checked = true;
            }
        }

        private void cboReadBuffer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.ReadBuffer = Convert.ToInt32(this.cboReadBuffer.Text);
            }
        }

        private void COMDeviceTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.COMDeviceTabControl.SelectedIndex == 4)
            {
                switch (this.extEphComboBox_Select.SelectedIndex)
                {
                    case 0:
                        this.extEphComboBox_ServerName.Enabled = false;
                        this.extEphEditBox_ServerPort.Enabled = false;
                        this.extEphEditBox_AuthCode.Enabled = false;
                        this.extEphComboBox_EEDay.Enabled = false;
                        this.extEphEditBox_BankTime.Enabled = false;
                        return;

                    case 1:
                        return;

                    case 2:
                        this.extEphEditBox_BankTime.Enabled = false;
                        return;
                }
            }
        }

        private void COMTcpIPClientRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.COMTcpIPClientRadioButton.Checked)
            {
                this.COMTcpIPServerRadioButton.Checked = false;
                this.COMServerGroupBox.Enabled = false;
                this.COMClientIPGroupBox.Enabled = true;
            }
        }

        private void COMTcpIPServerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.COMTcpIPServerRadioButton.Checked)
            {
                this.COMTcpIPClientRadioButton.Checked = false;
                this.COMServerGroupBox.Enabled = true;
                this.COMClientIPGroupBox.Enabled = false;
            }
        }

        public static void CreateGSD4tConfigFile(string fullHostAppPath, ref HostAppConfigParams hostAppConfigParams)
        {
            DirectoryInfo parent = Directory.GetParent(fullHostAppPath);
            if (hostAppConfigParams.HostSWFilePath == fullHostAppPath)
            {
                parent = Directory.GetParent(parent.FullName);
            }
            string str = @"\" + hostAppConfigParams.SiRFLiveInterfacePortName;
            string path = string.Empty;
            if (parent.FullName.Contains(str))
            {
                path = parent.FullName;
            }
            else
            {
                path = parent.FullName + str;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileInfo info2 = new FileInfo(fullHostAppPath);
            hostAppConfigParams.HostSWFilePath = path + @"\" + info2.Name;
            if (!File.Exists(hostAppConfigParams.HostSWFilePath))
            {
                File.Copy(fullHostAppPath, hostAppConfigParams.HostSWFilePath, true);
            }
            hostAppConfigParams.HostAppCfgFilePath = path + @"\sirfLiveHostConfig.txt";
            StreamWriter writer = new StreamWriter(hostAppConfigParams.HostAppCfgFilePath, false);
            FileInfo[] files = parent.GetFiles("*.bin");
            StringBuilder builder = new StringBuilder();
            if (files.Length > 0)
            {
                builder.Append("program_file ");
                builder.Append(files[0].FullName);
                builder.Append("\r\n");
            }
            files = new DirectoryInfo(path).GetFiles("NVM*");
            if (files.Length > 0)
            {
                foreach (FileInfo info4 in files)
                {
                    try
                    {
                        File.Delete(info4.FullName);
                    }
                    catch
                    {
                    }
                }
            }
            builder.Append("tracker_port_select ");
            builder.Append(string.Format("{0}\r\n", hostAppConfigParams.TrackerPortSelect));
            builder.Append(string.Format("uart_baud_rate {0}\r\n", hostAppConfigParams.BaudRate));
            builder.Append(@"tracker_port \\.\");
            builder.Append(hostAppConfigParams.TrackerPort);
            builder.Append("\r\n");
            builder.Append(@"on_off_port \\.\");
            builder.Append(hostAppConfigParams.ResetPort);
            builder.Append(string.Format("\r\non_off_line_usage {0}\r\next_sreset_n_line_usage {1}\r\n", hostAppConfigParams.OnOffLineUsage, hostAppConfigParams.ExtSResetNLineUsage));
            builder.Append(string.Format("ref_clk_frequency {0}\r\n", hostAppConfigParams.DefaultTCXOFreq));
            builder.Append(string.Format("ref_clk_warmup_delay {0}\r\ndebug_settings {1}\r\n", hostAppConfigParams.WarmupDelay, hostAppConfigParams.DebugSettings));
            if (hostAppConfigParams.LNAType == 0)
            {
                builder.Append("lna_type internal\r\n");
            }
            else
            {
                builder.Append("lna_type external\r\n");
            }
            if (hostAppConfigParams.LDOMode == 0)
            {
                builder.Append("internal_backup_LDO_mode disable\r\n");
            }
            else
            {
                builder.Append("internal_backup_LDO_mode enable\r\n");
            }
            builder.Append("\n// I2C DR GPIO settings\r\n");
            builder.Append("io_pin_configuration_mode enable\r\n");
            builder.Append("\n// set the enable bit (0x04) on both GPIO0 and GPIO1, mode = 0 (DR_I2C)\r\n");
            builder.Append("// [0:1] 0x000 = I2C DR mode\r\n");
            builder.Append("// [2:5] 0x03C = each power state enable bit\r\n");
            builder.Append("// [6:9] 0x3C0 = each power state DS setting\r\n");
            builder.Append("// |= 0x3FC = 1020\r\n");
            builder.Append("io_pin_0_configuration 0x3FC\r\n");
            builder.Append("io_pin_1_configuration 0x3FC\r\n");
            builder.Append("io_pin_3_configuration 0x003C");
            writer.WriteLine(builder.ToString());
            writer.Close();
        }

        public static void CreateGSD4tMEMSConfigFile(string fullHostAppPath, ref HostAppConfigParams hostAppConfigParams)
        {
            DirectoryInfo parent = Directory.GetParent(fullHostAppPath);
            if (hostAppConfigParams.HostSWFilePath == fullHostAppPath)
            {
                parent = Directory.GetParent(parent.FullName);
            }
            string str = @"\" + hostAppConfigParams.SiRFLiveInterfacePortName;
            string path = string.Empty;
            if (parent.FullName.Contains(str))
            {
                path = parent.FullName;
            }
            else
            {
                path = parent.FullName + str;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileInfo info2 = new FileInfo(fullHostAppPath);
            hostAppConfigParams.HostSWFilePath = path + @"\" + info2.Name;
            if (!File.Exists(hostAppConfigParams.HostSWFilePath))
            {
                File.Copy(fullHostAppPath, hostAppConfigParams.HostSWFilePath, true);
            }
            hostAppConfigParams.HostAppMEMSCfgPath = path + @"\SensCfg4t.txt";
            if (!File.Exists(hostAppConfigParams.HostAppMEMSCfgPath))
            {
                string str3 = clsGlobal.InstalledDirectory + @"\Config\SensCfg4t.txt";
                if (File.Exists(str3))
                {
                    File.Copy(str3, hostAppConfigParams.HostAppMEMSCfgPath, true);
                }
                else
                {
                    StreamWriter writer = new StreamWriter(hostAppConfigParams.HostAppMEMSCfgPath, false);
                    FileInfo[] files = parent.GetFiles("*.bin");
                    StringBuilder builder = new StringBuilder();
                    if (files.Length > 0)
                    {
                        builder.Append("program_file ");
                        builder.Append(files[0].FullName);
                        builder.Append("\r\n");
                    }
                    builder.Append("// Sensors Configuration (ALL VALUES MUST BE IN HEX FORMAT WITH EXACT NUMBER OF DIGITS. EX: 1 BYTE: 0x01, 2 BYTES: 0x0001)\r\n");
                    builder.Append("0x02    // Number of Sensors (1 - 4)\r\n");
                    builder.Append("0x02    // I2C bus speed (0 - Low, 1 - Standard, 2 - Fast, 3 - Fast_Plus, 4 - High)\r\n");
                    builder.Append("\n// Configuration for Sensor #1 (AK8974/AMI304)\r\n");
                    builder.Append("0x000E  // Sensor I2C Address (7 or 10 bit)\r\n");
                    builder.Append("0x02    // Sensor Type (1 - Accel, 2 - Magn, 3 - Press, 4 - Gyro, 5 - Accel_Gyro, 6 - Accel_Magn, 7 - Gyro_Magn, 8 - Accel_Magn_Gyro)\r\n");
                    builder.Append("0x00    // Initialization period after power up for Sensor, unit: 10ms\r\n");
                    builder.Append("0xCD    // Data type (bit 0), Number of bytes to read from each register (bits 1-3) and Data Resolution (bits 4-7)\r\n");
                    builder.Append("0x05    // Sample rate: 1- 1hz, 2- 2hz, 3- 5hz, 4- 10hz, 5- 25hz, 6- 50hz, 7- 100Hz\r\n");
                    builder.Append("0x03    // Send Rate:   1- 1hz, 2- 2hz, 3- 5hz, 4- 10hz, 5- 25hz, 6- 50hz, 7- 100Hz\r\n");
                    builder.Append("0x00    // Data decimation method (bits 0-2): 0- raw, 1- averaging, 2- sliding median\r\n");
                    builder.Append("0x20    // Acquisition time delay, unit: 10us\r\n");
                    builder.Append("0x01    // Number of sensor read registers\r\n");
                    builder.Append("0x01    // Measurement State: 0 - auto (sensor configured) 1 - forced (sw controlling)  was 0x00\r\n");
                    builder.Append("  0x03, 0x10   // Read Register #1: (Read Operation, Register Address)\r\n");
                    builder.Append("               // Read Operation Bit Definition:\r\n");
                    builder.Append("               //Bit7 ~ Bit4: Number of Right Shift before sending to host\r\n");
                    builder.Append("               //Bit3 ~ Bit2: Reserved\t\r\n");
                    builder.Append("               //Bit1:        Endian, 0 - big, 1 - little\r\n");
                    builder.Append("               //Bit0:        Read mode, 0 - read only, 1 - write with repeated start read\r\n");
                    builder.Append("0x1B    // Address of the Register that controls the sensor power states\r\n");
                    builder.Append("0x00    // Setting for Stand-by (low power) mode\r\n");
                    builder.Append("0x80    // Setting for Active mode\r\n");
                    builder.Append("0x04    // Number of Initialization Registers to be read\r\n");
                    builder.Append("  0x1B, 0x01   // Init Read Register #1: (Register Address, Number of Bytes)\r\n");
                    builder.Append("  0x1C, 0x01   // Init Read Register #2: (Register Address, Number of Bytes)\r\n");
                    builder.Append("  0x1D, 0x01   // Init Read Register #3: (Register Address, Number of Bytes)\r\n");
                    builder.Append("  0x0B, 0x01   // Init Read Register #4: (Register Address, Number of Bytes)\r\n");
                    builder.Append("0x04    // Number of sensor control registers\r\n");
                    builder.Append("0x20    // Time delay between two consecutive register writes, unit: ms\r\n");
                    builder.Append("  0x1B, 0x90   // CNTL1 Register  (Register Address, Value)\tValue: PwrCtrl, OutDatRate, FuncState\r\n");
                    builder.Append("  0x1C, 0x0C   // CNTL2 Register  (Register Address, Value) Value: IntEnb, DRDYEnb, DRDYPolarity\r\n");
                    builder.Append("  0x1D, 0x00   // CNTL3 Register  (Register Address, Value) Value: SoftReset, StartFORCEMeas, SelfTest\r\n");
                    builder.Append("  0x0B, 0x05   // ACNTL1 Register (Register Address, Value) Value: SensCorrByPass, OVF, OVFOut, BITS\r\n");
                    builder.Append("\n// Configuration for Sensor #2 (KXTF9)\r\n");
                    builder.Append("0x000F  // Sensor I2C Address (7 or 10 bit)\r\n");
                    builder.Append("0x01    // Sensor Type (1 - Accel, 2 - Magn, 3 - Press, 4 - Gyro, 5 - Accel_Gyro, 6 - Accel_Magn, 7 - Gyro_Magn, 8 - Accel_Magn_Gyro)\r\n");
                    builder.Append("0x00    // Initialization period after power up for Sensor, unit: 10ms\r\n");
                    builder.Append("0xCD    // Data type (bit 0), Number of bytes to read from each register (bits 1-3) and Data Resolution (bits 4-7)\r\n");
                    builder.Append("0x06    // Sample rate: 1- 1hz, 2- 2hz, 3- 5hz, 4- 10hz, 5- 25hz, 6- 50hz, 7- 100Hz\r\n");
                    builder.Append("0x03    // Send Rate:   1- 1hz, 2- 2hz, 3- 5hz, 4- 10hz, 5- 25hz, 6- 50hz, 7- 100Hz\r\n");
                    builder.Append("0x00    // Data decimation method (bits 0-2): 0- raw, 1- averaging, 2- sliding median\r\n");
                    builder.Append("0x20    // Acquisition time delay, unit: 10us\r\n");
                    builder.Append("0x01    // Number of sensor read registers\r\n");
                    builder.Append("0x01    // Measurement State: 0 - auto (sensor configured) 1 - forced (sw controlling)  was 0x00\r\n");
                    builder.Append("  0x43, 0x06   // Read Register #1: (Read Operation, Register Address)\r\n");
                    builder.Append("               // Read Operation Bit Definition:\r\n");
                    builder.Append("               //Bit7 ~ Bit4: Number of Right Shift before sending to host\r\n");
                    builder.Append("               //Bit3 ~ Bit2: Reserved\t\r\n");
                    builder.Append("               //Bit1:        Endian, 0 - big, 1 - little\r\n");
                    builder.Append("               //Bit0:        Read mode, 0 - read only, 1 - write with repeated start read\r\n");
                    builder.Append("0x1B    // Address of the Register to controls the sensor power states\r\n");
                    builder.Append("0x00    // Setting for Stand-by (OFF) mode\r\n");
                    builder.Append("0x80    // Setting for Operating (ON) mode\r\n");
                    builder.Append("0x02    // Number of Initialization Registers to be read\r\n");
                    builder.Append("  0x1B, 0x01   // Init Read Register #1: (Register Address, Number of Bytes)\r\n");
                    builder.Append("  0x1D, 0x01   // Init Read Register #2: (Register Address, Number of Bytes)\r\n");
                    builder.Append("0x02    // Number of sensor control registers\r\n");
                    builder.Append("0x20    // Time delay between two consecutive register writes, unit: ms\r\n");
                    builder.Append("  0x1B, 0xC0   // CTRL_REG1: (Register Address, Value) OpMode, Ressolution 12bit\r\n");
                    builder.Append("  0x1D, 0x4D   // CTRL_REG3: (Register Address, Value) Default\r\n");
                    builder.Append("\n0x01    // Sensor Data Processing Rate\r\n");
                    builder.Append("\n0x00  // sensor #1 Zero Point Value\r\n");
                    builder.Append("0x01  // sensor #1 Scale Factor (sensitivity)\r\n");
                    builder.Append("\n0x0000  // sensor #2 Zero Point Value\r\n");
                    builder.Append("0x0400  // sensor #2 Scale Factor (sensitivity)\r\n");
                    writer.WriteLine(builder.ToString());
                    writer.Close();
                    File.Copy(hostAppConfigParams.HostAppMEMSCfgPath, str3, true);
                }
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

        private void extEphComboBox_Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.extEphComboBox_Select.SelectedIndex == 0)
            {
                this.extEphComboBox_ServerName.Enabled = false;
                this.extEphEditBox_ServerPort.Enabled = false;
                this.extEphEditBox_AuthCode.Enabled = false;
                this.extEphComboBox_EEDay.Enabled = false;
                this.extEphEditBox_BankTime.Enabled = false;
                this.hostAppEEChkBox.Checked = false;
            }
            else if (this.hostAppRunHostChkBox.Checked)
            {
                this.extEphComboBox_ServerName.Enabled = true;
                this.extEphEditBox_ServerPort.Enabled = true;
                this.extEphEditBox_AuthCode.Enabled = true;
                this.extEphComboBox_EEDay.Enabled = true;
                this.extEphEditBox_BankTime.Enabled = true;
            }
            switch (this.extEphComboBox_Select.SelectedIndex)
            {
                case 0:
                    this.extEphComboBox_ServerName.Enabled = false;
                    this.extEphEditBox_ServerPort.Enabled = false;
                    this.extEphEditBox_AuthCode.Enabled = false;
                    this.extEphComboBox_EEDay.Enabled = false;
                    this.extEphEditBox_BankTime.Enabled = false;
                    break;

                case 1:
                case 3:
                    this.extEphEditBox_BankTime.Enabled = true;
                    break;

                case 2:
                    this.extEphEditBox_BankTime.Enabled = false;
                    break;
            }
            if (this.extEphComboBox_Select.SelectedIndex != 0)
            {
                this.hostAppRunHostChkBox.Checked = true;
                this.hostAppEEChkBox.Checked = true;
            }
        }

        private void frmCommSettigsRS232AdvanceSettingsBtn_Click(object sender, EventArgs e)
        {
            this._advancedRS232SettingsFlag = !this._advancedRS232SettingsFlag;
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Enabled = this._advancedRS232SettingsFlag;
        }

        private void frmCommSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (base.DialogResult != DialogResult.Cancel)
                {
                    if (this.comm.RequireHostRun)
                    {
                        if (this.comm.HostSWFilePath == "")
                        {
                            MessageBox.Show("Please select a valid - Host to run   ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if (this.comm.TrackerPort == "")
                        {
                            MessageBox.Show("Please select a valid - Tracker Port  ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if (this.comm.ResetPort == "")
                        {
                            MessageBox.Show("Please select a valid - Reset Port    ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232) && (this.comm.HostPair1 == ""))
                        {
                            MessageBox.Show("Please select a valid - Host Port Pair", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client) && (this.CMC.HostAppClient.TCPClientHostName == ""))
                        {
                            MessageBox.Show("Please select a valid - TCP IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server) && (this.CMC.HostAppServer.TCPServerHostName == ""))
                        {
                            MessageBox.Show("Please select a valid - TCP IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C) && ((this.I2C_PortTxtBox.Text == "") || (this.I2C_PortTxtBoxMaster.Text == "")))
                        {
                            MessageBox.Show("Please select a valid - I2C Port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if (this.comm.RequireEE)
                        {
                            if (((this.comm.EESelect != "NO EE") && (this.comm.EESelect != "SGEE")) && (this.comm.BankTime == ""))
                            {
                                MessageBox.Show("Please select a valid - Bank time     ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                e.Cancel = true;
                                this.COMDeviceTabControl.SelectedIndex = 4;
                            }
                            if ((this.comm.EESelect != "NO EE") && (this.comm.EESelect != "CGEE"))
                            {
                                if (this.comm.ServerName == "")
                                {
                                    MessageBox.Show("Please select a valid - Server Name   ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                if (this.comm.ServerPort == "")
                                {
                                    MessageBox.Show("Please select a valid - Server Port   ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                if (this.comm.AuthenticationCode == "")
                                {
                                    MessageBox.Show("Please select a valid - Authentication Code   ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                if (this.comm.EEDayNum == "")
                                {
                                    MessageBox.Show("Please select a valid - EE Day Number ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                if (((this.comm.ServerName == "") || (this.comm.ServerPort == "")) || ((this.comm.AuthenticationCode == "") || (this.comm.EEDayNum == "")))
                                {
                                    e.Cancel = true;
                                    this.COMDeviceTabControl.SelectedIndex = 4;
                                }
                            }
                        }
                        if (((this.comm.HostSWFilePath == "") || (this.comm.TrackerPort == "")) || (this.comm.ResetPort == ""))
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        this.comm.HostPair1 = this.hostAppCboHostPair2.Text;
                    }
                    if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232) && (this.comm.HostPair1 == ""))
                    {
                        MessageBox.Show("Please select a valid - Host Port Pair", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    Regex regex = new Regex(@"((0|1[0-9]{0,2}|2[0-9]{0,1}|2[0-4][0-9]|25[0-5]|[3-9][0-9]{0,1})\.){3}(0|1[0-9]{0,2}|2[0-9]{0,1}|2[0-4][0-9]|25[0-5]|[3-9][0-9]{0,1})$");
                    if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client) && ((this.CMC.HostAppClient.TCPClientHostName == "") || !regex.IsMatch(this.CMC.HostAppClient.TCPClientHostName)))
                    {
                        MessageBox.Show("Please select a valid - TCP IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server) && ((this.CMC.HostAppServer.TCPServerHostName == "") || !regex.IsMatch(this.CMC.HostAppServer.TCPServerHostName)))
                    {
                        MessageBox.Show("Please select a valid - TCP IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C) && (((this.I2C_PortTxtBox.Text == "") || (this.I2C_PortTxtBoxMaster.Text == "")) || (this.I2C_SlaveTxtBx.Text == "")))
                    {
                        MessageBox.Show("Please select a valid I2C Port number and Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if (((((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client) && (this.CMC.HostAppClient.TCPClientHostName == "")) || ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client) && !regex.IsMatch(this.CMC.HostAppClient.TCPClientHostName))) || (((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server) && (this.CMC.HostAppServer.TCPServerHostName == "")) || ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server) && !regex.IsMatch(this.CMC.HostAppServer.TCPServerHostName)))) || (((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232) && (this.comm.HostPair1 == "")) || ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C) && (((this.I2C_PortTxtBox.Text == "") || (this.I2C_SlaveTxtBx.Text == "")) || (this.I2C_PortTxtBoxMaster.Text == "")))))
                    {
                        e.Cancel = true;
                    }
                }
                else if (this._errorDetected)
                {
                    e.Cancel = true;
                }
                this.CMC.HostAppClient.TCPClientHostName = this.COMClientIPAddrTextBox.Text;
                this.CMC.HostAppServer.TCPServerHostName = this.COMServerIPTextBox.Text;
                this.CMC.HostAppI2CSlave.I2CDevicePortNum = Convert.ToInt32(this.I2C_PortTxtBox.Text);
                this.CMC.HostAppI2CSlave.I2CDevicePortNumMaster = Convert.ToInt32(this.I2C_PortTxtBoxMaster.Text);
                this.CMC.HostAppI2CSlave.I2CSlaveAddress = Convert.ToByte(this.I2C_SlaveTxtBx.Text, 0x10);
                this.CMC.HostAppI2CSlave.I2CMasterAddress = Convert.ToByte(this.I2C_MasterTxtBx.Text, 0x10);
                if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    this.comm.BaudRate = this.cboBaud.SelectedItem.ToString();
                    this.comm.PortName = this.cboPort.SelectedItem.ToString();
                }
                this.comm.MessageProtocol = this.cboProtocols.Text;
                this.comm.DefaultTCXOFreq = this.hostAppTCXOFreqComboBox.Text;
                this._errorDetected = false;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: frmCommSettings: frmCommSettings_FormClosing() - " + exception.ToString());
            }
        }

        private void frmCommSettings_Load(object sender, EventArgs e)
        {
            this._errorDetected = false;
            foreach (TabPage page in this.COMDeviceTabControl.TabPages)
            {
                this._backupTabPages.Add(page);
            }
            if (this.comm != null)
            {
                this.LoadValues();
                if (this.SetDefaults())
                {
                    this.SetControlState();
                    if (this.comm.RxType == CommunicationManager.ReceiverType.SLC)
                    {
                        this.frmComSettingsSLCRadioBtn.Checked = true;
                    }
                    else if (this.comm.RxType == CommunicationManager.ReceiverType.TTB)
                    {
                        this.frmComSettingsTTBRadioBtn.Checked = true;
                    }
                    else
                    {
                        this.frmComSettingsGSWRadioBtn.Checked = true;
                    }
                    if (this.comm.AutoReplyCtrl.AidingProtocolVersion == string.Empty)
                    {
                        this.comm.AutoReplyCtrl.AidingProtocolVersion = "1.0";
                    }
                    if (this.comm.AutoReplyCtrl.ControlChannelVersion == string.Empty)
                    {
                        this.comm.AutoReplyCtrl.ControlChannelVersion = "1.0";
                    }
                    if (this.comm.MessageProtocol == string.Empty)
                    {
                        this.comm.MessageProtocol = "OSP";
                    }
                    if (this.comm.DefaultTCXOFreq == string.Empty)
                    {
                        this.comm.DefaultTCXOFreq = "16369000";
                    }
                    this.cboAI3Version.Text = this.comm.AutoReplyCtrl.AidingProtocolVersion;
                    this.cboFVersion.Text = this.comm.AutoReplyCtrl.ControlChannelVersion;
                    this.cboProtocols.Text = this.comm.MessageProtocol;
                    this.hostAppTCXOFreqComboBox.Text = this.comm.DefaultTCXOFreq;
                    this.COMTcpIPClientRadioButton.Checked = true;
                    this.COMTcpIPServerRadioButton.Checked = false;
                    this.COMClientIPGroupBox.Enabled = true;
                    if (clsGlobal.IsMarketingUser())
                    {
                        this.frmCommSettingsProductFamilyComboBox.Items.Remove("GSW");
                        this.frmCommSettingsProductFamilyComboBox.Items.Remove("SLC");
                        this.frmCommSettingsAdvancedRxSettingsBtn.Visible = false;
                    }
                    this.hostAppSWDirTxtBox.Text = this.comm.HostSWFilePath;
                    if (this.hostAppCboTrackerPort.Items.Contains(this.comm.TrackerPort))
                    {
                        this.hostAppCboTrackerPort.SelectedText = this.comm.TrackerPort;
                        this.hostAppCboTrackerPort.Text = this.comm.TrackerPort;
                    }
                    if (this.hostAppCboResetPort.Items.Contains(this.comm.ResetPort))
                    {
                        this.hostAppCboResetPort.SelectedText = this.comm.ResetPort;
                        this.hostAppCboResetPort.Text = this.comm.ResetPort;
                    }
                    if (this.hostAppCboHostPair1.Items.Contains(this.comm.HostPair1))
                    {
                        this.hostAppCboHostPair1.SelectedText = this.comm.HostPair1;
                        this.hostAppCboHostPair1.Text = this.comm.HostPair1;
                    }
                    if (this.hostAppCboHostPair2.Items.Contains(this.comm.PortName))
                    {
                        this.hostAppCboHostPair2.SelectedText = this.comm.PortName;
                        this.hostAppCboHostPair2.Text = this.comm.PortName;
                    }
                    if (this.hostAppCboTCPIPClientPort.Items.Contains(this.comm.CMC.HostAppClient.TCPClientPortNum.ToString()))
                    {
                        this.hostAppCboTCPIPClientPort.SelectedText = this.comm.CMC.HostAppClient.TCPClientPortNum.ToString();
                        this.hostAppCboTCPIPClientPort.Text = this.comm.CMC.HostAppClient.TCPClientPortNum.ToString();
                    }
                    if (this.comm.RequireHostRun)
                    {
                        this.hostAppRunHostChkBox.Checked = true;
                        this.hostAppEEChkBox.Enabled = true;
                        this.hostAppEEChkBox.Checked = this.comm.RequireEE;
                    }
                    this.setRunHostAppEnable(this.comm.RequireHostRun);
                    if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client) || (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server))
                    {
                        this.hostAppRS232RadioBtn.Checked = false;
                        this.hostAppTCPIPRadioBtn.Checked = true;
                        this.hostAppCboTCPIPClientPort.Enabled = true;
                        this.setHostAppTCPIPEnable();
                    }
                    else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                    {
                        this.hostAppRS232RadioBtn.Checked = true;
                        this.setHostAppRS232Enable();
                    }
                    else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                    {
                        this.hostAppI2CRadioBtn.Checked = true;
                        this.setHostAppI2CEnable();
                    }
                    this.frmCommSettingsProductFamilyComboBox.SelectedIndex = (int) this.comm.ProductFamily;
                    if (this.frmCommSettingsProductFamilyComboBox.SelectedIndex != 0)
                    {
                        this.hostAppRunHostChkBox.Visible = false;
                    }
                    else
                    {
                        this.hostAppRunHostChkBox.Visible = true;
                    }
                    this.setAdvancedRxSettingsVisible(false);
                    this.frmCommSettingsAdvancedHostAppBtn.Enabled = false;
                    this.rdoHex.Visible = false;
                    this.rdoSSB.Visible = false;
                    if (clsGlobal.IsMarketingUser())
                    {
                        this.fVersion.Hide();
                        this.ai3Version.Hide();
                        this.cboFVersion.Hide();
                        this.cboAI3Version.Hide();
                        this.frmComSettingsSLCRadioBtn.Hide();
                        this.frmComSettingsGSWRadioBtn.Hide();
                        this.frmComSettingsTTBRadioBtn.Hide();
                        this.COMTcpIPServerRadioButton.Hide();
                        this.COMServerGroupBox.Hide();
                        this.COMTcpIPClientRadioButton.Checked = true;
                        this.COMTcpIPClientRadioButton.Enabled = false;
                        this.autoBaudChkBox.Visible = false;
                        this.autoBaudChkBox.Checked = true;
                        this.pulseOnOffPortLabel.Hide();
                        this.onOffActionComboBox.Hide();
                        this.gsd4eOnOffPortLabel.Hide();
                        this.gsd4eOnOffPortComboBox.Hide();
                    }
                    else
                    {
                        this.autoBaudChkBox.Visible = true;
                        this.autoBaudChkBox.Checked = this.comm.IsAutoDetectBaud;
                    }
                    base.Height = this.frmCommSettingsRxParamsGrpBox.Location.Y + 10;
                }
            }
        }

        private void frmCommSettingsAdvanceRxSettingsBtn_Click(object sender, EventArgs e)
        {
            this._advancedRxSettingsFlag = !this._advancedRxSettingsFlag;
            this.setAdvancedRxSettingsVisible(this._advancedRxSettingsFlag);
        }

        private void frmCommSettingsCancelBtn_Click(object sender, EventArgs e)
        {
            base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            base.Close();
        }

        private void frmCommSettingsOkBtn_Click(object sender, EventArgs e)
        {
            this.comm.RequireHostRun = this.hostAppRunHostChkBox.Checked;
            this.comm.TrackerPort = this.hostAppCboTrackerPort.Text;
            this.comm.ResetPort = this.hostAppCboResetPort.Text;
            this.CMC.HostAppClient.TCPClientHostName = this.COMClientIPAddrTextBox.Text;
            this.CMC.HostAppServer.TCPServerHostName = this.COMServerIPTextBox.Text;
            this.comm.LNAType = this.hostAppLnaComboBox.SelectedIndex;
            this.comm.LDOMode = this.hostAppLDOModeComboBox.SelectedIndex;
            this.comm.RxName = this.frmCommSettingsRxNameTxtBox.Text;
            if (this.comm.RxName == string.Empty)
            {
                this.comm.RxName = "SiRF_EVK";
            }
            this.comm.RequireEE = this.hostAppEEChkBox.Checked;
            this.comm.EESelect = this.extEphComboBox_Select.Text;
            this.comm.ServerName = this.extEphComboBox_ServerName.Text;
            this.comm.ServerPort = this.extEphEditBox_ServerPort.Text;
            this.comm.AuthenticationCode = this.extEphEditBox_AuthCode.Text;
            this.comm.EEDayNum = this.extEphComboBox_EEDay.Text;
            this.comm.BankTime = this.extEphEditBox_BankTime.Text;
            this.comm.IsAutoDetectBaud = this.autoBaudChkBox.Checked;
            this.comm.IsVersion4_1_A8AndAbove = this.hostAppVersionCheckBox.Checked;
            if (this.comm.RequireHostRun)
            {
                if ((this.comm.TrackerPort == this.comm.ResetPort) && (this.comm.ProductFamily == CommonClass.ProductType.GSD4t))
                {
                    MessageBox.Show("Error: Same port set for tracker and reset!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this._errorDetected = true;
                    return;
                }
                this.COMDeviceTabControl.SelectedIndex = 0;
            }
            if (this.hostAppRS232RadioBtn.Checked)
            {
                if (this.comm.RequireHostRun)
                {
                    this.COMDeviceTabControl.SelectedIndex = 0;
                }
                else
                {
                    this.COMDeviceTabControl.SelectedIndex = 1;
                }
                if (this.comm.RequireHostRun && (this.comm.ProductFamily == CommonClass.ProductType.GSD4t))
                {
                    List<string> inList = new List<string>();
                    inList.Add(this.hostAppCboTrackerPort.Text);
                    inList.Add(this.hostAppCboResetPort.Text);
                    inList.Add(this.hostAppCboHostPair1.Text);
                    inList.Add(this.hostAppCboHostPair2.Text);
                    if (HelperFunctions.IsDuplicateString(inList))
                    {
                        MessageBox.Show("Dupliate port settings detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        this._errorDetected = true;
                        return;
                    }
                }
                this.SetupRS232Comm();
            }
            else if (this.hostAppI2CRadioBtn.Checked)
            {
                this.SetupI2CComm();
            }
            else
            {
                if (this.COMTcpIPClientRadioButton.Checked)
                {
                    if (this.comm.RequireHostRun && (this.CMC.HostAppClient.TCPClientHostName == ""))
                    {
                        this.COMDeviceTabControl.SelectedIndex = 2;
                    }
                    else
                    {
                        this.COMDeviceTabControl.SelectedIndex = 0;
                    }
                }
                else if (this.comm.RequireHostRun && (this.CMC.HostAppServer.TCPServerHostName == ""))
                {
                    this.COMDeviceTabControl.SelectedIndex = 2;
                }
                else
                {
                    this.COMDeviceTabControl.SelectedIndex = 0;
                }
                this.SetupTcpIpComm();
            }
            if (this.comm.RequireHostRun)
            {
                if (!File.Exists(this.hostAppSWDirTxtBox.Text))
                {
                    MessageBox.Show("Host SW does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this._errorDetected = true;
                    return;
                }
                HostAppConfigParams hostAppConfigParams = new HostAppConfigParams();
                hostAppConfigParams.SiRFLiveInterfacePortName = "SW_" + this.comm.PortName;
                hostAppConfigParams.TrackerPort = this.comm.TrackerPort;
                hostAppConfigParams.TrackerPortSelect = this.comm.TrackerPortSelect;
                hostAppConfigParams.BaudRate = this.comm.BaudRate;
                hostAppConfigParams.ResetPort = this.comm.ResetPort;
                hostAppConfigParams.WarmupDelay = this.comm.WarmupDelay;
                hostAppConfigParams.OnOffLineUsage = this.comm.OnOffLineUsage;
                hostAppConfigParams.ExtSResetNLineUsage = this.comm.ExtSResetNLineUsage;
                hostAppConfigParams.DebugSettings = this.comm.DebugSettings;
                hostAppConfigParams.LNAType = this.comm.LNAType;
                hostAppConfigParams.LDOMode = this.comm.LDOMode;
                hostAppConfigParams.DefaultTCXOFreq = this.comm.DefaultTCXOFreq;
                hostAppConfigParams.HostSWFilePath = this.comm.HostSWFilePath;
                if (this.hostAppVersionCheckBox.Checked)
                {
                    hostAppConfigParams.OnOffLineUsage = "uart_rts";
                }
                CreateGSD4tConfigFile(this.hostAppSWDirTxtBox.Text, ref hostAppConfigParams);
                CreateGSD4tMEMSConfigFile(this.hostAppSWDirTxtBox.Text, ref hostAppConfigParams);
                this.comm.HostAppCfgFilePath = hostAppConfigParams.HostAppCfgFilePath;
                this.comm.HostAppMEMSCfgPath = hostAppConfigParams.HostAppMEMSCfgPath;
                this.comm.HostSWFilePath = hostAppConfigParams.HostSWFilePath;
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void frmCommSettingsProductFamilyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.frmCommSettingsProductFamilyComboBox.SelectedIndex)
            {
                case 0:
                    this.comm.RxTransType = CommunicationManager.TransmissionType.GPS;
                    this.comm.TxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                    this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                    this.comm.MessageProtocol = "OSP";
                    this.comm.AidingProtocol = "OSP";
                    this.cboFVersion.SelectedIndex = 0;
                    this.cboAI3Version.SelectedIndex = 0;
                    this.cboProtocols.Text = "OSP";
                    this.cboBaud.SelectedIndex = 9;
                    this.cboBaud.Text = this.cboBaud.Items[this.cboBaud.SelectedIndex].ToString();
                    this.comm.BaudRate = this.cboBaud.Text;
                    this.comm.ProductFamily = CommonClass.ProductType.GSD4t;
                    this.hostAppTCPIPRadioBtn.Visible = true;
                    if (this.hostAppI2CRadioBtn.Checked)
                    {
                        this.hostAppRS232RadioBtn.Checked = true;
                    }
                    this.hostAppI2CRadioBtn.Visible = false;
                    this.gsd4eOnOffPortComboBox.Hide();
                    this.gsd4eOnOffPortLabel.Hide();
                    this.onOffActionComboBox.Hide();
                    this.pulseOnOffPortLabel.Hide();
                    goto Label_042D;

                case 1:
                    this.comm.RxTransType = CommunicationManager.TransmissionType.Text;
                    this.comm.TxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                    this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                    this.comm.MessageProtocol = "NMEA";
                    this.comm.AidingProtocol = "OSP";
                    this.cboFVersion.SelectedIndex = 0;
                    this.cboAI3Version.SelectedIndex = 0;
                    this.cboProtocols.Text = "NMEA";
                    this.rdoText.Checked = true;
                    this.cboBaud.SelectedIndex = 4;
                    this.cboBaud.Text = this.cboBaud.Items[this.cboBaud.SelectedIndex].ToString();
                    this.comm.BaudRate = this.cboBaud.Text;
                    if (!clsGlobal.IsMarketingUser())
                    {
                        this.gsd4eOnOffPortComboBox.Show();
                        this.gsd4eOnOffPortLabel.Show();
                        this.onOffActionComboBox.Show();
                        this.pulseOnOffPortLabel.Show();
                        break;
                    }
                    this.hostAppTCPIPRadioBtn.Visible = false;
                    this.gsd4eOnOffPortComboBox.Hide();
                    this.gsd4eOnOffPortLabel.Hide();
                    this.onOffActionComboBox.Hide();
                    this.pulseOnOffPortLabel.Hide();
                    if (this.comm.InputDeviceMode != CommonClass.InputDeviceModes.I2C)
                    {
                        this.hostAppRS232RadioBtn.Checked = true;
                        break;
                    }
                    this.hostAppI2CRadioBtn.Checked = true;
                    break;

                case 2:
                    this.comm.RxTransType = CommunicationManager.TransmissionType.GPS;
                    this.comm.TxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                    this.comm.RxType = CommunicationManager.ReceiverType.GSW;
                    this.comm.MessageProtocol = "SSB";
                    this.cboFVersion.SelectedIndex = 1;
                    this.cboAI3Version.SelectedIndex = 1;
                    this.cboProtocols.Text = "SSB";
                    this.comm.ProductFamily = CommonClass.ProductType.GSW;
                    if (this.hostAppI2CRadioBtn.Checked)
                    {
                        this.hostAppRS232RadioBtn.Checked = true;
                    }
                    this.hostAppI2CRadioBtn.Visible = false;
                    this.gsd4eOnOffPortComboBox.Hide();
                    this.gsd4eOnOffPortLabel.Hide();
                    this.onOffActionComboBox.Hide();
                    this.pulseOnOffPortLabel.Hide();
                    goto Label_042D;

                case 3:
                    this.comm.RxTransType = CommunicationManager.TransmissionType.GPS;
                    this.comm.TxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                    this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                    this.cboFVersion.SelectedIndex = 1;
                    this.cboAI3Version.SelectedIndex = 1;
                    this.cboProtocols.Text = "SSB";
                    this.comm.ProductFamily = CommonClass.ProductType.SLC;
                    if (this.hostAppI2CRadioBtn.Checked)
                    {
                        this.hostAppRS232RadioBtn.Checked = true;
                    }
                    this.hostAppI2CRadioBtn.Visible = false;
                    this.gsd4eOnOffPortComboBox.Hide();
                    this.gsd4eOnOffPortLabel.Hide();
                    this.onOffActionComboBox.Hide();
                    this.pulseOnOffPortLabel.Hide();
                    goto Label_042D;

                default:
                    goto Label_042D;
            }
            this.comm.ProductFamily = CommonClass.ProductType.GSD4e;
            this.hostAppI2CRadioBtn.Visible = true;
        Label_042D:
            if (this.frmCommSettingsProductFamilyComboBox.SelectedIndex == 0)
            {
                this.hostAppRunHostChkBox.Visible = true;
            }
            else
            {
                this.hostAppRunHostChkBox.Visible = false;
                this.hostAppRunHostChkBox.Checked = false;
            }
        }

        private void frmComSettingsGSWRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmComSettingsGSWRadioBtn.Checked)
            {
                this.comm.RxType = CommunicationManager.ReceiverType.GSW;
                this.cboFVersion.Enabled = false;
                this.cboAI3Version.Enabled = false;
                this.cboFVersion.SelectedIndex = 1;
                this.cboAI3Version.SelectedIndex = 2;
            }
        }

        private void frmComSettingsSLCRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmComSettingsSLCRadioBtn.Checked)
            {
                this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                this.cboFVersion.Enabled = true;
                this.cboAI3Version.Enabled = true;
                this.cboFVersion.SelectedIndex = 1;
                this.cboAI3Version.SelectedIndex = 2;
            }
        }

        private void frmComSettingsTTBRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmComSettingsTTBRadioBtn.Checked)
            {
                this.comm.RxType = CommunicationManager.ReceiverType.TTB;
                this.cboProtocols.Text = "SSB";
                this.cboBaud.Text = "56700";
                this.cboBaud.SelectedIndex = 9;
                this.cboFVersion.Enabled = false;
                this.cboAI3Version.Enabled = false;
                this.cboFVersion.SelectedIndex = 1;
                this.cboAI3Version.SelectedIndex = 2;
            }
        }

        private void hostAppCboHostPair2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboPort.SelectedIndex = this.hostAppCboHostPair2.SelectedIndex;
        }

        private void hostAppCboTCPIPPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag1 = this.COMTcpIPClientRadioButton.Checked;
            bool flag2 = this.COMTcpIPServerRadioButton.Checked;
        }

        private void hostAppEEChkBox_CheckedChange(object sender, EventArgs e)
        {
            if (this.hostAppEEChkBox.Checked)
            {
                this.addRemoveTabpage(true, 3, ((TabPage) this._backupTabPages[3]).Name);
                this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[3]).Name);
                this.extEphComboBox_ServerName.Text = this.comm.ServerName;
                this.extEphEditBox_ServerPort.Text = this.comm.ServerPort;
                this.extEphEditBox_AuthCode.Text = this.comm.AuthenticationCode;
                this.extEphComboBox_EEDay.Text = this.comm.EEDayNum;
                this.extEphEditBox_BankTime.Text = this.comm.BankTime;
                this.extEphComboBox_Select.SelectedIndex = 2;
            }
            else
            {
                this.addRemoveTabpage(false, 3, ((TabPage) this._backupTabPages[3]).Name);
                this.extEphComboBox_ServerName.Enabled = false;
                this.extEphEditBox_ServerPort.Enabled = false;
                this.extEphEditBox_AuthCode.Enabled = false;
                this.extEphComboBox_EEDay.Enabled = false;
                this.extEphEditBox_BankTime.Enabled = false;
                this.extEphComboBox_Select.SelectedIndex = 0;
            }
        }

        private void hostAppLDOModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comm.LDOMode = this.hostAppLDOModeComboBox.SelectedIndex;
            }
            catch
            {
            }
        }

        private void hostAppLnaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comm.LNAType = this.hostAppLnaComboBox.SelectedIndex;
            }
            catch
            {
            }
        }

        private void hostAppRS232RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.hostAppRS232RadioBtn.Checked)
            {
                this.setHostAppRS232Enable();
            }
            else if (this.hostAppTCPIPRadioBtn.Checked)
            {
                this.setHostAppTCPIPEnable();
            }
            else
            {
                this.setHostAppI2CEnable();
            }
        }

        private void hostAppRS232TCPIPRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.hostAppTCPIPRadioBtn.Checked)
            {
                this.hostAppCboTCPIPClientPort.Enabled = true;
                this.setHostAppTCPIPEnable();
            }
            else if (this.hostAppRS232RadioBtn.Checked)
            {
                this.hostAppCboHostPair1.Enabled = true;
                this.hostAppCboHostPair2.Enabled = true;
                this.setHostAppRS232Enable();
                this.setHostAppRS232Enable();
            }
            else
            {
                this.setHostAppI2CEnable();
            }
        }

        private void hostAppRunHostChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.setRunHostAppEnable(this.hostAppRunHostChkBox.Checked);
        }

        private void hostAppSWDirBrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Host Apps (*.exe)|*.exe|All file (*.*)|*.*";
            dialog.FilterIndex = 3;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.hostAppSWDirTxtBox.Text = dialog.FileName;
            }
        }

        private void hostAppTCXOFreqComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comm.DefaultTCXOFreq = this.hostAppTCXOFreqComboBox.Text;
            }
            catch
            {
            }
        }

        private void I2CDetectBtn_Click(object sender, EventArgs e)
        {
            short[] devices = new short[0x10];
            int[] numArray2 = new int[0x10];
            int num = 0x10;
            try
            {
                AardvarkApi.aa_find_devices_ext(num, devices, num, numArray2);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception in searching for I2C devices: " + exception.Message);
                return;
            }
            string str = "(avail) ";
            if ((devices[0] & -32768) != 0)
            {
                devices[0] = (short) (devices[0] & 0x7fff);
                str = "(in-use)";
            }
            uint num2 = (uint) numArray2[0];
            string str2 = string.Format("{0} {1:d4}-{2:d6}", str, num2 / 0xf4240, num2 % 0xf4240);
            this.I2C_PortTxtBox.Text = devices[0].ToString();
            this.I2C_SerialNum.Text = str2;
            string str3 = "(avail) ";
            if ((devices[1] & -32768) != 0)
            {
                devices[1] = (short) (devices[1] & 0x7fff);
                str3 = "(in-use)";
            }
            uint num3 = (uint) numArray2[1];
            str2 = string.Format("{0} {1:d4}-{2:d6}", str3, num3 / 0xf4240, num3 % 0xf4240);
            this.I2C_PortTxtBoxMaster.Text = devices[1].ToString();
            this.I2C_SerialNumMaster.Text = str2;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCommSettings));
            this.frmCommSettingsOkBtn = new Button();
            this.frmCommSettingsCancelBtn = new Button();
            this.COMDeviceTabControl = new TabControl();
            this.HostAppTabPage = new TabPage();
            this.hostAppVersionCheckBox = new CheckBox();
            this.frmCommSettingsAdvancedHostAppBtn = new Button();
            this.hostAppEEChkBox = new CheckBox();
            this.label10 = new Label();
            this.hostAppSWDirBrowseBtn = new Button();
            this.hostAppSWDirTxtBox = new TextBox();
            this.hostAppSWDirLabel = new Label();
            this.hostAppPairLabel = new Label();
            this.hostAppResetPortLabel = new Label();
            this.hostAppCboHostPair2 = new ComboBox();
            this.hostAppCboHostPair1 = new ComboBox();
            this.hostAppCboResetPort = new ComboBox();
            this.hostAppLDOModeComboBox = new ComboBox();
            this.hostAppLDOModeLabel = new Label();
            this.hostAppLnaComboBox = new ComboBox();
            this.hostAppLnaTypeLabel = new Label();
            this.hostAppTCXOFreqComboBox = new ComboBox();
            this.hostAppTCXOFreqLabel = new Label();
            this.hostAppCboTrackerPort = new ComboBox();
            this.hostAppTrackerPortLabel = new Label();
            this.RS232TabPage = new TabPage();
            this.pulseOnOffPortLabel = new Label();
            this.gsd4eOnOffPortLabel = new Label();
            this.gsd4eOnOffPortComboBox = new ComboBox();
            this.onOffActionComboBox = new ComboBox();
            this.autoBaudChkBox = new CheckBox();
            this.frmCommSettigsRS232AdvancedSettingsBtn = new Button();
            this.frmCommSettingsRS232AdvanceSettingsGrpBox = new GroupBox();
            this.cboFlowControl = new ComboBox();
            this.cboReadBuffer = new ComboBox();
            this.label17 = new Label();
            this.label11 = new Label();
            this.cboParity = new ComboBox();
            this.label3 = new Label();
            this.label5 = new Label();
            this.cboStop = new ComboBox();
            this.label4 = new Label();
            this.cboData = new ComboBox();
            this.cboBaud = new ComboBox();
            this.label2 = new Label();
            this.Label1 = new Label();
            this.cboPort = new ComboBox();
            this.TcpIpTabPage = new TabPage();
            this.COMServerGroupBox = new GroupBox();
            this.label8 = new Label();
            this.label9 = new Label();
            this.COMServerIPTextBox = new TextBox();
            this.hostAppCboServerTCPIPPort = new ComboBox();
            this.COMClientIPGroupBox = new GroupBox();
            this.label7 = new Label();
            this.label6 = new Label();
            this.COMClientIPAddrTextBox = new TextBox();
            this.hostAppCboTCPIPClientPort = new ComboBox();
            this.COMTcpIpModeGroupBox = new GroupBox();
            this.COMTcpIPServerRadioButton = new RadioButton();
            this.COMTcpIPClientRadioButton = new RadioButton();
            this.ExtEphTabPage = new TabPage();
            this.EEgroupBoxCGEE = new GroupBox();
            this.extEphEditBox_BankTime = new TextBox();
            this.EElabel_Banktime = new Label();
            this.EEgroupBoxSGEE = new GroupBox();
            this.extEphComboBox_ServerName = new ComboBox();
            this.extEphComboBox_EEDay = new ComboBox();
            this.EElabel_ServerName = new Label();
            this.extEphEditBox_AuthCode = new TextBox();
            this.extEphEditBox_ServerPort = new TextBox();
            this.EElabel_Daynum = new Label();
            this.EElabel_AuthCode = new Label();
            this.EElabel_ServerPort = new Label();
            this.EElabel_Select = new Label();
            this.extEphComboBox_Select = new ComboBox();
            this.I2CTabPage = new TabPage();
            this.checkBox_SlaveMode = new CheckBox();
            this.I2C_SerialNumMaster = new Label();
            this.I2C_PortTxtBoxMaster = new TextBox();
            this.I2C_SerialNum = new Label();
            this.label16 = new Label();
            this.I2C_DetectBtn = new Button();
            this.I2C_PortTxtBox = new TextBox();
            this.I2C_SlaveTxtBx = new TextBox();
            this.I2C_MasterTxtBx = new TextBox();
            this.label15 = new Label();
            this.label14 = new Label();
            this.label13 = new Label();
            this.hostAppTCPIPRadioBtn = new RadioButton();
            this.hostAppRS232RadioBtn = new RadioButton();
            this.hostAppRunHostChkBox = new CheckBox();
            this.groupBox3 = new GroupBox();
            this.frmCommOpenBufferSizeTxtBox = new TextBox();
            this.frmCommOpenBufferSizeLabel = new Label();
            this.rdoCSV = new RadioButton();
            this.rdoGP2 = new RadioButton();
            this.rdoSSB = new RadioButton();
            this.rdoText = new RadioButton();
            this.rdoHex = new RadioButton();
            this.frmCommOpenRxTypeGrpBox = new GroupBox();
            this.ai3Version = new Label();
            this.fVersion = new Label();
            this.cboAI3Version = new ComboBox();
            this.frmComSettingsTTBRadioBtn = new RadioButton();
            this.cboFVersion = new ComboBox();
            this.frmComSettingsGSWRadioBtn = new RadioButton();
            this.frmComSettingsSLCRadioBtn = new RadioButton();
            this.frmCommOpenProtocolsLabel = new Label();
            this.cboProtocols = new ComboBox();
            this.frmCommSettingsConTypeGrpBox = new GroupBox();
            this.hostAppI2CRadioBtn = new RadioButton();
            this.frmCommSettingsRxParamsGrpBox = new GroupBox();
            this.frmCommSettingsProductFamilyComboBox = new ComboBox();
            this.label12 = new Label();
            this.frmCommSettingsAdvancedRxSettingsBtn = new Button();
            this.frmCommSettingsRxNameLabel = new Label();
            this.frmCommSettingsRxNameTxtBox = new TextBox();
            this.COMDeviceTabControl.SuspendLayout();
            this.HostAppTabPage.SuspendLayout();
            this.RS232TabPage.SuspendLayout();
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.SuspendLayout();
            this.TcpIpTabPage.SuspendLayout();
            this.COMServerGroupBox.SuspendLayout();
            this.COMClientIPGroupBox.SuspendLayout();
            this.COMTcpIpModeGroupBox.SuspendLayout();
            this.ExtEphTabPage.SuspendLayout();
            this.EEgroupBoxCGEE.SuspendLayout();
            this.EEgroupBoxSGEE.SuspendLayout();
            this.I2CTabPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.frmCommOpenRxTypeGrpBox.SuspendLayout();
            this.frmCommSettingsConTypeGrpBox.SuspendLayout();
            this.frmCommSettingsRxParamsGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.frmCommSettingsOkBtn.Location = new Point(0x149, 0x181);
            this.frmCommSettingsOkBtn.Name = "frmCommSettingsOkBtn";
            this.frmCommSettingsOkBtn.Size = new Size(0x4b, 0x17);
            this.frmCommSettingsOkBtn.TabIndex = 14;
            this.frmCommSettingsOkBtn.Text = "&OK";
            this.frmCommSettingsOkBtn.UseVisualStyleBackColor = true;
            this.frmCommSettingsOkBtn.Click += new EventHandler(this.frmCommSettingsOkBtn_Click);
            this.frmCommSettingsCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmCommSettingsCancelBtn.Location = new Point(0x1a9, 0x181);
            this.frmCommSettingsCancelBtn.Name = "frmCommSettingsCancelBtn";
            this.frmCommSettingsCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmCommSettingsCancelBtn.TabIndex = 15;
            this.frmCommSettingsCancelBtn.Text = "&Cancel";
            this.frmCommSettingsCancelBtn.UseVisualStyleBackColor = true;
            this.frmCommSettingsCancelBtn.Click += new EventHandler(this.frmCommSettingsCancelBtn_Click);
            this.COMDeviceTabControl.Controls.Add(this.HostAppTabPage);
            this.COMDeviceTabControl.Controls.Add(this.RS232TabPage);
            this.COMDeviceTabControl.Controls.Add(this.TcpIpTabPage);
            this.COMDeviceTabControl.Controls.Add(this.ExtEphTabPage);
            this.COMDeviceTabControl.Controls.Add(this.I2CTabPage);
            this.COMDeviceTabControl.Location = new Point(0x16, 0x63);
            this.COMDeviceTabControl.Name = "COMDeviceTabControl";
            this.COMDeviceTabControl.SelectedIndex = 0;
            this.COMDeviceTabControl.Size = new Size(0x1de, 0x111);
            this.COMDeviceTabControl.TabIndex = 0;
            this.COMDeviceTabControl.SelectedIndexChanged += new EventHandler(this.COMDeviceTabControl_SelectedIndexChanged);
            this.HostAppTabPage.Controls.Add(this.hostAppVersionCheckBox);
            this.HostAppTabPage.Controls.Add(this.frmCommSettingsAdvancedHostAppBtn);
            this.HostAppTabPage.Controls.Add(this.hostAppEEChkBox);
            this.HostAppTabPage.Controls.Add(this.label10);
            this.HostAppTabPage.Controls.Add(this.hostAppSWDirBrowseBtn);
            this.HostAppTabPage.Controls.Add(this.hostAppSWDirTxtBox);
            this.HostAppTabPage.Controls.Add(this.hostAppSWDirLabel);
            this.HostAppTabPage.Controls.Add(this.hostAppPairLabel);
            this.HostAppTabPage.Controls.Add(this.hostAppResetPortLabel);
            this.HostAppTabPage.Controls.Add(this.hostAppCboHostPair2);
            this.HostAppTabPage.Controls.Add(this.hostAppCboHostPair1);
            this.HostAppTabPage.Controls.Add(this.hostAppCboResetPort);
            this.HostAppTabPage.Controls.Add(this.hostAppLDOModeComboBox);
            this.HostAppTabPage.Controls.Add(this.hostAppLDOModeLabel);
            this.HostAppTabPage.Controls.Add(this.hostAppLnaComboBox);
            this.HostAppTabPage.Controls.Add(this.hostAppLnaTypeLabel);
            this.HostAppTabPage.Controls.Add(this.hostAppTCXOFreqComboBox);
            this.HostAppTabPage.Controls.Add(this.hostAppTCXOFreqLabel);
            this.HostAppTabPage.Controls.Add(this.hostAppCboTrackerPort);
            this.HostAppTabPage.Controls.Add(this.hostAppTrackerPortLabel);
            this.HostAppTabPage.Location = new Point(4, 0x16);
            this.HostAppTabPage.Name = "HostAppTabPage";
            this.HostAppTabPage.Padding = new Padding(3);
            this.HostAppTabPage.Size = new Size(470, 0xf7);
            this.HostAppTabPage.TabIndex = 2;
            this.HostAppTabPage.Text = "Host App";
            this.HostAppTabPage.UseVisualStyleBackColor = true;
            this.hostAppVersionCheckBox.AutoSize = true;
            this.hostAppVersionCheckBox.Location = new Point(0x67, 0x11);
            this.hostAppVersionCheckBox.Name = "hostAppVersionCheckBox";
            this.hostAppVersionCheckBox.Size = new Size(0x9a, 0x11);
            this.hostAppVersionCheckBox.TabIndex = 0x1c;
            this.hostAppVersionCheckBox.Text = "Version >= 4.1.0 (SSIV 2.0)";
            this.hostAppVersionCheckBox.UseVisualStyleBackColor = true;
            this.frmCommSettingsAdvancedHostAppBtn.Location = new Point(14, 0x9b);
            this.frmCommSettingsAdvancedHostAppBtn.Name = "frmCommSettingsAdvancedHostAppBtn";
            this.frmCommSettingsAdvancedHostAppBtn.Size = new Size(0xa5, 0x1c);
            this.frmCommSettingsAdvancedHostAppBtn.TabIndex = 0x1b;
            this.frmCommSettingsAdvancedHostAppBtn.Text = "Advanced Settings";
            this.frmCommSettingsAdvancedHostAppBtn.UseVisualStyleBackColor = true;
            this.hostAppEEChkBox.AutoSize = true;
            this.hostAppEEChkBox.Location = new Point(0x19, 0x11);
            this.hostAppEEChkBox.Name = "hostAppEEChkBox";
            this.hostAppEEChkBox.Size = new Size(0x45, 0x11);
            this.hostAppEEChkBox.TabIndex = 2;
            this.hostAppEEChkBox.Text = "Ext Eph?";
            this.hostAppEEChkBox.UseVisualStyleBackColor = true;
            this.hostAppEEChkBox.CheckedChanged += new EventHandler(this.hostAppEEChkBox_CheckedChange);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x1b0, 0x33);
            this.label10.Name = "label10";
            this.label10.Size = new Size(20, 13);
            this.label10.TabIndex = 0x1a;
            this.label10.Text = "Hz";
            this.hostAppSWDirBrowseBtn.Location = new Point(0x1aa, 0xc9);
            this.hostAppSWDirBrowseBtn.Name = "hostAppSWDirBrowseBtn";
            this.hostAppSWDirBrowseBtn.Size = new Size(0x1a, 0x17);
            this.hostAppSWDirBrowseBtn.TabIndex = 0x19;
            this.hostAppSWDirBrowseBtn.Text = "...";
            this.hostAppSWDirBrowseBtn.UseVisualStyleBackColor = true;
            this.hostAppSWDirBrowseBtn.Click += new EventHandler(this.hostAppSWDirBrowseBtn_Click);
            this.hostAppSWDirTxtBox.Location = new Point(14, 0xcc);
            this.hostAppSWDirTxtBox.Name = "hostAppSWDirTxtBox";
            this.hostAppSWDirTxtBox.Size = new Size(0x196, 20);
            this.hostAppSWDirTxtBox.TabIndex = 5;
            this.hostAppSWDirLabel.AutoSize = true;
            this.hostAppSWDirLabel.Location = new Point(0x12, 0xba);
            this.hostAppSWDirLabel.Name = "hostAppSWDirLabel";
            this.hostAppSWDirLabel.Size = new Size(0x76, 13);
            this.hostAppSWDirLabel.TabIndex = 0x17;
            this.hostAppSWDirLabel.Text = "Host Software File Path";
            this.hostAppPairLabel.AutoSize = true;
            this.hostAppPairLabel.Location = new Point(0x16, 0x69);
            this.hostAppPairLabel.Name = "hostAppPairLabel";
            this.hostAppPairLabel.Size = new Size(0x35, 13);
            this.hostAppPairLabel.TabIndex = 3;
            this.hostAppPairLabel.Text = "Host Pair:";
            this.hostAppResetPortLabel.AutoSize = true;
            this.hostAppResetPortLabel.Location = new Point(0x16, 0x44);
            this.hostAppResetPortLabel.Name = "hostAppResetPortLabel";
            this.hostAppResetPortLabel.Size = new Size(0x41, 13);
            this.hostAppResetPortLabel.TabIndex = 2;
            this.hostAppResetPortLabel.Text = "On/Off Port:";
            this.hostAppCboHostPair2.FormattingEnabled = true;
            this.hostAppCboHostPair2.Location = new Point(0x67, 0x7b);
            this.hostAppCboHostPair2.Name = "hostAppCboHostPair2";
            this.hostAppCboHostPair2.Size = new Size(0x4c, 0x15);
            this.hostAppCboHostPair2.TabIndex = 0;
            this.hostAppCboHostPair2.SelectedIndexChanged += new EventHandler(this.hostAppCboHostPair2_SelectedIndexChanged);
            this.hostAppCboHostPair1.FormattingEnabled = true;
            this.hostAppCboHostPair1.Location = new Point(0x67, 0x66);
            this.hostAppCboHostPair1.Name = "hostAppCboHostPair1";
            this.hostAppCboHostPair1.Size = new Size(0x4c, 0x15);
            this.hostAppCboHostPair1.TabIndex = 0;
            this.hostAppCboResetPort.FormattingEnabled = true;
            this.hostAppCboResetPort.Location = new Point(0x67, 0x40);
            this.hostAppCboResetPort.Name = "hostAppCboResetPort";
            this.hostAppCboResetPort.Size = new Size(0x4c, 0x15);
            this.hostAppCboResetPort.TabIndex = 0;
            this.hostAppLDOModeComboBox.FormattingEnabled = true;
            this.hostAppLDOModeComboBox.Items.AddRange(new object[] { "Disable", "Enable" });
            this.hostAppLDOModeComboBox.Location = new Point(0x12b, 0x59);
            this.hostAppLDOModeComboBox.Name = "hostAppLDOModeComboBox";
            this.hostAppLDOModeComboBox.Size = new Size(0x79, 0x15);
            this.hostAppLDOModeComboBox.TabIndex = 0;
            this.hostAppLDOModeComboBox.SelectedIndexChanged += new EventHandler(this.hostAppLDOModeComboBox_SelectedIndexChanged);
            this.hostAppLDOModeLabel.AutoSize = true;
            this.hostAppLDOModeLabel.Location = new Point(0xd7, 0x5d);
            this.hostAppLDOModeLabel.Name = "hostAppLDOModeLabel";
            this.hostAppLDOModeLabel.Size = new Size(0x3e, 13);
            this.hostAppLDOModeLabel.TabIndex = 0;
            this.hostAppLDOModeLabel.Text = "LDO Mode:";
            this.hostAppLnaComboBox.FormattingEnabled = true;
            this.hostAppLnaComboBox.Items.AddRange(new object[] { "Internal", "External" });
            this.hostAppLnaComboBox.Location = new Point(0x12b, 0x44);
            this.hostAppLnaComboBox.Name = "hostAppLnaComboBox";
            this.hostAppLnaComboBox.Size = new Size(0x79, 0x15);
            this.hostAppLnaComboBox.TabIndex = 0;
            this.hostAppLnaComboBox.SelectedIndexChanged += new EventHandler(this.hostAppLnaComboBox_SelectedIndexChanged);
            this.hostAppLnaTypeLabel.AutoSize = true;
            this.hostAppLnaTypeLabel.Location = new Point(0xd7, 0x48);
            this.hostAppLnaTypeLabel.Name = "hostAppLnaTypeLabel";
            this.hostAppLnaTypeLabel.Size = new Size(0x3a, 13);
            this.hostAppLnaTypeLabel.TabIndex = 0;
            this.hostAppLnaTypeLabel.Text = "LNA Type:";
            this.hostAppTCXOFreqComboBox.FormattingEnabled = true;
            this.hostAppTCXOFreqComboBox.Items.AddRange(new object[] { "16369000", "19200000", "26000000", "User Defined" });
            this.hostAppTCXOFreqComboBox.Location = new Point(0x12b, 0x2f);
            this.hostAppTCXOFreqComboBox.Name = "hostAppTCXOFreqComboBox";
            this.hostAppTCXOFreqComboBox.Size = new Size(0x79, 0x15);
            this.hostAppTCXOFreqComboBox.TabIndex = 0;
            this.hostAppTCXOFreqComboBox.SelectedIndexChanged += new EventHandler(this.hostAppTCXOFreqComboBox_SelectedIndexChanged);
            this.hostAppTCXOFreqLabel.AutoSize = true;
            this.hostAppTCXOFreqLabel.Location = new Point(0xd7, 0x33);
            this.hostAppTCXOFreqLabel.Name = "hostAppTCXOFreqLabel";
            this.hostAppTCXOFreqLabel.Size = new Size(0x3f, 13);
            this.hostAppTCXOFreqLabel.TabIndex = 0;
            this.hostAppTCXOFreqLabel.Text = "TCXO Freq:";
            this.hostAppCboTrackerPort.FormattingEnabled = true;
            this.hostAppCboTrackerPort.Location = new Point(0x67, 0x2b);
            this.hostAppCboTrackerPort.Name = "hostAppCboTrackerPort";
            this.hostAppCboTrackerPort.Size = new Size(0x4c, 0x15);
            this.hostAppCboTrackerPort.TabIndex = 0;
            this.hostAppTrackerPortLabel.AutoSize = true;
            this.hostAppTrackerPortLabel.Location = new Point(0x16, 0x2f);
            this.hostAppTrackerPortLabel.Name = "hostAppTrackerPortLabel";
            this.hostAppTrackerPortLabel.Size = new Size(0x45, 13);
            this.hostAppTrackerPortLabel.TabIndex = 0;
            this.hostAppTrackerPortLabel.Text = "Tracker Port:";
            this.RS232TabPage.Controls.Add(this.pulseOnOffPortLabel);
            this.RS232TabPage.Controls.Add(this.gsd4eOnOffPortLabel);
            this.RS232TabPage.Controls.Add(this.gsd4eOnOffPortComboBox);
            this.RS232TabPage.Controls.Add(this.onOffActionComboBox);
            this.RS232TabPage.Controls.Add(this.autoBaudChkBox);
            this.RS232TabPage.Controls.Add(this.frmCommSettigsRS232AdvancedSettingsBtn);
            this.RS232TabPage.Controls.Add(this.frmCommSettingsRS232AdvanceSettingsGrpBox);
            this.RS232TabPage.Controls.Add(this.cboBaud);
            this.RS232TabPage.Controls.Add(this.label2);
            this.RS232TabPage.Controls.Add(this.Label1);
            this.RS232TabPage.Controls.Add(this.cboPort);
            this.RS232TabPage.Location = new Point(4, 0x16);
            this.RS232TabPage.Name = "RS232TabPage";
            this.RS232TabPage.Padding = new Padding(3);
            this.RS232TabPage.Size = new Size(470, 0xf7);
            this.RS232TabPage.TabIndex = 0;
            this.RS232TabPage.Text = "RS232";
            this.RS232TabPage.UseVisualStyleBackColor = true;
            this.pulseOnOffPortLabel.AutoSize = true;
            this.pulseOnOffPortLabel.Location = new Point(0x16, 210);
            this.pulseOnOffPortLabel.Name = "pulseOnOffPortLabel";
            this.pulseOnOffPortLabel.Size = new Size(0x48, 13);
            this.pulseOnOffPortLabel.TabIndex = 0x1d;
            this.pulseOnOffPortLabel.Text = "Pulse On/Off:";
            this.gsd4eOnOffPortLabel.AutoSize = true;
            this.gsd4eOnOffPortLabel.Location = new Point(0x16, 0xac);
            this.gsd4eOnOffPortLabel.Name = "gsd4eOnOffPortLabel";
            this.gsd4eOnOffPortLabel.Size = new Size(0x41, 13);
            this.gsd4eOnOffPortLabel.TabIndex = 0x15;
            this.gsd4eOnOffPortLabel.Text = "On/Off Port:";
            this.gsd4eOnOffPortComboBox.FormattingEnabled = true;
            this.gsd4eOnOffPortComboBox.Location = new Point(100, 0xa8);
            this.gsd4eOnOffPortComboBox.Name = "gsd4eOnOffPortComboBox";
            this.gsd4eOnOffPortComboBox.Size = new Size(0x4c, 0x15);
            this.gsd4eOnOffPortComboBox.TabIndex = 20;
            this.onOffActionComboBox.FormattingEnabled = true;
            this.onOffActionComboBox.Items.AddRange(new object[] { "Toggle", "High", "Low" });
            this.onOffActionComboBox.Location = new Point(100, 0xce);
            this.onOffActionComboBox.Name = "onOffActionComboBox";
            this.onOffActionComboBox.Size = new Size(0x4c, 0x15);
            this.onOffActionComboBox.TabIndex = 0x1c;
            this.onOffActionComboBox.SelectedIndexChanged += new EventHandler(this.onOffActionComboBox_SelectedIndexChanged);
            this.autoBaudChkBox.AutoSize = true;
            this.autoBaudChkBox.Location = new Point(0x19, 0x7e);
            this.autoBaudChkBox.Name = "autoBaudChkBox";
            this.autoBaudChkBox.Size = new Size(0xce, 0x11);
            this.autoBaudChkBox.TabIndex = 0x13;
            this.autoBaudChkBox.Text = "Auto Detect Protocol and Baud Rate?";
            this.autoBaudChkBox.UseVisualStyleBackColor = true;
            this.frmCommSettigsRS232AdvancedSettingsBtn.Location = new Point(0x19, 0x54);
            this.frmCommSettigsRS232AdvancedSettingsBtn.Name = "frmCommSettigsRS232AdvancedSettingsBtn";
            this.frmCommSettigsRS232AdvancedSettingsBtn.Size = new Size(0x94, 0x17);
            this.frmCommSettigsRS232AdvancedSettingsBtn.TabIndex = 0x12;
            this.frmCommSettigsRS232AdvancedSettingsBtn.Text = "Advanced Settings";
            this.frmCommSettigsRS232AdvancedSettingsBtn.UseVisualStyleBackColor = true;
            this.frmCommSettigsRS232AdvancedSettingsBtn.Click += new EventHandler(this.frmCommSettigsRS232AdvanceSettingsBtn_Click);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.cboFlowControl);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.cboReadBuffer);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.label17);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.label11);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.cboParity);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.label3);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.label5);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.cboStop);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.label4);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Controls.Add(this.cboData);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Location = new Point(0xf5, 0x24);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Name = "frmCommSettingsRS232AdvanceSettingsGrpBox";
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Size = new Size(200, 0xa9);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.TabIndex = 0x11;
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.TabStop = false;
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Text = "Advance Settings";
            this.cboFlowControl.FormattingEnabled = true;
            this.cboFlowControl.Items.AddRange(new object[] { "None", "Xon/Xoff", "Hardware", "Both" });
            this.cboFlowControl.Location = new Point(100, 0x56);
            this.cboFlowControl.Name = "cboFlowControl";
            this.cboFlowControl.Size = new Size(0x4c, 0x15);
            this.cboFlowControl.TabIndex = 0x15;
            this.cboFlowControl.SelectedIndexChanged += new EventHandler(this.cboFlowControl_SelectedIndexChanged);
            this.cboReadBuffer.FormattingEnabled = true;
            this.cboReadBuffer.Items.AddRange(new object[] { "4096", "5120", "6144", "7168", "8192" });
            this.cboReadBuffer.Location = new Point(100, 0x6b);
            this.cboReadBuffer.Name = "cboReadBuffer";
            this.cboReadBuffer.Size = new Size(0x4c, 0x15);
            this.cboReadBuffer.TabIndex = 0x15;
            this.cboReadBuffer.SelectedIndexChanged += new EventHandler(this.cboReadBuffer_SelectedIndexChanged);
            this.label17.AutoSize = true;
            this.label17.Location = new Point(0x10, 90);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x44, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Flow Control:";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x10, 0x6f);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x43, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Read Buffer:";
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Location = new Point(100, 0x17);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new Size(0x4c, 0x15);
            this.cboParity.TabIndex = 2;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x10, 0x1b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 13);
            this.label3.TabIndex = 0x11;
            this.label3.Text = "Parity:";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x10, 0x45);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x35, 13);
            this.label5.TabIndex = 0x13;
            this.label5.Text = "Data Bits:";
            this.cboStop.FormattingEnabled = true;
            this.cboStop.Location = new Point(100, 0x2c);
            this.cboStop.Name = "cboStop";
            this.cboStop.Size = new Size(0x4c, 0x15);
            this.cboStop.TabIndex = 3;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x10, 0x30);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x34, 13);
            this.label4.TabIndex = 0x12;
            this.label4.Text = "Stop Bits:";
            this.cboData.FormattingEnabled = true;
            this.cboData.Items.AddRange(new object[] { "7", "8", "9" });
            this.cboData.Location = new Point(100, 0x41);
            this.cboData.Name = "cboData";
            this.cboData.Size = new Size(0x4c, 0x15);
            this.cboData.TabIndex = 4;
            this.cboBaud.FormattingEnabled = true;
            this.cboBaud.Items.AddRange(new object[] { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600", "1228800" });
            this.cboBaud.Location = new Point(100, 0x37);
            this.cboBaud.Name = "cboBaud";
            this.cboBaud.Size = new Size(0x4c, 0x15);
            this.cboBaud.TabIndex = 1;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x16, 0x3b);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3d, 13);
            this.label2.TabIndex = 0x10;
            this.label2.Text = "Baud Rate:";
            this.Label1.AutoSize = true;
            this.Label1.Location = new Point(0x16, 40);
            this.Label1.Name = "Label1";
            this.Label1.Size = new Size(0x1d, 13);
            this.Label1.TabIndex = 15;
            this.Label1.Text = "Port:";
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new Point(100, 0x24);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new Size(0x4c, 0x15);
            this.cboPort.TabIndex = 0;
            this.cboPort.SelectedIndexChanged += new EventHandler(this.cboPort_SelectedIndexChanged);
            this.TcpIpTabPage.Controls.Add(this.COMServerGroupBox);
            this.TcpIpTabPage.Controls.Add(this.COMClientIPGroupBox);
            this.TcpIpTabPage.Controls.Add(this.COMTcpIpModeGroupBox);
            this.TcpIpTabPage.Location = new Point(4, 0x16);
            this.TcpIpTabPage.Name = "TcpIpTabPage";
            this.TcpIpTabPage.Padding = new Padding(3);
            this.TcpIpTabPage.Size = new Size(470, 0xf7);
            this.TcpIpTabPage.TabIndex = 1;
            this.TcpIpTabPage.Text = "TCP/IP";
            this.TcpIpTabPage.UseVisualStyleBackColor = true;
            this.COMServerGroupBox.Controls.Add(this.label8);
            this.COMServerGroupBox.Controls.Add(this.label9);
            this.COMServerGroupBox.Controls.Add(this.COMServerIPTextBox);
            this.COMServerGroupBox.Controls.Add(this.hostAppCboServerTCPIPPort);
            this.COMServerGroupBox.Location = new Point(0xf8, 0x5d);
            this.COMServerGroupBox.Name = "COMServerGroupBox";
            this.COMServerGroupBox.Size = new Size(0xd4, 0x8a);
            this.COMServerGroupBox.TabIndex = 4;
            this.COMServerGroupBox.TabStop = false;
            this.COMServerGroupBox.Text = "Server Address and Port";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x11, 0x4e);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x1d, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Port:";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x1a, 0x2b);
            this.label9.Name = "label9";
            this.label9.Size = new Size(20, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "IP:";
            this.COMServerIPTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.COMServerIPTextBox.Location = new Point(0x37, 40);
            this.COMServerIPTextBox.Name = "COMServerIPTextBox";
            this.COMServerIPTextBox.Size = new Size(0x6a, 20);
            this.COMServerIPTextBox.TabIndex = 0;
            this.COMServerIPTextBox.Text = "127.0.0.1";
            this.hostAppCboServerTCPIPPort.FormattingEnabled = true;
            this.hostAppCboServerTCPIPPort.Items.AddRange(new object[] { "7555", "7556", "7557", "7558", "7559", "7560" });
            this.hostAppCboServerTCPIPPort.Location = new Point(0x38, 0x4a);
            this.hostAppCboServerTCPIPPort.Name = "hostAppCboServerTCPIPPort";
            this.hostAppCboServerTCPIPPort.Size = new Size(0x69, 0x15);
            this.hostAppCboServerTCPIPPort.TabIndex = 0;
            this.hostAppCboServerTCPIPPort.SelectedIndexChanged += new EventHandler(this.hostAppCboTCPIPPort_SelectedIndexChanged);
            this.COMClientIPGroupBox.Controls.Add(this.label7);
            this.COMClientIPGroupBox.Controls.Add(this.label6);
            this.COMClientIPGroupBox.Controls.Add(this.COMClientIPAddrTextBox);
            this.COMClientIPGroupBox.Controls.Add(this.hostAppCboTCPIPClientPort);
            this.COMClientIPGroupBox.Location = new Point(0x12, 0x5d);
            this.COMClientIPGroupBox.Name = "COMClientIPGroupBox";
            this.COMClientIPGroupBox.Size = new Size(0xd4, 0x8a);
            this.COMClientIPGroupBox.TabIndex = 3;
            this.COMClientIPGroupBox.TabStop = false;
            this.COMClientIPGroupBox.Text = "Client Address and Port";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x11, 0x4e);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x1d, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Port:";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x1a, 0x2b);
            this.label6.Name = "label6";
            this.label6.Size = new Size(20, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "IP:";
            this.COMClientIPAddrTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.COMClientIPAddrTextBox.Location = new Point(0x35, 40);
            this.COMClientIPAddrTextBox.Name = "COMClientIPAddrTextBox";
            this.COMClientIPAddrTextBox.Size = new Size(0x6a, 20);
            this.COMClientIPAddrTextBox.TabIndex = 0;
            this.COMClientIPAddrTextBox.Text = "127.0.0.1";
            this.hostAppCboTCPIPClientPort.FormattingEnabled = true;
            this.hostAppCboTCPIPClientPort.Items.AddRange(new object[] { "7555", "7556", "7557", "7558", "7559", "7560" });
            this.hostAppCboTCPIPClientPort.Location = new Point(0x35, 0x4b);
            this.hostAppCboTCPIPClientPort.Name = "hostAppCboTCPIPClientPort";
            this.hostAppCboTCPIPClientPort.Size = new Size(0x69, 0x15);
            this.hostAppCboTCPIPClientPort.TabIndex = 0;
            this.hostAppCboTCPIPClientPort.SelectedIndexChanged += new EventHandler(this.hostAppCboTCPIPPort_SelectedIndexChanged);
            this.COMTcpIpModeGroupBox.Controls.Add(this.COMTcpIPServerRadioButton);
            this.COMTcpIpModeGroupBox.Controls.Add(this.COMTcpIPClientRadioButton);
            this.COMTcpIpModeGroupBox.Location = new Point(0x12, 0x11);
            this.COMTcpIpModeGroupBox.Name = "COMTcpIpModeGroupBox";
            this.COMTcpIpModeGroupBox.Size = new Size(0xd4, 0x39);
            this.COMTcpIpModeGroupBox.TabIndex = 2;
            this.COMTcpIpModeGroupBox.TabStop = false;
            this.COMTcpIpModeGroupBox.Text = "TCP/IP Mode Selection";
            this.COMTcpIPServerRadioButton.AutoSize = true;
            this.COMTcpIPServerRadioButton.Location = new Point(0x6c, 20);
            this.COMTcpIPServerRadioButton.Name = "COMTcpIPServerRadioButton";
            this.COMTcpIPServerRadioButton.Size = new Size(0x5f, 0x11);
            this.COMTcpIPServerRadioButton.TabIndex = 3;
            this.COMTcpIPServerRadioButton.TabStop = true;
            this.COMTcpIPServerRadioButton.Text = "TCP/IP Server";
            this.COMTcpIPServerRadioButton.UseVisualStyleBackColor = true;
            this.COMTcpIPServerRadioButton.CheckedChanged += new EventHandler(this.COMTcpIPServerRadioButton_CheckedChanged);
            this.COMTcpIPClientRadioButton.AutoSize = true;
            this.COMTcpIPClientRadioButton.Location = new Point(10, 20);
            this.COMTcpIPClientRadioButton.Name = "COMTcpIPClientRadioButton";
            this.COMTcpIPClientRadioButton.Size = new Size(90, 0x11);
            this.COMTcpIPClientRadioButton.TabIndex = 2;
            this.COMTcpIPClientRadioButton.TabStop = true;
            this.COMTcpIPClientRadioButton.Text = "TCP/IP Client";
            this.COMTcpIPClientRadioButton.UseVisualStyleBackColor = true;
            this.COMTcpIPClientRadioButton.CheckedChanged += new EventHandler(this.COMTcpIPClientRadioButton_CheckedChanged);
            this.ExtEphTabPage.Controls.Add(this.EEgroupBoxCGEE);
            this.ExtEphTabPage.Controls.Add(this.EEgroupBoxSGEE);
            this.ExtEphTabPage.Controls.Add(this.EElabel_Select);
            this.ExtEphTabPage.Controls.Add(this.extEphComboBox_Select);
            this.ExtEphTabPage.Location = new Point(4, 0x16);
            this.ExtEphTabPage.Name = "ExtEphTabPage";
            this.ExtEphTabPage.Padding = new Padding(3);
            this.ExtEphTabPage.Size = new Size(470, 0xf7);
            this.ExtEphTabPage.TabIndex = 4;
            this.ExtEphTabPage.Text = "Ext Eph";
            this.ExtEphTabPage.UseVisualStyleBackColor = true;
            this.EEgroupBoxCGEE.Controls.Add(this.extEphEditBox_BankTime);
            this.EEgroupBoxCGEE.Controls.Add(this.EElabel_Banktime);
            this.EEgroupBoxCGEE.Location = new Point(0xf3, 0x24);
            this.EEgroupBoxCGEE.Name = "EEgroupBoxCGEE";
            this.EEgroupBoxCGEE.Size = new Size(0xd8, 0xcd);
            this.EEgroupBoxCGEE.TabIndex = 12;
            this.EEgroupBoxCGEE.TabStop = false;
            this.EEgroupBoxCGEE.Text = "CGEE";
            this.extEphEditBox_BankTime.Location = new Point(0x63, 0x10);
            this.extEphEditBox_BankTime.Name = "extEphEditBox_BankTime";
            this.extEphEditBox_BankTime.Size = new Size(0x38, 20);
            this.extEphEditBox_BankTime.TabIndex = 6;
            this.extEphEditBox_BankTime.Text = "86400";
            this.EElabel_Banktime.AutoSize = true;
            this.EElabel_Banktime.Location = new Point(12, 20);
            this.EElabel_Banktime.Name = "EElabel_Banktime";
            this.EElabel_Banktime.Size = new Size(0x54, 13);
            this.EElabel_Banktime.TabIndex = 6;
            this.EElabel_Banktime.Text = "Bank Time (sec)";
            this.EEgroupBoxSGEE.Controls.Add(this.extEphComboBox_ServerName);
            this.EEgroupBoxSGEE.Controls.Add(this.extEphComboBox_EEDay);
            this.EEgroupBoxSGEE.Controls.Add(this.EElabel_ServerName);
            this.EEgroupBoxSGEE.Controls.Add(this.extEphEditBox_AuthCode);
            this.EEgroupBoxSGEE.Controls.Add(this.extEphEditBox_ServerPort);
            this.EEgroupBoxSGEE.Controls.Add(this.EElabel_Daynum);
            this.EEgroupBoxSGEE.Controls.Add(this.EElabel_AuthCode);
            this.EEgroupBoxSGEE.Controls.Add(this.EElabel_ServerPort);
            this.EEgroupBoxSGEE.Location = new Point(11, 0x24);
            this.EEgroupBoxSGEE.Name = "EEgroupBoxSGEE";
            this.EEgroupBoxSGEE.Size = new Size(0xdd, 0xcd);
            this.EEgroupBoxSGEE.TabIndex = 0;
            this.EEgroupBoxSGEE.TabStop = false;
            this.EEgroupBoxSGEE.Text = "SGEE";
            this.extEphComboBox_ServerName.FormattingEnabled = true;
            this.extEphComboBox_ServerName.Items.AddRange(new object[] { "sirfgetee.sirf.com", "eedemo1.sirf.com", "sgee1.sirf.com" });
            this.extEphComboBox_ServerName.Location = new Point(6, 0x20);
            this.extEphComboBox_ServerName.Name = "extEphComboBox_ServerName";
            this.extEphComboBox_ServerName.Size = new Size(0x79, 0x15);
            this.extEphComboBox_ServerName.TabIndex = 2;
            this.extEphComboBox_ServerName.Text = "sirfgetee.sirf.com";
            this.extEphComboBox_EEDay.FormattingEnabled = true;
            this.extEphComboBox_EEDay.Items.AddRange(new object[] { "1", "3", "5", "7" });
            this.extEphComboBox_EEDay.Location = new Point(6, 0x7b);
            this.extEphComboBox_EEDay.Name = "extEphComboBox_EEDay";
            this.extEphComboBox_EEDay.Size = new Size(0x42, 0x15);
            this.extEphComboBox_EEDay.TabIndex = 4;
            this.EElabel_ServerName.AutoSize = true;
            this.EElabel_ServerName.Location = new Point(6, 0x10);
            this.EElabel_ServerName.Name = "EElabel_ServerName";
            this.EElabel_ServerName.Size = new Size(0x48, 13);
            this.EElabel_ServerName.TabIndex = 8;
            this.EElabel_ServerName.Text = "Server Name:";
            this.extEphEditBox_AuthCode.Location = new Point(6, 0xa8);
            this.extEphEditBox_AuthCode.Name = "extEphEditBox_AuthCode";
            this.extEphEditBox_AuthCode.Size = new Size(0xc2, 20);
            this.extEphEditBox_AuthCode.TabIndex = 5;
            this.extEphEditBox_ServerPort.Location = new Point(6, 0x4e);
            this.extEphEditBox_ServerPort.Name = "extEphEditBox_ServerPort";
            this.extEphEditBox_ServerPort.Size = new Size(0x42, 20);
            this.extEphEditBox_ServerPort.TabIndex = 3;
            this.extEphEditBox_ServerPort.Text = "80";
            this.EElabel_Daynum.AutoSize = true;
            this.EElabel_Daynum.Location = new Point(8, 0x6a);
            this.EElabel_Daynum.Name = "EElabel_Daynum";
            this.EElabel_Daynum.Size = new Size(0x2e, 13);
            this.EElabel_Daynum.TabIndex = 9;
            this.EElabel_Daynum.Text = "EE Day:";
            this.EElabel_AuthCode.AutoSize = true;
            this.EElabel_AuthCode.Location = new Point(6, 0x97);
            this.EElabel_AuthCode.Name = "EElabel_AuthCode";
            this.EElabel_AuthCode.Size = new Size(0x6a, 13);
            this.EElabel_AuthCode.TabIndex = 9;
            this.EElabel_AuthCode.Text = "Authentication Code:";
            this.EElabel_ServerPort.AutoSize = true;
            this.EElabel_ServerPort.Location = new Point(6, 0x3d);
            this.EElabel_ServerPort.Name = "EElabel_ServerPort";
            this.EElabel_ServerPort.Size = new Size(0x3f, 13);
            this.EElabel_ServerPort.TabIndex = 9;
            this.EElabel_ServerPort.Text = "Server Port:";
            this.EElabel_Select.AutoSize = true;
            this.EElabel_Select.Location = new Point(11, 13);
            this.EElabel_Select.Name = "EElabel_Select";
            this.EElabel_Select.Size = new Size(40, 13);
            this.EElabel_Select.TabIndex = 5;
            this.EElabel_Select.Text = "Select:";
            this.extEphComboBox_Select.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.extEphComboBox_Select.FormattingEnabled = true;
            this.extEphComboBox_Select.Items.AddRange(new object[] { "No EE", "CGEE", "SGEE", "Mixed SGEE + CGEE" });
            this.extEphComboBox_Select.Location = new Point(0x35, 9);
            this.extEphComboBox_Select.Name = "extEphComboBox_Select";
            this.extEphComboBox_Select.Size = new Size(0x83, 0x15);
            this.extEphComboBox_Select.TabIndex = 1;
            this.extEphComboBox_Select.Text = "No EE";
            this.extEphComboBox_Select.SelectedIndexChanged += new EventHandler(this.extEphComboBox_Select_SelectedIndexChanged);
            this.I2CTabPage.Controls.Add(this.checkBox_SlaveMode);
            this.I2CTabPage.Controls.Add(this.I2C_SerialNumMaster);
            this.I2CTabPage.Controls.Add(this.I2C_PortTxtBoxMaster);
            this.I2CTabPage.Controls.Add(this.I2C_SerialNum);
            this.I2CTabPage.Controls.Add(this.label16);
            this.I2CTabPage.Controls.Add(this.I2C_DetectBtn);
            this.I2CTabPage.Controls.Add(this.I2C_PortTxtBox);
            this.I2CTabPage.Controls.Add(this.I2C_SlaveTxtBx);
            this.I2CTabPage.Controls.Add(this.I2C_MasterTxtBx);
            this.I2CTabPage.Controls.Add(this.label15);
            this.I2CTabPage.Controls.Add(this.label14);
            this.I2CTabPage.Controls.Add(this.label13);
            this.I2CTabPage.Location = new Point(4, 0x16);
            this.I2CTabPage.Name = "I2CTabPage";
            this.I2CTabPage.Padding = new Padding(3);
            this.I2CTabPage.Size = new Size(470, 0xf7);
            this.I2CTabPage.TabIndex = 5;
            this.I2CTabPage.Text = "  I2C";
            this.I2CTabPage.UseVisualStyleBackColor = true;
            this.checkBox_SlaveMode.AutoSize = true;
            this.checkBox_SlaveMode.Location = new Point(0x157, 0x13);
            this.checkBox_SlaveMode.Name = "checkBox_SlaveMode";
            this.checkBox_SlaveMode.Size = new Size(0x53, 0x11);
            this.checkBox_SlaveMode.TabIndex = 0x23;
            this.checkBox_SlaveMode.Text = "Slave Mode";
            this.checkBox_SlaveMode.UseMnemonic = false;
            this.checkBox_SlaveMode.UseVisualStyleBackColor = true;
            this.I2C_SerialNumMaster.ForeColor = SystemColors.ButtonShadow;
            this.I2C_SerialNumMaster.Location = new Point(0x9b, 0x40);
            this.I2C_SerialNumMaster.Name = "I2C_SerialNumMaster";
            this.I2C_SerialNumMaster.Size = new Size(0x8e, 0x11);
            this.I2C_SerialNumMaster.TabIndex = 0x22;
            this.I2C_SerialNumMaster.Text = "Serial Number";
            this.I2C_PortTxtBoxMaster.Location = new Point(0x6a, 0x3e);
            this.I2C_PortTxtBoxMaster.Name = "I2C_PortTxtBoxMaster";
            this.I2C_PortTxtBoxMaster.Size = new Size(0x10, 20);
            this.I2C_PortTxtBoxMaster.TabIndex = 0x21;
            this.I2C_PortTxtBoxMaster.Text = "1";
            this.I2C_SerialNum.ForeColor = SystemColors.ButtonShadow;
            this.I2C_SerialNum.Location = new Point(0x9b, 0x22);
            this.I2C_SerialNum.Name = "I2C_SerialNum";
            this.I2C_SerialNum.Size = new Size(0x8e, 0x11);
            this.I2C_SerialNum.TabIndex = 0x1f;
            this.I2C_SerialNum.Text = "Serial Number";
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x11, 0x42);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x40, 13);
            this.label16.TabIndex = 0x20;
            this.label16.Text = "Write Port #";
            this.I2C_DetectBtn.Location = new Point(0x15f, 0x3d);
            this.I2C_DetectBtn.Name = "I2C_DetectBtn";
            this.I2C_DetectBtn.Size = new Size(0x4b, 0x17);
            this.I2C_DetectBtn.TabIndex = 8;
            this.I2C_DetectBtn.Text = "&Detect";
            this.I2C_DetectBtn.UseVisualStyleBackColor = true;
            this.I2C_DetectBtn.Click += new EventHandler(this.I2CDetectBtn_Click);
            this.I2C_PortTxtBox.Location = new Point(0x6a, 0x20);
            this.I2C_PortTxtBox.Name = "I2C_PortTxtBox";
            this.I2C_PortTxtBox.Size = new Size(0x10, 20);
            this.I2C_PortTxtBox.TabIndex = 7;
            this.I2C_PortTxtBox.Text = "0";
            this.I2C_SlaveTxtBx.Location = new Point(0x6a, 0x89);
            this.I2C_SlaveTxtBx.Name = "I2C_SlaveTxtBx";
            this.I2C_SlaveTxtBx.Size = new Size(0x22, 20);
            this.I2C_SlaveTxtBx.TabIndex = 6;
            this.I2C_SlaveTxtBx.Text = "62";
            this.I2C_MasterTxtBx.Location = new Point(0x6a, 0xa3);
            this.I2C_MasterTxtBx.Name = "I2C_MasterTxtBx";
            this.I2C_MasterTxtBx.Size = new Size(0x22, 20);
            this.I2C_MasterTxtBx.TabIndex = 5;
            this.I2C_MasterTxtBx.Text = "60";
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x11, 0x24);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x41, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Read Port #";
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x11, 0x8d);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x54, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Read Addr. :  0x";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x11, 0xa7);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x53, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Write Addr.:   0x";
            this.hostAppTCPIPRadioBtn.AutoSize = true;
            this.hostAppTCPIPRadioBtn.Location = new Point(0x7c, 0x1a);
            this.hostAppTCPIPRadioBtn.Name = "hostAppTCPIPRadioBtn";
            this.hostAppTCPIPRadioBtn.Size = new Size(0x3d, 0x11);
            this.hostAppTCPIPRadioBtn.TabIndex = 4;
            this.hostAppTCPIPRadioBtn.Text = "TCP/IP";
            this.hostAppTCPIPRadioBtn.UseVisualStyleBackColor = true;
            this.hostAppTCPIPRadioBtn.CheckedChanged += new EventHandler(this.hostAppRS232TCPIPRadioBtn_CheckedChanged);
            this.hostAppRS232RadioBtn.AutoSize = true;
            this.hostAppRS232RadioBtn.Checked = true;
            this.hostAppRS232RadioBtn.Location = new Point(0x12, 0x1a);
            this.hostAppRS232RadioBtn.Name = "hostAppRS232RadioBtn";
            this.hostAppRS232RadioBtn.Size = new Size(0x55, 0x11);
            this.hostAppRS232RadioBtn.TabIndex = 4;
            this.hostAppRS232RadioBtn.TabStop = true;
            this.hostAppRS232RadioBtn.Text = "RS232/USB";
            this.hostAppRS232RadioBtn.UseVisualStyleBackColor = true;
            this.hostAppRS232RadioBtn.CheckedChanged += new EventHandler(this.hostAppRS232RadioBtn_CheckedChanged);
            this.hostAppRunHostChkBox.AutoSize = true;
            this.hostAppRunHostChkBox.Location = new Point(0xa5, 0x3d);
            this.hostAppRunHostChkBox.Name = "hostAppRunHostChkBox";
            this.hostAppRunHostChkBox.Size = new Size(0x4d, 0x11);
            this.hostAppRunHostChkBox.TabIndex = 1;
            this.hostAppRunHostChkBox.Text = "Run Host?";
            this.hostAppRunHostChkBox.UseVisualStyleBackColor = true;
            this.hostAppRunHostChkBox.CheckedChanged += new EventHandler(this.hostAppRunHostChkBox_CheckedChanged);
            this.groupBox3.Controls.Add(this.frmCommOpenBufferSizeTxtBox);
            this.groupBox3.Controls.Add(this.frmCommOpenBufferSizeLabel);
            this.groupBox3.Controls.Add(this.rdoCSV);
            this.groupBox3.Controls.Add(this.rdoGP2);
            this.groupBox3.Controls.Add(this.rdoSSB);
            this.groupBox3.Controls.Add(this.rdoText);
            this.groupBox3.Controls.Add(this.rdoHex);
            this.groupBox3.Location = new Point(250, 0x13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xd5, 0x79);
            this.groupBox3.TabIndex = 0x18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "View Mode";
            this.frmCommOpenBufferSizeTxtBox.Location = new Point(0x74, 0x4a);
            this.frmCommOpenBufferSizeTxtBox.Name = "frmCommOpenBufferSizeTxtBox";
            this.frmCommOpenBufferSizeTxtBox.Size = new Size(0x37, 20);
            this.frmCommOpenBufferSizeTxtBox.TabIndex = 10;
            this.frmCommOpenBufferSizeTxtBox.Visible = false;
            this.frmCommOpenBufferSizeLabel.AutoSize = true;
            this.frmCommOpenBufferSizeLabel.Location = new Point(13, 0x4e);
            this.frmCommOpenBufferSizeLabel.Name = "frmCommOpenBufferSizeLabel";
            this.frmCommOpenBufferSizeLabel.Size = new Size(0x55, 13);
            this.frmCommOpenBufferSizeLabel.TabIndex = 12;
            this.frmCommOpenBufferSizeLabel.Text = "Buffer Size(lines)";
            this.frmCommOpenBufferSizeLabel.Visible = false;
            this.rdoCSV.AutoSize = true;
            this.rdoCSV.Location = new Point(0x10, 0x1d);
            this.rdoCSV.Name = "rdoCSV";
            this.rdoCSV.Size = new Size(0x2f, 0x11);
            this.rdoCSV.TabIndex = 9;
            this.rdoCSV.TabStop = true;
            this.rdoCSV.Text = "GPS";
            this.rdoCSV.UseVisualStyleBackColor = true;
            this.rdoGP2.AutoSize = true;
            this.rdoGP2.Location = new Point(0x10, 0x33);
            this.rdoGP2.Name = "rdoGP2";
            this.rdoGP2.Size = new Size(0x2e, 0x11);
            this.rdoGP2.TabIndex = 8;
            this.rdoGP2.TabStop = true;
            this.rdoGP2.Text = "GP2";
            this.rdoGP2.UseVisualStyleBackColor = true;
            this.rdoSSB.AutoSize = true;
            this.rdoSSB.Location = new Point(0x86, 0x1d);
            this.rdoSSB.Name = "rdoSSB";
            this.rdoSSB.Size = new Size(0x2e, 0x11);
            this.rdoSSB.TabIndex = 7;
            this.rdoSSB.TabStop = true;
            this.rdoSSB.Text = "SSB";
            this.rdoSSB.UseVisualStyleBackColor = true;
            this.rdoText.AutoSize = true;
            this.rdoText.Location = new Point(0x47, 0x1c);
            this.rdoText.Name = "rdoText";
            this.rdoText.Size = new Size(0x38, 0x11);
            this.rdoText.TabIndex = 6;
            this.rdoText.TabStop = true;
            this.rdoText.Text = "NMEA";
            this.rdoText.UseVisualStyleBackColor = true;
            this.rdoHex.AutoSize = true;
            this.rdoHex.Location = new Point(0x86, 0x31);
            this.rdoHex.Name = "rdoHex";
            this.rdoHex.Size = new Size(0x2c, 0x11);
            this.rdoHex.TabIndex = 5;
            this.rdoHex.TabStop = true;
            this.rdoHex.Text = "Hex";
            this.rdoHex.UseVisualStyleBackColor = true;
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.ai3Version);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.fVersion);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.cboAI3Version);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.frmComSettingsTTBRadioBtn);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.cboFVersion);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.frmComSettingsGSWRadioBtn);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.frmComSettingsSLCRadioBtn);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.frmCommOpenProtocolsLabel);
            this.frmCommOpenRxTypeGrpBox.Controls.Add(this.cboProtocols);
            this.frmCommOpenRxTypeGrpBox.Location = new Point(0x12, 0x13);
            this.frmCommOpenRxTypeGrpBox.Name = "frmCommOpenRxTypeGrpBox";
            this.frmCommOpenRxTypeGrpBox.Size = new Size(0xd5, 0x79);
            this.frmCommOpenRxTypeGrpBox.TabIndex = 0x17;
            this.frmCommOpenRxTypeGrpBox.TabStop = false;
            this.frmCommOpenRxTypeGrpBox.Text = "Rx Type";
            this.ai3Version.AutoSize = true;
            this.ai3Version.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ai3Version.Location = new Point(12, 0x5b);
            this.ai3Version.Name = "ai3Version";
            this.ai3Version.Size = new Size(0x3d, 13);
            this.ai3Version.TabIndex = 0x18;
            this.ai3Version.Text = "AI3 Version";
            this.fVersion.AutoSize = true;
            this.fVersion.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.fVersion.Location = new Point(12, 70);
            this.fVersion.Name = "fVersion";
            this.fVersion.Size = new Size(0x33, 13);
            this.fVersion.TabIndex = 0x18;
            this.fVersion.Text = "F Version";
            this.cboAI3Version.FormattingEnabled = true;
            this.cboAI3Version.Items.AddRange(new object[] { "1.0", "2.1", "2.2" });
            this.cboAI3Version.Location = new Point(0x4b, 0x57);
            this.cboAI3Version.Name = "cboAI3Version";
            this.cboAI3Version.Size = new Size(0x54, 0x15);
            this.cboAI3Version.TabIndex = 0x17;
            this.frmComSettingsTTBRadioBtn.AutoSize = true;
            this.frmComSettingsTTBRadioBtn.Location = new Point(0x90, 0x15);
            this.frmComSettingsTTBRadioBtn.Name = "frmComSettingsTTBRadioBtn";
            this.frmComSettingsTTBRadioBtn.Size = new Size(0x2e, 0x11);
            this.frmComSettingsTTBRadioBtn.TabIndex = 0x15;
            this.frmComSettingsTTBRadioBtn.Text = "TTB";
            this.frmComSettingsTTBRadioBtn.UseVisualStyleBackColor = true;
            this.frmComSettingsTTBRadioBtn.CheckedChanged += new EventHandler(this.frmComSettingsTTBRadioBtn_CheckedChanged);
            this.cboFVersion.FormattingEnabled = true;
            this.cboFVersion.Items.AddRange(new object[] { "1.0", "2.1" });
            this.cboFVersion.Location = new Point(0x4b, 0x42);
            this.cboFVersion.Name = "cboFVersion";
            this.cboFVersion.Size = new Size(0x54, 0x15);
            this.cboFVersion.TabIndex = 0x17;
            this.frmComSettingsGSWRadioBtn.AutoSize = true;
            this.frmComSettingsGSWRadioBtn.Location = new Point(0x4b, 0x15);
            this.frmComSettingsGSWRadioBtn.Name = "frmComSettingsGSWRadioBtn";
            this.frmComSettingsGSWRadioBtn.Size = new Size(0x33, 0x11);
            this.frmComSettingsGSWRadioBtn.TabIndex = 12;
            this.frmComSettingsGSWRadioBtn.Text = "GSW";
            this.frmComSettingsGSWRadioBtn.UseVisualStyleBackColor = true;
            this.frmComSettingsGSWRadioBtn.CheckedChanged += new EventHandler(this.frmComSettingsGSWRadioBtn_CheckedChanged);
            this.frmComSettingsSLCRadioBtn.AutoSize = true;
            this.frmComSettingsSLCRadioBtn.Checked = true;
            this.frmComSettingsSLCRadioBtn.Location = new Point(12, 0x15);
            this.frmComSettingsSLCRadioBtn.Name = "frmComSettingsSLCRadioBtn";
            this.frmComSettingsSLCRadioBtn.Size = new Size(0x2d, 0x11);
            this.frmComSettingsSLCRadioBtn.TabIndex = 11;
            this.frmComSettingsSLCRadioBtn.TabStop = true;
            this.frmComSettingsSLCRadioBtn.Text = "SLC";
            this.frmComSettingsSLCRadioBtn.UseVisualStyleBackColor = true;
            this.frmComSettingsSLCRadioBtn.CheckedChanged += new EventHandler(this.frmComSettingsSLCRadioBtn_CheckedChanged);
            this.frmCommOpenProtocolsLabel.AutoSize = true;
            this.frmCommOpenProtocolsLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.frmCommOpenProtocolsLabel.Location = new Point(12, 0x30);
            this.frmCommOpenProtocolsLabel.Name = "frmCommOpenProtocolsLabel";
            this.frmCommOpenProtocolsLabel.Size = new Size(0x33, 13);
            this.frmCommOpenProtocolsLabel.TabIndex = 20;
            this.frmCommOpenProtocolsLabel.Text = "Protocols";
            this.cboProtocols.FormattingEnabled = true;
            this.cboProtocols.Location = new Point(0x4b, 0x2d);
            this.cboProtocols.Name = "cboProtocols";
            this.cboProtocols.Size = new Size(0x54, 0x15);
            this.cboProtocols.TabIndex = 13;
            this.cboProtocols.SelectedIndexChanged += new EventHandler(this.cboProtocols_SelectedIndexChanged);
            this.frmCommSettingsConTypeGrpBox.Controls.Add(this.hostAppI2CRadioBtn);
            this.frmCommSettingsConTypeGrpBox.Controls.Add(this.hostAppRS232RadioBtn);
            this.frmCommSettingsConTypeGrpBox.Controls.Add(this.hostAppTCPIPRadioBtn);
            this.frmCommSettingsConTypeGrpBox.Location = new Point(0x116, 12);
            this.frmCommSettingsConTypeGrpBox.Name = "frmCommSettingsConTypeGrpBox";
            this.frmCommSettingsConTypeGrpBox.Size = new Size(0xde, 0x51);
            this.frmCommSettingsConTypeGrpBox.TabIndex = 0x19;
            this.frmCommSettingsConTypeGrpBox.TabStop = false;
            this.frmCommSettingsConTypeGrpBox.Text = "Physical Connection";
            this.hostAppI2CRadioBtn.AutoSize = true;
            this.hostAppI2CRadioBtn.Location = new Point(0x12, 0x31);
            this.hostAppI2CRadioBtn.Name = "hostAppI2CRadioBtn";
            this.hostAppI2CRadioBtn.Size = new Size(0x29, 0x11);
            this.hostAppI2CRadioBtn.TabIndex = 5;
            this.hostAppI2CRadioBtn.TabStop = true;
            this.hostAppI2CRadioBtn.Text = "I2C";
            this.hostAppI2CRadioBtn.UseVisualStyleBackColor = true;
            this.frmCommSettingsRxParamsGrpBox.Controls.Add(this.groupBox3);
            this.frmCommSettingsRxParamsGrpBox.Controls.Add(this.frmCommOpenRxTypeGrpBox);
            this.frmCommSettingsRxParamsGrpBox.Location = new Point(0x16, 0x1b2);
            this.frmCommSettingsRxParamsGrpBox.Name = "frmCommSettingsRxParamsGrpBox";
            this.frmCommSettingsRxParamsGrpBox.Size = new Size(0x1da, 0x9f);
            this.frmCommSettingsRxParamsGrpBox.TabIndex = 0x1a;
            this.frmCommSettingsRxParamsGrpBox.TabStop = false;
            this.frmCommSettingsRxParamsGrpBox.Text = "Receiver Parameters";
            this.frmCommSettingsProductFamilyComboBox.FormattingEnabled = true;
            this.frmCommSettingsProductFamilyComboBox.Items.AddRange(new object[] { "GSD4t", "GSD4e", "GSW", "SLC" });
            this.frmCommSettingsProductFamilyComboBox.Location = new Point(0x1a, 0x1a);
            this.frmCommSettingsProductFamilyComboBox.Name = "frmCommSettingsProductFamilyComboBox";
            this.frmCommSettingsProductFamilyComboBox.Size = new Size(0x79, 0x15);
            this.frmCommSettingsProductFamilyComboBox.TabIndex = 0x1b;
            this.frmCommSettingsProductFamilyComboBox.SelectedIndexChanged += new EventHandler(this.frmCommSettingsProductFamilyComboBox_SelectedIndexChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x1a, 7);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x4f, 13);
            this.label12.TabIndex = 0x1c;
            this.label12.Text = "Product Family:";
            this.frmCommSettingsAdvancedRxSettingsBtn.Location = new Point(0x16, 0x181);
            this.frmCommSettingsAdvancedRxSettingsBtn.Name = "frmCommSettingsAdvancedRxSettingsBtn";
            this.frmCommSettingsAdvancedRxSettingsBtn.Size = new Size(0x5f, 0x17);
            this.frmCommSettingsAdvancedRxSettingsBtn.TabIndex = 0x1d;
            this.frmCommSettingsAdvancedRxSettingsBtn.Text = "&Rx Parameters";
            this.frmCommSettingsAdvancedRxSettingsBtn.UseVisualStyleBackColor = true;
            this.frmCommSettingsAdvancedRxSettingsBtn.Click += new EventHandler(this.frmCommSettingsAdvanceRxSettingsBtn_Click);
            this.frmCommSettingsRxNameLabel.AutoSize = true;
            this.frmCommSettingsRxNameLabel.Location = new Point(0xa2, 7);
            this.frmCommSettingsRxNameLabel.Name = "frmCommSettingsRxNameLabel";
            this.frmCommSettingsRxNameLabel.Size = new Size(0x36, 13);
            this.frmCommSettingsRxNameLabel.TabIndex = 30;
            this.frmCommSettingsRxNameLabel.Text = "Rx Name:";
            this.frmCommSettingsRxNameTxtBox.Location = new Point(0xa3, 0x1b);
            this.frmCommSettingsRxNameTxtBox.Name = "frmCommSettingsRxNameTxtBox";
            this.frmCommSettingsRxNameTxtBox.Size = new Size(90, 20);
            this.frmCommSettingsRxNameTxtBox.TabIndex = 0x1f;
            base.AcceptButton = this.frmCommSettingsOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.frmCommSettingsCancelBtn;
            base.ClientSize = new Size(520, 0x260);
            base.Controls.Add(this.frmCommSettingsRxNameTxtBox);
            base.Controls.Add(this.frmCommSettingsRxNameLabel);
            base.Controls.Add(this.frmCommSettingsAdvancedRxSettingsBtn);
            base.Controls.Add(this.label12);
            base.Controls.Add(this.frmCommSettingsProductFamilyComboBox);
            base.Controls.Add(this.frmCommSettingsRxParamsGrpBox);
            base.Controls.Add(this.frmCommSettingsConTypeGrpBox);
            base.Controls.Add(this.COMDeviceTabControl);
            base.Controls.Add(this.frmCommSettingsCancelBtn);
            base.Controls.Add(this.hostAppRunHostChkBox);
            base.Controls.Add(this.frmCommSettingsOkBtn);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmCommSettings";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Rx Port Settings";
            base.Load += new EventHandler(this.frmCommSettings_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmCommSettings_FormClosing);
            this.COMDeviceTabControl.ResumeLayout(false);
            this.HostAppTabPage.ResumeLayout(false);
            this.HostAppTabPage.PerformLayout();
            this.RS232TabPage.ResumeLayout(false);
            this.RS232TabPage.PerformLayout();
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.ResumeLayout(false);
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.PerformLayout();
            this.TcpIpTabPage.ResumeLayout(false);
            this.COMServerGroupBox.ResumeLayout(false);
            this.COMServerGroupBox.PerformLayout();
            this.COMClientIPGroupBox.ResumeLayout(false);
            this.COMClientIPGroupBox.PerformLayout();
            this.COMTcpIpModeGroupBox.ResumeLayout(false);
            this.COMTcpIpModeGroupBox.PerformLayout();
            this.ExtEphTabPage.ResumeLayout(false);
            this.ExtEphTabPage.PerformLayout();
            this.EEgroupBoxCGEE.ResumeLayout(false);
            this.EEgroupBoxCGEE.PerformLayout();
            this.EEgroupBoxSGEE.ResumeLayout(false);
            this.EEgroupBoxSGEE.PerformLayout();
            this.I2CTabPage.ResumeLayout(false);
            this.I2CTabPage.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.frmCommOpenRxTypeGrpBox.ResumeLayout(false);
            this.frmCommOpenRxTypeGrpBox.PerformLayout();
            this.frmCommSettingsConTypeGrpBox.ResumeLayout(false);
            this.frmCommSettingsConTypeGrpBox.PerformLayout();
            this.frmCommSettingsRxParamsGrpBox.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadProtocolList()
        {
            if (this.comm != null)
            {
                ArrayList protocols = new ArrayList();
                protocols = this.comm.m_Protocols.GetProtocols();
                for (int i = 0; i < protocols.Count; i++)
                {
                    if (((protocols[i].ToString() != "LPL") && (protocols[i].ToString() != "F")) && (protocols[i].ToString() != "AI3"))
                    {
                        if (protocols[i].ToString() == "SSB")
                        {
                            if (!clsGlobal.IsMarketingUser())
                            {
                                this.cboProtocols.Items.Add(protocols[i].ToString());
                            }
                        }
                        else
                        {
                            this.cboProtocols.Items.Add(protocols[i].ToString());
                        }
                    }
                }
            }
        }

        private void LoadValues()
        {
            if (this.comm != null)
            {
                this.comm.SetPortNameValues(this.cboPort);
                this.comm.SetParityValues(this.cboParity);
                this.comm.SetStopBitValues(this.cboStop);
                this.comm.SetPortNameValues(this.hostAppCboResetPort);
                this.comm.SetPortNameValues(this.hostAppCboHostPair1);
                this.comm.SetPortNameValues(this.hostAppCboHostPair2);
                this.comm.SetPortNameValues(this.hostAppCboTrackerPort);
                this.comm.SetPortNameValues(this.gsd4eOnOffPortComboBox);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this._backupTabPages.Clear();
            base.OnClosed(e);
        }

        private void onOffActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.gsd4eOnOffPortComboBox.Text == string.Empty) || (this.gsd4eOnOffPortComboBox.Text == "-1"))
            {
                MessageBox.Show("No On Off port found! Please select On/Off port and try again", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.comm != null)
            {
                string s = this.gsd4eOnOffPortComboBox.Text.Replace("COM", "");
                int result = 0;
                if (int.TryParse(s, out result))
                {
                    if (!this.comm.Init4eMPMWakeupPort(result))
                    {
                        MessageBox.Show("Error controlling On/Off port.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        switch (this.onOffActionComboBox.SelectedIndex)
                        {
                            case 0:
                                this.comm.Toggle4eWakeupPort();
                                break;

                            case 1:
                                this.comm.SetWakeupPort(true);
                                break;

                            case 2:
                                this.comm.SetWakeupPort(false);
                                break;
                        }
                        this.comm.MPMWakeupPort.Close();
                    }
                }
            }
        }

        private void setAdvancedRxSettingsVisible(bool state)
        {
            if (this._advancedRxSettingsFlag)
            {
                base.Height = (this.frmCommSettingsRxParamsGrpBox.Location.Y + this.frmCommSettingsRxParamsGrpBox.Height) + 20;
                this.frmCommSettingsRxParamsGrpBox.Visible = true;
            }
            else
            {
                base.Height = this.frmCommSettingsRxParamsGrpBox.Location.Y + 10;
                this.frmCommSettingsRxParamsGrpBox.Visible = false;
            }
        }

        private void SetControlState()
        {
            if (this.comm != null)
            {
                switch (this.comm.RxCurrentTransmissionType)
                {
                    case CommunicationManager.TransmissionType.Text:
                        this.rdoText.Checked = true;
                        break;

                    case CommunicationManager.TransmissionType.Hex:
                        this.rdoHex.Checked = true;
                        break;

                    case CommunicationManager.TransmissionType.SSB:
                        this.rdoSSB.Checked = true;
                        break;

                    case CommunicationManager.TransmissionType.GP2:
                        this.rdoGP2.Checked = true;
                        break;

                    case CommunicationManager.TransmissionType.GPS:
                        this.rdoCSV.Checked = true;
                        break;

                    default:
                        this.rdoHex.Checked = true;
                        break;
                }
                switch (this.comm.RxType)
                {
                    case CommunicationManager.ReceiverType.GSW:
                        this.frmComSettingsGSWRadioBtn.Checked = true;
                        return;

                    case CommunicationManager.ReceiverType.SLC:
                        this.frmComSettingsSLCRadioBtn.Checked = true;
                        return;

                    default:
                        this.frmComSettingsGSWRadioBtn.Checked = true;
                        break;
                }
            }
        }

        private bool SetDefaults()
        {
            try
            {
                this.cboPort.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show("Error: No COM detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            this.cboBaud.SelectedText = this.comm.BaudRate;
            this.cboBaud.Text = this.comm.BaudRate;
            this.cboParity.SelectedIndex = 0;
            for (int i = 0; i < this.cboParity.Items.Count; i++)
            {
                this.cboParity.SelectedIndex = i;
                if (this.cboParity.SelectedItem.ToString() == this.comm.Parity)
                {
                    break;
                }
            }
            this.cboProtocols.Text = this.comm.MessageProtocol;
            this.cboStop.SelectedIndex = 0;
            this.cboData.SelectedIndex = 1;
            this.cboFlowControl.SelectedIndex = this.comm.FlowControl;
            this.cboReadBuffer.Text = this.comm.ReadBuffer.ToString();
            if (this.comm.CMC.HostAppClient.TCPClientPortNum == 0)
            {
                this.comm.CMC.HostAppClient.TCPClientPortNum = 0x1d83;
            }
            if (this.comm.CMC.HostAppServer.TCPServerPortNum == 0)
            {
                this.comm.CMC.HostAppServer.TCPServerPortNum = 0x1d83;
            }
            if (this.comm.CMC.HostAppClient.TCPClientHostName == string.Empty)
            {
                this.comm.CMC.HostAppClient.TCPClientHostName = "127.0.0.1";
            }
            if (this.comm.CMC.HostAppServer.TCPServerHostName == string.Empty)
            {
                this.comm.CMC.HostAppServer.TCPServerHostName = "127.0.0.1";
            }
            this.COMClientIPAddrTextBox.Text = this.comm.CMC.HostAppClient.TCPClientHostName;
            this.COMServerIPTextBox.Text = this.comm.CMC.HostAppServer.TCPServerHostName;
            this.hostAppLnaComboBox.SelectedIndex = this.comm.LNAType;
            this.hostAppLDOModeComboBox.SelectedIndex = this.comm.LDOMode;
            this.hostAppVersionCheckBox.Checked = this.comm.IsVersion4_1_A8AndAbove;
            this.frmCommSettingsRxNameTxtBox.Text = this.comm.RxName;
            if (this.COMClientIPAddrTextBox.Text == string.Empty)
            {
                this.COMClientIPAddrTextBox.Text = "127.0.0.1";
            }
            return true;
        }

        private void setHostAppI2CEnable()
        {
            this.addRemoveTabpage(false, 1, ((TabPage) this._backupTabPages[1]).Name);
            this.addRemoveTabpage(false, 2, ((TabPage) this._backupTabPages[2]).Name);
            this.addRemoveTabpage(true, 4, ((TabPage) this._backupTabPages[4]).Name);
            this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[4]).Name);
        }

        private void setHostAppRS232Enable()
        {
            this.addRemoveTabpage(true, 1, ((TabPage) this._backupTabPages[1]).Name);
            this.addRemoveTabpage(false, 2, ((TabPage) this._backupTabPages[2]).Name);
            this.addRemoveTabpage(false, 4, ((TabPage) this._backupTabPages[4]).Name);
            if (this.hostAppRunHostChkBox.Checked)
            {
                this.hostAppCboHostPair1.Enabled = true;
                this.hostAppCboHostPair2.Enabled = true;
                this.hostAppCboTCPIPClientPort.Enabled = false;
                this.addRemoveTabpage(true, 0, ((TabPage) this._backupTabPages[0]).Name);
                this.addRemoveTabpage(false, 4, ((TabPage) this._backupTabPages[4]).Name);
                this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[0]).Name);
            }
            else
            {
                if (clsGlobal.IsMarketingUser())
                {
                    this.addRemoveTabpage(false, 0, ((TabPage) this._backupTabPages[0]).Name);
                }
                else
                {
                    this.addRemoveTabpage(false, 0, ((TabPage) this._backupTabPages[0]).Name);
                    this.hostAppEEChkBox.Checked = false;
                }
                this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[1]).Name);
            }
            this.frmCommSettingsRS232AdvanceSettingsGrpBox.Enabled = false;
        }

        private void setHostAppTCPIPEnable()
        {
            this.addRemoveTabpage(true, 2, ((TabPage) this._backupTabPages[2]).Name);
            this.addRemoveTabpage(false, 1, ((TabPage) this._backupTabPages[1]).Name);
            this.addRemoveTabpage(false, 4, ((TabPage) this._backupTabPages[4]).Name);
            if (this.hostAppRunHostChkBox.Checked)
            {
                this.hostAppCboHostPair1.Enabled = false;
                this.hostAppCboHostPair2.Enabled = false;
                this.hostAppCboTCPIPClientPort.Enabled = true;
                this.addRemoveTabpage(true, 0, ((TabPage) this._backupTabPages[0]).Name);
                this.addRemoveTabpage(false, 4, ((TabPage) this._backupTabPages[4]).Name);
                this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[0]).Name);
            }
            else
            {
                this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[2]).Name);
            }
        }

        private void setRunHostAppEnable(bool state)
        {
            if (state)
            {
                this.addRemoveTabpage(true, 0, ((TabPage) this._backupTabPages[0]).Name);
                this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[0]).Name);
                if (this.hostAppRS232RadioBtn.Checked)
                {
                    this.hostAppCboHostPair1.Enabled = true;
                    this.hostAppCboHostPair2.Enabled = true;
                    this.hostAppCboTCPIPClientPort.Enabled = false;
                }
                else
                {
                    this.hostAppCboHostPair1.Enabled = false;
                    this.hostAppCboHostPair2.Enabled = false;
                    this.hostAppCboTCPIPClientPort.Enabled = true;
                }
                if (this.hostAppEEChkBox.Checked)
                {
                    this.addRemoveTabpage(true, 3, ((TabPage) this._backupTabPages[3]).Name);
                }
                else
                {
                    this.addRemoveTabpage(false, 3, ((TabPage) this._backupTabPages[3]).Name);
                }
            }
            else
            {
                if (clsGlobal.IsMarketingUser())
                {
                    this.addRemoveTabpage(false, 0, ((TabPage) this._backupTabPages[0]).Name);
                }
                else
                {
                    this.addRemoveTabpage(false, 0, ((TabPage) this._backupTabPages[0]).Name);
                    this.hostAppEEChkBox.Checked = false;
                }
                this.addRemoveTabpage(false, 3, ((TabPage) this._backupTabPages[3]).Name);
                if (this.hostAppRS232RadioBtn.Checked)
                {
                    this.addRemoveTabpage(true, 1, ((TabPage) this._backupTabPages[1]).Name);
                    this.addRemoveTabpage(false, 2, ((TabPage) this._backupTabPages[2]).Name);
                    this.addRemoveTabpage(false, 4, ((TabPage) this._backupTabPages[4]).Name);
                    this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[1]).Name);
                }
                else if (this.hostAppTCPIPRadioBtn.Checked)
                {
                    this.addRemoveTabpage(false, 1, ((TabPage) this._backupTabPages[1]).Name);
                    this.addRemoveTabpage(true, 2, ((TabPage) this._backupTabPages[2]).Name);
                    this.addRemoveTabpage(false, 4, ((TabPage) this._backupTabPages[4]).Name);
                    this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[2]).Name);
                }
                else if (this.hostAppI2CRadioBtn.Checked)
                {
                    this.addRemoveTabpage(false, 1, ((TabPage) this._backupTabPages[1]).Name);
                    this.addRemoveTabpage(false, 2, ((TabPage) this._backupTabPages[2]).Name);
                    this.addRemoveTabpage(true, 4, ((TabPage) this._backupTabPages[4]).Name);
                    this.COMDeviceTabControl.SelectTab(((TabPage) this._backupTabPages[4]).Name);
                }
            }
        }

        private void SetupI2CComm()
        {
            this.comm.InputDeviceMode = CommonClass.InputDeviceModes.I2C;
            try
            {
                this.comm.CMC.HostAppI2CSlave.I2CDevicePortNum = Convert.ToInt32(this.I2C_PortTxtBox.Text);
                this.comm.CMC.HostAppI2CSlave.I2CDevicePortNumMaster = Convert.ToInt32(this.I2C_PortTxtBoxMaster.Text);
                this.comm.CMC.HostAppI2CSlave.I2CSlaveAddress = Convert.ToByte(this.I2C_SlaveTxtBx.Text, 0x10);
                this.comm.CMC.HostAppI2CSlave.I2CMasterAddress = Convert.ToByte(this.I2C_MasterTxtBx.Text, 0x10);
                if (this.checkBox_SlaveMode.Checked)
                {
                    this.comm.CMC.HostAppI2CSlave.I2CTalkMode = CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_SLAVE;
                }
                else
                {
                    this.comm.CMC.HostAppI2CSlave.I2CTalkMode = CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_MULTI_MASTER;
                }
                if (this.comm.CMC.HostAppI2CSlave.I2CTalkMode == CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_SLAVE)
                {
                    this.comm.PortName = "I2C" + this.comm.CMC.HostAppI2CSlave.I2CDevicePortNumMaster.ToString();
                }
                else
                {
                    this.comm.PortName = "I2C" + this.comm.CMC.HostAppI2CSlave.I2CDevicePortNum.ToString();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: frmCommSettings: SetupI2CComm() I2C - " + exception.ToString());
            }
        }

        private void SetupRS232Comm()
        {
            CommonUtilsClass class2 = new CommonUtilsClass();
            if (this.comm != null)
            {
                try
                {
                    this.comm.Parity = this.cboParity.SelectedItem.ToString();
                    this.comm.StopBits = this.cboStop.SelectedItem.ToString();
                    this.comm.DataBits = this.cboData.SelectedItem.ToString();
                    this.comm.BaudRate = this.cboBaud.Text;
                    this.comm.PortName = this.cboPort.Text;
                    this.comm.FlowControl = this.cboFlowControl.SelectedIndex;
                    this.comm.ReadBuffer = Convert.ToInt32(this.cboReadBuffer.Text);
                    this.comm.MessageProtocol = this.cboProtocols.SelectedItem.ToString();
                    this.comm.AutoReplyCtrl.ControlChannelVersion = this.cboFVersion.Text.ToString();
                    this.comm.AutoReplyCtrl.AidingProtocolVersion = this.cboAI3Version.Text.ToString();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                if (this.frmComSettingsSLCRadioBtn.Checked)
                {
                    this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                }
                else if (this.frmComSettingsTTBRadioBtn.Checked)
                {
                    this.comm.RxType = CommunicationManager.ReceiverType.TTB;
                }
                else if (this.frmComSettingsGSWRadioBtn.Checked)
                {
                    this.comm.RxType = CommunicationManager.ReceiverType.GSW;
                }
                this.UpdateRxTransmissionType();
                try
                {
                    class2.DisplayBuffer = Convert.ToInt32(this.frmCommOpenBufferSizeTxtBox.Text);
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                this.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                this.comm.HostPair1 = this.hostAppCboHostPair1.Text;
                if (this.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                {
                    this.comm.ResetPort = this.gsd4eOnOffPortComboBox.Text;
                }
            }
        }

        private void SetupTcpIpComm()
        {
            if (this.COMTcpIPClientRadioButton.Checked)
            {
                this.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Client;
                try
                {
                    int selectedIndex = this.COMDeviceTabControl.SelectedIndex;
                    this.comm.CMC.HostAppClient.TCPClientHostName = this.COMClientIPAddrTextBox.Text;
                    this.comm.CMC.HostAppClient.TCPClientPortNum = Convert.ToInt32(this.hostAppCboTCPIPClientPort.Text);
                    this.comm.PortName = "TCP" + this.hostAppCboTCPIPClientPort.Text;
                    this.comm.PortNum = this.hostAppCboTCPIPClientPort.Text;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: frmCommSettings: SetupTcpIpComm() Tcp Client - " + exception.ToString());
                }
            }
            else
            {
                this.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Server;
                new CommMgrClass.TCPIPServer();
                try
                {
                    this.comm.CMC.HostAppServer.TCPServerIPAddress = IPAddress.Parse(this.COMServerIPTextBox.Text);
                    this.comm.CMC.HostAppServer.TCPServerPortNum = Convert.ToInt32(this.hostAppCboServerTCPIPPort.Text);
                    this.comm.PortName = "TCP" + this.hostAppCboServerTCPIPPort.Text;
                    this.comm.PortNum = this.hostAppCboServerTCPIPPort.Text;
                }
                catch (Exception exception2)
                {
                    Console.WriteLine("Error: frmCommSettings: SetupTcpIpComm() Tcp Server - " + exception2.ToString());
                }
            }
        }

        private void UpdateRxTransmissionType()
        {
            if (this.rdoHex.Checked)
            {
                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
            }
            else if (this.rdoText.Checked)
            {
                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
            }
            else if (this.rdoSSB.Checked)
            {
                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
            }
            else if (this.rdoGP2.Checked)
            {
                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
            }
            else if (this.rdoCSV.Checked)
            {
                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
            }
            else
            {
                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
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
                this.LoadProtocolList();
                this.cboParity.DataBindings.Add("SelectedItem", this.comm, "Parity");
                this.cboStop.DataBindings.Add("SelectedItem", this.comm, "StopBits");
                this.cboData.DataBindings.Add("SelectedItem", this.comm, "DataBits");
                this.cboBaud.DataBindings.Add("SelectedItem", this.comm, "BaudRate");
                this.cboPort.DataBindings.Add("SelectedItem", this.comm, "PortName");
                this.cboProtocols.DataBindings.Add("SelectedItem", this.comm, "MessageProtocol");
            }
        }
    }
}

