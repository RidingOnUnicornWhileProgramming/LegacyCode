using DebusClientLibrary;
using System.Threading;

namespace Example
{
    class ProgramAsyncLoading
    {
        static DebusClient client;

        static int waitingForDependencies;

        static void a()//Main(string[] args)
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

                waitingForDependencies = 0;

                reply = client.SendMessage("Base\\Core", "IsLoaded Base\\Hotkey"); // throws TimeoutException
                if (reply != "Loaded Base\\Hotkey")
                {
                    // Process.Start("Base\\Hotkey.exe")
                    waitingForDependencies++;
                }
                reply = client.SendMessage("Base\\Core", "IsLoaded AnotherDev\\AnotherDep"); // throws TimeoutException
                if (reply != "Loaded AnotherDev\\AnotherDep")
                {
                    // Process.Start("../../AnotherDev.exe");
                    waitingForDependencies++;
                }

                int timeout = 0;
                while (waitingForDependencies > 0)
                {
                    Thread.Sleep(10);
                    timeout++;
                    if (timeout > 300) { System.Console.WriteLine("Loading takes too long"); return; }
                }

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
            if (sender == "Base\\Core")
            {
                if (broadcast && (message == "Ready Base\\Hotkey" || message == "Ready AnotherDev\\AnotherDep"))
                {
                    waitingForDependencies--;
                }
            }
            else if (sender == "Base\\Hotkey")
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
