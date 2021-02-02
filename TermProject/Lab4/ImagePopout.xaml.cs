using PluginBaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace Lab4
{
    /// <summary>
    /// Interaction logic for ImagePopout.xaml
    /// </summary>
    public partial class ImagePopout : Window
    {
        public string URL { get; set; }
        public ImagePopout(string photo)
        {
            InitializeComponent();

            this.DataContext = this;

            URL = photo;
        }
    }
}
