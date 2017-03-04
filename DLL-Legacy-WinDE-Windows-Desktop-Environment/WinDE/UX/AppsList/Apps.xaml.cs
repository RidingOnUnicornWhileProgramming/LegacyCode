/*
 * Created by SharpDevelop.
 * User: jatom
 * Date: 04.12.2016
 * Time: 18:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using DE.Core.Shell;
using System.Threading;

namespace DE.UX
{
	/// <summary>
	/// Interaction logic for Apps.xaml
	/// Also, Load Apps and icons.
	/// </summary>
	public partial class Apps : Window
	{
		public static Apps instance;

		public Apps()
		{
			InitializeComponent();
			this.Loaded += Apps_Loaded;
			instance = this;
			this.StateChanged += Apps_StateChanged;
		}
		public void RefreshBlur()
		{
			Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
			using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
				using (Graphics g = Graphics.FromImage(bitmap)) {
					g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
				}
				ImageSource s = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					                bitmap.GetHbitmap(),
					                IntPtr.Zero,
					                Int32Rect.Empty,
					                BitmapSizeOptions.FromEmptyOptions());
				bg.Background = new ImageBrush(s);
			}
		}
		
		void Apps_StateChanged(object sender, EventArgs e)
		{
			RefreshBlur();
		}
		void Show_Activated(object sender, RoutedEventArgs e)
		{
			Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
			using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
				using (Graphics g = Graphics.FromImage(bitmap)) {
					g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
				}
				ImageSource s = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					                bitmap.GetHbitmap(),
					                IntPtr.Zero,
					                Int32Rect.Empty,
					                BitmapSizeOptions.FromEmptyOptions());
				bg.Background = new ImageBrush(s);
			}
		}
		void Apps_Loaded(object sender, RoutedEventArgs e)
		{
			Thread t = new Thread(new ThreadStart(PlaceApps));
			t.Start();
			Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
			using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
				using (Graphics g = Graphics.FromImage(bitmap)) {
					g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
				}
				ImageSource s = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					                bitmap.GetHbitmap(),
					                IntPtr.Zero,
					                Int32Rect.Empty,
					                BitmapSizeOptions.FromEmptyOptions());
				bg.Background = new ImageBrush(s);
			}
			
		}
		void PlaceApps()
		{
			//Dispatcher.Invoke(new Action(AppsGrid.Items.Clear));
			foreach (xApp x in AppsListing.Items) {
				Dispatcher.Invoke(new Action(() => AppsGrid.Items.Add(new AppIcon(x.name, x.icon, x.Path))));
			}
			Dispatcher.Invoke(new Action(() => AppsGrid.Items.RemoveAt(0)));

		}
		/// <summary>
		/// Updates List on input...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void updatelist(object sender, TextChangedEventArgs e)
		{
			if (SearchBox.Text != " " || SearchBox.Text != "")
			{
				AppsGrid.Items.Clear();
				new Thread(new ThreadStart(() => {
				                           	
					List<xApp> CacheList = new List<xApp>();
					Dispatcher.Invoke(new Action( () => CacheList = AppsListing.Items.Where(x => x.name.ToLower().Contains(SearchBox.Text.ToLower())).ToList<xApp>()));
			
					foreach (xApp x in CacheList)
						Dispatcher.Invoke(new Action(() => AppsGrid.Items.Add(new AppIcon(x.name, x.icon, x.Path))));
				})).Start();
			} else 
			{
				new Thread(new ThreadStart(PlaceApps)).Start();
			}
		}

		private void Close(object sender, RoutedEventArgs e)
		{
			OnHide();
			
		}
		public void OnShow()
		{
			(FindResource("OnShow") as Storyboard).Begin(this);

		}
		public void OnHide()
		{
			(FindResource("OnHide") as Storyboard).Begin(this);
			this.Hide();
		}
	}
	public class xApp
	{
		public string name;
		public Bitmap icon;
		public string Path;
		public xApp(string name, Bitmap icon, string Path)
		{
			this.name = name;
			this.icon = icon;
			this.Path = Path;
		}
	}
}