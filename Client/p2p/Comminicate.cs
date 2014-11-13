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
    public class Comminicate
    {
        EndPoint cepa;
        EndPoint cepb;
        public void Create(ref EndPoint sepa, ref EndPoint sepb, ref Socket udpa, ref Socket udpb)
        {
            sepa = new IPEndPoint(IPAddress.Parse(Generate.Address), Generate.PortA);
            sepb = new IPEndPoint(IPAddress.Parse(Generate.Address), Generate.PortB);

            udpa = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpb = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            udpa.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpb.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            udpa.Connect(sepa);
            udpb.Bind(udpa.LocalEndPoint);

            Reci(udpa, udpb);
        }
        public void Reci(Socket udpa, Socket udpb)
        {
            Reca(udpa);
            Recb(udpb);
        }
        private void Reca(Socket udpa)
        {
            DataHandle Hand = new DataHandle();
            Thread thread = new Thread(new ThreadStart(() =>
            {
                byte[] data = new byte[20480];
                cepa = new IPEndPoint(IPAddress.Any, 0);
                int size = udpa.ReceiveFrom(data, ref cepa);

                Thread threada = new Thread(new ThreadStart(() =>
                {
                    Hand.HaCo(data, size, udpa,cepa);
                }));
                threada.Start();
                Reca(udpa);
            }));
            thread.Start();
        }
        private void Recb(Socket udpb)
        {
            DataHandle Hand = new DataHandle();
            Thread thread = new Thread(new ThreadStart(() =>
            {
                byte[] data = new byte[20480];
                cepb = new IPEndPoint(IPAddress.Any, 0);
                int size = udpb.ReceiveFrom(data, ref cepb);

                Thread threada = new Thread(new ThreadStart(() =>
                {
                    Coming c = Hand.HaCo(data, size, udpb, cepb);
                    Protocol protocol = new Protocol(c);
                }));
                threada.Start();
                Recb(udpb);
            }));
            thread.Start();
        }
        internal void Rect(Socket udp)
        {
            DataHandle Hand = new DataHandle();
            Thread thread = new Thread(new ThreadStart(() =>
            {
                byte[] data = new byte[20480];
                cepb = new IPEndPoint(IPAddress.Any, 0);
                int size = udp.ReceiveFrom(data, ref cepb);
                Thread threada = new Thread(new ThreadStart(() =>
                {
                    Coming c = Hand.HaCo(data, size, udp, cepb);
                    Protocol protocol = new Protocol(c);
                }));
                threada.Start();
                Rect(udp);
            }));
            thread.Start();
        }
        internal void FirstMessage(Socket udp,EndPoint sep)
        {
            DataHandle Handle = new DataHandle();
            string _mess = Generate._AppID + "," + Generate._licenceKey + ","  + Generate.GetMacAddress() + "," + udp.LocalEndPoint.ToString();
            byte[] data = Handle.HaSe(1000,_mess);
            udp.BeginSendTo(data, 0, data.Length, SocketFlags.None, sep, new AsyncCallback((async) => { }), udp);
        }
        internal void SecondMessage(Socket udp, EndPoint sep)
        {
            DataHandle Handle = new DataHandle();
            string _mess = Generate._AppID + "," + Generate._licenceKey + "," + Generate.GetMacAddress() + "," + udp.LocalEndPoint.ToString();
            byte[] data = Handle.HaSe(2000, _mess);
            udp.BeginSendTo(data, 0, data.Length, SocketFlags.None, sep, new AsyncCallback((async) => { }), udp);
        }
        internal void Send(Socket udp,EndPoint sep, string message,int EventType)
        {
            DataHandle Handle = new DataHandle();
            byte[] data = Handle.HaSe(EventType, message);
            udp.BeginSendTo(data, 0, data.Length, SocketFlags.None, sep, new AsyncCallback((async) => { }), udp);
        }
    }
}
