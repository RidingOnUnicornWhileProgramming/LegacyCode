using System.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DE;
namespace DE.API
{
    public class ConfigManager
    {
        Dictionary<string, string> GLOBAL = new Dictionary<string, string>();
        Dictionary<string, string> PANEL = new Dictionary<string, string>();
        //static Dictionary<string, string> ITEM = new Dictionary<string, string>();
        int panelId;
        public ConfigManager(int panel)//, int item)
        {
            string GLOBAL_file = "rsc\\config\\main.json";
            if (File.Exists(GLOBAL_file)) GLOBAL = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GLOBAL_file));
            string PANEL_file = "rsc\\config\\panel" + panel.ToString() + ".json";
            if (File.Exists(PANEL_file)) PANEL = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(PANEL_file));
            //string ITEM_file = "rsc\\config\\item" + item + ".json";
            //ITEM = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("rsc\\config\\main.json"));
            panelId = panel;
        }

        public void SetVariable(string key, string value)
        {
            if(PANEL.Any(x => x.Key != key))
            {
            PANEL.Add(key, value);
            }
            else{
            	PANEL[key] = value;
            }
        }
        public void SaveVars()
        {
        	//this panel
        	string PANEL_file = "rsc\\config\\panel" + panelId.ToString() + ".json";
            string json = JsonConvert.SerializeObject(PANEL);
            File.WriteAllText(PANEL_file, json);
        }
        public string GetVariable(string key)
        {
            //if (ITEM.ContainsKey(key)) return ITEM[key];
            if (PANEL.ContainsKey(key)) return PANEL[key];
            if (GLOBAL.ContainsKey(key)) return GLOBAL[key];
            return null;
        }
    }
   //TODO: Consider this!
    

}
