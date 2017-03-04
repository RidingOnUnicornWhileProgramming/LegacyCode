
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using DE.Core.Shell;
using DE.API;

namespace DE.UX
{
	/// <summary>
	/// App Icon
	/// </summary>
	public partial class AppIcon : UserControl
	{
		public string Name, Path;
		public Bitmap icon;



        public AppIcon(string Name, Bitmap icon, string Path)
		{
			InitializeComponent();
			this.icon = icon;
			this.Name = Name;
			this.Path = Path;
			this.Loaded += Icon_Loaded;
		}

		void Icon_Loaded(object sender, RoutedEventArgs e)
		{
			appname.Content = Name;
			SetIcon();
		}
		void AppButton_Click(object sender, RoutedEventArgs e)
		{
			try{
			    Process.Start(Path);
                Apps.instance.Hide();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Can't run Appx!");
			}
		}
        void SetIcon()
        {
            try
            {
                icon.Save("ic.bmp");
                ImageSource s = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(),IntPtr.Zero,Int32Rect.Empty,BitmapSizeOptions.FromEmptyOptions());
                ic.Background = new ImageBrush(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
		public ImageSource imageSourceForImageControl(Bitmap yourBitmap)
		{
 		ImageSourceConverter c = new ImageSourceConverter();
 		return (ImageSource)c.ConvertFrom(yourBitmap);
		}

        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}