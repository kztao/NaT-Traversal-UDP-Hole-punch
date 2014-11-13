/*
 * Presented by © TiARPi® Softwaring & Development
 * All rights Reserved @TiARPi
 * Turap ÜLKÜ - Ozan ÜLKÜ - Zafer KETENCI - Burak SELÇUK
 * 
 * Client App
 * 
 * 08.11.2014
 * Ver1.2
 * For Help visit www.tiarpi.com/p2p
 * 
 * + 90 216 474 92 12/13/14
 * info@tiarpi.com
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Management;

namespace p2p
{
    public static class Generate
    {
        public static int SendedCode = -1;
        public static string Address = "185.22.185.53";
        public static int PortA = 4002;
        public static int PortB = 4003;
        public static string _AppID = "";
        public static string _licenceKey = "";
        public static int comingCounter = 0;
        public static int sendingCounter = 0;
        public static bool ShowMeInfoWithMessageBox = false;
        public static bool ShowMeInfoWithConsole = true;
        public static Dictionary<string,Agents> AgentList = new Dictionary<string,Agents>();
        public static bool isO = false;

        public static string GenerateWord()
        {
            string[] words = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "j", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "Y", "Z", "X", "W", "Q" ,"1" , "2" , "3" , "4" , "5" , "6" , "7" , "8",
                             "9" , "0"};
            Random random = new Random();
            int adet = 12;
            string word = "";
            for (int i = 0; i < adet; i++)
            {
                int index = random.Next(1, 35);
                word += words[index];
            }
            return word;
        }
        public static void p2pDEBUG(this string value)
        {
            Debug.Print(value);
        }
        internal static string GetMacAddress()
        {
            ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMOS.Get();
            string MACAddress = String.Empty;
            foreach (ManagementObject objMO in objMOC)
            {
                if (MACAddress == String.Empty)
                {
                    MACAddress = objMO["MacAddress"].ToString();
                }
                objMO.Dispose();
            }
            MACAddress = MACAddress.Replace(":", "");
            if (MACAddress == "") { return "NOT"; } else { return MACAddress; }
        }
    }
}
