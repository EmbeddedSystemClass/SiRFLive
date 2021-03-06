﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using SiRFLive.Reporting;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frmRXInit_cmd : Form
    {
        private int _defaultWidth;
        private int _maxItemWidth;
        private Button button_Cancel;
        private Button button_Send;
        private CheckBox checkBox_enable_Devlopment_Data;
        private CheckBox checkBox_enable_Navlib_Data;
        private CheckBox checkBox_enableFullSystemReset;
        private CheckBox checkBox_performedAdingOnFactory;
        private CheckBox checkBox_slcMode;
        private CheckBox checkBox_switchProtocolOnFactory;
        private CheckBox checkBox_usePCTime;
        private CommunicationManager comm;
        private IContainer components;
        internal ObjectInterface cpGuiCtrl = new ObjectInterface();
        private Button frmRxInitCmdConfigAutoReplyBtn;
        private Button frmRxInitCmdCurrentFixPositionBtn;
        private Label frmRxInitCmdLongitudeLabel;
        private Label frmRxInitCmdRefAltitudeLabel;
        private TextBox frmRxInitCmdRefAltitudeTxtBox;
        private Label frmRxInitCmdRefLatitudeLabel;
        private TextBox frmRxInitCmdRefLatitudeTxtBox;
        private ComboBox frmRxInitCmdRefLocationComboBox;
        private GroupBox frmRxInitCmdRefLocationGrpBox;
        private TextBox frmRxInitCmdRefLongitudeTxtBox;
        private Button frmRxInitCmdSetAsDefaultBtn;
        private CheckBox frmRxInitCmdValidateLocationChkBox;
        private GroupBox frmRxInitCmdWarmInitParamsGrpBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button initWarmInitDataBtn;
        private frmCommonSimpleInput inputForm = new frmCommonSimpleInput("Enter Location Name:");
        private Label label_x;
        private Label label_y;
        private Label label_z;
        private Label label10;
        private Label label11;
        private Label label7;
        private Label label8;
        private Label label9;
        private static frmRXInit_cmd m_SChildform;
        internal string outputStr = string.Empty;
        private RadioButton radioBtn_factoryKeepFlash;
        private RadioButton radioButton_ColdStart;
        private RadioButton radioButton_Factory_Reset;
        private RadioButton radioButton_Factory_Reset_XO;
        private RadioButton radioButton_HotStart;
        private RadioButton radioButton_WarmStart_Init;
        private RadioButton radioButton_WarmStart_noInit;
        private SSB_Format SSBBitMapConstruct = new SSB_Format();
        private TextBox textBox_Channels;
        private TextBox textBox_ClockDrift;
        private TextBox textBox_TimeOfWeek;
        private TextBox textBox_TrueHeading;
        private TextBox textBox_WeekNum;
        private TextBox textBox_X;
        private TextBox textBox_Y;
        private TextBox textBox_Z;
        private TextBox textBox4;
        private Label trueHeadingLabel;

        public frmRXInit_cmd()
        {
            this.InitializeComponent();
            this.inputForm.updateParent += new frmCommonSimpleInput.updateParentEventHandler(this.updateConfigList);
            this.inputForm.MdiParent = base.MdiParent;
            this._defaultWidth = this.frmRxInitCmdRefLocationComboBox.Width;
            this.radioButton_HotStart.Checked = true;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            base.Close();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        this.sendReset(ref manager.comm);
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else if (this.comm.IsSourceDeviceOpen())
            {
                this.sendReset(ref this.comm);
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void checkBox_usePCTime_CheckedChanged(object sender, EventArgs e)
        {
            this.updatewhenUsePCTimeChange();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmRXInit_cmd_Load(object sender, EventArgs e)
        {
            this.updateControlsForFactoryReset();
            this.updateControlsForWarmInit();
            this.updateDefautlReferenceLocationComboBox();
            this.radioButton_Factory_Reset_XO.Visible = true;
            this.radioBtn_factoryKeepFlash.Visible = true;
            this.frmRxInitCmdValidateLocationChkBox.Checked = true;
            this.checkBox_enable_Devlopment_Data.Checked = true;
            this.checkBox_enable_Navlib_Data.Checked = true;
            this.checkBox_enable_Navlib_Data.Visible = true;
            this.frmRxInitCmdCurrentFixPositionBtn.Enabled = false;
            this.checkBox_enableFullSystemReset.Checked = false;
            this.checkBox_enableFullSystemReset.Enabled = false;
            this.checkBox_enableFullSystemReset.Visible = false;
            if (clsGlobal.IsMarketingUser())
            {
                this.checkBox_slcMode.Visible = false;
            }
            if (this.comm.MessageProtocol == "NMEA")
            {
                this.checkBox_slcMode.Visible = false;
                this.radioButton_Factory_Reset_XO.Visible = false;
                this.radioBtn_factoryKeepFlash.Visible = false;
                this.checkBox_enable_Navlib_Data.Enabled = false;
                this.checkBox_enable_Navlib_Data.Checked = false;
                this.checkBox_enable_Navlib_Data.Visible = false;
                this.checkBox_switchProtocolOnFactory.Visible = false;
                this.checkBox_performedAdingOnFactory.Visible = false;
            }
            this.checkBox_usePCTime.Checked = true;
            this.textBox_WeekNum.Enabled = false;
            this.textBox_TimeOfWeek.Enabled = false;
        }

        private void frmRxInitCmdConfigAutoReplyBtn_Click(object sender, EventArgs e)
        {
            string path = clsGlobal.InstalledDirectory + @"\scripts\SiRFLiveAutomationSetupAutoReply.cfg";
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Configuration file not found!\n {0}", path), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                frmAutoReply reply = new frmAutoReply();
                reply.AutoReplyConfigFilePath = path;
                reply.CommWindow = this.comm;
                reply.CommWindow.ReadAutoReplyData(path);
                if (reply.ShowDialog() == DialogResult.Cancel)
                {
                }
            }
        }

        private void frmRxInitCmdCurrentFixPositionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int count = this.comm.dataGui.Positions.PositionList.Count;
                if (this.comm.dataGui.Positions.PositionList.Count > 0)
                {
                    PositionInfo.PositionStruct struct2 = (PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1];
                    if (((byte) (struct2.NavType & 7)) == 0)
                    {
                        MessageBox.Show("No fixed position yet -- Not set", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        this.frmRxInitCmdRefLatitudeTxtBox.Text = struct2.Latitude.ToString();
                        this.frmRxInitCmdRefLongitudeTxtBox.Text = struct2.Longitude.ToString();
                        this.frmRxInitCmdRefAltitudeTxtBox.Text = struct2.Altitude.ToString();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error updating location with current fix position", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void frmRxInitCmdRefLocationComboBox_DropDown(object sender, EventArgs e)
        {
            this.frmRxInitCmdRefLocationComboBox.Width = this._maxItemWidth;
        }

        private void frmRxInitCmdRefLocationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.frmRxInitCmdRefLocationComboBox.SelectedItem.ToString() != "USER_DEFINED")
            {
                this.comm.RxCtrl.RxNavData.RefLocationName = this.frmRxInitCmdRefLocationComboBox.SelectedItem.ToString();
                this.updateReferenceLocationComboBox();
            }
            else
            {
                this.inputForm.UpdateType = "UPDATE_REF_NAME";
                this.inputForm.ShowDialog();
            }
        }

        private void frmRxInitCmdSetAsDefaultBtn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    this.setAsDefault(ref manager.comm);
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.setAsDefault(ref this.comm);
            }
            this.Cursor = Cursors.Default;
        }

        private void frmRxInitCmdValidateLocationChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                try
                {
                    this.comm.m_NavData.ValidatePosition = this.frmRxInitCmdValidateLocationChkBox.Checked;
                }
                catch
                {
                }
            }
        }

        public static frmRXInit_cmd GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRXInit_cmd();
            }
            return m_SChildform;
        }

        private void GetGpsWkTow(ref ushort weeknum, ref uint tow, int UtcOffset)
        {
            int[] numArray = new int[] { 0, 0x1f, 0x3b, 90, 120, 0x97, 0xb5, 0xd4, 0xf3, 0x111, 0x130, 0x14e, 0x16d };
            int[] numArray2 = new int[] { 0, 0x1f, 60, 0x5b, 0x79, 0x98, 0xb6, 0xd5, 0xf4, 0x112, 0x131, 0x14f, 0x16e };
            DateTime now = DateTime.Now;
            if (UtcOffset > 0x3b)
            {
                MessageBox.Show("UTC offset > 59 seconds.");
                weeknum = 0;
                tow = 0;
            }
            else
            {
                now = DateTime.Now.ToUniversalTime();
                int num = 0x16d * (now.Year - 0x7bc);
                num -= 6;
                num++;
                for (int i = 0x7c0; i < now.Year; i += 4)
                {
                    if (this.IsLeapYear(i))
                    {
                        num++;
                    }
                }
                if ((now.Month > 2) && this.IsLeapYear(now.Year))
                {
                    num += numArray2[now.Month - 1] + now.Day;
                }
                else
                {
                    num += numArray[now.Month - 1] + now.Day;
                }
                weeknum = (ushort) (num / 7);
                tow = (uint) ((((86400 * (uint)now.DayOfWeek) + (3600 * now.Hour)) + (60 * now.Minute)) + (now.Second + UtcOffset));
                if (tow > 0x93a7f)
                {
                    tow -= 0x93a80;
                    weeknum = (ushort) (weeknum + 1);
                }
                tow += (uint)(now.Millisecond / 10);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmRXInit_cmd));
            this.groupBox1 = new GroupBox();
            this.radioBtn_factoryKeepFlash = new RadioButton();
            this.radioButton_Factory_Reset_XO = new RadioButton();
            this.radioButton_Factory_Reset = new RadioButton();
            this.radioButton_ColdStart = new RadioButton();
            this.radioButton_WarmStart_Init = new RadioButton();
            this.radioButton_WarmStart_noInit = new RadioButton();
            this.radioButton_HotStart = new RadioButton();
            this.checkBox_slcMode = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.checkBox_performedAdingOnFactory = new CheckBox();
            this.checkBox_switchProtocolOnFactory = new CheckBox();
            this.checkBox_enable_Devlopment_Data = new CheckBox();
            this.checkBox_enable_Navlib_Data = new CheckBox();
            this.checkBox_enableFullSystemReset = new CheckBox();
            this.button_Send = new Button();
            this.button_Cancel = new Button();
            this.textBox_X = new TextBox();
            this.label_x = new Label();
            this.label_y = new Label();
            this.textBox_Y = new TextBox();
            this.label_z = new Label();
            this.textBox_Z = new TextBox();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.textBox_WeekNum = new TextBox();
            this.textBox_TimeOfWeek = new TextBox();
            this.textBox_ClockDrift = new TextBox();
            this.textBox4 = new TextBox();
            this.label10 = new Label();
            this.label11 = new Label();
            this.textBox_Channels = new TextBox();
            this.frmRxInitCmdWarmInitParamsGrpBox = new GroupBox();
            this.checkBox_usePCTime = new CheckBox();
            this.initWarmInitDataBtn = new Button();
            this.textBox_TrueHeading = new TextBox();
            this.trueHeadingLabel = new Label();
            this.frmRxInitCmdRefLocationGrpBox = new GroupBox();
            this.frmRxInitCmdCurrentFixPositionBtn = new Button();
            this.frmRxInitCmdSetAsDefaultBtn = new Button();
            this.frmRxInitCmdValidateLocationChkBox = new CheckBox();
            this.frmRxInitCmdRefLatitudeTxtBox = new TextBox();
            this.frmRxInitCmdRefLatitudeLabel = new Label();
            this.frmRxInitCmdRefLongitudeTxtBox = new TextBox();
            this.frmRxInitCmdLongitudeLabel = new Label();
            this.frmRxInitCmdRefAltitudeTxtBox = new TextBox();
            this.frmRxInitCmdRefAltitudeLabel = new Label();
            this.frmRxInitCmdRefLocationComboBox = new ComboBox();
            this.frmRxInitCmdConfigAutoReplyBtn = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.frmRxInitCmdWarmInitParamsGrpBox.SuspendLayout();
            this.frmRxInitCmdRefLocationGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.radioBtn_factoryKeepFlash);
            this.groupBox1.Controls.Add(this.radioButton_Factory_Reset_XO);
            this.groupBox1.Controls.Add(this.radioButton_Factory_Reset);
            this.groupBox1.Controls.Add(this.radioButton_ColdStart);
            this.groupBox1.Controls.Add(this.radioButton_WarmStart_Init);
            this.groupBox1.Controls.Add(this.radioButton_WarmStart_noInit);
            this.groupBox1.Controls.Add(this.radioButton_HotStart);
            this.groupBox1.Controls.Add(this.checkBox_slcMode);
            this.groupBox1.Location = new Point(0x1b, 0x15f);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd7, 0x9f);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reset Mode";
            this.radioBtn_factoryKeepFlash.AutoSize = true;
            this.radioBtn_factoryKeepFlash.Location = new Point(8, 0x83);
            this.radioBtn_factoryKeepFlash.Margin = new Padding(3, 3, 0, 3);
            this.radioBtn_factoryKeepFlash.Name = "radioBtn_factoryKeepFlash";
            this.radioBtn_factoryKeepFlash.Size = new Size(0xbb, 0x11);
            this.radioBtn_factoryKeepFlash.TabIndex = 6;
            this.radioBtn_factoryKeepFlash.TabStop = true;
            this.radioBtn_factoryKeepFlash.Text = "Factory (Keep Flash/Eeprom data)";
            this.radioBtn_factoryKeepFlash.UseVisualStyleBackColor = true;
            this.radioBtn_factoryKeepFlash.CheckedChanged += new EventHandler(this.radioButton_Factory_Reset_XO_CheckedChanged);
            this.radioButton_Factory_Reset_XO.AutoSize = true;
            this.radioButton_Factory_Reset_XO.Location = new Point(8, 0x70);
            this.radioButton_Factory_Reset_XO.Margin = new Padding(3, 3, 0, 3);
            this.radioButton_Factory_Reset_XO.Name = "radioButton_Factory_Reset_XO";
            this.radioButton_Factory_Reset_XO.Size = new Size(0x97, 0x11);
            this.radioButton_Factory_Reset_XO.TabIndex = 5;
            this.radioButton_Factory_Reset_XO.TabStop = true;
            this.radioButton_Factory_Reset_XO.Text = "Factory (Clear &XO learning)";
            this.radioButton_Factory_Reset_XO.UseVisualStyleBackColor = true;
            this.radioButton_Factory_Reset_XO.CheckedChanged += new EventHandler(this.radioButton_Factory_Reset_XO_CheckedChanged);
            this.radioButton_Factory_Reset.AutoSize = true;
            this.radioButton_Factory_Reset.Location = new Point(8, 0x5d);
            this.radioButton_Factory_Reset.Name = "radioButton_Factory_Reset";
            this.radioButton_Factory_Reset.Size = new Size(0x5b, 0x11);
            this.radioButton_Factory_Reset.TabIndex = 4;
            this.radioButton_Factory_Reset.TabStop = true;
            this.radioButton_Factory_Reset.Text = "&Factory Reset";
            this.radioButton_Factory_Reset.UseVisualStyleBackColor = true;
            this.radioButton_Factory_Reset.CheckedChanged += new EventHandler(this.radioButton_Factory_Reset_CheckedChanged);
            this.radioButton_ColdStart.AutoSize = true;
            this.radioButton_ColdStart.Location = new Point(8, 0x4a);
            this.radioButton_ColdStart.Name = "radioButton_ColdStart";
            this.radioButton_ColdStart.Size = new Size(0x47, 0x11);
            this.radioButton_ColdStart.TabIndex = 3;
            this.radioButton_ColdStart.TabStop = true;
            this.radioButton_ColdStart.Text = "&Cold Start";
            this.radioButton_ColdStart.UseVisualStyleBackColor = true;
            this.radioButton_WarmStart_Init.AutoSize = true;
            this.radioButton_WarmStart_Init.Location = new Point(8, 0x37);
            this.radioButton_WarmStart_Init.Name = "radioButton_WarmStart_Init";
            this.radioButton_WarmStart_Init.Size = new Size(0x65, 0x11);
            this.radioButton_WarmStart_Init.TabIndex = 2;
            this.radioButton_WarmStart_Init.TabStop = true;
            this.radioButton_WarmStart_Init.Text = "Warm Start (&Init)";
            this.radioButton_WarmStart_Init.UseVisualStyleBackColor = true;
            this.radioButton_WarmStart_Init.CheckedChanged += new EventHandler(this.radioButton_WarmStart_Init_CheckedChanged);
            this.radioButton_WarmStart_noInit.AutoSize = true;
            this.radioButton_WarmStart_noInit.Location = new Point(8, 0x24);
            this.radioButton_WarmStart_noInit.Name = "radioButton_WarmStart_noInit";
            this.radioButton_WarmStart_noInit.Size = new Size(0x76, 0x11);
            this.radioButton_WarmStart_noInit.TabIndex = 1;
            this.radioButton_WarmStart_noInit.TabStop = true;
            this.radioButton_WarmStart_noInit.Text = "&Warm Start (No Init)";
            this.radioButton_WarmStart_noInit.UseVisualStyleBackColor = true;
            this.radioButton_HotStart.AutoSize = true;
            this.radioButton_HotStart.Location = new Point(8, 0x11);
            this.radioButton_HotStart.Name = "radioButton_HotStart";
            this.radioButton_HotStart.Size = new Size(0x43, 0x11);
            this.radioButton_HotStart.TabIndex = 0;
            this.radioButton_HotStart.TabStop = true;
            this.radioButton_HotStart.Text = "&Hot Start";
            this.radioButton_HotStart.UseVisualStyleBackColor = true;
            this.checkBox_slcMode.AutoSize = true;
            this.checkBox_slcMode.Location = new Point(0x8a, 0x11);
            this.checkBox_slcMode.Name = "checkBox_slcMode";
            this.checkBox_slcMode.Size = new Size(0x2e, 0x11);
            this.checkBox_slcMode.TabIndex = 7;
            this.checkBox_slcMode.Text = "SLC";
            this.checkBox_slcMode.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.checkBox_performedAdingOnFactory);
            this.groupBox2.Controls.Add(this.checkBox_switchProtocolOnFactory);
            this.groupBox2.Controls.Add(this.checkBox_enable_Devlopment_Data);
            this.groupBox2.Controls.Add(this.checkBox_enable_Navlib_Data);
            this.groupBox2.Controls.Add(this.checkBox_enableFullSystemReset);
            this.groupBox2.Location = new Point(0xf8, 0x15f);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xa8, 0x9f);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            this.checkBox_performedAdingOnFactory.AutoSize = true;
            this.checkBox_performedAdingOnFactory.Location = new Point(8, 0x76);
            this.checkBox_performedAdingOnFactory.Name = "checkBox_performedAdingOnFactory";
            this.checkBox_performedAdingOnFactory.Size = new Size(0x6c, 0x11);
            this.checkBox_performedAdingOnFactory.TabIndex = 4;
            this.checkBox_performedAdingOnFactory.Text = "Aiding on Factory";
            this.checkBox_performedAdingOnFactory.UseVisualStyleBackColor = true;
            this.checkBox_switchProtocolOnFactory.AutoSize = true;
            this.checkBox_switchProtocolOnFactory.Location = new Point(8, 0x55);
            this.checkBox_switchProtocolOnFactory.Name = "checkBox_switchProtocolOnFactory";
            this.checkBox_switchProtocolOnFactory.Size = new Size(0x85, 30);
            this.checkBox_switchProtocolOnFactory.TabIndex = 3;
            this.checkBox_switchProtocolOnFactory.Text = "Switch Protocol/Baud \r\non Factory";
            this.checkBox_switchProtocolOnFactory.UseVisualStyleBackColor = true;
            this.checkBox_enable_Devlopment_Data.AutoSize = true;
            this.checkBox_enable_Devlopment_Data.Location = new Point(8, 0x2b);
            this.checkBox_enable_Devlopment_Data.Name = "checkBox_enable_Devlopment_Data";
            this.checkBox_enable_Devlopment_Data.Size = new Size(0x97, 0x11);
            this.checkBox_enable_Devlopment_Data.TabIndex = 1;
            this.checkBox_enable_Devlopment_Data.Text = "Enable &Development Data";
            this.checkBox_enable_Devlopment_Data.UseVisualStyleBackColor = true;
            this.checkBox_enable_Navlib_Data.AutoSize = true;
            this.checkBox_enable_Navlib_Data.Location = new Point(8, 0x16);
            this.checkBox_enable_Navlib_Data.Name = "checkBox_enable_Navlib_Data";
            this.checkBox_enable_Navlib_Data.Size = new Size(0x76, 0x11);
            this.checkBox_enable_Navlib_Data.TabIndex = 0;
            this.checkBox_enable_Navlib_Data.Text = "Enable &Navlib Data";
            this.checkBox_enable_Navlib_Data.UseVisualStyleBackColor = true;
            this.checkBox_enableFullSystemReset.AutoSize = true;
            this.checkBox_enableFullSystemReset.Location = new Point(8, 0x40);
            this.checkBox_enableFullSystemReset.Name = "checkBox_enableFullSystemReset";
            this.checkBox_enableFullSystemReset.Size = new Size(0x92, 0x11);
            this.checkBox_enableFullSystemReset.TabIndex = 2;
            this.checkBox_enableFullSystemReset.Text = "Enable Full System &Reset";
            this.checkBox_enableFullSystemReset.UseVisualStyleBackColor = true;
            this.button_Send.Location = new Point(0x8f, 0x20f);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new Size(0x4b, 0x17);
            this.button_Send.TabIndex = 4;
            this.button_Send.Text = "&Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new EventHandler(this.button_Send_Click);
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(0xe3, 0x20f);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            this.textBox_X.Location = new Point(0x66, 0x49);
            this.textBox_X.Name = "textBox_X";
            this.textBox_X.Size = new Size(0x44, 20);
            this.textBox_X.TabIndex = 1;
            this.textBox_X.Text = "-2682834";
            this.label_x.AutoSize = true;
            this.label_x.Location = new Point(0x44, 0x4d);
            this.label_x.Name = "label_x";
            this.label_x.Size = new Size(0x1f, 13);
            this.label_x.TabIndex = 5;
            this.label_x.Text = "X (m)";
            this.label_y.AutoSize = true;
            this.label_y.Location = new Point(0x44, 0x61);
            this.label_y.Name = "label_y";
            this.label_y.Size = new Size(0x1f, 13);
            this.label_y.TabIndex = 7;
            this.label_y.Text = "Y (m)";
            this.textBox_Y.Location = new Point(0x66, 0x5d);
            this.textBox_Y.Name = "textBox_Y";
            this.textBox_Y.Size = new Size(0x44, 20);
            this.textBox_Y.TabIndex = 2;
            this.textBox_Y.Text = "-4307681";
            this.label_z.AutoSize = true;
            this.label_z.Location = new Point(0x44, 0x75);
            this.label_z.Name = "label_z";
            this.label_z.Size = new Size(0x1f, 13);
            this.label_z.TabIndex = 9;
            this.label_z.Text = "Z (m)";
            this.textBox_Z.Location = new Point(0x66, 0x71);
            this.textBox_Z.Name = "textBox_Z";
            this.textBox_Z.Size = new Size(0x44, 20);
            this.textBox_Z.TabIndex = 3;
            this.textBox_Z.Text = "3850571";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x69, 0x36);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x2c, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Position";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0xd0, 0x4d);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x40, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Ext Week #";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xef, 0x61);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x21, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "TOW";
            this.textBox_WeekNum.Location = new Point(0x113, 0x49);
            this.textBox_WeekNum.Name = "textBox_WeekNum";
            this.textBox_WeekNum.Size = new Size(0x44, 20);
            this.textBox_WeekNum.TabIndex = 6;
            this.textBox_WeekNum.Text = "0";
            this.textBox_TimeOfWeek.Location = new Point(0x113, 0x5d);
            this.textBox_TimeOfWeek.Name = "textBox_TimeOfWeek";
            this.textBox_TimeOfWeek.Size = new Size(0x44, 20);
            this.textBox_TimeOfWeek.TabIndex = 7;
            this.textBox_TimeOfWeek.Text = "0";
            this.textBox_ClockDrift.Location = new Point(0x66, 0x85);
            this.textBox_ClockDrift.Name = "textBox_ClockDrift";
            this.textBox_ClockDrift.Size = new Size(0x44, 20);
            this.textBox_ClockDrift.TabIndex = 4;
            this.textBox_ClockDrift.Text = "0";
            this.textBox4.Location = new Point(-208, 0x83);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Size(100, 20);
            this.textBox4.TabIndex = 8;
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x15, 0x89);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x4e, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Clock Drift (Hz)";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0xdd, 0x75);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x33, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Channels";
            this.textBox_Channels.Location = new Point(0x113, 0x71);
            this.textBox_Channels.Name = "textBox_Channels";
            this.textBox_Channels.Size = new Size(0x44, 20);
            this.textBox_Channels.TabIndex = 8;
            this.textBox_Channels.Text = "12";
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.checkBox_usePCTime);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.initWarmInitDataBtn);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_TrueHeading);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_ClockDrift);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_Channels);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_TimeOfWeek);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_X);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_WeekNum);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label_x);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label11);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_Y);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label9);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.trueHeadingLabel);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label10);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label8);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label_y);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.textBox_Z);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label_z);
            this.frmRxInitCmdWarmInitParamsGrpBox.Controls.Add(this.label7);
            this.frmRxInitCmdWarmInitParamsGrpBox.Location = new Point(0x1a, 0xa5);
            this.frmRxInitCmdWarmInitParamsGrpBox.Name = "frmRxInitCmdWarmInitParamsGrpBox";
            this.frmRxInitCmdWarmInitParamsGrpBox.Size = new Size(0x185, 0xa8);
            this.frmRxInitCmdWarmInitParamsGrpBox.TabIndex = 1;
            this.frmRxInitCmdWarmInitParamsGrpBox.TabStop = false;
            this.frmRxInitCmdWarmInitParamsGrpBox.Text = "Warm Init Params";
            this.checkBox_usePCTime.AutoSize = true;
            this.checkBox_usePCTime.Location = new Point(0xf1, 0x2e);
            this.checkBox_usePCTime.Name = "checkBox_usePCTime";
            this.checkBox_usePCTime.Size = new Size(0x7d, 0x11);
            this.checkBox_usePCTime.TabIndex = 5;
            this.checkBox_usePCTime.Text = "Use Current PC Time";
            this.checkBox_usePCTime.UseVisualStyleBackColor = true;
            this.checkBox_usePCTime.CheckedChanged += new EventHandler(this.checkBox_usePCTime_CheckedChanged);
            this.initWarmInitDataBtn.Location = new Point(20, 20);
            this.initWarmInitDataBtn.Name = "initWarmInitDataBtn";
            this.initWarmInitDataBtn.Size = new Size(0xab, 0x17);
            this.initWarmInitDataBtn.TabIndex = 0;
            this.initWarmInitDataBtn.Text = "Update with current fixed data";
            this.initWarmInitDataBtn.UseVisualStyleBackColor = true;
            this.initWarmInitDataBtn.Click += new EventHandler(this.initWarmInitDataBtn_Click);
            this.textBox_TrueHeading.Location = new Point(0x113, 0x85);
            this.textBox_TrueHeading.Name = "textBox_TrueHeading";
            this.textBox_TrueHeading.Size = new Size(0x44, 20);
            this.textBox_TrueHeading.TabIndex = 9;
            this.textBox_TrueHeading.Text = "0";
            this.trueHeadingLabel.AutoSize = true;
            this.trueHeadingLabel.Location = new Point(0xbb, 0x89);
            this.trueHeadingLabel.Name = "trueHeadingLabel";
            this.trueHeadingLabel.Size = new Size(0x55, 13);
            this.trueHeadingLabel.TabIndex = 15;
            this.trueHeadingLabel.Text = "True Heading (\x00b0)";
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdCurrentFixPositionBtn);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdSetAsDefaultBtn);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdValidateLocationChkBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdRefLatitudeTxtBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdRefLatitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdRefLongitudeTxtBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdLongitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdRefAltitudeTxtBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdRefAltitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmRxInitCmdRefLocationComboBox);
            this.frmRxInitCmdRefLocationGrpBox.Location = new Point(0x1b, 0x10);
            this.frmRxInitCmdRefLocationGrpBox.Name = "frmRxInitCmdRefLocationGrpBox";
            this.frmRxInitCmdRefLocationGrpBox.Size = new Size(0x185, 0x73);
            this.frmRxInitCmdRefLocationGrpBox.TabIndex = 0;
            this.frmRxInitCmdRefLocationGrpBox.TabStop = false;
            this.frmRxInitCmdRefLocationGrpBox.Text = "Reference Location";
            this.frmRxInitCmdCurrentFixPositionBtn.Location = new Point(0x145, 20);
            this.frmRxInitCmdCurrentFixPositionBtn.Name = "frmRxInitCmdCurrentFixPositionBtn";
            this.frmRxInitCmdCurrentFixPositionBtn.Size = new Size(50, 0x17);
            this.frmRxInitCmdCurrentFixPositionBtn.TabIndex = 0x10;
            this.frmRxInitCmdCurrentFixPositionBtn.Text = "Fix Pos";
            this.frmRxInitCmdCurrentFixPositionBtn.UseVisualStyleBackColor = true;
            this.frmRxInitCmdCurrentFixPositionBtn.Click += new EventHandler(this.frmRxInitCmdCurrentFixPositionBtn_Click);
            this.frmRxInitCmdSetAsDefaultBtn.Location = new Point(20, 0x4c);
            this.frmRxInitCmdSetAsDefaultBtn.Name = "frmRxInitCmdSetAsDefaultBtn";
            this.frmRxInitCmdSetAsDefaultBtn.Size = new Size(0x95, 0x17);
            this.frmRxInitCmdSetAsDefaultBtn.TabIndex = 2;
            this.frmRxInitCmdSetAsDefaultBtn.Text = "Set as Default";
            this.frmRxInitCmdSetAsDefaultBtn.UseVisualStyleBackColor = true;
            this.frmRxInitCmdSetAsDefaultBtn.Click += new EventHandler(this.frmRxInitCmdSetAsDefaultBtn_Click);
            this.frmRxInitCmdValidateLocationChkBox.AutoSize = true;
            this.frmRxInitCmdValidateLocationChkBox.Location = new Point(20, 0x31);
            this.frmRxInitCmdValidateLocationChkBox.Name = "frmRxInitCmdValidateLocationChkBox";
            this.frmRxInitCmdValidateLocationChkBox.Size = new Size(0x91, 0x11);
            this.frmRxInitCmdValidateLocationChkBox.TabIndex = 1;
            this.frmRxInitCmdValidateLocationChkBox.Text = "Check Position Accuracy";
            this.frmRxInitCmdValidateLocationChkBox.UseVisualStyleBackColor = true;
            this.frmRxInitCmdValidateLocationChkBox.CheckedChanged += new EventHandler(this.frmRxInitCmdValidateLocationChkBox_CheckedChanged);
            this.frmRxInitCmdRefLatitudeTxtBox.Location = new Point(0xf4, 0x15);
            this.frmRxInitCmdRefLatitudeTxtBox.Name = "frmRxInitCmdRefLatitudeTxtBox";
            this.frmRxInitCmdRefLatitudeTxtBox.Size = new Size(0x44, 20);
            this.frmRxInitCmdRefLatitudeTxtBox.TabIndex = 3;
            this.frmRxInitCmdRefLatitudeTxtBox.Text = "-2682834";
            this.frmRxInitCmdRefLatitudeLabel.AutoSize = true;
            this.frmRxInitCmdRefLatitudeLabel.Location = new Point(0xc7, 0x19);
            this.frmRxInitCmdRefLatitudeLabel.Name = "frmRxInitCmdRefLatitudeLabel";
            this.frmRxInitCmdRefLatitudeLabel.Size = new Size(0x2d, 13);
            this.frmRxInitCmdRefLatitudeLabel.TabIndex = 11;
            this.frmRxInitCmdRefLatitudeLabel.Text = "Latitude";
            this.frmRxInitCmdRefLongitudeTxtBox.Location = new Point(0xf4, 0x31);
            this.frmRxInitCmdRefLongitudeTxtBox.Name = "frmRxInitCmdRefLongitudeTxtBox";
            this.frmRxInitCmdRefLongitudeTxtBox.Size = new Size(0x44, 20);
            this.frmRxInitCmdRefLongitudeTxtBox.TabIndex = 4;
            this.frmRxInitCmdRefLongitudeTxtBox.Text = "-4307681";
            this.frmRxInitCmdLongitudeLabel.AutoSize = true;
            this.frmRxInitCmdLongitudeLabel.Location = new Point(190, 0x35);
            this.frmRxInitCmdLongitudeLabel.Name = "frmRxInitCmdLongitudeLabel";
            this.frmRxInitCmdLongitudeLabel.Size = new Size(0x36, 13);
            this.frmRxInitCmdLongitudeLabel.TabIndex = 13;
            this.frmRxInitCmdLongitudeLabel.Text = "Longitude";
            this.frmRxInitCmdRefAltitudeTxtBox.Location = new Point(0xf4, 0x4d);
            this.frmRxInitCmdRefAltitudeTxtBox.Name = "frmRxInitCmdRefAltitudeTxtBox";
            this.frmRxInitCmdRefAltitudeTxtBox.Size = new Size(0x44, 20);
            this.frmRxInitCmdRefAltitudeTxtBox.TabIndex = 5;
            this.frmRxInitCmdRefAltitudeTxtBox.Text = "3850571";
            this.frmRxInitCmdRefAltitudeLabel.AutoSize = true;
            this.frmRxInitCmdRefAltitudeLabel.Location = new Point(0xca, 0x51);
            this.frmRxInitCmdRefAltitudeLabel.Name = "frmRxInitCmdRefAltitudeLabel";
            this.frmRxInitCmdRefAltitudeLabel.Size = new Size(0x2a, 13);
            this.frmRxInitCmdRefAltitudeLabel.TabIndex = 15;
            this.frmRxInitCmdRefAltitudeLabel.Text = "Altitude";
            this.frmRxInitCmdRefLocationComboBox.FormattingEnabled = true;
            this.frmRxInitCmdRefLocationComboBox.Location = new Point(0x12, 20);
            this.frmRxInitCmdRefLocationComboBox.Name = "frmRxInitCmdRefLocationComboBox";
            this.frmRxInitCmdRefLocationComboBox.Size = new Size(0x97, 0x15);
            this.frmRxInitCmdRefLocationComboBox.Sorted = true;
            this.frmRxInitCmdRefLocationComboBox.TabIndex = 0;
            this.frmRxInitCmdRefLocationComboBox.SelectedIndexChanged += new EventHandler(this.frmRxInitCmdRefLocationComboBox_SelectedIndexChanged);
            this.frmRxInitCmdRefLocationComboBox.DropDown += new EventHandler(this.frmRxInitCmdRefLocationComboBox_DropDown);
            this.frmRxInitCmdConfigAutoReplyBtn.Location = new Point(0x8a, 0x89);
            this.frmRxInitCmdConfigAutoReplyBtn.Name = "frmRxInitCmdConfigAutoReplyBtn";
            this.frmRxInitCmdConfigAutoReplyBtn.Size = new Size(0xa7, 0x17);
            this.frmRxInitCmdConfigAutoReplyBtn.TabIndex = 6;
            this.frmRxInitCmdConfigAutoReplyBtn.Text = "Config AutoReply";
            this.frmRxInitCmdConfigAutoReplyBtn.UseVisualStyleBackColor = true;
            this.frmRxInitCmdConfigAutoReplyBtn.Click += new EventHandler(this.frmRxInitCmdConfigAutoReplyBtn_Click);
            base.AcceptButton = this.button_Send;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x1bd, 570);
            base.Controls.Add(this.frmRxInitCmdConfigAutoReplyBtn);
            base.Controls.Add(this.frmRxInitCmdRefLocationGrpBox);
            base.Controls.Add(this.frmRxInitCmdWarmInitParamsGrpBox);
            base.Controls.Add(this.textBox4);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.button_Send);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmRXInit_cmd";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Receiver Initialization";
            base.Load += new EventHandler(this.frmRXInit_cmd_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.frmRxInitCmdWarmInitParamsGrpBox.ResumeLayout(false);
            this.frmRxInitCmdWarmInitParamsGrpBox.PerformLayout();
            this.frmRxInitCmdRefLocationGrpBox.ResumeLayout(false);
            this.frmRxInitCmdRefLocationGrpBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void initWarmInitDataBtn_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                try
                {
                    this.textBox_X.Text = this.comm.NavSolutionParams.ECEFX.ToString();
                    this.textBox_Y.Text = this.comm.NavSolutionParams.ECEFY.ToString();
                    this.textBox_Z.Text = this.comm.NavSolutionParams.ECEFZ.ToString();
                    this.textBox_ClockDrift.Text = this.comm.NavSolutionParams.ClockDrift.ToString();
                    this.textBox_WeekNum.Text = this.comm.NavSolutionParams.WeekNumber.ToString();
                    this.textBox_TimeOfWeek.Text = this.comm.NavSolutionParams.TOW.ToString();
                    this.checkBox_usePCTime.Checked = false;
                }
                catch
                {
                }
            }
        }

        private bool IsLeapYear(int Yr)
        {
            if ((Yr % 100) <= 0)
            {
                return ((Yr % 400) <= 0);
            }
            return ((Yr % 4) <= 0);
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void radioButton_Factory_Reset_CheckedChanged(object sender, EventArgs e)
        {
            this.updateControlsForFactoryReset();
        }

        private void radioButton_Factory_Reset_XO_CheckedChanged(object sender, EventArgs e)
        {
            this.updateControlsForFactoryReset();
        }

        private void radioButton_WarmStart_Init_CheckedChanged(object sender, EventArgs e)
        {
            this.updateControlsForWarmInit();
        }

        private void sendReset(ref CommunicationManager targetComm)
        {
            if (CommunicationManager.ValidateCommManager(targetComm))
            {
                clsGlobal.ToSwitchProtocol = targetComm.MessageProtocol;
                clsGlobal.ToSwitchBaudRate = targetComm.BaudRate;
                double num = -9999.0;
                double num2 = -9999.0;
                double num3 = -9999.0;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.ECEFX = this.textBox_X.Text;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.ECEFY = this.textBox_Y.Text;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.ECEFZ = this.textBox_Z.Text;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.ClockDrift = this.textBox_ClockDrift.Text;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.Channels = this.textBox_Channels.Text;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Development_Data = this.checkBox_enable_Devlopment_Data.Checked;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Navlib_Data = this.checkBox_enable_Navlib_Data.Checked;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.EnableFullSystemReset = this.checkBox_enableFullSystemReset.Checked;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.KeepFlashData = this.radioBtn_factoryKeepFlash.Checked;
                targetComm.RxCtrl.ResetCtrl.ResetInitParams.TrueHeading = this.textBox_TrueHeading.Text;
                if (!this.checkBox_usePCTime.Checked)
                {
                    targetComm.RxCtrl.ResetCtrl.ResetInitParams.TOW = this.textBox_TimeOfWeek.Text;
                    targetComm.RxCtrl.ResetCtrl.ResetInitParams.WeekNumber = this.textBox_WeekNum.Text;
                }
                else
                {
                    ushort weeknum = 0;
                    uint tow = 0;
                    this.GetGpsWkTow(ref weeknum, ref tow, 0);
                    targetComm.RxCtrl.ResetCtrl.ResetInitParams.WeekNumber = weeknum.ToString();
                    targetComm.RxCtrl.ResetCtrl.ResetInitParams.TOW = tow.ToString();
                }
                try
                {
                    num = Convert.ToDouble(this.frmRxInitCmdRefLatitudeTxtBox.Text);
                    num2 = Convert.ToDouble(this.frmRxInitCmdRefLongitudeTxtBox.Text);
                    num3 = Convert.ToDouble(this.frmRxInitCmdRefAltitudeTxtBox.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Reference Location: " + exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                try
                {
                    targetComm.RxCtrl.RxNavData.RefLocationName = this.frmRxInitCmdRefLocationComboBox.Text;
                    targetComm.RxCtrl.RxNavData.RefLat = num;
                    targetComm.RxCtrl.RxNavData.RefLon = num2;
                    targetComm.RxCtrl.RxNavData.RefAlt = num3;
                    targetComm.RxCtrl.RxNavData.ValidatePosition = this.frmRxInitCmdValidateLocationChkBox.Checked;
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message + "\nMost likely invalid reference name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                targetComm.m_NavData = targetComm.RxCtrl.RxNavData;
                string resetType = string.Empty;
                if (this.radioButton_HotStart.Checked)
                {
                    if (this.checkBox_slcMode.Checked)
                    {
                        resetType = "SLC_HOT";
                    }
                    else
                    {
                        resetType = "HOT";
                    }
                }
                else if (this.radioButton_WarmStart_noInit.Checked)
                {
                    resetType = "WARM_NO_INIT";
                }
                else if (this.radioButton_WarmStart_Init.Checked)
                {
                    resetType = "WARM_INIT";
                }
                else if (this.radioButton_ColdStart.Checked)
                {
                    if (this.checkBox_slcMode.Checked)
                    {
                        resetType = "SLC_COLD";
                    }
                    else
                    {
                        resetType = "COLD";
                    }
                }
                else if (this.radioButton_Factory_Reset.Checked)
                {
                    if (this.checkBox_slcMode.Checked)
                    {
                        resetType = "SLC_FACTORY";
                    }
                    else
                    {
                        resetType = "FACTORY";
                    }
                }
                else if (this.radioButton_Factory_Reset_XO.Checked)
                {
                    resetType = "FACTORY_XO";
                }
                else if (this.radioBtn_factoryKeepFlash.Checked)
                {
                    resetType = "FACTORY";
                }
                this.setAsDefault(ref targetComm);
                targetComm.RxCtrl.ResetCtrl.IsProtocolSwitchedOnFactory = this.checkBox_switchProtocolOnFactory.Checked;
                targetComm.RxCtrl.ResetCtrl.IsAidingPerformedOnFactory = this.checkBox_performedAdingOnFactory.Checked;
                try
                {
                    targetComm.RxCtrl.ResetCtrl.Reset(resetType);
                }
                catch (Exception exception3)
                {
                    MessageBox.Show("frmRXInit_cmd: button_Send_Click()\r\r" + exception3.ToString(), "Error Information");
                }
            }
        }

        private void setAsDefault(ref CommunicationManager targetComm)
        {
            try
            {
                if (this.frmRxInitCmdRefLocationComboBox.Text != "USER_DEFINED")
                {
                    targetComm.m_NavData.SetDefaultReferencePosition(this.frmRxInitCmdRefLocationComboBox.Text, this.frmRxInitCmdRefLatitudeTxtBox.Text, this.frmRxInitCmdRefLongitudeTxtBox.Text, this.frmRxInitCmdRefAltitudeTxtBox.Text, false);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\nMost likely invalid reference name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void updateConfigList(string updatedName)
        {
            this.comm.RxCtrl.RxNavData.RefLocationName = updatedName;
            this.updateReferenceLocationComboBox();
        }

        private void updateControlsForFactoryReset()
        {
            if (CommunicationManager.ValidateCommManager(this.comm))
            {
                this.checkBox_switchProtocolOnFactory.Checked = this.comm.RxCtrl.ResetCtrl.IsProtocolSwitchedOnFactory;
                this.checkBox_performedAdingOnFactory.Checked = this.comm.RxCtrl.ResetCtrl.IsAidingPerformedOnFactory;
                if ((this.comm.ProductFamily != CommonClass.ProductType.GSD4e) && !clsGlobal.PerformOnAll)
                {
                    this.checkBox_switchProtocolOnFactory.Checked = false;
                    this.checkBox_switchProtocolOnFactory.Enabled = false;
                    this.checkBox_switchProtocolOnFactory.Visible = false;
                }
                else
                {
                    this.checkBox_switchProtocolOnFactory.Enabled = true;
                    this.checkBox_switchProtocolOnFactory.Visible = true;
                }
                if ((this.radioButton_Factory_Reset.Checked || this.radioButton_Factory_Reset_XO.Checked) || this.radioBtn_factoryKeepFlash.Checked)
                {
                    this.checkBox_enable_Navlib_Data.Enabled = false;
                    this.checkBox_enable_Devlopment_Data.Enabled = false;
                    this.checkBox_performedAdingOnFactory.Enabled = true;
                }
                else
                {
                    this.checkBox_enable_Navlib_Data.Enabled = true;
                    this.checkBox_enable_Devlopment_Data.Enabled = true;
                    this.checkBox_switchProtocolOnFactory.Enabled = false;
                    this.checkBox_performedAdingOnFactory.Enabled = false;
                }
            }
        }

        private void updateControlsForWarmInit()
        {
            try
            {
                this.textBox_Channels.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.Channels;
                this.textBox_TrueHeading.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.TrueHeading;
                this.textBox_ClockDrift.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.ClockDrift;
                this.textBox_TimeOfWeek.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.TOW;
                this.textBox_WeekNum.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.WeekNumber;
                this.textBox_X.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.ECEFX;
                this.textBox_Y.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.ECEFY;
                this.textBox_Z.Text = this.comm.RxCtrl.ResetCtrl.ResetInitParams.ECEFZ;
                this.updateDRControlsForWarmInit();
                if (this.radioButton_WarmStart_Init.Checked)
                {
                    this.checkBox_usePCTime.Enabled = true;
                    this.updatewhenUsePCTimeChange();
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_Channels, true);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_TrueHeading, true);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_ClockDrift, true);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_X, true);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_Y, true);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_Z, true);
                    this.initWarmInitDataBtn.Enabled = true;
                }
                else
                {
                    this.checkBox_usePCTime.Enabled = false;
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_Channels, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_TrueHeading, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_ClockDrift, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_TimeOfWeek, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_WeekNum, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_X, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_Y, false);
                    this.cpGuiCtrl.SetTextBoxState(this.textBox_Z, false);
                    this.initWarmInitDataBtn.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("frmRXInit_cmd: updateControlsForWarmInit() exception: " + exception.ToString());
            }
        }

        private void updateDefautlReferenceLocationComboBox()
        {
            this._maxItemWidth = this._defaultWidth;
            if (this.frmRxInitCmdRefLocationComboBox.Items.Count != 0)
            {
                this.frmRxInitCmdRefLocationComboBox.Items.Clear();
            }
            ArrayList referenceLocationName = new ArrayList();
            referenceLocationName = this.comm.m_NavData.GetReferenceLocationName();
            for (int i = 0; i < referenceLocationName.Count; i++)
            {
                this.frmRxInitCmdRefLocationComboBox.Items.Add(referenceLocationName[i]);
                if (this._maxItemWidth < (referenceLocationName[i].ToString().Length * 6))
                {
                    this._maxItemWidth = referenceLocationName[i].ToString().Length * 6;
                }
            }
            this.frmRxInitCmdRefLocationComboBox.Items.Add("USER_DEFINED");
            this.comm.m_NavData.RefLocationName = "Default";
            this.frmRxInitCmdRefLocationComboBox.Text = this.comm.m_NavData.RefLocationName;
            this.updateReferencePositionTextBox();
        }

        private void updateDRControlsForWarmInit()
        {
            if (clsGlobal.IsDROn)
            {
                this.textBox_TrueHeading.Visible = true;
                this.textBox_TrueHeading.Enabled = true;
                this.initWarmInitDataBtn.Visible = false;
                this.trueHeadingLabel.Visible = true;
                this.label_x.Text = "Lat (deg)";
                this.label_y.Text = "Lon (deg)";
                this.label_z.Text = "Alt (m)";
            }
            else
            {
                this.textBox_TrueHeading.Visible = false;
                this.initWarmInitDataBtn.Visible = true;
                this.initWarmInitDataBtn.Enabled = false;
                this.trueHeadingLabel.Visible = false;
                this.label_x.Text = "X (m)";
                this.label_y.Text = "Y (m)";
                this.label_z.Text = "Z (m)";
            }
        }

        private void updateReferenceLocationComboBox()
        {
            try
            {
                if (!this.frmRxInitCmdRefLocationComboBox.Items.Contains(this.comm.RxCtrl.RxNavData.RefLocationName))
                {
                    this.frmRxInitCmdRefLocationComboBox.Items.Add(this.comm.RxCtrl.RxNavData.RefLocationName);
                    this.frmRxInitCmdRefLocationComboBox.Text = this.comm.RxCtrl.RxNavData.RefLocationName;
                    this.frmRxInitCmdRefLatitudeTxtBox.Enabled = true;
                    this.frmRxInitCmdRefLongitudeTxtBox.Enabled = true;
                    this.frmRxInitCmdRefAltitudeTxtBox.Enabled = true;
                    this.frmRxInitCmdCurrentFixPositionBtn.Enabled = true;
                }
                else
                {
                    this.frmRxInitCmdRefLocationComboBox.Text = this.comm.RxCtrl.RxNavData.RefLocationName;
                    this.frmRxInitCmdRefLatitudeTxtBox.Enabled = false;
                    this.frmRxInitCmdRefLongitudeTxtBox.Enabled = false;
                    this.frmRxInitCmdRefAltitudeTxtBox.Enabled = false;
                    this.frmRxInitCmdCurrentFixPositionBtn.Enabled = false;
                }
                this.updateReferencePositionTextBox();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void updateReferencePositionTextBox()
        {
            PositionInLatLonAlt referencePosition = this.comm.m_NavData.GetReferencePosition(this.frmRxInitCmdRefLocationComboBox.Text);
            if (referencePosition.name == "LAST_FIX_POSITION")
            {
                if (this.comm.m_NavData.MeasLat != -9999.0)
                {
                    referencePosition.latitude = this.comm.m_NavData.MeasLat;
                }
                if (this.comm.m_NavData.MeasLon != -9999.0)
                {
                    referencePosition.longitude = this.comm.m_NavData.MeasLon;
                }
                if (this.comm.m_NavData.MeasAlt != -9999.0)
                {
                    referencePosition.altitude = this.comm.m_NavData.MeasAlt;
                }
            }
            this.frmRxInitCmdRefLatitudeTxtBox.Text = referencePosition.latitude.ToString();
            this.frmRxInitCmdRefLongitudeTxtBox.Text = referencePosition.longitude.ToString();
            this.frmRxInitCmdRefAltitudeTxtBox.Text = referencePosition.altitude.ToString();
            this.frmRxInitCmdRefLocationComboBox.Text = referencePosition.name;
            this.frmRxInitCmdRefLocationComboBox.Width = this._defaultWidth;
        }

        private void updatewhenUsePCTimeChange()
        {
            if (this.checkBox_usePCTime.Checked)
            {
                this.textBox_WeekNum.Enabled = false;
                this.textBox_TimeOfWeek.Enabled = false;
            }
            else
            {
                this.textBox_WeekNum.Enabled = true;
                this.textBox_TimeOfWeek.Enabled = true;
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
            }
        }
    }
}

