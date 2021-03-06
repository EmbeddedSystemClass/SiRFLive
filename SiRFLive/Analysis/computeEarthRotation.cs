﻿namespace SiRFLive.Analysis
{
    using GPSUtils;
    using System;

    public class computeEarthRotation
    {
        public static double earthRotationCorrection(double[] svPos, double[] navPos, double[] svVel)
        {
            double num = GPSUtilsClass.getDistance(svPos, navPos);
            double[] a = new double[] { (svPos[0] - navPos[0]) / num, (svPos[1] - navPos[1]) / num, (svPos[2] - navPos[2]) / num };
            double[] b = new double[] { svVel[0] - (navPos[1] * ComputeSVPos.NLERTE), svVel[1] + (navPos[0] * ComputeSVPos.NLERTE), svVel[2] };
            return (num * (1.0 - (GPSUtilsClass.DOTPROD(a, b) / ComputeSVPos.NLC)));
        }
    }
}

