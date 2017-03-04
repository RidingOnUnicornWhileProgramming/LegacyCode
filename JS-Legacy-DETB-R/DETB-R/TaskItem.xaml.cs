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
using System.Diagnostics;
using DE.WindowManagement;
using System.Drawing;
using System.Windows.Interop;
namespace DETB_R
{
    /// <summary>
    /// Interaction logic for TaskItem.xaml
    /// </summary>
    public partial class TaskItem : UserControl, IDisposable
    {
        Bitmap m;
        WinHandle thisproc;
        public TaskItem(WinHandle WinProcess, DLib.Base.Settings s)
        {
            InitializeComponent();
            thisproc = WinProcess;
            ImageBrush b = new ImageBrush();
            try
            {
                if (WinProcess.WindowIcon != null)
                {
                    m = WinProcess.WindowIcon.ToBitmap();
                    var d = Imaging.CreateBitmapSourceFromHBitmap(m.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    b.ImageSource = d;
                    TaskImage.Source = b.ImageSource;
                }
                WinProcess.TitleChanged += WinProcess_TitleChanged;
            }
            catch (Exception ex)
            {

            }
            TaskText.Content = WinProcess.Title;

            double height = s.WindowHeight;
            TaskText.Margin = new Thickness(height, 0, 0, 0);
            TaskImage.Width = height;
            TaskImage.Height = height;
            Height = height;

            DLib.WPF.StyleWPFLabel(TaskText, s);
        }

        public void Dispose()
        {
            GC.Collect(); // TODO it's EVIL, maybe remove
            GC.WaitForPendingFinalizers();
        }

        private void WinProcess_TitleChanged(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => TaskText.Content = thisproc.Title);
        }

        private void TaskClicked(object sender, MouseButtonEventArgs e)
        {
            thisproc.MaximizeMinimize();

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(20, 0, 255, 197));
        }

        private void TaskBarWidget_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(51, 0, 0, 0));
        }

    }

}
