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
using WinDE;

namespace DE.UX
{
    /// <summary>
    /// Interaction logic for AppsLauncher.xaml
    /// </summary>
    public partial class AppsLauncher : UserControl
    {
    	bool open;
        public AppsLauncher()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        	open =! open;
        	if(open)
        	{
        	Apps.instance.RefreshBlur();
            Apps.instance.Show();
            Apps.instance.OnShow();

        	}
        	else{
        		Apps.instance.OnHide();
        		Apps.instance.Hide();

        	}
        }
    }
}
