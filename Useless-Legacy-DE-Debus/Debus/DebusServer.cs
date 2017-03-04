using System;
using System.Collections.Generic;
using System.Linq;
using SuperSocket.SocketBase;
using System.Windows;
using System.Threading;

namespace Debus
{
    // packet example: "Base\\Core IsLoaded Base\\Hotkey"
    // default port is 31458
    class DebusServer
    {
        public AutoResetEvent waitForExit = new AutoResetEvent(false);

        class Client
        {
            public AppSession session;
            public string name;
            public bool ready;
        }

        private void SendMessage(string sender, string receiver, string message)
        {
            Client[] s = Clients
                .Where(x => x.name == receiver).ToArray();
            if (s.Length != 1) Broadcast("Base\\Log", s.Length + " clients with name = " + receiver);
            if (s.Length > 0)
            {
                s[0].session.Send("M" + sender + " " + message);
            }
        }

        private void Broadcast(string sender, string message)
        {
            Clients.ForEach(x => x.session.Send("B" + sender + " " + message));
        }

        private const string BASE_CORE = "Base\\Core";
        private List<Client> Clients = new List<Client>();
        AppServer appServer;

        public DebusServer(int port = 31458)
        {
            appServer = new AppServer();
            if (!appServer.Setup(port))
            {
                MessageBox.Show("Failed to setup!");
                throw new Exception("Failed to setup!");
            }
            if (!appServer.Start())
            {
                MessageBox.Show("Failed to setup!");
                throw new Exception("Failed to setup!");
            }
            appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
            appServer.NewRequestReceived += AppServer_NewRequestReceived;
            appServer.SessionClosed += AppServer_SessionClosed;
        }

        private void AppServer_NewRequestReceived(AppSession session, SuperSocket.SocketBase.Protocol.StringRequestInfo requestInfo)
        {
            int ParametersAmount = requestInfo.Parameters.Length;
            Client sender = Clients
                    .Find(x => x.session == session);
            string senderName = sender.name;
            if (requestInfo.Key == BASE_CORE)
            {
                if (ParametersAmount == 2
                    && requestInfo.Parameters[0] == "Register")
                {
                    string pluginName = requestInfo.Parameters[1];
                    if (pluginName != "Base\\Core"
                        && Clients.FindIndex(x => x.name == pluginName) == -1)
                    {
                        Clients.Find(x => x.session == session).name = pluginName;
                        session.Send("M" + BASE_CORE + " Registered " + pluginName);
                    }
                    else
                    {
                        session.Send("M" + BASE_CORE + " RegistrationNameTaken");
                    }
                }
                
                else if (ParametersAmount == 2
                    && requestInfo.Parameters[0] == "IsLoaded")
                {
                    string pluginName = requestInfo.Parameters[1];
                    Client[] c = Clients
                        .Where(x => x.name == pluginName)
                        .ToArray();
                    string result = c.Length > 0 && c[0].ready
                        ? " Loaded " + pluginName
                        : " NotLoaded " + pluginName;
                    session.Send("M" + BASE_CORE + result);
                }

                else if (ParametersAmount == 1
                    && requestInfo.Parameters[0] == "Ready")
                {
                    if (senderName == null) session.Send("M" + BASE_CORE + " NotRegistered");
                    else
                    {
                        sender.ready = true;
                        Broadcast(BASE_CORE, "Ready " + senderName);
                        session.Send("M" + BASE_CORE + " Ready " + senderName);
                    }
                }

                else if (ParametersAmount == 1
                    && requestInfo.Parameters[0] == "Shutdown")
                {
                    if (senderName == null) session.Send("M" + BASE_CORE + " NotRegistered");
                    else
                    {
                        Broadcast("Base\\Log", "Shutdown triggered by " + senderName);
                        appServer.Stop();
                        waitForExit.Set();
                    }
                }
            }
            else if (senderName != null)
            {
                SendMessage(senderName, requestInfo.Key, requestInfo.Body);
            }
        }

        private void appServer_NewSessionConnected(AppSession session)
        {
            Client c = new Client();
            c.session = session;
            Clients.Add(c);
        }

        private void AppServer_SessionClosed(AppSession session, CloseReason value)
        {
            int f = Clients.FindIndex(x => x.session == session);
            if (f != -1)
            {
                Clients.RemoveAt(f);
            }
        }
    }
}
