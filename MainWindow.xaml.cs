using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MD5ER
{
    public partial class MainWindow : Window
    {
        private MD5CryptoServiceProvider MD5Hasher = new MD5CryptoServiceProvider();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task GetMD5()
        {
            if (!File.Exists(FilePath.Text)) return;

            this.MainGrid.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            this.TextHash.Text = string.Empty;

            Stream MD5fileStream = null;
            Stream MD5bufferedStream = null;

            string MD5Hash = string.Empty;
            string _path = FilePath.Text;
            await Task.Run(() =>
            {
                try
                {
                    MD5fileStream = new FileStream(_path, FileMode.Open);
                    MD5bufferedStream = new BufferedStream(MD5fileStream, 409600);

                    MD5Hash = BitConverter.ToString(MD5Hasher.ComputeHash(MD5bufferedStream)).Replace("-", String.Empty).ToLower();
                }
                catch { }
                finally
                {
                    if (MD5bufferedStream != null)
                        MD5bufferedStream.Dispose();

                    if (MD5fileStream != null)
                        MD5fileStream.Dispose();
                }
            });

            this.TextHash.Text = "MD5 Hash: " + MD5Hash;

            this.MainGrid.IsEnabled = true;
            this.Cursor = Cursors.Arrow;
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                FilePath.Text = dlg.FileName;
                await GetMD5();
            }
        }

        private async void GetHash_Click(object sender, RoutedEventArgs e)
        {
            await GetMD5();
        }
    }
}
