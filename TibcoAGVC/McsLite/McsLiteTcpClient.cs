using JxSystem.Core;
using JxSystem.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoAGVC
{
    public class McsLiteTcpClient : ITcpClientHandler
    {
        private readonly JxTcpClient tcpClient;
        private readonly TibcoEventManager tibcoEventManager;

        public McsLiteTcpClient(IPEndPoint remoteEndPoint, TibcoEventManager tibcoEventManager)
        {
            this.tibcoEventManager = tibcoEventManager;

            tcpClient = new JxTcpClient(remoteEndPoint, this, 1024, 1024, true);
            tcpClient.OnException += TcpClient_OnException;
        }

        private void TcpClient_OnException(object sender, Exception e)
        {
            LoggerEventDispatcher.Error($"McsLiteTcpClient | OnException | Exception: {e}");
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
            LoggerEventDispatcher.Error($"McsLiteTcpClient | NotifyConnectFailure: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyConnecting(JxTcpClient tcpClient, NotifySocketConnectingEventArgs e)
        {
            LoggerEventDispatcher.Info($"McsLiteTcpClient | NotifyConnecting: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyConnectSuccess(JxTcpClient tcpClient, NotifySocketConnectSuccessEventArgs e)
        {
            LoggerEventDispatcher.Info($"McsLiteTcpClient | NotifyConnectSuccess: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyReceiveServerDisconnected(JxTcpClient tcpClient, NotifyReceiveSocketDisconnectedEventArgs e)
        {
            LoggerEventDispatcher.Error($"McsLiteTcpClient | NotifyReceiveServerDisconnected: {e.RemoteAddress}:{e.RemotePort}");
        }

        public void NotifyReceiveServerMessage(JxTcpClient tcpClient, NotifyReceiveSocketMessageEventArgs e)
        {
            int headerLength = (sizeof(int) * 2);
            if (e.CurrentRemainLength >= headerLength)
            {
                if (tcpClient.Receive(headerLength, out byte[] headerBuffer))
                {
                    TibcoEventType tibcoEventType = (TibcoEventType)BitConverter.ToInt32(headerBuffer, 0);
                    int messageLength = BitConverter.ToInt32(headerBuffer, sizeof(int));
                    if (messageLength > 0)
                    {
                        if (tcpClient.Receive(messageLength, out byte[] messageBuffer))
                        {
                            string tibcoMessage = Encoding.Default.GetString(messageBuffer, 0, messageLength);
                            LoggerEventDispatcher.Info($"McsLiteTcpClient | NotifyReceiveServerMessage | TibcoEventType: {tibcoEventType} , TibcoMessage: {tibcoMessage}");

                            switch(tibcoEventType)
                            {
                                case TibcoEventType.JobPrepare:
                                    tibcoEventManager.AddEvent(new JobPrepareEvent(tibcoMessage));
                                    break;
                                case TibcoEventType.Stocker:
                                    tibcoEventManager.AddEvent(new StockerEvent(tibcoMessage));
                                    break;
                                case TibcoEventType.LoadPort:
                                    tibcoEventManager.AddEvent(new LoadPortEvent(tibcoMessage));
                                    break;
                                case TibcoEventType.QueryJobPrepare:
                                    tibcoEventManager.AddEvent(new QueryJobPrepareEvent(tibcoMessage));
                                    break;
                                case TibcoEventType.QueryStocker:
                                    tibcoEventManager.AddEvent(new QueryStockerEvent(tibcoMessage));
                                    break;
                                case TibcoEventType.QueryLoadPort:
                                    tibcoEventManager.AddEvent(new QueryLoadPortEvent(tibcoMessage));
                                    break;
                            }
                        }
                    }
                    else
                    {
                        LoggerEventDispatcher.Error($"McsLiteTcpClient | NotifyReceiveServerMessage | Receive '{tibcoEventType}' Empty Message");
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
