﻿namespace SiRFLive.GUI
{
    using CommMgrClassLibrary;
    using CommonClassLibrary;
    using CommonUtilsClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    public class frmEncryCtrl : Form
    {
        private static string _EncryCtrlRestoredFilePath;
        private Button Cancel_Button;
        private static int[] checkBit = new int[] { 1, 2, 4, 8, 0x10, 0x20, 0x40, 0x80, 0 };
        public CommMgrClass CMC;
        public CommunicationManager comm;
        private IContainer components;
        private CommonUtilsClass CUC = new CommonUtilsClass();
        private DirectoryInfo currentDirInfo;
        private static EcryptCtrlMsgStruct EcryptCtrlMsg;
        private DataGridView EncryCtrlDataGridView;
        private static frmEncryCtrl encryptCtrlFormHandle = null;
        private string InstalledDirectory;
        private const int MAX_DBG_LEVEL = 9;
        private const int MAX_NAVLIB_VERS = 5;
        private const int MAX_PRG_MODULE = 0x31;
        private const int MODULEID_MASK_BYTES = 8;
        private string[] ModuleNames = new string[] { 
            "All Modules", "DSP", "INTC", "RTC", "HWTIMER", "UART", "MEMORY", "PWRCLK", "MISC", "SPI", "CBHANDLER", "CHDEVICE", "INSAMPLE", "AGC", "CLKGEN", "ATXCTRL", 
            "ACQ", "TRACK", "XCOR", "CW", "DGPSTRK", "DRM", "OS", "RESET", "PWRMGRCTRL", "CTRLPLATDATA", "NVM", "IOSTREAM", "MITASK", "UIDBG", "PWRMGRLOGIC", "AIDING", 
            "CTRLDATA", "GPSRXCTRL", "ATXCTRLMGR", "VISLIST", "MIGPS", "NAV", "DGPSAPP", "BEP", "SL", "UIGPS", "SSB", "NMEA", "QOS", "SVDATA", "TASKCTRL", "SPISLV", 
            "MEI", "DUMMY1"
         };
        private Button OkButton;

        public event updateParentEventHandler updateMainWindow;

        public frmEncryCtrl(CommunicationManager mainComWin)
        {
            this.InitializeComponent();
            encryptCtrlFormHandle = this;
            this.currentDirInfo = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
            this.InstalledDirectory = this.currentDirInfo.Parent.FullName;
            _EncryCtrlRestoredFilePath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Config\EncryptControlRestore.xml";
            this.CommWindow = mainComWin;
            this.CMC = new CommMgrClass();
            this.DrawTheEncryptControl(this.EncryCtrlDataGridView);
            this.AddRowNamesToEncryptControl(this.EncryCtrlDataGridView);
            this.SetAllCheckBoxesToAValue(this.EncryCtrlDataGridView, false);
        }

        private void AddRowNamesToEncryptControl(DataGridView dgv)
        {
            for (int i = 0; i < this.ModuleNames.Length; i++)
            {
                dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                dgv.Rows[i].Cells[0].Value = this.ModuleNames[i];
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ConvertCheckBoxesToEcryptCtrlMsgStruct(DataGridView dgv, ref EcryptCtrlMsgStruct ecm)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            int num3 = 0;
            for (int i = 1; i <= 0x31; i++)
            {
                int num4 = 0;
                bool flag = false;
                for (int j = 2; j < 10; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value.Equals(true))
                    {
                        num4 = checkBit[j - 2] | num4;
                        if (!flag)
                        {
                            flag = true;
                            num3 = 1;
                        }
                    }
                }
                str = string.Format("{0:X2} ", num4);
                str2 = str2 + str;
            }
            ecm.LevelChange = num3;
            ecm.strDbgLevels = str2;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawTheEncryptControl(DataGridView dgv)
        {
            dgv.DefaultCellStyle.BackColor = Color.Bisque;
            DataGridViewColumn dataGridViewColumn = new DataGridViewTextBoxColumn();
            dataGridViewColumn.HeaderText = "ModuleID";
            dataGridViewColumn.ReadOnly = true;
            dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns.Add(dataGridViewColumn);
            DataGridViewCheckBoxColumn[] columnArray = new DataGridViewCheckBoxColumn[9];
            columnArray.Initialize();
            for (int i = 0; i < 9; i++)
            {
                columnArray[i] = new DataGridViewCheckBoxColumn();
                columnArray[i].CellTemplate.Value = CheckState.Unchecked;
                columnArray[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            columnArray[0].HeaderText = "All Levels";
            columnArray[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns.Add(columnArray[0]);
            for (int j = 1; j < 9; j++)
            {
                columnArray[j].HeaderText = "Level" + j.ToString();
                columnArray[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv.Columns.Add(columnArray[j]);
            }
            dgv.Rows.AddCopies(0, 0x31);
        }

        private void EncryCtrlDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            this.SetStateOfRowsAndColumns(columnIndex, rowIndex);
        }

        private void EncryCtrlDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            this.SetStateOfRowsAndColumns(columnIndex, rowIndex);
        }

        private void frmEncryCtrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveEcryptCtrlDataSettings(EcryptCtrlMsg);
        }

        private void frmEncryCtrl_Load(object sender, EventArgs e)
        {
            if (this.LoadEcryptCtrlDataSettings(ref EcryptCtrlMsg))
            {
                this.RestoreCheckBoxStates(EcryptCtrlMsg, this.EncryCtrlDataGridView);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmEncryCtrl));
            this.OkButton = new Button();
            this.Cancel_Button = new Button();
            this.EncryCtrlDataGridView = new DataGridView();
            ((ISupportInitialize) this.EncryCtrlDataGridView).BeginInit();
            base.SuspendLayout();
            this.OkButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.OkButton.Location = new Point(0x26a, 0x1f);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new Size(0x39, 0x17);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "&OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new EventHandler(this.OkButton_Click);
            this.Cancel_Button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new Point(0x26a, 0x4a);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new Size(0x39, 0x17);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "&Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new EventHandler(this.CancelButton_Click);
            this.EncryCtrlDataGridView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.EncryCtrlDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EncryCtrlDataGridView.Location = new Point(12, 0x1c);
            this.EncryCtrlDataGridView.Name = "EncryCtrlDataGridView";
            this.EncryCtrlDataGridView.RowTemplate.DefaultCellStyle.ForeColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.EncryCtrlDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.EncryCtrlDataGridView.Size = new Size(580, 0x2ad);
            this.EncryCtrlDataGridView.TabIndex = 0;
            this.EncryCtrlDataGridView.CellContentDoubleClick += new DataGridViewCellEventHandler(this.EncryCtrlDataGridView_CellContentDoubleClick);
            this.EncryCtrlDataGridView.CellContentClick += new DataGridViewCellEventHandler(this.EncryCtrlDataGridView_CellContentClick);
            base.AcceptButton = this.OkButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.Cancel_Button;
            base.ClientSize = new Size(700, 0x2d5);
            base.Controls.Add(this.Cancel_Button);
            base.Controls.Add(this.OkButton);
            base.Controls.Add(this.EncryCtrlDataGridView);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmEncryCtrl";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Set Developer Debug Levels";
            base.FormClosing += new FormClosingEventHandler(this.frmEncryCtrl_FormClosing);
            base.Load += new EventHandler(this.frmEncryCtrl_Load);
            ((ISupportInitialize) this.EncryCtrlDataGridView).EndInit();
            base.ResumeLayout(false);
        }

        private bool LoadEcryptCtrlDataSettings(ref EcryptCtrlMsgStruct ecm)
        {
            bool flag = false;
            XmlDocument document = new XmlDocument();
            if (File.Exists(_EncryCtrlRestoredFilePath))
            {
                try
                {
                    document.Load(_EncryCtrlRestoredFilePath);
                    foreach (XmlNode node in document.SelectNodes("/EncryptionControl/Window"))
                    {
                        string str2;
                        if (((str2 = node.Attributes["name"].Value.ToString()) != null) && (str2 == "frmEncryCtrl"))
                        {
                            EcryptCtrlMsg.LevelChange = Convert.ToInt32(node.Attributes["LevelChange"].Value.ToString());
                            EcryptCtrlMsg.strDbgLevels = node.Attributes["strDbgLevels"].Value.ToString();
                            CommonUtilsClass.loadLocation(encryptCtrlFormHandle, node.Attributes["top"].Value.ToString(), node.Attributes["left"].Value.ToString(), node.Attributes["width"].Value.ToString(), node.Attributes["height"].Value.ToString(), node.Attributes["windowState"].Value.ToString());
                            flag = true;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("frmEncryCtrl() + LoadEcryptCtrlDataSettings()\r\r" + exception.ToString());
                }
            }
            return flag;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            bool isSLCRx = false;
            int mid = 0xe4;
            int sid = 0;
            byte channelType = 0;
            try
            {
                this.comm.CMC.TxCurrentTransmissionType = (CommonClass.TransmissionType) this.comm.TxCurrentTransmissionType;
                string protocol = "OSP";
                isSLCRx = this.comm.RxType == CommunicationManager.ReceiverType.SLC;
                ArrayList list = this.comm.m_Protocols.GetDefaultMsgFieldList(isSLCRx, mid, sid, "Encryption Control Settings", protocol);
                InputMsg[] ltc = new InputMsg[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    ltc[i].messageID = ((InputMsg) list[i]).messageID;
                    ltc[i].subID = ((InputMsg) list[i]).subID;
                    ltc[i].messageName = ((InputMsg) list[i]).messageName;
                    ltc[i].fieldNumber = ((InputMsg) list[i]).fieldNumber;
                    ltc[i].fieldName = ((InputMsg) list[i]).fieldName;
                    ltc[i].bytes = ((InputMsg) list[i]).bytes;
                    ltc[i].datatype = ((InputMsg) list[i]).datatype;
                    ltc[i].units = ((InputMsg) list[i]).units;
                    ltc[i].scale = ((InputMsg) list[i]).scale;
                    ltc[i].defaultValue = ((InputMsg) list[i]).defaultValue;
                    ltc[i].savedValue = ((InputMsg) list[i]).savedValue;
                }
                try
                {
                    this.ConvertCheckBoxesToEcryptCtrlMsgStruct(this.EncryCtrlDataGridView, ref EcryptCtrlMsg);
                    this.UpdateLocalMsgCopyWithEncCtrlSettings(ref ltc, EcryptCtrlMsg);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                string msg = this.comm.m_Protocols.FieldList_to_HexMsgStr(isSLCRx, ltc, channelType);
                if (clsGlobal.PerformOnAll)
                {
                    foreach (string str3 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str3];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            manager.comm.WriteData(msg);
                        }
                    }
                    clsGlobal.PerformOnAll = false;
                }
                else
                {
                    this.comm.WriteData(msg);
                }
            }
            catch (Exception exception2)
            {
                if (!this.comm.IsSourceDeviceOpen())
                {
                    MessageBox.Show("Source Device is not open", "frmEncryCtrl: OkButton_Click()");
                }
                else
                {
                    MessageBox.Show(exception2.ToString(), "frmEncryCtrl: OkButton_Click()");
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow("frmEncryCtrl");
            }
            base.OnClosed(e);
        }

        private void RestoreCheckBoxStates(EcryptCtrlMsgStruct ecm, DataGridView dgv)
        {
            string[] strArray = ecm.strDbgLevels.Split(new char[] { ' ' });
            for (int i = 0; i < 0x31; i++)
            {
                ushort num2 = Convert.ToUInt16(strArray[i], 0x10);
                for (int j = 0; j < 9; j++)
                {
                    if ((num2 & checkBit[j]) > 0)
                    {
                        dgv.Rows[i + 1].Cells[j + 2].Value = true;
                    }
                }
            }
        }

        private void SaveEcryptCtrlDataSettings(EcryptCtrlMsgStruct ecm)
        {
            StreamWriter writer;
            if (File.Exists(_EncryCtrlRestoredFilePath))
            {
                if ((File.GetAttributes(_EncryCtrlRestoredFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("File is read only - Window locations were not saved!\n{0}", _EncryCtrlRestoredFilePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                writer = new StreamWriter(_EncryCtrlRestoredFilePath);
            }
            else
            {
                writer = File.CreateText(_EncryCtrlRestoredFilePath);
            }
            if (writer != null)
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<EncryptionControl>");
                string str2 = string.Empty;
                string format = string.Empty;
                frmEncryCtrl encryptCtrlFormHandle = frmEncryCtrl.encryptCtrlFormHandle;
                format = "<Window name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\" LevelChange=\"{6}\" strDbgLevels=\"{7}\" >";
                str2 = string.Format(format, new object[] { encryptCtrlFormHandle.Name, encryptCtrlFormHandle.Top.ToString(), encryptCtrlFormHandle.Left.ToString(), encryptCtrlFormHandle.Width.ToString(), encryptCtrlFormHandle.Height.ToString(), encryptCtrlFormHandle.WindowState.ToString(), EcryptCtrlMsg.LevelChange.ToString(), EcryptCtrlMsg.strDbgLevels });
                writer.WriteLine(str2);
                writer.WriteLine("</Window>");
                writer.WriteLine("</EncryptionControl>");
                writer.Close();
            }
        }

        private void SetAllCheckBoxesInAColumnToValue(DataGridView dgv, int col, bool val)
        {
            if ((col > 0) && (col <= 9))
            {
                for (int i = 0; i < this.ModuleNames.Length; i++)
                {
                    dgv.Rows[i].Cells[col].Value = val;
                }
            }
        }

        private void SetAllCheckBoxesInARowToValue(DataGridView dgv, int row, bool val)
        {
            for (int i = 1; i <= 9; i++)
            {
                dgv.Rows[row].Cells[i].Value = val;
            }
        }

        private void SetAllCheckBoxesToAValue(DataGridView dgv, bool val)
        {
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 0; j < this.ModuleNames.Length; j++)
                {
                    dgv.Rows[j].Cells[i].Value = val;
                }
            }
        }

        private void SetStateOfRowsAndColumns(int colIndex, int rowIndex)
        {
            if (((colIndex >= 1) && (rowIndex >= 0)) && ((colIndex <= 9) && (rowIndex < this.ModuleNames.Length)))
            {
                if ((colIndex == 1) && (rowIndex == 0))
                {
                    if (this.EncryCtrlDataGridView.Rows[0].Cells[1].Value.Equals(true))
                    {
                        this.SetAllCheckBoxesToAValue(this.EncryCtrlDataGridView, false);
                    }
                    else
                    {
                        this.SetAllCheckBoxesToAValue(this.EncryCtrlDataGridView, true);
                    }
                }
                else if (rowIndex == 0)
                {
                    if (this.EncryCtrlDataGridView.Rows[rowIndex].Cells[colIndex].Value.Equals(true))
                    {
                        this.SetAllCheckBoxesInAColumnToValue(this.EncryCtrlDataGridView, colIndex, false);
                    }
                    else
                    {
                        this.SetAllCheckBoxesInAColumnToValue(this.EncryCtrlDataGridView, colIndex, true);
                    }
                }
                else if (colIndex == 1)
                {
                    if (this.EncryCtrlDataGridView.Rows[rowIndex].Cells[colIndex].Value.Equals(true))
                    {
                        this.SetAllCheckBoxesInARowToValue(this.EncryCtrlDataGridView, rowIndex, false);
                    }
                    else
                    {
                        this.SetAllCheckBoxesInARowToValue(this.EncryCtrlDataGridView, rowIndex, true);
                    }
                }
            }
        }

        private void UpdateLocalMsgCopyWithEncCtrlSettings(ref InputMsg[] ltc, EcryptCtrlMsgStruct ecm)
        {
            ltc[2].defaultValue = ecm.LevelChange.ToString();
            string[] strArray = ecm.strDbgLevels.Split(new char[] { ' ' });
            for (int i = 0; i < 0x31; i++)
            {
                ltc[i + 3].defaultValue = Convert.ToInt16(strArray[i], 0x10).ToString();
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
                this.Text = this.comm.sourceDeviceName + ": Encrypt Settings";
            }
        }

        public string EncryCtrlRestoredFilePath
        {
            get
            {
                return _EncryCtrlRestoredFilePath;
            }
            set
            {
                _EncryCtrlRestoredFilePath = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct EcryptCtrlMsgStruct
        {
            public int LevelChange;
            public string strDbgLevels;
        }

        public delegate void updateParentEventHandler(string titleString);
    }
}

