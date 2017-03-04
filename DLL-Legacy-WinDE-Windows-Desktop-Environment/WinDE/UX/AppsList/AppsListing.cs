using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DE.UX
{
    static class AppsListing
    {
        public static List<xApp> Items = new List<xApp>();
        static AppsListing()
        {
            Items = GetItems();
            
        }
        static List<xApp> GetItems()
        {
            List<xApp> appslist = new List<xApp>();
            try
            {
                foreach (string d in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs"))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        if (!f.EndsWith(".ini"))
                        {
                            appslist.Add(new xApp(ExtractIcon.GetName(f), System.Drawing.Icon.ExtractAssociatedIcon(f).ToBitmap(), f));
                        }
                    }
                }
                foreach (string d in Directory.GetDirectories(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs"))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        if (!f.EndsWith(".ini"))
                        {
                            if (!appslist.Any(x => x.name == (ExtractIcon.GetName(f))))
                            {
                                appslist.Add(new xApp(ExtractIcon.GetName(f), System.Drawing.Icon.ExtractAssociatedIcon(f).ToBitmap(), f));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return appslist;
        }
    }
}
