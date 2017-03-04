/*
    PanelSettings - Settings for each Panel
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
    along with this program. If not, see <http://www.gnu.org/licenses/>
 */
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using DE.API;
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
using System.Windows.Media.Imaging;
using DE.Core.Shell;
using System.Threading;

namespace DE.UX
{
    /// <summary>
    /// Settings for panel
    /// 
    /// </summary>
    //TODO: Add changing items order, saving panel would be a nice treat.
    public partial class PanelSettings : Window
    {
        DE.UX.Panel panel;
        public PanelSettings(DE.UX.Panel p)
        {
            InitializeComponent();
            panel = p;
           	LoadItems(p.panelElementOwners);
           	LoadPlugins();
        }

        private void LoadItems(List<PanelElement> panelel)
        {
            itemOrder.Items.Clear();
            foreach (PanelElement s in panelel)
            {
            	itemOrder.Items.Add(s);
            }
        }
        private void LoadPlugins()
        {
        	foreach(DE.PluginManagement.Types.AvailablePlugin s in MainWindow.AvpanelPlugins.AvailablePlugins)
        		pluginOrder.Items.Add(s.Type.ToString());
        }
        void MoveUp()
        {
            MoveItem(-1);
        }

        void MoveDown()
        {
            MoveItem(1);
        }

        public void MoveItem(int direction)
        {
            //For view
            if (itemOrder.SelectedItem == null || itemOrder.SelectedIndex < 0)
                return; 
            int newIndex = itemOrder.SelectedIndex + direction;
            if (newIndex < 0 || newIndex >= itemOrder.Items.Count)
                return;
            object selected = itemOrder.SelectedItem;
            itemOrder.Items.Remove(selected);
            itemOrder.Items.Insert(newIndex, selected);
            itemOrder.SelectedIndex = newIndex;
            //For Panel
            List<UIElement> panelcache = panel.PanelElements;
            panel.PanelElements.Clear();
            panel.panelElementOwners.Clear();
            foreach (PanelElement s in itemOrder.Items)
            {
            	panel.AddToPanel(s.GetType().ToString());
            }
           // PanelLoader.SavePanels();
        }

        private void UP_Click(object sender, RoutedEventArgs e)
        {
            MoveUp();
        }

        private void DOWN_Click(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
        	try{
            panel.AddToPanel(pluginOrder.SelectedItem.ToString());
            LoadItems(panel.panelElementOwners);
        	}
        	catch(Exception ex)
        	{
        	}
        }
		void rem_Click(object sender, RoutedEventArgs e)
		{
			panel.RemoveFromPanel(itemOrder.SelectedItem.ToString());
			LoadItems(panel.panelElementOwners);
		}
    }
}