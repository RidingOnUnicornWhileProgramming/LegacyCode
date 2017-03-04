namespace DLib
{
    class WPF
    {
        static public void StyleWPFWindow(System.Windows.Window w, Base.Settings s)
        {
            w.Width = s.WindowWidth;
            w.Height = s.WindowHeight;
            w.Top = s.WindowX;
            w.Left = s.WindowY;
            w.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(
                255, s.WindowBackgroundR, s.WindowBackgroundG, s.WindowBackgroundB));

            w.Topmost = true;
            w.WindowStyle = System.Windows.WindowStyle.None;
            w.ResizeMode = System.Windows.ResizeMode.NoResize;
            w.ShowInTaskbar = false;
        }

        static public void StyleWPFLabel(System.Windows.Controls.Label l, Base.Settings s)
        {
            l.FontSize = s.FontSize;
            l.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(
                255, s.FontColorR, s.FontColorG, s.FontColorB));
            l.FontFamily = s.FontFamilyFile != "system"
                    ? new System.Windows.Media.FontFamily(new System.Uri(s.FontFamilyFile), s.FontFamilyName)
                    : new System.Windows.Media.FontFamily(s.FontFamilyName);
        }
    }
}
