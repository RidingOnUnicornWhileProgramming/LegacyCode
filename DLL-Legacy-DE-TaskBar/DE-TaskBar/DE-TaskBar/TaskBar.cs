/*  DETaskbar - TaskBar for DE
    Copyright(C) 2017 Piotr "MiXer" Mikstacki & Paweł Zmarzły

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/>
    */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DETaskBar
{
    public class TaskBar : DE.API.PanelElement
    {
        public bool enabled;

        public TaskBar()
        {

        }

        public string Description { get { return "Simple DE Taskbar. Try to make your own!"; } }
        public bool Enabled { get; set; }

        private UIElement _module;
        public UIElement module { get { return _module; } }
        public string Name { get { return "TaskBar"; } }

        public void Init(int panel)
        {
            var cm = new DE.API.ConfigManager(panel);
            int width = 500;
            string w = cm.GetVariable("elWidth");
            if (!String.IsNullOrWhiteSpace(w)) Int32.TryParse(w, out width);
            int height = 50;
            string h = cm.GetVariable("height");
            if (!String.IsNullOrWhiteSpace(h)) Int32.TryParse(h, out height);
            if (_module != null) Console.Write("SINGLETON CALLED 2 TIMES????\n\n\n\n\n\\n\n\n\n\n\r\nNOT GOOD M8");
            _module = new Widget(width, height);
        }
    }
}