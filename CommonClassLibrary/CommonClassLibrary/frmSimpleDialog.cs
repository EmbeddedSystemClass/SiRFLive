﻿namespace CommonClassLibrary
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSimpleDialog : Form
    {
        private string _displayMessage = string.Empty;
        private bool _isPendingCancel;
        private IContainer components;
        private RichTextBox outputMessage;
        private Button simpleDialogCancelBtn;

        public frmSimpleDialog(string title)
        {
            this.InitializeComponent();
            this.Text = title;
            this.outputMessage.SelectionAlignment = HorizontalAlignment.Center;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmSimpleDialog));
            this.outputMessage = new RichTextBox();
            this.simpleDialogCancelBtn = new Button();
            base.SuspendLayout();
            this.outputMessage.AcceptsTab = true;
            this.outputMessage.Dock = DockStyle.Fill;
            this.outputMessage.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.outputMessage.Location = new Point(0, 0);
            this.outputMessage.Name = "outputMessage";
            this.outputMessage.ReadOnly = true;
            this.outputMessage.Size = new Size(0x17e, 0x86);
            this.outputMessage.TabIndex = 0;
            this.outputMessage.Text = "";
            this.outputMessage.UseWaitCursor = true;
            this.simpleDialogCancelBtn.Anchor = AnchorStyles.Bottom;
            this.simpleDialogCancelBtn.Location = new Point(0x9a, 0x6a);
            this.simpleDialogCancelBtn.Name = "simpleDialogCancelBtn";
            this.simpleDialogCancelBtn.Size = new Size(0x4b, 0x17);
            this.simpleDialogCancelBtn.TabIndex = 1;
            this.simpleDialogCancelBtn.Text = "Cancel";
            this.simpleDialogCancelBtn.UseVisualStyleBackColor = true;
            this.simpleDialogCancelBtn.Visible = false;
            this.simpleDialogCancelBtn.Click += new EventHandler(this.simpleDialogCancelBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x17e, 0x86);
            base.ControlBox = false;
            base.Controls.Add(this.simpleDialogCancelBtn);
            base.Controls.Add(this.outputMessage);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmSimpleDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Simple Dialog";
            base.ResumeLayout(false);
        }

        public void ShowMessage()
        {
            base.BringToFront();
            this.Refresh();
        }

        private void simpleDialogCancelBtn_Click(object sender, EventArgs e)
        {
            this._isPendingCancel = true;
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        public string DisplayMessage
        {
            get
            {
                return this._displayMessage;
            }
            set
            {
                this._displayMessage = value;
                this.outputMessage.Text = string.Format("\n{0}", value);
            }
        }

        public bool IsPendingCancel
        {
            get
            {
                return this._isPendingCancel;
            }
            set
            {
                this._isPendingCancel = value;
            }
        }
    }
}

