﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Collections.Generic;

    public class Stats : IDisposable
    {
        public List<double> DataList = new List<double>();
        private bool isDisposed;

        private double average(double[] data)
        {
            double num = 0.0;
            double num2 = 0.0;
            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    num2 += data[i];
                }
                num = this.safeDivide(num2, (double) data.Length);
            }
            catch (Exception)
            {
                throw;
            }
            return num;
        }

        public void Clear()
        {
            this.DataList.Clear();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.DataList.Clear();
                this.DataList = null;
            }
            this.isDisposed = true;
        }

        ~Stats()
        {
            this.Dispose(true);
        }

        public double GetAscendingValueByIndex(int idx, double excludedVal)
        {
            double num = -9999.0;
            if ((idx >= 0) && (this.DataList.Count > 0))
            {
                this.DataList.Sort();
                double[] numArray = this.Remove(excludedVal);
                if (numArray.Length >= idx)
                {
                    num = numArray[idx];
                }
            }
            return num;
        }

        public int GetInBoundCount(double num)
        {
            int num2 = 0;
            if (this.DataList.Count > 0)
            {
                foreach (double num3 in this.DataList)
                {
                    if (num3 < num)
                    {
                        num2++;
                    }
                }
            }
            return num2;
        }

        public int GetOutBoundCount(double num)
        {
            int num2 = 0;
            if (this.DataList.Count > 0)
            {
                foreach (double num3 in this.DataList)
                {
                    if (num3 > num)
                    {
                        num2++;
                    }
                }
            }
            return num2;
        }

        public double GetPercentile(double p, double limit)
        {
            double num = -9999.0;
            if (this.DataList.Count <= 0)
            {
                return num;
            }
            this.DataList.Sort();
            double[] numArray = this.Remove(limit);
            if (numArray.Length <= 0)
            {
                return -9999.0;
            }
            if ((p >= 100.0) || (numArray.Length == 1))
            {
                return numArray[numArray.Length - 1];
            }
            double num2 = ((numArray.Length + 1) * p) / 100.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double d = ((p / 100.0) * (numArray.Length - 1)) + 1.0;
            if (num2 >= 1.0)
            {
                num3 = numArray[((int) Math.Floor(d)) - 1];
                num4 = numArray[(int) Math.Floor(d)];
            }
            else
            {
                num3 = numArray[0];
                num4 = numArray[1];
            }
            if (num3 == num4)
            {
                return num3;
            }
            double num6 = d - Math.Floor(d);
            return (num3 + (num6 * (num4 - num3)));
        }

        public double GetPercentile(double[] data_arr, double p)
        {
            int length = data_arr.GetLength(0);
            this.SortArray(length, data_arr);
            if (data_arr.Length <= 0)
            {
                return -9999.0;
            }
            if ((p >= 100.0) || (data_arr.Length == 1))
            {
                return data_arr[data_arr.Length - 1];
            }
            double num2 = ((data_arr.Length + 1) * p) / 100.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double d = ((p / 100.0) * (data_arr.Length - 1)) + 1.0;
            if (num2 >= 1.0)
            {
                num3 = data_arr[((int) Math.Floor(d)) - 1];
                num4 = data_arr[(int) Math.Floor(d)];
            }
            else
            {
                num3 = data_arr[0];
                num4 = data_arr[1];
            }
            if (num3 == num4)
            {
                return num3;
            }
            double num6 = d - Math.Floor(d);
            return (num3 + (num6 * (num4 - num3)));
        }

        public int GetSamplesExcludingThese(double exclude)
        {
            int length = -9999;
            if (this.DataList.Count > 0)
            {
                this.DataList.Sort();
                double[] numArray = this.Remove(exclude);
                if (numArray.Length > 0)
                {
                    length = numArray.Length;
                }
            }
            return length;
        }

        public void Init(double value)
        {
        }

        public void InsertSample(double sample)
        {
            this.DataList.Add(sample);
        }

        public double[] Remove(double n)
        {
            List<double> list = new List<double>();
            foreach (double num in this.DataList)
            {
                if (num != n)
                {
                    list.Add(num);
                }
            }
            return list.ToArray();
        }

        private double safeDivide(double value1, double value2)
        {
            double num = 0.0;
            try
            {
                if ((value1 == 0.0) || (value2 == 0.0))
                {
                    return num;
                }
                num = value1 / value2;
            }
            catch (Exception)
            {
                throw;
            }
            return num;
        }

        public void SortArray(int x, double[] a)
        {
            int num3 = 3;
            while (num3 > 0)
            {
                for (int i = 0; i < x; i++)
                {
                    int index = i;
                    double num4 = a[i];
                    while ((index >= num3) && (a[index - num3] > num4))
                    {
                        a[index] = a[index - num3];
                        index -= num3;
                    }
                    a[index] = num4;
                }
                if ((num3 / 2) != 0)
                {
                    num3 /= 2;
                }
                else
                {
                    if (num3 == 1)
                    {
                        num3 = 0;
                        continue;
                    }
                    num3 = 1;
                }
            }
        }

        public double StandardDeviation(double exclussionValue)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            int length = 0;
            try
            {
                if (this.DataList.Count <= 0)
                {
                    return num;
                }
                this.DataList.Sort();
                double[] data = this.Remove(exclussionValue);
                length = data.Length;
                if (length == 0)
                {
                    return num;
                }
                num2 = this.average(data);
                for (int i = 0; i < length; i++)
                {
                    num3 += Math.Pow(data[i] - num2, 2.0);
                }
                num = Math.Sqrt(this.safeDivide(num3, (double) length));
            }
            catch
            {
            }
            return num;
        }

        public double StandardDeviation(double[] data)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            int length = 0;
            try
            {
                length = data.Length;
                if (length == 0)
                {
                    return num;
                }
                num2 = this.average(data);
                for (int i = 0; i < length; i++)
                {
                    num3 += Math.Pow(data[i] - num2, 2.0);
                }
                num = Math.Sqrt(this.safeDivide(num3, (double) length));
            }
            catch (Exception)
            {
                throw;
            }
            return num;
        }

        public double Stats_Max()
        {
            double num = -9999.0;
            if (this.DataList.Count <= 0)
            {
                return num;
            }
            this.DataList.Sort();
            if (this.DataList.Count > 1)
            {
                return this.DataList[this.DataList.Count - 1];
            }
            return this.DataList[0];
        }

        public double Stats_Max(double limit)
        {
            double num = -9999.0;
            if (this.DataList.Count > 0)
            {
                this.DataList.Sort();
                double[] numArray = this.Remove(limit);
                if (numArray.Length >= 1)
                {
                    num = numArray[numArray.Length - 1];
                }
            }
            return num;
        }

        public double Stats_Max(double[] data_arr)
        {
            int length = data_arr.GetLength(0);
            this.SortArray(length, data_arr);
            if (length > 0)
            {
                return data_arr[length - 1];
            }
            return -9999.0;
        }

        public double Stats_Mean()
        {
            double num = -9999.0;
            double num2 = 0.0;
            if (this.DataList.Count <= 0)
            {
                return num;
            }
            this.DataList.Sort();
            foreach (double num3 in this.DataList)
            {
                num2 += num3;
            }
            return (num2 / ((double) this.DataList.Count));
        }

        public double Stats_Mean(double limit)
        {
            double num = -9999.0;
            double num2 = 0.0;
            if (this.DataList.Count <= 0)
            {
                return num;
            }
            this.DataList.Sort();
            double[] numArray = this.Remove(limit);
            if (numArray.Length <= 0)
            {
                return num;
            }
            foreach (double num3 in numArray)
            {
                num2 += num3;
            }
            return (num2 / ((double) numArray.Length));
        }

        public double Stats_Mean(double[] data_arr)
        {
            double num = 0.0;
            int length = data_arr.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                num += data_arr[i];
            }
            return (num / ((double) length));
        }

        public double Stats_Min()
        {
            double num = -9999.0;
            if (this.DataList.Count > 0)
            {
                this.DataList.Sort();
                num = this.DataList[0];
            }
            return num;
        }

        public double Stats_Min(double limit)
        {
            double num = -9999.0;
            if (this.DataList.Count > 0)
            {
                this.DataList.Sort();
                double[] numArray = this.Remove(limit);
                if (numArray.Length > 0)
                {
                    num = numArray[0];
                }
            }
            return num;
        }

        public double Stats_Min(double[] data_arr)
        {
            int length = data_arr.GetLength(0);
            this.SortArray(length, data_arr);
            return data_arr[0];
        }

        public int Samples
        {
            get
            {
                return this.DataList.Count;
            }
        }
    }
}

