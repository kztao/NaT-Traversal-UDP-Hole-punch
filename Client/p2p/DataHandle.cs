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
        public byte[] HaSe(int EventType, string message)
        {
            byte[] ET = BitConverter.GetBytes(EventType);
            byte[] msg = Encoding.UTF8.GetBytes(message);
            byte[] _tempData = new byte[ET.Length + msg.Length];

            Array.Copy(ET, 0, _tempData, 0, ET.Length);
            Array.Copy(msg, 0, _tempData, ET.Length, msg.Length);

            return _tempData;
        }
        public Coming HaCo(byte[] data, int size, Socket udp, EndPoint cep)
        {
            byte[] ET = new byte[4];
            byte[] message = new byte[size - 4];

            Array.Copy(data, 0, ET, 0, 4);

            int EventType = BitConverter.ToInt32(ET, 0);
            Array.Copy(data, 4, message, 0, message.Length);

            if (EventType > 3000)
            {
                Coming co = HaCo2(data, size, udp, cep);
                return co;
            }
            else
            {
                string _message = Encoding.UTF8.GetString(message);

                Coming c = new Coming();
                c.EventType = EventType;
                c.message = _message;
                c.cep = cep;
                c._sock = udp;
                c.Address = ((IPEndPoint)cep).Address.ToString();
                c.port = ((IPEndPoint)cep).Port;
                c.data = data;

                return c;
            }
        }
        public Coming HaCo2(byte[] data, int size, Socket udp, EndPoint cep)
        {
            byte[] ET = new byte[4];
            byte[] DS = new byte[4];

            Array.Copy(data, 0, ET, 0, 4);
            Array.Copy(data, 4, DS, 0, 4);

            int messagesize = BitConverter.ToInt32(DS,0);

            byte[] message = new byte[messagesize];

            Array.Copy(data, 8, message,0, messagesize);

            int das = (size - (8 + messagesize));
            byte[] _data = new byte[das];

            Array.Copy(data, (8 + messagesize), _data,0, das);

            int EventType = BitConverter.ToInt32(ET, 0);
            string _message = Encoding.UTF8.GetString(message);

            Coming c = new Coming();
            c.EventType = EventType;
            c.message = _message;
            c.cep = cep;
            c._sock = udp;
            c.Address = ((IPEndPoint)cep).Address.ToString();
            c.port = ((IPEndPoint)cep).Port;
            c.data = _data;
            c.AgentID = _message;

            return c;
        }
        internal byte[] HaSe(int p1, string p2, byte[] data)
        {
            byte[] ET = BitConverter.GetBytes(p1);
            byte[] msg = Encoding.UTF8.GetBytes(p2);
            byte[] datasize = BitConverter.GetBytes(msg.Length);

            byte[] _tempData = new byte[ET.Length + msg.Length + datasize.Length + data.Length];

            Array.Copy(ET, 0, _tempData, 0, 4);
            Array.Copy(datasize, 0, _tempData, 4, 4);
            Array.Copy(msg, 0, _tempData, 8, msg.Length);
            Array.Copy(data, 0, _tempData, (8 + msg.Length),data.Length);

            return _tempData;
        }
    }
}
