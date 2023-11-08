using JxSystem.Core;
using JxSystem.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoMcsLite
{
    public class TibcoEventService : ITcpServerHandler
    {
        private Thread mainThread;
        private readonly JxTcpServer mcsLiteTcpServer;
        private readonly ITibcoAdapter tibcoAdapter;
        private readonly List<TibcoEvent> tibcoEventList;

        public TibcoEventService(IPEndPoint localEndPoint, ITibcoAdapter tibcoAdapter)
        {
            this.tibcoEventList = new List<TibcoEvent>();

            this.tibcoAdapter = tibcoAdapter;
            this.tibcoAdapter.OnListenLoadPortEvent += TibcoAdapter_OnListenLoadPortMessage;
            this.tibcoAdapter.OnListenStockerEvent += TibcoAdapter_OnListenOutStockerMessage;
            this.tibcoAdapter.OnListenJobPrepareEvent += TibcoAdapter_OnListenJobPrepareMessage;

            this.mcsLiteTcpServer = new JxTcpServer(localEndPoint, this, 1024, 1024);
            this.mcsLiteTcpServer.OnException += TcpServer_OnException;
        }

        private void TcpServer_OnException(object sender, Exception e)
        {
            Console.WriteLine($"TibcoMessageService | TcpServer | Exception: {e}");
        }

        public void Start()
        {
            if (mainThread == null)
            {
                mcsLiteTcpServer.StartListen();

                mainThread = new Thread(SendTibcoEventToClient) { IsBackground = true };
                mainThread.Start();
            }
        }

        private void SendTibcoEventToClient()
        {
            while(true)
            {
                try
                {
                    var firstEvent = tibcoEventList.ToList().FirstOrDefault();
                    if (firstEvent != null)
                    {
                        try
                        {
                            Console.WriteLine($"TibcoMessageService | SendTibcoMessageToClient | Send TibcoMessage Success: {firstEvent.Message}");

                            byte[] eventMessageBuffer = Encoding.Default.GetBytes(firstEvent.Message);
                            byte[] sendBuffer = new byte[sizeof(int) + sizeof(int) + eventMessageBuffer.Length];
                            // Event Type
                            Buffer.BlockCopy(BitConverter.GetBytes((int)firstEvent.EventType), 0, sendBuffer, 0, sizeof(int));
                            // Data Length
                            Buffer.BlockCopy(BitConverter.GetBytes(eventMessageBuffer.Length), 0, sendBuffer, sizeof(int), sizeof(int));
                            // Event Message
                            Buffer.BlockCopy(eventMessageBuffer, 0, sendBuffer, sizeof(int) * 2, eventMessageBuffer.Length);
                            
                            // Send Message
                            mcsLiteTcpServer.Broadcast(sendBuffer, 0, sendBuffer.Length);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"TibcoMessageService | SendTibcoMessageToClient | Send TibcoMessage Error: {ex}");
                        }

                        tibcoEventList.Remove(firstEvent);
                    }

                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"TibcoMessageService | SendTibcoMessageToClient | Send TibcoMessage Error: {ex}");

                    Thread.Sleep(3000);
                }
            }
        }

        private void TibcoAdapter_OnListenLoadPortMessage(object sender, string message)
        {
            tibcoEventList.Add(new LoadPortEvent(message));
        }

        private void TibcoAdapter_OnListenOutStockerMessage(object sender, string message)
        {
            tibcoEventList.Add(new OutStockerEvent(message));
        }

        private void TibcoAdapter_OnListenJobPrepareMessage(object sender, string message)
        {
            tibcoEventList.Add(new JobPrepareEvent(message));
        }

        public void NotifyAcceptClientConnect(JxTcpServer tcpServer, NotifyAcceptSocketEventArgs e)
        {

        }

        public void NotifyReceiveClientDisconnected(JxTcpServer tcpServer, NotifyReceiveSocketDisconnectedEventArgs e)
        {

        }

        public void NotifyReceiveClientMessage(JxTcpServer tcpServer, NotifyReceiveSocketMessageEventArgs e)
        {
            int headerLength = sizeof(int);
            if (e.CurrentRemainLength > headerLength)
            {
                if (tcpServer.Receive(e.Socket, headerLength, out byte[] receiveBuffer))
                {
                    int commandLength = BitConverter.ToInt32(receiveBuffer, 0);
                    if (commandLength > 0)
                    {
                        if (tcpServer.Receive(e.Socket, commandLength, out receiveBuffer))
                        {
                            string command = Encoding.Default.GetString(receiveBuffer, 0, commandLength);
                            if (command == "QueryLoadPortEvent")
                            {
                                string loadPortMessage = tibcoAdapter.QueryLoadPortEvent();
                                tibcoEventList.Add(new LoadPortEvent(loadPortMessage));
                            }
                            else if (command == "QueryOutStockerEvent")
                            {
                                string outStockerMessage = tibcoAdapter.QueryStockerEvent();
                                tibcoEventList.Add(new OutStockerEvent(outStockerMessage));
                            }
                            else if (command == "QueryJobPrepareEvent")
                            {
                                string jobPrepareMessage = tibcoAdapter.QueryJobPrepareEvent();
                                tibcoEventList.Add(new JobPrepareEvent(jobPrepareMessage));
                            }
                        }
                    }
                }
            }
        }
    }
}
