﻿namespace SiRFLive.Analysis
{
    using SiRFLive.MessageHandling;
    using SiRFLive.Utilities;
    using System;
    using System.Configuration;

    public class msgDecoded
    {
        public static string[] DecodeMsg(string message)
        {
            if ((message.Length == 0) || (message == null))
            {
                string[] strArray = new string[0x79];
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = "0";
                }
                return strArray;
            }
            byte[] comByte = HelperFunctions.HexToByte(message);
            MsgFactory factory = new MsgFactory(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\Protocols.xml");
            return factory.ConvertRawToFields(comByte).Split(new char[] { ',' });
        }
    }
}

