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
    public class p2p : Comminicate
    {
        Socket _udpa;
        Socket _udpb;
        EndPoint _sepa;
        EndPoint _sepb;

        public p2p(int portA,int portB)
        {
            Generate.porta = portA;
            Generate.portb = portB;

            if (Generate.isFirst) { Recording record = new Recording(); record.RecordAgent(); }
            Create(ref _sepa,ref _sepb,ref _udpa,ref _udpb);
            
        }
    }
}
