﻿/*
    Clock Widget, the internal plugin for DE
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
using DE.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DE.UX
{
    class ClockWidget : PanelElement

    {
        public string Description
        {
            get
            {
                return "Simple Clock";
            }
        }

        public bool Enabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public UIElement module
        {
            get
            {
                return new Clock();
            }
        }

        public string Name
        {
            get
            {
                return "Clock";
            }
        }

        public void Init(int panelID)
        {
            //throw new NotImplementedException();
        }
    }
}
