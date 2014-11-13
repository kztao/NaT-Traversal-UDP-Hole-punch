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

namespace p2p
{
    public class Coming
    {
        public string id { get; set; }
        public Socket _sock { get; set; }
        public string message { get; set; }
        public int EventType { get; set; }
        public EndPoint cep { get; set; }
        public string macAddress { get; set; }
        public string Address { get; set; }
        public int port { get; set; }
        public string lep { get; set; }
        public int Session { get; set; }
        public string licence { get; set; }
        public string[] messages { get; set; }
        public byte[] data { get; set; }
        public string AgentID { get; set; }
    }
    public class clients
    {
        public string id { get; set; }
        public string message { get; set; }
        public int EventType { get; set; }
        public string macAddress { get; set; }
        public string Address { get; set; }
        public int port { get; set; }
        public string lep { get; set; }
        public int Session { get; set; }
        public string licence { get; set; }
        public string cep { get; set; }
    }
}
