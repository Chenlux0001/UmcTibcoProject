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
        private readonly JxConcurrentList<TibcoEvent> tibcoEventList;

        public TibcoEventService(IPEndPoint localEndPoint, ITibcoAdapter tibcoAdapter)
        {
            this.tibcoEventList = new JxConcurrentList<TibcoEvent>();

            this.tibcoAdapter = tibcoAdapter;
            this.tibcoAdapter.OnListenLoadPortEvent += TibcoAdapter_OnListenLoadPortMessage;
            this.tibcoAdapter.OnListenStockerEvent += TibcoAdapter_OnListenStockerMessage;
            this.tibcoAdapter.OnListenJobPrepareEvent += TibcoAdapter_OnListenJobPrepareMessage;

            this.mcsLiteTcpServer = new JxTcpServer(localEndPoint, this, 1024, 1024);
            this.mcsLiteTcpServer.OnException += TcpServer_OnException;
        }

        private void TcpServer_OnException(object sender, Exception e)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff} | TibcoMessageService | TcpServer | Exception: {e}");
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
                            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff} | TibcoMessageService | SendTibcoMessageToClient | Send TibcoMessage Success: {firstEvent.Message}");

                            byte[] eventMessageBuffer = Encoding.Default.GetBytes(firstEvent.Message);
                            byte[] sendBuffer = new byte[sizeof(int) + sizeof(int) + eventMessageBuffer.Length];
                            // Event Type
                            Buffer.BlockCopy(BitConverter.GetBytes((int)firstEvent.EventType), 0, sendBuffer, 0, sizeof(int));
                            // Data Length
                            Buffer.BlockCopy(BitConverter.GetBytes(eventMessageBuffer.Length), 0, sendBuffer, sizeof(int), sizeof(int));
                            // Event Message
                            if (eventMessageBuffer.Length > 0)
                                Buffer.BlockCopy(eventMessageBuffer, 0, sendBuffer, sizeof(int) * 2, eventMessageBuffer.Length);

                            // Send Message
                            mcsLiteTcpServer.Broadcast(sendBuffer, 0, sendBuffer.Length);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff} | TibcoMessageService | SendTibcoMessageToClient | Send TibcoMessage Error: {ex}");
                        }

                        tibcoEventList.Remove(firstEvent);

                        Console.WriteLine();
                    }

                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff} | TibcoMessageService | SendTibcoMessageToClient | Send TibcoMessage Error: {ex}");

                    Thread.Sleep(3000);
                }
            }
        }

        private void TibcoAdapter_OnListenLoadPortMessage(object sender, string message)
        {
            tibcoEventList.Add(new LoadPortEvent(message.Trim()));
        }

        private void TibcoAdapter_OnListenStockerMessage(object sender, string message)
        {
            tibcoEventList.Add(new StockerEvent(message.Trim()));
        }

        private void TibcoAdapter_OnListenJobPrepareMessage(object sender, string message)
        {
            tibcoEventList.Add(new JobPrepareEvent(message.Trim()));
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
                                string queryLoadPortMessage = tibcoAdapter.QueryLoadPortEvent();
                                tibcoEventList.Add(new QueryLoadPortEvent(queryLoadPortMessage));
                            }
                            else if (command == "QueryStockerEvent")
                            {
                                string queryStockerMessage = tibcoAdapter.QueryStockerEvent();
                                tibcoEventList.Add(new QueryStockerEvent(queryStockerMessage));
                            }
                            else if (command == "QueryJobPrepareEvent")
                            {
                                string queryJobPrepareMessage = tibcoAdapter.QueryJobPrepareEvent();
                                tibcoEventList.Add(new QueryJobPrepareEvent(queryJobPrepareMessage));
                            }
                        }
                    }
                }
            }
        }
    }
}
