
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace WinDE.Core.StyleManager
{
	/// <summary>
	/// Description of Stylemanager.
	/// </summary>
   
    //TODO Make Theme Validator, and create LoadLastUsedStyle
	public static class StyleManager
	{
		public static List<ShellStyle> Styles = new List<ShellStyle>();
        public static Dictionary<string, string> curStyle = new Dictionary<string, string>();
		static StyleManager()
		{
		List<string> fileList = new List<string>();
		Thread t = new Thread(() => 
		{
		    foreach (string d in Directory.GetDirectories(@"\rsc\Themes"))
        	{
            	foreach (string f in Directory.GetFiles(d, "*.style"))
            	{
            	    string extension = Path.GetExtension(f);
            	    if (extension != null && (extension.Equals(".style")))
            	    {
                        ShellStyle s = new ShellStyle();
                        s.name = Path.GetFileName(f);
                        s.path = f;
                        s.Hashid = MD5.Create(f).ToString();
                        Styles.Add(s);
            	    }
            	}
       		}
		});

		}
        public static void LoadStyle(string name)
        {
            curStyle = GetStyle(Styles.Find(x => x.name == name).path);
        }
        static Dictionary<string, string> GetStyle(string path)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parsedLine = s.Split(new char[] { '|' });
                dict.Add(parsedLine[0].Trim(), parsedLine[1].Trim());
            }
            return dict;
        }
	}
	[System.Serializable]
	public class ShellStyle
	{
		public string Hashid {get; set;}
		public string name {get; set;}
		public string path {get; set;}
	}
}
