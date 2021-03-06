﻿namespace SiRFLive.GUI.Automation
{
    using SiRFLive.TestAutomation;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmUpdateTestLogDir : Form
    {
        private IContainer components;
        internal e_configParams elmType = e_configParams.E_CONFIG_UNKNOWN;
        private static frmUpdateTestLogDir m_SChildform;
        private Button updateTestLogCancelBut;
        private Button updateTestLogDirBrowser;
        private TextBox updateTestLogDirVal;
        private Button updateTestLogDoneBut;
        private Label updateTestLogPathLabel;

        internal event updateParentEventHandler updateParent;

        internal frmUpdateTestLogDir(e_configParams type)
        {
            this.InitializeComponent();
            this.elmType = type;
            switch (type)
            {
                case e_configParams.E_LOG_DIR:
                    this.updateTestLogPathLabel.Text = "Base Log Directory";
                    return;

                case e_configParams.E_HOST_DIR:
                    this.updateTestLogPathLabel.Text = "Host/Patch App Directory";
                    return;
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

        internal static frmUpdateTestLogDir GetChildInstance(e_configParams type)
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmUpdateTestLogDir(type);
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmUpdateTestLogDir));
            this.updateTestLogDirBrowser = new Button();
            this.updateTestLogPathLabel = new Label();
            this.updateTestLogDirVal = new TextBox();
            this.updateTestLogDoneBut = new Button();
            this.updateTestLogCancelBut = new Button();
            base.SuspendLayout();
            this.updateTestLogDirBrowser.Location = new Point(0x1de, 0x30);
            this.updateTestLogDirBrowser.Name = "updateTestLogDirBrowser";
            this.updateTestLogDirBrowser.Size = new Size(0x1a, 0x17);
            this.updateTestLogDirBrowser.TabIndex = 2;
            this.updateTestLogDirBrowser.Text = "...";
            this.updateTestLogDirBrowser.UseVisualStyleBackColor = true;
            this.updateTestLogDirBrowser.Click += new EventHandler(this.updateTestLogDirBrowser_Click);
            this.updateTestLogPathLabel.AutoSize = true;
            this.updateTestLogPathLabel.Location = new Point(0x19, 0x1d);
            this.updateTestLogPathLabel.Name = "updateTestLogPathLabel";
            this.updateTestLogPathLabel.Size = new Size(0x31, 13);
            this.updateTestLogPathLabel.TabIndex = 0;
            this.updateTestLogPathLabel.Text = "Directory";
            this.updateTestLogDirVal.AllowDrop = true;
            this.updateTestLogDirVal.Location = new Point(0x1c, 0x31);
            this.updateTestLogDirVal.Name = "updateTestLogDirVal";
            this.updateTestLogDirVal.Size = new Size(0x1b1, 20);
            this.updateTestLogDirVal.TabIndex = 1;
            this.updateTestLogDoneBut.Location = new Point(0xa5, 0x61);
            this.updateTestLogDoneBut.Name = "updateTestLogDoneBut";
            this.updateTestLogDoneBut.Size = new Size(0x4b, 0x17);
            this.updateTestLogDoneBut.TabIndex = 3;
            this.updateTestLogDoneBut.Text = "&Update";
            this.updateTestLogDoneBut.UseVisualStyleBackColor = true;
            this.updateTestLogDoneBut.Click += new EventHandler(this.updateTestLogDoneBut_Click);
            this.updateTestLogCancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.updateTestLogCancelBut.Location = new Point(0x11d, 0x61);
            this.updateTestLogCancelBut.Name = "updateTestLogCancelBut";
            this.updateTestLogCancelBut.Size = new Size(0x4b, 0x17);
            this.updateTestLogCancelBut.TabIndex = 4;
            this.updateTestLogCancelBut.Text = "&Cancel";
            this.updateTestLogCancelBut.UseVisualStyleBackColor = true;
            this.updateTestLogCancelBut.Click += new EventHandler(this.updateTestLogCancelBut_Click);
            base.AcceptButton = this.updateTestLogDoneBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.updateTestLogCancelBut;
            base.ClientSize = new Size(0x211, 0x94);
            base.Controls.Add(this.updateTestLogCancelBut);
            base.Controls.Add(this.updateTestLogDoneBut);
            base.Controls.Add(this.updateTestLogDirBrowser);
            base.Controls.Add(this.updateTestLogPathLabel);
            base.Controls.Add(this.updateTestLogDirVal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmUpdateTestLogDir";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Update Directory";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void updateTestLogCancelBut_Click(object sender, EventArgs e)
        {
            base.Close();
            m_SChildform = null;
        }

        private void updateTestLogDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ConfigurationManager.AppSettings["InstalledDirectory"];
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.updateTestLogDirVal.Text = dialog.SelectedPath;
            }
        }

        private void updateTestLogDoneBut_Click(object sender, EventArgs e)
        {
            if ((this.updateParent != null) && (this.updateTestLogDirVal.Text != string.Empty))
            {
                this.updateParent(this.updateTestLogDirVal.Text);
            }
            base.Close();
            m_SChildform = null;
        }

        internal delegate void updateParentEventHandler(string updatedData);
    }
}

