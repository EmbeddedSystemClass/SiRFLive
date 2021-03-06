﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.MessageHandling;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmRXInit : Form
    {
        private byte _CfgBits;
        private Button button_OK;
        private CheckBox checkBox_enable_Devlopment_Data;
        private CheckBox checkBox_enable_Navlib_Data;
        private CheckBox checkBox_enableFullSystemReset;
        private IContainer components;
        internal ObjectInterface cpGuiCtrl = new ObjectInterface();
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private static frmRXInit m_SChildform;
        internal string outputStr = string.Empty;
        private RadioButton radioButton_ColdStart;
        private RadioButton radioButton_Factory_Reset;
        private RadioButton radioButton_Factory_Reset_XO;
        private RadioButton radioButton_HotStart;
        private RadioButton radioButton_WarmStart_Init;
        private RadioButton radioButton_WarmStart_noInit;
        private SSB_Format SSBBitMapConstruct = new SSB_Format();

        internal event updateParentEventHandler updateParent;

        public frmRXInit()
        {
            this.InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.outputStr = string.Empty;
            string reset = string.Empty;
            if (this.radioButton_HotStart.Checked)
            {
                reset = "RESET_SSB_HOT";
            }
            else if (this.radioButton_WarmStart_noInit.Checked)
            {
                reset = "RESET_SSB_WARM_NO_INIT";
            }
            else if (this.radioButton_WarmStart_Init.Checked)
            {
                reset = "RESET_SSB_WARM_INIT";
            }
            else if (this.radioButton_ColdStart.Checked)
            {
                reset = "RESET_SSB_COLD";
            }
            else if (this.radioButton_Factory_Reset.Checked)
            {
                reset = "RESET_SSB_FACTORY";
            }
            else if (this.radioButton_Factory_Reset_XO.Checked)
            {
                reset = "RESET_SSB_FACTORY_XO";
            }
            else
            {
                reset = "RESET_SSB_COLD";
            }
            this._CfgBits = this.SSBBitMapConstruct.GetResetBitMap(reset, this.checkBox_enable_Navlib_Data.Checked, this.checkBox_enable_Devlopment_Data.Checked, this.checkBox_enableFullSystemReset.Checked);
            this.outputStr = this._CfgBits.ToString();
            if ((this.updateParent != null) && (this.outputStr != string.Empty))
            {
                this.updateParent(this.outputStr);
            }
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmRXInit_Load(object sender, EventArgs e)
        {
            this.updateControlsForFactoryReset();
        }

        public static frmRXInit GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRXInit();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmRXInit));
            this.groupBox1 = new GroupBox();
            this.radioButton_Factory_Reset_XO = new RadioButton();
            this.radioButton_Factory_Reset = new RadioButton();
            this.radioButton_ColdStart = new RadioButton();
            this.radioButton_WarmStart_Init = new RadioButton();
            this.radioButton_WarmStart_noInit = new RadioButton();
            this.radioButton_HotStart = new RadioButton();
            this.groupBox2 = new GroupBox();
            this.checkBox_enable_Devlopment_Data = new CheckBox();
            this.checkBox_enable_Navlib_Data = new CheckBox();
            this.checkBox_enableFullSystemReset = new CheckBox();
            this.button_OK = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.radioButton_Factory_Reset_XO);
            this.groupBox1.Controls.Add(this.radioButton_Factory_Reset);
            this.groupBox1.Controls.Add(this.radioButton_ColdStart);
            this.groupBox1.Controls.Add(this.radioButton_WarmStart_Init);
            this.groupBox1.Controls.Add(this.radioButton_WarmStart_noInit);
            this.groupBox1.Controls.Add(this.radioButton_HotStart);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(180, 170);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reset Mode";
            this.radioButton_Factory_Reset_XO.AutoSize = true;
            this.radioButton_Factory_Reset_XO.Location = new Point(8, 140);
            this.radioButton_Factory_Reset_XO.Name = "radioButton_Factory_Reset_XO";
            this.radioButton_Factory_Reset_XO.Size = new Size(150, 0x11);
            this.radioButton_Factory_Reset_XO.TabIndex = 5;
            this.radioButton_Factory_Reset_XO.TabStop = true;
            this.radioButton_Factory_Reset_XO.Text = "Factory (clear XO learning)";
            this.radioButton_Factory_Reset_XO.UseVisualStyleBackColor = true;
            this.radioButton_Factory_Reset_XO.CheckedChanged += new EventHandler(this.radioButton_Factory_Reset_XO_CheckedChanged);
            this.radioButton_Factory_Reset.AutoSize = true;
            this.radioButton_Factory_Reset.Location = new Point(8, 0x74);
            this.radioButton_Factory_Reset.Name = "radioButton_Factory_Reset";
            this.radioButton_Factory_Reset.Size = new Size(0x5b, 0x11);
            this.radioButton_Factory_Reset.TabIndex = 4;
            this.radioButton_Factory_Reset.TabStop = true;
            this.radioButton_Factory_Reset.Text = "Factory Reset";
            this.radioButton_Factory_Reset.UseVisualStyleBackColor = true;
            this.radioButton_Factory_Reset.CheckedChanged += new EventHandler(this.radioButton_Factory_Reset_CheckedChanged);
            this.radioButton_ColdStart.AutoSize = true;
            this.radioButton_ColdStart.Location = new Point(8, 0x5c);
            this.radioButton_ColdStart.Name = "radioButton_ColdStart";
            this.radioButton_ColdStart.Size = new Size(0x47, 0x11);
            this.radioButton_ColdStart.TabIndex = 3;
            this.radioButton_ColdStart.TabStop = true;
            this.radioButton_ColdStart.Text = "Cold Start";
            this.radioButton_ColdStart.UseVisualStyleBackColor = true;
            this.radioButton_WarmStart_Init.AutoSize = true;
            this.radioButton_WarmStart_Init.Location = new Point(8, 0x44);
            this.radioButton_WarmStart_Init.Name = "radioButton_WarmStart_Init";
            this.radioButton_WarmStart_Init.Size = new Size(0x65, 0x11);
            this.radioButton_WarmStart_Init.TabIndex = 2;
            this.radioButton_WarmStart_Init.TabStop = true;
            this.radioButton_WarmStart_Init.Text = "Warm Start (Init)";
            this.radioButton_WarmStart_Init.UseVisualStyleBackColor = true;
            this.radioButton_WarmStart_noInit.AutoSize = true;
            this.radioButton_WarmStart_noInit.Location = new Point(8, 0x2c);
            this.radioButton_WarmStart_noInit.Name = "radioButton_WarmStart_noInit";
            this.radioButton_WarmStart_noInit.Size = new Size(0x76, 0x11);
            this.radioButton_WarmStart_noInit.TabIndex = 1;
            this.radioButton_WarmStart_noInit.TabStop = true;
            this.radioButton_WarmStart_noInit.Text = "Warm Start (No Init)";
            this.radioButton_WarmStart_noInit.UseVisualStyleBackColor = true;
            this.radioButton_HotStart.AutoSize = true;
            this.radioButton_HotStart.Location = new Point(8, 20);
            this.radioButton_HotStart.Name = "radioButton_HotStart";
            this.radioButton_HotStart.Size = new Size(0x43, 0x11);
            this.radioButton_HotStart.TabIndex = 0;
            this.radioButton_HotStart.TabStop = true;
            this.radioButton_HotStart.Text = "Hot Start";
            this.radioButton_HotStart.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.checkBox_enable_Devlopment_Data);
            this.groupBox2.Controls.Add(this.checkBox_enable_Navlib_Data);
            this.groupBox2.Location = new Point(12, 0xbc);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(180, 0x4b);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            this.checkBox_enable_Devlopment_Data.AutoSize = true;
            this.checkBox_enable_Devlopment_Data.Location = new Point(7, 0x2b);
            this.checkBox_enable_Devlopment_Data.Name = "checkBox_enable_Devlopment_Data";
            this.checkBox_enable_Devlopment_Data.Size = new Size(0x97, 0x11);
            this.checkBox_enable_Devlopment_Data.TabIndex = 1;
            this.checkBox_enable_Devlopment_Data.Text = "Enable Development Data";
            this.checkBox_enable_Devlopment_Data.UseVisualStyleBackColor = true;
            this.checkBox_enable_Navlib_Data.AutoSize = true;
            this.checkBox_enable_Navlib_Data.Location = new Point(7, 0x13);
            this.checkBox_enable_Navlib_Data.Name = "checkBox_enable_Navlib_Data";
            this.checkBox_enable_Navlib_Data.Size = new Size(0x76, 0x11);
            this.checkBox_enable_Navlib_Data.TabIndex = 0;
            this.checkBox_enable_Navlib_Data.Text = "Enable Navlib Data";
            this.checkBox_enable_Navlib_Data.UseVisualStyleBackColor = true;
            this.checkBox_enableFullSystemReset.AutoSize = true;
            this.checkBox_enableFullSystemReset.Location = new Point(0x13, 0x117);
            this.checkBox_enableFullSystemReset.Name = "checkBox_enableFullSystemReset";
            this.checkBox_enableFullSystemReset.Size = new Size(0x92, 0x11);
            this.checkBox_enableFullSystemReset.TabIndex = 2;
            this.checkBox_enableFullSystemReset.Text = "Enable Full System Reset";
            this.checkBox_enableFullSystemReset.UseVisualStyleBackColor = true;
            this.button_OK.Location = new Point(0x45, 0x13a);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new Size(0x4b, 0x17);
            this.button_OK.TabIndex = 3;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new EventHandler(this.button_OK_Click);
            base.AcceptButton = this.button_OK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0xcc, 0x15d);
            base.Controls.Add(this.button_OK);
            base.Controls.Add(this.checkBox_enableFullSystemReset);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmRXInit";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Receiver Init";
            base.Load += new EventHandler(this.frmRXInit_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
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

        private void updateControlsForFactoryReset()
        {
            if (this.radioButton_Factory_Reset.Checked || this.radioButton_Factory_Reset_XO.Checked)
            {
                this.cpGuiCtrl.SetCheckBoxState(this.checkBox_enable_Navlib_Data, false);
                this.cpGuiCtrl.SetCheckBoxState(this.checkBox_enable_Devlopment_Data, false);
            }
            else
            {
                this.cpGuiCtrl.SetCheckBoxState(this.checkBox_enable_Navlib_Data, true);
                this.cpGuiCtrl.SetCheckBoxState(this.checkBox_enable_Devlopment_Data, true);
            }
        }

        internal delegate void updateParentEventHandler(string updatedData);
    }
}

