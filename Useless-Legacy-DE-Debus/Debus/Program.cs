using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Debus
{
    class Program
    {
        static bool AnotherInstanceRunning()
        {
            string exeName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            return Process.GetProcessesByName(exeName).Count() > 0;
        }

        static void Main(string[] args)
        {
            if (AnotherInstanceRunning())
            {
                MessageBox.Show("Another instance of this program is running!");
            }
            else
            {
                DebusServer server = new DebusServer(); // handles Base\\Core
                
                server.waitForExit.WaitOne();
            }
        }
    }
}