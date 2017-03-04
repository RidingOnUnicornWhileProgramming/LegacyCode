using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DE.API;
namespace DE.UX
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        System.Timers.Timer t = new System.Timers.Timer(1000);
        public Clock()
        {
            InitializeComponent();
            time.Content = DateTime.Now;
            t.Elapsed += T_Elapsed;
            t.Start();
            this.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { time.Content = DateTime.Now.ToString("HH:mm:ss"); }));
        }
    }
}
