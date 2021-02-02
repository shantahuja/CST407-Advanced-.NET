using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private AppSettings GetAppSettings()
        {
            return (this.DataContext as MainVM).Settings;
        }

        //private void Button_SaveClick(object sender, RoutedEventArgs e)
        //{

        //}

        //private void Button_LoadClick(object sender, RoutedEventArgs e)
        //{
        //    AppSettings settings;
        //    string json;

        //    json = File.ReadAllText((this.DataContext as AppSettings).AppSettingsPath);

        //    settings = JsonConvert.DeserializeObject<AppSettings>(json);

        //    this.DataContext = settings;
        //}

        private void Button_OpenFontInformationClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "JSON files|*.json";

            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                string filePath = dlg.FileName;

                (this.DataContext as MainVM).Settings.LastSavedPath = filePath;

                (this.DataContext as MainVM).Fonts.LoadSerializedFonts(filePath);
            }
        }

        private void Button_SaveFontInformationClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "JSON files|*.json";

            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                string filePath = dlg.FileName;

                FontList fontsToSerialize;

                fontsToSerialize = (this.DataContext as MainVM).Fonts;

                fontsToSerialize.GenerateFontsToSerialize(filePath);

                AppSettings appSettings = (this.DataContext as MainVM).Settings;

                appSettings.LastSavedPath = filePath;
            }            
        }

        private void Button_InitFontInformationClick(object sender, RoutedEventArgs e)
        {
            FontList fontsToInit = (this.DataContext as MainVM).Fonts;

            fontsToInit.InitializeFonts();

            fontsToInit.FilterFonts();
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            AppSettings settings = GetAppSettings();

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            File.WriteAllText(settings.AppSettingsPath, json);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppSettings settings;
            string json;

            json = File.ReadAllText(GetAppSettings().AppSettingsPath);

            settings = JsonConvert.DeserializeObject<AppSettings>(json);

            (this.DataContext as MainVM).Settings = settings;

            if (!string.IsNullOrWhiteSpace(settings.LastSavedPath))
            {
                (this.DataContext as MainVM).Fonts.LoadSerializedFonts(settings.LastSavedPath);
            }
        }
    }
}
