namespace DLib
{
    class Base
    {
        public struct Settings
        {
            public System.Collections.Generic.Dictionary<string, string> AppSettings;

            public int WindowWidth, WindowHeight, WindowX, WindowY;
            public byte WindowBackgroundR, WindowBackgroundG, WindowBackgroundB;
            public byte WindowBackgroundHoverR, WindowBackgroundHoverG, WindowBackgroundHoverB;

            public int FontSize;
            public byte FontColorR, FontColorG, FontColorB;
            public byte FontColorHoverR, FontColorHoverG, FontColorHoverB;
            public string FontFamilyFile, FontFamilyName;
        }

        /*string[] args = Environment.GetCommandLineArgs();
          string[] args = new string[] {
              "", // assembly name, doesn't matter
              "format=HH:mm:ss|anything=something", // AppSettings
              "40", "30", // width, height
              "100", "100", // Top, Left
              "0", "0", "0", // background RGB
              "30", "30", "30", // background hover RGB
              "10", // preffered font size
              "255", "255", "255", // text RGB
              "200", "200", "200", // text hover RGB
              "system", // font file (system = search in system dir)
              "Segoe UI" // font name (in this case W10 default)
          };
          Check out Project Settings->Debug->Command line arguments
            */
        static public bool TryParse(string[] args, out Settings set)
        {
            set = new Settings();

            if (args.Length < 21) return false;

            // TODO check what will crash if a param is negative number

            bool result = true;

            if (args[1].Length > 0)
            {
                var appParams = args[1].Split('|');
                set.AppSettings = new System.Collections.Generic.Dictionary<string, string>(appParams.Length);
                foreach (var pair in appParams)
                {
                    int i = pair.IndexOf('=');
                    if (i == -1) return false;
                    string key = pair.Substring(0, i);
                    string value = pair.Substring(i + 1);
                    set.AppSettings.Add(key, value);
                }
            }
            else
            {
                set.AppSettings = new System.Collections.Generic.Dictionary<string, string>(0);
            }

            result = result && int.TryParse(args[2], out set.WindowWidth);
            result = result && int.TryParse(args[3], out set.WindowHeight);
            result = result && int.TryParse(args[4], out set.WindowX);
            result = result && int.TryParse(args[5], out set.WindowY);

            result = result && byte.TryParse(args[6], out set.WindowBackgroundR);
            result = result && byte.TryParse(args[7], out set.WindowBackgroundG);
            result = result && byte.TryParse(args[8], out set.WindowBackgroundB);
            result = result && byte.TryParse(args[9], out set.WindowBackgroundHoverR);
            result = result && byte.TryParse(args[10], out set.WindowBackgroundHoverG);
            result = result && byte.TryParse(args[11], out set.WindowBackgroundHoverB);

            result = result && int.TryParse(args[12], out set.FontSize);

            result = result && byte.TryParse(args[13], out set.FontColorR);
            result = result && byte.TryParse(args[14], out set.FontColorG);
            result = result && byte.TryParse(args[15], out set.FontColorB);
            result = result && byte.TryParse(args[16], out set.FontColorHoverR);
            result = result && byte.TryParse(args[17], out set.FontColorHoverG);
            result = result && byte.TryParse(args[18], out set.FontColorHoverB);

            set.FontFamilyFile = args[19];
            set.FontFamilyName = args[20];

            return result;
        }
    }
}
