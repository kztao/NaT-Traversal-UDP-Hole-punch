using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using p2p;
using System.Threading;

namespace Client
{
    class Program
    {
        static p2p.p2p _p2p = new p2p.p2p();
        static void Main(string[] args)
        {
            _p2p.onRecieve += _p2p_onRecieve;

            string sd = "B TAKED DATA FROM A";
            
            _p2p.StartSendToClient("X3FXG6Y5DONB", Encoding.UTF8.GetBytes(sd));

            sd = "C TAKED DATA FROM A";
            _p2p.StartSendToClient("X3FXG6Y5DONC", Encoding.UTF8.GetBytes(sd));

            while (true)
            {
                
            }
        }

        private static void _p2p_onRecieve(Recieved r)
        {
            Console.WriteLine(r.id);
            Console.WriteLine(Encoding.UTF8.GetString(r.data));

            _p2p.StartSendToClient(r.id, r.data);
        }
    }
}
