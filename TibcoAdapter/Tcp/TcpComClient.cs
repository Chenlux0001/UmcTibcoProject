using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class TcpComClient
    {
        private readonly TcpClient tcpClient;

        private ReceiveCallback receiveCallback;

        public EndPoint RemoteEndPoint
        {
            get
            {
                return this.tcpClient.Client.RemoteEndPoint;
            }
        }

        public TcpComClient(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;

            this.tcpClient.ReceiveBufferSize = 1024 * 100;
            this.tcpClient.SendBufferSize = 1014 * 100;
        }

        public void StartListening(ReceiveCallback receiveCallback)
        {
            this.receiveCallback = receiveCallback;

            try
            {
                var networkStream = this.tcpClient.GetStream();

                var buffer = new byte[this.tcpClient.ReceiveBufferSize];

                networkStream.BeginRead(buffer, 0, buffer.Length, this.ReadCallback, buffer);
            }
            catch (Exception) { }
        }

        public bool Send(byte[] bytes)
        {
            try
            {
                this.tcpClient.Client.Send(bytes);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                var networkStream = this.tcpClient.GetStream();
                var buffer = result.AsyncState as byte[];

                if (buffer != null)
                {
                    if (buffer.Length > 0)
                    {
                        var bytes = new byte[buffer.Length];

                        for (var i = 0; i < buffer.Length; i++)
                            bytes[i] = buffer[i];

                        for (var i = 0; i < buffer.Length; i++)
                            buffer[i] = 0;

                        this.receiveCallback(bytes, this.tcpClient.Client.RemoteEndPoint);
                    }
                }

                networkStream.BeginRead(buffer, 0, buffer.Length, this.ReadCallback, buffer);
            }
            catch (Exception) { }
        }
    }
}
