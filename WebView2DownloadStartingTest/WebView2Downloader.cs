using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace WebView2DownloadStartingTest
{
    public class WebView2Downloader : WebView2
    {
        public string? Url;
        private bool UseTaskDelay;
       
        public WebView2Downloader(string aUrl, bool aUseTaskDealy) : base()
        {
            Url = aUrl;
            UseTaskDelay = aUseTaskDealy;
            this.Loaded += WebView2Downloader_Loaded;
        }

        private void WebView2Downloader_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CoreWebView2InitializationCompleted += IPSurferTempBrowser_CoreWebView2InitializationCompleted;
            SetUp();
        }

        private async void SetUp()
        {
            await this.EnsureCoreWebView2Async(null);

            _ = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new Action(async () =>
            {
                if (UseTaskDelay) await Task.Delay(1000);
                if (Url != null) this.Source = new Uri(Url);
            }));
        }

        private void IPSurferTempBrowser_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            this.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            this.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            this.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;

        }

        private void CoreWebView2_DownloadStarting(object? sender, CoreWebView2DownloadStartingEventArgs e)
        {
            System.Diagnostics.Debug.Print("##############################   CoreWebView2_DownloadStarting   ###############################################");
            System.Diagnostics.Debug.Print("State: " + e.DownloadOperation.State.ToString());
            System.Diagnostics.Debug.Print("TotalBytesToReceive: " + e.DownloadOperation.TotalBytesToReceive.ToString());
            System.Diagnostics.Debug.Print("BytesReceived: " + e.DownloadOperation.BytesReceived.ToString());
            System.Diagnostics.Debug.Print("##############################   CoreWebView2_DownloadStarting   ###############################################");

            CoreWebView2Deferral deferral = e.GetDeferral();

            using (deferral)
            {
                // Hide the default download dialog.
                e.Handled = true;
                var op = e.DownloadOperation;
                FileDownloads fd = new(ref op);

            }
        }

        private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            Debug.Print(String.Concat("CoreWebView2_NavigationCompleted: " + this.Source.AbsoluteUri));
        }

        private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            Debug.Print(String.Concat("CoreWebView2_NavigationStarting: URI:  ", e.Uri));
        }

       
    }
}
