﻿namespace SiRFLive.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class IniHelper
    {
        private string _iniFilepath = string.Empty;

        public IniHelper(string INIPath)
        {
            this._iniFilepath = INIPath;
        }

        private int checkDup(string str, char delim, string noCntStr)
        {
            int num = 0;
            string[] strArray = str.Split(new char[] { delim });
            List<string> list = new List<string>();
            foreach (string str2 in strArray)
            {
                list.Add(str2);
            }
            list.Sort();
            string[] strArray2 = list.ToArray();
            for (int i = 0; i < strArray2.Length; i++)
            {
                string str3 = strArray2[i];
                for (int j = i + 1; j < strArray2.Length; j++)
                {
                    string str4 = strArray2[j];
                    if (!(str4 == noCntStr) && (str3 == str4))
                    {
                        num++;
                        break;
                    }
                }
            }
            return num;
        }

        private int checkDup(string[] strArray, string str, string noCntStr)
        {
            int num = 0;
            foreach (string str2 in strArray)
            {
                if (!(str2 == noCntStr) && (str2 == str))
                {
                    num++;
                }
            }
            return num;
        }

        public string GetIniFileString(string category, string key, string defaultValue)
        {
            string lpFilename = this._iniFilepath;
            string lpReturnString = new string(' ', 0x400);
            GetPrivateProfileString(category, key, defaultValue, lpReturnString, 0x400, lpFilename);
            char[] separator = new char[1];
            return lpReturnString.ToString().Split(separator)[0];
        }

        public List<string> GetKeys(string category)
        {
            string lpFilename = this._iniFilepath;
            string lpReturnString = new string(' ', 0x8000);
            GetPrivateProfileString(category, null, null, lpReturnString, 0x8000, lpFilename);
            char[] separator = new char[1];
            List<string> list = new List<string>(lpReturnString.Split(separator));
            list.RemoveRange(list.Count - 2, 2);
            return list;
        }

        [DllImport("kernel32.dll", EntryPoint="GetPrivateProfileSectionNamesA")]
        private static extern int GetPrivateProfileSectionNames(byte[] lpszReturnBuffer, int nSize, string lpFileName);
        [DllImport("KERNEL32.DLL", EntryPoint="GetPrivateProfileStringW", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnString, int nSize, string lpFilename);
        public List<string> GetSections()
        {
            string lpFilename = this._iniFilepath;
            string lpReturnString = new string(' ', 0x10000);
            GetPrivateProfileString(null, null, null, lpReturnString, 0x10000, lpFilename);
            char[] separator = new char[1];
            List<string> list = new List<string>(lpReturnString.Split(separator));
            list.RemoveRange(list.Count - 2, 2);
            return list;
        }

        public string IniReadValue(string Section, string Key)
        {
            string lpReturnString = new string(' ', 0x400);
            GetPrivateProfileString(Section, Key, "", lpReturnString, 0x400, this._iniFilepath);
            return lpReturnString.Trim(new char[] { '\r', '\n', '\0', ' ' });
        }

        public int IniSiRFLiveRxSetupErrorCheck(string path)
        {
            int num = 0;
            string text = string.Empty;
            this._iniFilepath = path;
            List<string> list = new List<string>();
            string category = string.Empty;
            string key = string.Empty;
            category = "RF_PLAYBACK";
            key = "PLAY_TIME_LISTS";
            string str = string.Empty;
            str = this.GetIniFileString(category, key, "");
            category = "RF_PLAYBACK";
            key = "PLAYBACK_FILES";
            string str5 = this.GetIniFileString(category, key, "");
            if (((str.Length > 0) || (str5.Length > 0)) && (str.Split(new char[] { ',' }).Length < str5.Split(new char[] { ',' }).Length))
            {
                num++;
                text = text + "# PLAY_TIME_LISTS < # PLAYBACK_FILES\n";
            }
            int length = 0;
            category = "RX_SETUP";
            key = "ACTIVE_PORTS";
            str = this.GetIniFileString(category, key, "");
            string[] strArray = str.Split(new char[] { ',' });
            length = strArray.Length;
            if ((str.Length != 0) && (this.checkDup(str, ',', "-1") > 0))
            {
                num++;
                text = text + "Duplicate ACTIVE_PORTS\n";
            }
            category = "RX_SETUP";
            key = "RX_NAMES";
            str = this.GetIniFileString(category, key, "");
            str.Split(new char[] { ',' });
            if ((str.Length != 0) && (this.checkDup(str, ',', "-1") > 0))
            {
                num++;
                text = text + "Duplicate RX_NAMES\n";
            }
            category = "RX_SETUP";
            key = "REQUIRED_HOSTS";
            str = this.GetIniFileString(category, key, "");
            int num3 = 0;
            if (str.Length > 0)
            {
                string[] strArray2 = str.Split(new char[] { ',' });
                foreach (string str6 in strArray2)
                {
                    if (str6 == "1")
                    {
                        num3++;
                    }
                }
                if (strArray2.Length < length)
                {
                    num++;
                    text = text + "# REQUIRED_HOSTS < # ACTIVE_PORTS\n";
                }
            }
            category = "RX_SETUP";
            key = "REQUIRED_PATCH";
            str = this.GetIniFileString(category, key, "");
            int num4 = 0;
            if (str.Length > 0)
            {
                string[] strArray3 = str.Split(new char[] { ',' });
                foreach (string str7 in strArray3)
                {
                    if (str7 == "1")
                    {
                        num4++;
                    }
                }
                if (strArray3.Length < length)
                {
                    num++;
                    text = text + "# REQUIRED_PATCH < # ACTIVE_PORTS\n";
                }
            }
            category = "RX_SETUP";
            key = "TRACKER_PORTS";
            str = this.GetIniFileString(category, key, "");
            string[] strArray4 = str.Split(new char[] { ',' });
            if (str.Length > 0)
            {
                if (strArray4.Length < length)
                {
                    num++;
                    text = text + "# TRACKER_PORTS < # host ACTIVE_PORTS\n";
                }
                else
                {
                    if (this.checkDup(str, ',', "-1") > 0)
                    {
                        num++;
                        text = text + "Duplicate TRACKER_PORTS\n";
                    }
                    foreach (string str8 in strArray4)
                    {
                        if (this.checkDup(strArray, str8, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate TRACKER_PORTS and ACTIVE_PORTS\n";
                        }
                    }
                }
            }
            category = "RX_SETUP";
            key = "HOST_PORTS";
            str = this.GetIniFileString(category, key, "");
            string[] strArray5 = str.Split(new char[] { ',' });
            if (str.Length > 0)
            {
                if (strArray5.Length < length)
                {
                    num++;
                    text = text + "# HOST_PORTS < # ACTIVE_PORTS\n";
                }
                else
                {
                    if (this.checkDup(str, ',', "-1") > 0)
                    {
                        num++;
                        text = text + "Duplicate HOST_PORTS\n";
                    }
                    foreach (string str9 in strArray5)
                    {
                        if (this.checkDup(strArray, str9, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate HOST_PORTS and ACTIVE_PORTS\n";
                        }
                        if (this.checkDup(strArray4, str9, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate HOST_PORTS and TRACKER_PORTS\n";
                        }
                    }
                }
            }
            category = "RX_SETUP";
            key = "TTB_PORTS";
            str = this.GetIniFileString(category, key, "");
            string[] strArray6 = str.Split(new char[] { ',' });
            if (str.Length > 0)
            {
                if (strArray6.Length < length)
                {
                    num++;
                    text = text + "# TTB_PORTS < # ACTIVE_PORTS\n";
                }
                else
                {
                    if (this.checkDup(str, ',', "-1") > 0)
                    {
                        num++;
                        text = text + "Duplicate TTB_PORTS\n";
                    }
                    foreach (string str10 in strArray6)
                    {
                        if (this.checkDup(strArray, str10, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate TTB_PORTS and ACTIVE_PORTS\n";
                        }
                        if (this.checkDup(strArray4, str10, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate TTB_PORTS and TRACKER_PORTS\n";
                        }
                        if (this.checkDup(strArray5, str10, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate TTB_PORTS and HOST_PORTS\n";
                        }
                    }
                }
            }
            category = "RX_SETUP";
            key = "RESET_PORTS";
            str = this.GetIniFileString(category, key, "");
            string[] strArray7 = str.Split(new char[] { ',' });
            if (str.Length > 0)
            {
                if (strArray7.Length < length)
                {
                    num++;
                    text = text + "# RESET_PORTS < # ACTIVE_PORTS\n";
                }
                else
                {
                    if (this.checkDup(str, ',', "-1") > 0)
                    {
                        num++;
                        text = text + "Duplicate TTB_PORTS\n";
                    }
                    foreach (string str11 in strArray7)
                    {
                        if (this.checkDup(strArray, str11, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate RESET_PORTS and ACTIVE_PORTS\n";
                        }
                        if (this.checkDup(strArray4, str11, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate RESET_PORTS and TRACKER_PORTS\n";
                        }
                        if (this.checkDup(strArray5, str11, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate RESET_PORTS and HOST_PORTS\n";
                        }
                        if (this.checkDup(strArray6, str11, "-1") > 0)
                        {
                            num++;
                            text = text + "Duplicate RESET_PORTS and TTB_PORTS\n";
                        }
                    }
                }
            }
            foreach (string str12 in this.GetKeys("RX_SETUP"))
            {
                if (!str12.Contains("#"))
                {
                    str = this.GetIniFileString(category, str12, "");
                    switch (str12)
                    {
                        case "ACTIVE_PORTS":
                        case "TRACKER_PORTS":
                        case "HOST_PORTS":
                        case "RESET_PORTS":
                        case "HOST_APP_DIR":
                        case "EXTRA_HOST_APP_ARGVS":
                        {
                            continue;
                        }
                        case "HOST_APP":
                        {
                            if ((num3 != 0) && ((str.Length == 0) || (str.Split(new char[] { ',' }).Length < num3)))
                            {
                                num++;
                                text = text + "# " + str12 + " < # REQUIRED_HOSTS\n";
                            }
                            continue;
                        }
                        case "PATCH_FILE":
                        {
                            if ((num4 != 0) && ((str.Length == 0) || (str.Split(new char[] { ',' }).Length < num4)))
                            {
                                num++;
                                text = text + "# " + str12 + " < # REQUIRED_PATCH\n";
                            }
                            continue;
                        }
                    }
                    if ((str.Length == 0) || (str.Split(new char[] { ',' }).Length < length))
                    {
                        num++;
                        text = text + "# " + str12 + " < # ACTIVE_PORTS\n";
                    }
                }
            }
            if (num > 0)
            {
                MessageBox.Show(text, "ERROR!");
            }
            return num;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this._iniFilepath);
        }

        [DllImport("KERNEL32.DLL", EntryPoint="WritePrivateProfileStringW", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
        private static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFilename);

        public string IniFilePath
        {
            get
            {
                return this._iniFilepath;
            }
            set
            {
                this._iniFilepath = value;
            }
        }
    }
}

