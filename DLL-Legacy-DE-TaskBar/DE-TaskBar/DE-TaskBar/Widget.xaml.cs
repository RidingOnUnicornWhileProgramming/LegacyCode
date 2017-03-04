using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DE.WindowManagement;
using System.Windows;

namespace DETaskBar
{
    public partial class Widget : UserControl
    {
        private static int taskItemHeight;
        private static int taskItemWidth;

        private GUIItem AddNewTaskItem(WinHandle window)
        {
            var g = new GUIItem();
            Application.Current.Dispatcher.Invoke(() => {
                TaskItem tsk = new TaskItem(window, taskItemHeight);
                Tasks.Children.Add(tsk);
                g.Destroy = () => { Application.Current.Dispatcher.Invoke(() => Tasks.Children.Remove(tsk)); };
             });
            return g;
        }

        public Widget(int width, int height)
        {
            InitializeComponent();
            taskItemHeight = height;
            WindowManager wm = new WindowManager(AddNewTaskItem);
            // TODO add width & scrollbar
        }
        //TODO Somehow figure out how to scale TaskBar, that it will not come outside of screen xD
        public void ChangeTasksWidth(int x)
        {
        	foreach(UserControl uc in this.Tasks.Children)
        	{
        		uc.Width = x;
        	}
        }
        private void Tasks_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings not implemented yet!");
        }
    }
}
