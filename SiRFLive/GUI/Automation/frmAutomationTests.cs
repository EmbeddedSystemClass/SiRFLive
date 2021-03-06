﻿namespace SiRFLive.GUI.Automation
{
    using IronPython.Runtime.Exceptions;
    using SiRFLive.Configuration;
    using SiRFLive.General;
    using SiRFLive.GUI.DeviceControl;
    using SiRFLive.GUI.Python;
    using SiRFLive.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class frmAutomationTests : Form
    {
        private int _currentIndex;
        private bool _running;
        private string automationIniPath = (clsGlobal.InstalledDirectory + @"\Config\SiRFLiveAutomation.cfg");
        private Button automationTestAbortBtn;
        private Button autoTestAddAllBtn;
        private Button autoTestAddBtn;
        private Label autoTestAvailableScriptsLabel;
        private ListBox autoTestAvailableScriptsListVal;
        private Button autoTestClearAvailableListBtn;
        private Button autoTestDirBrowser;
        private TextBox autoTestDirVal;
        private Label autoTestEmailListLabel;
        private TextBox autoTestEmailListVal;
        private Button autoTestExitBtn;
        private Label autoTestFilePathLabel;
        private Button autoTestRefreshBtn;
        private Button autoTestRemoveAllBtn;
        private Button autoTestRemoveBtn;
        private Button autoTestRunBtn;
        private Label autoTestRunScriptsLabel;
        private Button autoTestScriptCfgBtn;
        private CheckBox autoTestSendEmailChkVal;
        private Button autoTestSetupCfgBtn;
        private Thread autoTestThread;
        private CheckedListBox autoTestToRunScriptsListVal;
        private IContainer components;
        private ObjectInterface crossThreadUpdateIntf = new ObjectInterface();
        private EmailHandler emailHelper = new EmailHandler();
        private static int fileIdx;
        private Label frmAutoTestSciptCntLabel;
        private static frmAutomationTests m_SChildform;
        private frmPython objPython = frmPython.GetChildInstance();
        private string[] scriptsArray = new string[0];
        private List<string> scriptsFullPathLists = new List<string>();
        private string testResults = string.Empty;
        private List<string> toRunList = new List<string>();
        private string[] toRunListArray = new string[0];

        public frmAutomationTests()
        {
            this.InitializeComponent();
            clsGlobal.Abort = false;
            clsGlobal.AbortSingle = false;
            clsGlobal.ScriptDone = true;
            IniHelper helper = new IniHelper(this.automationIniPath);
            string str = string.Empty;
            if (clsGlobal.IsMarketingUser())
            {
                this.autoTestEmailListLabel.Visible = false;
                this.autoTestEmailListVal.Visible = false;
                this.autoTestSendEmailChkVal.Visible = false;
            }
            try
            {
                if (File.Exists(this.automationIniPath))
                {
                    foreach (string str2 in helper.GetKeys("EMAIL"))
                    {
                        if (!str2.Contains("#"))
                        {
                            str = helper.GetIniFileString("EMAIL", str2, "");
                            if (str.Length != 0)
                            {
                                str = str.Replace(" ", "").TrimEnd(new char[] { '\n' }).TrimEnd(new char[] { '\r' });
                                ConfigurationManager.AppSettings[str2] = str;
                                if (str2 == "Email.Recipient")
                                {
                                    this.autoTestEmailListVal.Text = ConfigurationManager.AppSettings[str2];
                                }
                            }
                        }
                    }
                    str = helper.GetIniFileString("SETUP", "AVAILABLE_SCRIPTS", "");
                    if (str.Length != 0)
                    {
                        this.scriptsFullPathLists.Clear();
                        foreach (string str3 in str.Split(new char[] { ',' }))
                        {
                            if (str3.Length != 0)
                            {
                                this.autoTestAddAvailableScripts(str3);
                            }
                        }
                    }
                    this.scriptsArray = this.scriptsFullPathLists.ToArray();
                    this.autoTestDirVal.Text = helper.GetIniFileString("SETUP", "SCRIPTS_DIR", "");
                    str = helper.GetIniFileString("SETUP", "SEND_EMAIL", "");
                    if (str.Length != 0)
                    {
                        if (str == "1")
                        {
                            this.autoTestSendEmailChkVal.Checked = true;
                        }
                        else
                        {
                            this.autoTestSendEmailChkVal.Checked = false;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private DialogResult autoTestAbort()
        {
            DialogResult yes = DialogResult.Yes;
            if (!clsGlobal.ScriptDone)
            {
                yes = MessageBox.Show("Test in progress -- Abort?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (yes != DialogResult.Yes)
                {
                    return yes;
                }
                clsGlobal.Abort = true;
                this.objPython.PythonEngineOutput.CloseFile();
                this.objPython.WriteLine("Test aborted!");
                try
                {
                    if (frmRFPlaybackCtrl.GetChildInstance() != null)
                    {
                        frmRFPlaybackCtrl.GetChildInstance().PlaybackStop();
                    }
                }
                catch
                {
                }
                clsGlobal.g_objfrmMDIMain.CancelDelay();
                clsGlobal.AbortEvent.SiRFLiveEventSet();
                this.objPython.engine.Shutdown();
                this.objPython.engine.Dispose();
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if (manager != null)
                    {
                        manager.CloseAll();
                    }
                }
                clsGlobal.ScriptDone = true;
                this._running = false;
            }
            return yes;
        }

        private void autoTestAbortBtn_Click(object sender, EventArgs e)
        {
            this.autoTestAbort();
        }

        private void autoTestAddAllBtn_Click(object sender, EventArgs e)
        {
            this.autoTestToRunScriptsListVal.Items.Clear();
            this.toRunList.Clear();
            foreach (string str in this.autoTestAvailableScriptsListVal.Items)
            {
                this.autoTestToRunScriptsListVal.Items.Add(str);
            }
            foreach (string str2 in this.scriptsFullPathLists)
            {
                this.toRunList.Add(str2);
            }
            this.autoTestUpdateTestCnt();
        }

        private void autoTestAddAvailableScripts(string fileStr)
        {
            string[] strArray = new string[] { "" };
            int length = 0;
            this.scriptsFullPathLists.Add(fileStr);
            strArray = fileStr.Split(new char[] { '\\' });
            length = strArray.Length;
            this.autoTestAvailableScriptsListVal.Items.Add(strArray[length - 1]);
        }

        private void autoTestAddBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.autoTestAvailableScriptsListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.autoTestToRunScriptsListVal.Items.Add(this.autoTestAvailableScriptsListVal.SelectedItem);
                this.toRunList.Add(this.scriptsArray[selectedIndex]);
                this.autoTestUpdateTestCnt();
            }
        }

        private void autoTestAvailableScriptsListVal_DoubleClick(object sender, EventArgs e)
        {
            int selectedIndex = this.autoTestAvailableScriptsListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.autoTestToRunScriptsListVal.Items.Add(this.autoTestAvailableScriptsListVal.SelectedItem);
                this.toRunList.Add(this.scriptsArray[selectedIndex]);
                this.autoTestUpdateTestCnt();
            }
        }

        private void autoTestAvailableScriptsListVal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.scriptsFullPathLists.Count != 0)
            {
                string text = string.Empty;
                foreach (string str2 in this.scriptsFullPathLists)
                {
                    text = text + str2 + "\n";
                }
                MessageBox.Show(text, "Information");
            }
        }

        private void autoTestClearAvailableListBtn_Click(object sender, EventArgs e)
        {
            this.autoTestAvailableScriptsListVal.Items.Clear();
            this.scriptsFullPathLists.Clear();
        }

        private void autoTestDirectoryBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.autoTestDirVal.Text = dialog.SelectedPath;
                foreach (string str in Directory.GetFiles(dialog.SelectedPath))
                {
                    if (str.EndsWith(".py"))
                    {
                        this.autoTestAddAvailableScripts(str);
                    }
                }
            }
            this.scriptsArray = this.scriptsFullPathLists.ToArray();
        }

        private void autoTestEmailListVal_TextChanged(object sender, EventArgs e)
        {
            ConfigurationManager.AppSettings["Email.Recipient"] = this.autoTestEmailListVal.Text;
        }

        private void autoTestExitBtn_Click(object sender, EventArgs e)
        {
            DialogResult yes = DialogResult.Yes;
            if (!clsGlobal.ScriptDone)
            {
                yes = this.autoTestAbort();
            }
            if (yes == DialogResult.Yes)
            {
                m_SChildform = null;
                base.Close();
            }
        }

        private void autoTestRefreshBtn_Click(object sender, EventArgs e)
        {
            this.updateAvailableScript();
        }

        private void autoTestRemoveAllBtn_Click(object sender, EventArgs e)
        {
            this.autoTestToRunScriptsListVal.Items.Clear();
            this.toRunList.Clear();
            fileIdx = 0;
            this.autoTestUpdateTestCnt();
        }

        private void autoTestRemoveBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.autoTestToRunScriptsListVal.SelectedIndex;
            string[] strArray = this.toRunList.ToArray();
            if (selectedIndex >= 0)
            {
                this.autoTestToRunScriptsListVal.Items.Remove(this.autoTestToRunScriptsListVal.SelectedItem);
                this.toRunList.Remove(strArray[selectedIndex]);
                if ((fileIdx > 0) && (fileIdx > selectedIndex))
                {
                    fileIdx--;
                }
                this.autoTestUpdateTestCnt();
            }
        }

        private void autoTestReport(string message)
        {
            if (this.autoTestSendEmailChkVal.Checked)
            {
                string str = this.autoTestEmailListVal.Text.Replace(" ", "");
                this.emailHelper.EmailRecipient = str;
                this.emailHelper.EmailSender = ConfigurationManager.AppSettings["Email.Sender"];
                this.emailHelper.EmailSubject = ConfigurationManager.AppSettings["Email.Subject"];
                this.emailHelper.EmailMessage = message;
                this.emailHelper.SendMailMessage();
            }
        }

        private void autoTestRunBtn_Click(object sender, EventArgs e)
        {
            if (this.objPython != null)
            {
                this.objPython.Close();
            }
            this.objPython.Dispose();
            this.objPython = null;
            clsGlobal.g_objfrmMDIMain.ClearPortList();
            this.objPython = new frmPython();
            this.objPython.MdiParent = base.MdiParent;
            this.objPython.Show();
            this.objPython.BringToFront();
            if (!clsGlobal.ScriptDone)
            {
                this.autoTestAbort();
            }
            else
            {
                this.autoTestThread = new Thread(new ThreadStart(this.autoTestStart));
                this.autoTestThread.IsBackground = true;
                this.autoTestThread.Start();
            }
        }

        public void autoTestRunSingle()
        {
            if ((this.objPython == null) || this.objPython.IsDisposed)
            {
                this.objPython = new frmPython();
            }
            if (!this.objPython.Visible)
            {
                this.objPython.Show();
                this.objPython.BringToFront();
            }
            if (clsGlobal.ScriptDone)
            {
                if (fileIdx >= this.toRunList.Count)
                {
                    this.testResults = this.testResults + "Log files at: " + clsGlobal.LogDirectory + "\n";
                    this.testResults = this.testResults + "\nBest Regards,\n\nSiRFLive Team\n";
                    this.autoTestReport(this.testResults);
                }
                else if (clsGlobal.Abort)
                {
                    this.testResults = this.testResults + "\r\n *** Test Aborted ***\r\n";
                    this.testResults = this.testResults + "Log files at: " + clsGlobal.LogDirectory + "\n";
                    this.testResults = this.testResults + "\nBest Regards,\n\nSiRFLive Team\n";
                    this.autoTestReport(this.testResults);
                    fileIdx = this.toRunList.Count + 1;
                    clsGlobal.Abort = false;
                }
                else
                {
                    this.testResults = this.testResults + this.crossThreadUpdateIntf.GetSelectedCheckListBoxIdx(this.autoTestToRunScriptsListVal, fileIdx) + "\n";
                    clsGlobal.ScriptDone = false;
                    string fileName = this.toRunListArray[fileIdx];
                    clsGlobal.TestScriptPath = fileName;
                    fileIdx++;
                    this.objPython.engine.ExecuteFile(fileName);
                }
            }
        }

        private void autoTestScriptCfgBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.autoTestToRunScriptsListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                string configFilePath = this.toRunList[selectedIndex];
                string str2 = this.autoTestToRunScriptsListVal.Text.ToUpper();
                if (str2.Contains("3GPP") || str2.Contains("TIA916"))
                {
                    new frm3GPPConfig(configFilePath).ShowDialog();
                }
                else
                {
                    new frmConfiguration(configFilePath).ShowDialog();
                }
            }
        }

        private void autoTestSetupConfigBtn_Click(object sender, EventArgs e)
        {
            new frmConfiguration(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts\SiRFLiveAutomationSetup.cfg").ShowDialog();
        }

        private void autoTestStart()
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            this.setAutoTestToRunScriptsListValCheckBox(-1, false);
            this.objPython.testResult = "Hi,\nTest Summary:\n\n";
            string machineName = Environment.MachineName;
            this.testResults = string.Format("Hi,\nTest Station: {0}\nTest Summary:\n\n", machineName);
            this.toRunListArray = this.toRunList.ToArray();
            clsGlobal.ScriptDone = true;
            clsGlobal.Abort = false;
            clsGlobal.AbortSingle = false;
            bool flag = false;
            fileIdx = 0;
            string fileName = clsGlobal.InstalledDirectory + @"\scripts\core\rxSetup.py";
            string str3 = clsGlobal.InstalledDirectory + @"\scripts\core\portSetup.py";
            try
            {
                this.objPython.engine.ExecuteFile(fileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Setup script encountered an error: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                flag = true;
            }
            while (!clsGlobal.ScriptDone)
            {
                Thread.Sleep(100);
            }
            if (clsGlobal.ScriptError)
            {
                MessageBox.Show("Setup script encountered an error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                flag = true;
            }
            if (!flag)
            {
                try
                {
                    this.objPython.engine.ExecuteFile(str3);
                }
                catch (Exception exception2)
                {
                    MessageBox.Show("Port Setup script encountered an error: " + exception2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    flag = true;
                }
                while (!clsGlobal.ScriptDone)
                {
                    Thread.Sleep(100);
                }
                if (clsGlobal.ScriptError)
                {
                    MessageBox.Show("Port Setup script encountered an error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    flag = true;
                }
            }
            if (!flag)
            {
                this._currentIndex = 0;
                foreach (string str4 in this.toRunListArray)
                {
                    this._running = true;
                    clsGlobal.TestScriptPath = str4.TrimEnd(new char[] { '\n' });
                    clsGlobal.TestScriptPath = clsGlobal.TestScriptPath.TrimEnd(new char[] { '\r' });
                    if (this._currentIndex < this.autoTestToRunScriptsListVal.Items.Count)
                    {
                        this.setAutoTestToRunScriptsListValCheckBox(this._currentIndex, true);
                    }
                    try
                    {
                        this.objPython.engine.ExecuteFile(clsGlobal.TestScriptPath);
                    }
                    catch (PythonSyntaxErrorException exception3)
                    {
                        this.objPython.WriteLine("Syntax exception:");
                        this.objPython.WriteLine(string.Format("Message: {0}\nLineText: {1}\nLine:{2}\nColumn: {3}", new object[] { exception3.Message, exception3.LineText, exception3.Line, exception3.Column }));
                    }
                    while (!clsGlobal.ScriptDone)
                    {
                        if (!clsGlobal.AbortSingle)
                        {
                            try
                            {
                                Thread.Sleep(150);
                            }
                            catch
                            {
                            }
                        }
                    }
                    this._currentIndex++;
                    if (clsGlobal.Abort)
                    {
                        break;
                    }
                }
                try
                {
                    string str5 = clsGlobal.InstalledDirectory + @"\scripts\core\cleanup.py";
                    this.objPython.engine.ExecuteFile(str5);
                }
                catch
                {
                }
                this._running = false;
                clsGlobal.CommWinRef.Clear();
            }
            try
            {
                this.objPython.PythonEngineOutput.CloseFile();
            }
            catch
            {
            }
        }

        private void autoTestToRunScriptsListVal_DoubleClick(object sender, EventArgs e)
        {
            if (this._running)
            {
                MessageBox.Show("Can't modify test lists while test is running.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                int selectedIndex = this.autoTestToRunScriptsListVal.SelectedIndex;
                if (selectedIndex > this._currentIndex)
                {
                    this.setAutoTestToRunScriptsListValCheckBox(selectedIndex, false);
                }
                else
                {
                    this.setAutoTestToRunScriptsListValCheckBox(selectedIndex, true);
                }
            }
            else
            {
                int index = this.autoTestToRunScriptsListVal.SelectedIndex;
                string[] strArray = this.toRunList.ToArray();
                if (index >= 0)
                {
                    this.autoTestToRunScriptsListVal.Items.Remove(this.autoTestToRunScriptsListVal.SelectedItem);
                    this.toRunList.Remove(strArray[index]);
                    if ((fileIdx > 0) && (fileIdx > index))
                    {
                        fileIdx--;
                    }
                    this.autoTestUpdateTestCnt();
                }
            }
        }

        private void autoTestToRunScriptsListVal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.autoTestToRunScriptsListVal.SelectedIndex >= 0)
            {
                if (!this._running)
                {
                    this.autoTestToRunScriptsListVal.CheckOnClick = false;
                    this.autoTestToRunScriptsListVal.SetItemChecked(this.autoTestToRunScriptsListVal.SelectedIndex, false);
                }
                else if (this.autoTestToRunScriptsListVal.SelectedIndex > this._currentIndex)
                {
                    this.autoTestToRunScriptsListVal.CheckOnClick = false;
                    this.autoTestToRunScriptsListVal.SetItemChecked(this.autoTestToRunScriptsListVal.SelectedIndex, false);
                }
                else
                {
                    this.autoTestToRunScriptsListVal.CheckOnClick = false;
                    this.autoTestToRunScriptsListVal.SetItemChecked(this.autoTestToRunScriptsListVal.SelectedIndex, true);
                }
            }
        }

        private void autoTestUpdateTestCnt()
        {
            string str = string.Format("{0}", this.autoTestToRunScriptsListVal.Items.Count);
            this.frmAutoTestSciptCntLabel.Text = str;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmAutomationTests_DoubleClick(object sender, EventArgs e)
        {
            base.Height = 0x253;
            base.Width = 0x31b;
        }

        private void frmAutomationTests_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(this.autoTestDirVal.Text))
            {
                this.updateAvailableScript();
            }
        }

        public static frmAutomationTests GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmAutomationTests();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmAutomationTests));
            this.autoTestDirBrowser = new Button();
            this.autoTestFilePathLabel = new Label();
            this.autoTestDirVal = new TextBox();
            this.autoTestAvailableScriptsListVal = new ListBox();
            this.autoTestAvailableScriptsLabel = new Label();
            this.autoTestRunScriptsLabel = new Label();
            this.autoTestExitBtn = new Button();
            this.autoTestRunBtn = new Button();
            this.autoTestAddBtn = new Button();
            this.autoTestAddAllBtn = new Button();
            this.autoTestRemoveBtn = new Button();
            this.autoTestRemoveAllBtn = new Button();
            this.autoTestSetupCfgBtn = new Button();
            this.autoTestClearAvailableListBtn = new Button();
            this.autoTestSendEmailChkVal = new CheckBox();
            this.autoTestEmailListVal = new TextBox();
            this.autoTestEmailListLabel = new Label();
            this.automationTestAbortBtn = new Button();
            this.frmAutoTestSciptCntLabel = new Label();
            this.autoTestRefreshBtn = new Button();
            this.autoTestScriptCfgBtn = new Button();
            this.autoTestToRunScriptsListVal = new CheckedListBox();
            base.SuspendLayout();
            this.autoTestDirBrowser.Location = new Point(0x2c0, 40);
            this.autoTestDirBrowser.Name = "autoTestDirBrowser";
            this.autoTestDirBrowser.Size = new Size(0x1a, 0x17);
            this.autoTestDirBrowser.TabIndex = 1;
            this.autoTestDirBrowser.Text = "...";
            this.autoTestDirBrowser.UseVisualStyleBackColor = true;
            this.autoTestDirBrowser.Click += new EventHandler(this.autoTestDirectoryBrowser_Click);
            this.autoTestFilePathLabel.AutoSize = true;
            this.autoTestFilePathLabel.Location = new Point(0x18, 20);
            this.autoTestFilePathLabel.Name = "autoTestFilePathLabel";
            this.autoTestFilePathLabel.Size = new Size(0x6c, 13);
            this.autoTestFilePathLabel.TabIndex = 0;
            this.autoTestFilePathLabel.Text = "Test Scripts Directory";
            this.autoTestDirVal.Location = new Point(0x1b, 0x29);
            this.autoTestDirVal.Name = "autoTestDirVal";
            this.autoTestDirVal.Size = new Size(0x298, 20);
            this.autoTestDirVal.TabIndex = 0;
            this.autoTestAvailableScriptsListVal.FormattingEnabled = true;
            this.autoTestAvailableScriptsListVal.HorizontalScrollbar = true;
            this.autoTestAvailableScriptsListVal.Location = new Point(0x1b, 120);
            this.autoTestAvailableScriptsListVal.Name = "autoTestAvailableScriptsListVal";
            this.autoTestAvailableScriptsListVal.ScrollAlwaysVisible = true;
            this.autoTestAvailableScriptsListVal.Size = new Size(290, 0x13c);
            this.autoTestAvailableScriptsListVal.TabIndex = 4;
            this.autoTestAvailableScriptsListVal.DoubleClick += new EventHandler(this.autoTestAvailableScriptsListVal_DoubleClick);
            this.autoTestAvailableScriptsListVal.KeyPress += new KeyPressEventHandler(this.autoTestAvailableScriptsListVal_KeyPress);
            this.autoTestAvailableScriptsLabel.AutoSize = true;
            this.autoTestAvailableScriptsLabel.Location = new Point(0x18, 0x5e);
            this.autoTestAvailableScriptsLabel.Name = "autoTestAvailableScriptsLabel";
            this.autoTestAvailableScriptsLabel.Size = new Size(0x58, 13);
            this.autoTestAvailableScriptsLabel.TabIndex = 3;
            this.autoTestAvailableScriptsLabel.Text = "Available Scripts:";
            this.autoTestRunScriptsLabel.AutoSize = true;
            this.autoTestRunScriptsLabel.Location = new Point(0x1ba, 0x5e);
            this.autoTestRunScriptsLabel.Name = "autoTestRunScriptsLabel";
            this.autoTestRunScriptsLabel.Size = new Size(0x51, 13);
            this.autoTestRunScriptsLabel.TabIndex = 15;
            this.autoTestRunScriptsLabel.Text = "Scripts To Run:";
            this.autoTestExitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.autoTestExitBtn.Location = new Point(0x1c0, 0x210);
            this.autoTestExitBtn.Name = "autoTestExitBtn";
            this.autoTestExitBtn.Size = new Size(0x4b, 0x17);
            this.autoTestExitBtn.TabIndex = 0x10;
            this.autoTestExitBtn.Text = "E&xit";
            this.autoTestExitBtn.UseVisualStyleBackColor = true;
            this.autoTestExitBtn.Click += new EventHandler(this.autoTestExitBtn_Click);
            this.autoTestRunBtn.Location = new Point(0xf4, 0x210);
            this.autoTestRunBtn.Name = "autoTestRunBtn";
            this.autoTestRunBtn.Size = new Size(0x4b, 0x17);
            this.autoTestRunBtn.TabIndex = 14;
            this.autoTestRunBtn.Text = "&Start";
            this.autoTestRunBtn.UseVisualStyleBackColor = true;
            this.autoTestRunBtn.Click += new EventHandler(this.autoTestRunBtn_Click);
            this.autoTestAddBtn.Location = new Point(0x14d, 190);
            this.autoTestAddBtn.Name = "autoTestAddBtn";
            this.autoTestAddBtn.Size = new Size(0x63, 0x17);
            this.autoTestAddBtn.TabIndex = 5;
            this.autoTestAddBtn.Text = "&Add >";
            this.autoTestAddBtn.UseVisualStyleBackColor = true;
            this.autoTestAddBtn.Click += new EventHandler(this.autoTestAddBtn_Click);
            this.autoTestAddAllBtn.Location = new Point(0x14d, 0xdb);
            this.autoTestAddAllBtn.Name = "autoTestAddAllBtn";
            this.autoTestAddAllBtn.Size = new Size(0x63, 0x17);
            this.autoTestAddAllBtn.TabIndex = 6;
            this.autoTestAddAllBtn.Text = "A&dd All >>";
            this.autoTestAddAllBtn.UseVisualStyleBackColor = true;
            this.autoTestAddAllBtn.Click += new EventHandler(this.autoTestAddAllBtn_Click);
            this.autoTestRemoveBtn.Location = new Point(0x14d, 0xf8);
            this.autoTestRemoveBtn.Name = "autoTestRemoveBtn";
            this.autoTestRemoveBtn.Size = new Size(0x63, 0x17);
            this.autoTestRemoveBtn.TabIndex = 7;
            this.autoTestRemoveBtn.Text = "&Remove <";
            this.autoTestRemoveBtn.UseVisualStyleBackColor = true;
            this.autoTestRemoveBtn.Click += new EventHandler(this.autoTestRemoveBtn_Click);
            this.autoTestRemoveAllBtn.Location = new Point(0x14d, 0x115);
            this.autoTestRemoveAllBtn.Name = "autoTestRemoveAllBtn";
            this.autoTestRemoveAllBtn.Size = new Size(0x63, 0x17);
            this.autoTestRemoveAllBtn.TabIndex = 8;
            this.autoTestRemoveAllBtn.Text = "Re&move All <<";
            this.autoTestRemoveAllBtn.UseVisualStyleBackColor = true;
            this.autoTestRemoveAllBtn.Click += new EventHandler(this.autoTestRemoveAllBtn_Click);
            this.autoTestSetupCfgBtn.Location = new Point(0x14d, 0x17b);
            this.autoTestSetupCfgBtn.Name = "autoTestSetupCfgBtn";
            this.autoTestSetupCfgBtn.Size = new Size(0x63, 0x17);
            this.autoTestSetupCfgBtn.TabIndex = 10;
            this.autoTestSetupCfgBtn.Text = "Se&tup Config";
            this.autoTestSetupCfgBtn.UseVisualStyleBackColor = true;
            this.autoTestSetupCfgBtn.Click += new EventHandler(this.autoTestSetupConfigBtn_Click);
            this.autoTestClearAvailableListBtn.Location = new Point(0xf1, 0x59);
            this.autoTestClearAvailableListBtn.Name = "autoTestClearAvailableListBtn";
            this.autoTestClearAvailableListBtn.Size = new Size(0x4b, 0x17);
            this.autoTestClearAvailableListBtn.TabIndex = 3;
            this.autoTestClearAvailableListBtn.Text = "&Clear";
            this.autoTestClearAvailableListBtn.UseVisualStyleBackColor = true;
            this.autoTestClearAvailableListBtn.Click += new EventHandler(this.autoTestClearAvailableListBtn_Click);
            this.autoTestSendEmailChkVal.AutoSize = true;
            this.autoTestSendEmailChkVal.Location = new Point(0x28f, 0x1bf);
            this.autoTestSendEmailChkVal.Name = "autoTestSendEmailChkVal";
            this.autoTestSendEmailChkVal.Size = new Size(0x55, 0x11);
            this.autoTestSendEmailChkVal.TabIndex = 12;
            this.autoTestSendEmailChkVal.Text = "Send &Email?";
            this.autoTestSendEmailChkVal.UseVisualStyleBackColor = true;
            this.autoTestEmailListVal.Location = new Point(0x1b, 0x1d9);
            this.autoTestEmailListVal.Multiline = true;
            this.autoTestEmailListVal.Name = "autoTestEmailListVal";
            this.autoTestEmailListVal.Size = new Size(0x2c9, 40);
            this.autoTestEmailListVal.TabIndex = 13;
            this.autoTestEmailListVal.TextChanged += new EventHandler(this.autoTestEmailListVal_TextChanged);
            this.autoTestEmailListLabel.AutoSize = true;
            this.autoTestEmailListLabel.Location = new Point(0x18, 0x1c3);
            this.autoTestEmailListLabel.Name = "autoTestEmailListLabel";
            this.autoTestEmailListLabel.Size = new Size(0x33, 13);
            this.autoTestEmailListLabel.TabIndex = 0x12;
            this.autoTestEmailListLabel.Text = "Email List";
            this.automationTestAbortBtn.Location = new Point(0x15a, 0x210);
            this.automationTestAbortBtn.Name = "automationTestAbortBtn";
            this.automationTestAbortBtn.Size = new Size(0x4b, 0x17);
            this.automationTestAbortBtn.TabIndex = 15;
            this.automationTestAbortBtn.Text = "A&bort";
            this.automationTestAbortBtn.UseVisualStyleBackColor = true;
            this.automationTestAbortBtn.Click += new EventHandler(this.autoTestAbortBtn_Click);
            this.frmAutoTestSciptCntLabel.AutoSize = true;
            this.frmAutoTestSciptCntLabel.Location = new Point(0x20e, 0x5e);
            this.frmAutoTestSciptCntLabel.Name = "frmAutoTestSciptCntLabel";
            this.frmAutoTestSciptCntLabel.Size = new Size(13, 13);
            this.frmAutoTestSciptCntLabel.TabIndex = 0x10;
            this.frmAutoTestSciptCntLabel.Text = "0";
            this.autoTestRefreshBtn.Location = new Point(150, 0x59);
            this.autoTestRefreshBtn.Name = "autoTestRefreshBtn";
            this.autoTestRefreshBtn.Size = new Size(0x4b, 0x17);
            this.autoTestRefreshBtn.TabIndex = 2;
            this.autoTestRefreshBtn.Text = "&Refresh";
            this.autoTestRefreshBtn.UseVisualStyleBackColor = true;
            this.autoTestRefreshBtn.Click += new EventHandler(this.autoTestRefreshBtn_Click);
            this.autoTestScriptCfgBtn.Location = new Point(0x14d, 0x19c);
            this.autoTestScriptCfgBtn.Name = "autoTestScriptCfgBtn";
            this.autoTestScriptCfgBtn.Size = new Size(0x63, 0x17);
            this.autoTestScriptCfgBtn.TabIndex = 11;
            this.autoTestScriptCfgBtn.Text = "Scri&pt Config";
            this.autoTestScriptCfgBtn.UseVisualStyleBackColor = true;
            this.autoTestScriptCfgBtn.Click += new EventHandler(this.autoTestScriptCfgBtn_Click);
            this.autoTestToRunScriptsListVal.CheckOnClick = true;
            this.autoTestToRunScriptsListVal.FormattingEnabled = true;
            this.autoTestToRunScriptsListVal.HorizontalScrollbar = true;
            this.autoTestToRunScriptsListVal.Location = new Point(0x1bd, 120);
            this.autoTestToRunScriptsListVal.Name = "autoTestToRunScriptsListVal";
            this.autoTestToRunScriptsListVal.ScrollAlwaysVisible = true;
            this.autoTestToRunScriptsListVal.Size = new Size(290, 0x13f);
            this.autoTestToRunScriptsListVal.TabIndex = 9;
            this.autoTestToRunScriptsListVal.SelectedIndexChanged += new EventHandler(this.autoTestToRunScriptsListVal_SelectedIndexChanged);
            this.autoTestToRunScriptsListVal.DoubleClick += new EventHandler(this.autoTestToRunScriptsListVal_DoubleClick);
            base.AcceptButton = this.autoTestRunBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.autoTestExitBtn;
            base.ClientSize = new Size(0x2ff, 0x238);
            base.Controls.Add(this.autoTestToRunScriptsListVal);
            base.Controls.Add(this.autoTestScriptCfgBtn);
            base.Controls.Add(this.frmAutoTestSciptCntLabel);
            base.Controls.Add(this.autoTestEmailListLabel);
            base.Controls.Add(this.autoTestEmailListVal);
            base.Controls.Add(this.autoTestSendEmailChkVal);
            base.Controls.Add(this.autoTestRefreshBtn);
            base.Controls.Add(this.autoTestClearAvailableListBtn);
            base.Controls.Add(this.autoTestSetupCfgBtn);
            base.Controls.Add(this.automationTestAbortBtn);
            base.Controls.Add(this.autoTestRunBtn);
            base.Controls.Add(this.autoTestRemoveAllBtn);
            base.Controls.Add(this.autoTestRemoveBtn);
            base.Controls.Add(this.autoTestAddBtn);
            base.Controls.Add(this.autoTestAddAllBtn);
            base.Controls.Add(this.autoTestExitBtn);
            base.Controls.Add(this.autoTestAvailableScriptsListVal);
            base.Controls.Add(this.autoTestDirBrowser);
            base.Controls.Add(this.autoTestRunScriptsLabel);
            base.Controls.Add(this.autoTestAvailableScriptsLabel);
            base.Controls.Add(this.autoTestFilePathLabel);
            base.Controls.Add(this.autoTestDirVal);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmAutomationTests";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Automation Tests";
            base.Load += new EventHandler(this.frmAutomationTests_Load);
            base.DoubleClick += new EventHandler(this.frmAutomationTests_DoubleClick);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.saveNExit();
            if (this.autoTestThread != null)
            {
                this.autoTestThread.Abort();
                clsGlobal.ScriptDone = true;
            }
            m_SChildform = null;
        }

        private int saveNExit()
        {
            int num = 0;
            IniHelper helper = new IniHelper(this.automationIniPath);
            string str = string.Empty;
            foreach (string str2 in this.scriptsFullPathLists)
            {
                str = str + str2 + ",";
            }
            str = str.TrimEnd(new char[] { ',' });
            if (str.Length != 0)
            {
                str.Replace(" ", "");
            }
            if (File.Exists(this.automationIniPath))
            {
                if ((File.GetAttributes(this.automationIniPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("{0} File is read only!\nPlease change property and retry", this.automationIniPath), "Error");
                    return 1;
                }
                helper.IniWriteValue("SETUP", "AVAILABLE_SCRIPTS", str);
                helper.IniWriteValue("SETUP", "SCRIPTS_DIR", this.autoTestDirVal.Text);
                if (this.autoTestSendEmailChkVal.Checked)
                {
                    helper.IniWriteValue("SETUP", "SEND_EMAIL", "1");
                }
                else
                {
                    helper.IniWriteValue("SETUP", "SEND_EMAIL", "0");
                }
                string str4 = this.autoTestEmailListVal.Text.Replace(" ", "");
                helper.IniWriteValue("EMAIL", "Email.Recipient", str4);
                return num;
            }
            StreamWriter writer = File.CreateText(this.automationIniPath);
            writer.WriteLine("[EMAIL]");
            writer.WriteLine("Email.Recipient={0}", this.autoTestEmailListVal.Text.Replace(" ", ""));
            writer.WriteLine("Email.Sender=\"STING@sirf.com\"");
            writer.WriteLine("Email.Copy=\"\"");
            writer.WriteLine("Email.Priority=\"MailPriority.Normal\"");
            writer.WriteLine("Email.Encoding=\"Encoding.ASCII\"");
            writer.WriteLine("Email.Subject=\"SiRFLive Auto Test Report\"");
            writer.WriteLine("Email.Message=\"\nBest Regards\n\n SiRFLive Team\"");
            writer.WriteLine("Email.Smtp=\"192.168.2.254\"");
            writer.WriteLine("[SETUP]");
            writer.WriteLine("RESET_PERIOD_RANDOMIZATION_SEC=5");
            writer.WriteLine("SCRIPTS_DIR={0}", this.autoTestDirVal.Text);
            writer.WriteLine("AVAILABLE_SCRIPTS={0}", str);
            if (this.autoTestSendEmailChkVal.Checked)
            {
                writer.WriteLine("SEND_EMAIL=1");
            }
            else
            {
                writer.WriteLine("SEND_EMAIL=0");
            }
            writer.Close();
            return num;
        }

        private void setAutoTestToRunScriptsListValCheckBox(int idx, bool state)
        {
            EventHandler method = null;
            if (idx < 0)
            {
                EventHandler handler = null;
                for (int i = 0; i < (this.autoTestToRunScriptsListVal.Items.Count - 1); i++)
                {
                    if (handler == null)
                    {
                        handler = delegate {
                            this.autoTestToRunScriptsListVal.SetItemChecked(i, state);
                        };
                    }
                    this.autoTestToRunScriptsListVal.Invoke(handler);
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.autoTestToRunScriptsListVal.SetItemChecked(idx, state);
                    };
                }
                this.autoTestToRunScriptsListVal.Invoke(method);
            }
        }

        private void updateAvailableScript()
        {
            this.autoTestAvailableScriptsListVal.Items.Clear();
            this.scriptsFullPathLists.Clear();
            try
            {
                foreach (string str in Directory.GetFiles(this.autoTestDirVal.Text))
                {
                    if (str.EndsWith(".py"))
                    {
                        this.autoTestAddAvailableScripts(str);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!");
            }
            this.scriptsArray = this.scriptsFullPathLists.ToArray();
        }
    }
}

