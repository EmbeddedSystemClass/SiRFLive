﻿namespace SiRFLive.TruthData
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct XYZCoord
    {
        public double X;
        public double Y;
        public double Z;
        public static bool operator ==(XYZCoord lhs, XYZCoord rhs)
        {
            return (((lhs.X == rhs.X) && (lhs.Y == rhs.Y)) && (lhs.Z == rhs.Z));
        }

        public static bool operator !=(XYZCoord lhs, XYZCoord rhs)
        {
            if (((lhs.X == rhs.X) && (lhs.Y == rhs.Y)) && (lhs.Z == rhs.Z))
            {
                return false;
            }
            return true;
        }

        public static double getDistance(XYZCoord a, XYZCoord b)
        {
            double d = Math.Pow(a.X - b.X, 2.0) + Math.Pow(a.Y - b.Y, 2.0);
            d += Math.Pow(a.Z - b.Z, 2.0);
            return Math.Sqrt(d);
        }
    }
}

