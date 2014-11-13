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
            ("L1000").p2pDEBUG();

            sepa = new IPEndPoint(IPAddress.Any, Generate.porta);
            sepb = new IPEndPoint(IPAddress.Any, Generate.portb);

            udpa = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpb = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            udpa.Bind(sepa);
            udpb.Bind(sepb);

            Reci(udpa, udpb);
        }
        public void Reci(Socket udpa, Socket udpb)
        {
            ("L1001").p2pDEBUG();
            Reca(udpa);
            Recb(udpb);
        }
        private void Reca(Socket udpa)
        {
            DataHandle Hand = new DataHandle();
            Protocol prot = new Protocol();

            Thread thread = new Thread(new ThreadStart(() =>
            {
                byte[] data = new byte[20480];
                cepa = new IPEndPoint(IPAddress.Any, 0);
                int size = udpa.ReceiveFrom(data, ref cepa);
                ("L1002").p2pDEBUG(); //TODO : L1002
                Thread threada = new Thread(new ThreadStart(() =>
                    {
                        Coming comi = Hand.HaCo(data, size ,udpa ,cepa);
                        prot.ComingData(comi);
                    }));
                threada.Start();
                Reca(udpa);
            }));
            thread.Start();
        }
        private void Recb(Socket udpb)
        {
            DataHandle Hand = new DataHandle();
            Protocol prot = new Protocol();

            Thread thread = new Thread(new ThreadStart(() =>
            {
                byte[] data = new byte[20480];
                cepb = new IPEndPoint(IPAddress.Any, 0);
                int size = udpb.ReceiveFrom(data, ref cepb);
                ("L1004").p2pDEBUG(); //TODO : L1004
                Thread threada = new Thread(new ThreadStart(() =>
                {
                    Coming comi = Hand.HaCo(data, size , udpb , cepb);
                    prot.ComingData(comi);
                }));
                threada.Start();
                Recb(udpb);
            }));
            thread.Start();
        }
    }
}
