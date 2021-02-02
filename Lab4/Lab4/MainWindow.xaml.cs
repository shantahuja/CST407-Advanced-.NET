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
using System.Windows.Navigation;
using System.Windows.Shapes;
//using BingLibrary;
//using RedditLibrary;
using PluginBaseLibrary;
using System.Reflection;
using System.Windows.Controls.Primitives;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<PluginBase> plugins = AssemblyLoadInformation();

            listBoxPlugins.ItemsSource = plugins;
        }

        private List<PluginBase> AssemblyLoadInformation()
        {
            List<PluginBase> plugins = new List<PluginBase>();
            //plugins.Add(new BingPlugin());
            //plugins.Add(new RedditPlugin());

            string[] pluginFiles = Directory.GetFiles("Plugins", "*.dll");

            foreach (string pluginFile in pluginFiles)
            {
                Assembly assembly = Assembly.LoadFile(
                    System.IO.Path.Combine(Directory.GetCurrentDirectory(), pluginFile));

                PluginBase[] loadedPlugins = assembly.GetTypes()
                    .Where(x => typeof(PluginBase).IsAssignableFrom(x))
                    .Select(y => assembly.CreateInstance(y.FullName) as PluginBase).ToArray();

                plugins.AddRange(loadedPlugins);
            }

            return plugins;
        }

        private void listBoxPhotos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ImagePopout window = new ImagePopout(((sender as ListBox).SelectedItem as Photo).URL);
            window.Show();
        }
    }
}
