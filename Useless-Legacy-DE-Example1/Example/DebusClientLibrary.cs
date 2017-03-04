// Debus Client Library
// v 0.4.0

using System;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace DebusClientLibrary
{
    class DebusClient
    {
        class Packet : IPackageInfo
        {
            public bool broadcast;
            public string sender;
            public string message;
        }

        class StandardFilter : TerminatorReceiveFilter<Packet>
        {
            public StandardFilter()
            : base(Encoding.UTF8.GetBytes("\r\n"))
            {
            }

            public override Packet ResolvePackage(IBufferStream bufferStream)
            {
                if (bufferStream.Length > Int32.MaxValue) throw new ArgumentOutOfRangeException("Param too long"); // should never happen
                string all = bufferStream.ReadString((int)bufferStream.Length, Encoding.UTF8);
                Packet p = new Packet();
                if (all[0] == 'B')
                {
                    p.broadcast = true;
                }
                else if (all[0] == 'M')
                {
                    p.broadcast = false;
                }
                else
                {
                    return null;
                }
                int i = all.IndexOf(' ');
                if (i == -1)
                {
                    return null;
                }
                else
                {
                    p.sender = all.Substring(1, i - 1);
                    p.message = all.Substring(i + 1, all.Length - i - 3);
                }
                return p;
            }
        }

        public delegate void Listener(string sender, string message, bool broadcast);
        public Listener AsyncListener;
        private EasyClient client;

        public DebusClient(long ip = 16777343, int port = 31458) // 127.0.0.1, 31458
        {
            client = new EasyClient();
            client.Initialize(new StandardFilter(), OnMessage);

            Task<bool> t = client.ConnectAsync(new IPEndPoint(ip, port));
            t.Wait();
            if (!t.Result) throw new TimeoutException("Cannot connect to the server");
        }

        public void SendMessageAsync(string receiver, string message)
        {
            client.Send(Encoding.UTF8.GetBytes(receiver + " " + message + "\r\n"));
        }

        private bool _waitingForMessage = false;
        private bool _waitingForMessage_broadcast = false;
        private string _waitingForMessage_from = "";
        private string _waitingForMessage_resultMessage = "";

        private void OnMessage(Packet p)
        {
            if (p != null)
            {
                if (_waitingForMessage && p.sender == _waitingForMessage_from && p.broadcast == _waitingForMessage_broadcast)
                {
                    _waitingForMessage_resultMessage = p.message;
                    _waitingForMessage = false;
                }
                else
                {
                    AsyncListener?.Invoke(p.sender, p.message, p.broadcast);
                }
            }
        }

        public string SendMessage(string receiver, string message, bool waitForBroadcast = false, int maxTime = 5000)
        {
            _waitingForMessage = true;
            _waitingForMessage_broadcast = waitForBroadcast;
            _waitingForMessage_from = receiver;

            SendMessageAsync(receiver, message);

            int time = 0;
            while (_waitingForMessage)
            {
                Thread.Sleep(50);
                time++;
                if (time > (maxTime / 50) + 1) throw new TimeoutException("Request timed out.");
            }

            return _waitingForMessage_resultMessage;
        }
    }
}
