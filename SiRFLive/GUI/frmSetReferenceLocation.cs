﻿namespace SiRFLive.GUI
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using SiRFLive.Reporting;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSetReferenceLocation : Form
    {
        private int _defaultWidth;
        private int _maxItemWidth;
        private Button button_FixPCurrentLocation;
        private CommunicationManager comm;
        private IContainer components;
        internal ObjectInterface cpGuiCtrl = new ObjectInterface();
        private GroupBox frmRxInitCmdRefLocationGrpBox;
        private Label frmSetLocationRefLatitudeLabel;
        private Button frmSetRefLocationCancelBtn;
        private Button frmSetRefLocationOkBtn;
        private Label frmSetRefLocationRefAltitudeLabel;
        private TextBox frmSetRefLocationRefAltitudeTxtBox;
        private TextBox frmSetRefLocationRefLatitudeTxtBox;
        private ComboBox frmSetRefLocationRefLocationComboBox;
        private Label frmSetRefLocationRefLongitudeLabel;
        private TextBox frmSetRefLocationRefLongitudeTxtBox;
        private Button frmSetRefLocationSetAsDefaultBtn;
        private frmCommonSimpleInput inputForm = new frmCommonSimpleInput("Enter Location Name:");
        internal string outputStr = string.Empty;

        public frmSetReferenceLocation()
        {
            this.InitializeComponent();
            this.inputForm.updateParent += new frmCommonSimpleInput.updateParentEventHandler(this.updateConfigList);
            this.inputForm.MdiParent = base.MdiParent;
            this._defaultWidth = this.frmSetRefLocationRefLocationComboBox.Width;
        }

        private void button_FixPCurrentLocation_Click(object sender, EventArgs e)
        {
            try
            {
                int count = this.comm.dataGui.Positions.PositionList.Count;
                if (this.comm.dataGui.Positions.PositionList.Count > 0)
                {
                    PositionInfo.PositionStruct struct2 = (PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[count - 1];
                    if (((byte) (struct2.NavType & 7)) == 0)
                    {
                        MessageBox.Show("Position not fixed yet -- Not set", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        this.frmSetRefLocationRefLatitudeTxtBox.Text = struct2.Latitude.ToString();
                        this.frmSetRefLocationRefLongitudeTxtBox.Text = struct2.Longitude.ToString();
                        this.frmSetRefLocationRefAltitudeTxtBox.Text = struct2.Altitude.ToString();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error updating location with current fix position", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

        private void frmSetReferenceLocation_Load(object sender, EventArgs e)
        {
            this.updateDefautlReferenceLocationComboBox();
        }

        private void frmSetRefLocationCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmSetRefLocationOkBtn_Click(object sender, EventArgs e)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            try
            {
                num = Convert.ToDouble(this.frmSetRefLocationRefLatitudeTxtBox.Text);
                num2 = Convert.ToDouble(this.frmSetRefLocationRefLongitudeTxtBox.Text);
                num3 = Convert.ToDouble(this.frmSetRefLocationRefAltitudeTxtBox.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Reference Location: " + exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str2 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str2];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        try
                        {
                            manager.comm.RxCtrl.RxNavData.RefLocationName = this.frmSetRefLocationRefLocationComboBox.Text;
                            manager.comm.RxCtrl.RxNavData.RefLat = num;
                            manager.comm.RxCtrl.RxNavData.RefLon = num2;
                            manager.comm.RxCtrl.RxNavData.RefAlt = num3;
                            manager.comm.RxCtrl.RxNavData.ValidatePosition = true;
                            manager.comm.m_NavData = this.comm.RxCtrl.RxNavData;
                            manager.comm.m_NavData.SetDefaultReferencePosition(manager.comm.RxCtrl.RxNavData.RefLocationName, manager.comm.RxCtrl.RxNavData.RefLat.ToString(), manager.comm.RxCtrl.RxNavData.RefLon.ToString(), manager.comm.RxCtrl.RxNavData.RefAlt.ToString(), false);
                            if (this.comm.dataPlot != null)
                            {
                                manager.comm.dataPlot.RefLat = num;
                                manager.comm.dataPlot.RefLon = num2;
                                manager.comm.dataPlot.RefAlt = num3;
                            }
                        }
                        catch (Exception exception2)
                        {
                            MessageBox.Show(string.Format("{0}: Error set reference location\n{1}", manager.comm.PortName, exception2.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    this.comm.RxCtrl.RxNavData.RefLocationName = this.frmSetRefLocationRefLocationComboBox.Text;
                    this.comm.RxCtrl.RxNavData.RefLat = num;
                    this.comm.RxCtrl.RxNavData.RefLon = num2;
                    this.comm.RxCtrl.RxNavData.RefAlt = num3;
                    this.comm.RxCtrl.RxNavData.ValidatePosition = true;
                    this.comm.m_NavData = this.comm.RxCtrl.RxNavData;
                    this.comm.m_NavData.SetDefaultReferencePosition(this.comm.RxCtrl.RxNavData.RefLocationName, this.comm.RxCtrl.RxNavData.RefLat.ToString(), this.comm.RxCtrl.RxNavData.RefLon.ToString(), this.comm.RxCtrl.RxNavData.RefAlt.ToString(), false);
                    if (this.comm.dataPlot != null)
                    {
                        this.comm.dataPlot.RefLat = num;
                        this.comm.dataPlot.RefLon = num2;
                        this.comm.dataPlot.RefAlt = num3;
                    }
                }
                catch (Exception exception3)
                {
                    MessageBox.Show(string.Format("{0}: Error set reference location\n{1}", this.comm.PortName, exception3.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            base.Close();
        }

        private void frmSetRefLocationRefLocationComboBox_DropDown(object sender, EventArgs e)
        {
            this.frmSetRefLocationRefLocationComboBox.Width = this._maxItemWidth;
        }

        private void frmSetRefLocationRefLocationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.frmSetRefLocationRefLocationComboBox.SelectedItem.ToString() != "USER_DEFINED")
                {
                    this.comm.RxCtrl.RxNavData.RefLocationName = this.frmSetRefLocationRefLocationComboBox.SelectedItem.ToString();
                    this.updateReferenceLocationComboBox();
                }
                else
                {
                    this.inputForm.UpdateType = "UPDATE_REF_NAME";
                    this.inputForm.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Error Updating Reference Location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSetReferenceLocation));
            this.frmRxInitCmdRefLocationGrpBox = new GroupBox();
            this.button_FixPCurrentLocation = new Button();
            this.frmSetRefLocationSetAsDefaultBtn = new Button();
            this.frmSetRefLocationRefLatitudeTxtBox = new TextBox();
            this.frmSetLocationRefLatitudeLabel = new Label();
            this.frmSetRefLocationRefLongitudeTxtBox = new TextBox();
            this.frmSetRefLocationRefLongitudeLabel = new Label();
            this.frmSetRefLocationRefAltitudeTxtBox = new TextBox();
            this.frmSetRefLocationRefAltitudeLabel = new Label();
            this.frmSetRefLocationRefLocationComboBox = new ComboBox();
            this.frmSetRefLocationOkBtn = new Button();
            this.frmSetRefLocationCancelBtn = new Button();
            this.frmRxInitCmdRefLocationGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.button_FixPCurrentLocation);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationSetAsDefaultBtn);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationRefLatitudeTxtBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetLocationRefLatitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationRefLongitudeTxtBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationRefLongitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationRefAltitudeTxtBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationRefAltitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmSetRefLocationRefLocationComboBox);
            this.frmRxInitCmdRefLocationGrpBox.Location = new Point(20, 0x13);
            this.frmRxInitCmdRefLocationGrpBox.Name = "frmRxInitCmdRefLocationGrpBox";
            this.frmRxInitCmdRefLocationGrpBox.Size = new Size(0x175, 0x6f);
            this.frmRxInitCmdRefLocationGrpBox.TabIndex = 1;
            this.frmRxInitCmdRefLocationGrpBox.TabStop = false;
            this.frmRxInitCmdRefLocationGrpBox.Text = "Reference Location";
            this.button_FixPCurrentLocation.Location = new Point(0x137, 20);
            this.button_FixPCurrentLocation.Name = "button_FixPCurrentLocation";
            this.button_FixPCurrentLocation.Size = new Size(0x37, 0x17);
            this.button_FixPCurrentLocation.TabIndex = 3;
            this.button_FixPCurrentLocation.Text = "Fix Pos";
            this.button_FixPCurrentLocation.UseVisualStyleBackColor = true;
            this.button_FixPCurrentLocation.Click += new EventHandler(this.button_FixPCurrentLocation_Click);
            this.frmSetRefLocationSetAsDefaultBtn.Location = new Point(0x1b, 0x4c);
            this.frmSetRefLocationSetAsDefaultBtn.Name = "frmSetRefLocationSetAsDefaultBtn";
            this.frmSetRefLocationSetAsDefaultBtn.Size = new Size(0x95, 0x17);
            this.frmSetRefLocationSetAsDefaultBtn.TabIndex = 2;
            this.frmSetRefLocationSetAsDefaultBtn.Text = "Set as Default";
            this.frmSetRefLocationSetAsDefaultBtn.UseVisualStyleBackColor = true;
            this.frmSetRefLocationRefLatitudeTxtBox.Location = new Point(0xef, 0x15);
            this.frmSetRefLocationRefLatitudeTxtBox.Name = "frmSetRefLocationRefLatitudeTxtBox";
            this.frmSetRefLocationRefLatitudeTxtBox.Size = new Size(0x44, 20);
            this.frmSetRefLocationRefLatitudeTxtBox.TabIndex = 3;
            this.frmSetRefLocationRefLatitudeTxtBox.Text = "-2682834";
            this.frmSetLocationRefLatitudeLabel.AutoSize = true;
            this.frmSetLocationRefLatitudeLabel.Location = new Point(0xc0, 0x19);
            this.frmSetLocationRefLatitudeLabel.Name = "frmSetLocationRefLatitudeLabel";
            this.frmSetLocationRefLatitudeLabel.Size = new Size(0x2d, 13);
            this.frmSetLocationRefLatitudeLabel.TabIndex = 11;
            this.frmSetLocationRefLatitudeLabel.Text = "Latitude";
            this.frmSetRefLocationRefLongitudeTxtBox.Location = new Point(0xef, 0x31);
            this.frmSetRefLocationRefLongitudeTxtBox.Name = "frmSetRefLocationRefLongitudeTxtBox";
            this.frmSetRefLocationRefLongitudeTxtBox.Size = new Size(0x44, 20);
            this.frmSetRefLocationRefLongitudeTxtBox.TabIndex = 4;
            this.frmSetRefLocationRefLongitudeTxtBox.Text = "-4307681";
            this.frmSetRefLocationRefLongitudeLabel.AutoSize = true;
            this.frmSetRefLocationRefLongitudeLabel.Location = new Point(0xb7, 0x35);
            this.frmSetRefLocationRefLongitudeLabel.Name = "frmSetRefLocationRefLongitudeLabel";
            this.frmSetRefLocationRefLongitudeLabel.Size = new Size(0x36, 13);
            this.frmSetRefLocationRefLongitudeLabel.TabIndex = 13;
            this.frmSetRefLocationRefLongitudeLabel.Text = "Longitude";
            this.frmSetRefLocationRefAltitudeTxtBox.Location = new Point(0xef, 0x4d);
            this.frmSetRefLocationRefAltitudeTxtBox.Name = "frmSetRefLocationRefAltitudeTxtBox";
            this.frmSetRefLocationRefAltitudeTxtBox.Size = new Size(0x44, 20);
            this.frmSetRefLocationRefAltitudeTxtBox.TabIndex = 5;
            this.frmSetRefLocationRefAltitudeTxtBox.Text = "3850571";
            this.frmSetRefLocationRefAltitudeLabel.AutoSize = true;
            this.frmSetRefLocationRefAltitudeLabel.Location = new Point(0xc3, 0x51);
            this.frmSetRefLocationRefAltitudeLabel.Name = "frmSetRefLocationRefAltitudeLabel";
            this.frmSetRefLocationRefAltitudeLabel.Size = new Size(0x2a, 13);
            this.frmSetRefLocationRefAltitudeLabel.TabIndex = 15;
            this.frmSetRefLocationRefAltitudeLabel.Text = "Altitude";
            this.frmSetRefLocationRefLocationComboBox.FormattingEnabled = true;
            this.frmSetRefLocationRefLocationComboBox.Location = new Point(0x19, 20);
            this.frmSetRefLocationRefLocationComboBox.Name = "frmSetRefLocationRefLocationComboBox";
            this.frmSetRefLocationRefLocationComboBox.Size = new Size(0x97, 0x15);
            this.frmSetRefLocationRefLocationComboBox.Sorted = true;
            this.frmSetRefLocationRefLocationComboBox.TabIndex = 0;
            this.frmSetRefLocationRefLocationComboBox.SelectedIndexChanged += new EventHandler(this.frmSetRefLocationRefLocationComboBox_SelectedIndexChanged);
            this.frmSetRefLocationOkBtn.Location = new Point(0x77, 0x93);
            this.frmSetRefLocationOkBtn.Name = "frmSetRefLocationOkBtn";
            this.frmSetRefLocationOkBtn.Size = new Size(0x4b, 0x17);
            this.frmSetRefLocationOkBtn.TabIndex = 2;
            this.frmSetRefLocationOkBtn.Text = "&OK";
            this.frmSetRefLocationOkBtn.UseVisualStyleBackColor = true;
            this.frmSetRefLocationOkBtn.Click += new EventHandler(this.frmSetRefLocationOkBtn_Click);
            this.frmSetRefLocationCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmSetRefLocationCancelBtn.Location = new Point(0xd9, 0x93);
            this.frmSetRefLocationCancelBtn.Name = "frmSetRefLocationCancelBtn";
            this.frmSetRefLocationCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmSetRefLocationCancelBtn.TabIndex = 2;
            this.frmSetRefLocationCancelBtn.Text = "&Cancel";
            this.frmSetRefLocationCancelBtn.UseVisualStyleBackColor = true;
            this.frmSetRefLocationCancelBtn.Click += new EventHandler(this.frmSetRefLocationCancelBtn_Click);
            base.AcceptButton = this.frmSetRefLocationOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmSetRefLocationCancelBtn;
            base.ClientSize = new Size(0x19b, 0xb8);
            base.Controls.Add(this.frmSetRefLocationCancelBtn);
            base.Controls.Add(this.frmSetRefLocationOkBtn);
            base.Controls.Add(this.frmRxInitCmdRefLocationGrpBox);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmSetReferenceLocation";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Set Reference Location";
            base.Load += new EventHandler(this.frmSetReferenceLocation_Load);
            this.frmRxInitCmdRefLocationGrpBox.ResumeLayout(false);
            this.frmRxInitCmdRefLocationGrpBox.PerformLayout();
            base.ResumeLayout(false);
        }

        protected override void OnClosed(EventArgs e)
        {
        }

        private void updateConfigList(string updatedName)
        {
            this.comm.RxCtrl.RxNavData.RefLocationName = updatedName;
            this.updateReferenceLocationComboBox();
        }

        private void updateDefautlReferenceLocationComboBox()
        {
            this._maxItemWidth = this._defaultWidth;
            if (this.frmSetRefLocationRefLocationComboBox.Items.Count != 0)
            {
                this.frmSetRefLocationRefLocationComboBox.Items.Clear();
            }
            ArrayList referenceLocationName = new ArrayList();
            referenceLocationName = this.comm.m_NavData.GetReferenceLocationName();
            for (int i = 0; i < referenceLocationName.Count; i++)
            {
                this.frmSetRefLocationRefLocationComboBox.Items.Add(referenceLocationName[i]);
                if (this._maxItemWidth < (referenceLocationName[i].ToString().Length * 6))
                {
                    this._maxItemWidth = referenceLocationName[i].ToString().Length * 6;
                }
            }
            this.frmSetRefLocationRefLocationComboBox.Items.Add("USER_DEFINED");
            if (this.comm.m_NavData.RefLocationName == "")
            {
                this.comm.m_NavData.RefLocationName = "Default";
            }
            this.frmSetRefLocationRefLocationComboBox.Text = this.comm.m_NavData.RefLocationName;
            this.updateReferencePositionTextBox();
        }

        private void updateReferenceLocationComboBox()
        {
            try
            {
                if (!this.frmSetRefLocationRefLocationComboBox.Items.Contains(this.comm.RxCtrl.RxNavData.RefLocationName))
                {
                    this.frmSetRefLocationRefLocationComboBox.Items.Add(this.comm.RxCtrl.RxNavData.RefLocationName);
                    this.frmSetRefLocationRefLocationComboBox.Text = this.comm.RxCtrl.RxNavData.RefLocationName;
                    this.frmSetRefLocationRefLatitudeTxtBox.Enabled = true;
                    this.frmSetRefLocationRefLongitudeTxtBox.Enabled = true;
                    this.frmSetRefLocationRefAltitudeTxtBox.Enabled = true;
                    this.button_FixPCurrentLocation.Enabled = true;
                }
                else
                {
                    this.frmSetRefLocationRefLocationComboBox.Text = this.comm.RxCtrl.RxNavData.RefLocationName;
                    this.frmSetRefLocationRefLatitudeTxtBox.Enabled = false;
                    this.frmSetRefLocationRefLongitudeTxtBox.Enabled = false;
                    this.frmSetRefLocationRefAltitudeTxtBox.Enabled = false;
                    this.button_FixPCurrentLocation.Enabled = false;
                }
                this.updateReferencePositionTextBox();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void updateReferencePositionTextBox()
        {
            PositionInLatLonAlt referencePosition = this.comm.m_NavData.GetReferencePosition(this.frmSetRefLocationRefLocationComboBox.Text);
            if (referencePosition.name == "LAST_FIX_POSITION")
            {
                if (this.comm.m_NavData.MeasLat != -9999.0)
                {
                    referencePosition.latitude = this.comm.m_NavData.MeasLat;
                }
                if (this.comm.m_NavData.MeasLon != -9999.0)
                {
                    referencePosition.longitude = this.comm.m_NavData.MeasLon;
                }
                if (this.comm.m_NavData.MeasAlt != -9999.0)
                {
                    referencePosition.altitude = this.comm.m_NavData.MeasAlt;
                }
            }
            this.frmSetRefLocationRefLatitudeTxtBox.Text = referencePosition.latitude.ToString();
            this.frmSetRefLocationRefLongitudeTxtBox.Text = referencePosition.longitude.ToString();
            this.frmSetRefLocationRefAltitudeTxtBox.Text = referencePosition.altitude.ToString();
            this.frmSetRefLocationRefLocationComboBox.Text = referencePosition.name;
            this.frmSetRefLocationRefLocationComboBox.Width = this._defaultWidth;
        }

        public CommunicationManager CommWindow
        {
            get
            {
                return this.comm;
            }
            set
            {
                this.comm = value;
            }
        }
    }
}

