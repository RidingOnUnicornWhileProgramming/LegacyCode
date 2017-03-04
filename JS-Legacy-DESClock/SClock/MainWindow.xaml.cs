using System;
using System.Threading;
using System.Windows;

namespace SClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer t;

        public MainWindow()
        {
            InitializeComponent();

            DLib.Base.Settings s;
            string[] args = Environment.GetCommandLineArgs();
            if (!DLib.Base.TryParse(args, out s)) {
                MessageBox.Show("API version mismatch.");
                Application.Current.Shutdown();
            }

            DLib.WPF.StyleWPFWindow(this, s);
            DLib.WPF.StyleWPFLabel(Time, s);

            string HourFormat = s.AppSettings["format"];

            t = new Timer((object stateInfo) => {
                Dispatcher.Invoke(() => {
                    Time.Content = DateTime.Now.ToString(HourFormat);
                });
            }, null, 0, 1000);
        }
    }
}
