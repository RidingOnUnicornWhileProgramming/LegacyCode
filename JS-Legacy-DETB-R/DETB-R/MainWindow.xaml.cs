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

using DE.WindowManagement;

namespace DETB_R
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DLib.Base.Settings s;

        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (!DLib.Base.TryParse(args, out s))
            {
                MessageBox.Show("API version mismatch.");
                Application.Current.Shutdown();
            }

            DLib.WPF.StyleWPFWindow(this, s);

            WindowManager wm = new WindowManager(AddNewTaskItem);
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/279e29b8-e439-4d1a-9834-86ce78f6f64f/how-to-enable-horizontal-scrolling-with-mouse-wheel?forum=wpf
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();
            e.Handled = true;
        }

        private GUIItem AddNewTaskItem(WinHandle window)
        {
            var g = new GUIItem();
            Application.Current.Dispatcher.Invoke(() => {
                TaskItem tsk = new TaskItem(window, s);
                Tasks.Children.Add(tsk);
                g.Destroy = () => { Application.Current.Dispatcher.Invoke(() => Tasks.Children.Remove(tsk)); };
            });
            return g;
        }

        private void Tasks_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("C:\\nwjs-sdk\\nw.exe", "C:\\devenv\\config");
        }
    }
}
