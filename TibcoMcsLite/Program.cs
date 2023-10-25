using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoMcsLite
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ITibcoAdapter tibcoAdapter = new TibcoAdapter();

                IPEndPoint tcpServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
                TibcoEventService tibcoEventService = new TibcoEventService(tcpServerEndPoint, tibcoAdapter);
                tibcoEventService.Start();

                while (true)
                {
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
