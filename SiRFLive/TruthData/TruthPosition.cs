﻿namespace SiRFLive.TruthData
{
    using GPSUtils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public class TruthPosition
    {
        private int m_current_idx;
        private SortedList<double, XYZCoord> m_PositionData = new SortedList<double, XYZCoord>();

        public double GetEndTime()
        {
            if (this.m_PositionData.Count > 0)
            {
                return this.m_PositionData.Keys[this.m_PositionData.Count - 1];
            }
            return -1.0;
        }

        public int GetNumSamples()
        {
            return this.m_PositionData.Count;
        }

        public bool GetPositionAtTime(double inTime, out XYZCoord result)
        {
            result.X = 0.0;
            result.Y = 0.0;
            result.Z = 0.0;
            try
            {
                int num = this.m_PositionData.IndexOfKey(inTime);
                if (num != -1)
                {
                    result = this.m_PositionData.Values[num];
                    this.m_current_idx = num;
                    return true;
                }
                int num2 = 0;
                int num3 = this.m_PositionData.Count - 1;
                return (this.GetPositionAtTimeBetweenIndexes(inTime, this.m_current_idx, num3, out result) || this.GetPositionAtTimeBetweenIndexes(inTime, num2, num3, out result));
            }
            catch
            {
            }
            return false;
        }

        private bool GetPositionAtTimeBetweenIndexes(double inTime, int low_idx, int high_idx, out XYZCoord result)
        {
            try
            {
                if ((inTime >= this.m_PositionData.Keys[low_idx]) && (inTime <= this.m_PositionData.Keys[high_idx]))
                {
                    int num = low_idx;
                    while (num < high_idx)
                    {
                        if (inTime <= this.m_PositionData.Keys[num])
                        {
                            break;
                        }
                        num++;
                    }
                    num--;
                    this.m_current_idx = num;
                    double percent = (100.0 * (inTime - this.m_PositionData.Keys[num])) / (this.m_PositionData.Keys[num + 1] - this.m_PositionData.Keys[num]);
                    result = this.LinearInterpolate(this.m_PositionData.Values[num], this.m_PositionData.Values[num + 1], percent);
                    return true;
                }
            }
            catch
            {
            }
            result.X = 0.0;
            result.Y = 0.0;
            result.Z = 0.0;
            return false;
        }

        public double GetStartTime()
        {
            if (this.m_PositionData.Count > 0)
            {
                return this.m_PositionData.Keys[0];
            }
            return -1.0;
        }

        private XYZCoord LinearInterpolate(XYZCoord in1, XYZCoord in2, double percent)
        {
            XYZCoord coord;
            GPSUtilsClass class2 = new GPSUtilsClass();
            double[] numArray = new double[] { in1.X, in1.Y, in1.Z };
            double[] numArray2 = new double[] { in2.X, in2.Y, in2.Z };
            double[] numArray3 = new double[3];
            numArray3 = class2.LinearInterpolateXYZ(numArray, numArray2, percent);
            coord.X = numArray3[0];
            coord.Y = numArray3[1];
            coord.Z = numArray3[2];
            return coord;
        }

        public bool LoadFromFile(string filename)
        {
            try
            {
                StreamReader reader = File.OpenText(filename);
                string str = "";
                while (((str = reader.ReadLine()) != null) && !str.Contains("End of File"))
                {
                    string[] strArray = str.Split(new char[] { ',' });
                    if (strArray[0] == "41")
                    {
                        double num9;
                        double num10;
                        double num11;
                        XYZCoord coord;
                        int index = 4;
                        int num2 = 12;
                        int num3 = 13;
                        int num4 = 14;
                        double key = double.Parse(strArray[index]) / 1000.0;
                        double lat = double.Parse(strArray[num2]);
                        double lng = double.Parse(strArray[num3]);
                        double ht = double.Parse(strArray[num4]);
                        lat *= 1E-07;
                        lng *= 1E-07;
                        ht *= 0.01;
                        GPSUtilsClass.ConvertGEO2XYZ(lat * 0.017453292519944444, lng * 0.017453292519944444, ht, out num9, out num10, out num11);
                        GPSUtilsClass.ConvertXYZ2GEO(num9, num10, num11, out lat, out lng, out ht);
                        lat *= 57.29577951307855;
                        lng *= 57.29577951307855;
                        coord.X = num9;
                        coord.Y = num10;
                        coord.Z = num11;
                        this.m_PositionData.Add(key, coord);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

