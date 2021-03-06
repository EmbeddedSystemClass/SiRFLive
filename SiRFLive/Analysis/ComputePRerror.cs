﻿namespace SiRFLive.Analysis
{
    using GPSUtils;
    using SiRFLive.TruthData;
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ComputePRerror
    {
        private TruthPosition m_ImuFile = new TruthPosition();
        private RinexFile m_RinexFile = new RinexFile();
        private int m_SV_PRN;
        private double m_SV_TOW;
        private double m_SV_TxTOW;

        public bool ComputePRerrorOneFile(string GPSFileName, string RinexFileName, string IMUFileName, string outFileName)
        {
            bool flag = false;
            try
            {
                StreamWriter writer;
                StreamReader reader = File.OpenText(GPSFileName);
                if (File.Exists(outFileName))
                {
                    writer = File.AppendText(outFileName);
                }
                else
                {
                    writer = File.CreateText(outFileName);
                }
                string str = string.Format("PRTrue, ClkBiasRange, TxTOW", new object[0]);
                string str2 = string.Format("svPRN, TOW, PR, {0}, ClkBias, PRError\r\n", str);
                writer.Write(str2);
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                string str3 = "";
                while (((str3 = reader.ReadLine()) != null) && !str3.Contains("End of File"))
                {
                    string[] strArray = str3.Split(new char[] { ',' });
                    if (strArray[0] == "28")
                    {
                        Msg28svData data;
                        int index = 3;
                        int num2 = 4;
                        int num3 = 5;
                        data.svPRN = int.Parse(strArray[index]);
                        data.TOW = double.Parse(strArray[num2]);
                        data.PR = double.Parse(strArray[num3]);
                        list.Add(data);
                    }
                    else
                    {
                        if (strArray[0] == "30")
                        {
                            Msg30svData data2;
                            int num4 = 1;
                            int num5 = 14;
                            data2.svPRN = int.Parse(strArray[num4]);
                            data2.ionoErr = double.Parse(strArray[num5]);
                            list2.Add(data2);
                            continue;
                        }
                        if (str3.StartsWith("Week:") || (strArray[0] == "7"))
                        {
                            double clkBias = 0.0;
                            if (str3.StartsWith("Week:"))
                            {
                                string[] strArray2 = str3.Split(new char[] { ':' });
                                clkBias = double.Parse(strArray2[strArray2.Length - 1].Split(new char[] { ' ' })[0]) * 1E-09;
                            }
                            if (strArray[0] == "7")
                            {
                                int num7 = 5;
                                clkBias = double.Parse(strArray[num7]) * 1E-09;
                            }
                            foreach (Msg28svData data3 in list)
                            {
                                string str4;
                                Msg30svData data4 = new Msg30svData();
                                foreach (Msg30svData data5 in list2)
                                {
                                    if (data5.svPRN == data3.svPRN)
                                    {
                                        data4 = data5;
                                        break;
                                    }
                                }
                                double num8 = this.ComputePRerrorOneSV(data3.svPRN, data3.TOW, data3.PR, data4.ionoErr, clkBias, RinexFileName, IMUFileName, out str4);
                                string str5 = string.Format("{0},{1},{2},{3},{4},{5}\r\n", new object[] { data3.svPRN, data3.TOW, data3.PR, str4, clkBias, num8 });
                                writer.Write(str5);
                                flag = true;
                            }
                            list.Clear();
                            list2.Clear();
                        }
                    }
                }
                writer.Flush();
                writer.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Computing PR error...", MessageBoxButtons.OK);
            }
            return flag;
        }

        public double ComputePRerrorOneSV(int svPRN, double svTOW, double svPseudoRange, double ionoErr, double clkBias, string RinexFileName, string IMUFileName, out string strPRtrue_ClkBiasRange_txTOW)
        {
            double num6;
            double num7;
            double num = 0.0;
            strPRtrue_ClkBiasRange_txTOW = "-1.0,-1.0,-1.0";
            this.m_SV_PRN = svPRN;
            this.m_SV_TOW = svTOW;
            double num2 = svPseudoRange / 299792458.0;
            this.m_SV_TxTOW = (this.m_SV_TOW - num2) - clkBias;
            double ionoError = 0.0;
            double svClkBias = 0.0;
            XYZCoord svVel = new XYZCoord();
            XYZCoord a = this.GetRinexSVPos(RinexFileName, this.m_SV_PRN, this.m_SV_TxTOW, out ionoError, out svClkBias, out svVel);
            XYZCoord iMUTruePos = this.GetIMUTruePos(IMUFileName, svTOW);
            XYZCoord coord4 = new XYZCoord();
            if ((a == coord4) || (iMUTruePos == coord4))
            {
                return -1.0;
            }
            double num5 = XYZCoord.getDistance(a, iMUTruePos);
            double[] svPos = new double[] { a.X, a.Y, a.Z };
            double[] navPos = new double[] { iMUTruePos.X, iMUTruePos.Y, iMUTruePos.Z };
            double[] numArray3 = new double[] { svVel.X, svVel.Y, svVel.Z };
            computeTropo tropo = new computeTropo();
            GPSUtilsClass.ComputeAzEl(navPos, svPos, out num6, out num7);
            double alt = GPSUtilsClass.ComputeApproxAltitude(navPos);
            double num9 = tropo.NL_ComputeTropo(alt, num7);
            double num10 = computeEarthRotation.earthRotationCorrection(svPos, navPos, numArray3);
            double num11 = num5 - num10;
            double num12 = (svPseudoRange - (clkBias * 299792458.0)) + (svClkBias * 299792458.0);
            strPRtrue_ClkBiasRange_txTOW = string.Format("{0}, {1}, {2}", num5, num12, this.m_SV_TxTOW);
            num = num5 - num12;
            return (((num - ionoErr) - num9) - num11);
        }

        public double ComputeRangeAtTow(double TOW)
        {
            return 0.0;
        }

        public XYZCoord GetIMUTruePos(string filename, double TOW)
        {
            XYZCoord result = new XYZCoord();
            if (this.m_ImuFile.GetNumSamples() == 0)
            {
                this.m_ImuFile.LoadFromFile(filename);
            }
            this.m_ImuFile.GetPositionAtTime(TOW, out result);
            return result;
        }

        public XYZCoord GetRinexSVPos(string filename, int svPRN, double TOW, out double ionoError, out double svClkBias, out XYZCoord svVel)
        {
            XYZCoord coord = new XYZCoord();
            if (this.m_RinexFile.GetNumEphemerides() == 0)
            {
                this.m_RinexFile.Read(@"..\scripts\TunnelsMeasAndNav\brdc2140.07n");
            }
            RinexEph eph = this.m_RinexFile.SearchRinexArrayList((byte) svPRN, (int) TOW);
            if (eph == null)
            {
                ionoError = 0.0;
                svClkBias = 0.0;
                svVel.X = svVel.Y = svVel.Z = 0.0;
                return coord;
            }
            tSVD_SVState st = new tSVD_SVState();
            tGPSTime gTime = new tGPSTime();
            gTime.isTOWValid = true;
            gTime.isWeekValid = true;
            gTime.time = TOW;
            gTime.week = (int) eph.weekNo;
            new ComputeSVPos().computeSVState(st, (short) svPRN, eph, gTime);
            coord.X = st.pos[0];
            coord.Y = st.pos[1];
            coord.Z = st.pos[2];
            svVel.X = st.vel[0];
            svVel.Y = st.vel[1];
            svVel.Z = st.vel[2];
            ionoError = st.ionoDelayStd;
            svClkBias = st.clockBias;
            return coord;
        }

        public int GetSV_PRN()
        {
            return this.m_SV_PRN;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Msg28svData
        {
            public int svPRN;
            public double TOW;
            public double PR;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Msg30svData
        {
            public int svPRN;
            public double ionoErr;
        }
    }
}

