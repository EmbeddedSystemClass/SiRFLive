﻿namespace SiRFLive.GUI
{
    using CommonUtilsClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.Configuration;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class frmFileAnalysis : Form
    {
        private bool _abort;
        private List<string> _analyzedMsgNamesList = new List<string>(msgNamesArray);
        private List<string> _analyzedMsgsList = new List<string>(msgMatchesArray);
        private string _fileType = ".gpx";
        private List<StreamWriter> _msgIndexFileList = new List<StreamWriter>();
        private int _processesdFilesCount;
        private bool _processingStatus;
        private Hashtable _resultHash = new Hashtable();
        private List<string> _rxNamesList = new List<string>();
        private List<string> _softwareVersionsList = new List<string>();
        private bool autoDetect = true;
        private IniHelper autoIniIntf;
        private Button automationTestAbortBtn;
        private Button autoTestAddAllBtn;
        private Button autoTestAddBtn;
        private Label autoTestAvailableScriptsLabel;
        private Button autoTestClearAvailableListBtn;
        private Button autoTestDirBrowser;
        private TextBox autoTestDirVal;
        private Button autoTestExitBtn;
        private Label autoTestFilePathLabel;
        private Button autoTestRefreshBtn;
        private Button autoTestRemoveAllBtn;
        private Button autoTestRemoveBtn;
        private Button autoTestRunBtn;
        private Label autoTestRunScriptsLabel;
        private List<string> availableCategoryList = new List<string>();
        private IContainer components;
        private ObjectInterface crossThreadUpdateIntf = new ObjectInterface();
        private EmailHandler emailHelper = new EmailHandler();
        private Button fileAnalysisAddAllCategoryBtn;
        private Button fileAnalysisAddCategoryBtn;
        private ListBox fileAnalysisAvailableFilesListVal;
        private ListBox fileAnalysisCategoryList;
        private string fileAnalysisIniPath = (clsGlobal.InstalledDirectory + @"\Config\SiRFLiveFileAnalysis.cfg");
        private Button fileAnalysisRemoveAllCategoryBtn;
        private Button fileAnalysisRemoveCategoryBtn;
        private ListBox fileAnalysisToProcessFilesListVal;
        private ListBox fileAnalysisToRunCategoryList;
        private static int fileIdx = 0;
        private Label fileProcessedStatusLabel;
        private string[] filesArray = new string[0];
        private List<string> filesFullPathLists = new List<string>();
        private Label frmFileConversionFilesCntLabel;
        private const double GPS_DOP_LSB = 0.2;
        private const int GPS_MODE_DGPS_USED = 0x80;
        private const int GPS_MODE_MASK = 7;
        private const int GPS_NUM_CHANNELS = 12;
        private CheckBox includeDateTimeChkBox;
        private Label label1;
        private static frmFileAnalysis m_SChildform;
        private static string[] msgMatchesArray = new string[] { "2,", "4,", "41,", "69,1", "69,2", "80 00", "rt_raise, heap error" };
        private static string[] msgNamesArray = new string[] { "Msg2", "Msg4", "Msg41", "Msg69_1", "Msg69_2", "Msg128", "MsgError" };
        private float[] Power10 = new float[] { 1f, 10f, 100f, 1000f, 10000f, 100000f, 1000000f, 1E+07f, 1E+08f, 1E+09f, 1E+10f, 1E+11f, 1E+12f };
        private Label processedFilesLabel;
        private ProgressBar progressBar1;
        private string testResults = string.Empty;
        private List<string> toAnalysisCategoryList = new List<string>();
        private List<string> toRunList = new List<string>();
        private string[] toRunListArray = new string[0];
        private bool useOspMsg69 = true;
        private bool useSSBMsg41 = true;

        public frmFileAnalysis(string fileType)
        {
            this.InitializeComponent();
            string str = string.Empty;
            this._fileType = fileType;
            try
            {
                if (File.Exists(this.fileAnalysisIniPath))
                {
                    this.autoIniIntf = new IniHelper(this.fileAnalysisIniPath);
                    foreach (string str2 in this.autoIniIntf.GetKeys("EMAIL"))
                    {
                        if (!str2.Contains("#"))
                        {
                            str = this.autoIniIntf.GetIniFileString("EMAIL", str2, "");
                            if (str.Length != 0)
                            {
                                str = str.Replace(" ", "").TrimEnd(new char[] { '\n' }).TrimEnd(new char[] { '\r' });
                                ConfigurationManager.AppSettings[str2] = str;
                            }
                        }
                    }
                    str = this.autoIniIntf.GetIniFileString("SETUP", "AVAILABLE_SCRIPTS", "");
                    if (str.Length != 0)
                    {
                        this.filesFullPathLists.Clear();
                        foreach (string str3 in str.Split(new char[] { ',' }))
                        {
                            if (str3.Length != 0)
                            {
                                this.addAvailableFiles(str3);
                            }
                        }
                    }
                    this.filesArray = this.filesFullPathLists.ToArray();
                    this.autoTestDirVal.Text = this.autoIniIntf.GetIniFileString("SETUP", "SCRIPTS_DIR", "");
                    str = this.autoIniIntf.GetIniFileString("SETUP", "SEND_EMAIL", "");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void addAvailableFiles(string fileStr)
        {
            string[] strArray = new string[] { "" };
            int length = 0;
            this.filesFullPathLists.Add(fileStr);
            strArray = fileStr.Split(new char[] { '\\' });
            length = strArray.Length;
            this.fileAnalysisAvailableFilesListVal.Items.Add(strArray[length - 1]);
        }

        private string AddCheckSum(string inputString)
        {
            inputString.Replace(" ", "");
            char[] chArray = inputString.ToCharArray();
            byte num = 0;
            int num2 = 1;
            while (num2 < inputString.Length)
            {
                num = (byte) (num ^ Convert.ToByte(chArray[num2++]));
            }
            return (inputString + string.Format("*{0:X2}\r\n", num));
        }

        private void AnalyzeFiles()
        {
            this.parseGPSFile();
        }

        private void cleanupHashtablesAndLists()
        {
            this._resultHash.Clear();
            this._analyzedMsgsList.Clear();
            this._analyzedMsgNamesList.Clear();
            this._rxNamesList.Clear();
            this._msgIndexFileList.Clear();
        }

        private void conversionAbort()
        {
            if (MessageBox.Show("Abort conversion?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                this._abort = true;
                this._processingStatus = false;
            }
        }

        private void convertFiles()
        {
            EventHandler method = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            EventHandler handler7 = null;
            EventHandler handler8 = null;
            EventHandler handler9 = null;
            EventHandler handler10 = null;
            CommunicationManager manager = new CommunicationManager();
            manager.RxCtrl = new OSPReceiver();
            manager.RxCtrl.ResetCtrl = new OSPReset();
            StreamWriter writer = null;
            StreamWriter writer2 = null;
            StreamReader reader = null;
            LargeFileHandler handler = null;
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            try
            {
                foreach (string str in this.toRunList)
                {
                    if (!File.Exists(str))
                    {
                        this._processesdFilesCount++;
                        if (method == null)
                        {
                            method = delegate {
                                this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                            };
                        }
                        this.processedFilesLabel.BeginInvoke(method);
                        continue;
                    }
                    string str2 = string.Empty;
                    string path = string.Empty;
                    string str4 = string.Empty;
                    if (str.EndsWith(".gp2"))
                    {
                        path = str.Replace(".gp2", ".gps");
                    }
                    else if (str.EndsWith(".gpx"))
                    {
                        path = str.Replace(".gpx", ".gps");
                    }
                    else if (str.EndsWith(".bin"))
                    {
                        path = str.Replace(".bin", ".gps");
                        str4 = str.Replace(".bin", ".gp2");
                    }
                    else if (str.EndsWith(".gps"))
                    {
                        if (this.useSSBMsg41)
                        {
                            path = str.Replace(".gps", "_msg41.nmea");
                        }
                        else if (this.useOspMsg69)
                        {
                            path = str.Replace(".gps", "_msg69.nmea");
                        }
                        else if (this.autoDetect)
                        {
                            path = str.Replace(".gps", "_auto.nmea");
                            str4 = str.Replace(".gps", ".msg69parse");
                        }
                        else
                        {
                            path = str.Replace(".gps", ".nmea");
                        }
                    }
                    else
                    {
                        path = str.Substring(0, str.Length - 4) + ".gps";
                    }
                    writer = new StreamWriter(path);
                    reader = new StreamReader(str);
                    FileInfo info = new FileInfo(str);
                    double length = info.Length;
                    if (length == 0.0)
                    {
                        this._processesdFilesCount++;
                        if (handler3 == null)
                        {
                            handler3 = delegate {
                                this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                            };
                        }
                        this.processedFilesLabel.BeginInvoke(handler3);
                        continue;
                    }
                    if (handler4 == null)
                    {
                        handler4 = delegate {
                            this.progressBar1.Value = 0;
                            this.progressBar1.Maximum = 100;
                            this.progressBar1.Minimum = 0;
                        };
                    }
                    this.progressBar1.BeginInvoke(handler4);
                    long num2 = 0L;
                    string str13 = this._fileType;
                    if (str13 != null)
                    {
                        if (!(str13 == ".bin"))
                        {
                            if (str13 == ".gpx")
                            {
                                goto Label_042F;
                            }
                            if (str13 == ".gps")
                            {
                                goto Label_055C;
                            }
                        }
                        else
                        {
                            reader.Close();
                            FileStream stream = File.OpenRead(str);
                            writer2 = new StreamWriter(str4);
                            if (handler5 == null)
                            {
                                handler5 = delegate {
                                    this.fileProcessedStatusLabel.Text = "Status: converting...";
                                };
                            }
                            this.fileProcessedStatusLabel.BeginInvoke(handler5);
                            int offset = 0;
                            int count = 0x1000;
                            byte[] buffer = new byte[count];
                            int len = stream.Read(buffer, offset, count);
                            while (len > 0)
                            {
                                manager.SetupRxCtrl();
                                manager.PopulateData(buffer, len);
                                string str5 = manager.ByteToMsgQueue(new byte[1]);
                                string[] separator = new string[] { "\r\n" };
                                foreach (string str6 in str5.Split(separator, StringSplitOptions.None))
                                {
                                    string str7 = manager.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(str6));
                                    writer.WriteLine(str7);
                                    writer2.WriteLine(CommonUtilsClass.LogToGP2(str6, string.Empty));
                                }
                                offset += len;
                                int percent = (int) ((((double) offset) / length) * 100.0);
                                if (percent > 100)
                                {
                                    percent = 100;
                                }
								this.progressBar1.BeginInvoke((MethodInvoker)delegate
								{
                                    this.progressBar1.Value = percent;
                                });
                                len = stream.Read(buffer, 0, count);
                                if (this._abort)
                                {
                                    break;
                                }
                            }
                            stream.Close();
                            writer2.Close();
                        }
                    }
                    goto Label_0A75;
                Label_042F:
                    if (handler6 == null)
                    {
                        handler6 = delegate {
                            this.fileProcessedStatusLabel.Text = "Status: converting...";
                        };
                    }
                    this.fileProcessedStatusLabel.BeginInvoke(handler6);
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        num2 += str2.Length;
                        int percent = (int) ((((double) num2) / length) * 100.0);
                        if (percent > 100)
                        {
                            percent = 100;
                        }
						this.progressBar1.BeginInvoke((MethodInvoker)delegate
						{
                            this.progressBar1.Value = percent;
                        });
                        try
                        {
                            if (str2.Contains("A0 A2"))
                            {
                                int index = str2.IndexOf("A0 A2");
                                byte[] comByte = HelperFunctions.HexToByte(str2.Substring(index));
                                string str9 = manager.m_Protocols.ConvertRawToFields(comByte);
                                if (this.includeDateTimeChkBox.Checked)
                                {
                                    writer.WriteLine(str2.Substring(0, index) + " " + str9);
                                }
                                else
                                {
                                    writer.WriteLine(str9);
                                }
                            }
                            else
                            {
                                writer.WriteLine(str2);
                            }
                        }
                        catch
                        {
                            writer.WriteLine(str2);
                            continue;
                        }
                        if (this._abort)
                        {
                            break;
                        }
                    }
                    goto Label_0A75;
                Label_055C:
                    reader.Close();
                    Queue<uint> queue = new Queue<uint>();
                    Queue<long> queue2 = new Queue<long>();
                    reader = new StreamReader(str);
                    long item = 0L;
                    if (this.autoDetect)
                    {
                        writer2 = new StreamWriter(str4);
                        if (handler7 == null)
                        {
                            handler7 = delegate {
                                this.fileProcessedStatusLabel.Text = "Status: scanning...";
                            };
                        }
                        this.fileProcessedStatusLabel.BeginInvoke(handler7);
                        str2 = reader.ReadLine();
                        while (str2 != null)
                        {
                            str2 = str2.Replace(" ", "");
                            if (str2.StartsWith("69,1"))
                            {
                                try
                                {
                                    string[] strArray3 = str2.Split(new char[] { ',' });
                                    if (strArray3.Length > 9)
                                    {
                                        uint num8 = Convert.ToUInt32(strArray3[9]);
                                        writer2.WriteLine(string.Format("{0} -- {1}", num8, strArray3[5]));
                                        if ((strArray3[3] != "0") && (strArray3[5] == "1"))
                                        {
                                            queue.Enqueue(num8);
                                            queue2.Enqueue(item);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            str2 = reader.ReadLine();
                        }
                        writer2.Close();
                        reader.Close();
                        reader = new StreamReader(str);
                    }
                    if (handler8 == null)
                    {
                        handler8 = delegate {
                            this.fileProcessedStatusLabel.Text = "Status: converting...";
                        };
                    }
                    this.fileProcessedStatusLabel.BeginInvoke(handler8);
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        string str11;
                        num2 += str2.Length;
                        int percent = (int) ((((double) num2) / length) * 100.0);
                        if (percent > 100)
                        {
                            percent = 100;
                        }
						this.progressBar1.BeginInvoke((MethodInvoker)delegate
						{
                            this.progressBar1.Value = percent;
                        });
                        str2 = str2.TrimEnd(new char[] { '\n' }).TrimEnd(new char[] { '\r' }).Replace(" ", "");
                        if (str2.StartsWith("4,"))
                        {
                            string[] strArray4 = str2.Split(new char[] { ',' });
                            string str10 = this.GPS_NMEA_OutputGSV(strArray4);
                            if (str10 != string.Empty)
                            {
                                writer.Write(str10);
                            }
                            goto Label_0A5F;
                        }
                        if (!str2.StartsWith("41,"))
                        {
                            goto Label_096E;
                        }
                        if (!this.useSSBMsg41 && !this.autoDetect)
                        {
                            goto Label_0A5F;
                        }
                        string[] msgArray = str2.Split(new char[] { ',' });
                        if (msgArray.Length < 0x22)
                        {
                            item = handler.Index + 1L;
                            str2 = handler[item];
                            continue;
                        }
                        if ((queue.Count > 0) && this.autoDetect)
                        {
                            uint num9 = Convert.ToUInt32(msgArray[4]);
                            uint num10 = queue.Peek();
                            long num11 = queue2.Peek();
                            if (num9 == num10)
                            {
                                queue.Dequeue();
                                queue2.Dequeue();
                            }
                            else
                            {
                                if (num9 > num10)
                                {
                                    if (item > num11)
                                    {
                                        while (item > num11)
                                        {
                                            queue.Dequeue();
                                            queue2.Dequeue();
                                            num10 = queue.Peek();
                                            num11 = queue2.Peek();
                                            if (num9 == num10)
                                            {
                                                break;
                                            }
                                        }
                                        if (item > num11)
                                        {
                                            goto Label_08C6;
                                        }
                                        continue;
                                    }
                                    item = handler.Index + 1L;
                                    str2 = handler[item];
                                    continue;
                                }
                                item = handler.Index + 1L;
                                str2 = handler[item];
                                continue;
                            }
                        }
                    Label_08C6:
                        str11 = this.GPS_NMEA_OutputGGA(msgArray);
                        if (str11 != string.Empty)
                        {
                            writer.Write(str11);
                        }
                        str11 = this.GPS_NMEA_OutputRMC(msgArray);
                        if (str11 != string.Empty)
                        {
                            writer.Write(str11);
                        }
                        str11 = this.GPS_NMEA_OutputGLL(msgArray);
                        if (str11 != string.Empty)
                        {
                            writer.Write(str11);
                        }
                        str11 = this.GPS_NMEA_OutputGSA(msgArray);
                        if (str11 != string.Empty)
                        {
                            writer.Write(str11);
                        }
                        str11 = this.GPS_NMEA_OutputVTG(msgArray);
                        if (str11 != string.Empty)
                        {
                            writer.Write(str11);
                        }
                        goto Label_0A5F;
                    Label_096E:
                        if (str2.StartsWith("69,1") && this.useOspMsg69)
                        {
                            string[] strArray6 = new string[0x24];
                            if (this.msg69ToMsg4AndMsg41Format(str2, ref strArray6) != 0)
                            {
                                item = handler.Index + 1L;
                                str2 = handler[item];
                                continue;
                            }
                            string str12 = this.GPS_NMEA_OutputGGA(strArray6);
                            if (str12 != string.Empty)
                            {
                                writer.Write(str12);
                            }
                            str12 = this.GPS_NMEA_OutputRMC(strArray6);
                            if (str12 != string.Empty)
                            {
                                writer.Write(str12);
                            }
                            str12 = this.GPS_NMEA_OutputGLL(strArray6);
                            if (str12 != string.Empty)
                            {
                                writer.Write(str12);
                            }
                            str12 = this.GPS_NMEA_OutputGSA(strArray6);
                            if (str12 != string.Empty)
                            {
                                writer.Write(str12);
                            }
                            str12 = this.GPS_NMEA_OutputVTG(strArray6);
                            if (str12 != string.Empty)
                            {
                                writer.Write(str12);
                            }
                        }
                    Label_0A5F:
                        if (this._abort)
                        {
                            break;
                        }
                    }
                Label_0A75:
                    if (this._abort)
                    {
                        break;
                    }
                    reader.Close();
                    writer.Close();
                    this._processesdFilesCount++;
                    if (handler9 == null)
                    {
                        handler9 = delegate {
                            this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                        };
                    }
                    this.processedFilesLabel.BeginInvoke(handler9);
                }
                if (this._abort)
                {
                    MessageBox.Show("Conversion Aborted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    if (writer2 != null)
                    {
                        writer2.Close();
                    }
                    if (handler != null)
                    {
                        handler.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Conversion Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                this._processingStatus = false;
            }
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
                if (writer2 != null)
                {
                    writer2.Close();
                }
                if (handler != null)
                {
                    handler.Close();
                }
                this._processingStatus = false;
                this._abort = false;
                MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            try
            {
                if (handler10 == null)
                {
                    handler10 = delegate {
                        this.fileProcessedStatusLabel.Text = "Status: idle";
                    };
                }
                this.fileProcessedStatusLabel.BeginInvoke(handler10);
            }
            catch
            {
            }
            manager.Dispose();
            manager = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void fileAnalysisAbortBtn_Click(object sender, EventArgs e)
        {
            this.conversionAbort();
        }

        private void fileAnalysisAddAllBtn_Click(object sender, EventArgs e)
        {
            this.fileAnalysisToProcessFilesListVal.Items.Clear();
            this.toRunList.Clear();
            foreach (string str in this.fileAnalysisAvailableFilesListVal.Items)
            {
                this.fileAnalysisToProcessFilesListVal.Items.Add(str);
            }
            foreach (string str2 in this.filesFullPathLists)
            {
                this.toRunList.Add(str2);
            }
            this.updateFiles2ProcessCnt();
        }

        private void fileAnalysisAddAllCategoryBtn_Click(object sender, EventArgs e)
        {
            this.fileAnalysisToRunCategoryList.Items.Clear();
            this.toAnalysisCategoryList.Clear();
            foreach (string str in this.fileAnalysisCategoryList.Items)
            {
                this.fileAnalysisToRunCategoryList.Items.Add(str);
            }
            foreach (string str2 in this.availableCategoryList)
            {
                this.toAnalysisCategoryList.Add(str2);
            }
        }

        private void fileAnalysisAddBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAnalysisAvailableFilesListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.fileAnalysisToProcessFilesListVal.Items.Add(this.fileAnalysisAvailableFilesListVal.SelectedItem);
                this.toRunList.Add(this.filesArray[selectedIndex]);
                this.updateFiles2ProcessCnt();
            }
        }

        private void fileAnalysisAddCategoryBtn_Click(object sender, EventArgs e)
        {
            if (this.fileAnalysisCategoryList.SelectedIndex >= 0)
            {
                this.fileAnalysisToRunCategoryList.Items.Add(this.fileAnalysisCategoryList.SelectedItem);
                this.toAnalysisCategoryList.Add(this.fileAnalysisCategoryList.Text);
            }
        }

        private void fileAnalysisAvailableFilesListVal_DoubleClick(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAnalysisAvailableFilesListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.fileAnalysisToProcessFilesListVal.Items.Add(this.fileAnalysisAvailableFilesListVal.SelectedItem);
                this.toRunList.Add(this.filesArray[selectedIndex]);
                this.updateFiles2ProcessCnt();
            }
        }

        private void fileAnalysisAvailableFilesListVal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.filesFullPathLists.Count != 0)
            {
                string text = string.Empty;
                foreach (string str2 in this.filesFullPathLists)
                {
                    text = text + str2 + "\n";
                }
                MessageBox.Show(text, "Information");
            }
        }

        private void fileAnalysisClearAvailableListBtn_Click(object sender, EventArgs e)
        {
            this.fileAnalysisAvailableFilesListVal.Items.Clear();
            this.filesFullPathLists.Clear();
        }

        private void fileAnalysisClosing()
        {
            m_SChildform = null;
            this.saveNExit();
        }

        private void fileAnalysisDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.autoTestDirVal.Text = dialog.SelectedPath;
                foreach (string str in Directory.GetFiles(dialog.SelectedPath))
                {
                    if ((str.EndsWith(".gp2") || str.EndsWith(".gps")) || (str.EndsWith(".gpx") || str.EndsWith(".bin")))
                    {
                        this.addAvailableFiles(str);
                    }
                }
            }
            this.filesArray = this.filesFullPathLists.ToArray();
        }

        private void fileAnalysisExitBtn_Click(object sender, EventArgs e)
        {
            this._abort = false;
            this._processingStatus = false;
            base.Close();
        }

        private void fileAnalysisRefreshBtn_Click(object sender, EventArgs e)
        {
            this.updateAvailableFiles();
        }

        private void fileAnalysisRemoveAllBtn_Click(object sender, EventArgs e)
        {
            this.fileAnalysisToProcessFilesListVal.Items.Clear();
            this.toRunList.Clear();
            fileIdx = 0;
            this.updateFiles2ProcessCnt();
        }

        private void fileAnalysisRemoveAllCategoryBtn_Click(object sender, EventArgs e)
        {
            this.fileAnalysisToRunCategoryList.Items.Clear();
            this.toAnalysisCategoryList.Clear();
        }

        private void fileAnalysisRemoveBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAnalysisToProcessFilesListVal.SelectedIndex;
            string[] strArray = this.toRunList.ToArray();
            if (selectedIndex >= 0)
            {
                this.fileAnalysisToProcessFilesListVal.Items.Remove(this.fileAnalysisToProcessFilesListVal.SelectedItem);
                this.toRunList.Remove(strArray[selectedIndex]);
                if ((fileIdx > 0) && (fileIdx > selectedIndex))
                {
                    fileIdx--;
                }
                this.updateFiles2ProcessCnt();
            }
        }

        private void fileAnalysisRemoveCategoryBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAnalysisToRunCategoryList.SelectedIndex;
            string[] strArray = this.toAnalysisCategoryList.ToArray();
            if (selectedIndex >= 0)
            {
                this.fileAnalysisToRunCategoryList.Items.Remove(this.fileAnalysisToRunCategoryList.SelectedItem);
                this.toAnalysisCategoryList.Remove(strArray[selectedIndex]);
            }
        }

        private void fileAnalysisRunBtn_Click(object sender, EventArgs e)
        {
            this._abort = false;
            if (!this._processingStatus)
            {
                this._processingStatus = false;
                this._processesdFilesCount = 0;
                this.processedFilesLabel.Text = string.Format("Files Processed: 0", new object[0]);
                this.progressBar1.Value = 0;
                Thread.Sleep(100);
                try
                {
                    this._processingStatus = true;
                    Thread thread = new Thread(new ThreadStart(this.AnalyzeFiles));
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void fileAnalysisToProcessedFilesListVal_DoubleClick(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAnalysisToProcessFilesListVal.SelectedIndex;
            string[] strArray = this.toRunList.ToArray();
            if (selectedIndex >= 0)
            {
                this.fileAnalysisToProcessFilesListVal.Items.Remove(this.fileAnalysisToProcessFilesListVal.SelectedItem);
                this.toRunList.Remove(strArray[selectedIndex]);
                if ((fileIdx > 0) && (fileIdx > selectedIndex))
                {
                    fileIdx--;
                }
                this.updateFiles2ProcessCnt();
            }
        }

        private string Float2AsciiPad(float FValue, int left, int right)
        {
            char[] chArray = new char[20];
            int index = 0x13;
            int num4 = index - right;
            int num5 = num4 - left;
            StringBuilder builder = new StringBuilder();
            long num = (long) (FValue * this.Power10[right + 1]);
            if (num < 0L)
            {
                builder.Append('-');
                num = -num;
                num5++;
            }
            chArray[num4] = '.';
            num /= 10L;
            while (index >= num5)
            {
                if (index != num4)
                {
                    if (num != 0L)
                    {
                        long num2 = num / 10L;
                        char ch = (char) ((ushort) (num - (10L * num2)));
                        chArray[index] = (char) ('0' + ch);
                        num = num2;
                    }
                    else
                    {
                        chArray[index] = '0';
                    }
                }
                index--;
            }
            for (int i = 0; i < chArray.Length; i++)
            {
                builder.Append(chArray[i]);
            }
            return builder.ToString().Replace("\0", "");
        }

        private void frmFileAnalysis_Load(object sender, EventArgs e)
        {
            if (this._fileType == ".gpx")
            {
                this.includeDateTimeChkBox.Enabled = true;
                this.includeDateTimeChkBox.Visible = true;
            }
            else if (this._fileType == ".gps")
            {
                this.includeDateTimeChkBox.Enabled = false;
                this.includeDateTimeChkBox.Visible = false;
            }
            else if (this._fileType == ".bin")
            {
                this.includeDateTimeChkBox.Enabled = false;
                this.includeDateTimeChkBox.Visible = false;
            }
            else
            {
                this.includeDateTimeChkBox.Enabled = true;
                this.includeDateTimeChkBox.Visible = true;
            }
            try
            {
                IniHelper helper = new IniHelper(this.fileAnalysisIniPath);
                foreach (string str2 in helper.IniReadValue("SETUP", "AVAILABLE_CATEGORY").Split(new char[] { ',' }))
                {
                    this.availableCategoryList.Add(str2);
                    this.fileAnalysisCategoryList.Items.Add(str2);
                }
            }
            catch
            {
            }
        }

        private void getCategoryConfig()
        {
            foreach (string str in this.toAnalysisCategoryList)
            {
                string str2 = string.Empty;
                str2 = this.autoIniIntf.GetIniFileString(str, "SEQUENCE", "");
                str2 = this.autoIniIntf.GetIniFileString(str, "MATCH_MSGS", "");
                if (str2 != string.Empty)
                {
                    foreach (string str3 in str2.Split(new char[] { '|' }))
                    {
                        if (!this._analyzedMsgsList.Contains(str3))
                        {
                            this._analyzedMsgsList.Add(str3);
                        }
                    }
                }
                str2 = this.autoIniIntf.GetIniFileString(str, "MSG_NAMES", "");
                if (str2 != string.Empty)
                {
                    foreach (string str4 in str2.Split(new char[] { ',' }))
                    {
                        if (!this._analyzedMsgNamesList.Contains(str4))
                        {
                            this._analyzedMsgNamesList.Add(str4);
                        }
                    }
                }
            }
        }

        public static frmFileAnalysis GetChildInstance(string fileType)
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmFileAnalysis(fileType);
            }
            return m_SChildform;
        }

        private string GPS_NMEA_OutputGGA(string[] msgArray)
        {
            if (msgArray.Length <= 0)
            {
                return string.Empty;
            }
            double num = 0.0;
            short num2 = 0;
            double num3 = 0.0;
            double num4 = 0.0;
            short num5 = 0;
            double num6 = 0.0;
            int num7 = 0;
            string str = "";
            string str2 = "";
            try
            {
                int num8 = Convert.ToInt32(msgArray[2]);
                if ((num8 & 7) == 0)
                {
                    num7 = 0;
                }
                else if ((num8 & 7) == 7)
                {
                    num7 = 6;
                }
                else if ((num8 & 0x80) == 0x80)
                {
                    num7 = 2;
                }
                else
                {
                    num7 = 1;
                }
                num = Convert.ToDouble(msgArray[12]) * 1E-07;
                num4 = Convert.ToDouble(msgArray[13]) * 1E-07;
                num2 = (short) num;
                num3 = Math.Abs((double) ((num - num2) * 60.0));
                num5 = (short) num4;
                num6 = Math.Abs((double) ((num4 - num5) * 60.0));
                str = this.Float2AsciiPad((float) num3, 2, 4);
                str2 = this.Float2AsciiPad((float) num6, 2, 4);
                string inputString = string.Format("$GPGGA,{0:00}{1:00}{2:00.000},{3:00}{4},{5},{6:000}{7},{8},{9},{10},{11:F1},{12:F1},M,,,,", new object[] { Convert.ToByte(msgArray[8]), Convert.ToByte(msgArray[9]), Convert.ToDouble(msgArray[10]) * 0.001, Math.Abs(num2), str.ToString(), (num >= 0.0) ? 'N' : 'S', Math.Abs(num5), str2.ToString(), (num4 < 0.0) ? 'W' : 'E', num7, Convert.ToByte(msgArray[0x21]).ToString().PadLeft(2, '0'), Convert.ToDouble(msgArray[0x22]) * 0.2, Convert.ToDouble(msgArray[15]) * 0.01 });
                return this.AddCheckSum(inputString);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GPS_NMEA_OutputGLL(string[] msgArray)
        {
            double num = 0.0;
            short num2 = 0;
            double num3 = 0.0;
            double num4 = 0.0;
            short num5 = 0;
            double num6 = 0.0;
            string str = "";
            string str2 = "";
            try
            {
                char ch;
                char ch2;
                int num7 = Convert.ToInt32(msgArray[2]);
                if ((num7 & 7) == 0)
                {
                    ch = 'N';
                    ch2 = 'V';
                }
                else if ((num7 & 7) == 7)
                {
                    ch = 'E';
                    ch2 = 'A';
                }
                else if ((num7 & 0x80) == 0x80)
                {
                    ch = 'D';
                    ch2 = 'A';
                }
                else
                {
                    ch = 'A';
                    ch2 = 'A';
                }
                num = Convert.ToDouble(msgArray[12]) * 1E-07;
                num4 = Convert.ToDouble(msgArray[13]) * 1E-07;
                num2 = (short) num;
                num3 = Math.Abs((double) ((num - num2) * 60.0));
                num5 = (short) num4;
                num6 = Math.Abs((double) ((num4 - num5) * 60.0));
                str = this.Float2AsciiPad((float) num3, 2, 4);
                str2 = this.Float2AsciiPad((float) num6, 2, 4);
                string inputString = string.Format("$GPGLL,{0:00}{1},{2},{3:000}{4},{5},{6:00}{7:00}{8:00.000},{9},{10}", new object[] { Math.Abs(num2), str.ToString(), (num >= 0.0) ? 'N' : 'S', Math.Abs(num5), str2.ToString(), (num4 < 0.0) ? 'W' : 'E', Convert.ToByte(msgArray[8]), Convert.ToByte(msgArray[9]), Convert.ToDouble(msgArray[10]) * 0.001, ch2, ch });
                return this.AddCheckSum(inputString);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GPS_NMEA_OutputGSA(string[] msgArray)
        {
            try
            {
                int num;
                int num2;
                switch ((Convert.ToInt32(msgArray[2]) & 7))
                {
                    case 4:
                    case 6:
                        num2 = 3;
                        break;

                    case 0:
                        num2 = 1;
                        break;

                    default:
                        num2 = 2;
                        break;
                }
                uint num5 = Convert.ToUInt32(msgArray[11]);
                StringBuilder builder = new StringBuilder();
                for (num = 0; num < 0x20; num++)
                {
                    uint num6 = ((uint) 1) << num;
                    if ((num5 & num6) == num6)
                    {
                        builder.Append(string.Format("{0:00},", num + 1));
                    }
                }
                int num7 = Convert.ToInt32(msgArray[0x21]);
                for (num = 0; num < (12 - num7); num++)
                {
                    builder.Append(",");
                }
                string inputString = string.Format("$GPGSA,A,{0:0},{1},{2:F1},", num2, builder.ToString(), Convert.ToDouble(msgArray[0x22]) * 0.2);
                return this.AddCheckSum(inputString);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GPS_NMEA_OutputGSV(string[] msgArray)
        {
            double num7 = 0.0;
            int num2 = 0;
            try
            {
                int num8 = Convert.ToInt32(msgArray[3]);
                int index = 4;
                int num4 = 0;
                for (index = 4; num4 < num8; index += 14)
                {
                    if (Convert.ToInt32(msgArray[index]) != 0)
                    {
                        num2++;
                    }
                    num4++;
                }
                int num = (num2 + 3) / 4;
                num4 = 0;
                index = 4;
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                for (int i = 1; i <= num; i++)
                {
                    builder2.Append(string.Format("$GPGSV,{0:D},{1:D},{2:D2}", num, i, num2));
                    int num6 = 0;
                    while ((num6 < 4) && (num4 < 12))
                    {
                        if (num4 < 12)
                        {
                            num7 = 0.0;
                            int num10 = 0;
                            for (int j = 0; j < 10; j++)
                            {
                                int num11 = Convert.ToInt32(msgArray[(index + 4) + j]);
                                if (num11 > 0)
                                {
                                    num10++;
                                    num7 += num11;
                                }
                            }
                            num7 /= (num10 > 0) ? ((double) num10) : 1.0;
                            int num12 = Convert.ToInt32(msgArray[index]);
                            double num13 = Convert.ToDouble(msgArray[index + 2]);
                            double num14 = Convert.ToDouble(msgArray[index + 1]);
                            if (num12 != 0)
                            {
                                builder2.Append(string.Format(",{0:00},{1:00},{2:000},{3:00}", new object[] { (num12 >= 120) ? (num12 - 0x57) : num12, num13, num14, num7 }));
                                num6++;
                            }
                            num4++;
                            index += 14;
                        }
                    }
                    builder.Append(this.AddCheckSum(builder2.ToString()));
                    builder2.Remove(0, builder2.Length);
                }
                return builder.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GPS_NMEA_OutputRMC(string[] msgArray)
        {
            double num = 0.0;
            short num2 = 0;
            double num3 = 0.0;
            double num4 = 0.0;
            short num5 = 0;
            double num6 = 0.0;
            string str = "";
            string str2 = "";
            try
            {
                char ch;
                char ch2;
                int num8 = Convert.ToInt32(msgArray[2]);
                if ((num8 & 7) == 0)
                {
                    ch = 'N';
                    ch2 = 'V';
                }
                else if ((num8 & 7) == 7)
                {
                    ch = 'E';
                    ch2 = 'A';
                }
                else if ((num8 & 0x80) == 0x80)
                {
                    ch = 'D';
                    ch2 = 'A';
                }
                else
                {
                    ch = 'A';
                    ch2 = 'A';
                }
                num = Convert.ToDouble(msgArray[12]) * 1E-07;
                num4 = Convert.ToDouble(msgArray[13]) * 1E-07;
                num2 = (short) num;
                num3 = Math.Abs((double) ((num - num2) * 60.0));
                num5 = (short) num4;
                num6 = Math.Abs((double) ((num4 - num5) * 60.0));
                str = this.Float2AsciiPad((float) num3, 2, 4);
                str2 = this.Float2AsciiPad((float) num6, 2, 4);
                double num7 = ((Convert.ToDouble(msgArray[0x11]) * 0.01) * 3600.0) / 1851.9648000000002;
                long result = 0L;
                Math.DivRem(Convert.ToInt64(msgArray[5]), 100L, out result);
                string inputString = string.Format("$GPRMC,{0:00}{1:00}{2:00.000},{3},{4:D2}{5},{6},{7:D3}{8},{9},{10:F1},{11:F1},{12:00}{13:00}{14:00},,,{15}", new object[] { Convert.ToByte(msgArray[8]), Convert.ToByte(msgArray[9]), Convert.ToDouble(msgArray[10]) * 0.001, ch2, Math.Abs(num2), str.ToString(), (num >= 0.0) ? 'N' : 'S', Math.Abs(num5), str2.ToString(), (num4 < 0.0) ? 'W' : 'E', num7, Convert.ToDouble(msgArray[0x12]) * 0.01, Convert.ToByte(msgArray[7]), Convert.ToByte(msgArray[6]), Convert.ToByte(result), ch });
                return this.AddCheckSum(inputString);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GPS_NMEA_OutputVTG(string[] msgArray)
        {
            double num = 0.0;
            double num2 = 0.0;
            try
            {
                char ch;
                int num3 = Convert.ToInt32(msgArray[2]);
                if ((num3 & 7) == 0)
                {
                    ch = 'N';
                }
                else if ((num3 & 7) == 7)
                {
                    ch = 'E';
                }
                else if ((num3 & 0x80) == 0x80)
                {
                    ch = 'D';
                }
                else
                {
                    ch = 'A';
                }
                double num4 = Convert.ToDouble(msgArray[0x11]);
                num = ((num4 * 0.01) * 3600.0) / 1851.9648000000002;
                num2 = ((num4 * 0.01) * 3600.0) / 1000.0;
                string inputString = string.Format("$GPVTG,{0:F1},T,,M,{1:F1},N,{2:F1},K,{3}", new object[] { Convert.ToDouble(msgArray[0x12]) * 0.01, num, num2, ch });
                return this.AddCheckSum(inputString);
            }
            catch
            {
                return string.Empty;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmFileAnalysis));
            this.frmFileConversionFilesCntLabel = new Label();
            this.autoTestRefreshBtn = new Button();
            this.autoTestClearAvailableListBtn = new Button();
            this.automationTestAbortBtn = new Button();
            this.autoTestRunBtn = new Button();
            this.autoTestRemoveAllBtn = new Button();
            this.autoTestRemoveBtn = new Button();
            this.autoTestAddBtn = new Button();
            this.autoTestAddAllBtn = new Button();
            this.autoTestExitBtn = new Button();
            this.fileAnalysisAvailableFilesListVal = new ListBox();
            this.fileAnalysisToProcessFilesListVal = new ListBox();
            this.autoTestDirBrowser = new Button();
            this.autoTestRunScriptsLabel = new Label();
            this.autoTestAvailableScriptsLabel = new Label();
            this.autoTestFilePathLabel = new Label();
            this.autoTestDirVal = new TextBox();
            this.includeDateTimeChkBox = new CheckBox();
            this.progressBar1 = new ProgressBar();
            this.fileProcessedStatusLabel = new Label();
            this.processedFilesLabel = new Label();
            this.fileAnalysisRemoveAllCategoryBtn = new Button();
            this.fileAnalysisRemoveCategoryBtn = new Button();
            this.fileAnalysisAddCategoryBtn = new Button();
            this.fileAnalysisAddAllCategoryBtn = new Button();
            this.fileAnalysisCategoryList = new ListBox();
            this.fileAnalysisToRunCategoryList = new ListBox();
            this.label1 = new Label();
            base.SuspendLayout();
            this.frmFileConversionFilesCntLabel.AutoSize = true;
            this.frmFileConversionFilesCntLabel.Location = new Point(0x215, 0x54);
            this.frmFileConversionFilesCntLabel.Name = "frmFileConversionFilesCntLabel";
            this.frmFileConversionFilesCntLabel.Size = new Size(13, 13);
            this.frmFileConversionFilesCntLabel.TabIndex = 0x26;
            this.frmFileConversionFilesCntLabel.Text = "0";
            this.autoTestRefreshBtn.Location = new Point(0x8e, 0x51);
            this.autoTestRefreshBtn.Name = "autoTestRefreshBtn";
            this.autoTestRefreshBtn.Size = new Size(0x4b, 0x17);
            this.autoTestRefreshBtn.TabIndex = 0x1c;
            this.autoTestRefreshBtn.Text = "Re&fresh";
            this.autoTestRefreshBtn.UseVisualStyleBackColor = true;
            this.autoTestRefreshBtn.Click += new EventHandler(this.fileAnalysisRefreshBtn_Click);
            this.autoTestClearAvailableListBtn.Location = new Point(0xe9, 0x51);
            this.autoTestClearAvailableListBtn.Name = "autoTestClearAvailableListBtn";
            this.autoTestClearAvailableListBtn.Size = new Size(0x4b, 0x17);
            this.autoTestClearAvailableListBtn.TabIndex = 0x1d;
            this.autoTestClearAvailableListBtn.Text = "&Clear";
            this.autoTestClearAvailableListBtn.UseVisualStyleBackColor = true;
            this.autoTestClearAvailableListBtn.Click += new EventHandler(this.fileAnalysisClearAvailableListBtn_Click);
            this.automationTestAbortBtn.Location = new Point(0x150, 0x1db);
            this.automationTestAbortBtn.Name = "automationTestAbortBtn";
            this.automationTestAbortBtn.Size = new Size(0x4b, 0x17);
            this.automationTestAbortBtn.TabIndex = 0x2c;
            this.automationTestAbortBtn.Text = "A&bort";
            this.automationTestAbortBtn.UseVisualStyleBackColor = true;
            this.automationTestAbortBtn.Click += new EventHandler(this.fileAnalysisAbortBtn_Click);
            this.autoTestRunBtn.Location = new Point(0xea, 0x1db);
            this.autoTestRunBtn.Name = "autoTestRunBtn";
            this.autoTestRunBtn.Size = new Size(0x4b, 0x17);
            this.autoTestRunBtn.TabIndex = 0x2b;
            this.autoTestRunBtn.Text = "&Start";
            this.autoTestRunBtn.UseVisualStyleBackColor = true;
            this.autoTestRunBtn.Click += new EventHandler(this.fileAnalysisRunBtn_Click);
            this.autoTestRemoveAllBtn.Location = new Point(0x144, 0xca);
            this.autoTestRemoveAllBtn.Name = "autoTestRemoveAllBtn";
            this.autoTestRemoveAllBtn.Size = new Size(0x63, 0x17);
            this.autoTestRemoveAllBtn.TabIndex = 0x22;
            this.autoTestRemoveAllBtn.Text = "Remove All <<";
            this.autoTestRemoveAllBtn.UseVisualStyleBackColor = true;
            this.autoTestRemoveAllBtn.Click += new EventHandler(this.fileAnalysisRemoveAllBtn_Click);
            this.autoTestRemoveBtn.Location = new Point(0x144, 0xad);
            this.autoTestRemoveBtn.Name = "autoTestRemoveBtn";
            this.autoTestRemoveBtn.Size = new Size(0x63, 0x17);
            this.autoTestRemoveBtn.TabIndex = 0x21;
            this.autoTestRemoveBtn.Text = "Remove <";
            this.autoTestRemoveBtn.UseVisualStyleBackColor = true;
            this.autoTestRemoveBtn.Click += new EventHandler(this.fileAnalysisRemoveBtn_Click);
            this.autoTestAddBtn.Location = new Point(0x144, 0x73);
            this.autoTestAddBtn.Name = "autoTestAddBtn";
            this.autoTestAddBtn.Size = new Size(0x63, 0x17);
            this.autoTestAddBtn.TabIndex = 0x1f;
            this.autoTestAddBtn.Text = "Add >";
            this.autoTestAddBtn.UseVisualStyleBackColor = true;
            this.autoTestAddBtn.Click += new EventHandler(this.fileAnalysisAddBtn_Click);
            this.autoTestAddAllBtn.Location = new Point(0x144, 0x90);
            this.autoTestAddAllBtn.Name = "autoTestAddAllBtn";
            this.autoTestAddAllBtn.Size = new Size(0x63, 0x17);
            this.autoTestAddAllBtn.TabIndex = 0x20;
            this.autoTestAddAllBtn.Text = "Add All >>";
            this.autoTestAddAllBtn.UseVisualStyleBackColor = true;
            this.autoTestAddAllBtn.Click += new EventHandler(this.fileAnalysisAddAllBtn_Click);
            this.autoTestExitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.autoTestExitBtn.Location = new Point(0x1b6, 0x1db);
            this.autoTestExitBtn.Name = "autoTestExitBtn";
            this.autoTestExitBtn.Size = new Size(0x4b, 0x17);
            this.autoTestExitBtn.TabIndex = 0x2d;
            this.autoTestExitBtn.Text = "E&xit";
            this.autoTestExitBtn.UseVisualStyleBackColor = true;
            this.autoTestExitBtn.Click += new EventHandler(this.fileAnalysisExitBtn_Click);
            this.fileAnalysisAvailableFilesListVal.FormattingEnabled = true;
            this.fileAnalysisAvailableFilesListVal.HorizontalScrollbar = true;
            this.fileAnalysisAvailableFilesListVal.Location = new Point(0x13, 110);
            this.fileAnalysisAvailableFilesListVal.Name = "fileAnalysisAvailableFilesListVal";
            this.fileAnalysisAvailableFilesListVal.ScrollAlwaysVisible = true;
            this.fileAnalysisAvailableFilesListVal.Size = new Size(290, 0x79);
            this.fileAnalysisAvailableFilesListVal.TabIndex = 30;
            this.fileAnalysisAvailableFilesListVal.DoubleClick += new EventHandler(this.fileAnalysisAvailableFilesListVal_DoubleClick);
            this.fileAnalysisAvailableFilesListVal.KeyPress += new KeyPressEventHandler(this.fileAnalysisAvailableFilesListVal_KeyPress);
            this.fileAnalysisToProcessFilesListVal.FormattingEnabled = true;
            this.fileAnalysisToProcessFilesListVal.HorizontalScrollbar = true;
            this.fileAnalysisToProcessFilesListVal.Location = new Point(0x1b5, 110);
            this.fileAnalysisToProcessFilesListVal.Name = "fileAnalysisToProcessFilesListVal";
            this.fileAnalysisToProcessFilesListVal.ScrollAlwaysVisible = true;
            this.fileAnalysisToProcessFilesListVal.Size = new Size(0x127, 0x79);
            this.fileAnalysisToProcessFilesListVal.TabIndex = 0x27;
            this.fileAnalysisToProcessFilesListVal.DoubleClick += new EventHandler(this.fileAnalysisToProcessedFilesListVal_DoubleClick);
            this.autoTestDirBrowser.Location = new Point(0x2b7, 30);
            this.autoTestDirBrowser.Name = "autoTestDirBrowser";
            this.autoTestDirBrowser.Size = new Size(0x24, 0x15);
            this.autoTestDirBrowser.TabIndex = 0x1a;
            this.autoTestDirBrowser.Text = "&...";
            this.autoTestDirBrowser.UseVisualStyleBackColor = true;
            this.autoTestDirBrowser.Click += new EventHandler(this.fileAnalysisDirBrowser_Click);
            this.autoTestRunScriptsLabel.AutoSize = true;
            this.autoTestRunScriptsLabel.Location = new Point(0x1b5, 0x54);
            this.autoTestRunScriptsLabel.Name = "autoTestRunScriptsLabel";
            this.autoTestRunScriptsLabel.Size = new Size(0x58, 13);
            this.autoTestRunScriptsLabel.TabIndex = 0x25;
            this.autoTestRunScriptsLabel.Text = "Files To Process:";
            this.autoTestAvailableScriptsLabel.AutoSize = true;
            this.autoTestAvailableScriptsLabel.Location = new Point(0x13, 0x56);
            this.autoTestAvailableScriptsLabel.Name = "autoTestAvailableScriptsLabel";
            this.autoTestAvailableScriptsLabel.Size = new Size(0x4a, 13);
            this.autoTestAvailableScriptsLabel.TabIndex = 0x1b;
            this.autoTestAvailableScriptsLabel.Text = "Available Files";
            this.autoTestFilePathLabel.AutoSize = true;
            this.autoTestFilePathLabel.Location = new Point(0x13, 14);
            this.autoTestFilePathLabel.Name = "autoTestFilePathLabel";
            this.autoTestFilePathLabel.Size = new Size(0x44, 13);
            this.autoTestFilePathLabel.TabIndex = 0x18;
            this.autoTestFilePathLabel.Text = "File Directory";
            this.autoTestDirVal.Location = new Point(0x13, 0x1f);
            this.autoTestDirVal.Name = "autoTestDirVal";
            this.autoTestDirVal.Size = new Size(670, 20);
            this.autoTestDirVal.TabIndex = 0x19;
            this.includeDateTimeChkBox.AutoSize = true;
            this.includeDateTimeChkBox.Location = new Point(0x13, 0x37);
            this.includeDateTimeChkBox.Name = "includeDateTimeChkBox";
            this.includeDateTimeChkBox.Size = new Size(0x76, 0x11);
            this.includeDateTimeChkBox.TabIndex = 0x2e;
            this.includeDateTimeChkBox.Text = "include date string?";
            this.includeDateTimeChkBox.UseVisualStyleBackColor = true;
            this.progressBar1.Location = new Point(0x13, 0x1b7);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0x2c9, 13);
            this.progressBar1.TabIndex = 0x30;
            this.fileProcessedStatusLabel.AutoSize = true;
            this.fileProcessedStatusLabel.Location = new Point(0x13, 0x1d1);
            this.fileProcessedStatusLabel.Name = "fileProcessedStatusLabel";
            this.fileProcessedStatusLabel.Size = new Size(0x3b, 13);
            this.fileProcessedStatusLabel.TabIndex = 50;
            this.fileProcessedStatusLabel.Text = "Status: idle";
            this.processedFilesLabel.AutoSize = true;
            this.processedFilesLabel.Location = new Point(0x25d, 0x54);
            this.processedFilesLabel.Name = "processedFilesLabel";
            this.processedFilesLabel.Size = new Size(0x54, 13);
            this.processedFilesLabel.TabIndex = 0x2f;
            this.processedFilesLabel.Text = "Files Processed:";
            this.fileAnalysisRemoveAllCategoryBtn.Location = new Point(0x144, 0x189);
            this.fileAnalysisRemoveAllCategoryBtn.Name = "fileAnalysisRemoveAllCategoryBtn";
            this.fileAnalysisRemoveAllCategoryBtn.Size = new Size(0x63, 0x17);
            this.fileAnalysisRemoveAllCategoryBtn.TabIndex = 0x37;
            this.fileAnalysisRemoveAllCategoryBtn.Text = "Remove All <<";
            this.fileAnalysisRemoveAllCategoryBtn.UseVisualStyleBackColor = true;
            this.fileAnalysisRemoveAllCategoryBtn.Click += new EventHandler(this.fileAnalysisRemoveAllCategoryBtn_Click);
            this.fileAnalysisRemoveCategoryBtn.Location = new Point(0x144, 0x16c);
            this.fileAnalysisRemoveCategoryBtn.Name = "fileAnalysisRemoveCategoryBtn";
            this.fileAnalysisRemoveCategoryBtn.Size = new Size(0x63, 0x17);
            this.fileAnalysisRemoveCategoryBtn.TabIndex = 0x36;
            this.fileAnalysisRemoveCategoryBtn.Text = "Remove <";
            this.fileAnalysisRemoveCategoryBtn.UseVisualStyleBackColor = true;
            this.fileAnalysisRemoveCategoryBtn.Click += new EventHandler(this.fileAnalysisRemoveCategoryBtn_Click);
            this.fileAnalysisAddCategoryBtn.Location = new Point(0x144, 0x132);
            this.fileAnalysisAddCategoryBtn.Name = "fileAnalysisAddCategoryBtn";
            this.fileAnalysisAddCategoryBtn.Size = new Size(0x63, 0x17);
            this.fileAnalysisAddCategoryBtn.TabIndex = 0x34;
            this.fileAnalysisAddCategoryBtn.Text = "Add >";
            this.fileAnalysisAddCategoryBtn.UseVisualStyleBackColor = true;
            this.fileAnalysisAddCategoryBtn.Click += new EventHandler(this.fileAnalysisAddCategoryBtn_Click);
            this.fileAnalysisAddAllCategoryBtn.Location = new Point(0x144, 0x14f);
            this.fileAnalysisAddAllCategoryBtn.Name = "fileAnalysisAddAllCategoryBtn";
            this.fileAnalysisAddAllCategoryBtn.Size = new Size(0x63, 0x17);
            this.fileAnalysisAddAllCategoryBtn.TabIndex = 0x35;
            this.fileAnalysisAddAllCategoryBtn.Text = "Add All >>";
            this.fileAnalysisAddAllCategoryBtn.UseVisualStyleBackColor = true;
            this.fileAnalysisAddAllCategoryBtn.Click += new EventHandler(this.fileAnalysisAddAllCategoryBtn_Click);
            this.fileAnalysisCategoryList.FormattingEnabled = true;
            this.fileAnalysisCategoryList.HorizontalScrollbar = true;
            this.fileAnalysisCategoryList.Location = new Point(0x13, 0x12d);
            this.fileAnalysisCategoryList.Name = "fileAnalysisCategoryList";
            this.fileAnalysisCategoryList.ScrollAlwaysVisible = true;
            this.fileAnalysisCategoryList.Size = new Size(290, 0x79);
            this.fileAnalysisCategoryList.TabIndex = 0x33;
            this.fileAnalysisToRunCategoryList.FormattingEnabled = true;
            this.fileAnalysisToRunCategoryList.HorizontalScrollbar = true;
            this.fileAnalysisToRunCategoryList.Location = new Point(0x1b5, 0x12d);
            this.fileAnalysisToRunCategoryList.Name = "fileAnalysisToRunCategoryList";
            this.fileAnalysisToRunCategoryList.ScrollAlwaysVisible = true;
            this.fileAnalysisToRunCategoryList.Size = new Size(0x127, 0x79);
            this.fileAnalysisToRunCategoryList.TabIndex = 0x38;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x120, 250);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xb0, 0x18);
            this.label1.TabIndex = 0x39;
            this.label1.Text = "Analysis Category";
            base.AcceptButton = this.autoTestRunBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.autoTestExitBtn;
            base.ClientSize = new Size(0x2f0, 0x20c);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.fileAnalysisRemoveAllCategoryBtn);
            base.Controls.Add(this.fileAnalysisRemoveCategoryBtn);
            base.Controls.Add(this.fileAnalysisAddCategoryBtn);
            base.Controls.Add(this.fileAnalysisAddAllCategoryBtn);
            base.Controls.Add(this.fileAnalysisCategoryList);
            base.Controls.Add(this.fileAnalysisToRunCategoryList);
            base.Controls.Add(this.fileProcessedStatusLabel);
            base.Controls.Add(this.progressBar1);
            base.Controls.Add(this.processedFilesLabel);
            base.Controls.Add(this.includeDateTimeChkBox);
            base.Controls.Add(this.frmFileConversionFilesCntLabel);
            base.Controls.Add(this.autoTestRefreshBtn);
            base.Controls.Add(this.autoTestClearAvailableListBtn);
            base.Controls.Add(this.automationTestAbortBtn);
            base.Controls.Add(this.autoTestRunBtn);
            base.Controls.Add(this.autoTestRemoveAllBtn);
            base.Controls.Add(this.autoTestRemoveBtn);
            base.Controls.Add(this.autoTestAddBtn);
            base.Controls.Add(this.autoTestAddAllBtn);
            base.Controls.Add(this.autoTestExitBtn);
            base.Controls.Add(this.fileAnalysisAvailableFilesListVal);
            base.Controls.Add(this.fileAnalysisToProcessFilesListVal);
            base.Controls.Add(this.autoTestDirBrowser);
            base.Controls.Add(this.autoTestRunScriptsLabel);
            base.Controls.Add(this.autoTestAvailableScriptsLabel);
            base.Controls.Add(this.autoTestFilePathLabel);
            base.Controls.Add(this.autoTestDirVal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmFileAnalysis";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "File Analysis";
            base.Load += new EventHandler(this.frmFileAnalysis_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private int msg69ToMsg4AndMsg41Format(string inputString, ref string[] msg41Array)
        {
            string[] strArray = inputString.Split(new char[] { ',' });
            int num = 0;
            try
            {
                Convert.ToInt32(strArray[2]);
                num = Convert.ToInt32(strArray[3]);
                Convert.ToInt32(strArray[4]);
            }
            catch
            {
            }
            if (num == 0)
            {
                return 1;
            }
            int num2 = 0;
            int num3 = 0;
            int inWeek = 0;
            double inTOW = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            int num8 = 0;
            double num9 = 0.0;
            double num10 = 0.0;
            double num11 = 0.0;
            int inUTCOffset = 0;
            int num13 = 0;
            uint num14 = 0;
            try
            {
                num2 = Convert.ToInt32(strArray[5]);
                num3 = Convert.ToInt32(strArray[6]);
                Convert.ToInt32(strArray[7]);
                inWeek = Convert.ToInt32(strArray[8]);
                inTOW = Convert.ToDouble(strArray[9]) / 1000.0;
                num6 = (Convert.ToDouble(strArray[10]) * 180.0) / 4294967296;
                num7 = (Convert.ToDouble(strArray[11]) * 360.0) / 4294967296;
                num8 = Convert.ToInt32(strArray[12]);
                double num1 = (Convert.ToDouble(strArray[13]) * 180.0) / 256.0;
                num9 = (Convert.ToDouble(strArray[0x10]) * 0.1) - 500.0;
                num10 = Convert.ToDouble(strArray[0x12]) * 0.0625;
                num11 = (Convert.ToDouble(strArray[0x13]) * 360.0) / 65536.0;
                Convert.ToDouble(strArray[20]);
                Convert.ToDouble(strArray[0x15]);
                Convert.ToDouble(strArray[0x16]);
                Convert.ToDouble(strArray[0x17]);
                Convert.ToDouble(strArray[0x18]);
                Convert.ToInt32(strArray[0x19]);
                Convert.ToUInt16(strArray[0x1a]);
                Convert.ToDouble(strArray[0x1c]);
                if ((num8 & 8) == 8)
                {
                    inUTCOffset = Convert.ToInt32(strArray[0x1d]);
                }
                else
                {
                    inUTCOffset = 15;
                }
                num13 = Convert.ToInt32(strArray[30]);
            }
            catch
            {
            }
            if (num2 <= 0)
            {
                return 1;
            }
            msg41Array[0] = "41";
            msg41Array[1] = "0";
            switch ((num3 & 3))
            {
                case 0:
                    msg41Array[2] = "5";
                    break;

                case 1:
                    msg41Array[2] = "6";
                    break;

                default:
                    msg41Array[2] = "0";
                    break;
            }
            msg41Array[3] = strArray[8];
            msg41Array[4] = strArray[9];
            GPSDateTime time = new GPSDateTime();
            time.SetTime(inWeek, inTOW);
            time.SetUTCOffset(inUTCOffset);
            DateTime time2 = time.GetTime();
            msg41Array[5] = time2.Year.ToString();
            msg41Array[6] = time2.Month.ToString();
            msg41Array[7] = time2.Day.ToString();
            msg41Array[8] = time2.Hour.ToString();
            msg41Array[9] = time2.Minute.ToString();
            msg41Array[10] = ((time2.Second * 0x3e8) + time2.Millisecond).ToString();
            msg41Array[12] = string.Format("{0:F0}", num6 * 10000000.0);
            msg41Array[13] = string.Format("{0:F0}", num7 * 10000000.0);
            if ((num8 & 2) == 2)
            {
                msg41Array[14] = string.Format("{0:F0}", num9 * 100.0);
            }
            else
            {
                msg41Array[14] = "0";
            }
            msg41Array[15] = "0";
            msg41Array[0x10] = "0";
            if ((num8 & 4) == 4)
            {
                msg41Array[0x11] = string.Format("{0:F0}", num10 * 100.0);
                msg41Array[0x12] = string.Format("{0:F0}", num11 * 100.0);
            }
            else
            {
                msg41Array[0x11] = "0";
                msg41Array[0x12] = "0";
            }
            for (int i = 0x13; i <= 0x20; i++)
            {
                msg41Array[i] = "0";
            }
            msg41Array[0x21] = num13.ToString();
            msg41Array[0x22] = "0";
            msg41Array[0x23] = "0";
            try
            {
                int[] numArray = new int[num13];
                int[] numArray2 = new int[num13];
                int[] numArray3 = new int[num13];
                if ((num8 & 0x10) == 0x10)
                {
                    int index = 0;
                    int num19 = 0x1f;
                    while (index < num13)
                    {
                        numArray[index] = Convert.ToInt32(strArray[num19++]);
                        numArray2[index] = Convert.ToInt32(strArray[num19++]);
                        numArray3[index] = Convert.ToInt32(strArray[num19++]);
                        num14 |= ((uint) 1) << (numArray[index] - 1);
                        index++;
                    }
                }
                else
                {
                    for (int j = 0; j < num13; j++)
                    {
                        numArray[j] = 0;
                        numArray2[j] = 0;
                        numArray3[j] = 0;
                        num14 = 0;
                    }
                }
            }
            catch
            {
                return 1;
            }
            msg41Array[11] = string.Format("{0}", num14);
            return 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.fileAnalysisClosing();
        }

        private void parseGPSFile()
        {
            LargeFileHandler handler = null;
            EventHandler method = null;
            EventHandler handler3 = null;
            long length = 0L;
            try
            {
                this.getCategoryConfig();
                foreach (string str in this.toRunList)
                {
                    string item = "Unknown";
                    string str3 = "Unknown";
                    if (!File.Exists(str))
                    {
                        this._processesdFilesCount++;
                        if (method == null)
                        {
                            method = delegate {
                                this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                            };
                        }
                        this.processedFilesLabel.BeginInvoke(method);
                    }
                    else
                    {
                        new FileInfo(str);
                        if (str.Length == 0)
                        {
                            this._processesdFilesCount++;
                            if (handler3 == null)
                            {
                                handler3 = delegate {
                                    this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                                };
                            }
                            this.processedFilesLabel.BeginInvoke(handler3);
                        }
                        else
                        {
                            handler = new LargeFileHandler(str);
                            length = handler.Length;
                            for (int i = 0; i < this._analyzedMsgsList.Count; i++)
                            {
                                StreamWriter writer = new StreamWriter(str.Replace(".gps", string.Format("_{0}.ipar", this._analyzedMsgNamesList[i])));
                                this._msgIndexFileList.Add(writer);
                            }
                            try
                            {
                                long num3 = 0L;
                                string str5 = handler[num3];
                                int count = this._analyzedMsgsList.Count;
                                while (str5 != "EOF")
                                {
                                    if (str5.Contains("SW Version"))
                                    {
                                        item = str5.TrimEnd(new char[] { '\r', '\n' });
                                    }
                                    else if (str5.Contains("DUT Name:"))
                                    {
                                        str3 = str5.TrimEnd(new char[] { '\r', '\n' });
                                    }
                                    else
                                    {
                                        for (int j = 0; j < count; j++)
                                        {
                                            if (str5.Contains(this._analyzedMsgsList[j]))
                                            {
                                                this._msgIndexFileList[j].WriteLine(num3);
                                                break;
                                            }
                                        }
                                    }
                                    if (length != 0L)
                                    {
                                        int processPercentage = (int) ((((double) handler.Index) / ((double) length)) * 100.0);
                                        this.progressBar1.BeginInvoke((MethodInvoker)delegate {
                                            this.progressBar1.Value = processPercentage;
                                        });
                                    }
                                    num3 = handler.Index + 1L;
                                    str5 = handler[num3];
                                }
                                this._softwareVersionsList.Add(item);
                                this._rxNamesList.Add(str3);
                            }
                            catch
                            {
                            }
                            if (handler != null)
                            {
                                handler.Close();
                            }
                            foreach (StreamWriter writer2 in this._msgIndexFileList)
                            {
                                if (writer2 != null)
                                {
                                    writer2.Close();
                                }
                            }
                            this._msgIndexFileList.Clear();
                        }
                    }
                }
            }
            catch
            {
                if (handler != null)
                {
                    handler.Close();
                }
                foreach (StreamWriter writer3 in this._msgIndexFileList)
                {
                    if (writer3 != null)
                    {
                        writer3.Close();
                    }
                }
                this._msgIndexFileList.Clear();
            }
        }

        private Hashtable parseResString(string inputStr)
        {
            if (inputStr == string.Empty)
            {
                return null;
            }
            Hashtable hashtable = new Hashtable();
            string[] strArray = inputStr.Split(new char[] { ':' });
            List<string> list = new List<string>();
            if (strArray.Length <= 1)
            {
                return null;
            }
            string key = strArray[0].Trim();
            string[] strArray2 = strArray[1].Split(new char[] { ',' });
            if (strArray2.Length <= 0)
            {
                return null;
            }
            foreach (string str2 in strArray2)
            {
                list.Add(str2.Trim());
            }
            if (!hashtable.Contains(key))
            {
                hashtable.Add(key, list);
            }
            return hashtable;
        }

        private void performCatAnalyze()
        {
            EventHandler method = null;
            EventHandler handler3 = null;
            foreach (string str in this.toAnalysisCategoryList)
            {
                string str2 = string.Empty;
                int num = 1;
                List<Hashtable> list = new List<Hashtable>();
                while (num > 0)
                {
                    string str3 = "Res";
                    try
                    {
                        str2 = this.autoIniIntf.GetIniFileString(str, str3 + num.ToString(), "");
                        if (str2 != string.Empty)
                        {
                            if (str2 == "Done")
                            {
                                break;
                            }
                            Hashtable item = this.parseResString(str2.TrimEnd(new char[] { '\r', '\n' }));
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                        continue;
                    }
                    catch
                    {
                        break;
                    }
                }
                LargeFileHandler handler = null;
                foreach (string str4 in this.toRunList)
                {
                    if (!File.Exists(str4))
                    {
                        this._processesdFilesCount++;
                        if (method == null)
                        {
                            method = delegate {
                                this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                            };
                        }
                        this.processedFilesLabel.BeginInvoke(method);
                    }
                    else
                    {
                        new FileInfo(str4);
                        if (str4.Length == 0)
                        {
                            if (handler3 == null)
                            {
                                handler3 = delegate {
                                    this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                                };
                            }
                            this.processedFilesLabel.BeginInvoke(handler3);
                        }
                        else
                        {
                            handler = new LargeFileHandler(str4);
                            long length = handler.Length;
                            for (int i = 0; i < list.Count; i++)
                            {
                                Hashtable hashtable2 = list[i];
                                foreach (string str5 in hashtable2.Keys)
                                {
                                    string path = str4.Replace(".gps", str5 + ".ipar");
                                    if (File.Exists(path))
                                    {
                                        new StreamReader(path);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private int saveNExit()
        {
            int num = 0;
            IniHelper helper = new IniHelper(this.fileAnalysisIniPath);
            string str = string.Empty;
            foreach (string str2 in this.filesFullPathLists)
            {
                str = str + str2 + ",";
            }
            str = str.TrimEnd(new char[] { ',' });
            if (str.Length != 0)
            {
                str.Replace(" ", "");
            }
            if (File.Exists(this.fileAnalysisIniPath))
            {
                if ((File.GetAttributes(this.fileAnalysisIniPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("{0} File is read only!\nPlease change property and retry", this.fileAnalysisIniPath), "Error");
                    return 1;
                }
                helper.IniWriteValue("SETUP", "AVAILABLE_SCRIPTS", str);
                helper.IniWriteValue("SETUP", "SCRIPTS_DIR", this.autoTestDirVal.Text);
                return num;
            }
            StreamWriter writer = File.CreateText(this.fileAnalysisIniPath);
            writer.WriteLine("[EMAIL]");
            writer.WriteLine("Email.Sender=\"STING@sirf.com\"");
            writer.WriteLine("Email.Copy=\"\"");
            writer.WriteLine("Email.Priority=\"MailPriority.Normal\"");
            writer.WriteLine("Email.Encoding=\"Encoding.ASCII\"");
            writer.WriteLine("Email.Subject=\"SiRFLive Auto Test Report\"");
            writer.WriteLine("Email.Message=\"\nBest Regards\n\n SiRFLive Team\"");
            writer.WriteLine("Email.Smtp=\"192.168.2.254\"");
            writer.WriteLine("[SETUP]");
            writer.WriteLine("SCRIPTS_DIR={0}", this.autoTestDirVal.Text);
            writer.WriteLine("AVAILABLE_SCRIPTS={0}", str);
            writer.Close();
            return num;
        }

        private void timeJumpCheck()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            CommunicationManager manager = new CommunicationManager();
            StreamWriter writer = null;
            StreamWriter writer2 = null;
            StreamWriter writer3 = null;
            StreamReader reader = null;
            StreamWriter writer4 = null;
            StreamReader reader2 = null;
            try
            {
                foreach (string str in this.toRunList)
                {
                    if (!File.Exists(str))
                    {
                        this._processesdFilesCount++;
                        if (method == null)
                        {
                            method = delegate {
                                this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                            };
                        }
                        this.processedFilesLabel.BeginInvoke(method);
                        continue;
                    }
                    string str2 = string.Empty;
                    string path = string.Empty;
                    string str4 = string.Empty;
                    if (str.EndsWith(".gp2"))
                    {
                        path = str.Replace(".gp2", ".gps");
                        this._fileType = ".gpx";
                    }
                    else if (str.EndsWith(".gpx"))
                    {
                        path = str.Replace(".gpx", ".gps");
                        this._fileType = ".gpx";
                    }
                    else if (str.EndsWith(".bin"))
                    {
                        path = str.Replace(".bin", ".gps");
                        str.Replace(".bin", ".gp2");
                        this._fileType = ".bin";
                    }
                    else
                    {
                        path = str;
                        this._fileType = ".gps";
                    }
                    str4 = str.Substring(0, str.Length - 4) + ".csv";
                    reader = new StreamReader(str);
                    FileInfo info = new FileInfo(str);
                    writer4 = new StreamWriter(info.DirectoryName + @".\procSummary.txt", true);
                    if (handler2 == null)
                    {
                        handler2 = delegate {
                            this.progressBar1.Value = 0;
                            this.progressBar1.Maximum = 100;
                            this.progressBar1.Minimum = 0;
                        };
                    }
                    this.progressBar1.BeginInvoke(handler2);
                    long num = 0L;
                    reader = new StreamReader(path);
                    writer3 = new StreamWriter(str4);
                    info = new FileInfo(path);
                    if (handler3 == null)
                    {
                        handler3 = delegate {
                            this.progressBar1.Value = 0;
                            this.progressBar1.Maximum = 100;
                            this.progressBar1.Minimum = 0;
                        };
                    }
                    this.progressBar1.BeginInvoke(handler3);
                    if (handler4 == null)
                    {
                        handler4 = delegate {
                            this.fileProcessedStatusLabel.Text = "Status: processing...";
                        };
                    }
                    this.fileProcessedStatusLabel.BeginInvoke(handler4);
                    uint num2 = 0;
                    uint num3 = 0;
                    double num4 = 0.0;
                    double num5 = 0.0;
                    uint num6 = 0;
                    uint num7 = 0;
                    long num8 = 0L;
                    uint num9 = 0;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        num8 += 1L;
                        num += str2.Length;
                        int percent = (int) ((((double) num) / ((double) info.Length)) * 100.0);
                        if (percent > 100)
                        {
                            percent = 100;
                        }
						this.progressBar1.BeginInvoke((MethodInvoker)delegate
						{
                            this.progressBar1.Value = percent;
                        });
                        str2 = str2.Replace(" ", "");
                        str2.TrimEnd(new char[] { '\n' });
                        str2.TrimEnd(new char[] { '\r' });
                        if (str2 != string.Empty)
                        {
                            string[] strArray = str2.Split(new char[] { ',' });
                            string str6 = string.Empty;
                            if (str2.StartsWith("2,"))
                            {
                                num2 = Convert.ToUInt32(strArray[11]);
                                if (num2 < num3)
                                {
                                    str6 = string.Format("Mesg 2, line number: {0}, {1}, {2}", num8, num2, num3);
                                    num9++;
                                }
                                else if ((num2 - num3) > 200)
                                {
                                    str6 = string.Format("Mesg 2, line number: {0}, {1}, {2}", num8, num2, num3);
                                    num9++;
                                }
                                num3 = num2;
                            }
                            else if (str2.StartsWith("4,"))
                            {
                                num4 = Convert.ToDouble(strArray[2]) * 100.0;
                                if (num4 < num5)
                                {
                                    str6 = string.Format("Mesg 4, line number: {0}, {1}, {2}", num8, num4, num5);
                                    num9++;
                                }
                                else if ((num4 - num5) > 200.0)
                                {
                                    str6 = string.Format("Mesg 4, line number: {0}, {1}, {2}", num8, num4, num5);
                                    num9++;
                                }
                                num5 = num4;
                            }
                            else if (str2.StartsWith("41,"))
                            {
                                num6 = Convert.ToUInt32(strArray[4]);
                                if (num6 < num7)
                                {
                                    str6 = string.Format("Mesg 41, line number: {0}, {1}, {2}", num8, num6, num7);
                                    num9++;
                                }
                                else if ((num6 - num7) > 0x7d0)
                                {
                                    str6 = string.Format("Mesg 41, line number: {0}, {1}, {2}", num8, num6, num7);
                                    num9++;
                                }
                                num7 = num6;
                            }
                            if (str6 != string.Empty)
                            {
                                writer3.WriteLine(str6);
                            }
                            if (this._abort)
                            {
                                break;
                            }
                        }
                    }
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    if (writer3 != null)
                    {
                        writer3.Close();
                    }
                    this._processesdFilesCount++;
                    if (handler5 == null)
                    {
                        handler5 = delegate {
                            this.processedFilesLabel.Text = string.Format("Files Processed: {0}", this._processesdFilesCount);
                        };
                    }
                    this.processedFilesLabel.BeginInvoke(handler5);
                    writer4.WriteLine(string.Format("File {0} has {1} error", str, num9));
                    uint num10 = (num9 > 10) ? 10 : num9;
                    reader2 = new StreamReader(str4);
                    for (int i = 0; i < num10; i++)
                    {
                        writer4.WriteLine(string.Format("     {0}", reader2.ReadLine()));
                    }
                    reader2.Close();
                    writer4.Close();
                }
                if (this._abort)
                {
                    MessageBox.Show("Aborted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                this._processingStatus = false;
            }
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
                if (writer2 != null)
                {
                    writer2.Close();
                }
                if (writer3 != null)
                {
                    writer3.Close();
                }
                if (reader2 != null)
                {
                    reader2.Close();
                }
                if (writer4 != null)
                {
                    writer4.Close();
                }
                this._processingStatus = false;
                this._abort = false;
                MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            manager.Dispose();
            manager = null;
			this.fileProcessedStatusLabel.BeginInvoke((MethodInvoker)delegate
			{
                this.fileProcessedStatusLabel.Text = "Status: idle";
            });
        }

        private void updateAvailableFiles()
        {
            this.fileAnalysisAvailableFilesListVal.Items.Clear();
            this.filesFullPathLists.Clear();
            try
            {
                foreach (string str in Directory.GetFiles(this.autoTestDirVal.Text))
                {
                    if ((str.EndsWith(".gp2") || str.EndsWith(".gpx")) || (str.EndsWith(".gps") || str.EndsWith(".bin")))
                    {
                        this.addAvailableFiles(str);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!");
            }
            this.filesArray = this.filesFullPathLists.ToArray();
        }

        private void updateFiles2ProcessCnt()
        {
            string str = string.Format("{0}", this.fileAnalysisToProcessFilesListVal.Items.Count);
            this.frmFileConversionFilesCntLabel.Text = str;
        }
    }
}

