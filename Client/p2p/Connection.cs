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
    class Connection : Comminicate
    {
        public static int counterA = 0;

        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer timerc = new System.Timers.Timer();

        public Connection()
        {
            timer.Interval = p2p.ServerConnectionTimeOut;
            timer.Enabled = true;
            timer.Start();

            timerc.Interval = p2p.WaitingTimeForClientToClientCommunicate;
            timerc.Enabled = true;
            timerc.Start();
        }
        internal void FirstKeepAlive(Socket _udpa,EndPoint _sepa)
        {
            FirstMessage(_udpa, _sepa);

            timer.Elapsed += (Sender, e) =>
            {
                FirstMessage(_udpa, _sepa);
                counterA++;

                if (counterA > 6)
                {
                    if (Generate.ShowMeInfoWithMessageBox)
                        System.Windows.Forms.MessageBox.Show("SERVER DOES NOT EXISTS");

                    if (Generate.ShowMeInfoWithConsole)
                        Console.WriteLine("SEVER DOES NOT EXISTS");

                    timer.Interval = p2p.ServerConnectionTimeOut;
                }
            };
        }
        internal void KeepAgentsAlive()
        {
            DataHandle Handle = new DataHandle();
            byte[] _data = Handle.HaSe(3002, Generate._AppID, Encoding.UTF8.GetBytes(""));

            timerc.Elapsed += (Sender, e) =>
            {
                foreach (KeyValuePair<string, Agents> agent in Generate.AgentList)
                {
                    agent.Value._sock.BeginSendTo(_data, 0, _data.Length, SocketFlags.None, agent.Value.cep, new AsyncCallback((async) => { }), agent.Value._sock);
                    Thread.Sleep(10);
                }
            };
        }
    }
}
