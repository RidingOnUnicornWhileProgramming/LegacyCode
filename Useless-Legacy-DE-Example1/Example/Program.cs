using DebusClientLibrary;
using System.Threading;

namespace Example
{
    class Program
    {
        static DebusClient client;
        static void Main(string[] args)
        {
            try
            {
                client = new DebusClient(); // throws TimeoutException
                client.AsyncListener = OnReceiveMessage;

                string reply;
                reply = client.SendMessage("Base\\Core", "Register MyName\\Example1"); // throws TimeoutException
                if (reply != "Registered MyName\\Example1")
                {
                    System.Console.WriteLine(reply);
                    Thread.Sleep(453543);
                    return;
                    // or check if reply == "ERROR RegistrationNameTaken"
                }

                reply = client.SendMessage("Base\\Core", "IsLoaded Base\\Hotkey"); // throws TimeoutException
                if (reply != "Loaded Base\\Hotkey")
                {
                    // Process.Start("Base\\Hotkey.exe")
                }
                reply = client.SendMessage("Base\\Core", "IsLoaded AnotherDev\\AnotherDep"); // throws TimeoutException
                if (reply != "Loaded AnotherDev\\AnotherDep")
                {
                    // Process.Start("../../AnotherDev.exe");
                }
                while (reply != "Loaded Base\\Hotkey") { reply = client.SendMessage("Base\\Core", "IsLoaded Base\\Hotkey"); Thread.Sleep(10); } // throws TimeoutException
                while (reply != "Loaded AnotherDev\\AnotherDep") { reply = client.SendMessage("Base\\Core", "IsLoaded AnotherDev\\AnotherDep"); Thread.Sleep(10); } // throws TimeoutException

                // TODO add timeout to loops above

                client.SendMessageAsync("Base\\Hotkey", "Register Win F1"); // if you don't care much about result (reply will go to AsyncListener)

                reply = client.SendMessage("Base\\Core", "Ready"); // throws TimeoutException
                if (reply != "Ready MyName\\Example1")
                {
                    System.Console.WriteLine(reply);
                    Thread.Sleep(453543);
                    return;
                }
                Thread.Sleep(123456); // some random code
            }
            catch (System.TimeoutException) { /* code */ }
        }

        static void OnReceiveMessage(string sender, string message, bool broadcast)
        {
            if (sender == "Base\\Hotkey")
            {
                if (!broadcast && message == "Registered Win F1")
                {
                    // doNothing()
                }
                if (!broadcast && message == "Taken Win F1")
                {
                    // doSomething("error")
                }
                if (!broadcast && message == "Triggered Win F1")
                {
                    // doSomething()
                }
            }
        }
    }
}
