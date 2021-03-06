﻿namespace CommonClassLibrary
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmDialogWith3Button : Form
    {
        private string _displayMessage = string.Empty;
        private Button button1;
        private Button button2;
        private Button button3;
        private IContainer components;
        private RichTextBox outputMessage;

        public frmDialogWith3Button(string title, string bnt1Text, string bnt2Text, string bnt3Text)
        {
            this.InitializeComponent();
            this.Text = title;
            this.button1.Text = bnt1Text;
            this.button2.Text = bnt2Text;
            this.button3.Text = bnt3Text;
            this.outputMessage.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Yes;
            base.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
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

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmDialogWith3Button));
            this.outputMessage = new RichTextBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            base.SuspendLayout();
            this.outputMessage.BackColor = SystemColors.Control;
            this.outputMessage.BorderStyle = BorderStyle.None;
            this.outputMessage.Dock = DockStyle.Fill;
            this.outputMessage.Location = new Point(0, 0);
            this.outputMessage.Name = "outputMessage";
            this.outputMessage.ReadOnly = true;
            this.outputMessage.Size = new Size(0x1e4, 0x9e);
            this.outputMessage.TabIndex = 0;
            this.outputMessage.Text = "";
            this.button1.Anchor = AnchorStyles.Bottom;
            this.button1.Location = new Point(0x52, 0x72);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Anchor = AnchorStyles.Bottom;
            this.button2.Location = new Point(0xcd, 0x71);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.Anchor = AnchorStyles.Bottom;
            this.button3.Location = new Point(0x148, 0x71);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1e4, 0x9e);
            base.ControlBox = false;
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.outputMessage);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmDialogWith3Button";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Message Box";
            base.ResumeLayout(false);
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
                this.outputMessage.Text = "\n\n" + value;
            }
        }
    }
}

