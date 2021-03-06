﻿namespace SiRFLive.GUI.Python
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.IO;
    using System.Windows.Forms;

    public class PythonHistory
    {
        public ArrayList cmdBuffer = new ArrayList();
        private int index;

        public PythonHistory()
        {
            this.Add("");
            this.LoadHistory(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts\hist.txt");
        }

        public void Add(object o)
        {
            this.cmdBuffer.Add(o);
            this.index = 0;
        }

        public void ClearHistory()
        {
            this.cmdBuffer.Clear();
            this.Add("");
            this.index = 0;
        }

        public void LoadHistory(string inFilename)
        {
            try
            {
                if (File.Exists(inFilename))
                {
                    string str;
                    StreamReader reader = File.OpenText(inFilename);
                    while ((str = reader.ReadLine()) != null)
                    {
                        this.Add(str);
                    }
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "SiRFLive...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public string Next()
        {
            if (this.index == (this.cmdBuffer.Count - 1))
            {
                this.index = 0;
            }
            else
            {
                this.index++;
            }
            return (string) this.cmdBuffer[this.index];
        }

        public string Prev()
        {
            if (this.index == 0)
            {
                this.index = this.cmdBuffer.Count - 1;
            }
            else
            {
                this.index--;
            }
            return (string) this.cmdBuffer[this.index];
        }

        public void SaveHistory(string inFilename)
        {
            try
            {
                if (File.Exists(inFilename))
                {
                    File.Delete(inFilename);
                }
                StreamWriter writer = File.CreateText(inFilename);
                for (int i = 0; i < this.cmdBuffer.Count; i++)
                {
                    writer.WriteLine(this.cmdBuffer[i]);
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "SiRFLive...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}

