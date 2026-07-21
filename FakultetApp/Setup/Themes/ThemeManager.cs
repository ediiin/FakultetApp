using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace Fakultet.Servisi.Helperi
{
    public static class ThemeManager
    {
        public static bool jelDarkMode { get; private set; } = false;

        public static void ToggleTheme()
        {
            SetTheme(!jelDarkMode);
        }

        public static void SetTheme(bool jelDark)
        {
            jelDarkMode = jelDark;

            // Vodeća kosa crta '/' osigurava da WPF traži od korijena projekta
            string pathTeme = jelDark ? "/Setup/Themes/DarkTheme.xaml" : "/Setup/Themes/LightTheme.xaml";

            try
            {
                var novaTema = new System.Windows.ResourceDictionary
                {
                    Source = new Uri(pathTeme, UriKind.RelativeOrAbsolute)
                };

                var appResources = System.Windows.Application.Current.Resources;

                // Pronalazimo ako već postoji učitana tema iz "Themes/" foldera i uklanjamo je
                var staraTema = appResources.MergedDictionaries
                    .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Themes/"));

                if (staraTema != null)
                {
                    appResources.MergedDictionaries.Remove(staraTema);
                }

                // Dodajemo novu temu
                appResources.MergedDictionaries.Add(novaTema);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Greška pri promjeni teme: {ex.Message}");
            }
        }
    }
}
