using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public delegate void ReceiveCallback(byte[] receivedBytes, EndPoint clientAddress);

    public class TcpServer
    {
        private readonly List<TcpComClient> tcpClients;
        private ReceiveCallback receiveCallback;

        public TcpServer()
        {
            this.tcpClients = new List<TcpComClient>();
        }

        public void StartListening(IPEndPoint localEndPoint, ReceiveCallback receiveCallback)
        {
            this.receiveCallback = receiveCallback;

            new Thread(() => this.StartTcpServer(localEndPoint)).Start();
        }

        public void Stop()
        {
            // Todo: create reference to all threads to get them killed
        }

        public bool Send(byte[] bytes, EndPoint clientAddress)
        {
            var tcpClient = this.tcpClients.FirstOrDefault(client => client.RemoteEndPoint == clientAddress);

            if (tcpClient != null)
            {
                if (tcpClient.Send(bytes) == false)
                {
                    this.tcpClients.Remove(tcpClient);
                    return false;
                }

                return true;
            }

            return false;
        }

        private void StartTcpServer(IPEndPoint localEndPoint)
        {
            var listener = new TcpListener(localEndPoint);

            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();

                new Thread(() => this.HandleTcpClient(client)).Start();
            }
        }

        private void HandleTcpClient(TcpClient client)
        {
            var tcpClient = new TcpComClient(client);

            tcpClient.StartListening(this.receiveCallback);

            this.tcpClients.Add(tcpClient);
        }

        public List<EndPoint> GetClientEndPoints()
        {
            return this.tcpClients.Select(x => x.RemoteEndPoint).ToList();
        }
    }
}
