/*
    Panel - Main Panel Class
    Copyright (C) 2017  Piotr 'MiXer' Mikstacki

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>
 */
using DE.PluginManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DE.Core.Shell;
using DE.PluginManagement.Types;
using Newtonsoft.Json;
using RadialMenu.Controls;
using DE.API;
namespace DE.UX
{
	/// <summary>
	/// Main Class for Panel
	/// </summary>
    /// //TODO END RADIAL MENU
    /// 
    [System.Serializable()]
	public partial class Panel : Window
	{
        public List<UIElement> PanelElements = new List<UIElement>();
        public List<PanelElement> panelElementOwners = new List<PanelElement>();
        public PanelPos PanelPosition = PanelPos.Bottom;
        public int PanelHeight = 50;
        public int PanelWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;

        public int PanelID;
        static int LastPanelID = 0;

        public Panel(PanelPos panelPos)
        {
            this.Show();
            InitializeComponent();
            PanelPosition = panelPos;
            SetPanelPos(PanelPosition);

            PanelID = LastPanelID++;
        }

		void b_Click(object sender, RoutedEventArgs e)
		{
			PanelSettings pn = new PanelSettings(this);
            pn.Show();
		}
		public void RemoveFromPanel(string Name)
		{
			MessageBox.Show(Name);
			
			int index = panelElementOwners.IndexOf(panelElementOwners.Find(x => x.GetType().ToString() == Name));
			MessageBox.Show(index.ToString());
			PanelElements.RemoveAt(index);
			panelElementOwners.RemoveAt(index);
			
			RefreshPanel();
			
		}
        public void AddToPanel(string Name)
        {
            foreach (DE.PluginManagement.Types.AvailablePlugin p in MainWindow.AvpanelPlugins.AvailablePlugins)
            {
                if (p.Type.ToString() == Name)
                {
                	
                    var instance = p.MakeInstance();
                    instance.Init(PanelID);
                    if(!PanelElements.Any(x => x.GetType() == instance.module.GetType()))
                    {
                    PanelElements.Add(instance.module);
                    panelElementOwners.Add(instance);
                    RefreshPanel();
                    return;
                    }
                    else{
                    	MessageBox.Show("Element "+instance.module.GetType().ToString()+" already exists in panel!");
                    }
                }
            }
            MessageBox.Show("Unable to find: " + Name);
        }
        private void RefreshPanel()
        {
            PanelElem.Children.Clear();
            foreach (UIElement n in PanelElements)
            {
                PanelElem.Children.Add(n);
            }
        }

        public void SetPanelPos(PanelPos panelPos)// Sets working area and checks if panels arent overlaying
        {
            switch (panelPos)
            {
                case PanelPos.Bottom:
                    if (!MainWindow.Panels.Any(x => x.PanelPosition == PanelPos.Bottom))
                    {
                        SpaceReserver.MakeNewDesktopArea(0, 0, 0, PanelHeight);
                        PanelPosition = PanelPos.Bottom;
                        this.Left = 0;
                        this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - PanelHeight;
                    }
                    break;
                case PanelPos.Top:
                    if (!MainWindow.Panels.Any(x => x.PanelPosition == PanelPos.Top))
                    {
                        SpaceReserver.MakeNewDesktopArea(0, PanelHeight, 0, 0);
                        PanelPosition = PanelPos.Top;
                        this.Left = 0;
                        this.Top = 0;
                    }
                    break;
                case PanelPos.Left:
                    if (!MainWindow.Panels.Any(x => x.PanelPosition == PanelPos.Left))
                    {
                        int tmp = PanelHeight;
                        PanelHeight = PanelWidth;
                        PanelWidth = tmp;
                        SpaceReserver.MakeNewDesktopArea(PanelHeight, 0, 0, 0);
                        PanelPosition = PanelPos.Left;
                        this.Left = 0;
                        this.Top = 0;
                    }
                    break;
                case PanelPos.Right:
                    if (!MainWindow.Panels.Any(x => x.PanelPosition == PanelPos.Right))
                    {
                        int tmp = PanelHeight;
                        PanelHeight = PanelWidth;
                        PanelWidth = tmp;
                        SpaceReserver.MakeNewDesktopArea(0, 0, PanelHeight, 0);
                        PanelPosition = PanelPos.Right;
                        this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - PanelWidth;
                        this.Top = 0;
                    }
                    break;
            }
            this.Width = PanelWidth;
            this.Height = PanelHeight;
            PanelElem.MaxWidth = PanelWidth;
            PanelElem.MaxHeight = PanelHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetPanelPos(PanelPosition);
        }

        private void clicked(object sender, RoutedEventArgs e)
        {
        }
		void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			PanelSettings pn = new PanelSettings(this);
            pn.Show();
		}
    }
    public enum PanelPos
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3
    }
}