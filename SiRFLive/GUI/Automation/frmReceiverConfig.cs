﻿namespace SiRFLive.GUI.Automation
{
    using CommonClassLibrary;
    using SiRFLive.General;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmReceiverConfig : Form
    {
        private IContainer components;
        private int currentIndex;
        private Button frmConfigReceiverBackBtn;
        private Button frmReceiverConfigAddBtn;
        private ComboBox frmReceiverConfigAI3ICDComboBox;
        private Label frmReceiverConfigAI3ICDLabel;
        private ComboBox frmReceiverConfigAidingProtocolComboBox;
        private Label frmReceiverConfigAidingProtocolLabel;
        private Button frmReceiverConfigCancelBtn;
        private ComboBox frmReceiverConfigComboBox;
        private ComboBox frmReceiverConfigDefaultClockComboBox;
        private Label frmReceiverConfigDefaultClockLabel;
        private Button frmReceiverConfigDoneBtn;
        private Label frmReceiverConfigDUTBaudLabel;
        private Label frmReceiverConfigDUTPortLabel;
        private ComboBox frmReceiverConfigExtClkFreqComboBox;
        private Label frmReceiverConfigExtClkFreqLabel;
        private ComboBox frmReceiverConfigFamilyComboBox;
        private Label frmReceiverConfigFamilyLabel;
        private ComboBox frmReceiverConfigFICDComboBox;
        private Label frmReceiverConfigFICDLabel;
        private Label frmReceiverConfigHostArgsLabel;
        private TextBox frmReceiverConfigHostArgsTxtBox;
        private Button frmReceiverConfigHostDirBrowserBtn;
        private TextBox frmReceiverConfigHostDirTxtBox;
        private Label frmReceiverConfigHostPortLabel;
        private TextBox frmReceiverConfigHostPortTxtBox;
        private Button frmReceiverConfigHostSWBrowseBtn;
        private Label frmReceiverConfigHostSWLabel;
        private TextBox frmReceiverConfigHostSWTxtBox;
        private Label frmReceiverConfigIPAddressLabel;
        private TextBox frmReceiverConfigIPAddressTxtBox;
        private ComboBox frmReceiverConfigLDOModeComboBox;
        private Label frmReceiverConfigLDOModeLabel;
        private ComboBox frmReceiverConfigLnaComboBox;
        private Label frmReceiverConfigLnaTypeLabel;
        private ComboBox frmReceiverConfigMessageProtocolComboBox;
        private Label frmReceiverConfigMessageProtocolLabel;
        private Label frmReceiverConfigNameLabel;
        private TextBox frmReceiverConfigNameTxtBox;
        private Button frmReceiverConfigNextBtn;
        private Button frmReceiverConfigPatchSWBrowseBtn;
        private Label frmReceiverConfigPatchSWLabel;
        private TextBox frmReceiverConfigPatchSWTxtBox;
        private ComboBox frmReceiverConfigPhyCommProtocolComboBox;
        private Label frmReceiverConfigPhyCommProtocolLabel;
        private TextBox frmReceiverConfigPortTxtBox;
        private Button frmReceiverConfigRemoveBtn;
        private CheckBox frmReceiverConfigRequiredHostChkBox;
        private CheckBox frmReceiverConfigRequiredPatchChkBox;
        private Label frmReceiverConfigResetPortLabel;
        private TextBox frmReceiverConfigResetPortTxtBox;
        private Label frmReceiverConfigRxIndexLabel;
        private ComboBox frmReceiverConfigRxLogTypeComboBox;
        private Label frmReceiverConfigRxLogTypeLabel;
        private Label frmReceiverConfigTitle;
        private Label frmReceiverConfigTrackerPortLabel;
        private TextBox frmReceiverConfigTrackerPortTxtBox;
        private Label frmReceiverConfigTTBPortLabel;
        private TextBox frmReceiverConfigTTBPortTxtBox;
        private ComboBox frmReceiverConfigTxLogTypeComboBox;
        private Label frmReceiverConfigTxLogTypeLabel;
        private ComboBox frmReceiverConfigTypeComboBox;
        private Label frmReceiverConfigTypeLabel;
        private ComboBox frmReceiverMfgComboBox;
        private Label frmReceiverMfgLabel;
        private ComboBox frmReceiverPackageComboBox;
        private Label frmReceiverPackageLabel;
        private ComboBox frmReceiverPlatformCombox;
        private Label frmReceiverRevLabel;
        private TextBox frmReceiverRevTextBox;
        private string hostNPatchSWPath;
        private frmCommonSimpleInput inputForm = new frmCommonSimpleInput("Enter Data:");
        private static frmReceiverConfig m_SChildform;
        private Label platformLabel;
        private ComboBox refFreqSrcComboBox;
        private Label refFreqSrcLabel;
        private List<string> rxAI3ICDs = new List<string>();
        private List<string> rxAidingProtocols = new List<string>();
        private List<string> rxBauds = new List<string>();
        private List<string> rxDefaultClocks = new List<string>();
        private List<string> rxExtClkFreqs = new List<string>();
        private List<string> rxFamily = new List<string>();
        private List<string> rxFICDs = new List<string>();
        private List<string> rxHostPorts = new List<string>();
        private List<string> rxHostSWArgs = new List<string>();
        private List<string> rxHostSWs = new List<string>();
        private List<string> rxIPAddress = new List<string>();
        private List<string> rxLdoMode = new List<string>();
        private List<string> rxLnaType = new List<string>();
        private List<string> rxMessageProtocols = new List<string>();
        private List<string> rxMfg = new List<string>();
        private List<string> rxNames = new List<string>();
        private List<string> rxPackage = new List<string>();
        private List<string> rxPatchSWs = new List<string>();
        private List<string> rxPhyCommProtocols = new List<string>();
        private List<string> rxPlatform = new List<string>();
        private List<string> rxPorts = new List<string>();
        private List<string> rxRefFreqSrc = new List<string>();
        private List<string> rxRequiredHosts = new List<string>();
        private List<string> rxRequiredPatches = new List<string>();
        private List<string> rxResetPorts = new List<string>();
        private List<string> rxRev = new List<string>();
        private List<string> rxRxLogTypes = new List<string>();
        private List<string> rxSirfNavStartStopIntfStr = new List<string>();
        private List<string> rxTemperature = new List<string>();
        private List<string> rxTemperatureUnit = new List<string>();
        private List<string> rxTrackerPorts = new List<string>();
        private List<string> rxTTBPorts = new List<string>();
        private List<string> rxTxLogTypes = new List<string>();
        private List<string> rxTypes = new List<string>();
        private List<string> rxVoltage = new List<string>();
        private Label rxVoltlabel;
        private TextBox rxVoltTextBox;
        private TextBox sirfNavStartStopTextBox;
        private Label sirfNavStrLabel;
        private Label temperatureUnitLabel;
        private Label temperLabel;
        private TextBox temperTextBox;
        private ComboBox temperUnitComboBox;

        internal event updateParentEventHandler updateParent;

        internal frmReceiverConfig()
        {
            this.InitializeComponent();
        }

        private void addElementToHash(ref Hashtable inHash, string key, string value)
        {
            if (value != "Error")
            {
                if (inHash.ContainsKey(key))
                {
                    inHash[key] = value;
                }
                else
                {
                    inHash.Add(key, value);
                }
            }
        }

        private int countMatch(List<string> inputList, string searchStr)
        {
            int num = 0;
            foreach (string str in inputList)
            {
                if (str == searchStr)
                {
                    num++;
                }
            }
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmConfigReceiverBackBtn_Click(object sender, EventArgs e)
        {
            this.currentIndex--;
            if (this.currentIndex < 0)
            {
                this.currentIndex = 0;
                MessageBox.Show("Min configured receivers reached", "Information");
            }
            else
            {
                this.updateformFields(this.currentIndex);
            }
        }

        private void frmReceiverConfig_Load(object sender, EventArgs e)
        {
            string[] strArray17;
            int num7;
            string key = "AVAILABLE_PROD_FAMILY";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray.Length > 0)
                {
                    strArray17 = strArray;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str2 = strArray17[num7];
                        this.frmReceiverConfigFamilyComboBox.Items.Add(str2.Trim());
                    }
                }
            }
            key = "AVAILABLE_RX_TYPE";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray2 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray2.Length > 0)
                {
                    foreach (string str3 in strArray2)
                    {
                        this.frmReceiverConfigTypeComboBox.Items.Add(str3.Trim());
                    }
                }
            }
            key = "AVAILABLE_DEFAULT_CLK_FREQ";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray3 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray3.Length > 0)
                {
                    foreach (string str4 in strArray3)
                    {
                        this.frmReceiverConfigDefaultClockComboBox.Items.Add(str4.Trim());
                    }
                }
            }
            key = "AVAILABLE_TCXO_FREQ";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray4 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray4.Length > 0)
                {
                    strArray17 = strArray4;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str5 = strArray17[num7];
                        this.frmReceiverConfigExtClkFreqComboBox.Items.Add(str5.Trim());
                    }
                }
            }
            key = "AVAILABLE_PHYSICAL_CONNECTION";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray5 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray5.Length > 0)
                {
                    strArray17 = strArray5;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str6 = strArray17[num7];
                        this.frmReceiverConfigPhyCommProtocolComboBox.Items.Add(str6.Trim());
                    }
                }
            }
            key = "AVAILABLE_MESSAGE_PROTOCOL";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray6 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray6.Length > 0)
                {
                    strArray17 = strArray6;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str7 = strArray17[num7];
                        this.frmReceiverConfigMessageProtocolComboBox.Items.Add(str7.Trim());
                    }
                }
            }
            key = "AVAILABLE_AIDING_PROTOCOL";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray7 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray7.Length > 0)
                {
                    strArray17 = strArray7;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str8 = strArray17[num7];
                        this.frmReceiverConfigAidingProtocolComboBox.Items.Add(str8.Trim());
                    }
                }
            }
            key = "AVAILABLE_AIDING_ICD";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray8 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray8.Length > 0)
                {
                    strArray17 = strArray8;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str9 = strArray17[num7];
                        this.frmReceiverConfigAI3ICDComboBox.Items.Add(str9.Trim());
                    }
                }
            }
            key = "AVAILABLE_CONTROL_ICD";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray9 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray9.Length > 0)
                {
                    strArray17 = strArray9;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str10 = strArray17[num7];
                        this.frmReceiverConfigFICDComboBox.Items.Add(str10.Trim());
                    }
                }
            }
            key = "AVAILABLE_RX_LOG_FORMAT";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray10 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray10.Length > 0)
                {
                    strArray17 = strArray10;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str11 = strArray17[num7];
                        this.frmReceiverConfigRxLogTypeComboBox.Items.Add(str11.Trim());
                    }
                }
            }
            key = "AVAILABLE_TX_LOG_FORMAT";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray11 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray11.Length > 0)
                {
                    strArray17 = strArray11;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str12 = strArray17[num7];
                        this.frmReceiverConfigTxLogTypeComboBox.Items.Add(str12.Trim());
                    }
                }
            }
            key = "AVAILABLE_TCXO_FREQ_SRC";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray12 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray12.Length > 0)
                {
                    strArray17 = strArray12;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str13 = strArray17[num7];
                        this.refFreqSrcComboBox.Items.Add(str13.Trim());
                    }
                }
            }
            key = "AVAILABLE_PLATFORM";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray13 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray13.Length > 0)
                {
                    strArray17 = strArray13;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str14 = strArray17[num7];
                        this.frmReceiverPlatformCombox.Items.Add(str14.Trim());
                    }
                }
            }
            key = "AVAILABLE_PACKAGE";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray14 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray14.Length > 0)
                {
                    strArray17 = strArray14;
                    for (num7 = 0; num7 < strArray17.Length; num7++)
                    {
                        string str15 = strArray17[num7];
                        this.frmReceiverPackageComboBox.Items.Add(str15.Trim());
                    }
                }
            }
            key = "AVAILABLE_MANUFACTURE_NAME";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                string[] strArray15 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                if (strArray15.Length > 0)
                {
                    strArray17 = strArray15;
                    num7 = 0;
                    while (num7 < strArray17.Length)
                    {
                        string str16 = strArray17[num7];
                        this.frmReceiverMfgComboBox.Items.Add(str16.Trim());
                        num7++;
                    }
                }
            }
            this.inputForm.updateParent += new frmCommonSimpleInput.updateParentEventHandler(this.updateConfigList);
            this.inputForm.MdiParent = base.MdiParent;
            Hashtable automationParamsHash = clsGlobal.AutomationParamsHash;
            string str17 = (string) automationParamsHash["ACTIVE_PORTS"];
            int length = str17.Split(new char[] { ',' }).Length;
            int index = 0;
            int num3 = 0;
            foreach (string str18 in automationParamsHash.Keys)
            {
                string[] strArray16;
                string str19;
                string str20;
                string str21;
                str17 = (string) automationParamsHash[str18];
                switch (str18)
                {
                    case "PROD_FAMILY":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_0A59;

                    case "ACTIVE_PORTS":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_0A98;

                    case "PHY_COMM_PROTOCOL":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_0AD7;

                    case "TCPIP_IP_ADDRESS":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_0B16;

                    case "TTB_PORTS":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_0B55;

                    case "RX_BAUDS":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0BA6;

                    case "TRACKER_PORTS":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0BF7;

                    case "RESET_PORTS":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0C48;

                    case "HOST_PORTS":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0C99;

                    case "REQUIRED_HOSTS":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0CEA;

                    case "HOST_APP":
                        strArray17 = str17.Split(new char[] { ',' });
                        num7 = 0;
                        goto Label_0D2D;

                    case "REQUIRED_PATCH":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0D80;

                    case "PATCH_FILE":
                        strArray17 = str17.Split(new char[] { ',' });
                        num7 = 0;
                        goto Label_0DC3;

                    case "EXTRA_HOST_APP_ARGVS":
                        strArray17 = str17.Split(new char[] { ',' });
                        num7 = 0;
                        goto Label_0E08;

                    case "RX_NAMES":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length > length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0E5B;

                    case "RX_TYPES":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0EAC;

                    case "RX_LOG_TYPES":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0EFD;

                    case "TX_LOG_TYPES":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0F4E;

                    case "DEFAULT_CLK":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0F9F;

                    case "EXT_CLK_FREQ":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_0FF0;

                    case "LNA_TYPE":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_102F;

                    case "LDO_MODE":
                        strArray16 = str17.Split(new char[] { ',' });
                        index = 0;
                        goto Label_106E;

                    case "AI3_ICD":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_10BF;

                    case "F_ICD":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1110;

                    case "MESSAGE_PROTOCOL":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1161;

                    case "AIDING_PROTOCOL":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_11B2;

                    case "SIRFNAV_INTF_STR":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1203;

                    case "PLATFORM":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1254;

                    case "PACKAGE":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_12A5;

                    case "REVISION":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_12F6;

                    case "MANUFACTURE_NAME":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1347;

                    case "EXT_CLK_SRC":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1398;

                    case "TEMPERATURE":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_13E9;

                    case "TEMPERATURE_UNIT":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_143A;

                    case "VOLTAGE":
                        strArray16 = str17.Split(new char[] { ',' });
                        num3 = (strArray16.Length < length) ? strArray16.Length : length;
                        index = 0;
                        goto Label_1488;

                    case "HOST_APP_DIR":
                    {
                        this.hostNPatchSWPath = str17;
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
            Label_0A43:
                this.rxFamily.Add(strArray16[index]);
                index++;
            Label_0A59:
                if (index < length)
                {
                    goto Label_0A43;
                }
                continue;
            Label_0A82:
                this.rxPorts.Add(strArray16[index]);
                index++;
            Label_0A98:
                if (index < length)
                {
                    goto Label_0A82;
                }
                continue;
            Label_0AC1:
                this.rxPhyCommProtocols.Add(strArray16[index]);
                index++;
            Label_0AD7:
                if (index < length)
                {
                    goto Label_0AC1;
                }
                continue;
            Label_0B00:
                this.rxIPAddress.Add(strArray16[index]);
                index++;
            Label_0B16:
                if (index < length)
                {
                    goto Label_0B00;
                }
                continue;
            Label_0B3F:
                this.rxTTBPorts.Add(strArray16[index]);
                index++;
            Label_0B55:
                if (index < length)
                {
                    goto Label_0B3F;
                }
                continue;
            Label_0B90:
                this.rxBauds.Add(strArray16[index]);
                index++;
            Label_0BA6:
                if (index < num3)
                {
                    goto Label_0B90;
                }
                continue;
            Label_0BE1:
                this.rxTrackerPorts.Add(strArray16[index]);
                index++;
            Label_0BF7:
                if (index < num3)
                {
                    goto Label_0BE1;
                }
                continue;
            Label_0C32:
                this.rxResetPorts.Add(strArray16[index]);
                index++;
            Label_0C48:
                if (index < num3)
                {
                    goto Label_0C32;
                }
                continue;
            Label_0C83:
                this.rxHostPorts.Add(strArray16[index]);
                index++;
            Label_0C99:
                if (index < num3)
                {
                    goto Label_0C83;
                }
                continue;
            Label_0CD4:
                this.rxRequiredHosts.Add(strArray16[index]);
                index++;
            Label_0CEA:
                if (index < num3)
                {
                    goto Label_0CD4;
                }
                continue;
            Label_0D13:
                str19 = strArray17[num7];
                this.rxHostSWs.Add(str19);
                num7++;
            Label_0D2D:
                if (num7 < strArray17.Length)
                {
                    goto Label_0D13;
                }
                continue;
            Label_0D6A:
                this.rxRequiredPatches.Add(strArray16[index]);
                index++;
            Label_0D80:
                if (index < num3)
                {
                    goto Label_0D6A;
                }
                continue;
            Label_0DA9:
                str20 = strArray17[num7];
                this.rxPatchSWs.Add(str20);
                num7++;
            Label_0DC3:
                if (num7 < strArray17.Length)
                {
                    goto Label_0DA9;
                }
                continue;
            Label_0DEE:
                str21 = strArray17[num7];
                this.rxHostSWArgs.Add(str21);
                num7++;
            Label_0E08:
                if (num7 < strArray17.Length)
                {
                    goto Label_0DEE;
                }
                continue;
            Label_0E45:
                this.rxNames.Add(strArray16[index]);
                index++;
            Label_0E5B:
                if (index < num3)
                {
                    goto Label_0E45;
                }
                continue;
            Label_0E96:
                this.rxTypes.Add(strArray16[index]);
                index++;
            Label_0EAC:
                if (index < num3)
                {
                    goto Label_0E96;
                }
                continue;
            Label_0EE7:
                this.rxRxLogTypes.Add(strArray16[index]);
                index++;
            Label_0EFD:
                if (index < num3)
                {
                    goto Label_0EE7;
                }
                continue;
            Label_0F38:
                this.rxTxLogTypes.Add(strArray16[index]);
                index++;
            Label_0F4E:
                if (index < num3)
                {
                    goto Label_0F38;
                }
                continue;
            Label_0F89:
                this.rxDefaultClocks.Add(strArray16[index]);
                index++;
            Label_0F9F:
                if (index < num3)
                {
                    goto Label_0F89;
                }
                continue;
            Label_0FDA:
                this.rxExtClkFreqs.Add(strArray16[index]);
                index++;
            Label_0FF0:
                if (index < num3)
                {
                    goto Label_0FDA;
                }
                continue;
            Label_1019:
                this.rxLnaType.Add(strArray16[index]);
                index++;
            Label_102F:
                if (index < length)
                {
                    goto Label_1019;
                }
                continue;
            Label_1058:
                this.rxLdoMode.Add(strArray16[index]);
                index++;
            Label_106E:
                if (index < length)
                {
                    goto Label_1058;
                }
                continue;
            Label_10A9:
                this.rxAI3ICDs.Add(strArray16[index]);
                index++;
            Label_10BF:
                if (index < num3)
                {
                    goto Label_10A9;
                }
                continue;
            Label_10FA:
                this.rxFICDs.Add(strArray16[index]);
                index++;
            Label_1110:
                if (index < num3)
                {
                    goto Label_10FA;
                }
                continue;
            Label_114B:
                this.rxMessageProtocols.Add(strArray16[index]);
                index++;
            Label_1161:
                if (index < num3)
                {
                    goto Label_114B;
                }
                continue;
            Label_119C:
                this.rxAidingProtocols.Add(strArray16[index]);
                index++;
            Label_11B2:
                if (index < num3)
                {
                    goto Label_119C;
                }
                continue;
            Label_11ED:
                this.rxSirfNavStartStopIntfStr.Add(strArray16[index]);
                index++;
            Label_1203:
                if (index < num3)
                {
                    goto Label_11ED;
                }
                continue;
            Label_123E:
                this.rxPlatform.Add(strArray16[index]);
                index++;
            Label_1254:
                if (index < num3)
                {
                    goto Label_123E;
                }
                continue;
            Label_128F:
                this.rxPackage.Add(strArray16[index]);
                index++;
            Label_12A5:
                if (index < num3)
                {
                    goto Label_128F;
                }
                continue;
            Label_12E0:
                this.rxRev.Add(strArray16[index]);
                index++;
            Label_12F6:
                if (index < num3)
                {
                    goto Label_12E0;
                }
                continue;
            Label_1331:
                this.rxMfg.Add(strArray16[index]);
                index++;
            Label_1347:
                if (index < num3)
                {
                    goto Label_1331;
                }
                continue;
            Label_1382:
                this.rxRefFreqSrc.Add(strArray16[index]);
                index++;
            Label_1398:
                if (index < num3)
                {
                    goto Label_1382;
                }
                continue;
            Label_13D3:
                this.rxTemperature.Add(strArray16[index]);
                index++;
            Label_13E9:
                if (index < num3)
                {
                    goto Label_13D3;
                }
                continue;
            Label_1424:
                this.rxTemperatureUnit.Add(strArray16[index]);
                index++;
            Label_143A:
                if (index < num3)
                {
                    goto Label_1424;
                }
                continue;
            Label_1472:
                this.rxVoltage.Add(strArray16[index]);
                index++;
            Label_1488:
                if (index < num3)
                {
                    goto Label_1472;
                }
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            int num4 = 0;
            foreach (string str22 in this.rxRequiredHosts)
            {
                if (Convert.ToInt32(str22) == 1)
                {
                    try
                    {
                        list2.Add(this.rxHostSWArgs[num4]);
                        list.Add(this.rxHostSWs[num4]);
                        num4++;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "ERROR!");
                    }
                }
                else
                {
                    list2.Add("-1");
                    list.Add("-1");
                }
            }
            this.rxHostSWArgs = list2;
            this.rxHostSWs = list;
            List<string> list3 = new List<string>();
            num4 = 0;
            foreach (string str23 in this.rxRequiredPatches)
            {
                if (Convert.ToInt32(str23) == 1)
                {
                    try
                    {
                        list3.Add(this.rxPatchSWs[num4]);
                        num4++;
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show(exception2.Message, "ERROR!");
                    }
                }
                else
                {
                    list3.Add("-1");
                }
            }
            this.rxPatchSWs = list3;
            this.updateformFields(0);
            if (clsGlobal.IsMarketingUser())
            {
                this.frmReceiverConfigAddBtn.Visible = false;
                this.frmConfigReceiverBackBtn.Visible = false;
                this.frmReceiverConfigNextBtn.Visible = false;
                this.frmReceiverConfigRemoveBtn.Visible = false;
            }
            this.frmReceiverConfigTxLogTypeComboBox.Enabled = false;
        }

        private void frmReceiverConfigAddBtn_Click(object sender, EventArgs e)
        {
            this.rxFamily.Add(this.frmReceiverConfigFamilyComboBox.Text);
            this.rxTypes.Add(this.frmReceiverConfigTypeComboBox.Text);
            this.rxDefaultClocks.Add(this.frmReceiverConfigDefaultClockComboBox.Text);
            this.rxExtClkFreqs.Add(this.frmReceiverConfigExtClkFreqComboBox.Text);
            this.rxBauds.Add(this.frmReceiverConfigComboBox.Text);
            this.rxNames.Add(this.frmReceiverConfigNameTxtBox.Text);
            this.rxPorts.Add(this.frmReceiverConfigPortTxtBox.Text);
            this.rxTrackerPorts.Add(this.frmReceiverConfigTrackerPortTxtBox.Text);
            this.rxResetPorts.Add(this.frmReceiverConfigResetPortTxtBox.Text);
            this.rxHostPorts.Add(this.frmReceiverConfigHostPortTxtBox.Text);
            this.rxTTBPorts.Add(this.frmReceiverConfigTTBPortTxtBox.Text);
            this.rxLnaType.Add(this.frmReceiverConfigLnaComboBox.SelectedIndex.ToString());
            this.rxLdoMode.Add(this.frmReceiverConfigLDOModeComboBox.SelectedIndex.ToString());
            this.rxSirfNavStartStopIntfStr.Add(this.sirfNavStartStopTextBox.Text);
            this.rxTemperature.Add(this.temperTextBox.Text);
            this.rxTemperatureUnit.Add(this.temperUnitComboBox.Text);
            this.rxRefFreqSrc.Add(this.refFreqSrcComboBox.Text);
            this.rxPlatform.Add(this.frmReceiverPlatformCombox.Text);
            this.rxPackage.Add(this.frmReceiverPackageComboBox.Text);
            this.rxMfg.Add(this.frmReceiverMfgComboBox.Text);
            this.rxRev.Add(this.frmReceiverRevTextBox.Text);
            this.rxVoltage.Add(this.rxVoltTextBox.Text);
            this.rxAI3ICDs.Add(this.frmReceiverConfigAI3ICDComboBox.Text);
            this.rxFICDs.Add(this.frmReceiverConfigFICDComboBox.Text);
            this.rxRxLogTypes.Add(this.frmReceiverConfigRxLogTypeComboBox.Text);
            this.rxTxLogTypes.Add(this.frmReceiverConfigTxLogTypeComboBox.Text);
            this.rxMessageProtocols.Add(this.frmReceiverConfigMessageProtocolComboBox.Text);
            this.rxAidingProtocols.Add(this.frmReceiverConfigAidingProtocolComboBox.Text);
            this.rxIPAddress.Add(this.frmReceiverConfigIPAddressTxtBox.Text);
            this.rxPhyCommProtocols.Add(this.frmReceiverConfigPhyCommProtocolComboBox.Text);
            if (this.frmReceiverConfigRequiredHostChkBox.Checked)
            {
                this.rxRequiredHosts.Add("1");
                this.rxHostSWArgs.Add(this.frmReceiverConfigHostArgsTxtBox.Text);
                this.rxHostSWs.Add(this.frmReceiverConfigHostSWTxtBox.Text);
            }
            else
            {
                this.rxRequiredHosts.Add("0");
                this.rxHostSWArgs.Add("-1");
                this.rxHostSWs.Add("-1");
            }
            if (this.frmReceiverConfigRequiredPatchChkBox.Checked)
            {
                this.rxPatchSWs.Add(this.frmReceiverConfigPatchSWTxtBox.Text);
                this.rxRequiredPatches.Add("1");
            }
            else
            {
                this.rxPatchSWs.Add("-1");
                this.rxRequiredPatches.Add("0");
            }
            this.currentIndex++;
            this.updateformFields(this.currentIndex);
        }

        private void frmReceiverConfigAI3ICDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigAI3ICDComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "AI3_ICD";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxAI3ICDs[this.currentIndex] = this.frmReceiverConfigAI3ICDComboBox.SelectedItem.ToString();
                this.frmReceiverConfigAI3ICDComboBox.Text = this.frmReceiverConfigAI3ICDComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigAidingProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigAidingProtocolComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "AIDING_PROTOCOL";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxAidingProtocols[this.currentIndex] = this.frmReceiverConfigAidingProtocolComboBox.SelectedItem.ToString();
                this.frmReceiverConfigAidingProtocolComboBox.Text = this.frmReceiverConfigAidingProtocolComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigBaudComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.frmReceiverConfigComboBox.SelectedItem.ToString())
            {
                case "300":
                case "600":
                case "1200":
                case "2400":
                case "4800":
                case "9600":
                case "14400":
                case "28800":
                case "38400":
                case "57600":
                case "115200":
                case "230400":
                case "460800":
                    this.rxBauds[this.currentIndex] = this.frmReceiverConfigComboBox.SelectedItem.ToString();
                    this.frmReceiverConfigComboBox.Text = this.frmReceiverConfigComboBox.SelectedItem.ToString();
                    return;

                case "USER_DEFINED":
                    this.inputForm.UpdateType = "RX_BAUDS";
                    this.inputForm.ShowDialog();
                    return;
            }
        }

        private void frmReceiverConfigCancelBtn_Click(object sender, EventArgs e)
        {
            m_SChildform = null;
            base.Close();
        }

        private void frmReceiverConfigDefaultClockComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigDefaultClockComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "DEFAULT_CLK";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxDefaultClocks[this.currentIndex] = this.frmReceiverConfigDefaultClockComboBox.SelectedItem.ToString();
                this.frmReceiverConfigDefaultClockComboBox.Text = this.frmReceiverConfigDefaultClockComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigDoneBtn_Click(object sender, EventArgs e)
        {
            Hashtable automationParamsHash = clsGlobal.AutomationParamsHash;
            string text = string.Empty;
            string nameStr = string.Empty;
            nameStr = "PROD_FAMILY";
            text = this.list2String(this.rxFamily, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "RX_TYPES";
            text = this.list2String(this.rxTypes, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "RX_BAUDS";
            text = this.list2String(this.rxBauds, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "RX_NAMES";
            text = this.list2String(this.rxNames, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "ACTIVE_PORTS";
            text = this.list2String(this.rxPorts, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "PHY_COMM_PROTOCOL";
            text = this.list2String(this.rxPhyCommProtocols, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "TCPIP_IP_ADDRESS";
            text = this.list2String(this.rxIPAddress, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "TTB_PORTS";
            text = this.list2String(this.rxTTBPorts, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "DEFAULT_CLK";
            text = this.list2String(this.rxDefaultClocks, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "EXT_CLK_FREQ";
            text = this.list2String(this.rxExtClkFreqs, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "EXT_CLK_SRC";
            text = this.list2String(this.rxRefFreqSrc, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "LNA_TYPE";
            text = this.list2String(this.rxLnaType, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "LDO_MODE";
            text = this.list2String(this.rxLdoMode, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "AI3_ICD";
            text = this.list2String(this.rxAI3ICDs, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "F_ICD";
            text = this.list2String(this.rxFICDs, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "RX_LOG_TYPES";
            text = this.list2String(this.rxRxLogTypes, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "TX_LOG_TYPES";
            text = this.list2String(this.rxTxLogTypes, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "MESSAGE_PROTOCOL";
            text = this.list2String(this.rxMessageProtocols, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "AIDING_PROTOCOL";
            text = this.list2String(this.rxAidingProtocols, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "REQUIRED_HOSTS";
            text = this.list2String(this.rxRequiredHosts, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "REQUIRED_PATCH";
            text = this.list2String(this.rxRequiredPatches, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "TRACKER_PORTS";
            text = this.list2String(this.rxTrackerPorts, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "HOST_PORTS";
            text = this.list2String(this.rxHostPorts, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "RESET_PORTS";
            text = this.list2String(this.rxResetPorts, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "PLATFORM";
            text = this.list2String(this.rxPlatform, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "PACKAGE";
            text = this.list2String(this.rxPackage, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "REVISION";
            text = this.list2String(this.rxRev, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "MANUFACTURE_NAME";
            text = this.list2String(this.rxMfg, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "TEMPERATURE";
            text = this.list2String(this.rxTemperature, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "TEMPERATURE_UNIT";
            text = this.list2String(this.rxTemperatureUnit, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "VOLTAGE";
            text = this.list2String(this.rxVoltage, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "SIRFNAV_INTF_STR";
            text = this.list2String(this.rxSirfNavStartStopIntfStr, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "EXTRA_HOST_APP_ARGVS";
            text = this.list2String(this.rxHostSWArgs, nameStr, "");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "HOST_APP";
            text = this.list2String(this.rxHostSWs, nameStr, "-1");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "PATCH_FILE";
            text = this.list2String(this.rxPatchSWs, nameStr, "-1");
            this.addElementToHash(ref clsGlobal.AutomationParamsHash, nameStr, text);
            nameStr = "HOST_APP_DIR";
            text = this.frmReceiverConfigHostDirTxtBox.Text;
            if (clsGlobal.AutomationParamsHash.ContainsKey(nameStr))
            {
                clsGlobal.AutomationParamsHash[nameStr] = text;
            }
            else
            {
                clsGlobal.AutomationParamsHash.Add(nameStr, text);
            }
            this.updateParent(clsGlobal.AutomationParamsHash);
            m_SChildform = null;
            base.Close();
        }

        private void frmReceiverConfigExtClkFreqComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigExtClkFreqComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "EXT_CLK_FREQ";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxExtClkFreqs[this.currentIndex] = this.frmReceiverConfigExtClkFreqComboBox.SelectedItem.ToString();
                this.frmReceiverConfigExtClkFreqComboBox.Text = this.frmReceiverConfigExtClkFreqComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigFamilyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rxFamily[this.currentIndex] = this.frmReceiverConfigFamilyComboBox.SelectedItem.ToString();
            this.frmReceiverConfigFamilyComboBox.Text = this.rxFamily[this.currentIndex];
            this.updateParamsPerFamily();
        }

        private void frmReceiverConfigFICDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigFICDComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "F_ICD";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxFICDs[this.currentIndex] = this.frmReceiverConfigFICDComboBox.SelectedItem.ToString();
                this.frmReceiverConfigFICDComboBox.Text = this.frmReceiverConfigFICDComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigHostArgsTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxHostSWArgs[this.currentIndex] = this.frmReceiverConfigHostArgsTxtBox.Text;
        }

        private void frmReceiverConfigHostDirBrowserBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.frmReceiverConfigHostDirTxtBox.Text = dialog.SelectedPath;
                this.hostNPatchSWPath = this.frmReceiverConfigHostDirTxtBox.Text;
            }
        }

        private void frmReceiverConfigHostPortTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxHostPorts[this.currentIndex] = this.frmReceiverConfigHostPortTxtBox.Text;
        }

        private void frmReceiverConfigHostSWBrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = this.hostNPatchSWPath;
            dialog.Filter = "Host Apps (*.exe)|*.exe|All file (*.*)|*.*";
            dialog.FilterIndex = 3;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] strArray = dialog.FileName.Split(new char[] { '\\' });
                this.frmReceiverConfigHostSWTxtBox.Text = strArray[strArray.Length - 1];
            }
        }

        private void frmReceiverConfigHostSWTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxHostSWs[this.currentIndex] = this.frmReceiverConfigHostSWTxtBox.Text;
        }

        private void frmReceiverConfigIPAddressTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxIPAddress[this.currentIndex] = this.frmReceiverConfigIPAddressTxtBox.Text;
        }

        private void frmReceiverConfigLDOModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rxLdoMode[this.currentIndex] = this.frmReceiverConfigLDOModeComboBox.SelectedIndex.ToString();
            this.frmReceiverConfigLDOModeComboBox.Text = this.frmReceiverConfigLDOModeComboBox.SelectedItem.ToString();
        }

        private void frmReceiverConfigLnaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rxLnaType[this.currentIndex] = this.frmReceiverConfigLnaComboBox.SelectedIndex.ToString();
            this.frmReceiverConfigLnaComboBox.Text = this.frmReceiverConfigLnaComboBox.SelectedItem.ToString();
        }

        private void frmReceiverConfigMessageProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigMessageProtocolComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "MESSAGE_PROTOCOL";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxMessageProtocols[this.currentIndex] = this.frmReceiverConfigMessageProtocolComboBox.SelectedItem.ToString();
                this.frmReceiverConfigMessageProtocolComboBox.Text = this.frmReceiverConfigMessageProtocolComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigNextBtn_Click(object sender, EventArgs e)
        {
            this.currentIndex++;
            if (this.currentIndex >= this.rxPorts.Count)
            {
                this.currentIndex--;
                MessageBox.Show("Max configured receivers reached", "Information");
            }
            else
            {
                this.updateformFields(this.currentIndex);
            }
        }

        private void frmReceiverConfigPatchSWBrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = this.hostNPatchSWPath;
            dialog.Filter = "Patch files (*.pd2)|*.pd2|All file (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] strArray = dialog.FileName.Split(new char[] { '\\' });
                this.frmReceiverConfigPatchSWTxtBox.Text = strArray[strArray.Length - 1];
            }
        }

        private void frmReceiverConfigPatchSWTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxPatchSWs[this.currentIndex] = this.frmReceiverConfigPatchSWTxtBox.Text;
        }

        private void frmReceiverConfigPhyCommProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rxPhyCommProtocols[this.currentIndex] = this.frmReceiverConfigPhyCommProtocolComboBox.SelectedItem.ToString();
            this.frmReceiverConfigPhyCommProtocolComboBox.Text = this.frmReceiverConfigPhyCommProtocolComboBox.SelectedItem.ToString();
            if (this.rxPhyCommProtocols[this.currentIndex] != "UART")
            {
                this.frmReceiverConfigHostPortTxtBox.Text = "-1";
                this.rxHostPorts[this.currentIndex] = "-1";
                this.frmReceiverConfigHostPortTxtBox.Enabled = false;
            }
            else if (!this.frmReceiverConfigRequiredHostChkBox.Checked)
            {
                this.frmReceiverConfigHostPortTxtBox.Text = "-1";
                this.rxHostPorts[this.currentIndex] = "-1";
                this.frmReceiverConfigHostPortTxtBox.Enabled = false;
            }
            else
            {
                this.frmReceiverConfigHostPortTxtBox.Enabled = true;
                if (this.frmReceiverConfigHostPortTxtBox.Text == "-1")
                {
                    this.frmReceiverConfigHostPortTxtBox.Text = "";
                    this.rxHostPorts[this.currentIndex] = "";
                }
            }
        }

        private void frmReceiverConfigRemoveBtn_Click(object sender, EventArgs e)
        {
            if (this.rxPorts.Count <= 1)
            {
                MessageBox.Show("Min configured receivers reached", "Information");
            }
            else
            {
                this.rxTypes.RemoveAt(this.currentIndex);
                this.rxBauds.RemoveAt(this.currentIndex);
                this.rxNames.RemoveAt(this.currentIndex);
                this.rxPorts.RemoveAt(this.currentIndex);
                this.rxTTBPorts.RemoveAt(this.currentIndex);
                this.rxDefaultClocks.RemoveAt(this.currentIndex);
                this.rxExtClkFreqs.RemoveAt(this.currentIndex);
                this.rxAI3ICDs.RemoveAt(this.currentIndex);
                this.rxFICDs.RemoveAt(this.currentIndex);
                this.rxRxLogTypes.RemoveAt(this.currentIndex);
                this.rxTxLogTypes.RemoveAt(this.currentIndex);
                this.rxRequiredHosts.RemoveAt(this.currentIndex);
                this.rxRequiredPatches.RemoveAt(this.currentIndex);
                this.rxTrackerPorts.RemoveAt(this.currentIndex);
                this.rxHostPorts.RemoveAt(this.currentIndex);
                this.rxHostSWArgs.RemoveAt(this.currentIndex);
                this.rxHostSWs.RemoveAt(this.currentIndex);
                this.rxPatchSWs.RemoveAt(this.currentIndex);
                this.rxAidingProtocols.RemoveAt(this.currentIndex);
                this.rxMessageProtocols.RemoveAt(this.currentIndex);
                this.rxPhyCommProtocols.RemoveAt(this.currentIndex);
                this.rxIPAddress.RemoveAt(this.currentIndex);
                this.rxFamily.RemoveAt(this.currentIndex);
                this.rxResetPorts.RemoveAt(this.currentIndex);
                this.rxLnaType.RemoveAt(this.currentIndex);
                this.rxLdoMode.RemoveAt(this.currentIndex);
                this.rxSirfNavStartStopIntfStr.RemoveAt(this.currentIndex);
                this.rxTemperature.RemoveAt(this.currentIndex);
                this.rxTemperatureUnit.RemoveAt(this.currentIndex);
                this.rxRefFreqSrc.RemoveAt(this.currentIndex);
                this.rxPlatform.RemoveAt(this.currentIndex);
                this.rxPackage.RemoveAt(this.currentIndex);
                this.rxMfg.RemoveAt(this.currentIndex);
                this.rxRev.RemoveAt(this.currentIndex);
                this.currentIndex--;
                if (this.currentIndex < 0)
                {
                    this.currentIndex = 0;
                }
                this.updateformFields(this.currentIndex);
            }
        }

        private void frmReceiverConfigRequiredHostChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmReceiverConfigRequiredHostChkBox.Checked)
            {
                if (this.frmReceiverConfigPhyCommProtocolComboBox.Text == "UART")
                {
                    this.frmReceiverConfigHostPortTxtBox.Enabled = true;
                    if (this.frmReceiverConfigHostPortTxtBox.Text == "-1")
                    {
                        this.frmReceiverConfigHostPortTxtBox.Text = "";
                    }
                }
                else
                {
                    this.frmReceiverConfigHostPortTxtBox.Enabled = false;
                    this.frmReceiverConfigHostPortTxtBox.Text = "-1";
                }
                this.frmReceiverConfigHostSWTxtBox.Enabled = true;
                this.frmReceiverConfigHostSWBrowseBtn.Enabled = true;
                this.frmReceiverConfigHostArgsTxtBox.Enabled = true;
                this.frmReceiverConfigHostDirTxtBox.Enabled = true;
                this.frmReceiverConfigHostDirBrowserBtn.Enabled = true;
                this.frmReceiverConfigHostPortTxtBox.Enabled = true;
                this.frmReceiverConfigTrackerPortTxtBox.Enabled = true;
                this.frmReceiverConfigResetPortTxtBox.Enabled = true;
                this.rxRequiredHosts[this.currentIndex] = "1";
                this.rxHostSWs[this.currentIndex] = this.frmReceiverConfigHostSWTxtBox.Text;
                this.rxHostSWArgs[this.currentIndex] = this.frmReceiverConfigHostArgsTxtBox.Text;
                this.rxHostPorts[this.currentIndex] = this.frmReceiverConfigHostPortTxtBox.Text;
                this.rxTrackerPorts[this.currentIndex] = this.frmReceiverConfigTrackerPortTxtBox.Text;
                this.rxResetPorts[this.currentIndex] = this.frmReceiverConfigResetPortTxtBox.Text;
            }
            else
            {
                this.frmReceiverConfigHostSWTxtBox.Enabled = false;
                this.frmReceiverConfigHostArgsTxtBox.Enabled = false;
                this.frmReceiverConfigHostSWBrowseBtn.Enabled = false;
                this.frmReceiverConfigHostDirTxtBox.Enabled = false;
                this.frmReceiverConfigHostDirBrowserBtn.Enabled = false;
                this.frmReceiverConfigHostPortTxtBox.Enabled = true;
                this.frmReceiverConfigTrackerPortTxtBox.Enabled = true;
                this.frmReceiverConfigResetPortTxtBox.Enabled = true;
                this.rxRequiredHosts[this.currentIndex] = "0";
                this.rxHostSWs[this.currentIndex] = "-1";
                this.rxHostSWArgs[this.currentIndex] = "-1";
            }
        }

        private void frmReceiverConfigRequiredPatchChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmReceiverConfigRequiredPatchChkBox.Checked)
            {
                this.frmReceiverConfigPatchSWTxtBox.Enabled = true;
                this.frmReceiverConfigPatchSWBrowseBtn.Enabled = true;
                this.rxRequiredPatches[this.currentIndex] = "1";
                this.rxPatchSWs[this.currentIndex] = this.frmReceiverConfigPatchSWTxtBox.Text;
            }
            else
            {
                this.frmReceiverConfigPatchSWTxtBox.Enabled = false;
                this.frmReceiverConfigPatchSWBrowseBtn.Enabled = false;
                this.rxRequiredPatches[this.currentIndex] = "0";
                this.rxPatchSWs[this.currentIndex] = "-1";
            }
        }

        private void frmReceiverConfigResetPortTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxResetPorts[this.currentIndex] = this.frmReceiverConfigResetPortTxtBox.Text;
        }

        private void frmReceiverConfigRxLogTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigRxLogTypeComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "RX_LOG_TYPES";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxRxLogTypes[this.currentIndex] = this.frmReceiverConfigRxLogTypeComboBox.SelectedItem.ToString();
                this.frmReceiverConfigRxLogTypeComboBox.Text = this.frmReceiverConfigRxLogTypeComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigRxNameTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxNames[this.currentIndex] = this.frmReceiverConfigNameTxtBox.Text;
        }

        private void frmReceiverConfigRxPortTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxPorts[this.currentIndex] = this.frmReceiverConfigPortTxtBox.Text;
        }

        private void frmReceiverConfigRxTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigTypeComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "RX_TYPES";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxTypes[this.currentIndex] = this.frmReceiverConfigTypeComboBox.SelectedItem.ToString();
                this.frmReceiverConfigTypeComboBox.Text = this.frmReceiverConfigTypeComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverConfigTrackerPortTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxTrackerPorts[this.currentIndex] = this.frmReceiverConfigTrackerPortTxtBox.Text;
        }

        private void frmReceiverConfigTTBPortTxtBox_TextChanged(object sender, EventArgs e)
        {
            this.rxTTBPorts[this.currentIndex] = this.frmReceiverConfigTTBPortTxtBox.Text;
        }

        private void frmReceiverConfigTxLogTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (((str = this.frmReceiverConfigTxLogTypeComboBox.SelectedItem.ToString()) != null) && (str == "USER_DEFINED"))
            {
                this.inputForm.UpdateType = "TX_LOG_TYPES";
                this.inputForm.ShowDialog();
            }
            else
            {
                this.rxTxLogTypes[this.currentIndex] = this.frmReceiverConfigTxLogTypeComboBox.SelectedItem.ToString();
                this.frmReceiverConfigTxLogTypeComboBox.Text = this.frmReceiverConfigTxLogTypeComboBox.SelectedItem.ToString();
            }
        }

        private void frmReceiverMfgComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.frmReceiverMfgComboBox.Text = this.frmReceiverMfgComboBox.SelectedItem.ToString();
            this.rxMfg[this.currentIndex] = this.frmReceiverMfgComboBox.Text;
        }

        private void frmReceiverPackageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.frmReceiverPackageComboBox.Text = this.frmReceiverPackageComboBox.SelectedItem.ToString();
            this.rxPackage[this.currentIndex] = this.frmReceiverPackageComboBox.Text;
        }

        private void frmReceiverPlatformCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.frmReceiverPlatformCombox.Text = this.frmReceiverPlatformCombox.SelectedItem.ToString();
            this.rxPlatform[this.currentIndex] = this.frmReceiverPlatformCombox.Text;
        }

        private void frmReceiverRevTextBox_TextChanged(object sender, EventArgs e)
        {
            this.rxRev[this.currentIndex] = this.frmReceiverRevTextBox.Text;
        }

        internal static frmReceiverConfig GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmReceiverConfig();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmReceiverConfig));
            this.frmReceiverConfigPortTxtBox = new TextBox();
            this.frmReceiverConfigDUTPortLabel = new Label();
            this.frmReceiverConfigTitle = new Label();
            this.frmReceiverConfigTrackerPortLabel = new Label();
            this.frmReceiverConfigTrackerPortTxtBox = new TextBox();
            this.frmReceiverConfigHostPortLabel = new Label();
            this.frmReceiverConfigHostPortTxtBox = new TextBox();
            this.frmReceiverConfigRequiredHostChkBox = new CheckBox();
            this.frmReceiverConfigRequiredPatchChkBox = new CheckBox();
            this.frmReceiverConfigHostSWLabel = new Label();
            this.frmReceiverConfigHostSWTxtBox = new TextBox();
            this.frmReceiverConfigHostSWBrowseBtn = new Button();
            this.frmReceiverConfigHostArgsLabel = new Label();
            this.frmReceiverConfigHostArgsTxtBox = new TextBox();
            this.frmReceiverConfigDUTBaudLabel = new Label();
            this.frmReceiverConfigNameTxtBox = new TextBox();
            this.frmReceiverConfigNameLabel = new Label();
            this.frmReceiverConfigRxLogTypeLabel = new Label();
            this.frmReceiverConfigTxLogTypeLabel = new Label();
            this.frmReceiverConfigTypeLabel = new Label();
            this.frmReceiverConfigAI3ICDLabel = new Label();
            this.frmReceiverConfigFICDLabel = new Label();
            this.frmReceiverConfigPatchSWLabel = new Label();
            this.frmReceiverConfigPatchSWTxtBox = new TextBox();
            this.frmReceiverConfigPatchSWBrowseBtn = new Button();
            this.frmReceiverConfigRxIndexLabel = new Label();
            this.frmReceiverConfigDefaultClockLabel = new Label();
            this.frmReceiverConfigDoneBtn = new Button();
            this.frmReceiverConfigCancelBtn = new Button();
            this.frmConfigReceiverBackBtn = new Button();
            this.frmReceiverConfigNextBtn = new Button();
            this.frmReceiverConfigAddBtn = new Button();
            this.frmReceiverConfigRemoveBtn = new Button();
            this.frmReceiverConfigMessageProtocolLabel = new Label();
            this.frmReceiverConfigAidingProtocolLabel = new Label();
            this.frmReceiverConfigHostDirTxtBox = new TextBox();
            this.frmReceiverConfigHostDirBrowserBtn = new Button();
            this.frmReceiverConfigTypeComboBox = new ComboBox();
            this.frmReceiverConfigDefaultClockComboBox = new ComboBox();
            this.frmReceiverConfigMessageProtocolComboBox = new ComboBox();
            this.frmReceiverConfigComboBox = new ComboBox();
            this.frmReceiverConfigAI3ICDComboBox = new ComboBox();
            this.frmReceiverConfigFICDComboBox = new ComboBox();
            this.frmReceiverConfigRxLogTypeComboBox = new ComboBox();
            this.frmReceiverConfigTxLogTypeComboBox = new ComboBox();
            this.frmReceiverConfigAidingProtocolComboBox = new ComboBox();
            this.frmReceiverConfigExtClkFreqLabel = new Label();
            this.frmReceiverConfigExtClkFreqComboBox = new ComboBox();
            this.frmReceiverConfigTTBPortTxtBox = new TextBox();
            this.frmReceiverConfigTTBPortLabel = new Label();
            this.frmReceiverConfigPhyCommProtocolComboBox = new ComboBox();
            this.frmReceiverConfigPhyCommProtocolLabel = new Label();
            this.frmReceiverConfigIPAddressLabel = new Label();
            this.frmReceiverConfigIPAddressTxtBox = new TextBox();
            this.frmReceiverConfigFamilyComboBox = new ComboBox();
            this.frmReceiverConfigFamilyLabel = new Label();
            this.frmReceiverConfigLDOModeComboBox = new ComboBox();
            this.frmReceiverConfigLDOModeLabel = new Label();
            this.frmReceiverConfigLnaComboBox = new ComboBox();
            this.frmReceiverConfigLnaTypeLabel = new Label();
            this.frmReceiverConfigResetPortLabel = new Label();
            this.frmReceiverConfigResetPortTxtBox = new TextBox();
            this.frmReceiverPlatformCombox = new ComboBox();
            this.platformLabel = new Label();
            this.frmReceiverPackageLabel = new Label();
            this.frmReceiverPackageComboBox = new ComboBox();
            this.frmReceiverRevLabel = new Label();
            this.frmReceiverMfgLabel = new Label();
            this.frmReceiverMfgComboBox = new ComboBox();
            this.refFreqSrcComboBox = new ComboBox();
            this.refFreqSrcLabel = new Label();
            this.sirfNavStartStopTextBox = new TextBox();
            this.sirfNavStrLabel = new Label();
            this.temperatureUnitLabel = new Label();
            this.temperUnitComboBox = new ComboBox();
            this.temperLabel = new Label();
            this.temperTextBox = new TextBox();
            this.frmReceiverRevTextBox = new TextBox();
            this.rxVoltlabel = new Label();
            this.rxVoltTextBox = new TextBox();
            Label label = new Label();
            base.SuspendLayout();
            label.AutoSize = true;
            label.Location = new Point(0x18, 0x163);
            label.Name = "frmReceiverConfigHostDirLabel";
            label.Size = new Size(0x63, 13);
            label.TabIndex = 0x1a;
            label.Text = "Host App Directory:";
            this.frmReceiverConfigPortTxtBox.Location = new Point(0x5d, 0x5d);
            this.frmReceiverConfigPortTxtBox.Name = "frmReceiverConfigPortTxtBox";
            this.frmReceiverConfigPortTxtBox.Size = new Size(0x51, 20);
            this.frmReceiverConfigPortTxtBox.TabIndex = 3;
            this.frmReceiverConfigPortTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigRxPortTxtBox_TextChanged);
            this.frmReceiverConfigDUTPortLabel.AutoSize = true;
            this.frmReceiverConfigDUTPortLabel.Location = new Point(20, 0x61);
            this.frmReceiverConfigDUTPortLabel.Name = "frmReceiverConfigDUTPortLabel";
            this.frmReceiverConfigDUTPortLabel.Size = new Size(0x37, 13);
            this.frmReceiverConfigDUTPortLabel.TabIndex = 2;
            this.frmReceiverConfigDUTPortLabel.Text = "Main Port:";
            this.frmReceiverConfigTitle.AutoSize = true;
            this.frmReceiverConfigTitle.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.frmReceiverConfigTitle.Location = new Point(0xf1, 0x10);
            this.frmReceiverConfigTitle.Name = "frmReceiverConfigTitle";
            this.frmReceiverConfigTitle.Size = new Size(0xc0, 20);
            this.frmReceiverConfigTitle.TabIndex = 0;
            this.frmReceiverConfigTitle.Text = "Receiver Configuration";
            this.frmReceiverConfigTitle.TextAlign = ContentAlignment.TopCenter;
            this.frmReceiverConfigTrackerPortLabel.AutoSize = true;
            this.frmReceiverConfigTrackerPortLabel.Location = new Point(20, 0x89);
            this.frmReceiverConfigTrackerPortLabel.Name = "frmReceiverConfigTrackerPortLabel";
            this.frmReceiverConfigTrackerPortLabel.Size = new Size(0x45, 13);
            this.frmReceiverConfigTrackerPortLabel.TabIndex = 30;
            this.frmReceiverConfigTrackerPortLabel.Text = "Tracker Port:";
            this.frmReceiverConfigTrackerPortTxtBox.Location = new Point(0x5d, 0x85);
            this.frmReceiverConfigTrackerPortTxtBox.Name = "frmReceiverConfigTrackerPortTxtBox";
            this.frmReceiverConfigTrackerPortTxtBox.Size = new Size(0x51, 20);
            this.frmReceiverConfigTrackerPortTxtBox.TabIndex = 0x1f;
            this.frmReceiverConfigTrackerPortTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigTrackerPortTxtBox_TextChanged);
            this.frmReceiverConfigHostPortLabel.AutoSize = true;
            this.frmReceiverConfigHostPortLabel.Location = new Point(20, 0x9d);
            this.frmReceiverConfigHostPortLabel.Name = "frmReceiverConfigHostPortLabel";
            this.frmReceiverConfigHostPortLabel.Size = new Size(0x36, 13);
            this.frmReceiverConfigHostPortLabel.TabIndex = 0x20;
            this.frmReceiverConfigHostPortLabel.Text = "Host Port:";
            this.frmReceiverConfigHostPortTxtBox.Location = new Point(0x5d, 0x99);
            this.frmReceiverConfigHostPortTxtBox.Name = "frmReceiverConfigHostPortTxtBox";
            this.frmReceiverConfigHostPortTxtBox.Size = new Size(0x51, 20);
            this.frmReceiverConfigHostPortTxtBox.TabIndex = 0x21;
            this.frmReceiverConfigHostPortTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigHostPortTxtBox_TextChanged);
            this.frmReceiverConfigRequiredHostChkBox.AutoSize = true;
            this.frmReceiverConfigRequiredHostChkBox.Location = new Point(0x17, 0x149);
            this.frmReceiverConfigRequiredHostChkBox.Name = "frmReceiverConfigRequiredHostChkBox";
            this.frmReceiverConfigRequiredHostChkBox.Size = new Size(0x4d, 0x11);
            this.frmReceiverConfigRequiredHostChkBox.TabIndex = 0x1d;
            this.frmReceiverConfigRequiredHostChkBox.Text = "Run Host?";
            this.frmReceiverConfigRequiredHostChkBox.UseVisualStyleBackColor = true;
            this.frmReceiverConfigRequiredHostChkBox.CheckedChanged += new EventHandler(this.frmReceiverConfigRequiredHostChkBox_CheckedChanged);
            this.frmReceiverConfigRequiredPatchChkBox.AutoSize = true;
            this.frmReceiverConfigRequiredPatchChkBox.Location = new Point(0x17, 0x1a8);
            this.frmReceiverConfigRequiredPatchChkBox.Name = "frmReceiverConfigRequiredPatchChkBox";
            this.frmReceiverConfigRequiredPatchChkBox.Size = new Size(0x6a, 0x11);
            this.frmReceiverConfigRequiredPatchChkBox.TabIndex = 0x27;
            this.frmReceiverConfigRequiredPatchChkBox.Text = "Required Patch?";
            this.frmReceiverConfigRequiredPatchChkBox.UseVisualStyleBackColor = true;
            this.frmReceiverConfigRequiredPatchChkBox.CheckedChanged += new EventHandler(this.frmReceiverConfigRequiredPatchChkBox_CheckedChanged);
            this.frmReceiverConfigHostSWLabel.AutoSize = true;
            this.frmReceiverConfigHostSWLabel.Location = new Point(0x17, 0x177);
            this.frmReceiverConfigHostSWLabel.Name = "frmReceiverConfigHostSWLabel";
            this.frmReceiverConfigHostSWLabel.Size = new Size(0x35, 13);
            this.frmReceiverConfigHostSWLabel.TabIndex = 0x22;
            this.frmReceiverConfigHostSWLabel.Text = "Host SW:";
            this.frmReceiverConfigHostSWTxtBox.Location = new Point(0x9c, 0x173);
            this.frmReceiverConfigHostSWTxtBox.Name = "frmReceiverConfigHostSWTxtBox";
            this.frmReceiverConfigHostSWTxtBox.Size = new Size(0x1c4, 20);
            this.frmReceiverConfigHostSWTxtBox.TabIndex = 0x23;
            this.frmReceiverConfigHostSWTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigHostSWTxtBox_TextChanged);
            this.frmReceiverConfigHostSWBrowseBtn.Location = new Point(0x266, 0x174);
            this.frmReceiverConfigHostSWBrowseBtn.Name = "frmReceiverConfigHostSWBrowseBtn";
            this.frmReceiverConfigHostSWBrowseBtn.Size = new Size(0x1f, 0x17);
            this.frmReceiverConfigHostSWBrowseBtn.TabIndex = 0x24;
            this.frmReceiverConfigHostSWBrowseBtn.Text = "...";
            this.frmReceiverConfigHostSWBrowseBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigHostSWBrowseBtn.Click += new EventHandler(this.frmReceiverConfigHostSWBrowseBtn_Click);
            this.frmReceiverConfigHostArgsLabel.AutoSize = true;
            this.frmReceiverConfigHostArgsLabel.Location = new Point(0x17, 0x18b);
            this.frmReceiverConfigHostArgsLabel.Name = "frmReceiverConfigHostArgsLabel";
            this.frmReceiverConfigHostArgsLabel.Size = new Size(0x86, 13);
            this.frmReceiverConfigHostArgsLabel.TabIndex = 0x25;
            this.frmReceiverConfigHostArgsLabel.Text = "Additional Host Arguments:";
            this.frmReceiverConfigHostArgsTxtBox.Location = new Point(0x9c, 0x187);
            this.frmReceiverConfigHostArgsTxtBox.Name = "frmReceiverConfigHostArgsTxtBox";
            this.frmReceiverConfigHostArgsTxtBox.Size = new Size(0x1c4, 20);
            this.frmReceiverConfigHostArgsTxtBox.TabIndex = 0x26;
            this.frmReceiverConfigHostArgsTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigHostArgsTxtBox_TextChanged);
            this.frmReceiverConfigDUTBaudLabel.AutoSize = true;
            this.frmReceiverConfigDUTBaudLabel.Location = new Point(0xca, 0x61);
            this.frmReceiverConfigDUTBaudLabel.Name = "frmReceiverConfigDUTBaudLabel";
            this.frmReceiverConfigDUTBaudLabel.Size = new Size(0x23, 13);
            this.frmReceiverConfigDUTBaudLabel.TabIndex = 4;
            this.frmReceiverConfigDUTBaudLabel.Text = "Baud:";
            this.frmReceiverConfigNameTxtBox.Location = new Point(0x5d, 0x71);
            this.frmReceiverConfigNameTxtBox.Name = "frmReceiverConfigNameTxtBox";
            this.frmReceiverConfigNameTxtBox.Size = new Size(0x51, 20);
            this.frmReceiverConfigNameTxtBox.TabIndex = 7;
            this.frmReceiverConfigNameTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigRxNameTxtBox_TextChanged);
            this.frmReceiverConfigNameLabel.AutoSize = true;
            this.frmReceiverConfigNameLabel.Location = new Point(20, 0x76);
            this.frmReceiverConfigNameLabel.Name = "frmReceiverConfigNameLabel";
            this.frmReceiverConfigNameLabel.Size = new Size(0x26, 13);
            this.frmReceiverConfigNameLabel.TabIndex = 6;
            this.frmReceiverConfigNameLabel.Text = "Name:";
            this.frmReceiverConfigRxLogTypeLabel.AutoSize = true;
            this.frmReceiverConfigRxLogTypeLabel.Location = new Point(460, 0x119);
            this.frmReceiverConfigRxLogTypeLabel.Name = "frmReceiverConfigRxLogTypeLabel";
            this.frmReceiverConfigRxLogTypeLabel.Size = new Size(0x6a, 13);
            this.frmReceiverConfigRxLogTypeLabel.TabIndex = 0x10;
            this.frmReceiverConfigRxLogTypeLabel.Text = "Receive Log Format:";
            this.frmReceiverConfigTxLogTypeLabel.AutoSize = true;
            this.frmReceiverConfigTxLogTypeLabel.Location = new Point(460, 0x12e);
            this.frmReceiverConfigTxLogTypeLabel.Name = "frmReceiverConfigTxLogTypeLabel";
            this.frmReceiverConfigTxLogTypeLabel.Size = new Size(0x6a, 13);
            this.frmReceiverConfigTxLogTypeLabel.TabIndex = 0x12;
            this.frmReceiverConfigTxLogTypeLabel.Text = "Transmit Log Format:";
            this.frmReceiverConfigTypeLabel.AutoSize = true;
            this.frmReceiverConfigTypeLabel.Location = new Point(20, 0xd9);
            this.frmReceiverConfigTypeLabel.Name = "frmReceiverConfigTypeLabel";
            this.frmReceiverConfigTypeLabel.Size = new Size(0x22, 13);
            this.frmReceiverConfigTypeLabel.TabIndex = 8;
            this.frmReceiverConfigTypeLabel.Text = "Type:";
            this.frmReceiverConfigAI3ICDLabel.AutoSize = true;
            this.frmReceiverConfigAI3ICDLabel.Location = new Point(460, 0xef);
            this.frmReceiverConfigAI3ICDLabel.Name = "frmReceiverConfigAI3ICDLabel";
            this.frmReceiverConfigAI3ICDLabel.Size = new Size(60, 13);
            this.frmReceiverConfigAI3ICDLabel.TabIndex = 10;
            this.frmReceiverConfigAI3ICDLabel.Text = "Aiding ICD:";
            this.frmReceiverConfigFICDLabel.AutoSize = true;
            this.frmReceiverConfigFICDLabel.Location = new Point(460, 260);
            this.frmReceiverConfigFICDLabel.Name = "frmReceiverConfigFICDLabel";
            this.frmReceiverConfigFICDLabel.Size = new Size(0x40, 13);
            this.frmReceiverConfigFICDLabel.TabIndex = 12;
            this.frmReceiverConfigFICDLabel.Text = "Control ICD:";
            this.frmReceiverConfigPatchSWLabel.AutoSize = true;
            this.frmReceiverConfigPatchSWLabel.Location = new Point(0x17, 0x1be);
            this.frmReceiverConfigPatchSWLabel.Name = "frmReceiverConfigPatchSWLabel";
            this.frmReceiverConfigPatchSWLabel.Size = new Size(0x3b, 13);
            this.frmReceiverConfigPatchSWLabel.TabIndex = 40;
            this.frmReceiverConfigPatchSWLabel.Text = "Patch SW:";
            this.frmReceiverConfigPatchSWTxtBox.Location = new Point(0x9c, 0x1ba);
            this.frmReceiverConfigPatchSWTxtBox.Name = "frmReceiverConfigPatchSWTxtBox";
            this.frmReceiverConfigPatchSWTxtBox.Size = new Size(0x1c4, 20);
            this.frmReceiverConfigPatchSWTxtBox.TabIndex = 0x29;
            this.frmReceiverConfigPatchSWTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigPatchSWTxtBox_TextChanged);
            this.frmReceiverConfigPatchSWBrowseBtn.Location = new Point(0x265, 0x1b7);
            this.frmReceiverConfigPatchSWBrowseBtn.Name = "frmReceiverConfigPatchSWBrowseBtn";
            this.frmReceiverConfigPatchSWBrowseBtn.Size = new Size(0x1f, 0x17);
            this.frmReceiverConfigPatchSWBrowseBtn.TabIndex = 0x2a;
            this.frmReceiverConfigPatchSWBrowseBtn.Text = "...";
            this.frmReceiverConfigPatchSWBrowseBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigPatchSWBrowseBtn.Click += new EventHandler(this.frmReceiverConfigPatchSWBrowseBtn_Click);
            this.frmReceiverConfigRxIndexLabel.AutoSize = true;
            this.frmReceiverConfigRxIndexLabel.Location = new Point(0x13d, 0x2e);
            this.frmReceiverConfigRxIndexLabel.Name = "frmReceiverConfigRxIndexLabel";
            this.frmReceiverConfigRxIndexLabel.Size = new Size(40, 13);
            this.frmReceiverConfigRxIndexLabel.TabIndex = 1;
            this.frmReceiverConfigRxIndexLabel.Text = "DUT #";
            this.frmReceiverConfigDefaultClockLabel.AutoSize = true;
            this.frmReceiverConfigDefaultClockLabel.Location = new Point(0xca, 0xb1);
            this.frmReceiverConfigDefaultClockLabel.Name = "frmReceiverConfigDefaultClockLabel";
            this.frmReceiverConfigDefaultClockLabel.Size = new Size(0x76, 13);
            this.frmReceiverConfigDefaultClockLabel.TabIndex = 14;
            this.frmReceiverConfigDefaultClockLabel.Text = "Default Freq Offset(Hz):";
            this.frmReceiverConfigDoneBtn.Location = new Point(0x1f6, 0x48);
            this.frmReceiverConfigDoneBtn.Name = "frmReceiverConfigDoneBtn";
            this.frmReceiverConfigDoneBtn.Size = new Size(0x35, 0x17);
            this.frmReceiverConfigDoneBtn.TabIndex = 0x2f;
            this.frmReceiverConfigDoneBtn.Text = "&Done";
            this.frmReceiverConfigDoneBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigDoneBtn.Click += new EventHandler(this.frmReceiverConfigDoneBtn_Click);
            this.frmReceiverConfigCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmReceiverConfigCancelBtn.Location = new Point(0x1f6, 0x63);
            this.frmReceiverConfigCancelBtn.Name = "frmReceiverConfigCancelBtn";
            this.frmReceiverConfigCancelBtn.Size = new Size(0x35, 0x17);
            this.frmReceiverConfigCancelBtn.TabIndex = 0x30;
            this.frmReceiverConfigCancelBtn.Text = "&Cancel";
            this.frmReceiverConfigCancelBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigCancelBtn.Click += new EventHandler(this.frmReceiverConfigCancelBtn_Click);
            this.frmConfigReceiverBackBtn.Location = new Point(0x232, 0x63);
            this.frmConfigReceiverBackBtn.Name = "frmConfigReceiverBackBtn";
            this.frmConfigReceiverBackBtn.Size = new Size(0x52, 0x17);
            this.frmConfigReceiverBackBtn.TabIndex = 0x2c;
            this.frmConfigReceiverBackBtn.Text = "<< Previous";
            this.frmConfigReceiverBackBtn.UseVisualStyleBackColor = true;
            this.frmConfigReceiverBackBtn.Click += new EventHandler(this.frmConfigReceiverBackBtn_Click);
            this.frmReceiverConfigNextBtn.Location = new Point(0x232, 0x7e);
            this.frmReceiverConfigNextBtn.Name = "frmReceiverConfigNextBtn";
            this.frmReceiverConfigNextBtn.Size = new Size(0x52, 0x17);
            this.frmReceiverConfigNextBtn.TabIndex = 0x2d;
            this.frmReceiverConfigNextBtn.Text = "Next >>";
            this.frmReceiverConfigNextBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigNextBtn.Click += new EventHandler(this.frmReceiverConfigNextBtn_Click);
            this.frmReceiverConfigAddBtn.Location = new Point(0x232, 0x48);
            this.frmReceiverConfigAddBtn.Name = "frmReceiverConfigAddBtn";
            this.frmReceiverConfigAddBtn.Size = new Size(0x52, 0x17);
            this.frmReceiverConfigAddBtn.TabIndex = 0x2b;
            this.frmReceiverConfigAddBtn.Text = "Add";
            this.frmReceiverConfigAddBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigAddBtn.Click += new EventHandler(this.frmReceiverConfigAddBtn_Click);
            this.frmReceiverConfigRemoveBtn.Location = new Point(0x232, 0x99);
            this.frmReceiverConfigRemoveBtn.Name = "frmReceiverConfigRemoveBtn";
            this.frmReceiverConfigRemoveBtn.Size = new Size(0x52, 0x17);
            this.frmReceiverConfigRemoveBtn.TabIndex = 0x2e;
            this.frmReceiverConfigRemoveBtn.Text = "Delete";
            this.frmReceiverConfigRemoveBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigRemoveBtn.Click += new EventHandler(this.frmReceiverConfigRemoveBtn_Click);
            this.frmReceiverConfigMessageProtocolLabel.AutoSize = true;
            this.frmReceiverConfigMessageProtocolLabel.Location = new Point(460, 0xc5);
            this.frmReceiverConfigMessageProtocolLabel.Name = "frmReceiverConfigMessageProtocolLabel";
            this.frmReceiverConfigMessageProtocolLabel.Size = new Size(0x5f, 13);
            this.frmReceiverConfigMessageProtocolLabel.TabIndex = 0x16;
            this.frmReceiverConfigMessageProtocolLabel.Text = "Message Protocol:";
            this.frmReceiverConfigAidingProtocolLabel.AutoSize = true;
            this.frmReceiverConfigAidingProtocolLabel.Location = new Point(460, 0xda);
            this.frmReceiverConfigAidingProtocolLabel.Name = "frmReceiverConfigAidingProtocolLabel";
            this.frmReceiverConfigAidingProtocolLabel.Size = new Size(0x51, 13);
            this.frmReceiverConfigAidingProtocolLabel.TabIndex = 0x18;
            this.frmReceiverConfigAidingProtocolLabel.Text = "Aiding Protocol:";
            this.frmReceiverConfigHostDirTxtBox.Location = new Point(0x9c, 0x15f);
            this.frmReceiverConfigHostDirTxtBox.Name = "frmReceiverConfigHostDirTxtBox";
            this.frmReceiverConfigHostDirTxtBox.Size = new Size(0x1c4, 20);
            this.frmReceiverConfigHostDirTxtBox.TabIndex = 0x1b;
            this.frmReceiverConfigHostDirBrowserBtn.Location = new Point(0x266, 0x15d);
            this.frmReceiverConfigHostDirBrowserBtn.Name = "frmReceiverConfigHostDirBrowserBtn";
            this.frmReceiverConfigHostDirBrowserBtn.Size = new Size(0x1f, 0x17);
            this.frmReceiverConfigHostDirBrowserBtn.TabIndex = 0x1c;
            this.frmReceiverConfigHostDirBrowserBtn.Text = "...";
            this.frmReceiverConfigHostDirBrowserBtn.UseVisualStyleBackColor = true;
            this.frmReceiverConfigHostDirBrowserBtn.Click += new EventHandler(this.frmReceiverConfigHostDirBrowserBtn_Click);
            this.frmReceiverConfigTypeComboBox.FormattingEnabled = true;
            this.frmReceiverConfigTypeComboBox.Location = new Point(0x5d, 0xd5);
            this.frmReceiverConfigTypeComboBox.Name = "frmReceiverConfigTypeComboBox";
            this.frmReceiverConfigTypeComboBox.Size = new Size(0x51, 0x15);
            this.frmReceiverConfigTypeComboBox.TabIndex = 9;
            this.frmReceiverConfigTypeComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigRxTypeComboBox_SelectedIndexChanged);
            this.frmReceiverConfigDefaultClockComboBox.FormattingEnabled = true;
            this.frmReceiverConfigDefaultClockComboBox.Location = new Point(0x150, 0xad);
            this.frmReceiverConfigDefaultClockComboBox.Name = "frmReceiverConfigDefaultClockComboBox";
            this.frmReceiverConfigDefaultClockComboBox.Size = new Size(0x6b, 0x15);
            this.frmReceiverConfigDefaultClockComboBox.TabIndex = 15;
            this.frmReceiverConfigDefaultClockComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigDefaultClockComboBox_SelectedIndexChanged);
            this.frmReceiverConfigMessageProtocolComboBox.FormattingEnabled = true;
            this.frmReceiverConfigMessageProtocolComboBox.Location = new Point(570, 0xc1);
            this.frmReceiverConfigMessageProtocolComboBox.Name = "frmReceiverConfigMessageProtocolComboBox";
            this.frmReceiverConfigMessageProtocolComboBox.Size = new Size(0x4a, 0x15);
            this.frmReceiverConfigMessageProtocolComboBox.TabIndex = 0x17;
            this.frmReceiverConfigMessageProtocolComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigMessageProtocolComboBox_SelectedIndexChanged);
            this.frmReceiverConfigComboBox.FormattingEnabled = true;
            this.frmReceiverConfigComboBox.Items.AddRange(new object[] { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400", "460800", "USER_DEFINED" });
            this.frmReceiverConfigComboBox.Location = new Point(0x150, 0x5d);
            this.frmReceiverConfigComboBox.Name = "frmReceiverConfigComboBox";
            this.frmReceiverConfigComboBox.Size = new Size(0x6b, 0x15);
            this.frmReceiverConfigComboBox.TabIndex = 5;
            this.frmReceiverConfigComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigBaudComboBox_SelectedIndexChanged);
            this.frmReceiverConfigAI3ICDComboBox.FormattingEnabled = true;
            this.frmReceiverConfigAI3ICDComboBox.Location = new Point(570, 0xeb);
            this.frmReceiverConfigAI3ICDComboBox.Name = "frmReceiverConfigAI3ICDComboBox";
            this.frmReceiverConfigAI3ICDComboBox.Size = new Size(0x4a, 0x15);
            this.frmReceiverConfigAI3ICDComboBox.TabIndex = 11;
            this.frmReceiverConfigAI3ICDComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigAI3ICDComboBox_SelectedIndexChanged);
            this.frmReceiverConfigFICDComboBox.FormattingEnabled = true;
            this.frmReceiverConfigFICDComboBox.Location = new Point(570, 0x100);
            this.frmReceiverConfigFICDComboBox.Name = "frmReceiverConfigFICDComboBox";
            this.frmReceiverConfigFICDComboBox.Size = new Size(0x4a, 0x15);
            this.frmReceiverConfigFICDComboBox.TabIndex = 13;
            this.frmReceiverConfigFICDComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigFICDComboBox_SelectedIndexChanged);
            this.frmReceiverConfigRxLogTypeComboBox.FormattingEnabled = true;
            this.frmReceiverConfigRxLogTypeComboBox.Location = new Point(570, 0x115);
            this.frmReceiverConfigRxLogTypeComboBox.Name = "frmReceiverConfigRxLogTypeComboBox";
            this.frmReceiverConfigRxLogTypeComboBox.Size = new Size(0x4a, 0x15);
            this.frmReceiverConfigRxLogTypeComboBox.TabIndex = 0x11;
            this.frmReceiverConfigRxLogTypeComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigRxLogTypeComboBox_SelectedIndexChanged);
            this.frmReceiverConfigTxLogTypeComboBox.FormattingEnabled = true;
            this.frmReceiverConfigTxLogTypeComboBox.Location = new Point(570, 0x128);
            this.frmReceiverConfigTxLogTypeComboBox.Name = "frmReceiverConfigTxLogTypeComboBox";
            this.frmReceiverConfigTxLogTypeComboBox.Size = new Size(0x4a, 0x15);
            this.frmReceiverConfigTxLogTypeComboBox.TabIndex = 0x13;
            this.frmReceiverConfigTxLogTypeComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigTxLogTypeComboBox_SelectedIndexChanged);
            this.frmReceiverConfigAidingProtocolComboBox.FormattingEnabled = true;
            this.frmReceiverConfigAidingProtocolComboBox.Location = new Point(570, 0xd6);
            this.frmReceiverConfigAidingProtocolComboBox.Name = "frmReceiverConfigAidingProtocolComboBox";
            this.frmReceiverConfigAidingProtocolComboBox.Size = new Size(0x4a, 0x15);
            this.frmReceiverConfigAidingProtocolComboBox.TabIndex = 0x19;
            this.frmReceiverConfigAidingProtocolComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigAidingProtocolComboBox_SelectedIndexChanged);
            this.frmReceiverConfigExtClkFreqLabel.AutoSize = true;
            this.frmReceiverConfigExtClkFreqLabel.Location = new Point(0xca, 0xc6);
            this.frmReceiverConfigExtClkFreqLabel.Name = "frmReceiverConfigExtClkFreqLabel";
            this.frmReceiverConfigExtClkFreqLabel.Size = new Size(0x52, 13);
            this.frmReceiverConfigExtClkFreqLabel.TabIndex = 20;
            this.frmReceiverConfigExtClkFreqLabel.Text = "TCXO Freq(Hz):";
            this.frmReceiverConfigExtClkFreqComboBox.FormattingEnabled = true;
            this.frmReceiverConfigExtClkFreqComboBox.Location = new Point(0x150, 0xc2);
            this.frmReceiverConfigExtClkFreqComboBox.Name = "frmReceiverConfigExtClkFreqComboBox";
            this.frmReceiverConfigExtClkFreqComboBox.Size = new Size(0x6b, 0x15);
            this.frmReceiverConfigExtClkFreqComboBox.TabIndex = 0x15;
            this.frmReceiverConfigExtClkFreqComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigExtClkFreqComboBox_SelectedIndexChanged);
            this.frmReceiverConfigTTBPortTxtBox.Location = new Point(0x5d, 0xc1);
            this.frmReceiverConfigTTBPortTxtBox.Name = "frmReceiverConfigTTBPortTxtBox";
            this.frmReceiverConfigTTBPortTxtBox.Size = new Size(0x51, 20);
            this.frmReceiverConfigTTBPortTxtBox.TabIndex = 3;
            this.frmReceiverConfigTTBPortTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigTTBPortTxtBox_TextChanged);
            this.frmReceiverConfigTTBPortLabel.AutoSize = true;
            this.frmReceiverConfigTTBPortLabel.Location = new Point(20, 0xc5);
            this.frmReceiverConfigTTBPortLabel.Name = "frmReceiverConfigTTBPortLabel";
            this.frmReceiverConfigTTBPortLabel.Size = new Size(0x35, 13);
            this.frmReceiverConfigTTBPortLabel.TabIndex = 2;
            this.frmReceiverConfigTTBPortLabel.Text = "TTB Port:";
            this.frmReceiverConfigPhyCommProtocolComboBox.FormattingEnabled = true;
            this.frmReceiverConfigPhyCommProtocolComboBox.Location = new Point(0x150, 0x48);
            this.frmReceiverConfigPhyCommProtocolComboBox.Name = "frmReceiverConfigPhyCommProtocolComboBox";
            this.frmReceiverConfigPhyCommProtocolComboBox.Size = new Size(0x6b, 0x15);
            this.frmReceiverConfigPhyCommProtocolComboBox.TabIndex = 50;
            this.frmReceiverConfigPhyCommProtocolComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigPhyCommProtocolComboBox_SelectedIndexChanged);
            this.frmReceiverConfigPhyCommProtocolLabel.AutoSize = true;
            this.frmReceiverConfigPhyCommProtocolLabel.Location = new Point(0xca, 0x4c);
            this.frmReceiverConfigPhyCommProtocolLabel.Name = "frmReceiverConfigPhyCommProtocolLabel";
            this.frmReceiverConfigPhyCommProtocolLabel.Size = new Size(0x5b, 13);
            this.frmReceiverConfigPhyCommProtocolLabel.TabIndex = 0x31;
            this.frmReceiverConfigPhyCommProtocolLabel.Text = "Connection Type:";
            this.frmReceiverConfigIPAddressLabel.AutoSize = true;
            this.frmReceiverConfigIPAddressLabel.Location = new Point(0xca, 0x76);
            this.frmReceiverConfigIPAddressLabel.Name = "frmReceiverConfigIPAddressLabel";
            this.frmReceiverConfigIPAddressLabel.Size = new Size(0x3d, 13);
            this.frmReceiverConfigIPAddressLabel.TabIndex = 0x33;
            this.frmReceiverConfigIPAddressLabel.Text = "IP Address:";
            this.frmReceiverConfigIPAddressTxtBox.Location = new Point(0x150, 0x72);
            this.frmReceiverConfigIPAddressTxtBox.Name = "frmReceiverConfigIPAddressTxtBox";
            this.frmReceiverConfigIPAddressTxtBox.Size = new Size(0x6b, 20);
            this.frmReceiverConfigIPAddressTxtBox.TabIndex = 0x34;
            this.frmReceiverConfigIPAddressTxtBox.Text = "127.0.0.1";
            this.frmReceiverConfigIPAddressTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigIPAddressTxtBox_TextChanged);
            this.frmReceiverConfigFamilyComboBox.FormattingEnabled = true;
            this.frmReceiverConfigFamilyComboBox.Location = new Point(0x5d, 0x48);
            this.frmReceiverConfigFamilyComboBox.Name = "frmReceiverConfigFamilyComboBox";
            this.frmReceiverConfigFamilyComboBox.Size = new Size(0x51, 0x15);
            this.frmReceiverConfigFamilyComboBox.TabIndex = 0x35;
            this.frmReceiverConfigFamilyComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigFamilyComboBox_SelectedIndexChanged);
            this.frmReceiverConfigFamilyLabel.AutoSize = true;
            this.frmReceiverConfigFamilyLabel.Location = new Point(20, 0x4c);
            this.frmReceiverConfigFamilyLabel.Name = "frmReceiverConfigFamilyLabel";
            this.frmReceiverConfigFamilyLabel.Size = new Size(0x27, 13);
            this.frmReceiverConfigFamilyLabel.TabIndex = 0x36;
            this.frmReceiverConfigFamilyLabel.Text = "Family:";
            this.frmReceiverConfigLDOModeComboBox.FormattingEnabled = true;
            this.frmReceiverConfigLDOModeComboBox.Items.AddRange(new object[] { "Disable", "Enable" });
            this.frmReceiverConfigLDOModeComboBox.Location = new Point(0x150, 0x101);
            this.frmReceiverConfigLDOModeComboBox.Name = "frmReceiverConfigLDOModeComboBox";
            this.frmReceiverConfigLDOModeComboBox.Size = new Size(0x6b, 0x15);
            this.frmReceiverConfigLDOModeComboBox.TabIndex = 0x39;
            this.frmReceiverConfigLDOModeComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigLDOModeComboBox_SelectedIndexChanged);
            this.frmReceiverConfigLDOModeLabel.AutoSize = true;
            this.frmReceiverConfigLDOModeLabel.Location = new Point(0xca, 0x105);
            this.frmReceiverConfigLDOModeLabel.Name = "frmReceiverConfigLDOModeLabel";
            this.frmReceiverConfigLDOModeLabel.Size = new Size(0x3e, 13);
            this.frmReceiverConfigLDOModeLabel.TabIndex = 0x3a;
            this.frmReceiverConfigLDOModeLabel.Text = "LDO Mode:";
            this.frmReceiverConfigLnaComboBox.FormattingEnabled = true;
            this.frmReceiverConfigLnaComboBox.Items.AddRange(new object[] { "Internal", "External" });
            this.frmReceiverConfigLnaComboBox.Location = new Point(0x150, 0xec);
            this.frmReceiverConfigLnaComboBox.Name = "frmReceiverConfigLnaComboBox";
            this.frmReceiverConfigLnaComboBox.Size = new Size(0x6b, 0x15);
            this.frmReceiverConfigLnaComboBox.TabIndex = 0x37;
            this.frmReceiverConfigLnaComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverConfigLnaComboBox_SelectedIndexChanged);
            this.frmReceiverConfigLnaTypeLabel.AutoSize = true;
            this.frmReceiverConfigLnaTypeLabel.Location = new Point(0xca, 240);
            this.frmReceiverConfigLnaTypeLabel.Name = "frmReceiverConfigLnaTypeLabel";
            this.frmReceiverConfigLnaTypeLabel.Size = new Size(0x3a, 13);
            this.frmReceiverConfigLnaTypeLabel.TabIndex = 0x38;
            this.frmReceiverConfigLnaTypeLabel.Text = "LNA Type:";
            this.frmReceiverConfigResetPortLabel.AutoSize = true;
            this.frmReceiverConfigResetPortLabel.Location = new Point(20, 0xb1);
            this.frmReceiverConfigResetPortLabel.Name = "frmReceiverConfigResetPortLabel";
            this.frmReceiverConfigResetPortLabel.Size = new Size(0x41, 13);
            this.frmReceiverConfigResetPortLabel.TabIndex = 30;
            this.frmReceiverConfigResetPortLabel.Text = "On/Off Port:";
            this.frmReceiverConfigResetPortTxtBox.Location = new Point(0x5d, 0xad);
            this.frmReceiverConfigResetPortTxtBox.Name = "frmReceiverConfigResetPortTxtBox";
            this.frmReceiverConfigResetPortTxtBox.Size = new Size(0x51, 20);
            this.frmReceiverConfigResetPortTxtBox.TabIndex = 0x1f;
            this.frmReceiverConfigResetPortTxtBox.TextChanged += new EventHandler(this.frmReceiverConfigResetPortTxtBox_TextChanged);
            this.frmReceiverPlatformCombox.FormattingEnabled = true;
            this.frmReceiverPlatformCombox.Location = new Point(0x5d, 0xea);
            this.frmReceiverPlatformCombox.Name = "frmReceiverPlatformCombox";
            this.frmReceiverPlatformCombox.Size = new Size(0x51, 0x15);
            this.frmReceiverPlatformCombox.TabIndex = 60;
            this.frmReceiverPlatformCombox.SelectedIndexChanged += new EventHandler(this.frmReceiverPlatformCombox_SelectedIndexChanged);
            this.platformLabel.AutoSize = true;
            this.platformLabel.Location = new Point(20, 0xee);
            this.platformLabel.Name = "platformLabel";
            this.platformLabel.Size = new Size(0x30, 13);
            this.platformLabel.TabIndex = 0x3b;
            this.platformLabel.Text = "Platform:";
            this.frmReceiverPackageLabel.AutoSize = true;
            this.frmReceiverPackageLabel.Location = new Point(20, 0x103);
            this.frmReceiverPackageLabel.Name = "frmReceiverPackageLabel";
            this.frmReceiverPackageLabel.Size = new Size(0x35, 13);
            this.frmReceiverPackageLabel.TabIndex = 0x3b;
            this.frmReceiverPackageLabel.Text = "Package:";
            this.frmReceiverPackageComboBox.FormattingEnabled = true;
            this.frmReceiverPackageComboBox.Location = new Point(0x5d, 0xff);
            this.frmReceiverPackageComboBox.Name = "frmReceiverPackageComboBox";
            this.frmReceiverPackageComboBox.Size = new Size(0x51, 0x15);
            this.frmReceiverPackageComboBox.TabIndex = 60;
            this.frmReceiverPackageComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverPackageComboBox_SelectedIndexChanged);
            this.frmReceiverRevLabel.AutoSize = true;
            this.frmReceiverRevLabel.Location = new Point(20, 280);
            this.frmReceiverRevLabel.Name = "frmReceiverRevLabel";
            this.frmReceiverRevLabel.Size = new Size(0x33, 13);
            this.frmReceiverRevLabel.TabIndex = 0x3b;
            this.frmReceiverRevLabel.Text = "Revision:";
            this.frmReceiverMfgLabel.AutoSize = true;
            this.frmReceiverMfgLabel.Location = new Point(20, 0x12d);
            this.frmReceiverMfgLabel.Name = "frmReceiverMfgLabel";
            this.frmReceiverMfgLabel.Size = new Size(0x21, 13);
            this.frmReceiverMfgLabel.TabIndex = 0x3b;
            this.frmReceiverMfgLabel.Text = "MFG:";
            this.frmReceiverMfgComboBox.FormattingEnabled = true;
            this.frmReceiverMfgComboBox.Location = new Point(0x5d, 0x128);
            this.frmReceiverMfgComboBox.Name = "frmReceiverMfgComboBox";
            this.frmReceiverMfgComboBox.Size = new Size(0x51, 0x15);
            this.frmReceiverMfgComboBox.TabIndex = 60;
            this.frmReceiverMfgComboBox.SelectedIndexChanged += new EventHandler(this.frmReceiverMfgComboBox_SelectedIndexChanged);
            this.refFreqSrcComboBox.FormattingEnabled = true;
            this.refFreqSrcComboBox.Location = new Point(0x150, 0xd7);
            this.refFreqSrcComboBox.Name = "refFreqSrcComboBox";
            this.refFreqSrcComboBox.Size = new Size(0x6b, 0x15);
            this.refFreqSrcComboBox.TabIndex = 0x3e;
            this.refFreqSrcComboBox.SelectedIndexChanged += new EventHandler(this.refFreqSrcComboBox_SelectedIndexChanged);
            this.refFreqSrcLabel.AutoSize = true;
            this.refFreqSrcLabel.Location = new Point(0xca, 0xdb);
            this.refFreqSrcLabel.Name = "refFreqSrcLabel";
            this.refFreqSrcLabel.Size = new Size(0x52, 13);
            this.refFreqSrcLabel.TabIndex = 0x3d;
            this.refFreqSrcLabel.Text = "TCXO Freq Src:";
            this.sirfNavStartStopTextBox.Location = new Point(0x150, 0x89);
            this.sirfNavStartStopTextBox.Name = "sirfNavStartStopTextBox";
            this.sirfNavStartStopTextBox.Size = new Size(0x6b, 20);
            this.sirfNavStartStopTextBox.TabIndex = 0x34;
            this.sirfNavStartStopTextBox.Text = "-1";
            this.sirfNavStartStopTextBox.TextChanged += new EventHandler(this.sirfNavStartStopTextBox_TextChanged);
            this.sirfNavStrLabel.AutoSize = true;
            this.sirfNavStrLabel.Location = new Point(0xca, 0x8d);
            this.sirfNavStrLabel.Name = "sirfNavStrLabel";
            this.sirfNavStrLabel.Size = new Size(0x7b, 13);
            this.sirfNavStrLabel.TabIndex = 0x33;
            this.sirfNavStrLabel.Text = "SiRFNav Start/Stop Intf:";
            this.temperatureUnitLabel.AutoSize = true;
            this.temperatureUnitLabel.Location = new Point(0x150, 300);
            this.temperatureUnitLabel.Name = "temperatureUnitLabel";
            this.temperatureUnitLabel.Size = new Size(0x1d, 13);
            this.temperatureUnitLabel.TabIndex = 0x3a;
            this.temperatureUnitLabel.Text = "Unit:";
            this.temperUnitComboBox.FormattingEnabled = true;
            this.temperUnitComboBox.Items.AddRange(new object[] { "C", "F" });
            this.temperUnitComboBox.Location = new Point(0x181, 0x128);
            this.temperUnitComboBox.Name = "temperUnitComboBox";
            this.temperUnitComboBox.Size = new Size(0x3a, 0x15);
            this.temperUnitComboBox.TabIndex = 0x39;
            this.temperUnitComboBox.SelectedIndexChanged += new EventHandler(this.temperUnitComboBox_SelectedIndexChanged);
            this.temperLabel.AutoSize = true;
            this.temperLabel.Location = new Point(0xca, 0x12d);
            this.temperLabel.Name = "temperLabel";
            this.temperLabel.Size = new Size(70, 13);
            this.temperLabel.TabIndex = 30;
            this.temperLabel.Text = "Temperature:";
            this.temperTextBox.Location = new Point(0x116, 0x129);
            this.temperTextBox.Name = "temperTextBox";
            this.temperTextBox.Size = new Size(0x2f, 20);
            this.temperTextBox.TabIndex = 0x1f;
            this.temperTextBox.TextChanged += new EventHandler(this.temperTextBox_TextChanged);
            this.frmReceiverRevTextBox.Location = new Point(0x5d, 0x114);
            this.frmReceiverRevTextBox.Name = "frmReceiverRevTextBox";
            this.frmReceiverRevTextBox.Size = new Size(0x51, 20);
            this.frmReceiverRevTextBox.TabIndex = 3;
            this.frmReceiverRevTextBox.TextChanged += new EventHandler(this.frmReceiverRevTextBox_TextChanged);
            this.rxVoltlabel.AutoSize = true;
            this.rxVoltlabel.Location = new Point(0xca, 0x11b);
            this.rxVoltlabel.Name = "rxVoltlabel";
            this.rxVoltlabel.Size = new Size(0x3b, 13);
            this.rxVoltlabel.TabIndex = 30;
            this.rxVoltlabel.Text = "Voltage(V):";
            this.rxVoltTextBox.Location = new Point(0x116, 0x114);
            this.rxVoltTextBox.Name = "rxVoltTextBox";
            this.rxVoltTextBox.Size = new Size(0x2f, 20);
            this.rxVoltTextBox.TabIndex = 0x1f;
            this.rxVoltTextBox.TextChanged += new EventHandler(this.rxVoltTextBox_TextChanged);
            base.AcceptButton = this.frmReceiverConfigDoneBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.frmReceiverConfigCancelBtn;
            base.ClientSize = new Size(0x293, 0x1e7);
            base.Controls.Add(this.refFreqSrcComboBox);
            base.Controls.Add(this.refFreqSrcLabel);
            base.Controls.Add(this.frmReceiverMfgComboBox);
            base.Controls.Add(this.frmReceiverMfgLabel);
            base.Controls.Add(this.frmReceiverRevLabel);
            base.Controls.Add(this.frmReceiverPackageComboBox);
            base.Controls.Add(this.frmReceiverPackageLabel);
            base.Controls.Add(this.frmReceiverPlatformCombox);
            base.Controls.Add(this.platformLabel);
            base.Controls.Add(this.temperUnitComboBox);
            base.Controls.Add(this.temperatureUnitLabel);
            base.Controls.Add(this.frmReceiverConfigLDOModeComboBox);
            base.Controls.Add(this.frmReceiverConfigLDOModeLabel);
            base.Controls.Add(this.frmReceiverConfigLnaComboBox);
            base.Controls.Add(this.frmReceiverConfigLnaTypeLabel);
            base.Controls.Add(this.frmReceiverConfigFamilyLabel);
            base.Controls.Add(this.frmReceiverConfigFamilyComboBox);
            base.Controls.Add(this.sirfNavStrLabel);
            base.Controls.Add(this.frmReceiverConfigIPAddressLabel);
            base.Controls.Add(this.sirfNavStartStopTextBox);
            base.Controls.Add(this.frmReceiverConfigIPAddressTxtBox);
            base.Controls.Add(this.frmReceiverConfigPhyCommProtocolComboBox);
            base.Controls.Add(this.frmReceiverConfigPhyCommProtocolLabel);
            base.Controls.Add(this.frmReceiverConfigAidingProtocolComboBox);
            base.Controls.Add(this.frmReceiverConfigTxLogTypeComboBox);
            base.Controls.Add(this.frmReceiverConfigRxLogTypeComboBox);
            base.Controls.Add(this.frmReceiverConfigFICDComboBox);
            base.Controls.Add(this.frmReceiverConfigAI3ICDComboBox);
            base.Controls.Add(this.frmReceiverConfigComboBox);
            base.Controls.Add(this.frmReceiverConfigMessageProtocolComboBox);
            base.Controls.Add(this.frmReceiverConfigExtClkFreqComboBox);
            base.Controls.Add(this.frmReceiverConfigDefaultClockComboBox);
            base.Controls.Add(this.frmReceiverConfigTypeComboBox);
            base.Controls.Add(label);
            base.Controls.Add(this.frmReceiverConfigHostDirBrowserBtn);
            base.Controls.Add(this.frmReceiverConfigHostDirTxtBox);
            base.Controls.Add(this.frmReceiverConfigAidingProtocolLabel);
            base.Controls.Add(this.frmReceiverConfigMessageProtocolLabel);
            base.Controls.Add(this.frmReceiverConfigNextBtn);
            base.Controls.Add(this.frmConfigReceiverBackBtn);
            base.Controls.Add(this.frmReceiverConfigRemoveBtn);
            base.Controls.Add(this.frmReceiverConfigAddBtn);
            base.Controls.Add(this.frmReceiverConfigCancelBtn);
            base.Controls.Add(this.frmReceiverConfigDoneBtn);
            base.Controls.Add(this.frmReceiverConfigRxIndexLabel);
            base.Controls.Add(this.frmReceiverConfigFICDLabel);
            base.Controls.Add(this.frmReceiverConfigAI3ICDLabel);
            base.Controls.Add(this.frmReceiverConfigExtClkFreqLabel);
            base.Controls.Add(this.frmReceiverConfigDefaultClockLabel);
            base.Controls.Add(this.frmReceiverConfigTypeLabel);
            base.Controls.Add(this.frmReceiverConfigTxLogTypeLabel);
            base.Controls.Add(this.frmReceiverConfigRxLogTypeLabel);
            base.Controls.Add(this.frmReceiverConfigPatchSWBrowseBtn);
            base.Controls.Add(this.frmReceiverConfigHostSWBrowseBtn);
            base.Controls.Add(this.frmReceiverConfigRequiredPatchChkBox);
            base.Controls.Add(this.frmReceiverConfigRequiredHostChkBox);
            base.Controls.Add(this.frmReceiverConfigHostArgsTxtBox);
            base.Controls.Add(this.frmReceiverConfigHostArgsLabel);
            base.Controls.Add(this.frmReceiverConfigPatchSWTxtBox);
            base.Controls.Add(this.frmReceiverConfigPatchSWLabel);
            base.Controls.Add(this.frmReceiverConfigHostSWTxtBox);
            base.Controls.Add(this.frmReceiverConfigHostSWLabel);
            base.Controls.Add(this.frmReceiverConfigHostPortTxtBox);
            base.Controls.Add(this.frmReceiverConfigHostPortLabel);
            base.Controls.Add(this.rxVoltTextBox);
            base.Controls.Add(this.rxVoltlabel);
            base.Controls.Add(this.temperTextBox);
            base.Controls.Add(this.temperLabel);
            base.Controls.Add(this.frmReceiverConfigResetPortTxtBox);
            base.Controls.Add(this.frmReceiverConfigResetPortLabel);
            base.Controls.Add(this.frmReceiverConfigTrackerPortTxtBox);
            base.Controls.Add(this.frmReceiverConfigTrackerPortLabel);
            base.Controls.Add(this.frmReceiverConfigTitle);
            base.Controls.Add(this.frmReceiverConfigNameLabel);
            base.Controls.Add(this.frmReceiverConfigDUTBaudLabel);
            base.Controls.Add(this.frmReceiverConfigTTBPortLabel);
            base.Controls.Add(this.frmReceiverConfigDUTPortLabel);
            base.Controls.Add(this.frmReceiverConfigNameTxtBox);
            base.Controls.Add(this.frmReceiverRevTextBox);
            base.Controls.Add(this.frmReceiverConfigTTBPortTxtBox);
            base.Controls.Add(this.frmReceiverConfigPortTxtBox);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmReceiverConfig";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Receiver Config";
            base.Load += new EventHandler(this.frmReceiverConfig_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private string list2String(List<string> inputList, string nameStr, string excludedStr)
        {
            string str2 = string.Empty;
            int num = 0;
            foreach (string str3 in inputList)
            {
                if (str3 == string.Empty)
                {
                    MessageBox.Show(string.Format("RX # {0} {1} blank - Use -1 for unused parameters", num + 1, nameStr), "ERROR!");
                    return "Error";
                }
                num++;
                if (str3 != excludedStr)
                {
                    str2 = str2 + str3 + ",";
                }
            }
            str2 = str2.TrimEnd(new char[] { ',' });
            if ((!(nameStr == "RX_TYPES") && !(nameStr == "RX_LOG_TYPES")) && !(nameStr == "TX_LOG_TYPES"))
            {
                return str2;
            }
            return str2.ToUpper();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void refFreqSrcComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.refFreqSrcComboBox.Text = this.refFreqSrcComboBox.SelectedItem.ToString();
            this.rxRefFreqSrc[this.currentIndex] = this.refFreqSrcComboBox.Text;
        }

        private void rxVoltTextBox_TextChanged(object sender, EventArgs e)
        {
            this.rxVoltage[this.currentIndex] = this.rxVoltTextBox.Text;
        }

        private void sirfNavStartStopTextBox_TextChanged(object sender, EventArgs e)
        {
            this.rxSirfNavStartStopIntfStr[this.currentIndex] = this.sirfNavStartStopTextBox.Text;
        }

        private void temperTextBox_TextChanged(object sender, EventArgs e)
        {
            this.rxTemperature[this.currentIndex] = this.temperTextBox.Text;
        }

        private void temperUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.temperUnitComboBox.Text = this.temperUnitComboBox.SelectedItem.ToString();
            this.rxTemperatureUnit[this.currentIndex] = this.temperUnitComboBox.Text;
        }

        internal void updateConfigList(string updatedData)
        {
            switch (this.inputForm.UpdateType)
            {
                case "RX_TYPES":
                    this.rxTypes[this.currentIndex] = updatedData;
                    this.frmReceiverConfigTypeComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigTypeComboBox.Text = updatedData;
                    return;

                case "DEFAULT_CLK":
                    try
                    {
                        Convert.ToInt32(updatedData);
                        this.rxDefaultClocks[this.currentIndex] = updatedData;
                        this.frmReceiverConfigDefaultClockComboBox.Items.Add(updatedData);
                        this.frmReceiverConfigDefaultClockComboBox.Text = updatedData;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("DEFAULT_CLK: " + exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    break;

                case "EXT_CLK_FREQ":
                    try
                    {
                        Convert.ToInt32(updatedData);
                        this.rxExtClkFreqs[this.currentIndex] = updatedData;
                        this.frmReceiverConfigExtClkFreqComboBox.Items.Add(updatedData);
                        this.frmReceiverConfigExtClkFreqComboBox.Text = updatedData;
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show("EXT_CLK_FREQ: " + exception2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    break;

                case "MESSAGE_PROTOCOL":
                    this.rxMessageProtocols[this.currentIndex] = updatedData;
                    this.frmReceiverConfigMessageProtocolComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigMessageProtocolComboBox.Text = updatedData;
                    return;

                case "RX_BAUDS":
                    try
                    {
                        Convert.ToInt32(updatedData);
                        this.rxBauds[this.currentIndex] = updatedData;
                        this.frmReceiverConfigComboBox.Items.Add(updatedData);
                        this.frmReceiverConfigComboBox.Text = updatedData;
                    }
                    catch (Exception exception3)
                    {
                        MessageBox.Show("RX_BAUDS: " + exception3.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    break;

                case "AI3_ICD":
                    this.rxAI3ICDs[this.currentIndex] = updatedData;
                    this.frmReceiverConfigAI3ICDComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigAI3ICDComboBox.Text = updatedData;
                    return;

                case "F_ICD":
                    this.rxFICDs[this.currentIndex] = updatedData;
                    this.frmReceiverConfigFICDComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigFICDComboBox.Text = updatedData;
                    return;

                case "RX_LOG_TYPES":
                    this.rxRxLogTypes[this.currentIndex] = updatedData;
                    this.frmReceiverConfigRxLogTypeComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigRxLogTypeComboBox.Text = updatedData;
                    return;

                case "TX_LOG_TYPES":
                    this.rxTxLogTypes[this.currentIndex] = updatedData;
                    this.frmReceiverConfigTxLogTypeComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigTxLogTypeComboBox.Text = updatedData;
                    return;

                case "AIDING_PROTOCOL":
                    this.rxAidingProtocols[this.currentIndex] = updatedData;
                    this.frmReceiverConfigAidingProtocolComboBox.Items.Add(updatedData);
                    this.frmReceiverConfigAidingProtocolComboBox.Text = updatedData;
                    break;

                default:
                    return;
            }
        }

        private void updateformFields(int idx)
        {
            try
            {
                this.frmReceiverConfigRxIndexLabel.Text = "DUT #: " + ((idx + 1)).ToString();
                if (this.frmReceiverConfigTypeComboBox.Items.Contains(this.rxTypes[idx]))
                {
                    this.frmReceiverConfigTypeComboBox.Text = this.rxTypes[idx];
                }
                else
                {
                    this.frmReceiverConfigTypeComboBox.Items.Add(this.rxTypes[idx]);
                    this.frmReceiverConfigTypeComboBox.Text = this.rxTypes[idx];
                }
                if (this.frmReceiverConfigFamilyComboBox.Items.Contains(this.rxFamily[idx]))
                {
                    this.frmReceiverConfigFamilyComboBox.Text = this.rxFamily[idx];
                }
                else
                {
                    this.frmReceiverConfigFamilyComboBox.Items.Add(this.rxFamily[idx]);
                    this.frmReceiverConfigFamilyComboBox.Text = this.rxFamily[idx];
                }
                this.frmReceiverConfigLnaComboBox.SelectedIndex = Convert.ToInt32(this.rxLnaType[idx]);
                this.frmReceiverConfigLnaComboBox.Text = this.frmReceiverConfigLnaComboBox.Items[this.frmReceiverConfigLnaComboBox.SelectedIndex].ToString();
                this.frmReceiverConfigLDOModeComboBox.SelectedIndex = Convert.ToInt32(this.rxLdoMode[idx]);
                this.frmReceiverConfigLDOModeComboBox.Text = this.frmReceiverConfigLDOModeComboBox.Items[this.frmReceiverConfigLDOModeComboBox.SelectedIndex].ToString();
                if (this.frmReceiverConfigDefaultClockComboBox.Items.Contains(this.rxDefaultClocks[idx]))
                {
                    this.frmReceiverConfigDefaultClockComboBox.Text = this.rxDefaultClocks[idx];
                }
                else
                {
                    this.frmReceiverConfigDefaultClockComboBox.Items.Add(this.rxDefaultClocks[idx]);
                    this.frmReceiverConfigDefaultClockComboBox.Text = this.rxDefaultClocks[idx];
                }
                if (this.frmReceiverConfigExtClkFreqComboBox.Items.Contains(this.rxExtClkFreqs[idx]))
                {
                    this.frmReceiverConfigExtClkFreqComboBox.Text = this.rxExtClkFreqs[idx];
                }
                else
                {
                    this.frmReceiverConfigExtClkFreqComboBox.Items.Add(this.rxExtClkFreqs[idx]);
                    this.frmReceiverConfigExtClkFreqComboBox.Text = this.rxExtClkFreqs[idx];
                }
                if (this.frmReceiverConfigComboBox.Items.Contains(this.rxBauds[idx]))
                {
                    this.frmReceiverConfigComboBox.Text = this.rxBauds[idx];
                }
                else
                {
                    this.frmReceiverConfigComboBox.Items.Add(this.rxBauds[idx]);
                    this.frmReceiverConfigComboBox.Text = this.rxBauds[idx];
                }
                if (this.frmReceiverConfigAI3ICDComboBox.Items.Contains(this.rxAI3ICDs[idx]))
                {
                    this.frmReceiverConfigAI3ICDComboBox.Text = this.rxAI3ICDs[idx];
                }
                else
                {
                    this.frmReceiverConfigAI3ICDComboBox.Items.Add(this.rxAI3ICDs[idx]);
                    this.frmReceiverConfigAI3ICDComboBox.Text = this.rxAI3ICDs[idx];
                }
                if (this.frmReceiverConfigFICDComboBox.Items.Contains(this.rxFICDs[idx]))
                {
                    this.frmReceiverConfigFICDComboBox.Text = this.rxFICDs[idx];
                }
                else
                {
                    this.frmReceiverConfigFICDComboBox.Items.Add(this.rxFICDs[idx]);
                    this.frmReceiverConfigFICDComboBox.Text = this.rxFICDs[idx];
                }
                if (this.frmReceiverConfigRxLogTypeComboBox.Items.Contains(this.rxRxLogTypes[idx]))
                {
                    this.frmReceiverConfigRxLogTypeComboBox.Text = this.rxRxLogTypes[idx];
                }
                else
                {
                    this.frmReceiverConfigRxLogTypeComboBox.Items.Add(this.rxRxLogTypes[idx]);
                    this.frmReceiverConfigRxLogTypeComboBox.Text = this.rxRxLogTypes[idx];
                }
                this.frmReceiverConfigNameTxtBox.Text = this.rxNames[idx];
                this.frmReceiverConfigPortTxtBox.Text = this.rxPorts[idx];
                this.frmReceiverConfigTTBPortTxtBox.Text = this.rxTTBPorts[idx];
                if (this.frmReceiverConfigTxLogTypeComboBox.Items.Contains(this.rxTxLogTypes[idx]))
                {
                    this.frmReceiverConfigTxLogTypeComboBox.Text = this.rxTxLogTypes[idx];
                }
                else
                {
                    this.frmReceiverConfigTxLogTypeComboBox.Items.Add(this.rxTxLogTypes[idx]);
                    this.frmReceiverConfigTxLogTypeComboBox.Text = this.rxTxLogTypes[idx];
                }
                if (this.frmReceiverConfigAidingProtocolComboBox.Items.Contains(this.rxAidingProtocols[idx]))
                {
                    this.frmReceiverConfigAidingProtocolComboBox.Text = this.rxAidingProtocols[idx];
                }
                else
                {
                    this.frmReceiverConfigAidingProtocolComboBox.Items.Add(this.rxAidingProtocols[idx]);
                    this.frmReceiverConfigAidingProtocolComboBox.Text = this.rxAidingProtocols[idx];
                }
                if (this.frmReceiverConfigMessageProtocolComboBox.Items.Contains(this.rxMessageProtocols[idx]))
                {
                    this.frmReceiverConfigMessageProtocolComboBox.Text = this.rxMessageProtocols[idx];
                }
                else
                {
                    this.frmReceiverConfigMessageProtocolComboBox.Items.Add(this.rxMessageProtocols[idx]);
                    this.frmReceiverConfigMessageProtocolComboBox.Text = this.rxMessageProtocols[idx];
                }
                this.frmReceiverConfigIPAddressTxtBox.Text = this.rxIPAddress[idx];
                this.frmReceiverConfigPhyCommProtocolComboBox.Text = this.rxPhyCommProtocols[idx];
                this.frmReceiverConfigHostDirTxtBox.Text = this.hostNPatchSWPath;
                this.frmReceiverConfigTrackerPortTxtBox.Text = this.rxTrackerPorts[idx];
                this.frmReceiverConfigResetPortTxtBox.Text = this.rxResetPorts[idx];
                this.sirfNavStartStopTextBox.Text = this.rxSirfNavStartStopIntfStr[idx];
                this.frmReceiverRevTextBox.Text = this.rxRev[idx];
                this.temperTextBox.Text = this.rxTemperature[idx];
                this.temperUnitComboBox.Text = this.rxTemperatureUnit[idx];
                this.rxVoltTextBox.Text = this.rxVoltage[idx];
                if (this.frmReceiverPlatformCombox.Items.Contains(this.rxPlatform[idx]))
                {
                    this.frmReceiverPlatformCombox.Text = this.rxPlatform[idx];
                }
                else
                {
                    this.frmReceiverPlatformCombox.Items.Add(this.rxPlatform[idx]);
                    this.frmReceiverPlatformCombox.Text = this.rxPlatform[idx];
                }
                if (this.frmReceiverPackageComboBox.Items.Contains(this.rxPackage[idx]))
                {
                    this.frmReceiverPackageComboBox.Text = this.rxPackage[idx];
                }
                else
                {
                    this.frmReceiverPackageComboBox.Items.Add(this.rxPackage[idx]);
                    this.frmReceiverPackageComboBox.Text = this.rxPackage[idx];
                }
                if (this.frmReceiverMfgComboBox.Items.Contains(this.rxMfg[idx]))
                {
                    this.frmReceiverMfgComboBox.Text = this.rxMfg[idx];
                }
                else
                {
                    this.frmReceiverMfgComboBox.Items.Add(this.rxMfg[idx]);
                    this.frmReceiverMfgComboBox.Text = this.rxMfg[idx];
                }
                if (this.refFreqSrcComboBox.Items.Contains(this.rxRefFreqSrc[idx]))
                {
                    this.refFreqSrcComboBox.Text = this.rxRefFreqSrc[idx];
                }
                else
                {
                    this.refFreqSrcComboBox.Items.Add(this.rxRefFreqSrc[idx]);
                    this.refFreqSrcComboBox.Text = this.rxRefFreqSrc[idx];
                }
                if (Convert.ToInt32(this.rxRequiredHosts[idx]) == 1)
                {
                    this.frmReceiverConfigHostSWTxtBox.Enabled = true;
                    this.frmReceiverConfigHostPortTxtBox.Enabled = true;
                    this.frmReceiverConfigHostArgsTxtBox.Enabled = true;
                    this.frmReceiverConfigTrackerPortTxtBox.Enabled = true;
                    this.frmReceiverConfigHostSWBrowseBtn.Enabled = true;
                    this.frmReceiverConfigHostDirTxtBox.Enabled = true;
                    this.frmReceiverConfigHostDirBrowserBtn.Enabled = true;
                    this.frmReceiverConfigHostSWTxtBox.Text = this.rxHostSWs[idx];
                    if ((this.rxPhyCommProtocols[idx] == "UART") && (this.rxHostPorts[idx] == "-1"))
                    {
                        this.rxHostPorts[idx] = "";
                    }
                    this.frmReceiverConfigHostPortTxtBox.Text = this.rxHostPorts[idx];
                    this.frmReceiverConfigHostArgsTxtBox.Text = this.rxHostSWArgs[idx];
                    this.frmReceiverConfigRequiredHostChkBox.Checked = true;
                }
                else
                {
                    this.frmReceiverConfigHostSWTxtBox.Enabled = false;
                    this.frmReceiverConfigHostPortTxtBox.Enabled = false;
                    this.frmReceiverConfigHostArgsTxtBox.Enabled = false;
                    this.frmReceiverConfigResetPortTxtBox.Enabled = true;
                    this.frmReceiverConfigHostSWBrowseBtn.Enabled = false;
                    this.frmReceiverConfigHostDirTxtBox.Enabled = false;
                    this.frmReceiverConfigHostDirBrowserBtn.Enabled = false;
                    this.frmReceiverConfigRequiredHostChkBox.Checked = false;
                    this.rxHostPorts[idx] = "-1";
                    this.frmReceiverConfigHostPortTxtBox.Text = this.rxHostPorts[idx];
                }
                if (Convert.ToInt32(this.rxRequiredPatches[idx]) == 1)
                {
                    this.frmReceiverConfigPatchSWTxtBox.Text = this.rxPatchSWs[idx];
                    this.frmReceiverConfigPatchSWTxtBox.Enabled = true;
                    this.frmReceiverConfigPatchSWBrowseBtn.Enabled = true;
                    this.frmReceiverConfigRequiredPatchChkBox.Checked = true;
                }
                else
                {
                    this.frmReceiverConfigPatchSWTxtBox.Enabled = false;
                    this.frmReceiverConfigPatchSWBrowseBtn.Enabled = false;
                    this.frmReceiverConfigRequiredPatchChkBox.Checked = false;
                }
                this.updateParamsPerFamily();
            }
            catch (Exception exception)
            {
                this.currentIndex--;
                if (this.currentIndex < 0)
                {
                    this.currentIndex = 0;
                }
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void updateParamsPerFamily()
        {
            if ((this.frmReceiverConfigFamilyComboBox.Text == "GSD4e") || (this.frmReceiverConfigFamilyComboBox.Text == "GSD4t"))
            {
                this.frmReceiverConfigTTBPortTxtBox.Enabled = true;
                this.frmReceiverConfigRequiredPatchChkBox.Checked = false;
                this.frmReceiverConfigRequiredPatchChkBox.Enabled = false;
                this.frmReceiverConfigAidingProtocolComboBox.Text = "OSP";
                this.frmReceiverConfigAidingProtocolComboBox.Enabled = false;
                this.frmReceiverConfigAI3ICDComboBox.Text = "1.0";
                this.frmReceiverConfigAI3ICDComboBox.Enabled = false;
                this.frmReceiverConfigFICDComboBox.Text = "1.0";
                this.frmReceiverConfigFICDComboBox.Enabled = false;
                this.frmReceiverConfigHostDirTxtBox.Enabled = false;
                if (this.frmReceiverConfigFamilyComboBox.Text == "GSD4e")
                {
                    this.frmReceiverConfigPhyCommProtocolComboBox.Text = "UART";
                    this.frmReceiverConfigIPAddressTxtBox.Enabled = false;
                    this.frmReceiverConfigLnaComboBox.Enabled = false;
                    this.frmReceiverConfigLDOModeComboBox.Enabled = false;
                    this.frmReceiverConfigRequiredHostChkBox.Checked = false;
                    this.frmReceiverConfigRequiredHostChkBox.Enabled = false;
                    this.frmReceiverConfigHostDirTxtBox.Enabled = false;
                    this.frmReceiverConfigHostDirBrowserBtn.Enabled = false;
                    this.frmReceiverConfigHostSWTxtBox.Enabled = false;
                    this.frmReceiverConfigHostSWBrowseBtn.Enabled = false;
                    this.frmReceiverConfigTrackerPortTxtBox.Enabled = false;
                    this.frmReceiverConfigTrackerPortTxtBox.Text = "-1";
                    this.frmReceiverConfigHostPortTxtBox.Text = "-1";
                }
                else
                {
                    this.frmReceiverConfigPhyCommProtocolComboBox.Enabled = true;
                    this.frmReceiverConfigIPAddressTxtBox.Enabled = true;
                    this.frmReceiverConfigLnaComboBox.Enabled = true;
                    this.frmReceiverConfigLDOModeComboBox.Enabled = true;
                    this.frmReceiverConfigRequiredHostChkBox.Enabled = true;
                    this.frmReceiverConfigTrackerPortTxtBox.Enabled = true;
                    this.frmReceiverConfigResetPortTxtBox.Enabled = true;
                    if (this.frmReceiverConfigRequiredHostChkBox.Checked)
                    {
                        this.frmReceiverConfigHostDirTxtBox.Enabled = true;
                        this.frmReceiverConfigHostDirBrowserBtn.Enabled = true;
                        this.frmReceiverConfigHostSWTxtBox.Enabled = true;
                        this.frmReceiverConfigHostSWBrowseBtn.Enabled = true;
                    }
                    else
                    {
                        this.frmReceiverConfigHostDirTxtBox.Enabled = false;
                        this.frmReceiverConfigHostDirBrowserBtn.Enabled = false;
                        this.frmReceiverConfigHostSWTxtBox.Enabled = false;
                        this.frmReceiverConfigHostSWBrowseBtn.Enabled = false;
                    }
                }
            }
            else
            {
                this.frmReceiverConfigRequiredPatchChkBox.Enabled = true;
                this.frmReceiverConfigAidingProtocolComboBox.Enabled = true;
                this.frmReceiverConfigAI3ICDComboBox.Enabled = true;
                this.frmReceiverConfigFICDComboBox.Enabled = true;
                this.frmReceiverConfigPhyCommProtocolComboBox.Enabled = true;
                this.frmReceiverConfigIPAddressTxtBox.Enabled = true;
                this.frmReceiverConfigLnaComboBox.Enabled = true;
                this.frmReceiverConfigLDOModeComboBox.Enabled = true;
                this.frmReceiverConfigRequiredHostChkBox.Enabled = true;
                this.frmReceiverConfigHostDirTxtBox.Enabled = true;
                this.frmReceiverConfigHostDirBrowserBtn.Enabled = true;
            }
        }

        internal delegate void updateParentEventHandler(Hashtable updatedHash);
    }
}

