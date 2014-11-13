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
    class DataHandle
    {
        public Coming HaCo(byte[] data,int size , Socket udp,EndPoint cep)
        {
            ("LRECIEVE").p2pDEBUG(); //TODO : LRECIEVE

            byte[] ET = new byte[4];
            byte[] message = new byte[size - 4];

            Array.Copy(data, 0, ET, 0, 4);
            Array.Copy(data, 4, message, 0, message.Length);

            int EventType = BitConverter.ToInt32(ET, 0);
            string _message = Encoding.UTF8.GetString(message);

            Coming c = new Coming();
            string[] messages = _message.Split(',');
            c.id = messages[0];
            c.EventType = EventType;
            c.message = _message;
            c.cep = cep;
            c._sock = udp;
            c.Address = ((IPEndPoint)cep).Address.ToString();
            c.port = ((IPEndPoint)cep).Port;

            return c;
        }
        public byte[] HaSe(int EventType, string message)
        {
            ("LSEND").p2pDEBUG(); //TODO : LSEND

            byte[] ET = BitConverter.GetBytes(EventType);
            byte[] msg = Encoding.UTF8.GetBytes(message);
            byte[] _tempData = new byte[ET.Length + msg.Length];

            Array.Copy(ET, 0, _tempData, 0, ET.Length);
            Array.Copy(msg, 0, _tempData, ET.Length, msg.Length);

            return _tempData;
        }
    }
}
