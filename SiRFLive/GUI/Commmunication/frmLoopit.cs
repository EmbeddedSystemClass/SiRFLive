﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class frmLoopit : Form
    {
        private IContainer components;
        private CheckBox frmLoopitAllowEarlyTerminateChkBox;
        private Button frmLoopitCancelBtn;
        private Label frmLoopitComNameLabel;
        private Label frmLoopitIterationLabel;
        private TextBox frmLoopitIterationTxtBox;
        private Label frmLoopitNumberComDetectedLabel;
        private CheckBox frmLoopitPerformAidingOnFactoryChkBox;
        private ComboBox frmLoopitResetTypeComboBox;
        private TextBox frmLoopitSecPerIterationTxtBox;
        private Button frmLoopitStartBtn;
        private CheckBox frmLoopitSwitchProtocolOnFactoryChkBox;
        private frmCommonSimpleInput inputForm = new frmCommonSimpleInput("Enter Reset Type:");
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        public static string TempRType = "";

        public frmLoopit()
        {
            this.InitializeComponent();
            this.inputForm.updateParent += new frmCommonSimpleInput.updateParentEventHandler(this.updateConfigList);
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

        private void frmLoopit_Load(object sender, EventArgs e)
        {
            if (this.frmLoopitResetTypeComboBox.Items.Count == 0)
            {
                this.loadResetType();
            }
            string str = string.Empty;
            int num = 0;
            foreach (string str2 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
            {
                PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str2];
                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                {
                    str = str + " " + str2 + ",";
                    num++;
                }
            }
            this.frmLoopitNumberComDetectedLabel.Text = "Number of DUT detected: " + num;
            this.frmLoopitComNameLabel.Text = str.TrimEnd(new char[] { ',' });
            this.frmLoopitSecPerIterationTxtBox.Text = clsGlobal.LoopitTimeout.ToString();
            this.frmLoopitIterationTxtBox.Text = clsGlobal.LoopitIteration.ToString();
            this.frmLoopitAllowEarlyTerminateChkBox.Checked = clsGlobal.LoopitEarlyTermination;
        }

        private void frmLoopitCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmLoopitResetTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.frmLoopitResetTypeComboBox.SelectedItem.ToString() == "USER_DEFINED")
            {
                this.inputForm.UpdateType = "UPDATE_RESET_TYPE";
                this.inputForm.ShowDialog();
            }
        }

        private void frmLoopitStartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                clsGlobal.LoopitIteration = Convert.ToInt32(this.frmLoopitIterationTxtBox.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Iterations: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                clsGlobal.LoopitTimeout = Convert.ToUInt32(this.frmLoopitSecPerIterationTxtBox.Text);
                if (clsGlobal.LoopitTimeout < 10)
                {
                    MessageBox.Show("Min value is 10s", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    clsGlobal.LoopitTimeout = 10;
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show("Iterations: " + exception2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            clsGlobal.LoopitResetType = this.frmLoopitResetTypeComboBox.Text;
            clsGlobal.LoopitEarlyTermination = this.frmLoopitAllowEarlyTerminateChkBox.Checked;
            clsGlobal.LoopitSwitchProtocolNBaudOnFactory = this.frmLoopitSwitchProtocolOnFactoryChkBox.Checked;
            clsGlobal.LoopitPerformedAidingOnFactory = this.frmLoopitPerformAidingOnFactoryChkBox.Checked;
            if (this.StartLoopit())
            {
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmLoopit));
            this.label1 = new Label();
            this.frmLoopitNumberComDetectedLabel = new Label();
            this.frmLoopitComNameLabel = new Label();
            this.frmLoopitResetTypeComboBox = new ComboBox();
            this.frmLoopitIterationTxtBox = new TextBox();
            this.frmLoopitIterationLabel = new Label();
            this.frmLoopitSecPerIterationTxtBox = new TextBox();
            this.label2 = new Label();
            this.frmLoopitAllowEarlyTerminateChkBox = new CheckBox();
            this.frmLoopitStartBtn = new Button();
            this.frmLoopitCancelBtn = new Button();
            this.label3 = new Label();
            this.label4 = new Label();
            this.frmLoopitSwitchProtocolOnFactoryChkBox = new CheckBox();
            this.frmLoopitPerformAidingOnFactoryChkBox = new CheckBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x11, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2c4, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "For Aided Resets (MSA/MSB) --- Remember to  setup aiding parameters in AGPS Configuration -> Configure...";
            this.frmLoopitNumberComDetectedLabel.AutoSize = true;
            this.frmLoopitNumberComDetectedLabel.Location = new Point(0x11, 0x3b);
            this.frmLoopitNumberComDetectedLabel.Name = "frmLoopitNumberComDetectedLabel";
            this.frmLoopitNumberComDetectedLabel.Size = new Size(0x7a, 13);
            this.frmLoopitNumberComDetectedLabel.TabIndex = 10;
            this.frmLoopitNumberComDetectedLabel.Text = "Number COM detected: ";
            this.frmLoopitComNameLabel.AutoSize = true;
            this.frmLoopitComNameLabel.Location = new Point(0x11, 0x57);
            this.frmLoopitComNameLabel.Name = "frmLoopitComNameLabel";
            this.frmLoopitComNameLabel.Size = new Size(0x1f, 13);
            this.frmLoopitComNameLabel.TabIndex = 11;
            this.frmLoopitComNameLabel.Text = "COM";
            this.frmLoopitResetTypeComboBox.FormattingEnabled = true;
            this.frmLoopitResetTypeComboBox.Location = new Point(20, 130);
            this.frmLoopitResetTypeComboBox.Name = "frmLoopitResetTypeComboBox";
            this.frmLoopitResetTypeComboBox.Size = new Size(0x79, 0x15);
            this.frmLoopitResetTypeComboBox.TabIndex = 0;
            this.frmLoopitResetTypeComboBox.SelectedIndexChanged += new EventHandler(this.frmLoopitResetTypeComboBox_SelectedIndexChanged);
            this.frmLoopitIterationTxtBox.Location = new Point(0xa3, 0x83);
            this.frmLoopitIterationTxtBox.Name = "frmLoopitIterationTxtBox";
            this.frmLoopitIterationTxtBox.Size = new Size(100, 20);
            this.frmLoopitIterationTxtBox.TabIndex = 1;
            this.frmLoopitIterationLabel.AutoSize = true;
            this.frmLoopitIterationLabel.Location = new Point(0x93, 110);
            this.frmLoopitIterationLabel.Name = "frmLoopitIterationLabel";
            this.frmLoopitIterationLabel.Size = new Size(0x85, 13);
            this.frmLoopitIterationLabel.TabIndex = 13;
            this.frmLoopitIterationLabel.Text = "Iterations (-1 = Continuous)";
            this.frmLoopitSecPerIterationTxtBox.Location = new Point(0x11d, 0x83);
            this.frmLoopitSecPerIterationTxtBox.Name = "frmLoopitSecPerIterationTxtBox";
            this.frmLoopitSecPerIterationTxtBox.Size = new Size(100, 20);
            this.frmLoopitSecPerIterationTxtBox.TabIndex = 2;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12a, 110);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4a, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Secs/Iteration";
            this.frmLoopitAllowEarlyTerminateChkBox.AutoSize = true;
            this.frmLoopitAllowEarlyTerminateChkBox.Location = new Point(0x199, 0x85);
            this.frmLoopitAllowEarlyTerminateChkBox.Name = "frmLoopitAllowEarlyTerminateChkBox";
            this.frmLoopitAllowEarlyTerminateChkBox.Size = new Size(0xad, 0x11);
            this.frmLoopitAllowEarlyTerminateChkBox.TabIndex = 3;
            this.frmLoopitAllowEarlyTerminateChkBox.Text = "Allow Early Iteration Completion";
            this.frmLoopitAllowEarlyTerminateChkBox.UseVisualStyleBackColor = true;
            this.frmLoopitStartBtn.Location = new Point(0x120, 0xcc);
            this.frmLoopitStartBtn.Name = "frmLoopitStartBtn";
            this.frmLoopitStartBtn.Size = new Size(0x4b, 0x17);
            this.frmLoopitStartBtn.TabIndex = 6;
            this.frmLoopitStartBtn.Text = "&Start";
            this.frmLoopitStartBtn.UseVisualStyleBackColor = true;
            this.frmLoopitStartBtn.Click += new EventHandler(this.frmLoopitStartBtn_Click);
            this.frmLoopitCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmLoopitCancelBtn.Location = new Point(380, 0xcc);
            this.frmLoopitCancelBtn.Name = "frmLoopitCancelBtn";
            this.frmLoopitCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmLoopitCancelBtn.TabIndex = 7;
            this.frmLoopitCancelBtn.Text = "&Cancel";
            this.frmLoopitCancelBtn.UseVisualStyleBackColor = true;
            this.frmLoopitCancelBtn.Click += new EventHandler(this.frmLoopitCancelBtn_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x11, 0x70);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x3e, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Reset Type";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x11, 0x22);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x152, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Initialize reset paramters in Rx Commands -> Reset";
            this.frmLoopitSwitchProtocolOnFactoryChkBox.AutoSize = true;
            this.frmLoopitSwitchProtocolOnFactoryChkBox.Location = new Point(20, 0xa6);
            this.frmLoopitSwitchProtocolOnFactoryChkBox.Name = "frmLoopitSwitchProtocolOnFactoryChkBox";
            this.frmLoopitSwitchProtocolOnFactoryChkBox.Size = new Size(0xb7, 0x11);
            this.frmLoopitSwitchProtocolOnFactoryChkBox.TabIndex = 4;
            this.frmLoopitSwitchProtocolOnFactoryChkBox.Text = "Switch Protocol/Baud on Factory";
            this.frmLoopitSwitchProtocolOnFactoryChkBox.UseVisualStyleBackColor = true;
            this.frmLoopitPerformAidingOnFactoryChkBox.AutoSize = true;
            this.frmLoopitPerformAidingOnFactoryChkBox.Location = new Point(20, 0xb7);
            this.frmLoopitPerformAidingOnFactoryChkBox.Name = "frmLoopitPerformAidingOnFactoryChkBox";
            this.frmLoopitPerformAidingOnFactoryChkBox.Size = new Size(0x93, 0x11);
            this.frmLoopitPerformAidingOnFactoryChkBox.TabIndex = 5;
            this.frmLoopitPerformAidingOnFactoryChkBox.Text = "Perform Aiding on Factory";
            this.frmLoopitPerformAidingOnFactoryChkBox.UseVisualStyleBackColor = true;
            base.AcceptButton = this.frmLoopitStartBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmLoopitCancelBtn;
            base.ClientSize = new Size(0x2e7, 0xf2);
            base.Controls.Add(this.frmLoopitPerformAidingOnFactoryChkBox);
            base.Controls.Add(this.frmLoopitSwitchProtocolOnFactoryChkBox);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.frmLoopitCancelBtn);
            base.Controls.Add(this.frmLoopitStartBtn);
            base.Controls.Add(this.frmLoopitAllowEarlyTerminateChkBox);
            base.Controls.Add(this.frmLoopitSecPerIterationTxtBox);
            base.Controls.Add(this.frmLoopitIterationTxtBox);
            base.Controls.Add(this.frmLoopitResetTypeComboBox);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.frmLoopitIterationLabel);
            base.Controls.Add(this.frmLoopitComNameLabel);
            base.Controls.Add(this.frmLoopitNumberComDetectedLabel);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmLoopit";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Loopit";
            base.Load += new EventHandler(this.frmLoopit_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void loadResetType()
        {
            if (clsGlobal.IsMarketingUser())
            {
                this.frmLoopitResetTypeComboBox.Items.AddRange(new object[] { "COLD", "HOT", "WARM_INIT", "WARM_NO_INIT", "FACTORY" });
            }
            else
            {
                this.frmLoopitResetTypeComboBox.Items.AddRange(new object[] { "COLD", "HOT", "WARM_INIT", "WARM_NO_INIT", "FACTORY", "FACTORY_XO", "SLC_HOT", "SLC_COLD", "USER_DEFINED" });
            }
            this.frmLoopitResetTypeComboBox.Text = clsGlobal.LoopitResetType;
        }

        public bool StartLoopit()
        {
            DateTime now = DateTime.Now;
            string str = string.Format("{0:D2}{1:D2}{2:D2}", now.Hour, now.Minute, now.Second);
            string path = clsGlobal.InstalledDirectory + string.Format(@"\Log\{0:D2}{1:D2}{2:D4}", now.Month, now.Day, now.Year);
            string inFilename = string.Empty;
            string filename = string.Empty;
            string str5 = string.Empty;
            foreach (string str6 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
            {
                EventHandler method = null;
                PortManager thisComWin = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str6];
                if (((thisComWin == null) || !CommunicationManager.ValidateCommManager(thisComWin.comm)) || !thisComWin.comm.IsSourceDeviceOpen())
                {
                    continue;
                }
                try
                {
                    thisComWin.comm.m_TestSetup.TestID = string.Format("GPS-LOOPIT-{0}", clsGlobal.LoopitResetType);
                    thisComWin.comm.m_TestSetup.TestName = "GPS-LOOPIT";
                    thisComWin.comm.m_TestSetup.TestDescription = "Loopit Reset Test";
                    thisComWin.comm.m_TestSetup.TestGroup = "Loopit Reset Test";
                    thisComWin.comm.m_TestSetup.DataClass = "GPS Performance Characterization";
                    thisComWin.comm.m_TestSetup.TestOperator = "Guest";
                    thisComWin.comm.m_TestSetup.TestRun = HelperFunctions.GetTimeStampInString();
                    thisComWin.comm.m_TestSetup.StartTime = HelperFunctions.GetTimeStampInString();
                    thisComWin.comm.m_TestSetup.RXProductType = thisComWin.comm.ProductFamily.ToString();
                    thisComWin.comm.m_TestSetup.TTFFLimit = clsGlobal.LoopitTimeout;
                    thisComWin.comm.m_TestSetup.TTFFTimeout = clsGlobal.LoopitTimeout;
                    thisComWin.comm.UpdateTestSetup();
                    if (MessageBox.Show(string.Format("{0}: Log GPS and TTFF/Nav Accuracy data?", thisComWin.comm.PortName), "Log File", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                    {
                        goto Label_067D;
                    }
                    switch (thisComWin.comm.LogFormat)
                    {
                        case CommunicationManager.TransmissionType.Text:
                            thisComWin.comm.Log.IsBin = false;
                            str5 = ".txt";
                            break;

                        case CommunicationManager.TransmissionType.GP2:
                            thisComWin.comm.Log.IsBin = false;
                            str5 = ".gp2";
                            break;

                        case CommunicationManager.TransmissionType.GPS:
                            thisComWin.comm.Log.IsBin = false;
                            str5 = ".gps";
                            break;

                        case CommunicationManager.TransmissionType.Bin:
                            thisComWin.comm.Log.IsBin = true;
                            str5 = ".bin";
                            break;

                        default:
                            str5 = ".log";
                            break;
                    }
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if ((thisComWin.comm.Log.filename != null) && (thisComWin.comm.Log.filename != string.Empty))
                    {
                        if ((TempRType == "") || (TempRType == clsGlobal.LoopitResetType))
                        {
                            inFilename = thisComWin.comm.Log.filename;
                            TempRType = clsGlobal.LoopitResetType;
                            goto Label_053D;
                        }
                        TempRType = clsGlobal.LoopitResetType;
                        string directoryName = string.Empty;
                        Regex regex = new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$");
                        if (!regex.IsMatch(thisComWin.comm.Log.filename))
                        {
                            MessageBox.Show(string.Format("Invalid File Path\n{0}", thisComWin.comm.Log.filename), "Loopit Error Check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return true;
                        }
                        try
                        {
                            FileInfo info = new FileInfo(thisComWin.comm.Log.filename);
                            directoryName = info.DirectoryName;
                            inFilename = string.Format(@"{0}\{1}_{2}_{3}{4}", new object[] { directoryName, str, thisComWin.comm.PortName, clsGlobal.LoopitResetType, str5 });
                            goto Label_053D;
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, "Loopit Error Check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return true;
                        }
                    }
                    TempRType = clsGlobal.LoopitResetType;
                    inFilename = string.Format(@"{0}\{1}_{2}_{3}{4}", new object[] { path, str, thisComWin.comm.PortName, clsGlobal.LoopitResetType, str5 });
                Label_053D:
                    if (!thisComWin.comm.Log.IsFileOpen() && !thisComWin.comm.Log.OpenFile(inFilename))
                    {
                        thisComWin.comm.RxCtrl.ResetCtrl.LoopitInprogress = false;
                        return true;
                    }
                    inFilename = thisComWin.comm.Log.filename;
                    thisComWin.comm.Log.SetDurationLoggingStatusLabel(inFilename);
                    FileInfo info2 = new FileInfo(inFilename);
                    string extension = info2.Extension;
                    if (extension != string.Empty)
                    {
                        filename = inFilename.Replace(extension, "_ttff.csv");
                    }
                    else
                    {
                        filename = inFilename + "_ttff.csv";
                    }
                    if (!thisComWin.comm.RxCtrl.ResetCtrl.OpenTTFFLog(filename))
                    {
                        thisComWin.comm.RxCtrl.ResetCtrl.LoopitInprogress = false;
                        return true;
                    }
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    thisComWin.comm.RxCtrl.ResetCtrl.LoopitInprogress = false;
                    return true;
                }
            Label_067D:
                thisComWin.comm.RxCtrl.ResetCtrl.ResetInterval = clsGlobal.LoopitTimeout;
                thisComWin.comm.RxCtrl.ResetCtrl.TotalNumberOfResets = clsGlobal.LoopitIteration;
                thisComWin.comm.RxCtrl.ResetCtrl.ResetEarlyTerminate = clsGlobal.LoopitEarlyTermination;
                thisComWin.comm.RxCtrl.ResetCtrl.ResetType = clsGlobal.LoopitResetType;
                thisComWin.comm.RxCtrl.ResetCtrl.IsAidingPerformedOnFactory = clsGlobal.LoopitPerformedAidingOnFactory;
                thisComWin.comm.RxCtrl.ResetCtrl.IsProtocolSwitchedOnFactory = clsGlobal.LoopitSwitchProtocolNBaudOnFactory;
                if ((thisComWin.comm.ProductFamily != CommonClass.ProductType.GSD4e) && (this.frmLoopitResetTypeComboBox.Text == "FACTORY"))
                {
                    thisComWin.comm.RxCtrl.ResetCtrl.IsProtocolSwitchedOnFactory = false;
                }
                thisComWin.comm.RxCtrl.ResetCtrl.LoopitInprogress = true;
                if ((thisComWin.DebugView != null) && thisComWin.DebugViewLocation.IsOpen)
                {
                    if (method == null)
                    {
                        method = delegate {
                            thisComWin.DebugView.Text = "Loopit -- " + thisComWin.comm.WindowTitle;
                        };
                    }
                    thisComWin.DebugView.BeginInvoke(method);
                }
                thisComWin.comm.RxCtrl.ResetCtrl.ResetTimerStart(true);
            }
            return true;
        }

        private void updateConfigList(string updateData)
        {
            this.frmLoopitResetTypeComboBox.Items.Add(updateData);
            this.frmLoopitResetTypeComboBox.Text = updateData;
        }
    }
}

