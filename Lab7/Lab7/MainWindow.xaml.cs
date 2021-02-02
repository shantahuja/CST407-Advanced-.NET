using FontAwesome.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

namespace Lab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public static class ExtensionMethods
    {
        public static string ToHexString(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
        }
    }


    public partial class MainWindow : Window
    {


        string md5value, sha1value, sha256value;


        public MainWindow()
        {
            InitializeComponent();
        }



        private Task DownloadFile(string url, string path, IProgress<DownloadProgressChangedEventArgs> progress = null)
        {
            using (WebClient client = new WebClient())
            {
                if (progress != null)
                {
                    client.DownloadProgressChanged += (o, e) =>
                    {
                        progress.Report(e);
                    };
                }    
                return client.DownloadFileTaskAsync(url, path);
            }
        }

        private Task<string> DownloadString(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadStringTaskAsync(url);
            }
        }

        private string ComputeHash(HashAlgorithm hashAlgorithm, Stream fileStream)
        {
            return hashAlgorithm.ComputeHash(fileStream).ToHexString();
        }


        private async void Button_ClickDownloadFiles(object sender, RoutedEventArgs e)
        {
            // const string iso = "http://releases.ubuntu.com/focal/ubuntu-20.04-desktop-amd64.iso";
            const string iso = "http://releases.ubuntu.com/focal/ubuntu-20.04-live-server-amd64.iso";
            const string md5 = "http://releases.ubuntu.com/focal/MD5SUMS";
            const string sha1 = "http://releases.ubuntu.com/focal/SHA1SUMS";
            const string sha256 = "http://releases.ubuntu.com/focal/SHA256SUMS";


            Progress<DownloadProgressChangedEventArgs> progress = new Progress<DownloadProgressChangedEventArgs>((percent) =>
            {
                prgBar.Value = percent.ProgressPercentage;
                txtPercent.Text = $"{percent.ProgressPercentage}% - {percent.BytesReceived / 1000000} MB / {percent.TotalBytesToReceive / 1000000} MB";
            });

            List<Task> tasks = new List<Task>()
            {
                DownloadString(md5),
                DownloadString(sha1),
                DownloadString(sha256)
            };

            // JEREMY: if you want to download the file, use this code.
            //Task isodownload = DownloadFile(iso, "ubuntu-20.04-live-server-amd64.iso", progress);

            Task checksums = Task.Factory.ContinueWhenAll(tasks.ToArray(), (t) => 
            {
                Task<string>[] tarray = t.Select(x => x as Task<string>).ToArray();

                md5value = tarray[0].Result;
                md5value = md5value.Split('*')[0];

                sha1value = tarray[1].Result;
                sha1value = sha1value.Split('*')[0];

                sha256value = tarray[2].Result;
                sha256value = sha256value.Split('*')[0];

            });

            List<Task> mainTasks = new List<Task>()
            {
                //isodownload,
                checksums
            };

            await Task.Factory.ContinueWhenAll(mainTasks.ToArray(), (t) => { });

        }

        private async void Button_ClickVerifyHashes(object sender, RoutedEventArgs e)
        {
            txtMd5.Text = md5value;
            txtSha1.Text = sha1value;
            txtSha256.Text = sha256value;

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "ISO Files|*.iso";
            bool isCancel = open.ShowDialog().Value;

            if (!isCancel)
            {
                MessageBox.Show("you didn't select an iso. please select an ISO!");
                return;
            }

            if (!File.Exists(open.FileName))
            {
                MessageBox.Show("the file doesn't exist what the heck!");
                return;
            }

            FileStream fs = new FileStream(open.FileName, FileMode.Open);

            string md5hash, sha1hash, sha256hash;
            Task<string> t1 = new Task<string>(() =>
            {
                fs.Position = 0;
                return ComputeHash(MD5.Create(), fs);
            });
            Task<string> t2 = new Task<string>(() =>
            {
                fs.Position = 0;
                return ComputeHash(SHA1.Create(), fs);
            });
            Task<string> t3 = new Task<string>(() =>
            {
                fs.Position = 0;
                return ComputeHash(SHA256.Create(), fs);
            });

            t1.Start();
            md5hash = await t1;
            SetIcon(md5Icon, md5value.Trim() == md5hash);

            t2.Start();
            sha1hash = await t2;
            SetIcon(sha1Icon, sha1value.Trim() == sha1hash);

            t3.Start();
            sha256hash = await t3;
            SetIcon(sha256Icon, sha256value.Trim() == sha256hash);

            fileIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Check;
        }   

        private void SetIcon(ImageAwesome icon, bool state)
        {
            if (state)
            {
                icon.Icon = FontAwesome.WPF.FontAwesomeIcon.Check;
                icon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                icon.Icon = FontAwesome.WPF.FontAwesomeIcon.TimesCircle;
                icon.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
