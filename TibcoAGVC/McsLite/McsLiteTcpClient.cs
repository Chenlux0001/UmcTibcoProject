using JxSystem.Core;
using JxSystem.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public class McsLiteTcpClient : ITcpClientHandler
    {
        private readonly JxTcpClient tcpClient;

        public McsLiteTcpClient(IPEndPoint remoteEndPoint)
        {
            tcpClient = new JxTcpClient(remoteEndPoint, this, 1024, 1024, true);
            tcpClient.OnException += TcpClient_OnException;
        }

        private void TcpClient_OnException(object sender, Exception e)
        {
            LoggerEventDispatcher.Error($"TibcoAdapterProxy | TcpClient | Exception: {e}");
        }

        public void Connect()
        {
            tcpClient.Connect();
        }

        public void Disconnect()
        {
            tcpClient.Disconnect();
        }

        public void NotifyConnectFailure(JxTcpClient tcpClient, NotifySocketConnectFailureEventArgs e)
        {
            LoggerEventDispatcher.Error($"TibcoAdapterProxy | NotifyConnectFailure: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyConnecting(JxTcpClient tcpClient, NotifySocketConnectingEventArgs e)
        {
            LoggerEventDispatcher.Info($"TibcoAdapterProxy | NotifyConnecting: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyConnectSuccess(JxTcpClient tcpClient, NotifySocketConnectSuccessEventArgs e)
        {
            LoggerEventDispatcher.Info($"TibcoAdapterProxy | NotifyConnectSuccess: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyReceiveServerDisconnected(JxTcpClient tcpClient, NotifyReceiveSocketDisconnectedEventArgs e)
        {
            LoggerEventDispatcher.Error($"TibcoAdapterProxy | NotifyReceiveServerDisconnected: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyReceiveServerMessage(JxTcpClient tcpClient, NotifyReceiveSocketMessageEventArgs e)
        {
            int headerLength = (sizeof(int) * 2);
            if (e.CurrentRemainLength > headerLength)
            {
                if (tcpClient.Receive(headerLength, out byte[] headerBuffer))
                {
                    int dataType = BitConverter.ToInt32(headerBuffer, 0);
                    int messageLength = BitConverter.ToInt32(headerBuffer, sizeof(int));
                    if (messageLength > 0)
                    {
                        // Data Type:
                        // 1: LoadPortMessage
                        // 2: OutStockerMessage
                        // 3: JobPrepareMessage
                        if (tcpClient.Receive(messageLength, out byte[] messageBuffer))
                        {
                            string tibcoMessage = Encoding.Default.GetString(messageBuffer, 0, messageLength);
                            LoggerEventDispatcher.Info($"TibcoAdapterProxy | NotifyReceiveServerMessage: {tibcoMessage}");
                        }
                    }
                }
            }
        }

        public void SendCommand(string command)
        {
            byte[] commandBuffer = Encoding.Default.GetBytes(command);
            byte[] sendBuffer = new byte[sizeof(int) + commandBuffer.Length];
            Buffer.BlockCopy(BitConverter.GetBytes(commandBuffer.Length), 0, sendBuffer, 0, sizeof(int));
            Buffer.BlockCopy(commandBuffer, 0, sendBuffer, sizeof(int), commandBuffer.Length);
            tcpClient.Send(sendBuffer, 0, sendBuffer.Length);
        }
    }
}
