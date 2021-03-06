﻿namespace SiRFLive.GUI
{
    using LogManagerClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frmLogDuration : Form
    {
        private CommunicationManager _comm;
        private LogManager _Log;
        private IContainer components;
        private CheckBox DelayedStartCheckBox;
        private CheckBox durationLogCheckBox;
        private GroupBox DurationLoggingGroupBox;
        private int durationMinutes;
        private NumericUpDown DurationNumericUpDown;
        private DateTime endDate;
        private DateTimePicker EndDateDateTimePicker;
        private DateTime endTime;
        private DateTimePicker EndTimeDateTimePicker;
        private const int InitialDuration = 60;
        private CheckBox islogUserSpecMsgChkBox;
        private bool keepOpen;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button LogDurationCancelButton;
        private Button LogDurationClearButton;
        private Button LogDurationStartButton;
        private Button LogDurationUpdateButton;
        private Button LogDurationUserSpecButton;
        private Button logFileBroswerBtn;
        private Label logFilePathLabel;
        private TextBox logFilePathTextBox;
        private ComboBox logTypeComboBox;
        private Label logTypeLabel;
        private DateTime startDate;
        private DateTimePicker StartDateDateTimePicker;
        private DateTime startTime;
        private DateTimePicker StartTimeDateTimePicker;

        public frmLogDuration(CommunicationManager comm)
        {
            this.InitializeComponent();
            this._comm = comm;
            this._Log = this._comm.Log;
        }

        private int ComputeDuration()
        {
            TimeSpan span = new TimeSpan(0, 0, 0);
            this.startTime = this.StartTimeDateTimePicker.Value;
            this.endTime = this.EndTimeDateTimePicker.Value;
            span = (TimeSpan) (this.endTime - this.startTime);
            int num = (int) Math.Ceiling(span.TotalMinutes);
            this.startDate = this.StartDateDateTimePicker.Value;
            this.endDate = this.EndDateDateTimePicker.Value;
            if ((this.endDate - this.startDate) > TimeSpan.Zero)
            {
                TimeSpan span2 = (TimeSpan) (this.endDate - this.startDate);
                num += (span2.Days * 0x18) * 60;
            }
            return num;
        }

        private void ComputeStartAndEndTimeFromDuration(TimeSpan duration, DateTime startT, DateTime startDt, ref DateTime endT, ref DateTime endDt)
        {
            endT = startT + duration;
            endDt = endT;
        }

        private bool createDurationLogs(LogManager targetLog)
        {
            DateTime time = new DateTime(this.EndDateDateTimePicker.Value.Year, this.EndDateDateTimePicker.Value.Month, this.EndDateDateTimePicker.Value.Day, this.EndTimeDateTimePicker.Value.Hour, this.EndTimeDateTimePicker.Value.Minute, this.EndTimeDateTimePicker.Value.Second);
            targetLog.DurationLoggingStopTime = time;
            targetLog.LoggingState = LogManager.LoggingStates.duration_logging;
            targetLog.DurationLoggingStatusLabel.Text = string.Format("Logging Stop Time: {0} -- {1}", targetLog.DurationLoggingStopTime.ToString(), targetLog.filename);
            if (this.DelayedStartCheckBox.Checked)
            {
                targetLog.DelayedLoggingStartTime = this.StartTimeDateTimePicker.Value;
                targetLog.LoggingState = LogManager.LoggingStates.delayed_logging;
                targetLog.DurationLoggingStatusLabel.Text = string.Format("Delayed Logging Start Time: {0} -- {1}", targetLog.DelayedLoggingStartTime.ToString(), targetLog.filename);
            }
            return targetLog.OpenFile();
        }

        private void createLogFiles(string dirPath)
        {
            this._Log.DurationLoggingStatusLabel.Text = dirPath;
            foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
            {
                PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                if ((manager != null) && (manager.comm != null))
                {
                    string str2 = "log";
                    switch (manager.comm.LogFormat)
                    {
                        case CommunicationManager.TransmissionType.Text:
                            str2 = "txt";
                            break;

                        case CommunicationManager.TransmissionType.Hex:
                            str2 = "hex";
                            break;

                        case CommunicationManager.TransmissionType.SSB:
                            str2 = "hex";
                            break;

                        case CommunicationManager.TransmissionType.GP2:
                            str2 = "gp2";
                            break;

                        case CommunicationManager.TransmissionType.GPS:
                            str2 = "gps";
                            break;

                        case CommunicationManager.TransmissionType.Bin:
                            str2 = "bin";
                            break;
                    }
                    if (manager.comm.Log.IsFileOpen())
                    {
                        manager.comm.Log.CloseFile();
                    }
                    DateTime now = DateTime.Now;
                    string str3 = string.Format("{0:D2}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D4}_{6}.{7}", new object[] { now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second, str, str2 });
                    manager.comm.Log.filename = dirPath + @"\" + str3;
                }
            }
        }

        private void DelayedStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DelayedStartCheckBox.CheckState == CheckState.Checked)
            {
                this.StartTimeDateTimePicker.Enabled = true;
                this.StartDateDateTimePicker.Enabled = true;
            }
            else
            {
                this.StartTimeDateTimePicker.Enabled = false;
                this.StartDateDateTimePicker.Enabled = false;
            }
        }

        private void DisableEndDateTimePickers()
        {
            this.EndDateDateTimePicker.ValueChanged -= new EventHandler(this.EndDateDateTimePicker_ValueChanged);
            this.EndTimeDateTimePicker.ValueChanged -= new EventHandler(this.EndTimeDateTimePicker_ValueChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void durationLogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.enableDisableDurationLog();
        }

        private void DurationNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            int minutes = (int) this.DurationNumericUpDown.Value;
            TimeSpan duration = new TimeSpan(0, minutes, 0);
            this.ComputeStartAndEndTimeFromDuration(duration, this.startTime, this.startDate, ref this.endTime, ref this.endDate);
            this.DisableEndDateTimePickers();
            this.EndTimeDateTimePicker.Value = this.endTime;
            this.EndDateDateTimePicker.Value = this.endDate;
            this.EnableEndDateTimePickers();
        }

        private void enableDisableDurationLog()
        {
            if (this.durationLogCheckBox.Checked)
            {
                this.DurationLoggingGroupBox.Enabled = true;
            }
            else
            {
                this.DurationLoggingGroupBox.Enabled = false;
            }
        }

        private void EnableEndDateTimePickers()
        {
            this.EndDateDateTimePicker.ValueChanged += new EventHandler(this.EndDateDateTimePicker_ValueChanged);
            this.EndTimeDateTimePicker.ValueChanged += new EventHandler(this.EndTimeDateTimePicker_ValueChanged);
        }

        private void EndDateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.durationMinutes = this.ComputeDuration();
            this.SetDurationNumericUpDown(this.durationMinutes);
        }

        private void EndTimeDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.durationMinutes = this.ComputeDuration();
            this.SetDurationNumericUpDown(this.durationMinutes);
        }

        private void frmLogDuration_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime time2 = now + new TimeSpan(0, 60, 0);
            this.StartTimeDateTimePicker.Value = now;
            this.StartDateDateTimePicker.Value = now.Date;
            this.EndTimeDateTimePicker.Value = time2;
            this.EndDateDateTimePicker.Value = time2.Date;
            this.StartTimeDateTimePicker.Enabled = false;
            this.StartDateDateTimePicker.Enabled = false;
            this.DelayedStartCheckBox.Checked = false;
            this.enableDisableDurationLog();
            switch (this._comm.LogFormat)
            {
                case CommunicationManager.TransmissionType.Text:
                    this.logTypeComboBox.SelectedIndex = 2;
                    break;

                case CommunicationManager.TransmissionType.GP2:
                    this.logTypeComboBox.SelectedIndex = 1;
                    break;

                case CommunicationManager.TransmissionType.GPS:
                    this.logTypeComboBox.SelectedIndex = 0;
                    break;

                case CommunicationManager.TransmissionType.Bin:
                    this.logTypeComboBox.SelectedIndex = 3;
                    break;

                default:
                    this.logTypeComboBox.SelectedIndex = 0;
                    break;
            }
            if (this._comm.RxTransType == CommunicationManager.TransmissionType.Text)
            {
                this.logTypeComboBox.Enabled = false;
            }
            else
            {
                this.logTypeComboBox.Enabled = true;
            }
            if (clsGlobal.PerformOnAll)
            {
                this.logFileBroswerBtn.Click += new EventHandler(this.logFileDirBroswerBtn_Click);
                if (this._comm != null)
                {
                    if ((this._comm.Log.filename != null) && (this._comm.Log.filename != string.Empty))
                    {
                        FileInfo info = new FileInfo(this._comm.Log.filename);
                        string directoryName = info.DirectoryName;
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        this.logFilePathTextBox.Text = directoryName;
                    }
                    else
                    {
                        this.logFilePathTextBox.Text = string.Empty;
                    }
                    this.islogUserSpecMsgChkBox.Checked = this._comm.IsUserSpecifiedMsgLog;
                }
            }
            else
            {
                this.logFileBroswerBtn.Click += new EventHandler(this.logFileBroswerBtn_Click);
                if (this._comm != null)
                {
                    this.logFilePathTextBox.Text = this._comm.Log.filename;
                    this.islogUserSpecMsgChkBox.Checked = this._comm.IsUserSpecifiedMsgLog;
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmLogDuration));
            this.DurationLoggingGroupBox = new GroupBox();
            this.DurationNumericUpDown = new NumericUpDown();
            this.DelayedStartCheckBox = new CheckBox();
            this.label4 = new Label();
            this.label3 = new Label();
            this.EndDateDateTimePicker = new DateTimePicker();
            this.EndTimeDateTimePicker = new DateTimePicker();
            this.label2 = new Label();
            this.label1 = new Label();
            this.StartTimeDateTimePicker = new DateTimePicker();
            this.StartDateDateTimePicker = new DateTimePicker();
            this.LogDurationStartButton = new Button();
            this.LogDurationCancelButton = new Button();
            this.logFileBroswerBtn = new Button();
            this.logFilePathLabel = new Label();
            this.logFilePathTextBox = new TextBox();
            this.durationLogCheckBox = new CheckBox();
            this.logTypeComboBox = new ComboBox();
            this.logTypeLabel = new Label();
            this.LogDurationClearButton = new Button();
            this.LogDurationUpdateButton = new Button();
            this.LogDurationUserSpecButton = new Button();
            this.islogUserSpecMsgChkBox = new CheckBox();
            this.DurationLoggingGroupBox.SuspendLayout();
            this.DurationNumericUpDown.BeginInit();
            base.SuspendLayout();
            this.DurationLoggingGroupBox.Controls.Add(this.DurationNumericUpDown);
            this.DurationLoggingGroupBox.Controls.Add(this.DelayedStartCheckBox);
            this.DurationLoggingGroupBox.Controls.Add(this.label4);
            this.DurationLoggingGroupBox.Controls.Add(this.label3);
            this.DurationLoggingGroupBox.Controls.Add(this.EndDateDateTimePicker);
            this.DurationLoggingGroupBox.Controls.Add(this.EndTimeDateTimePicker);
            this.DurationLoggingGroupBox.Controls.Add(this.label2);
            this.DurationLoggingGroupBox.Controls.Add(this.label1);
            this.DurationLoggingGroupBox.Controls.Add(this.StartTimeDateTimePicker);
            this.DurationLoggingGroupBox.Controls.Add(this.StartDateDateTimePicker);
            this.DurationLoggingGroupBox.Location = new Point(10, 140);
            this.DurationLoggingGroupBox.Name = "DurationLoggingGroupBox";
            this.DurationLoggingGroupBox.Size = new Size(0x159, 0x95);
            this.DurationLoggingGroupBox.TabIndex = 0;
            this.DurationLoggingGroupBox.TabStop = false;
            this.DurationLoggingGroupBox.Text = "Duration Logging";
            this.DurationNumericUpDown.Location = new Point(0x57, 0x67);
            int[] bits = new int[4];
            bits[0] = 0x270f;
            this.DurationNumericUpDown.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.DurationNumericUpDown.Minimum = new decimal(numArray2);
            this.DurationNumericUpDown.Name = "DurationNumericUpDown";
            this.DurationNumericUpDown.Size = new Size(0x5c, 20);
            this.DurationNumericUpDown.TabIndex = 10;
            int[] numArray3 = new int[4];
            numArray3[0] = 60;
            this.DurationNumericUpDown.Value = new decimal(numArray3);
            this.DurationNumericUpDown.ValueChanged += new EventHandler(this.DurationNumericUpDown_ValueChanged);
            this.DelayedStartCheckBox.AutoSize = true;
            this.DelayedStartCheckBox.Location = new Point(0x57, 0x19);
            this.DelayedStartCheckBox.Name = "DelayedStartCheckBox";
            this.DelayedStartCheckBox.Size = new Size(90, 0x11);
            this.DelayedStartCheckBox.TabIndex = 9;
            this.DelayedStartCheckBox.Text = "Delayed Start";
            this.DelayedStartCheckBox.UseVisualStyleBackColor = true;
            this.DelayedStartCheckBox.CheckedChanged += new EventHandler(this.DelayedStartCheckBox_CheckedChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xb3, 0x6b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x2c, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Minutes";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x26, 0x6b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x2f, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Duration";
            this.EndDateDateTimePicker.Format = DateTimePickerFormat.Short;
            this.EndDateDateTimePicker.Location = new Point(0xc6, 0x4f);
            this.EndDateDateTimePicker.MinDate = new DateTime(0x7d9, 7, 0x1d, 0, 0, 0, 0);
            this.EndDateDateTimePicker.Name = "EndDateDateTimePicker";
            this.EndDateDateTimePicker.Size = new Size(0x76, 20);
            this.EndDateDateTimePicker.TabIndex = 5;
            this.EndDateDateTimePicker.ValueChanged += new EventHandler(this.EndDateDateTimePicker_ValueChanged);
            this.EndTimeDateTimePicker.Format = DateTimePickerFormat.Time;
            this.EndTimeDateTimePicker.Location = new Point(0x57, 0x4f);
            this.EndTimeDateTimePicker.Name = "EndTimeDateTimePicker";
            this.EndTimeDateTimePicker.ShowUpDown = true;
            this.EndTimeDateTimePicker.Size = new Size(0x5c, 20);
            this.EndTimeDateTimePicker.TabIndex = 4;
            this.EndTimeDateTimePicker.ValueChanged += new EventHandler(this.EndTimeDateTimePicker_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x21, 0x53);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Time";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(30, 0x34);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Time";
            this.StartTimeDateTimePicker.Format = DateTimePickerFormat.Time;
            this.StartTimeDateTimePicker.Location = new Point(0x57, 0x30);
            this.StartTimeDateTimePicker.Name = "StartTimeDateTimePicker";
            this.StartTimeDateTimePicker.ShowUpDown = true;
            this.StartTimeDateTimePicker.Size = new Size(0x5c, 20);
            this.StartTimeDateTimePicker.TabIndex = 1;
            this.StartTimeDateTimePicker.ValueChanged += new EventHandler(this.StartTimeDateTimePicker_ValueChanged);
            this.StartDateDateTimePicker.Format = DateTimePickerFormat.Short;
            this.StartDateDateTimePicker.Location = new Point(0xc6, 0x30);
            this.StartDateDateTimePicker.MinDate = new DateTime(0x7d9, 7, 0x1d, 0, 0, 0, 0);
            this.StartDateDateTimePicker.Name = "StartDateDateTimePicker";
            this.StartDateDateTimePicker.Size = new Size(0x76, 20);
            this.StartDateDateTimePicker.TabIndex = 0;
            this.StartDateDateTimePicker.ValueChanged += new EventHandler(this.StartDateDateTimePicker_ValueChanged);
            this.LogDurationStartButton.Location = new Point(0x65, 0x133);
            this.LogDurationStartButton.Name = "LogDurationStartButton";
            this.LogDurationStartButton.Size = new Size(0x4b, 0x17);
            this.LogDurationStartButton.TabIndex = 2;
            this.LogDurationStartButton.Text = "&Start ";
            this.LogDurationStartButton.UseVisualStyleBackColor = true;
            this.LogDurationStartButton.Click += new EventHandler(this.LogDurationStartButton_Click);
            this.LogDurationCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LogDurationCancelButton.Location = new Point(0xc0, 0x133);
            this.LogDurationCancelButton.Name = "LogDurationCancelButton";
            this.LogDurationCancelButton.Size = new Size(0x4b, 0x17);
            this.LogDurationCancelButton.TabIndex = 4;
            this.LogDurationCancelButton.Text = "&Cancel";
            this.LogDurationCancelButton.UseVisualStyleBackColor = true;
            this.LogDurationCancelButton.Click += new EventHandler(this.LogDurationCancelButton_Click);
            this.logFileBroswerBtn.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.logFileBroswerBtn.Location = new Point(0x145, 0x30);
            this.logFileBroswerBtn.Name = "logFileBroswerBtn";
            this.logFileBroswerBtn.Size = new Size(0x1f, 0x17);
            this.logFileBroswerBtn.TabIndex = 5;
            this.logFileBroswerBtn.Text = "...";
            this.logFileBroswerBtn.UseVisualStyleBackColor = true;
            this.logFilePathLabel.AutoSize = true;
            this.logFilePathLabel.BackColor = SystemColors.Control;
            this.logFilePathLabel.Location = new Point(0x13, 0x35);
            this.logFilePathLabel.Name = "logFilePathLabel";
            this.logFilePathLabel.Size = new Size(0x45, 13);
            this.logFilePathLabel.TabIndex = 6;
            this.logFilePathLabel.Text = "Log File Path";
            this.logFilePathTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.logFilePathTextBox.Location = new Point(90, 0x31);
            this.logFilePathTextBox.Name = "logFilePathTextBox";
            this.logFilePathTextBox.Size = new Size(0xe5, 20);
            this.logFilePathTextBox.TabIndex = 7;
            this.durationLogCheckBox.AutoSize = true;
            this.durationLogCheckBox.Location = new Point(0x13, 0x6f);
            this.durationLogCheckBox.Name = "durationLogCheckBox";
            this.durationLogCheckBox.Size = new Size(0x6b, 0x11);
            this.durationLogCheckBox.TabIndex = 8;
            this.durationLogCheckBox.Text = "Duration Logging";
            this.durationLogCheckBox.UseVisualStyleBackColor = true;
            this.durationLogCheckBox.CheckedChanged += new EventHandler(this.durationLogCheckBox_CheckedChanged);
            this.logTypeComboBox.FormattingEnabled = true;
            this.logTypeComboBox.Items.AddRange(new object[] { "GPS", "GP2", "TEXT", "BIN" });
            this.logTypeComboBox.Location = new Point(0xea, 0x6d);
            this.logTypeComboBox.Name = "logTypeComboBox";
            this.logTypeComboBox.Size = new Size(0x79, 0x15);
            this.logTypeComboBox.TabIndex = 9;
            this.logTypeComboBox.SelectedIndexChanged += new EventHandler(this.logTypeComboBox_SelectedIndexChanged);
            this.logTypeLabel.AutoSize = true;
            this.logTypeLabel.Location = new Point(0xac, 0x71);
            this.logTypeLabel.Name = "logTypeLabel";
            this.logTypeLabel.Size = new Size(60, 13);
            this.logTypeLabel.TabIndex = 10;
            this.logTypeLabel.Text = "Log Format";
            this.LogDurationClearButton.Location = new Point(0x13, 14);
            this.LogDurationClearButton.Name = "LogDurationClearButton";
            this.LogDurationClearButton.Size = new Size(0x66, 0x17);
            this.LogDurationClearButton.TabIndex = 11;
            this.LogDurationClearButton.Text = "Clear Log Path";
            this.LogDurationClearButton.UseVisualStyleBackColor = true;
            this.LogDurationClearButton.Click += new EventHandler(this.LogDurationClearButton_Click);
            this.LogDurationUpdateButton.Location = new Point(0x7f, 14);
            this.LogDurationUpdateButton.Name = "LogDurationUpdateButton";
            this.LogDurationUpdateButton.Size = new Size(0x66, 0x17);
            this.LogDurationUpdateButton.TabIndex = 12;
            this.LogDurationUpdateButton.Text = "Update Log Path";
            this.LogDurationUpdateButton.UseVisualStyleBackColor = true;
            this.LogDurationUpdateButton.Click += new EventHandler(this.LogDurationUpdateButton_Click);
            this.LogDurationUserSpecButton.Location = new Point(0xeb, 14);
            this.LogDurationUserSpecButton.Name = "LogDurationUserSpecButton";
            this.LogDurationUserSpecButton.Size = new Size(120, 0x17);
            this.LogDurationUserSpecButton.TabIndex = 13;
            this.LogDurationUserSpecButton.Text = "Config Log Message";
            this.LogDurationUserSpecButton.UseVisualStyleBackColor = true;
            this.LogDurationUserSpecButton.Click += new EventHandler(this.LogDurationUserSpecButton_Click);
            this.islogUserSpecMsgChkBox.AutoSize = true;
            this.islogUserSpecMsgChkBox.Location = new Point(0x13, 0x53);
            this.islogUserSpecMsgChkBox.Name = "islogUserSpecMsgChkBox";
            this.islogUserSpecMsgChkBox.Size = new Size(0xa7, 0x11);
            this.islogUserSpecMsgChkBox.TabIndex = 14;
            this.islogUserSpecMsgChkBox.Text = "Log User Specified Messages";
            this.islogUserSpecMsgChkBox.UseVisualStyleBackColor = true;
            this.islogUserSpecMsgChkBox.CheckedChanged += new EventHandler(this.islogUserSpecMsgChkBox_CheckedChanged);
            base.AcceptButton = this.LogDurationStartButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.LogDurationCancelButton;
            base.ClientSize = new Size(0x170, 0x158);
            base.Controls.Add(this.islogUserSpecMsgChkBox);
            base.Controls.Add(this.LogDurationUserSpecButton);
            base.Controls.Add(this.LogDurationUpdateButton);
            base.Controls.Add(this.LogDurationClearButton);
            base.Controls.Add(this.logTypeLabel);
            base.Controls.Add(this.logTypeComboBox);
            base.Controls.Add(this.durationLogCheckBox);
            base.Controls.Add(this.logFilePathTextBox);
            base.Controls.Add(this.logFilePathLabel);
            base.Controls.Add(this.logFileBroswerBtn);
            base.Controls.Add(this.LogDurationCancelButton);
            base.Controls.Add(this.LogDurationStartButton);
            base.Controls.Add(this.DurationLoggingGroupBox);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmLogDuration";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Log To File";
            base.Load += new EventHandler(this.frmLogDuration_Load);
            this.DurationLoggingGroupBox.ResumeLayout(false);
            this.DurationLoggingGroupBox.PerformLayout();
            this.DurationNumericUpDown.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void islogUserSpecMsgChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && (manager.comm != null))
                    {
                        manager.comm.IsUserSpecifiedMsgLog = this.islogUserSpecMsgChkBox.Checked;
                    }
                }
            }
            else
            {
                this._comm.IsUserSpecifiedMsgLog = this.islogUserSpecMsgChkBox.Checked;
            }
        }

        private void LogDurationCancelButton_Click(object sender, EventArgs e)
        {
			this._Log.DurationLoggingStatusLabel.BeginInvoke((MethodInvoker)delegate
			{
                this._Log.DurationLoggingStatusLabel.Text = "";
            });
            clsGlobal.PerformOnAll = false;
            base.Close();
        }

        private void LogDurationClearButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("{0}\n{1}", "Clearing log path will terminate logging", "Proceed?"), "Log", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.setLogPath(string.Empty);
            }
            base.Close();
        }

        private void LogDurationStartButton_Click(object sender, EventArgs e)
        {
            this.keepOpen = false;
            if (this.logFilePathTextBox.Text == string.Empty)
            {
                clsGlobal.PerformOnAll = false;
                MessageBox.Show("File path can not be blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (clsGlobal.PerformOnAll)
                {
                    this.createLogFiles(this.logFilePathTextBox.Text);
                    if (this.durationLogCheckBox.Checked)
                    {
                        foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                        {
                            PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                            if (((manager != null) && (manager.comm != null)) && !this.createDurationLogs(manager.comm.Log))
                            {
                                this.keepOpen = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (string str2 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                        {
                            PortManager manager2 = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str2];
                            if (manager2 != null)
                            {
                                if (!manager2.comm.Log.OpenFile())
                                {
                                    this.keepOpen = true;
                                    return;
                                }
                                if ((manager2.comm != null) && manager2.comm.IsSourceDeviceOpen())
                                {
                                    manager2.comm.RxCtrl.PollSWVersion();
                                    manager2.comm.RxCtrl.PollNavigationParameters();
                                }
                            }
                        }
                    }
                    this._Log.RunCallBackFnc();
                }
                else if ((this._comm != null) && (this._Log != null))
                {
                    this._Log.filename = this.logFilePathTextBox.Text;
                    if (this.durationLogCheckBox.Checked)
                    {
                        this.createDurationLogs(this._Log);
                    }
                    else if (this._Log.OpenFile())
                    {
                        this._Log.DurationLoggingStatusLabel.Text = this._Log.filename;
                        if (this._comm.IsSourceDeviceOpen())
                        {
                            this._comm.RxCtrl.PollSWVersion();
                            this._comm.RxCtrl.PollNavigationParameters();
                        }
                    }
                    else
                    {
                        this.keepOpen = true;
                        return;
                    }
                    this._Log.RunCallBackFnc();
                }
                clsGlobal.PerformOnAll = false;
                base.Close();
            }
        }

        private void LogDurationStartButton_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = this.keepOpen;
        }

        private void LogDurationUpdateButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("{0}\n{1}", "Updating log path will terminate logging", "Proceed?"), "Log", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.setLogPath(this.logFilePathTextBox.Text);
            }
            base.Close();
        }

        private void LogDurationUserSpecButton_Click(object sender, EventArgs e)
        {
            if (this._comm != null)
            {
                string str = string.Format("{0}: Set User Log Messages", this._comm.PortName);
                frmErrorLogConfig config = new frmErrorLogConfig(this._comm, 1);
                config.Text = str;
                config.ShowDialog();
            }
        }

        private void logFileBroswerBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Specify log file name:";
            dialog.InitialDirectory = @"..\..\logs\";
            dialog.Filter = "GPS files (*.gps)|*.gps|GP2 files (*.gp2)|*.gp2|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (this._comm.RxTransType == CommunicationManager.TransmissionType.GPS)
            {
                dialog.FilterIndex = 1;
            }
            else if (this._comm.RxTransType == CommunicationManager.TransmissionType.GP2)
            {
                dialog.FilterIndex = 2;
            }
            else if (this._comm.RxTransType == CommunicationManager.TransmissionType.Text)
            {
                dialog.FilterIndex = 3;
            }
            else
            {
                dialog.FilterIndex = 4;
            }
            dialog.CheckPathExists = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.logFilePathTextBox.Text = dialog.FileName;
                if (this._comm != null)
                {
                    this._comm.Log.filename = this.logFilePathTextBox.Text;
                }
            }
        }

        private void logFileDirBroswerBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.logFilePathTextBox.Text = dialog.SelectedPath;
                this.createLogFiles(this.logFilePathTextBox.Text);
            }
        }

        private void logTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (str != clsGlobal.FilePlayBackPortName)
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                        if ((manager != null) && (manager.comm != null))
                        {
                            switch (this.logTypeComboBox.SelectedIndex)
                            {
                                case 0:
                                {
                                    manager.comm.LogFormat = CommunicationManager.TransmissionType.GPS;
                                    manager.comm.Log.IsBin = false;
                                    continue;
                                }
                                case 1:
                                {
                                    manager.comm.LogFormat = CommunicationManager.TransmissionType.GP2;
                                    manager.comm.Log.IsBin = false;
                                    continue;
                                }
                                case 2:
                                {
                                    manager.comm.LogFormat = CommunicationManager.TransmissionType.Text;
                                    manager.comm.Log.IsBin = false;
                                    continue;
                                }
                                case 3:
                                {
                                    manager.comm.LogFormat = CommunicationManager.TransmissionType.Bin;
                                    manager.comm.Log.IsBin = true;
                                    continue;
                                }
                            }
                            manager.comm.LogFormat = CommunicationManager.TransmissionType.GPS;
                            manager.comm.Log.IsBin = false;
                        }
                    }
                }
            }
            else if (this._comm != null)
            {
                switch (this.logTypeComboBox.SelectedIndex)
                {
                    case 0:
                        this._comm.LogFormat = CommunicationManager.TransmissionType.GPS;
                        this._comm.Log.IsBin = false;
                        return;

                    case 1:
                        this._comm.LogFormat = CommunicationManager.TransmissionType.GP2;
                        this._comm.Log.IsBin = false;
                        return;

                    case 2:
                        this._comm.LogFormat = CommunicationManager.TransmissionType.Text;
                        this._comm.Log.IsBin = false;
                        return;

                    case 3:
                        this._comm.LogFormat = CommunicationManager.TransmissionType.Bin;
                        this._comm.Log.IsBin = true;
                        return;
                }
                this._comm.LogFormat = CommunicationManager.TransmissionType.GPS;
                this._comm.Log.IsBin = false;
            }
        }

        private void SetDurationNumericUpDown(int val)
        {
            try
            {
                this.DurationNumericUpDown.ValueChanged -= new EventHandler(this.DurationNumericUpDown_ValueChanged);
                this.DurationNumericUpDown.Value = val;
                this.DurationNumericUpDown.ValueChanged += new EventHandler(this.DurationNumericUpDown_ValueChanged);
            }
            catch
            {
                this.DurationNumericUpDown.Value = 60M;
            }
        }

        private void setLogPath(string newLogPath)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if (manager != null)
                    {
                        if (manager.comm != null)
                        {
                            if (manager.comm.Log.IsFileOpen())
                            {
                                manager.comm.Log.CloseFile();
                            }
                            manager.comm.Log.filename = newLogPath;
                        }
                        this._Log.RunCallBackFnc();
                    }
                }
            }
            else
            {
                if ((this._comm != null) && (this._Log != null))
                {
                    if (this._Log.IsFileOpen())
                    {
                        this._Log.CloseFile();
                    }
                    this._Log.filename = newLogPath;
                }
                this._Log.RunCallBackFnc();
            }
            clsGlobal.PerformOnAll = false;
        }

        private void StartDateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.durationMinutes = this.ComputeDuration();
            this.SetDurationNumericUpDown(this.durationMinutes);
        }

        private void StartTimeDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.durationMinutes = this.ComputeDuration();
            this.SetDurationNumericUpDown(this.durationMinutes);
        }
    }
}

