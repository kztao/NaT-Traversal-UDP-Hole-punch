using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2p
{
    public delegate void _Connected(Coming c);
    public delegate void _Requested(Coming c);
    public delegate void _Recieved(Recieved recieve);
    public delegate void _Ready();
    public delegate void _FirstSocketArrived();
    class Protocol
    {
        public static event _Connected onConnected;
        public static event _Requested onRequested;
        public static event _Recieved onRecieved;
        public static event _Ready onReady;
        public static event _FirstSocketArrived FirstArrived;
        public Protocol(Coming comi)
        {
            switch (comi.EventType)
            {
                case 1001:
                    FirstArrived();
                    break;

                case 1002:
                    FirstArrived();
                    break;

                case 1003:
                    FirstArrived();
                    break;

                case 1004:
                    onConnected(comi);
                    break;

                case 1005:
                    ("NaT SYMETRIC").p2pDEBUG();
                    break;

                case 1006:
                    ("1006 : " + comi.message).p2pDEBUG();
                    comi.messages = comi.message.Split(',');
                    onRequested(comi);
                    break;

                case 1007:
                    ("CLIENT DOES NOT EXISTS IN THE CURRENT CONTEXT").p2pDEBUG();

                    if (Generate.ShowMeInfoWithMessageBox)
                        System.Windows.Forms.MessageBox.Show("CLIENT DOES NOT EXISTS IN THE CURRENT CONTEXT");

                    if (Generate.ShowMeInfoWithConsole)
                        Console.WriteLine("CLIENT DOES NOT EXISTS IN THE CURRENT CONTEXT");

                    break;

                case 1008:
                    ("1008 : " + comi.message).p2pDEBUG();
                    comi.messages = comi.message.Split(',');
                    onRequested(comi);
                    break;

                case 3000:
                    ("3000").p2pDEBUG();
                    break;

                case 3001:
                    ("3001").p2pDEBUG();
                    Recieved r = new Recieved();
                    r.data = comi.data;
                    r.id = comi.AgentID;
                    onRecieved(r);
                    break;

                case 3002:
                    ("3002").p2pDEBUG();
                    break;
            }
        }
    }
}
