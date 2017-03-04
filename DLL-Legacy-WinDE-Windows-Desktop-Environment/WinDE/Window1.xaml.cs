/*
     DE - Desktop Environment for Windows Ecosystem
    Copyright (C) 2017  Piotr 'MiXer' Mikstacki & Paweł Zmarzły

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
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DE.Core.Shell;
using DE.PluginManagement;
using DE.PluginManagement.Types;
using DE.UX;

namespace DE
{
	/// <summary>
	/// This is the core constructor for all winde. Add Only must-have modules!
	/// </summary>
	public partial class MainWindow : Window
	{
        public static List<UX.Panel> Panels = new List<UX.Panel>();
        public static PanelPluginServices AvpanelPlugins;

        public MainWindow()
		{
			InitializeComponent();
            Apps s = new UX.Apps();
            AvpanelPlugins = new PanelPluginServices();

            AvpanelPlugins.FindPlugins();
            AvpanelPlugins.RegisterInternalPLugin(typeof(AppsWidget));
            AvpanelPlugins.RegisterInternalPLugin(typeof(ClockWidget));
            this.Hide();
            this.Closing += MainWindow_Closing; 
            PanelLoader.LoadPanels();

        }

		void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PanelLoader.SavePanels();
		}
    }
}