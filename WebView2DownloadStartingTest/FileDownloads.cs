using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebView2DownloadStartingTest
{
    public class FileDownloads
    {
        public CoreWebView2DownloadOperation Operation { get; set; }

        internal FileDownloads(ref CoreWebView2DownloadOperation anOperation)
        {
            Operation = anOperation;
            Operation.BytesReceivedChanged += Operation_BytesReceivedChanged;
            Operation.StateChanged += Operation_StateChanged;
        }

        private void Operation_StateChanged(object? sender, object e)
        {
            System.Diagnostics.Debug.Print("##############################   DownloadOperation_BytesReceivedChanged   ###############################################");
            System.Diagnostics.Debug.Print("State: " + Operation.State.ToString());
            System.Diagnostics.Debug.Print("##############################   DownloadOperation_BytesReceivedChanged   ###############################################");
        }

        private void Operation_BytesReceivedChanged(object? sender, object e)
        {
            System.Diagnostics.Debug.Print("##############################   DownloadOperation_StateChanged   ###############################################");
            System.Diagnostics.Debug.Print("TotalBytesToReceive: " + Operation.TotalBytesToReceive.ToString());
            System.Diagnostics.Debug.Print("##############################   DownloadOperation_StateChanged   ###############################################");
        }
    }
}
