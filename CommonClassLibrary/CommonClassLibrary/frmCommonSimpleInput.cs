﻿namespace CommonClassLibrary
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class frmCommonSimpleInput : Form
    {
        private string _callingWindowStr = string.Empty;
        private string _initialString = string.Empty;
        private string _updateType = string.Empty;
        private IContainer components;
        private TextBox frmCommonSimpleInputTxtBox;
        private Button frmSimpleInputCancelBtn;
        private Label frmSimpleInputLabel;
        private Button frmSimpleInputOkBtn;

        public event updateParentEventHandler updateParent;

        public frmCommonSimpleInput(string label)
        {
            this.InitializeComponent();
            this.frmSimpleInputLabel.Text = label;
            this._callingWindowStr = label;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommonSimpleInput_Load(object sender, EventArgs e)
        {
            this.frmCommonSimpleInputTxtBox.Text = this._initialString;
        }

        private void frmCommonSimpleInputCancelBtn_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void frmCommonSimpleInputOkBtn_Click(object sender, EventArgs e)
        {
            string text = this.frmCommonSimpleInputTxtBox.Text;
            if (this._callingWindowStr == "Enter Location Name:")
            {
                Regex regex = new Regex("[^0-9a-zA-Z_ ]+");
                if (regex.IsMatch(text))
                {
                    MessageBox.Show("Reference Location name contains invaid characters");
                }
                else
                {
                    if (this.updateParent != null)
                    {
                        this.updateParent(this.frmCommonSimpleInputTxtBox.Text);
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            else
            {
                if (this.updateParent != null)
                {
                    this.updateParent(this.frmCommonSimpleInputTxtBox.Text);
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCommonSimpleInput));
            this.frmSimpleInputCancelBtn = new Button();
            this.frmSimpleInputOkBtn = new Button();
            this.frmSimpleInputLabel = new Label();
            this.frmCommonSimpleInputTxtBox = new TextBox();
            base.SuspendLayout();
            this.frmSimpleInputCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmSimpleInputCancelBtn.Location = new Point(0x115, 0x58);
            this.frmSimpleInputCancelBtn.Name = "frmSimpleInputCancelBtn";
            this.frmSimpleInputCancelBtn.Size = new Size(0x48, 0x1a);
            this.frmSimpleInputCancelBtn.TabIndex = 7;
            this.frmSimpleInputCancelBtn.Text = "&Cancel";
            this.frmSimpleInputCancelBtn.UseVisualStyleBackColor = true;
            this.frmSimpleInputCancelBtn.Click += new EventHandler(this.frmCommonSimpleInputCancelBtn_Click);
            this.frmSimpleInputOkBtn.Location = new Point(0xc7, 0x58);
            this.frmSimpleInputOkBtn.Name = "frmSimpleInputOkBtn";
            this.frmSimpleInputOkBtn.Size = new Size(0x48, 0x1a);
            this.frmSimpleInputOkBtn.TabIndex = 6;
            this.frmSimpleInputOkBtn.Text = "&OK";
            this.frmSimpleInputOkBtn.UseVisualStyleBackColor = true;
            this.frmSimpleInputOkBtn.Click += new EventHandler(this.frmCommonSimpleInputOkBtn_Click);
            this.frmSimpleInputLabel.AutoSize = true;
            this.frmSimpleInputLabel.Location = new Point(0x26, 20);
            this.frmSimpleInputLabel.Name = "frmSimpleInputLabel";
            this.frmSimpleInputLabel.Size = new Size(0x3a, 13);
            this.frmSimpleInputLabel.TabIndex = 4;
            this.frmSimpleInputLabel.Text = "Enter Data";
            this.frmCommonSimpleInputTxtBox.Location = new Point(0x26, 0x2b);
            this.frmCommonSimpleInputTxtBox.Name = "frmCommonSimpleInputTxtBox";
            this.frmCommonSimpleInputTxtBox.Size = new Size(0x1e3, 20);
            this.frmCommonSimpleInputTxtBox.TabIndex = 5;
            base.AcceptButton = this.frmSimpleInputOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmSimpleInputCancelBtn;
            base.ClientSize = new Size(0x224, 0x88);
            base.Controls.Add(this.frmSimpleInputCancelBtn);
            base.Controls.Add(this.frmSimpleInputOkBtn);
            base.Controls.Add(this.frmSimpleInputLabel);
            base.Controls.Add(this.frmCommonSimpleInputTxtBox);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmCommonSimpleInput";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Simple Input";
            base.Load += new EventHandler(this.frmCommonSimpleInput_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string InitialData
        {
            get
            {
                return this._initialString;
            }
            set
            {
                this._initialString = value;
            }
        }

        public string UpdateType
        {
            get
            {
                return this._updateType;
            }
            set
            {
                this._updateType = value;
            }
        }

        public delegate void updateParentEventHandler(string updatedData);
    }
}

