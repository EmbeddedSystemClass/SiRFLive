﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmMaskDOP : Form
    {
        private RadioButton AutoDOP_rBtn;
        private Button button_Cancel;
        private Button button_Send;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton GDOPonly_rBtn;
        private RadioButton HDOPonly_rBtn;
        private Label labelGDOP;
        private Label labelHDOP;
        private Label labelPDOP;
        private RadioButton NoDOP_rBtn;
        private RadioButton PDOPonly_rBtn;
        private TextBox valueGDOP_txtBx;
        private TextBox valueHDOP_txtBx;
        private TextBox valuePDOP_txtBx;

        public frmMaskDOP()
        {
            this.InitializeComponent();
            this.NoDOP_rBtn.Checked = true;
        }

        public frmMaskDOP(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
            this.NoDOP_rBtn.Checked = true;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            int selectDOP = 4;
            string valueDOP = "8";
            string fieldNameDOP = "";
            if (this.AutoDOP_rBtn.Checked)
            {
                selectDOP = 0;
            }
            if (this.PDOPonly_rBtn.Checked)
            {
                selectDOP = 1;
                try
                {
                    Convert.ToInt64(this.valuePDOP_txtBx.Text);
                }
                catch
                {
                    MessageBox.Show("Incorrect value entered. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if ((Convert.ToInt64(this.valuePDOP_txtBx.Text) < 1L) || (Convert.ToInt64(this.valuePDOP_txtBx.Text) > 50L))
                {
                    MessageBox.Show("Range is 1 to 50", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                valueDOP = this.valuePDOP_txtBx.Text;
                fieldNameDOP = "PDOP Value";
            }
            if (this.HDOPonly_rBtn.Checked)
            {
                selectDOP = 2;
                try
                {
                    Convert.ToInt64(this.valueHDOP_txtBx.Text);
                }
                catch
                {
                    MessageBox.Show("Incorrect value entered. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if ((Convert.ToInt64(this.valueHDOP_txtBx.Text) < 1L) || (Convert.ToInt64(this.valueHDOP_txtBx.Text) > 50L))
                {
                    MessageBox.Show("Range is 1 to 50", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                valueDOP = this.valueHDOP_txtBx.Text;
                fieldNameDOP = "HDOP Value";
            }
            if (this.GDOPonly_rBtn.Checked)
            {
                selectDOP = 3;
                try
                {
                    Convert.ToInt64(this.valueGDOP_txtBx.Text);
                }
                catch
                {
                    MessageBox.Show("Incorrect value entered. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if ((Convert.ToInt64(this.valueGDOP_txtBx.Text) < 1L) || (Convert.ToInt64(this.valueGDOP_txtBx.Text) > 50L))
                {
                    MessageBox.Show("Range is 1 to 50", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                valueDOP = this.valueGDOP_txtBx.Text;
                fieldNameDOP = "GDOP Value";
            }
            if (this.NoDOP_rBtn.Checked)
            {
                selectDOP = 4;
            }
            this.SetDOPMaskControl(selectDOP, fieldNameDOP, valueDOP);
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

        private void GDOPonly_rBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.GDOPonly_rBtn.Checked)
            {
                this.valueGDOP_txtBx.Enabled = true;
                this.labelGDOP.Enabled = true;
            }
            else
            {
                this.valueGDOP_txtBx.Enabled = false;
                this.labelGDOP.Enabled = false;
            }
        }

        private void HDOPonly_rBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HDOPonly_rBtn.Checked)
            {
                this.valueHDOP_txtBx.Enabled = true;
                this.labelHDOP.Enabled = true;
            }
            else
            {
                this.valueHDOP_txtBx.Enabled = false;
                this.labelHDOP.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmMaskDOP));
            this.button_Send = new Button();
            this.button_Cancel = new Button();
            this.AutoDOP_rBtn = new RadioButton();
            this.HDOPonly_rBtn = new RadioButton();
            this.GDOPonly_rBtn = new RadioButton();
            this.NoDOP_rBtn = new RadioButton();
            this.labelPDOP = new Label();
            this.labelHDOP = new Label();
            this.labelGDOP = new Label();
            this.valuePDOP_txtBx = new TextBox();
            this.valueHDOP_txtBx = new TextBox();
            this.valueGDOP_txtBx = new TextBox();
            this.PDOPonly_rBtn = new RadioButton();
            base.SuspendLayout();
            this.button_Send.Location = new Point(250, 13);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new Size(0x4b, 0x17);
            this.button_Send.TabIndex = 0;
            this.button_Send.Text = "&Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new EventHandler(this.button_Send_Click);
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(250, 0x2b);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            this.AutoDOP_rBtn.AutoSize = true;
            this.AutoDOP_rBtn.Location = new Point(0x17, 13);
            this.AutoDOP_rBtn.Name = "AutoDOP_rBtn";
            this.AutoDOP_rBtn.Size = new Size(0x74, 0x11);
            this.AutoDOP_rBtn.TabIndex = 2;
            this.AutoDOP_rBtn.TabStop = true;
            this.AutoDOP_rBtn.Text = "&Auto PDOP/HDOP";
            this.AutoDOP_rBtn.UseVisualStyleBackColor = true;
            this.HDOPonly_rBtn.AutoSize = true;
            this.HDOPonly_rBtn.Location = new Point(0x17, 0x3d);
            this.HDOPonly_rBtn.Name = "HDOPonly_rBtn";
            this.HDOPonly_rBtn.Size = new Size(100, 0x11);
            this.HDOPonly_rBtn.TabIndex = 4;
            this.HDOPonly_rBtn.TabStop = true;
            this.HDOPonly_rBtn.Text = "Use &HDOP only";
            this.HDOPonly_rBtn.UseVisualStyleBackColor = true;
            this.HDOPonly_rBtn.CheckedChanged += new EventHandler(this.HDOPonly_rBtn_CheckedChanged);
            this.GDOPonly_rBtn.AutoSize = true;
            this.GDOPonly_rBtn.Location = new Point(0x17, 0x55);
            this.GDOPonly_rBtn.Name = "GDOPonly_rBtn";
            this.GDOPonly_rBtn.Size = new Size(100, 0x11);
            this.GDOPonly_rBtn.TabIndex = 5;
            this.GDOPonly_rBtn.TabStop = true;
            this.GDOPonly_rBtn.Text = "Use &GDOP only";
            this.GDOPonly_rBtn.UseVisualStyleBackColor = true;
            this.GDOPonly_rBtn.CheckedChanged += new EventHandler(this.GDOPonly_rBtn_CheckedChanged);
            this.NoDOP_rBtn.AutoSize = true;
            this.NoDOP_rBtn.Location = new Point(0x17, 0x6d);
            this.NoDOP_rBtn.Name = "NoDOP_rBtn";
            this.NoDOP_rBtn.Size = new Size(0x4d, 0x11);
            this.NoDOP_rBtn.TabIndex = 6;
            this.NoDOP_rBtn.TabStop = true;
            this.NoDOP_rBtn.Text = "&Do not use";
            this.NoDOP_rBtn.UseVisualStyleBackColor = true;
            this.labelPDOP.AutoSize = true;
            this.labelPDOP.Enabled = false;
            this.labelPDOP.Location = new Point(0x86, 0x29);
            this.labelPDOP.Name = "labelPDOP";
            this.labelPDOP.Size = new Size(0x36, 13);
            this.labelPDOP.TabIndex = 7;
            this.labelPDOP.Text = "Threshold";
            this.labelHDOP.AutoSize = true;
            this.labelHDOP.Enabled = false;
            this.labelHDOP.Location = new Point(0x86, 0x3f);
            this.labelHDOP.Name = "labelHDOP";
            this.labelHDOP.Size = new Size(0x36, 13);
            this.labelHDOP.TabIndex = 8;
            this.labelHDOP.Text = "Threshold";
            this.labelGDOP.AutoSize = true;
            this.labelGDOP.Enabled = false;
            this.labelGDOP.Location = new Point(0x86, 0x55);
            this.labelGDOP.Name = "labelGDOP";
            this.labelGDOP.Size = new Size(0x36, 13);
            this.labelGDOP.TabIndex = 9;
            this.labelGDOP.Text = "Threshold";
            this.valuePDOP_txtBx.Enabled = false;
            this.valuePDOP_txtBx.Location = new Point(0xbc, 0x25);
            this.valuePDOP_txtBx.Name = "valuePDOP_txtBx";
            this.valuePDOP_txtBx.Size = new Size(0x30, 20);
            this.valuePDOP_txtBx.TabIndex = 10;
            this.valuePDOP_txtBx.Text = "10";
            this.valueHDOP_txtBx.Enabled = false;
            this.valueHDOP_txtBx.Location = new Point(0xbc, 0x3d);
            this.valueHDOP_txtBx.Name = "valueHDOP_txtBx";
            this.valueHDOP_txtBx.ScrollBars = ScrollBars.Both;
            this.valueHDOP_txtBx.Size = new Size(0x30, 20);
            this.valueHDOP_txtBx.TabIndex = 11;
            this.valueHDOP_txtBx.Text = "10";
            this.valueGDOP_txtBx.Enabled = false;
            this.valueGDOP_txtBx.Location = new Point(0xbc, 0x55);
            this.valueGDOP_txtBx.Name = "valueGDOP_txtBx";
            this.valueGDOP_txtBx.Size = new Size(0x30, 20);
            this.valueGDOP_txtBx.TabIndex = 12;
            this.valueGDOP_txtBx.Text = "10";
            this.PDOPonly_rBtn.AutoSize = true;
            this.PDOPonly_rBtn.Location = new Point(0x17, 0x25);
            this.PDOPonly_rBtn.Name = "PDOPonly_rBtn";
            this.PDOPonly_rBtn.Size = new Size(0x63, 0x11);
            this.PDOPonly_rBtn.TabIndex = 3;
            this.PDOPonly_rBtn.TabStop = true;
            this.PDOPonly_rBtn.Text = "Use &PDOP only";
            this.PDOPonly_rBtn.UseVisualStyleBackColor = true;
            this.PDOPonly_rBtn.CheckedChanged += new EventHandler(this.PDOPOnly_rBtn_CheckedChanged);
            base.AcceptButton = this.button_Send;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x151, 0x90);
            base.Controls.Add(this.valueGDOP_txtBx);
            base.Controls.Add(this.valueHDOP_txtBx);
            base.Controls.Add(this.valuePDOP_txtBx);
            base.Controls.Add(this.labelGDOP);
            base.Controls.Add(this.labelHDOP);
            base.Controls.Add(this.labelPDOP);
            base.Controls.Add(this.NoDOP_rBtn);
            base.Controls.Add(this.GDOPonly_rBtn);
            base.Controls.Add(this.HDOPonly_rBtn);
            base.Controls.Add(this.PDOPonly_rBtn);
            base.Controls.Add(this.AutoDOP_rBtn);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.button_Send);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmMaskDOP";
            this.RightToLeft = RightToLeft.No;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "DOP Mask";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PDOPOnly_rBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.PDOPonly_rBtn.Checked)
            {
                this.valuePDOP_txtBx.Enabled = true;
                this.labelPDOP.Enabled = true;
            }
            else
            {
                this.valuePDOP_txtBx.Enabled = false;
                this.labelPDOP.Enabled = false;
            }
        }

        public virtual void SetDOPMaskControl(int selectDOP, string fieldNameDOP, string valueDOP)
        {
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = this.comm.m_Protocols.GetInputMessageStructure(0x89, clsGlobal.noSID, "DOP Mask Control", "SSB");
            for (int i = 0; i < list.Count; i++)
            {
                if (((InputMsg) list[i]).fieldName == "DOP Selection")
                {
                    builder.Append(selectDOP);
                }
                else
                {
                    if (((selectDOP != 1) && (selectDOP != 2)) && (selectDOP != 3))
                    {
                        goto Label_00C2;
                    }
                    switch (selectDOP)
                    {
                        case 1:
                        case 2:
                        case 3:
                            if (!(((InputMsg) list[i]).fieldName == fieldNameDOP))
                            {
                                goto Label_00A8;
                            }
                            builder.Append(valueDOP);
                            break;
                    }
                }
                goto Label_00DA;
            Label_00A8:
                builder.Append(((InputMsg) list[i]).defaultValue);
                goto Label_00DA;
            Label_00C2:
                builder.Append(((InputMsg) list[i]).defaultValue);
            Label_00DA:
                builder.Append(",");
            }
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString().TrimEnd(new char[] { ',' }), "DOP Mask Control", "OSP");
            this.comm.WriteData(msg);
        }
    }
}

