using DE.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DE.UX
{
    class AppsWidget : PanelElement
    {
        public string Description
        {
            get
            {
                return "Apps";
            }
        }

        public bool Enabled
        {
            get
            {
                return true;
            }

            set
            {

            }
        }

        public UIElement module
        {
            get
            {
                return new AppsLauncher();
            }
        }

        public string Name
        {
            get
            {
                return "Apps";
            }
        }

        public void Init(int panelID)
        {
            //throw new NotImplementedException();
        }
    }
}
