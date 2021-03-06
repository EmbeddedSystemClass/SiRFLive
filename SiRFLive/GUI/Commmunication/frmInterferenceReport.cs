﻿namespace SiRFLive.GUI.Commmunication
{
    using CommMgrClassLibrary;
    using CommonClassLibrary;
    using CommonUtilsClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using System.Timers;

    public class frmInterferenceReport : Form
    {
        private List<string> _CWControllerScanResultList = new List<string>();
        private double BlueLimit;
        private NumericUpDown BlueNumericUpDown;
        public CommMgrClass CMC;
        public CommunicationManager comm;
        private IContainer components;
        private CommonUtilsClass CUC = new CommonUtilsClass();
        private ToolStripComboBox cwConfigComboBox;
        private ToolStripLabel cwConfigLabel;
        private ToolStripButton cwDetectionStart;
        private ToolStripButton cwHelpBtn;
        private ToolStripButton cwPlotClearBtn;
        private CommonClass.BinarycheckBox detectionOnCheckBox;
        private double GreenLimit;
        private NumericUpDown GreenNumericUpDown;
        public CW_PeakRegs[] InterferenceReport = new CW_PeakRegs[8];
        private Label label2;
        private Label label3;
        private Label label5;
        public object LockInterferenceReport = new object();
        private ZedGraph.GraphPane myPane = new ZedGraph.GraphPane();
        private int noMsgCnt;
        private double RedLimit;
        private NumericUpDown RedNumericUpDown;
        private System.Timers.Timer refreshTimer = new System.Timers.Timer();
        private ToolStrip toolStrip1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;
        private ZedGraph.ZedGraphControl zedGraphControl1;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmInterferenceReport(CommunicationManager mainComWin)
        {
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
            this.CommWindow = mainComWin;
            this.zedGraphControl1.GraphPane = this.myPane;
            this.cwConfigComboBox.SelectedIndex = 0;
            this.CMC = new CommMgrClass();
            this.refreshTimer.Elapsed += new ElapsedEventHandler(this.updateSignalWindow);
            this.refreshTimer.Interval = 2000.0;
            this.refreshTimer.AutoReset = true;
            this.refreshTimer.Start();
        }

        private static void AddlabelToBars(ZedGraph.GraphPane myPn, double[] X, double[] Y)
        {
            for (int i = 0; i < Y.Length; i++)
            {
                ZedGraph.TextObj item = new ZedGraph.TextObj(Y[i].ToString("F1"), (double) ((float) X[i]), (double) (((Y[i] < 0.0) ? ((float) 0.0) : ((float) Y[i])) + 1f));
                item.Location.CoordinateFrame = ZedGraph.CoordType.AxisXYScale;
                item.Location.AlignH = ZedGraph.AlignH.Left;
                item.Location.AlignV = ZedGraph.AlignV.Center;
                item.FontSpec.Border.IsVisible = false;
                item.FontSpec.Fill.IsVisible = false;
                item.FontSpec.Angle = 90f;
                myPn.GraphObjList.Add(item);
            }
        }

        private void BlueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
			base.Invoke((MethodInvoker)delegate
			{
                this.BlueLimit = (double) this.BlueNumericUpDown.Value;
            });
        }

        private void clearImage()
        {
            EventHandler method = null;
            lock (this.LockInterferenceReport)
            {
                this.zedGraphControl1.GraphPane.CurveList.Clear();
                this.zedGraphControl1.GraphPane.GraphObjList.Clear();
                this.Refresh();
                if ((this.comm != null) && (this.comm.CWInterferenceDetectModeIndex == 3))
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.detectionOnCheckBox.BackgroundBrush = new SolidBrush(Color.Gray);
                            this.detectionOnCheckBox.Checked = false;
                            this.detectionOnCheckBox.Refresh();
                        };
                    }
                    base.BeginInvoke(method);
                }
            }
        }

        public void CreateChart(ZedGraph.ZedGraphControl zgc, ref CW_PeakRegs[] IR)
        {
            EventHandler method = null;
            try
            {
                zgc.GraphPane = this.myPane;
                zgc.GraphPane.CurveList.Clear();
                zgc.GraphPane.GraphObjList.Clear();
                this.RedLimit = (double) this.RedNumericUpDown.Value;
                this.BlueLimit = (double) this.BlueNumericUpDown.Value;
                this.GreenLimit = (double) this.GreenNumericUpDown.Value;
                this.myPane.Title.Text = "CW Interference Detection";
                this.myPane.YAxis.Title.Text = "Power dB/Hz";
                this.myPane.XAxis.Title.Text = "Frequency (from L1 center)";
                this.myPane.BarSettings.Type = ZedGraph.BarType.Overlay;
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (IR[i].magD >= this.GreenLimit)
                    {
                        if (IR[i].magD >= this.RedLimit)
                        {
                            num++;
                        }
                        if ((IR[i].magD >= this.BlueLimit) && (IR[i].magD < this.RedLimit))
                        {
                            num2++;
                        }
                        if (IR[i].magD < this.BlueLimit)
                        {
                            num3++;
                        }
                    }
                }
                double[] x = new double[num];
                double[] y = new double[num];
                double[] numArray3 = new double[num3];
                double[] numArray4 = new double[num3];
                double[] numArray5 = new double[num2];
                double[] numArray6 = new double[num2];
                int index = -1;
                int num6 = -1;
                int num7 = -1;
                for (int j = 0; j < 8; j++)
                {
                    try
                    {
                        if (IR[j].magD >= this.GreenLimit)
                        {
                            if ((num > 0) && (IR[j].magD >= this.RedLimit))
                            {
                                index++;
                                x[index] = IR[j].freqD;
                                y[index] = IR[j].magD;
                            }
                            if (((num2 > 0) && (IR[j].magD >= this.BlueLimit)) && (IR[j].magD < this.RedLimit))
                            {
                                num6++;
                                numArray5[num6] = IR[j].freqD;
                                numArray6[num6] = IR[j].magD;
                            }
                            if ((num3 > 0) && (IR[j].magD < this.BlueLimit))
                            {
                                num7++;
                                numArray3[num7] = IR[j].freqD;
                                numArray4[num7] = IR[j].magD;
                            }
                        }
                    }
                    catch (IndexOutOfRangeException exception)
                    {
                        string str = exception.ToString();
                        string msg = "### Interference Report GUI handler error -- " + str;
                        this.comm.WriteApp(msg);
                    }
                }
                if (num > 0)
                {
                    string label = string.Format(">{0:F1}", this.RedLimit);
                    this.myPane.AddBar(label, x, y, Color.Red);
                }
                if (num2 > 0)
                {
                    string str4 = string.Format(">{0:F1}", this.BlueLimit);
                    this.myPane.AddBar(str4, numArray5, numArray6, Color.Blue);
                }
                if (num3 > 0)
                {
                    string str5 = string.Format("<{0:F1}", this.GreenLimit);
                    this.myPane.AddBar(str5, numArray3, numArray4, Color.Green);
                }
                AddlabelToBars(this.myPane, x, y);
                AddlabelToBars(this.myPane, numArray5, numArray6);
                AddlabelToBars(this.myPane, numArray3, numArray4);
                this.myPane.Chart.Fill = new ZedGraph.Fill(Color.White, Color.LightGoldenrodYellow, 45f);
                this.myPane.YAxis.Scale.MaxAuto = false;
                this.myPane.YAxis.Scale.MinAuto = false;
                this.myPane.XAxis.Scale.MaxAuto = false;
                this.myPane.XAxis.Scale.MinAuto = false;
                this.myPane.YAxis.Scale.Max = 100.0;
                this.myPane.YAxis.Scale.Min = 0.0;
                this.myPane.XAxis.Scale.Min = 1.57E+15;
                this.myPane.XAxis.Scale.Max = 1.58E+15;
                this.myPane.BarSettings.ClusterScaleWidthAuto = false;
                this.myPane.BarSettings.ClusterScaleWidth = 1000000000000;
                zgc.AxisChange();
                this.myPane.Fill = new ZedGraph.Fill(Color.WhiteSmoke);
                if (method == null)
                {
                    method = delegate {
                        zgc.Refresh();
                        zgc.Update();
                    };
                }
                base.Invoke(method);
                zgc.Size = new Size(base.ClientRectangle.Width - 0x19, base.ClientRectangle.Height - 90);
            }
            catch (Exception exception2)
            {
                string str6 = exception2.ToString();
                string str7 = "### Interference Report GUI handler error -- " + str6;
                this.comm.WriteApp(str7);
            }
        }

        private void CreateSameData(ref CW_PeakRegs[] IR)
        {
            IR[0].freqD = -1270000.0;
            IR[1].freqD = -475000.0;
            IR[2].freqD = -200000.0;
            IR[3].freqD = 1500000.0;
            IR[4].freqD = 2000000.0;
            IR[5].freqD = 2270000.0;
            IR[6].freqD = 2475000.0;
            IR[7].freqD = 3125000.0;
            IR[0].magD = 5.0;
            IR[1].magD = 4.0;
            IR[2].magD = 10.0;
            IR[3].magD = 20.0;
            IR[4].magD = 25.0;
            IR[5].magD = 5.0;
            IR[6].magD = 4.0;
            IR[7].magD = 6.0;
        }

        private void cwDetectionStart_Click(object sender, EventArgs e)
        {
            bool isSLCRx = false;
            int mid = 220;
            int sid = 1;
            byte channelType = 0;
            this.comm.CMC.TxCurrentTransmissionType = (CommonClass.TransmissionType) this.comm.TxCurrentTransmissionType;
            string protocol = "OSP";
            isSLCRx = this.comm.RxType == CommunicationManager.ReceiverType.SLC;
            ArrayList list = this.comm.m_Protocols.GetDefaultMsgFieldList(isSLCRx, mid, sid, "CW Controller Input_CONFIG", protocol);
            InputMsg[] fieldList = new InputMsg[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                fieldList[i].messageID = ((InputMsg) list[i]).messageID;
                fieldList[i].subID = ((InputMsg) list[i]).subID;
                fieldList[i].messageName = ((InputMsg) list[i]).messageName;
                fieldList[i].fieldNumber = ((InputMsg) list[i]).fieldNumber;
                fieldList[i].fieldName = ((InputMsg) list[i]).fieldName;
                fieldList[i].bytes = ((InputMsg) list[i]).bytes;
                fieldList[i].datatype = ((InputMsg) list[i]).datatype;
                fieldList[i].units = ((InputMsg) list[i]).units;
                fieldList[i].scale = ((InputMsg) list[i]).scale;
                fieldList[i].defaultValue = ((InputMsg) list[i]).defaultValue;
                fieldList[i].savedValue = ((InputMsg) list[i]).savedValue;
            }
            try
            {
                string text = this.cwConfigComboBox.Text;
                if (text == string.Empty)
                {
                    return;
                }
                if (text.StartsWith("2:"))
                {
                    fieldList[2].defaultValue = "2";
                    this.comm.CWInterferenceDetectModeIndex = 1;
                }
                else if (text.StartsWith("3:"))
                {
                    fieldList[2].defaultValue = "3";
                    this.comm.CWInterferenceDetectModeIndex = 2;
                }
                else if (text.StartsWith("4:"))
                {
                    fieldList[2].defaultValue = "4";
                    this.comm.CWInterferenceDetectModeIndex = 3;
                }
                else
                {
                    fieldList[2].defaultValue = "0";
                    this.comm.CWInterferenceDetectModeIndex = 0;
                }
                if (this.comm.CWInterferenceDetectModeIndex != 3)
                {
                    this.detectionOnCheckBox.BackgroundBrush = new SolidBrush(Color.Green);
                    this.detectionOnCheckBox.Checked = true;
                    this.detectionOnCheckBox.Refresh();
                }
                else
                {
                    this.detectionOnCheckBox.BackgroundBrush = new SolidBrush(Color.Gray);
                    this.detectionOnCheckBox.Checked = false;
                    this.detectionOnCheckBox.Refresh();
                }
            }
            catch (Exception exception)
            {
                string str3 = exception.ToString();
                string str4 = "### Interference Report GUI handler error -- " + str3;
                this.comm.WriteApp(str4);
            }
            string msg = this.comm.m_Protocols.FieldList_to_HexMsgStr(isSLCRx, fieldList, channelType);
            this.comm.WriteData(msg);
        }

        private void cwHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(clsGlobal.PlotHelpString, "Plot Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void CWInterferenceReportMsgReceived(ref CW_PeakRegs[] IR, string decodedScanResultsStr)
        {
            try
            {
                string[] strArray = decodedScanResultsStr.Split(new char[] { ' ' });
                int num2 = strArray.GetLength(0) / 2;
                lock (this.LockInterferenceReport)
                {
                    for (int i = 0; i < num2; i++)
                    {
                        IR[i].freq = Convert.ToInt32(strArray[i], 0x10);
                    }
                    for (int j = 0; j < num2; j++)
                    {
                        IR[j].mag = Convert.ToUInt16(strArray[num2 + j], 0x10);
                    }
                    IR[0].freqD = IR[0].freq * 1000000.0;
                    IR[1].freqD = IR[1].freq * 1000000.0;
                    IR[2].freqD = IR[2].freq * 1000000.0;
                    IR[3].freqD = IR[3].freq * 1000000.0;
                    IR[4].freqD = IR[4].freq * 1000000.0;
                    IR[5].freqD = IR[5].freq * 1000000.0;
                    IR[6].freqD = IR[6].freq * 1000000.0;
                    IR[7].freqD = IR[7].freq * 1000000.0;
                    IR[0].magD = IR[0].mag * 0.01;
                    IR[1].magD = IR[1].mag * 0.01;
                    IR[2].magD = IR[2].mag * 0.01;
                    IR[3].magD = IR[3].mag * 0.01;
                    IR[4].magD = IR[4].mag * 0.01;
                    IR[5].magD = IR[5].mag * 0.01;
                    IR[6].magD = IR[6].mag * 0.01;
                    IR[7].magD = IR[7].mag * 0.01;
                }
            }
            catch (Exception exception)
            {
                string msg = "### Interference Report GUI handler error -- " + exception.Message;
                this.comm.WriteApp(msg);
            }
        }

        private void cwPlotClearBtn_Click(object sender, EventArgs e)
        {
            this.clearImage();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmInterferenceCWControllerScanResultDisplayQueueHandler(object sender, DoWorkEventArgs myQContent)
        {
            EventHandler method = null;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                MessageQData argument = (MessageQData) myQContent.Argument;
                if (argument.MessageText != string.Empty)
                {
                    string gWControllerScanResult = this.comm.RxCtrl.GetGWControllerScanResult(argument.MessageText);
                    if (gWControllerScanResult != string.Empty)
                    {
                        this.CWInterferenceReportMsgReceived(ref this.InterferenceReport, gWControllerScanResult);
                        this.CreateChart(this.zedGraphControl1, ref this.InterferenceReport);
                        if (method == null)
                        {
                            method = delegate {
                                this.detectionOnCheckBox.Checked = true;
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    this.noMsgCnt = 0;
                }
            }
            catch (Exception exception)
            {
                string msg = "### Interference Report GUI handler error -- " + exception.Message;
                this.comm.WriteApp(msg);
            }
        }

        private void frmInterferenceReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopListeners();
        }

        private void frmInterferenceReport_Load(object sender, EventArgs e)
        {
            this.CreateChart(this.zedGraphControl1, ref this.InterferenceReport);
            this.detectionOnCheckBox.BackgroundBrush = new SolidBrush(Color.Gray);
            this.detectionOnCheckBox.Checked = false;
            if (this.comm != null)
            {
                this.cwConfigComboBox.SelectedIndex = this.comm.CWInterferenceDetectModeIndex;
            }
            else
            {
                this.cwConfigComboBox.SelectedIndex = 0;
            }
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ListenerWaitTask));
            thread.IsBackground = true;
            thread.Start();
            base.Top = this.WinTop;
            base.Left = this.WinLeft;
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
        }

        private void frmInterferenceReport_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void GreenNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
			base.Invoke((MethodInvoker)delegate
			{
                this.GreenLimit = (double) this.GreenNumericUpDown.Value;
            });
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmInterferenceReport));
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.RedNumericUpDown = new NumericUpDown();
            this.label2 = new Label();
            this.BlueNumericUpDown = new NumericUpDown();
            this.label3 = new Label();
            this.GreenNumericUpDown = new NumericUpDown();
            this.label5 = new Label();
            this.toolStrip1 = new ToolStrip();
            this.cwConfigLabel = new ToolStripLabel();
            this.cwConfigComboBox = new ToolStripComboBox();
            this.cwDetectionStart = new ToolStripButton();
            this.cwPlotClearBtn = new ToolStripButton();
            this.cwHelpBtn = new ToolStripButton();
            this.detectionOnCheckBox = new CommonClass.BinarycheckBox();
            this.RedNumericUpDown.BeginInit();
            this.BlueNumericUpDown.BeginInit();
            this.GreenNumericUpDown.BeginInit();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.zedGraphControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.Location = new Point(15, 0x51);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0.0;
            this.zedGraphControl1.ScrollMaxX = 0.0;
            this.zedGraphControl1.ScrollMaxY = 0.0;
            this.zedGraphControl1.ScrollMaxY2 = 0.0;
            this.zedGraphControl1.ScrollMinX = 0.0;
            this.zedGraphControl1.ScrollMinY = 0.0;
            this.zedGraphControl1.ScrollMinY2 = 0.0;
            this.zedGraphControl1.Size = new Size(0x22c, 0x1a5);
            this.zedGraphControl1.TabIndex = 12;
            this.RedNumericUpDown.Location = new Point(0x3d, 50);
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.RedNumericUpDown.Maximum = new decimal(bits);
            this.RedNumericUpDown.Name = "RedNumericUpDown";
            this.RedNumericUpDown.Size = new Size(0x39, 20);
            this.RedNumericUpDown.TabIndex = 13;
            int[] numArray2 = new int[4];
            numArray2[0] = 50;
            this.RedNumericUpDown.Value = new decimal(numArray2);
            this.RedNumericUpDown.ValueChanged += new EventHandler(this.RedNumericUpDown_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.Green;
            this.label2.Location = new Point(0xeb, 0x36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2d, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Green:";
            this.BlueNumericUpDown.Location = new Point(0xa9, 50);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x2710;
            this.BlueNumericUpDown.Maximum = new decimal(numArray3);
            this.BlueNumericUpDown.Name = "BlueNumericUpDown";
            this.BlueNumericUpDown.Size = new Size(0x39, 20);
            this.BlueNumericUpDown.TabIndex = 15;
            int[] numArray4 = new int[4];
            numArray4[0] = 40;
            this.BlueNumericUpDown.Value = new decimal(numArray4);
            this.BlueNumericUpDown.ValueChanged += new EventHandler(this.BlueNumericUpDown_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.ForeColor = Color.Blue;
            this.label3.Location = new Point(0x80, 0x36);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 13);
            this.label3.TabIndex = 0x10;
            this.label3.Text = "Blue:";
            this.GreenNumericUpDown.Location = new Point(0x11e, 50);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x2710;
            this.GreenNumericUpDown.Maximum = new decimal(numArray5);
            this.GreenNumericUpDown.Name = "GreenNumericUpDown";
            this.GreenNumericUpDown.Size = new Size(0x39, 20);
            this.GreenNumericUpDown.TabIndex = 0x11;
            int[] numArray6 = new int[4];
            numArray6[0] = 30;
            this.GreenNumericUpDown.Value = new decimal(numArray6);
            this.GreenNumericUpDown.ValueChanged += new EventHandler(this.GreenNumericUpDown_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.ForeColor = Color.Red;
            this.label5.Location = new Point(12, 0x36);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x22, 13);
            this.label5.TabIndex = 0x10;
            this.label5.Text = "Red:";
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.cwConfigLabel, this.cwConfigComboBox, this.cwDetectionStart, this.cwPlotClearBtn, this.cwHelpBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x247, 0x19);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            this.cwConfigLabel.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.cwConfigLabel.Name = "cwConfigLabel";
            this.cwConfigLabel.Size = new Size(110, 0x16);
            this.cwConfigLabel.Text = "CW Configuration: ";
            this.cwConfigComboBox.Items.AddRange(new object[] { "0:  Enable scan,  enable filtering", "2:  Enable scan,  use 2MHz", "3:  Enable scan,  disable filtering", "4:  Disable scan,  disable filtering" });
            this.cwConfigComboBox.Name = "cwConfigComboBox";
            this.cwConfigComboBox.Size = new Size(250, 0x19);
            this.cwDetectionStart.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.cwDetectionStart.Image = Resources.graphhsCW;
            this.cwDetectionStart.ImageTransparentColor = Color.Magenta;
            this.cwDetectionStart.Name = "cwDetectionStart";
            this.cwDetectionStart.Size = new Size(0x17, 0x16);
            this.cwDetectionStart.Text = "Set";
            this.cwDetectionStart.Click += new EventHandler(this.cwDetectionStart_Click);
            this.cwPlotClearBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.cwPlotClearBtn.Image = Resources.clearTableHS;
            this.cwPlotClearBtn.ImageTransparentColor = Color.Magenta;
            this.cwPlotClearBtn.Name = "cwPlotClearBtn";
            this.cwPlotClearBtn.Size = new Size(0x17, 0x16);
            this.cwPlotClearBtn.Text = "Clear";
            this.cwPlotClearBtn.Click += new EventHandler(this.cwPlotClearBtn_Click);
            this.cwHelpBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.cwHelpBtn.Image = Resources.Help;
            this.cwHelpBtn.ImageTransparentColor = Color.Magenta;
            this.cwHelpBtn.Name = "cwHelpBtn";
            this.cwHelpBtn.Size = new Size(0x17, 0x16);
            this.cwHelpBtn.Text = "Help";
            this.cwHelpBtn.Click += new EventHandler(this.cwHelpBtn_Click);
            this.detectionOnCheckBox.AutoSize = true;
            this.detectionOnCheckBox.CheckAlign = ContentAlignment.MiddleRight;
            this.detectionOnCheckBox.FlatStyle = FlatStyle.Flat;
            this.detectionOnCheckBox.Location = new Point(12, 0x1b);
            this.detectionOnCheckBox.Name = "detectionOnCheckBox";
            this.detectionOnCheckBox.Size = new Size(0x66, 0x11);
            this.detectionOnCheckBox.TabIndex = 0x25;
            this.detectionOnCheckBox.Text = "Detection Active";
            this.detectionOnCheckBox.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x247, 0x202);
            base.Controls.Add(this.detectionOnCheckBox);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.GreenNumericUpDown);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.BlueNumericUpDown);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.RedNumericUpDown);
            base.Controls.Add(this.zedGraphControl1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmInterferenceReport";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "CW Interference Detection";
            base.Load += new EventHandler(this.frmInterferenceReport_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmInterferenceReport_FormClosing);
            base.LocationChanged += new EventHandler(this.frmInterferenceReport_LocationChanged);
            this.RedNumericUpDown.EndInit();
            this.BlueNumericUpDown.EndInit();
            this.GreenNumericUpDown.EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ListenerWaitTask()
        {
            while (!this.StartListen())
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.refreshTimer.Stop();
            if (this.comm != null)
            {
                this.StopListeners();
            }
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow(base.Name);
            }
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
        }

        private void RedNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
			base.Invoke((MethodInvoker)delegate
			{
                this.RedLimit = (double) this.RedNumericUpDown.Value;
            });
        }

        public bool StartListen()
        {
            bool flag = false;
            string listenerName = "CWControllerScanResult_GUI";
            if (this.comm.ListenersCtrl == null)
            {
                return flag;
            }
            if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
            {
                ListenerContent content = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                if (content != null)
                {
                    content.DoUserWork.DoWork += new DoWorkEventHandler(this.frmInterferenceCWControllerScanResultDisplayQueueHandler);
                    this._CWControllerScanResultList.Add(content.ListenerName);
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    flag = true;
                }
                return flag;
            }
            this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
            return true;
        }

        internal void StopListeners()
        {
            foreach (string str in this._CWControllerScanResultList)
            {
                if (this.comm.ListenersCtrl != null)
                {
                    this.comm.ListenersCtrl.Stop(str);
                    this.comm.ListenersCtrl.Delete(str);
                    this.comm.ListenersCtrl.DeleteListener(str);
                }
            }
            this._CWControllerScanResultList.Clear();
        }

        private void updateSignalWindow(object source, ElapsedEventArgs e)
        {
            EventHandler method = null;
            if ((this.comm != null) && this.comm.IsSourceDeviceOpen())
            {
                if (this.noMsgCnt > 5)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.clearImage();
                        };
                    }
                    base.BeginInvoke(method);
                    this.noMsgCnt = 0;
                }
                else
                {
                    this.noMsgCnt++;
                }
            }
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
                this.Text = this.comm.sourceDeviceName + ": Interference";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CW_PeakRegs
        {
            public int freq;
            public ushort mag;
            public double freqD;
            public double magD;
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

