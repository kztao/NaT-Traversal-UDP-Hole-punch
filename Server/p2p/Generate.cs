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

namespace p2p
{
    public static class Generate
    {
        public static  string xmlPath = AppDomain.CurrentDomain.BaseDirectory + @"Agents.xml";
        public static bool isFirst = true;
        public static int porta;
        public static int portb;
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
    }
}
