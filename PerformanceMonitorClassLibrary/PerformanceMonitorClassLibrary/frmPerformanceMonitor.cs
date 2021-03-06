﻿namespace PerformanceMonitorClassLibrary
{
    using Microsoft.VisualBasic.Devices;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Management;
    using System.Windows.Forms;
    using System.Xml;

    public class frmPerformanceMonitor : Form
    {
        private static string _PerfMonRestoredFilePath;
        private static bool _PerformanceParametersRestoredFlag;
        private static bool _PerfTRigger_SiRFLiveCPUUsage;
        private static bool _PerfTrigger_SiRFLivePhysMemory;
        private static bool _PerfTrigger_TimeInterval;
        private static bool _PerfTrigger_TotalCPUUsage;
        private static bool _PerfTrigger_TotalPhysMemory;
        private static bool _PerfTrigger_VirtualMemoryUsage;
        private static double _PhysicalMemoryUsagePercentage;
        private static double _SiRFLiveCPUUsagePercentage;
        private static double _SiRFLivePhysicalMemoryUsagePercentage;
        private static uint _TimeInterval;
        private static double _TotalCPUUsagePercentage;
        private static double _VirtualMemoryUsagePercentage;
        private Button btn_logFileBroswer;
        private IContainer components;
        private string CPU_Asterisk = string.Empty;
        private DirectoryInfo currentDirInfo;
        private DateTime CurrentTime = new DateTime();
        private TimeSpan ElapsedTime = new TimeSpan(0, 0, 0);
        private Label ElapsedTimeLabel;
        private ToolStripDropDownButton frmCommOpenToolFilter;
        private ToolStripSplitButton frmCommOpenToolLog;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private string InstalledDirectory;
        private Label label_ProcessCPUUsage;
        private Label label_ProcessMemoryUsageRatio;
        private Label label_ProcessVirtualMemoryUsage;
        private Label label_TotalCPUUsage;
        private Label label_TotalMemoryUsage;
        private Label label_TotalMemoryUsageRatio;
        private Label label_TotalVirtualMemoryUsage;
        private Label label_TotalVirtualMemoryUsageRatio;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DateTime LastTimerTick = new DateTime();
        private string line = string.Empty;
        private ListBox LogFilePathListBox;
        private DateTime LogStartTime = new DateTime();
        private string Memory_Asterisk = string.Empty;
        private ToolStripMenuItem modifyPerformanceLoggingConditionsToolStripMenuItem;
        private int NumberOfProcessors;
        private string PerfLogFullPathName;
        private PerfLoggingStates PerfLoggingState;
        private OpenFileDialog PerfLogOpenFileDialog;
        private string PerfLogPathDir;
        private static StreamWriter PerfLogSW;
        private static frmPerformanceMonitor perfmonFormHandle;
        private Label performaceLogStatusLabel;
        private PerformanceCounter performanceCounter_ProcessCPUUsage;
        private PerformanceCounter performanceCounter_TotalCPUUsage;
        private ToolStripMenuItem performanceMonitorMenuItem;
        private ProgressBar progressBar_ProcessCPUUsage;
        private ProgressBar progressBar_ProcessMemoryUsage;
        private ProgressBar progressBar_TotalCPUUsage;
        private ProgressBar progressBar_TotalMemoryUsage;
        private ProgressBar progressBar_TotalVirtualMemoryUsage;
        private string SiRFLiveCPU_Asterisk = string.Empty;
        private string SiRFLiveMemory_Asterisk = string.Empty;
        private ToolStripMenuItem startLogToolStripMenuItem;
        private ToolStripMenuItem stopLogToolStripMenuItem;
        private Timer timer1;
        private ToolStrip toolStrip1;
        private string VirtualMemory_Asterisk = string.Empty;

        public frmPerformanceMonitor()
        {
            Application.DoEvents();
            this.InitializeComponent();
            perfmonFormHandle = this;
            this.currentDirInfo = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
            this.InstalledDirectory = this.currentDirInfo.Parent.FullName;
            _PerfMonRestoredFilePath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Config\PerformanceMonitorRestore.xml";
            this.NumberOfProcessors = Convert.ToInt32(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
            if (this.NumberOfProcessors == 0)
            {
                this.NumberOfProcessors = 1;
            }
            this.timer1.Interval = 0x3e8;
            this.timer1.Start();
            this.performanceCounter_TotalCPUUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            this.performanceCounter_ProcessCPUUsage = new PerformanceCounter("Process", "% Processor Time", "_Total");
        }

        private string CreatePerformanceLogFileName()
        {
            DateTime now = DateTime.Now;
            string str = "PerfLog_";
            string str2 = string.Format("{0:ddMMyyyy_hhmm}", now);
            return (str + str2 + ".txt");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmPerformanceMonitor));
            this.groupBox1 = new GroupBox();
            this.label_ProcessCPUUsage = new Label();
            this.label_TotalCPUUsage = new Label();
            this.progressBar_ProcessCPUUsage = new ProgressBar();
            this.progressBar_TotalCPUUsage = new ProgressBar();
            this.label2 = new Label();
            this.label1 = new Label();
            this.groupBox2 = new GroupBox();
            this.label_ProcessMemoryUsageRatio = new Label();
            this.label_TotalMemoryUsageRatio = new Label();
            this.label_ProcessVirtualMemoryUsage = new Label();
            this.label_TotalMemoryUsage = new Label();
            this.progressBar_ProcessMemoryUsage = new ProgressBar();
            this.progressBar_TotalMemoryUsage = new ProgressBar();
            this.label4 = new Label();
            this.label3 = new Label();
            this.groupBox3 = new GroupBox();
            this.label_TotalVirtualMemoryUsageRatio = new Label();
            this.label_TotalVirtualMemoryUsage = new Label();
            this.progressBar_TotalVirtualMemoryUsage = new ProgressBar();
            this.label5 = new Label();
            this.timer1 = new Timer(this.components);
            this.groupBox4 = new GroupBox();
            this.LogFilePathListBox = new ListBox();
            this.ElapsedTimeLabel = new Label();
            this.performaceLogStatusLabel = new Label();
            this.btn_logFileBroswer = new Button();
            this.toolStrip1 = new ToolStrip();
            this.frmCommOpenToolLog = new ToolStripSplitButton();
            this.startLogToolStripMenuItem = new ToolStripMenuItem();
            this.stopLogToolStripMenuItem = new ToolStripMenuItem();
            this.frmCommOpenToolFilter = new ToolStripDropDownButton();
            this.modifyPerformanceLoggingConditionsToolStripMenuItem = new ToolStripMenuItem();
            this.PerfLogOpenFileDialog = new OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.label_ProcessCPUUsage);
            this.groupBox1.Controls.Add(this.label_TotalCPUUsage);
            this.groupBox1.Controls.Add(this.progressBar_ProcessCPUUsage);
            this.groupBox1.Controls.Add(this.progressBar_TotalCPUUsage);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(0x11, 0x7a);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1a6, 0x6c);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CPU Usage";
            this.label_ProcessCPUUsage.AutoSize = true;
            this.label_ProcessCPUUsage.Location = new Point(0x16a, 70);
            this.label_ProcessCPUUsage.Name = "label_ProcessCPUUsage";
            this.label_ProcessCPUUsage.Size = new Size(0x16, 13);
            this.label_ProcessCPUUsage.TabIndex = 5;
            this.label_ProcessCPUUsage.Text = "0.0";
            this.label_TotalCPUUsage.AutoSize = true;
            this.label_TotalCPUUsage.Location = new Point(0x16a, 0x1f);
            this.label_TotalCPUUsage.Name = "label_TotalCPUUsage";
            this.label_TotalCPUUsage.Size = new Size(0x16, 13);
            this.label_TotalCPUUsage.TabIndex = 4;
            this.label_TotalCPUUsage.Text = "0.0";
            this.progressBar_ProcessCPUUsage.Location = new Point(0x5c, 0x43);
            this.progressBar_ProcessCPUUsage.Name = "progressBar_ProcessCPUUsage";
            this.progressBar_ProcessCPUUsage.Size = new Size(0xfc, 0x17);
            this.progressBar_ProcessCPUUsage.TabIndex = 3;
            this.progressBar_TotalCPUUsage.Location = new Point(0x5c, 0x19);
            this.progressBar_TotalCPUUsage.Name = "progressBar_TotalCPUUsage";
            this.progressBar_TotalCPUUsage.Size = new Size(0xfc, 0x17);
            this.progressBar_TotalCPUUsage.TabIndex = 2;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(20, 0x48);
            this.label2.Name = "label2";
            this.label2.Size = new Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "SiRFLive";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x27, 30);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1f, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total";
            this.groupBox2.Controls.Add(this.label_ProcessMemoryUsageRatio);
            this.groupBox2.Controls.Add(this.label_TotalMemoryUsageRatio);
            this.groupBox2.Controls.Add(this.label_ProcessVirtualMemoryUsage);
            this.groupBox2.Controls.Add(this.label_TotalMemoryUsage);
            this.groupBox2.Controls.Add(this.progressBar_ProcessMemoryUsage);
            this.groupBox2.Controls.Add(this.progressBar_TotalMemoryUsage);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new Point(0x11, 0xee);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x1a6, 0x91);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Physical Memory Usage";
            this.label_ProcessMemoryUsageRatio.AutoSize = true;
            this.label_ProcessMemoryUsageRatio.Location = new Point(0x5f, 0x7b);
            this.label_ProcessMemoryUsageRatio.Name = "label_ProcessMemoryUsageRatio";
            this.label_ProcessMemoryUsageRatio.Size = new Size(0x8a, 13);
            this.label_ProcessMemoryUsageRatio.TabIndex = 8;
            this.label_ProcessMemoryUsageRatio.Text = "ProcessMemoryUsageRatio";
            this.label_TotalMemoryUsageRatio.AutoSize = true;
            this.label_TotalMemoryUsageRatio.Location = new Point(0x5f, 0x42);
            this.label_TotalMemoryUsageRatio.Name = "label_TotalMemoryUsageRatio";
            this.label_TotalMemoryUsageRatio.Size = new Size(0x7c, 13);
            this.label_TotalMemoryUsageRatio.TabIndex = 7;
            this.label_TotalMemoryUsageRatio.Text = "TotalMemoryUsageRatio";
            this.label_ProcessVirtualMemoryUsage.AutoSize = true;
            this.label_ProcessVirtualMemoryUsage.Location = new Point(0x16a, 0x5b);
            this.label_ProcessVirtualMemoryUsage.Name = "label_ProcessVirtualMemoryUsage";
            this.label_ProcessVirtualMemoryUsage.Size = new Size(0x8e, 13);
            this.label_ProcessVirtualMemoryUsage.TabIndex = 6;
            this.label_ProcessVirtualMemoryUsage.Text = "ProcessVirtualMemoryUsage";
            this.label_TotalMemoryUsage.AutoSize = true;
            this.label_TotalMemoryUsage.Location = new Point(0x16a, 0x27);
            this.label_TotalMemoryUsage.Name = "label_TotalMemoryUsage";
            this.label_TotalMemoryUsage.Size = new Size(0x39, 13);
            this.label_TotalMemoryUsage.TabIndex = 5;
            this.label_TotalMemoryUsage.Text = "Total Mem";
            this.progressBar_ProcessMemoryUsage.Location = new Point(0x5c, 0x58);
            this.progressBar_ProcessMemoryUsage.Name = "progressBar_ProcessMemoryUsage";
            this.progressBar_ProcessMemoryUsage.Size = new Size(0xfc, 0x17);
            this.progressBar_ProcessMemoryUsage.TabIndex = 4;
            this.progressBar_TotalMemoryUsage.Location = new Point(0x5c, 0x1d);
            this.progressBar_TotalMemoryUsage.Name = "progressBar_TotalMemoryUsage";
            this.progressBar_TotalMemoryUsage.Size = new Size(0xfc, 0x17);
            this.progressBar_TotalMemoryUsage.TabIndex = 3;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(20, 0x5d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(50, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "SiRFLive";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x27, 0x22);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1f, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Total";
            this.groupBox3.Controls.Add(this.label_TotalVirtualMemoryUsageRatio);
            this.groupBox3.Controls.Add(this.label_TotalVirtualMemoryUsage);
            this.groupBox3.Controls.Add(this.progressBar_TotalVirtualMemoryUsage);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new Point(0x11, 0x185);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x1a6, 0x6c);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Virtual Memory Usage";
            this.label_TotalVirtualMemoryUsageRatio.AutoSize = true;
            this.label_TotalVirtualMemoryUsageRatio.Location = new Point(0x5d, 0x4d);
            this.label_TotalVirtualMemoryUsageRatio.Name = "label_TotalVirtualMemoryUsageRatio";
            this.label_TotalVirtualMemoryUsageRatio.Size = new Size(0xf6, 13);
            this.label_TotalVirtualMemoryUsageRatio.TabIndex = 7;
            this.label_TotalVirtualMemoryUsageRatio.Text = "TotalVirtualMemoryUsageRatioMemoryUsageRatio";
            this.label_TotalVirtualMemoryUsage.AutoSize = true;
            this.label_TotalVirtualMemoryUsage.Location = new Point(0x16a, 0x23);
            this.label_TotalVirtualMemoryUsage.Name = "label_TotalVirtualMemoryUsage";
            this.label_TotalVirtualMemoryUsage.Size = new Size(0x37, 13);
            this.label_TotalVirtualMemoryUsage.TabIndex = 6;
            this.label_TotalVirtualMemoryUsage.Text = "T. VirMem";
            this.progressBar_TotalVirtualMemoryUsage.Location = new Point(0x5c, 0x1f);
            this.progressBar_TotalVirtualMemoryUsage.Name = "progressBar_TotalVirtualMemoryUsage";
            this.progressBar_TotalVirtualMemoryUsage.Size = new Size(0xfc, 0x17);
            this.progressBar_TotalVirtualMemoryUsage.TabIndex = 5;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x27, 0x24);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1f, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Total";
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.groupBox4.Controls.Add(this.LogFilePathListBox);
            this.groupBox4.Controls.Add(this.ElapsedTimeLabel);
            this.groupBox4.Controls.Add(this.performaceLogStatusLabel);
            this.groupBox4.Controls.Add(this.btn_logFileBroswer);
            this.groupBox4.Location = new Point(0x11, 0x1c);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x1a6, 0x57);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Logging";
            this.LogFilePathListBox.FormattingEnabled = true;
            this.LogFilePathListBox.HorizontalExtent = 50;
            this.LogFilePathListBox.HorizontalScrollbar = true;
            this.LogFilePathListBox.Location = new Point(5, 0x37);
            this.LogFilePathListBox.Name = "LogFilePathListBox";
            this.LogFilePathListBox.Size = new Size(0x197, 0x11);
            this.LogFilePathListBox.TabIndex = 10;
            this.ElapsedTimeLabel.AutoSize = true;
            this.ElapsedTimeLabel.Location = new Point(0xad, 30);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.Size = new Size(0x83, 13);
            this.ElapsedTimeLabel.TabIndex = 9;
            this.ElapsedTimeLabel.Text = "Elapsed Time:  0:00:00:00";
            this.performaceLogStatusLabel.AutoSize = true;
            this.performaceLogStatusLabel.Location = new Point(0x3a, 30);
            this.performaceLogStatusLabel.Name = "performaceLogStatusLabel";
            this.performaceLogStatusLabel.Size = new Size(0x1b, 13);
            this.performaceLogStatusLabel.TabIndex = 8;
            this.performaceLogStatusLabel.Text = " Idle";
            this.btn_logFileBroswer.Location = new Point(-144, 0x12);
            this.btn_logFileBroswer.Name = "btn_logFileBroswer";
            this.btn_logFileBroswer.Size = new Size(10, 0x17);
            this.btn_logFileBroswer.TabIndex = 4;
            this.btn_logFileBroswer.Text = "...";
            this.btn_logFileBroswer.UseVisualStyleBackColor = true;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.frmCommOpenToolLog, this.frmCommOpenToolFilter });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x1cd, 0x19);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.frmCommOpenToolLog.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmCommOpenToolLog.DropDownItems.AddRange(new ToolStripItem[] { this.startLogToolStripMenuItem, this.stopLogToolStripMenuItem });
            this.frmCommOpenToolLog.Image = (Image) resources.GetObject("frmCommOpenToolLog.Image");
            this.frmCommOpenToolLog.ImageTransparentColor = Color.Magenta;
            this.frmCommOpenToolLog.Name = "frmCommOpenToolLog";
            this.frmCommOpenToolLog.Size = new Size(0x20, 0x16);
            this.frmCommOpenToolLog.Text = "Logging";
            this.startLogToolStripMenuItem.Name = "startLogToolStripMenuItem";
            this.startLogToolStripMenuItem.Size = new Size(0x81, 0x16);
            this.startLogToolStripMenuItem.Text = "Start Log";
            this.startLogToolStripMenuItem.Click += new EventHandler(this.startLogToolStripMenuItem_Click);
            this.stopLogToolStripMenuItem.Name = "stopLogToolStripMenuItem";
            this.stopLogToolStripMenuItem.Size = new Size(0x81, 0x16);
            this.stopLogToolStripMenuItem.Text = "Stop Log";
            this.stopLogToolStripMenuItem.Click += new EventHandler(this.stopLogToolStripMenuItem_Click);
            this.frmCommOpenToolFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmCommOpenToolFilter.DropDownItems.AddRange(new ToolStripItem[] { this.modifyPerformanceLoggingConditionsToolStripMenuItem });
            this.frmCommOpenToolFilter.Image = (Image) resources.GetObject("frmCommOpenToolFilter.Image");
            this.frmCommOpenToolFilter.ImageTransparentColor = Color.Magenta;
            this.frmCommOpenToolFilter.Name = "frmCommOpenToolFilter";
            this.frmCommOpenToolFilter.Size = new Size(0x1d, 0x16);
            this.frmCommOpenToolFilter.Text = "SiRFLive Phys Memory Limit";
            this.modifyPerformanceLoggingConditionsToolStripMenuItem.Name = "modifyPerformanceLoggingConditionsToolStripMenuItem";
            this.modifyPerformanceLoggingConditionsToolStripMenuItem.Size = new Size(0x112, 0x16);
            this.modifyPerformanceLoggingConditionsToolStripMenuItem.Text = "Modify Performance Logging Conditions";
            this.modifyPerformanceLoggingConditionsToolStripMenuItem.Click += new EventHandler(this.modifyPerformanceLoggingConditionsToolStripMenuItem_Click);
            this.PerfLogOpenFileDialog.FileName = "openFileDialog1";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1cd, 0x206);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.groupBox4);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmPerformanceMonitor";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SiRFLive Performance Monitor";
            base.FormClosing += new FormClosingEventHandler(this.PerformanceMonitorForm_FormClosing);
            base.Load += new EventHandler(this.PerformanceMonitorForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private static void loadLocation(Form formWindow, string top, string left, string width, string height, string state)
        {
            formWindow.Left = Convert.ToInt32(left);
            formWindow.Top = Convert.ToInt32(top);
            formWindow.Width = Convert.ToInt32(width);
            formWindow.Height = Convert.ToInt32(height);
            if (state == "Maximized")
            {
                formWindow.WindowState = FormWindowState.Maximized;
            }
            else if (state == "Minimized")
            {
                formWindow.WindowState = FormWindowState.Minimized;
            }
            else
            {
                formWindow.WindowState = FormWindowState.Normal;
            }
        }

        private void LogPerformance(StreamWriter sw, string valueStr)
        {
            DateTime now = DateTime.Now;
            string str2 = string.Format("{0:MM/dd/yyyy HH:mm:ss.ff} - ", now) + valueStr;
            sw.WriteLine(str2);
        }

        private void modifyPerformanceLoggingConditionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmPerformanceLoggingConditions().Show();
        }

        private void PerformanceMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool flag = true;
            if (((e.CloseReason == CloseReason.UserClosing) && (this.PerfLoggingState == PerfLoggingStates.Logging)) && (MessageBox.Show("Closing the Performance Monitor will stop logging.\r\rIs this what you want?", "Stopping Logging", MessageBoxButtons.YesNo) == DialogResult.No))
            {
                e.Cancel = true;
                flag = false;
            }
            if (flag)
            {
                SavePerformanceMonitorParameters();
                this.performanceMonitorMenuItem.Enabled = true;
                this.PerfLoggingState = PerfLoggingStates.Stopped;
                this.timer1.Stop();
            }
        }

        private void PerformanceMonitorForm_Load(object sender, EventArgs e)
        {
            this.PerfLoggingState = PerfLoggingStates.Not_Logging;
            this.performaceLogStatusLabel.Text = this.PerfLoggingState.ToString();
            if (!RestorePerformanceMonitorParameters())
            {
                this.PerfTrigger_TimeInterval = true;
                this.PerfTrigger_TotalCPUUsage = false;
                this.PerfTRigger_SiRFLiveCPUUsage = false;
                this.PerfTrigger_TotalPhysMemory = false;
                this.PerfTrigger_SiRFLivePhysMemory = false;
                this.PerfTrigger_VirtualMemoryUsage = false;
                this.TimeInterval = 1;
                this.TotalCPUUsagePercentage = 50.0;
                this.SiRFLiveCPUUsagePercentage = 50.0;
                this.PhysicalMemoryUsagePercentage = 50.0;
                this.SiRFLivePhysicalMemoryUsagePercentage = 50.0;
                this.VirtualMemoryUsagePercentage = 50.0;
            }
            if (this.performanceMonitorMenuItem != null)
            {
                this.performanceMonitorMenuItem.Enabled = false;
            }
        }

        private void PrintSystemEnvironmentInformation(StreamWriter sw)
        {
            sw.WriteLine();
            this.line = string.Format("\t*** System Environment ***", new object[0]);
            sw.WriteLine(this.line);
            sw.WriteLine();
            this.line = string.Format("\tMachine Name:\t\t{0}", Environment.MachineName);
            sw.WriteLine(this.line);
            this.line = string.Format("\tOperating System:\t{0}", Environment.OSVersion);
            sw.WriteLine(this.line);
            this.line = string.Format("\tProcessor Count:\t{0}", Environment.ProcessorCount);
            sw.WriteLine(this.line);
            string str = string.Empty;
            string str2 = string.Empty;
            int num = 0;
            ManagementObjectCollection instances = new ManagementClass("Win32_Processor").GetInstances();
            Application.DoEvents();
            foreach (ManagementObject obj2 in instances)
            {
                if (num == 0)
                {
                    num = Convert.ToInt32(obj2.Properties["CurrentClockSpeed"].Value.ToString());
                }
                if (str == string.Empty)
                {
                    str = (string) obj2.Properties["Name"].Value;
                }
                if (str2 == string.Empty)
                {
                    str2 = (string) obj2.Properties["Description"].Value;
                }
            }
            this.line = string.Format("\tProcessor Name:\t{0}", str);
            sw.WriteLine(this.line);
            this.line = string.Format("\tProcessor Family:\t{0}", str2);
            sw.WriteLine(this.line);
            this.line = string.Format("\tClock Speed:\t\t{0} Mhz", num);
            sw.WriteLine(this.line);
            sw.WriteLine();
        }

        private void PrintTriggerValues(StreamWriter sw)
        {
            sw.WriteLine();
            this.line = string.Format("\t*** Logging Trigger Values ***", new object[0]);
            sw.WriteLine(this.line);
            sw.WriteLine();
            this.line = string.Format("\tTime Interval:\t\t{0} seconds", _TimeInterval);
            sw.WriteLine(this.line);
            this.line = string.Format("\tTotal CPU Usage:\t{0} percentage", _TotalCPUUsagePercentage);
            sw.WriteLine(this.line);
            this.line = string.Format("\tSiRFLive CPU Usage:\t{0} percentage", _SiRFLiveCPUUsagePercentage);
            sw.WriteLine(this.line);
            this.line = string.Format("\tPhysical Memory Usage:\t{0} percentage", _PhysicalMemoryUsagePercentage);
            sw.WriteLine(this.line);
            this.line = string.Format("\tSiRFLine Memory Usage:\t{0} percentage", _SiRFLivePhysicalMemoryUsagePercentage);
            sw.WriteLine(this.line);
            this.line = string.Format("\tVirtual Memory Usage:\t{0} percentage", _VirtualMemoryUsagePercentage);
            sw.WriteLine(this.line);
            sw.WriteLine();
        }

        private static bool RestorePerformanceMonitorParameters()
        {
            bool flag = false;
            XmlDocument document = new XmlDocument();
            if (File.Exists(_PerfMonRestoredFilePath))
            {
                try
                {
                    document.Load(_PerfMonRestoredFilePath);
                    foreach (XmlNode node in document.SelectNodes("/PerformanceMonitor/Window"))
                    {
                        string str2;
                        if (((str2 = node.Attributes["name"].Value.ToString()) != null) && (str2 == "frmPerformanceMonitor"))
                        {
                            frmPerformanceMonitor perfmonFormHandle = frmPerformanceMonitor.perfmonFormHandle;
                            perfmonFormHandle.PerfTrigger_TimeInterval = Convert.ToBoolean(node.Attributes["trigger_timeInternal"].Value.ToString());
                            perfmonFormHandle.TimeInterval = Convert.ToUInt32(node.Attributes["timeInterval"].Value.ToString());
                            perfmonFormHandle.PerfTrigger_TotalCPUUsage = Convert.ToBoolean(node.Attributes["trigger_totalCPU"].Value.ToString());
                            perfmonFormHandle.TotalCPUUsagePercentage = Convert.ToDouble(node.Attributes["totalCPU"].Value.ToString());
                            perfmonFormHandle.PerfTRigger_SiRFLiveCPUUsage = Convert.ToBoolean(node.Attributes["trigger_SiRFLiveCPU"].Value.ToString());
                            perfmonFormHandle.SiRFLiveCPUUsagePercentage = Convert.ToDouble(node.Attributes["SiRFLiveCPU"].Value.ToString());
                            perfmonFormHandle.PerfTrigger_TotalPhysMemory = Convert.ToBoolean(node.Attributes["trigger_physicalMemory"].Value.ToString());
                            perfmonFormHandle.PhysicalMemoryUsagePercentage = Convert.ToDouble(node.Attributes["totalPhysicalMemory"].Value.ToString());
                            perfmonFormHandle.PerfTrigger_SiRFLivePhysMemory = Convert.ToBoolean(node.Attributes["trigger_SiRFLivelMemory"].Value.ToString());
                            perfmonFormHandle.SiRFLivePhysicalMemoryUsagePercentage = Convert.ToDouble(node.Attributes["SiRFLivePhysicalMemory"].Value.ToString());
                            perfmonFormHandle.PerfTrigger_VirtualMemoryUsage = Convert.ToBoolean(node.Attributes["trigger_VirtualMemory"].Value.ToString());
                            perfmonFormHandle.VirtualMemoryUsagePercentage = Convert.ToDouble(node.Attributes["VirtuallMemory"].Value.ToString());
                            loadLocation(frmPerformanceMonitor.perfmonFormHandle, node.Attributes["top"].Value.ToString(), node.Attributes["left"].Value.ToString(), node.Attributes["width"].Value.ToString(), node.Attributes["height"].Value.ToString(), node.Attributes["windowState"].Value.ToString());
                            flag = true;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("frmPerformanceMonitor() + RestorePerformanceMonitorParameters()\r\r" + exception.ToString());
                }
            }
            return flag;
        }

        public void SavePerformanceMonitorMenuItem(ToolStripMenuItem menuItem)
        {
            this.performanceMonitorMenuItem = menuItem;
        }

        public static void SavePerformanceMonitorParameters()
        {
            StreamWriter writer;
            if (File.Exists(_PerfMonRestoredFilePath))
            {
                if ((File.GetAttributes(_PerfMonRestoredFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("File is read only - Window locations were not saved!\n{0}", _PerfMonRestoredFilePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                writer = new StreamWriter(_PerfMonRestoredFilePath);
            }
            else
            {
                writer = File.CreateText(_PerfMonRestoredFilePath);
            }
            if (writer != null)
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<PerformanceMonitor>");
                string str2 = string.Empty;
                string format = string.Empty;
                frmPerformanceMonitor perfmonFormHandle = frmPerformanceMonitor.perfmonFormHandle;
                format = "<Window name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\" trigger_timeInternal=\"{6}\" timeInterval=\"{7}\" trigger_totalCPU=\"{8}\" totalCPU=\"{9}\" trigger_SiRFLiveCPU=\"{10}\" SiRFLiveCPU=\"{11}\" trigger_physicalMemory=\"{12}\" totalPhysicalMemory=\"{13}\" trigger_SiRFLivelMemory=\"{14}\" SiRFLivePhysicalMemory=\"{15}\" trigger_VirtualMemory=\"{16}\" VirtuallMemory=\"{17}\" >";
                str2 = string.Format(format, new object[] { 
                    perfmonFormHandle.Name, perfmonFormHandle.Top.ToString(), perfmonFormHandle.Left.ToString(), perfmonFormHandle.Width.ToString(), perfmonFormHandle.Height.ToString(), perfmonFormHandle.WindowState.ToString(), perfmonFormHandle.PerfTrigger_TimeInterval.ToString(), perfmonFormHandle.TimeInterval, perfmonFormHandle.PerfTrigger_TotalCPUUsage.ToString(), perfmonFormHandle.TotalCPUUsagePercentage, perfmonFormHandle.PerfTRigger_SiRFLiveCPUUsage.ToString(), perfmonFormHandle.SiRFLiveCPUUsagePercentage, perfmonFormHandle.PerfTrigger_TotalPhysMemory.ToString(), perfmonFormHandle.PhysicalMemoryUsagePercentage, perfmonFormHandle.PerfTrigger_SiRFLivePhysMemory.ToString(), perfmonFormHandle.SiRFLivePhysicalMemoryUsagePercentage, 
                    perfmonFormHandle.PerfTrigger_VirtualMemoryUsage.ToString(), perfmonFormHandle.VirtualMemoryUsagePercentage
                 });
                writer.WriteLine(str2);
                writer.WriteLine("</Window>");
                writer.WriteLine("</PerformanceMonitor>");
                writer.Close();
            }
        }

        private void startLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.PerfLoggingState == PerfLoggingStates.Not_Logging)
            {
                try
                {
                    this.PerfLogPathDir = this.InstalledDirectory + @"\Log";
                    if (!Directory.Exists(this.PerfLogPathDir))
                    {
                        Directory.CreateDirectory(this.PerfLogPathDir);
                    }
                    this.PerfLogFullPathName = this.PerfLogPathDir + @"\" + this.CreatePerformanceLogFileName();
                    PerfLogSW = File.CreateText(this.PerfLogFullPathName);
                    this.LogFilePathListBox.Items.Add(this.PerfLogFullPathName);
                    this.PrintTriggerValues(PerfLogSW);
                    this.PrintSystemEnvironmentInformation(PerfLogSW);
                    this.timer1.Interval = 0x3e8;
                    this.timer1.Start();
                    this.PerfLoggingState = PerfLoggingStates.Logging;
                    this.performaceLogStatusLabel.Text = this.PerfLoggingState.ToString();
                    this.LogStartTime = DateTime.Now;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            else
            {
                MessageBox.Show("Logging has already started");
            }
        }

        private void stopLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.PerfLoggingState == PerfLoggingStates.Logging)
            {
                this.timer1.Stop();
                this.PerfLoggingState = PerfLoggingStates.Stopped;
                this.performaceLogStatusLabel.Text = this.PerfLoggingState.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float num = this.performanceCounter_TotalCPUUsage.NextValue();
            int num2 = (int) Math.Floor((double) num);
            if (num2 >= 100)
            {
                num2 = 100;
            }
            this.progressBar_TotalCPUUsage.Value = num2;
            this.label_TotalCPUUsage.Text = string.Format("{0:F2} %", num);
            this.performanceCounter_ProcessCPUUsage.InstanceName = Process.GetCurrentProcess().ProcessName;
            float num3 = this.performanceCounter_ProcessCPUUsage.NextValue() / ((float) this.NumberOfProcessors);
            num2 = (int) Math.Floor((double) num3);
            if (num2 >= 100)
            {
                num2 = 100;
            }
            this.progressBar_ProcessCPUUsage.Value = num2;
            this.label_ProcessCPUUsage.Text = string.Format("{0:F2} %", num3);
            int num4 = ((int) Process.GetCurrentProcess().WorkingSet64) / 0x400;
            int num5 = ((int) Process.GetCurrentProcess().VirtualMemorySize64) / 0x400;
            ComputerInfo info = new ComputerInfo();
            ulong num6 = info.TotalPhysicalMemory / ((ulong) 0x400L);
            ulong num7 = info.AvailablePhysicalMemory / ((ulong) 0x400L);
            ulong num8 = info.TotalVirtualMemory / ((ulong) 0x400L);
            ulong num9 = info.AvailableVirtualMemory / ((ulong) 0x400L);
            double d = (100.0 * (num6 - num7)) / ((double) num6);
            num2 = (int) Math.Floor(d);
            this.progressBar_TotalMemoryUsage.Value = num2;
            this.label_TotalMemoryUsage.Text = string.Format("{0:F2} %", d);
            this.label_TotalMemoryUsageRatio.Text = string.Format("{0:N0} K / {1:N0} K", num6 - num7, num6);
            double num11 = (100.0 * num4) / ((double) num6);
            num2 = (int) Math.Floor(num11);
            if (num2 >= 100)
            {
                num2 = 100;
            }
            this.progressBar_ProcessMemoryUsage.Value = num2;
            this.label_ProcessMemoryUsageRatio.Text = string.Format("{0:N0} K / {1:N0} K", num4, num6);
            double num12 = (100.0 * (num8 - num9)) / ((double) num8);
            num2 = (int) Math.Floor(num12);
            if (num2 >= 100)
            {
                num2 = 100;
            }
            this.progressBar_TotalVirtualMemoryUsage.Value = num2;
            this.label_TotalVirtualMemoryUsage.Text = string.Format("{0:F2} %", num12);
            this.label_TotalVirtualMemoryUsageRatio.Text = string.Format("{0:N0} K / {1:N0} K", num8 - num9, num8);
            double num13 = (100.0 * num5) / ((double) num8);
            num2 = (int) Math.Floor(num13);
            if (num2 >= 100)
            {
                num2 = 100;
            }
            this.label_ProcessVirtualMemoryUsage.Text = string.Format("{0:F2} %", num13);
            this.label_TotalVirtualMemoryUsageRatio.Text = string.Format("{0:N0} K / {1:N0} K", num5, num8);
            if (this.PerfLoggingState == PerfLoggingStates.Logging)
            {
                this.ElapsedTime = (TimeSpan) (DateTime.Now - this.LogStartTime);
                string str = this.ElapsedTime.ToString();
                this.ElapsedTimeLabel.Text = "Elapsed Time: " + str.Remove(str.Length - 8);
                this.CPU_Asterisk = " ";
                this.SiRFLiveCPU_Asterisk = " ";
                this.Memory_Asterisk = " ";
                this.SiRFLiveMemory_Asterisk = " ";
                this.VirtualMemory_Asterisk = " ";
                bool flag = false;
                if (this.PerfTrigger_TimeInterval)
                {
                    this.CurrentTime = DateTime.Now;
                    if ((this.CurrentTime - this.LastTimerTick) > new TimeSpan(0, 0, (int) _TimeInterval))
                    {
                        flag = true;
                        this.LastTimerTick = this.CurrentTime;
                    }
                }
                if (this.PerfTrigger_TotalCPUUsage && (this.progressBar_TotalCPUUsage.Value > this.TotalCPUUsagePercentage))
                {
                    flag = true;
                    this.CPU_Asterisk = "*";
                }
                if (this.PerfTRigger_SiRFLiveCPUUsage && (this.progressBar_ProcessCPUUsage.Value > this.SiRFLiveCPUUsagePercentage))
                {
                    flag = true;
                    this.SiRFLiveCPU_Asterisk = "*";
                }
                if (this.PerfTrigger_TotalPhysMemory && (this.progressBar_TotalMemoryUsage.Value > this.PhysicalMemoryUsagePercentage))
                {
                    flag = true;
                    this.Memory_Asterisk = "*";
                }
                if (this.PerfTrigger_SiRFLivePhysMemory && (this.progressBar_ProcessMemoryUsage.Value > this.SiRFLivePhysicalMemoryUsagePercentage))
                {
                    flag = true;
                    this.SiRFLiveMemory_Asterisk = "*";
                }
                if (this.PerfTrigger_VirtualMemoryUsage && (this.progressBar_TotalVirtualMemoryUsage.Value > this.VirtualMemoryUsagePercentage))
                {
                    flag = true;
                    this.VirtualMemory_Asterisk = "*";
                }
                if (flag)
                {
                    this.line = string.Format("{0}CPU: {1:D3} {2}SiRFLive CPU: {3:D3} {4}Memory: {5:D3} {6}SiRFLive Memory: {7:D3} {8}Virtual Memory: {9:D3}", new object[] { this.CPU_Asterisk, this.progressBar_TotalCPUUsage.Value, this.SiRFLiveCPU_Asterisk, this.progressBar_ProcessCPUUsage.Value, this.Memory_Asterisk, this.progressBar_TotalMemoryUsage.Value, this.SiRFLiveMemory_Asterisk, this.progressBar_ProcessMemoryUsage.Value, this.VirtualMemory_Asterisk, this.progressBar_TotalVirtualMemoryUsage.Value });
                    this.LogPerformance(PerfLogSW, this.line);
                    this.performaceLogStatusLabel.Text = "*" + this.PerfLoggingState.ToString();
                }
                else
                {
                    this.performaceLogStatusLabel.Text = this.PerfLoggingState.ToString();
                }
            }
        }

        public string PerfMonRestoredFilePath
        {
            get
            {
                return _PerfMonRestoredFilePath;
            }
            set
            {
                _PerfMonRestoredFilePath = value;
            }
        }

        public bool PerformanceParametersRestoredFlag
        {
            get
            {
                return _PerformanceParametersRestoredFlag;
            }
            set
            {
                _PerformanceParametersRestoredFlag = value;
            }
        }

        public bool PerfTRigger_SiRFLiveCPUUsage
        {
            get
            {
                return _PerfTRigger_SiRFLiveCPUUsage;
            }
            set
            {
                _PerfTRigger_SiRFLiveCPUUsage = value;
            }
        }

        public bool PerfTrigger_SiRFLivePhysMemory
        {
            get
            {
                return _PerfTrigger_SiRFLivePhysMemory;
            }
            set
            {
                _PerfTrigger_SiRFLivePhysMemory = value;
            }
        }

        public bool PerfTrigger_TimeInterval
        {
            get
            {
                return _PerfTrigger_TimeInterval;
            }
            set
            {
                _PerfTrigger_TimeInterval = value;
            }
        }

        public bool PerfTrigger_TotalCPUUsage
        {
            get
            {
                return _PerfTrigger_TotalCPUUsage;
            }
            set
            {
                _PerfTrigger_TotalCPUUsage = value;
            }
        }

        public bool PerfTrigger_TotalPhysMemory
        {
            get
            {
                return _PerfTrigger_TotalPhysMemory;
            }
            set
            {
                _PerfTrigger_TotalPhysMemory = value;
            }
        }

        public bool PerfTrigger_VirtualMemoryUsage
        {
            get
            {
                return _PerfTrigger_VirtualMemoryUsage;
            }
            set
            {
                _PerfTrigger_VirtualMemoryUsage = value;
            }
        }

        public double PhysicalMemoryUsagePercentage
        {
            get
            {
                return _PhysicalMemoryUsagePercentage;
            }
            set
            {
                _PhysicalMemoryUsagePercentage = value;
            }
        }

        public double SiRFLiveCPUUsagePercentage
        {
            get
            {
                return _SiRFLiveCPUUsagePercentage;
            }
            set
            {
                _SiRFLiveCPUUsagePercentage = value;
            }
        }

        public double SiRFLivePhysicalMemoryUsagePercentage
        {
            get
            {
                return _SiRFLivePhysicalMemoryUsagePercentage;
            }
            set
            {
                _SiRFLivePhysicalMemoryUsagePercentage = value;
            }
        }

        public uint TimeInterval
        {
            get
            {
                return _TimeInterval;
            }
            set
            {
                _TimeInterval = value;
            }
        }

        public double TotalCPUUsagePercentage
        {
            get
            {
                return _TotalCPUUsagePercentage;
            }
            set
            {
                _TotalCPUUsagePercentage = value;
            }
        }

        public double VirtualMemoryUsagePercentage
        {
            get
            {
                return _VirtualMemoryUsagePercentage;
            }
            set
            {
                _VirtualMemoryUsagePercentage = value;
            }
        }

        private enum PerfLoggingStates
        {
            Not_Logging,
            Idle,
            Opened,
            Stopped,
            Logging
        }
    }
}

