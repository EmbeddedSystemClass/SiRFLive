﻿namespace SiRFLive.GUI.Automation
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmUpdatePlaybackTime : Form
    {
        private int _elmCnt;
        private Button clearBtn;
        private IContainer components;
        private Button frmUpdatePbTimeAddBut;
        private Button frmUpdatePbTimeCancelBut;
        private Label frmUpdatePbTimeCntLabel;
        private Button frmUpdatePbTimeDoneBut;
        private TextBox frmUpdatePbTimeHourVal;
        private Label frmUpdatePbTimeLabel;
        private TextBox frmUpdatePbTimeMinuteVal;
        private Label frmUpdatePbTimeOutStrLabel;
        private TextBox frmUpdatePbTimeOutStrVal;
        private Label frmUpdatePbTimeRepLabel;
        private TextBox frmUpdatePbTimeRepVal;
        private TextBox frmUpdatePbTimeSecVal;
        private CheckBox frnUpdatePbTimeSetAllChk;
        private Label label1;
        private static frmUpdatePlaybackTime m_SChildform;
        private Label pbTimeDivider1;
        private Label pbTimeDivider2;

        internal event updateParentEventHandler updateParent;

        public frmUpdatePlaybackTime()
        {
            this.InitializeComponent();
            this.frmUpdatePbTimeHourVal.Text = "0";
            this.frmUpdatePbTimeMinuteVal.Text = "0";
            this.frmUpdatePbTimeSecVal.Text = "0";
            this.frmUpdatePbTimeRepVal.Text = "0";
            this.frmUpdatePbTimeOutStrVal.Text = string.Empty;
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            this._elmCnt = 0;
            this.frmUpdatePbTimeOutStrVal.Text = string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmUpdatePbTimeAddBut_Click(object sender, EventArgs e)
        {
            if (((this.frmUpdatePbTimeHourVal.Text.Length == 0) || (this.frmUpdatePbTimeMinuteVal.Text.Length == 0)) || (this.frmUpdatePbTimeSecVal.Text.Length == 0))
            {
                MessageBox.Show("Empty input time", "Error");
            }
            else
            {
                int num = Convert.ToInt16(this.frmUpdatePbTimeHourVal.Text);
                int num2 = Convert.ToInt16(this.frmUpdatePbTimeMinuteVal.Text);
                int num3 = Convert.ToInt16(this.frmUpdatePbTimeSecVal.Text);
                if (((num < 0) || (num2 < 0)) || (((num3 < 0) || (num2 >= 60)) || (num3 >= 60)))
                {
                    MessageBox.Show("Invalid minute or second value", "Error");
                }
                else
                {
                    if (this.frnUpdatePbTimeSetAllChk.Checked)
                    {
                        if (this.frmUpdatePbTimeRepVal.Text.Length == 0)
                        {
                            MessageBox.Show("Repeat is empty", "Error");
                            return;
                        }
                        int num4 = Convert.ToInt16(this.frmUpdatePbTimeRepVal.Text);
                        if (this.frmUpdatePbTimeOutStrVal.Text.Length == 0)
                        {
                            this.frmUpdatePbTimeOutStrVal.Text = this.frmUpdatePbTimeOutStrVal.Text + string.Format("{0}:{1}:{2}", num, num2, num3);
                            this._elmCnt++;
                            num4--;
                        }
                        for (int i = 0; i < num4; i++)
                        {
                            this.frmUpdatePbTimeOutStrVal.Text = this.frmUpdatePbTimeOutStrVal.Text + string.Format(",{0}:{1}:{2}", num, num2, num3);
                            this._elmCnt++;
                        }
                    }
                    else
                    {
                        if (this.frmUpdatePbTimeOutStrVal.Text.Length == 0)
                        {
                            this.frmUpdatePbTimeOutStrVal.Text = this.frmUpdatePbTimeOutStrVal.Text + string.Format("{0}:{1}:{2}", num, num2, num3);
                        }
                        else
                        {
                            this.frmUpdatePbTimeOutStrVal.Text = this.frmUpdatePbTimeOutStrVal.Text + string.Format(",{0}:{1}:{2}", num, num2, num3);
                        }
                        this._elmCnt++;
                    }
                    this.frmUpdatePbTimeCntLabel.Text = string.Format("Total: {0}", this._elmCnt);
                }
            }
        }

        private void frmUpdatePbTimeCancelBut_Click(object sender, EventArgs e)
        {
            base.Close();
            m_SChildform = null;
        }

        private void frmUpdatePbTimeDoneBut_Click(object sender, EventArgs e)
        {
            if ((this.updateParent != null) && (this.frmUpdatePbTimeOutStrVal.Text.Length != 0))
            {
                this.updateParent(this.frmUpdatePbTimeOutStrVal.Text);
                base.Close();
            }
            else
            {
                MessageBox.Show("Output is empty -- no update!", "Error");
            }
        }

        public static frmUpdatePlaybackTime GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmUpdatePlaybackTime();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmUpdatePlaybackTime));
            this.pbTimeDivider2 = new Label();
            this.frmUpdatePbTimeSecVal = new TextBox();
            this.pbTimeDivider1 = new Label();
            this.frmUpdatePbTimeMinuteVal = new TextBox();
            this.frmUpdatePbTimeHourVal = new TextBox();
            this.frnUpdatePbTimeSetAllChk = new CheckBox();
            this.frmUpdatePbTimeLabel = new Label();
            this.frmUpdatePbTimeAddBut = new Button();
            this.frmUpdatePbTimeCancelBut = new Button();
            this.frmUpdatePbTimeRepVal = new TextBox();
            this.frmUpdatePbTimeRepLabel = new Label();
            this.frmUpdatePbTimeOutStrVal = new TextBox();
            this.frmUpdatePbTimeOutStrLabel = new Label();
            this.frmUpdatePbTimeCntLabel = new Label();
            this.frmUpdatePbTimeDoneBut = new Button();
            this.label1 = new Label();
            this.clearBtn = new Button();
            base.SuspendLayout();
            this.pbTimeDivider2.AutoSize = true;
            this.pbTimeDivider2.Location = new Point(0xb1, 0x23);
            this.pbTimeDivider2.Name = "pbTimeDivider2";
            this.pbTimeDivider2.Size = new Size(10, 13);
            this.pbTimeDivider2.TabIndex = 4;
            this.pbTimeDivider2.Text = ":";
            this.frmUpdatePbTimeSecVal.Location = new Point(0xbd, 0x1f);
            this.frmUpdatePbTimeSecVal.Name = "frmUpdatePbTimeSecVal";
            this.frmUpdatePbTimeSecVal.Size = new Size(0x1b, 20);
            this.frmUpdatePbTimeSecVal.TabIndex = 2;
            this.pbTimeDivider1.AutoSize = true;
            this.pbTimeDivider1.Location = new Point(0x88, 0x23);
            this.pbTimeDivider1.Name = "pbTimeDivider1";
            this.pbTimeDivider1.Size = new Size(10, 13);
            this.pbTimeDivider1.TabIndex = 2;
            this.pbTimeDivider1.Text = ":";
            this.frmUpdatePbTimeMinuteVal.Location = new Point(0x94, 0x1f);
            this.frmUpdatePbTimeMinuteVal.Name = "frmUpdatePbTimeMinuteVal";
            this.frmUpdatePbTimeMinuteVal.Size = new Size(0x1b, 20);
            this.frmUpdatePbTimeMinuteVal.TabIndex = 1;
            this.frmUpdatePbTimeHourVal.Location = new Point(0x6b, 0x1f);
            this.frmUpdatePbTimeHourVal.Name = "frmUpdatePbTimeHourVal";
            this.frmUpdatePbTimeHourVal.Size = new Size(0x1b, 20);
            this.frmUpdatePbTimeHourVal.TabIndex = 0;
            this.frnUpdatePbTimeSetAllChk.AutoSize = true;
            this.frnUpdatePbTimeSetAllChk.Location = new Point(0xa2, 0x45);
            this.frnUpdatePbTimeSetAllChk.Name = "frnUpdatePbTimeSetAllChk";
            this.frnUpdatePbTimeSetAllChk.Size = new Size(0x3e, 0x11);
            this.frnUpdatePbTimeSetAllChk.TabIndex = 3;
            this.frnUpdatePbTimeSetAllChk.Text = "Set All?";
            this.frnUpdatePbTimeSetAllChk.UseVisualStyleBackColor = true;
            this.frmUpdatePbTimeLabel.AutoSize = true;
            this.frmUpdatePbTimeLabel.Location = new Point(0x16, 0x1f);
            this.frmUpdatePbTimeLabel.Name = "frmUpdatePbTimeLabel";
            this.frmUpdatePbTimeLabel.Size = new Size(0x30, 13);
            this.frmUpdatePbTimeLabel.TabIndex = 0;
            this.frmUpdatePbTimeLabel.Text = "Play For:";
            this.frmUpdatePbTimeAddBut.Location = new Point(230, 0x3e);
            this.frmUpdatePbTimeAddBut.Name = "frmUpdatePbTimeAddBut";
            this.frmUpdatePbTimeAddBut.Size = new Size(0x4b, 0x17);
            this.frmUpdatePbTimeAddBut.TabIndex = 7;
            this.frmUpdatePbTimeAddBut.Text = "&Add";
            this.frmUpdatePbTimeAddBut.UseVisualStyleBackColor = true;
            this.frmUpdatePbTimeAddBut.Click += new EventHandler(this.frmUpdatePbTimeAddBut_Click);
            this.frmUpdatePbTimeCancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmUpdatePbTimeCancelBut.Location = new Point(230, 0xac);
            this.frmUpdatePbTimeCancelBut.Name = "frmUpdatePbTimeCancelBut";
            this.frmUpdatePbTimeCancelBut.Size = new Size(0x4b, 0x17);
            this.frmUpdatePbTimeCancelBut.TabIndex = 9;
            this.frmUpdatePbTimeCancelBut.Text = "&Cancel";
            this.frmUpdatePbTimeCancelBut.UseVisualStyleBackColor = true;
            this.frmUpdatePbTimeCancelBut.Click += new EventHandler(this.frmUpdatePbTimeCancelBut_Click);
            this.frmUpdatePbTimeRepVal.Location = new Point(0x6b, 0x41);
            this.frmUpdatePbTimeRepVal.Name = "frmUpdatePbTimeRepVal";
            this.frmUpdatePbTimeRepVal.Size = new Size(0x31, 20);
            this.frmUpdatePbTimeRepVal.TabIndex = 4;
            this.frmUpdatePbTimeRepLabel.AutoSize = true;
            this.frmUpdatePbTimeRepLabel.Location = new Point(0x16, 0x45);
            this.frmUpdatePbTimeRepLabel.Name = "frmUpdatePbTimeRepLabel";
            this.frmUpdatePbTimeRepLabel.Size = new Size(0x2d, 13);
            this.frmUpdatePbTimeRepLabel.TabIndex = 7;
            this.frmUpdatePbTimeRepLabel.Text = "Repeat:";
            this.frmUpdatePbTimeOutStrVal.Location = new Point(0x6b, 0x81);
            this.frmUpdatePbTimeOutStrVal.Name = "frmUpdatePbTimeOutStrVal";
            this.frmUpdatePbTimeOutStrVal.ReadOnly = true;
            this.frmUpdatePbTimeOutStrVal.Size = new Size(0x13f, 20);
            this.frmUpdatePbTimeOutStrVal.TabIndex = 6;
            this.frmUpdatePbTimeOutStrLabel.AutoSize = true;
            this.frmUpdatePbTimeOutStrLabel.Location = new Point(0x16, 0x85);
            this.frmUpdatePbTimeOutStrLabel.Name = "frmUpdatePbTimeOutStrLabel";
            this.frmUpdatePbTimeOutStrLabel.Size = new Size(0x2a, 13);
            this.frmUpdatePbTimeOutStrLabel.TabIndex = 11;
            this.frmUpdatePbTimeOutStrLabel.Text = "Output ";
            this.frmUpdatePbTimeCntLabel.AutoSize = true;
            this.frmUpdatePbTimeCntLabel.Location = new Point(0x16, 0x6f);
            this.frmUpdatePbTimeCntLabel.Name = "frmUpdatePbTimeCntLabel";
            this.frmUpdatePbTimeCntLabel.Size = new Size(0x2b, 13);
            this.frmUpdatePbTimeCntLabel.TabIndex = 9;
            this.frmUpdatePbTimeCntLabel.Text = "Total: 0";
            this.frmUpdatePbTimeDoneBut.Location = new Point(0x8b, 0xac);
            this.frmUpdatePbTimeDoneBut.Name = "frmUpdatePbTimeDoneBut";
            this.frmUpdatePbTimeDoneBut.Size = new Size(0x4b, 0x17);
            this.frmUpdatePbTimeDoneBut.TabIndex = 8;
            this.frmUpdatePbTimeDoneBut.Text = "&Update";
            this.frmUpdatePbTimeDoneBut.UseVisualStyleBackColor = true;
            this.frmUpdatePbTimeDoneBut.Click += new EventHandler(this.frmUpdatePbTimeDoneBut_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x15, 0x2c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x39, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "(hh:mm:ss)";
            this.clearBtn.Location = new Point(0x148, 0x3e);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new Size(0x4b, 0x17);
            this.clearBtn.TabIndex = 13;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new EventHandler(this.clearBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmUpdatePbTimeCancelBut;
            base.ClientSize = new Size(0x1bc, 0xd8);
            base.Controls.Add(this.clearBtn);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.frmUpdatePbTimeDoneBut);
            base.Controls.Add(this.frmUpdatePbTimeCntLabel);
            base.Controls.Add(this.frmUpdatePbTimeOutStrLabel);
            base.Controls.Add(this.frmUpdatePbTimeOutStrVal);
            base.Controls.Add(this.frmUpdatePbTimeRepLabel);
            base.Controls.Add(this.frmUpdatePbTimeRepVal);
            base.Controls.Add(this.frmUpdatePbTimeCancelBut);
            base.Controls.Add(this.frmUpdatePbTimeAddBut);
            base.Controls.Add(this.frmUpdatePbTimeLabel);
            base.Controls.Add(this.pbTimeDivider2);
            base.Controls.Add(this.frmUpdatePbTimeSecVal);
            base.Controls.Add(this.pbTimeDivider1);
            base.Controls.Add(this.frmUpdatePbTimeMinuteVal);
            base.Controls.Add(this.frmUpdatePbTimeHourVal);
            base.Controls.Add(this.frnUpdatePbTimeSetAllChk);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmUpdatePlaybackTime";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Set Playback Time";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        internal delegate void updateParentEventHandler(string updatedData);
    }
}

