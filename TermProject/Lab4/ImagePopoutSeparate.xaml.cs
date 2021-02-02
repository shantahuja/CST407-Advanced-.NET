using PluginBaseLibrary;
using System;
using System.Collections.Generic;
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

namespace Lab4
{
    /// <summary>
    /// Interaction logic for ImagePopoutSeparate.xaml
    /// </summary>
    public partial class ImagePopoutSeparate : Window
    {
        public ImagePopoutSeparate(Photo[] photoArray)
        {
            InitializeComponent();

            listBoxPhotoArray.ItemsSource = photoArray;
        }

        private void listBoxPhotos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ImagePopout window = new ImagePopout(((sender as ListBox).SelectedItem as Photo).URL);
            window.Show();
        }
    }
}
