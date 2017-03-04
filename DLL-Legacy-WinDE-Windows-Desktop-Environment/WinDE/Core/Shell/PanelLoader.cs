/*
    Panel Loader - Static class for loading Panel's config using Stream Serialization
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using DE.API;
using WinDE;
using DE.UX;
using System.IO;
using System.Windows;

namespace DE.Core.Shell
{
    static class PanelLoader
    {
    	static List<Panel> Panels = new List<Panel>();

        public static void SavePanels()
        {
            try
		    {
			
		    }
		    catch (Exception ex)
		    {
		    	MessageBox.Show("PanelLoader.SavePanels(): "+ex.Message);
		    }

        }

		static void RefreshPanels()
		{
			foreach(Panel panel in Panels)
			{
				panel.Show();
			}
		}

        public static void LoadPanels()
        {
			try
		    {
			using (Stream stream = File.Open("rsc\\config\\panels.bin", FileMode.Open))
			{
			    BinaryFormatter bin = new BinaryFormatter();

			    Panels = (List<Panel>)bin.Deserialize(stream);
			    RefreshPanels();
			}
		    }
		    catch(Exception ex)
		    {
		    	InitializeDefaults();
		    	
		    }

		    
        }
        static void InitializeDefaults()
        {
            Panel p = new UX.Panel(PanelPos.Bottom);

            p.AddToPanel("DE.UX.AppsWidget");
            p.AddToPanel("DETaskBar.TaskBar");
            p.AddToPanel("DE.UX.ClockWidget");

            Panels.Add(p);

        }
    }
}
