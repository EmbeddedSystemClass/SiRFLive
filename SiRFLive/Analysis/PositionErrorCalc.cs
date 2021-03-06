﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Collections.Generic;

    public class PositionErrorCalc
    {
        private double _alongTrackError;
        private double _position2DError;
        private double _position3DError;
        private double _verticalError;
        private double _xTrackError;
        private static double E_SQR = (E_VALUE * E_VALUE);
        private static double E_VALUE = 0.081819190843;
        private static double EARTH_RADIUS = 6378137.0;
        private static double LAT_METERS_PER_DEGREE = 111199.233;
        private static double RADIANS_PER_DEGREE = 0.017453292519943295;

        public static double Get2DPositionErrorInMeters(double Lat, double Lon, double Ref_Lat, double Ref_Lon)
        {
            double num = MetersPerLonDegree(Lat);
            double num2 = (Lat - Ref_Lat) * LAT_METERS_PER_DEGREE;
            double num3 = (Lon - Ref_Lon) * num;
            return Math.Sqrt((num2 * num2) + (num3 * num3));
        }

        public double Get3DPositionErrorsInMeter(double Lat, double Lon, double alt, double Ref_Lat, double Ref_Lon, double Ref_Alt)
        {
            double num = MetersPerLonDegree(Lat);
            double num2 = (Lat - Ref_Lat) * LAT_METERS_PER_DEGREE;
            double num3 = (Lon - Ref_Lon) * num;
            double num4 = alt - Ref_Alt;
            double num5 = (num2 * num2) + (num3 * num3);
            return Math.Sqrt(num5 + (num4 * num4));
        }

        public void GetPositionErrorsInMeter(List<double> positionDataList)
        {
            double num6 = positionDataList[0] - positionDataList[4];
            double num7 = positionDataList[1] - positionDataList[5];
            double num = MetersPerLonDegree(positionDataList[0]);
            double num2 = num6 * LAT_METERS_PER_DEGREE;
            double num3 = num7 * num;
            double num4 = positionDataList[2] - positionDataList[6];
            this._verticalError = Math.Abs(num4);
            double d = (num2 * num2) + (num3 * num3);
            this._position2DError = Math.Sqrt(d);
            this._position3DError = Math.Sqrt(d + (num4 * num4));
            double a = ((positionDataList[0] + positionDataList[4]) * RADIANS_PER_DEGREE) / 2.0;
            double num9 = Math.Sin(a);
            double num10 = Math.Cos(a);
            double num11 = Math.Sqrt(1.0 - ((E_SQR * num9) * num9));
            double num12 = (EARTH_RADIUS * (1.0 - E_SQR)) / ((num11 * num11) * num11);
            double num13 = EARTH_RADIUS / num11;
            double x = (num12 * num6) * RADIANS_PER_DEGREE;
            double y = ((num13 * num10) * num7) * RADIANS_PER_DEGREE;
            double num16 = (90.0 - positionDataList[7]) * RADIANS_PER_DEGREE;
            double num17 = Math.Atan2(y, x) - num16;
            double num18 = Math.Sqrt((y * y) + (x * x));
            this._alongTrackError = num18 * Math.Cos(num17);
            this._xTrackError = num18 * Math.Sin(num17);
        }

        public void GetPositionErrorsInMeter(double Lat, double Lon, double alt, double Ref_Lat, double Ref_Lon, double Ref_Alt)
        {
            double num = MetersPerLonDegree(Lat);
            double num2 = (Lat - Ref_Lat) * LAT_METERS_PER_DEGREE;
            double num3 = (Lon - Ref_Lon) * num;
            double num4 = alt - Ref_Alt;
            this._verticalError = Math.Abs(num4);
            double d = (num2 * num2) + (num3 * num3);
            this._position2DError = Math.Sqrt(d);
            this._position3DError = Math.Sqrt(d + (num4 * num4));
        }

        public static double GetVerticalErrorInMeters(double Alt, double Ref_Alt)
        {
            return Math.Abs((double) (Alt - Ref_Alt));
        }

        private static double MetersPerLonDegree(double lat)
        {
            return (Math.Cos(lat * RADIANS_PER_DEGREE) * LAT_METERS_PER_DEGREE);
        }

        public double AlongTrackErrorInMeter
        {
            get
            {
                return this._alongTrackError;
            }
        }

        public double HorizontalError
        {
            get
            {
                return this._position2DError;
            }
        }

        public double Position3DError
        {
            get
            {
                return this._position3DError;
            }
        }

        public double VerticalErrorInMeter
        {
            get
            {
                return this._verticalError;
            }
        }

        public double XTrackErrorInMeter
        {
            get
            {
                return this._xTrackError;
            }
        }
    }
}

