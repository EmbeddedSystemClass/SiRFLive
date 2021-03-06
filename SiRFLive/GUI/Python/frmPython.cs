﻿namespace SiRFLive.GUI.Python
{
    using IronPython.Hosting;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class frmPython : Form
    {
        private bool _isEngineGlobalInitialized;
        private IContainer components;
        public PythonEngine engine;
        private PythonHistory history = new PythonHistory();
        private static frmPython m_SChildform;
        public ListBoxStream PythonEngineOutput;
        private TextBox pythonInput;
        private ListBox pythonOutput;
        public string testResult = string.Empty;

        public frmPython()
        {
            this.InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            this.InitializePythonEngine();
            this.InitPythonEngineGlobals();
            m_SChildform = this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmPython_DoubleClick(object sender, EventArgs e)
        {
            base.Height = 300;
            base.Width = 400;
        }

        private void frmPython_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!clsGlobal.ScriptDone)
            {
                e.Cancel = true;
            }
        }

        private void frmPython_Load(object sender, EventArgs e)
        {
        }

        public static frmPython GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmPython();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmPython));
            this.pythonInput = new TextBox();
            this.pythonOutput = new ListBox();
            base.SuspendLayout();
            this.pythonInput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.pythonInput.Location = new Point(13, 0x197);
            this.pythonInput.Margin = new Padding(10);
            this.pythonInput.Name = "pythonInput";
            this.pythonInput.Size = new Size(0x2a5, 20);
            this.pythonInput.TabIndex = 1;
            this.pythonInput.TextChanged += new EventHandler(this.pythonInput_TextChanged);
            this.pythonInput.KeyDown += new KeyEventHandler(this.pythonInput_KeyDown);
            this.pythonOutput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pythonOutput.FormattingEnabled = true;
            this.pythonOutput.HorizontalScrollbar = true;
            this.pythonOutput.Location = new Point(13, 13);
            this.pythonOutput.Name = "pythonOutput";
            this.pythonOutput.Size = new Size(0x2a5, 0x17d);
            this.pythonOutput.TabIndex = 0;
            this.pythonOutput.SelectedIndexChanged += new EventHandler(this.pythonOutput_SelectedIndexChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x2be, 0x1b7);
            base.Controls.Add(this.pythonOutput);
            base.Controls.Add(this.pythonInput);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmPython";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Output Console";
            base.Load += new EventHandler(this.frmPython_Load);
            base.DoubleClick += new EventHandler(this.frmPython_DoubleClick);
            base.FormClosing += new FormClosingEventHandler(this.frmPython_FormClosing);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void InitializePythonEngine()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                this.PythonEngineOutput = new ListBoxStream(ref this.pythonOutput);
                this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add("Initializing Python Engine...");
                EngineOptions engineOptions = new EngineOptions();
                engineOptions.ClrDebuggingEnabled = true;
                this.engine = new PythonEngine(engineOptions);
                this.engine.AddToPath(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\PythonStdLib");
                this.engine.AddToPath(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts");
                this.engine.AddToPath(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts\core");
                this.engine.AddToPath(Environment.CurrentDirectory);
                this.engine.AddToPath(Path.GetDirectoryName(Application.ExecutablePath));
                this.engine.Import("Site");
                this.engine.Import("System");
                this.engine.Import("System.Diagnostics");
                this.engine.Import("sys");
                this.engine.Import("nt");
                this.engine.Import("os");
                this.engine.Import("shutil");
                this.engine.Import("ConfigParser");
                this.engine.Import("time");
                this.engine.Import("array");
                this.engine.Import("pdb");
                this.engine.Execute("from System import Array");
                this.engine.Import("clr");
                this.engine.Execute("clr.AddReference('System.Messaging')");
                this.engine.Execute("from System.Messaging import MessageQueue");
                this.engine.Execute("MessageQueue.EnableConnectionCache = False");
                this.engine.Import("clr");
                this.engine.Execute("clr.AddReferenceByPartialName('CommonClassLibrary')");
                this.engine.Execute("from CommonClassLibrary import *");
                this.engine.SetStandardOutput(this.PythonEngineOutput);
                this.engine.SetStandardError(this.PythonEngineOutput);
                this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add("Python Engine Running.");
                this.engine.Execute("import clr");
                this.engine.Execute("clr.AddReferenceByPartialName('SiRFLive')");
                this.engine.Execute("from SiRFLive import *");
                this.engine.Execute("print dir()");
                this.engine.Execute("nt.chdir(\"..\\scripts\")");
            }
            catch (Exception exception)
            {
                this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add("Python Engine Failed to Start Properly: " + exception.Message);
            }
        }

        public void InitPythonEngineGlobals()
        {
            try
            {
                if ((this.engine != null) && !this._isEngineGlobalInitialized)
                {
                    this.engine.DefaultModule.Globals.Add("test", 0x4d2);
                    this.engine.DefaultModule.Globals.Add("pythonConsole", this);
                    this.engine.DefaultModule.Globals.Add("mainFrame", clsGlobal.g_objfrmMDIMain);
                    this._isEngineGlobalInitialized = true;
                }
            }
            catch (Exception exception)
            {
                this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add("Python Engine Failed to Start Properly: " + exception.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (clsGlobal.ScriptDone)
            {
                m_SChildform = null;
            }
        }

        private void pythonInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string o = new string(this.pythonInput.Text.ToCharArray());
                    this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add(">>> " + this.pythonInput.Text);
                    if (o.Contains("RUNFILE "))
                    {
                        this.history.Add(o);
                        o = o.Remove(0, 8);
                        this.engine.ExecuteFile(o);
                    }
                    else if (o.Contains("LOADHISTORY"))
                    {
                        o = o.Remove(0, 12);
                        this.history.LoadHistory(o);
                    }
                    else if (o.Contains("SAVEHISTORY"))
                    {
                        o = o.Remove(0, 12);
                        this.history.SaveHistory(o);
                    }
                    else if (o.Contains("CLEARHISTORY"))
                    {
                        this.history.ClearHistory();
                    }
                    else
                    {
                        this.engine.Execute(this.pythonInput.Text);
                        this.history.Add(o);
                    }
                }
                catch (Exception exception)
                {
                    this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add("IronPython engine threw exception: " + exception.Message);
                }
                this.pythonInput.ResetText();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.pythonInput.Text = this.history.Prev();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.pythonInput.Text = this.history.Next();
            }
        }

        private void pythonInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void pythonOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.pythonOutput.Items.Count > 100)
            {
                this.pythonOutput.Items.Clear();
            }
        }

        public void WriteLine(string input)
        {
			this.pythonOutput.BeginInvoke((MethodInvoker)delegate
			{
                this.pythonOutput.SelectedIndex = this.pythonOutput.Items.Add(">>> " + input);
            });
        }

        public bool IsEngineGlobalInitialized
        {
            get
            {
                return this._isEngineGlobalInitialized;
            }
            set
            {
                this._isEngineGlobalInitialized = value;
            }
        }
    }
}

