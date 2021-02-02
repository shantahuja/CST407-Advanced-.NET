using Lab2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>   
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WCFServiceContract serviceContract = new WCFServiceContract();
        ServiceHost host = null;

        private const string fontDataPath = "C:/Users/shant/source/repos/Labs/Lab5/fontData.json";
        List<FavoriteFont> FavoriteFontList = new List<FavoriteFont>();

        public MainWindow()
        {
            InitializeComponent();


        }

        private void Button_ClickStartServer(object sender, RoutedEventArgs e)
        {
            if (host == null)
            {
                host = new ServiceHost(serviceContract);
            }

            host.Open();

            IsServerUp.Content = "Yes";
        }

        private void Button_ClickStopServer(object sender, RoutedEventArgs e)
        {
            host.Close();

            host = null;

            IsServerUp.Content = "No";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(fontDataPath);
            serviceContract.favoriteFonts = JsonConvert.DeserializeObject<List<FavoriteFont>>(json);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(serviceContract.favoriteFonts, Formatting.Indented);
            File.WriteAllText(fontDataPath, json);
        }
    }
}
