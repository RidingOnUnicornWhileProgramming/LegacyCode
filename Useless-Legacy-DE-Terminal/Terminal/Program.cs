using DebusClientLibrary;

namespace Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            DebusClient client;
            try
            {
                client = new DebusClient();

                client.AsyncListener = OnReceiveMessage;
                while (true)
                {
                    string cmd = System.Console.ReadLine();
                    int i = cmd.IndexOf(' ');
                    if (i != -1)
                    {
                        string receiver = cmd.Substring(0, i);
                        string message = cmd.Substring(i + 1);
                        client.SendMessageAsync(receiver, message);
                    }
                }
            }
            catch (System.TimeoutException) {
                System.Console.WriteLine("Request timed out.");
                System.Console.ReadKey(false);
            }
        }

        static void OnReceiveMessage(string sender, string message, bool broadcast)
        {
            char b = broadcast ? 'B' : 'M';
            System.Console.WriteLine(":" + b + " " + sender + " " + message);
        }
    }
}
