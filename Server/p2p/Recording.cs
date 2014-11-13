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
    class Recording
    {
        public clients Record(ref Coming c)
        {
            string[] _messages = c.message.Split(',');
            c.id = _messages[0];
            c.licence = _messages[1];
            c.macAddress = _messages[2];
            c.lep = _messages[3];

            clients _clients = new clients();
            _clients.Address = c.Address;
            _clients.EventType = c.EventType;
            _clients.id = c.id;
            _clients.lep = c.lep;
            _clients.licence = c.licence;
            _clients.macAddress = c.macAddress;
            _clients.message = c.message;
            _clients.port = c.port;
            _clients.Session = c.Session;
            _clients.cep = c.cep.ToString();

            return _clients;
        }
        public void RecordAgent()
        {
            #region CLIENT RECORD

            if (!File.Exists(Generate.xmlPath))
                return;

            XmlDocument xmld = new XmlDocument();
            try { xmld.Load(Generate.xmlPath); }
            catch { Thread.Sleep(50); xmld.Load(Generate.xmlPath); }
            foreach (XmlElement xmle in xmld.GetElementsByTagName("clients"))
            {
                Coming c = new Coming();
                c.id = xmle["id"].InnerText;
                c.lep = xmle["lep"].InnerText;
                c.macAddress = xmle["macAddress"].InnerText;
                c.licence = xmle["licence"].InnerText;
                c.Session = Convert.ToInt32(xmle["Session"].InnerText);
                c.port = Convert.ToInt32(xmle["port"].InnerText);
                c.Address = xmle["Address"].InnerText;
                c.cep = new IPEndPoint(IPAddress.Parse(c.Address), c.port);

                Agent.clients.Add(c.id, c);
            }
            try { xmld.Save(Generate.xmlPath); }
            catch { Thread.Sleep(50); xmld.Save(Generate.xmlPath); }


            Generate.isFirst = false;
            ("SERVER WORKING FIRST TIME & TRANSFERRED XML DATA TO Agent.clients").p2pDEBUG();
            #endregion
        }
    }
}
