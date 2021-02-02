using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RsaEncDec rsaKeyPair = new RsaEncDec();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_ClickGenerateKeyPair(object sender, RoutedEventArgs e)
        {
            rsaKeyPair.GeneratePrivatePublicKey();
        }

        private void Button_ClickUploadKeyPair(object sender, RoutedEventArgs e)
        {
            rsaKeyPair.GetEmployeeID();
            EmployeeID.Content = rsaKeyPair.EmpID;
        }

        private void Button_ClickVerifyMissionDetails(object sender, RoutedEventArgs e)
        {
            rsaKeyPair.VerifyMissionDetails();
            DecryptedMessage.Content = rsaKeyPair.DecryptedMessageResponse;
        }

        private void Button_ClickUploadMissionDetails(object sender, RoutedEventArgs e)
        {
            rsaKeyPair.UploadMissionDetails();
            MissionResponse.Text = rsaKeyPair.MissionResponse;
        }
    }
}
