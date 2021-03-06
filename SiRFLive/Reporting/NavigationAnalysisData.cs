﻿namespace SiRFLive.Reporting
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class NavigationAnalysisData
    {
        private string _aidingFlags = "-9999";
        private string _avgCno = "-9999";
        private string _fErr = "-9999";
        private double _firstFix2DPositionError = -9999.0;
        private double _firstFix3DPositionError = -9999.0;
        private double _firstFixMeasAlt = -9999.0;
        private double _firstFixMeasLat = -9999.0;
        private double _firstFixMeasLon = -9999.0;
        private double _firstFixTOW = -9999.0;
        private double _firstFixVerticalPositionError = -9999.0;
        private string _fUncer = "-9999";
        private bool _isNav;
        private bool _isValid;
        private double _measAlt = -9999.0;
        private double _measLat = -9999.0;
        private double _measLon = -9999.0;
        private double _nav2DPositionError = -9999.0;
        private double _nav3DPositionError = -9999.0;
        private double _navVerticalPositionError = -9999.0;
        private int _numSVsInFix;
        private double _posErr = -9999.0;
        private double _refAlt = -9999.0;
        private double _refLat = -9999.0;
        private string _refLocationName = "Default";
        private double _refLon = -9999.0;
        private string _tErr = "-9999";
        private double _tow = -9999.0;
        private double _ttffAided = -9999.0;
        private double _ttffFirstNav = -9999.0;
        private string _ttffLogTime = "";
        private double _ttffReport = -9999.0;
        private double _ttffReset = -9999.0;
        private double _ttffSiRFLive = -9999.0;
        private string _tUncer = "-9999";
        private bool _validatePosition = true;
        private XmlDocument m_XMLDocument;
        private string m_XmlFile;

        internal NavigationAnalysisData(string xmlFile)
        {
            try
            {
                this.m_XmlFile = xmlFile;
                this.m_XMLDocument = new XmlDocument();
                this.m_XMLDocument.Load(xmlFile);
                this._isNav = false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public ArrayList GetReferenceLocationName()
        {
            ArrayList list = new ArrayList();
            XmlNodeList list2 = this.m_XMLDocument.SelectNodes("/referenceLocation/location");
            try
            {
                foreach (XmlNode node in list2)
                {
                    list.Add(node.Attributes["name"].Value);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public PositionInLatLonAlt GetReferencePosition(string locationName)
        {
            PositionInLatLonAlt alt = new PositionInLatLonAlt();
            alt.name = "Error";
            string str = string.Empty;
            if (locationName == "Default")
            {
                str = this.m_XMLDocument.SelectSingleNode("/referenceLocation/location[@name='" + locationName + "']/field[@name='REF']").Attributes["default"].Value;
            }
            else
            {
                str = locationName;
            }
            XmlNodeList list = this.m_XMLDocument.SelectNodes("/referenceLocation/location[@name='" + str + "']/field");
            try
            {
                foreach (XmlNode node2 in list)
                {
                    string str2 = node2.Attributes["name"].Value;
                    string str3 = node2.Attributes["default"].Value;
                    if (str3.Length == 0)
                    {
                        str3 = "-9999";
                    }
                    switch (str2)
                    {
                        case "LAT":
                            alt.latitude = Convert.ToDouble(str3);
                            break;

                        case "LON":
                            alt.longitude = Convert.ToDouble(str3);
                            break;

                        case "ALT":
                            alt.altitude = Convert.ToDouble(str3);
                            break;
                    }
                }
                alt.name = str;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Get Reference Location: " + exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return alt;
        }

        public void SetDefaultReferencePosition(string locationName, string lat, string lon, string alt, bool echoWarning)
        {
            if (File.Exists(this.m_XmlFile))
            {
                if (echoWarning && (MessageBox.Show(string.Format("File exists -- Overwrite?\n {0}", this.m_XmlFile), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No))
                {
                    return;
                }
                if ((File.GetAttributes(this.m_XmlFile) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("Read only file -- Please change property and retry\n {0}", this.m_XmlFile), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            XmlNodeList list = this.m_XMLDocument.SelectNodes("/referenceLocation/location");
            bool flag = false;
            foreach (XmlNode node in list)
            {
                if (locationName == node.Attributes["name"].Value)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                XmlNode newChild = this.m_XMLDocument.SelectSingleNode("/referenceLocation/location[@name='Sim-STORMLab']").Clone();
                newChild.Attributes["name"].Value = locationName;
                foreach (XmlElement element in newChild.ChildNodes)
                {
                    if (element.GetAttribute("name") == "LAT")
                    {
                        element.SetAttribute("default", lat);
                    }
                    if (element.GetAttribute("name") == "LON")
                    {
                        element.SetAttribute("default", lon);
                    }
                    if (element.GetAttribute("name") == "ALT")
                    {
                        element.SetAttribute("default", alt);
                    }
                }
                this.m_XMLDocument.SelectSingleNode("referenceLocation").AppendChild(newChild);
                this.m_XMLDocument.Save(this.m_XmlFile);
            }
            this.m_XMLDocument.SelectSingleNode("/referenceLocation/location[@name='Default']/field[@name='REF']").Attributes["default"].Value = locationName;
            this.m_XMLDocument.SelectSingleNode("/referenceLocation/location[@name='Default']/field[@name='LAT']").Attributes["default"].Value = lat;
            this.m_XMLDocument.SelectSingleNode("/referenceLocation/location[@name='Default']/field[@name='LON']").Attributes["default"].Value = lon;
            this.m_XMLDocument.SelectSingleNode("/referenceLocation/location[@name='Default']/field[@name='ALT']").Attributes["default"].Value = alt;
            this.m_XMLDocument.Save(this.m_XmlFile);
        }

        public string AidingFlags
        {
            get
            {
                return this._aidingFlags;
            }
            set
            {
                this._aidingFlags = value;
            }
        }

        public string AvgCNo
        {
            get
            {
                return this._avgCno;
            }
            set
            {
                this._avgCno = value;
            }
        }

        public double FirstFix2DPositionError
        {
            get
            {
                return this._firstFix2DPositionError;
            }
            set
            {
                this._firstFix2DPositionError = value;
            }
        }

        public double FirstFix3DPositionError
        {
            get
            {
                return this._firstFix3DPositionError;
            }
            set
            {
                this._firstFix3DPositionError = value;
            }
        }

        public double FirstFixMeasAlt
        {
            get
            {
                return this._firstFixMeasAlt;
            }
            set
            {
                this._firstFixMeasAlt = value;
            }
        }

        public double FirstFixMeasLat
        {
            get
            {
                return this._firstFixMeasLat;
            }
            set
            {
                this._firstFixMeasLat = value;
            }
        }

        public double FirstFixMeasLon
        {
            get
            {
                return this._firstFixMeasLon;
            }
            set
            {
                this._firstFixMeasLon = value;
            }
        }

        public double FirstFixTOW
        {
            get
            {
                return this._firstFixTOW;
            }
            set
            {
                this._firstFixTOW = value;
            }
        }

        public double FirstFixVerticalPositionError
        {
            get
            {
                return this._firstFixVerticalPositionError;
            }
            set
            {
                this._firstFixVerticalPositionError = value;
            }
        }

        public string FreqErr
        {
            get
            {
                return this._fErr;
            }
            set
            {
                this._fErr = value;
            }
        }

        public string FreqUncer
        {
            get
            {
                return this._fUncer;
            }
            set
            {
                this._fUncer = value;
            }
        }

        public bool IsNav
        {
            get
            {
                return this._isNav;
            }
            set
            {
                this._isNav = value;
            }
        }

        public double MeasAlt
        {
            get
            {
                return this._measAlt;
            }
            set
            {
                this._measAlt = value;
            }
        }

        public double MeasLat
        {
            get
            {
                return this._measLat;
            }
            set
            {
                this._measLat = value;
            }
        }

        public double MeasLon
        {
            get
            {
                return this._measLon;
            }
            set
            {
                this._measLon = value;
            }
        }

        public double Nav2DPositionError
        {
            get
            {
                return this._nav2DPositionError;
            }
            set
            {
                this._nav2DPositionError = value;
            }
        }

        public double Nav3DPositionError
        {
            get
            {
                return this._nav3DPositionError;
            }
            set
            {
                this._nav3DPositionError = value;
            }
        }

        public double NavVerticalPositionError
        {
            get
            {
                return this._navVerticalPositionError;
            }
            set
            {
                this._navVerticalPositionError = value;
            }
        }

        public int NumSVsInFix
        {
            get
            {
                return this._numSVsInFix;
            }
            set
            {
                this._numSVsInFix = value;
            }
        }

        public double PosErr
        {
            get
            {
                return this._posErr;
            }
            set
            {
                this._posErr = value;
            }
        }

        public double RefAlt
        {
            get
            {
                return this._refAlt;
            }
            set
            {
                this._refAlt = value;
            }
        }

        public double RefLat
        {
            get
            {
                return this._refLat;
            }
            set
            {
                this._refLat = value;
            }
        }

        public string RefLocationName
        {
            get
            {
                return this._refLocationName;
            }
            set
            {
                this._refLocationName = value;
            }
        }

        public double RefLon
        {
            get
            {
                return this._refLon;
            }
            set
            {
                this._refLon = value;
            }
        }

        public string TimeErr
        {
            get
            {
                return this._tErr;
            }
            set
            {
                this._tErr = value;
            }
        }

        public string TimeUncer
        {
            get
            {
                return this._tUncer;
            }
            set
            {
                this._tUncer = value;
            }
        }

        public double TOW
        {
            get
            {
                return this._tow;
            }
            set
            {
                this._tow = value;
            }
        }

        public double TTFFAided
        {
            get
            {
                return this._ttffAided;
            }
            set
            {
                this._ttffAided = value;
            }
        }

        public double TTFFFirstNav
        {
            get
            {
                return this._ttffFirstNav;
            }
            set
            {
                this._ttffFirstNav = value;
            }
        }

        public string TTFFLogTime
        {
            get
            {
                return this._ttffLogTime;
            }
            set
            {
                this._ttffLogTime = value;
            }
        }

        public double TTFFReport
        {
            get
            {
                return this._ttffReport;
            }
            set
            {
                this._ttffReport = value;
            }
        }

        public double TTFFReset
        {
            get
            {
                return this._ttffReset;
            }
            set
            {
                this._ttffReset = value;
            }
        }

        public double TTFFSiRFLive
        {
            get
            {
                return this._ttffSiRFLive;
            }
            set
            {
                this._ttffSiRFLive = value;
            }
        }

        public bool Valid
        {
            get
            {
                return this._isValid;
            }
            set
            {
                this._isValid = value;
            }
        }

        public bool ValidatePosition
        {
            get
            {
                return this._validatePosition;
            }
            set
            {
                this._validatePosition = value;
            }
        }
    }
}

