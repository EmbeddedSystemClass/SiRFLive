﻿namespace SiRFLive.TruthData
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class TimestampFile
    {
        private string m_filename;
        public List<Timestamp> m_Timestamps;
        private XmlDocument m_XMLDocument;

        public TimestampFile(string filename)
        {
            try
            {
                this.m_Timestamps = new List<Timestamp>();
                this.m_filename = filename;
                this.m_XMLDocument = new XmlDocument();
                this.m_XMLDocument.Load(this.m_filename);
                this.ReadTimestampListFromXML();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string PrintTimestampList()
        {
            string str = "";
            foreach (Timestamp timestamp in this.m_Timestamps)
            {
                string str2 = str;
                str = str2 + timestamp.name + "(" + timestamp.gps_tow + "," + timestamp.gps_week + "),\r\n";
            }
            return str;
        }

        private void ReadTimestampListFromXML()
        {
            try
            {
                foreach (XmlNode node in this.m_XMLDocument.SelectNodes("/TIMESTAMP_DATA/TIMESTAMPS/TIMESTAMP"))
                {
                    Timestamp item = new Timestamp();
                    item.name = node.Attributes["NAME"].Value;
                    item.gps_tow = node.Attributes["GPS_TOW"].Value;
                    item.gps_week = node.Attributes["GPS_WEEK"].Value;
                    this.m_Timestamps.Add(item);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}

