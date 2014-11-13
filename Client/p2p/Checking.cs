/*
 * Presented by © TiARPi® Softwaring & Development
 * All rights Reserved @TiARPi
 * Turap ÜLKÜ - Ozan ÜLKÜ - Zafer KETENCI - Burak SELÇUK
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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace p2p
{
    class Checking
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        public static string AssemblyPath = AppDomain.CurrentDomain.BaseDirectory + @"/p2pClient.ini";
        internal void CreateID(ref string _AppID, ref string _LicenceKey)
        {
            ("ID CHECKING").p2pDEBUG();
            if (File.Exists(AssemblyPath))
            {
                ("ID FILE EXISTS").p2pDEBUG();
                StringBuilder b = new StringBuilder(255);
                GetPrivateProfileString("Client", "id", "", b, 255, AssemblyPath);
                if (b.ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    ("ID IS EMPTY").p2pDEBUG();
                    _AppID = Generate.GenerateWord();
                    WritePrivateProfileString("Client", "id", _AppID, AssemblyPath);
                    ("ID CREATED : " + _AppID).p2pDEBUG();
                }
                else
                {
                    _AppID = b.ToString();
                    ("ID TAKED : " + _AppID).p2pDEBUG();
                }
            }
            else
            {
                ("ID FILE NOT EXISTS AND CREATED").p2pDEBUG();
                _AppID = Generate.GenerateWord();
                WritePrivateProfileString("Client", "id", _AppID, AssemblyPath);
                ("ID CREATED : " + _AppID).p2pDEBUG();
            }

            ("LICENCE KEY IS DEMO").p2pDEBUG();
            _LicenceKey = "DEMO";
        }
        internal static void CheckID(string AppID, ref string _AppID, ref string _LicenceKey)
        {
            CheckingID(AppID, ref _AppID);
            _LicenceKey = "DEMO";
            ("LICENCE KEY IS DEMO").p2pDEBUG();
        }
        internal static void CheckID(string AppID, ref string _AppID, string LicenceKey, ref string _LicenceKey)
        {
            CheckingID(AppID, ref _AppID);

            if (LicenceKey.Length < 12 || LicenceKey.Length > 12)
            {
                MessageBox.Show("Licence Key Must Be 12 Character");
                throw new Exception("Licence Key Must Be 12 Character");
            }
            else
            {
                if (_LicenceKey.Length < 1 || _LicenceKey.Equals("NOT", StringComparison.CurrentCultureIgnoreCase))
                {
                    ("LICENCE KEY IS RECORDED : " + _LicenceKey).p2pDEBUG();
                    _LicenceKey = LicenceKey;
                }
                else
                {
                    if (LicenceKey.Equals(_LicenceKey, StringComparison.CurrentCulture))
                    {
                        ("Licence Defineded Before : " + _LicenceKey).p2pDEBUG();
                    }
                    else
                    {
                        MessageBox.Show("Licence Key Defined Before");
                        throw new Exception("Licence KEy Defined Before");
                    }
                }
            }
        }
        private static void CheckingID(string AppID, ref string _AppID)
        {
            ("CHECKING ID").p2pDEBUG();
            if (_AppID.Length < 1 || _AppID.Equals("NOT", StringComparison.CurrentCultureIgnoreCase) || _AppID == "")
            {
                if (AppID.Length < 12 || AppID.Length > 12)
                {
                    MessageBox.Show("Your id must be 12 character");
                    throw new Exception("Your id must be 12 character");
                }
                else
                {
                    _AppID = AppID;
                    ("ID IS RECORDED : " + _AppID).p2pDEBUG();
                }
            }
            else
            {
                if (_AppID != AppID)
                {
                    MessageBox.Show("Id Defined Before");
                    throw new Exception("Id Defined Before");
                }
            }
        }
    }
}
