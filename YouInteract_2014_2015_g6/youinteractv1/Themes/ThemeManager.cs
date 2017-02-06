using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using YouInteractV1.LoaderData;

namespace YouInteractV1.Themes
{
    public static class ThemeManager
    {
        public static Theme CurrentTheme { get; set; }

        public static Theme DefaultTheme = new Theme();

        public static void StoreCurrentActiveTheme()
        {
            CurrentTheme = ManageStructs.GetActiveTheme();
        }

        public static string GetThemeOrDefaultPath()
        {

            var realPath = AppDomain.CurrentDomain.BaseDirectory + "/Themes/" + CurrentTheme.Background;
            return FileOrDirectoryExists(realPath) ? realPath : realPath.Replace(CurrentTheme.Name, DefaultTheme.Name);
        }

        internal static bool FileOrDirectoryExists(string name)
        {
            try
            {
                Application.GetResourceStream(new Uri(name, UriKind.Absolute));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
