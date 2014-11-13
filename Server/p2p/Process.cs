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
    class Process
    {
        private string xmlPath = AppDomain.CurrentDomain.BaseDirectory + @"Agents.xml";
        public Process(Coming comi)
        {
            switch (comi.EventType)
            {
                case 1000:
                    p1000(comi);
                    break;

                case 2000:
                    p2000(comi);
                    break;

                case 2001:
                    p2001(comi);
                    break;
            }
        }
        private void p1000(Coming c)
        {
            Recording recording = new Recording();
            clients _clients = recording.Record(ref c);
            DataHandle Handle = new DataHandle();

            if (Agent.clients.ContainsKey(c.id))
            {
                #region XML && CLIENT UPLOADED

                XmlDocument xmld = new XmlDocument();
                try { xmld.Load(xmlPath); }
                catch { Thread.Sleep(50); xmld.Load(xmlPath); }
                foreach (XmlElement xmle in xmld.GetElementsByTagName("clients"))
                {
                    if (xmle["id"].InnerText.Equals(_clients.id, StringComparison.CurrentCultureIgnoreCase))
                    {
                        xmle["id"].InnerText = _clients.id;
                        xmle["lep"].InnerText = _clients.lep;
                        xmle["macAddress"].InnerText = _clients.macAddress;
                        xmle["licence"].InnerText = _clients.licence;
                        xmle["Session"].InnerText = (Convert.ToInt32(xmle["Session"].InnerText) + 1).ToString();
                        xmle["port"].InnerText = _clients.port.ToString();
                        xmle["Address"].InnerText = _clients.Address;
                        xmle["cep"].InnerText = _clients.cep;
                    }
                }
                try { xmld.Save(xmlPath); }
                catch { Thread.Sleep(50); xmld.Save(xmlPath); }

                #endregion
                #region CLIENT CONNECTED AGAIN REPLACE PROPERTIES

                foreach (KeyValuePair<string, Coming> client in Agent.clients)
                {
                    if (client.Key.Equals(c.id, StringComparison.CurrentCultureIgnoreCase))
                    {

                        #region MAC ADDRESS DIFFERENT
                        if (!client.Value.macAddress.Equals("NOT"))
                        {
                            if (!client.Value.macAddress.Equals(c.macAddress))
                            {
                                byte[] data = Handle.HaSe(1001, "ID SAVED BEFORE FOR DIFFERENT CLIENT");
                                c._sock.BeginSendTo(data, 0, data.Length, SocketFlags.None, c.cep, new AsyncCallback((async) => { }), c._sock);
                                return;
                            }
                        }
                        #endregion
                        #region CLIENT UPDATE
                        client.Value.lep = c.lep;
                        client.Value._sock = c._sock;
                        client.Value.Address = c.Address;
                        client.Value.cep = c.cep;
                        client.Value.EventType = c.EventType;
                        client.Value.licence = c.licence;
                        client.Value.macAddress = c.macAddress;
                        client.Value.message = c.message;
                        client.Value.port = c.port;
                        #endregion
                        byte[] d = Handle.HaSe(1002, "CLIENT ALREADY IN THE LIST");
                        c._sock.BeginSendTo(d, 0, d.Length, SocketFlags.None, c.cep, new AsyncCallback((async) => { }), c._sock);
                    }
                }

                #endregion
            }
            else
            {
                Agent.clients.Add(c.id, c);
                if (!File.Exists(xmlPath))
                {
                    #region WRITE DOC
                    XmlTextWriter writer = new XmlTextWriter(xmlPath, Encoding.UTF8);
                    writer.Formatting = Formatting.Indented;
                    writer.WriteComment("Saved Clients list");
                    writer.WriteStartElement("Clients");
                    writer.WriteEndElement();
                    writer.Close();
                    #endregion
                }
                #region XML SAVE
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(xmlPath);
                }
                catch
                {
                    Thread.Sleep(50);
                    doc.Load(xmlPath);
                }

                XmlNode client = doc.DocumentElement;
                XmlElement element = doc.CreateElement("clients");

                XmlNode node1 = doc.CreateElement("id");
                node1.InnerText = _clients.id;
                element.AppendChild(node1);

                XmlNode node2 = doc.CreateElement("licence");
                node2.InnerText = _clients.licence;
                element.AppendChild(node2);

                XmlNode node3 = doc.CreateElement("macAddress");
                node3.InnerText = _clients.macAddress;
                element.AppendChild(node3);

                XmlNode node4 = doc.CreateElement("Address");
                node4.InnerText = _clients.Address;
                element.AppendChild(node4);

                XmlNode node5 = doc.CreateElement("port");
                node5.InnerText = _clients.port.ToString();
                element.AppendChild(node5);

                XmlNode node6 = doc.CreateElement("cep");
                node6.InnerText = _clients.cep;
                element.AppendChild(node6);

                XmlNode node7 = doc.CreateElement("lep");
                node7.InnerText = _clients.lep;
                element.AppendChild(node7);

                XmlNode node8 = doc.CreateElement("Session");
                node8.InnerText = "1";
                element.AppendChild(node8);

                XmlNode node9 = doc.CreateElement("message");
                node9.InnerText = _clients.message;
                element.AppendChild(node9);

                client.AppendChild(element);
                try
                {
                    doc.Save(xmlPath);
                }
                catch
                {
                    Thread.Sleep(50);
                    doc.Save(xmlPath);
                }
                #endregion
                byte[] data = Handle.HaSe(1003, "CLIENT RECORDED SUCCESFULY");
                c._sock.BeginSendTo(data, 0, data.Length, SocketFlags.None, c.cep, new AsyncCallback((async) => { }), c._sock);
            }
        }
        private void p2000(Coming comi)
        {
            foreach (KeyValuePair<string, Coming> clie in Agent.clients)
            {
                ("L1006").p2pDEBUG(); //TODO : L1006
                if (clie.Key == comi.id)
                {
                    if (comi.port == clie.Value.port)
                    {
                        //NaT CONIC

                        ("L1007").p2pDEBUG(); //TODO : L1006

                        var c = from n in Agent.clients where n.Value.id == comi.id select n;
                        foreach (KeyValuePair<string, Coming> a in c) { a.Value.NaTType = 1; }

                        DataHandle Handle = new DataHandle();
                        byte[] data = Handle.HaSe(1004, "NaT CONIC");
                        comi._sock.BeginSendTo(data, 0, data.Length, SocketFlags.None, comi.cep, new AsyncCallback((async) => { }), comi._sock);
                    }
                    else
                    {
                        //NaT SYMETRIC
                        ("L1008").p2pDEBUG(); //TODO : L1007
                        DataHandle Handle = new DataHandle();
                        byte[] data = Handle.HaSe(1005, "NaT SYMETRIC");
                        comi._sock.BeginSendTo(data, 0, data.Length, SocketFlags.None, comi.cep, new AsyncCallback((async) => { }), comi._sock);
                    }
                }
            }
        }
        private void p2001(Coming comi)
        {
            string message = comi.message;
            string[] messages = message.Split(',');

            if (messages[1].Length > 10)
            {
                if (Agent.clients.ContainsKey(messages[1]))
                {
                    foreach (KeyValuePair<string, Coming> client in Agent.clients)
                    {
                        if (client.Value.id.Equals(messages[1], StringComparison.CurrentCultureIgnoreCase))
                        {
                            DataHandle Handle = new DataHandle();
                            byte[] data = Handle.HaSe(1006, client.Value.id + "," + client.Value.NaTType + "," + client.Value.cep + "," + client.Value.Address + "," + client.Value.port);
                            comi._sock.BeginSendTo(data, 0, data.Length, SocketFlags.None, comi.cep, new AsyncCallback((async) => { }), comi._sock);

                            byte[] data2 = Handle.HaSe(1008, comi.id + "," + comi.NaTType + "," + comi.cep + "," + comi.Address + "," + comi.port);
                            client.Value._sock.BeginSendTo(data2, 0, data2.Length, SocketFlags.None, client.Value.cep, new AsyncCallback((async) => { }), client.Value._sock);
                        }
                    }
                }
                else
                {
                    DataHandle Handle = new DataHandle();
                    byte[] data = Handle.HaSe(1007, "");
                    comi._sock.BeginSendTo(data, 0, data.Length, SocketFlags.None, comi.cep, new AsyncCallback((async) => { }), comi._sock);
                }
            }
            else
            {
                //WAITING FOR REQ.
            }
        }
    }
}
