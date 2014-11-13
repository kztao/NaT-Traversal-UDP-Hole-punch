using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2p
{
    class Protocol
    {
        internal void ComingData(Coming comi)
        {
            switch (comi.EventType)
            {
                case 1000:
                    ("L1003").p2pDEBUG(); //TODO : L1003
                    Process proca = new Process(comi);
                    break;

                case 2000:
                    ("L1005").p2pDEBUG(); //TODO : L1005
                    Process procb = new Process(comi);
                    break;

                case 2001:
                    ("L1009").p2pDEBUG(); //TODO : L1005
                    Process procc = new Process(comi);
                    break;
            }
        }
    }
}
