using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SStart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            DLib.Base.Settings s;
            if (!DLib.Base.TryParse(args, out s))
            {
                MessageBox.Show("API version mismatch.");
                Application.Current.Shutdown();
            }

            DLib.WPF.StyleWPFWindow(this, s);

            fgColor.Fill = s.AppSettings["useFontColor"] == "1"
                ? new SolidColorBrush(Color.FromArgb(255, s.FontColorR, s.FontColorG, s.FontColorB))
                : new SolidColorBrush(Color.FromArgb(255, byte.Parse(s.AppSettings["colorR"]), byte.Parse(s.AppSettings["colorG"]), byte.Parse(s.AppSettings["colorB"])));
            bgColor.Fill = Background;

            string f = s.AppSettings["imageFile"];
            if (f[0] == '/') mask.ImageSource = new BitmapImage(new Uri(f.Substring(1)));
            else if (f[0] == '.') mask.ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + f.Substring(1)));
            else
            {
                MessageBox.Show("Incorrect setting: imageFile.");
                Application.Current.Shutdown();
            }
        }
    }
}
