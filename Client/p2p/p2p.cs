/*
 * Presented by © TiARPi® Softwaring & Development
 * All rights Reserved @TiARPi
 * Turap ÜLKÜ - Ozan ÜLKÜ - Zafer KETENCI - Burak SELÇUK
 * 
 * Cilent App
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
    public delegate void _Recieve(Recieved r);

    /// <summary>
    ///* TiARPi Sofware Solutions & Development Company
    /// * Ver. 1.0
    ///* 
    ///* 12.11.2014
    ///* 
    /// </summary>
    public class p2p
    {
        public event _Recieve onRecieve;

        Checking Checking = new Checking();
        Connection connection = new Connection();

        string _AppID = "NOT";
        string _licenceKey = "NOT";
        Socket _udpa;
        Socket _udpb;
        EndPoint _sepa;
        EndPoint _sepb;

        /// <summary>
        /// Client Connection Time Delay
        /// </summary>
        public static int WaitingTimeForClientRequest = 2000;
        /// <summary>
        /// Server Keep Alive Delay Time
        /// </summary>
        public static int ServerKeepAlive = 25000;
        /// <summary>
        /// Server Connection timer Out
        /// </summary>
        public static int ServerConnectionTimeOut = 3000;
        /// <summary>
        /// Keep Alive Client To Client Send Package Delay Time
        /// </summary>
        public static int WaitingTimeForClientToClientCommunicate = 12000;
        /// <summary>
        /// ID Will Generate Randomly | Generate._AppID
        /// </summary>
        public p2p()
        {
            Checking.CreateID(ref _AppID, ref _licenceKey);
            Generate._AppID = _AppID;
            Generate._licenceKey = _licenceKey;

            startConnect();
        }
        /// <summary>
        /// Pass Unic Agent ID
        /// </summary>
        /// <param name="AppID"></param>
        public p2p(string AppID)
        {
            Checking.CheckID(AppID, ref _AppID, ref _licenceKey);
            Generate._AppID = _AppID;
            Generate._licenceKey = _licenceKey;

            startConnect();
        }
        /// <summary>
        /// Pass Unic ID ANDLicence Key
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="LicenceKey"></param>
        public p2p(string AppID, string LicenceKey)
        {
            Checking.CheckID(AppID, ref _AppID, LicenceKey, ref _licenceKey);
            Generate._AppID = _AppID;
            Generate._licenceKey = _licenceKey;

            startConnect();
        }
        /// <summary>
        /// Define Your Own Server with Specific Unic ID
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="ServerAddress"></param>
        /// <param name="ServerFirdtPort"></param>
        /// <param name="ServerSecondPort"></param>
        public p2p(string AppID, string ServerAddress, int ServerFirdtPort, int ServerSecondPort)
        {

        }
        /// <summary>
        /// Define Your Own Server
        /// </summary>
        /// <param name="ServerAddress"></param>
        /// <param name="ServerFirstPort"></param>
        /// <param name="ServerSecondPort"></param>
        public p2p(string ServerAddress, int ServerFirstPort, int ServerSecondPort)
        {

        }
        void startConnect()
        {
            connection.Create(ref _sepa, ref _sepb, ref _udpa, ref _udpb);
            StartCommunicate();
        }
        void StartCommunicate()
        {
            connection.FirstKeepAlive(_udpa, _sepa);
            connection.KeepAgentsAlive();

            Protocol.onConnected += (c) => { Protocol_onConnected(c); };
            Protocol.onRequested += (c) => { protocol_onRequested(c); };
            Protocol.onRecieved += (recieve) => { ("onRecieved").p2pDEBUG(); Recieved r = new Recieved(); r = recieve; onRecieve(r); };
            Protocol.FirstArrived += () => { ("onFirstArrived").p2pDEBUG(); connection.SecondMessage(_udpb, _sepb); };
        }
        void Protocol_onConnected(Coming c)
        {
            Connection.counterA = 0;
            ServerConnectionTimeOut = ServerKeepAlive;
            ("onConnected").p2pDEBUG();
            Generate.isO = true;
        }
        void protocol_onRequested(Coming c)
        {
            ("onRequested").p2pDEBUG();
            StartConnectionWithClient(c);
        }
        void StartConnectionWithClient(Coming c)
        {
            Socket _udpc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _udpc.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpc.Bind(_udpb.LocalEndPoint);

            EndPoint cep = ((EndPoint)new IPEndPoint(IPAddress.Parse(c.messages[3]), Convert.ToInt32(c.messages[4])));

            connection.Rect(_udpc);

            DataHandle Handle = new DataHandle();
            byte[] data = Handle.HaSe(3000, Generate._AppID);

            _udpc.BeginSendTo(data, 0, data.Length, SocketFlags.None, cep, new AsyncCallback((async) => { }), _udpc);
            Thread.Sleep(new Random().Next(100, 300));
            _udpc.BeginSendTo(data, 0, data.Length, SocketFlags.None, cep, new AsyncCallback((async) => { }), _udpc);

            if (!Generate.AgentList.ContainsKey(c.messages[0]))
            {
                try
                {
                    Generate.AgentList.Add(c.messages[0], new Agents
                            {
                                id = c.messages[0],
                                cep = cep,
                                _sock = c._sock
                            });
                }
                catch
                {

                }
            }
        }
        public void StartSendToClient(string id, byte[] data)
        {
            while (!Generate.isO)
            {
                Thread.Sleep(50);
            }

            if (Generate.AgentList.ContainsKey(id))
            {
                KeyValuePair<string, Agents> a = Generate.AgentList.Single(t => t.Value.id == id);
                DataHandle Handle = new DataHandle();
                byte[] _data = Handle.HaSe(3001, Generate._AppID, data);
                a.Value._sock.BeginSendTo(_data, 0, _data.Length, SocketFlags.None, a.Value.cep, new AsyncCallback((async) => { }), a.Value._sock);
                return;
            }
            else
            {
                connection.Send(_udpb, _sepb, Generate._AppID + "," + id, 2001);

                Thread.Sleep(WaitingTimeForClientRequest);
                WaitingTimeForClientRequest += 500;
                if (WaitingTimeForClientRequest > 8000)
                {
                    WaitingTimeForClientRequest = 4000;
                }

                StartSendToClient(id, data);
            }
        }
    }
}
