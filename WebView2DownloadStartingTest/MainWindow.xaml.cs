using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WebView2DownloadStartingTest
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<WebView2Downloader> WebView2DownloadCollection
        {
            get { return _WebView2DownloadCollection ??= new(); }
            set { _WebView2DownloadCollection = value; }
        }

        private ObservableCollection<WebView2Downloader>? _WebView2DownloadCollection;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Browser.CoreWebView2InitializationCompleted += Browser_CoreWebView2InitializationCompleted;
            this.chk_NoDelay.IsChecked = true;
        }

        private void Browser_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            this.Browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            this.Browser.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;
        }


        //************
        // Target website lets you download the file in the same window this is just here to test this works.
        //************
        private void CoreWebView2_DownloadStarting(object? sender, CoreWebView2DownloadStartingEventArgs e)
        {
            CoreWebView2Deferral deferral = e.GetDeferral();

                using (deferral)
                {
                    e.Handled = true;
                var op = e.DownloadOperation;
                FileDownloads fd = new(ref op);

                }
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;

            //************
            // i just tested 2 methods for adding this webview2 to a window.
            //AddToContainer - simply adds the WebView2 as a child to a grid
            //AddToCollection - simply adds the webview2 to an itemscontrol (which is what I am using in my project)
            //************

            AddToContainer(e.Uri);
            //AddToCollection(e.Uri);
        }

        private void AddToContainer(string aUrl)
        {
            bool newBool = chk_WithDelay.IsChecked ?? false;
            WebView2Downloader webDownloader = new(aUrl, newBool);
            this.WV2Container.Children.Add(webDownloader);
        }

        private  void AddToCollection(string aUrl)
        {
            bool newBool = chk_WithDelay.IsChecked ?? false;

            WebView2Downloader webDownloader = new(aUrl, newBool);
            WebView2DownloadCollection.Add(webDownloader);
        }

        private void chk_NoDelay_Click(object sender, RoutedEventArgs e)
        {
            this.chk_WithDelay.IsChecked = false;
        }

        private void chk_WithDelay_Click(object sender, RoutedEventArgs e)
        {
            this.chk_NoDelay.IsChecked = false;
        }
    }
}
