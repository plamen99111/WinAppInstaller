using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Collections;

namespace WinAppInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       ArrayList downloadLinks = new ArrayList();
        public MainWindow()
        {
            InitializeComponent();
            downloadLinks = downloadUrls();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(Object link in downloadLinks)
            {
                DownloadFile((string)link, "D:\\images\\skype.exe");
            }

            lblApplicationPath.Content = Process.GetCurrentProcess().MainModule.FileName;

        }

        private  async void DownloadFile(string downloadLink, string downloadedLocation)
        {
            using (WebClient wc = new WebClient())
            { 
                wc.DownloadProgressChanged += downloadOnProgressChanged;
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler((object sender, AsyncCompletedEventArgs e) => 
                {
                    if(e.Cancelled)
                    {
                        Console.WriteLine("File download cancelled.");
                    }
                    else if (e.Error != null)
                    {
                        Console.WriteLine(e.Error.ToString());
                    }
                    else
                    {
                        Process.Start(downloadedLocation);
                    }
                });
                //https://go.skype.com/windows.desktop.download

                wc.DownloadFileAsync(new Uri(downloadLink), downloadedLocation);
            }
        }

        private static async void DownloadFileCallback2(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("File download cancelled.");
            }
            if (e.Error != null)
            {
                Console.WriteLine(e.Error.ToString());
            }
            else
            {
                string programPath = "D:\\images\\skype.exe";
                Process.Start(programPath);
            }
        }

        private void downloadOnProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
        private ArrayList downloadUrls()
        {
            string executablePath = Process.GetCurrentProcess().MainModule.FileName;
            string lastLine = ReadLastLines(executablePath);
            string[] listofstrings = lastLine.Split(',');

            ArrayList linksList = new ArrayList();
            foreach (var substring in listofstrings)
            {
                linksList.Add(substring);
            }

            return linksList;
        }

        public static string ReadLastLines(string path)
        {
            var queue = new Queue<string>(1);

            foreach (var line in File.ReadLines(path))
            {
                if (queue.Count == 1)
                    queue.Dequeue();

                queue.Enqueue(line);
            }

            return queue.Dequeue();
        }
    }
}
