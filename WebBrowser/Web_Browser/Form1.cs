using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web_Browser
{
    public partial class IntelligenceDashboardLicensing : Form
    {
        String Url = string.Empty;
        public IntelligenceDashboardLicensing()
        {
            InitializeComponent();
            Url = "about : blank";
            myBrowser();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            myBrowser();
        }

        private void myBrowser()
        {
            if (toolStripComboBox1.Text != "")
                Url = toolStripComboBox1.Text;
            webBrowser1.Navigate(Url);
            webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(webpage_ProgressChanged);
            webBrowser1.DocumentTitleChanged += new EventHandler(webpage_DocumentTitleChanged);
            webBrowser1.StatusTextChanged += new EventHandler(webpage_StatusTextChanged);
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webpage_Navigated);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted);
        }

        private void webpage_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.CanGoBack) toolStripButton1.Enabled = true;
            else toolStripButton1.Enabled = false;

            if (webBrowser1.CanGoForward) toolStripButton2.Enabled = true;
            else toolStripButton2.Enabled = false;
            toolStripStatusLabel1.Text = "Done";
        }

        private void webpage_DocumentTitleChanged(object sender, EventArgs e)
        {
            this.Text = webBrowser1.DocumentTitle.ToString();
        }
        private void webpage_StatusTextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = webBrowser1.StatusText;
        }

        private void webpage_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Maximum = (int)e.MaximumProgress;
            toolStripProgressBar1.Value = ((int)e.CurrentProgress < 0 || (int)e.MaximumProgress < (int)e.CurrentProgress) ? (int)e.MaximumProgress : (int)e.CurrentProgress;
        }

        private void webpage_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            toolStripComboBox1.Text = webBrowser1.Url.ToString();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //In case windows is trying to shut down, don't hold the process up
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

             
                // Assume that X has been clicked and act accordingly.
                // Confirm user wants to close
                switch (MessageBox.Show(this, "Are you sure want to close.?", "Confirmation dialog ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //Stay on this form
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:

                    //var sURL = "http://localhost:50123/wwIntelligence/DashboardLicensing/ReleaseLicense?" + (new Date()).getTime();
                    //var xmlHttp = new XMLHttpRequest();
                    //xmlHttp.open("GET", sURL, true);
                    //xmlHttp.send();
                    this.deActivateLicense(); 

                    break;
                }
             
        }

        private void deActivateLicense()
        {
            var t=DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var d = DateTime.Now.Subtract(new DateTime(1970, 1, 1).ToUniversalTime()).TotalMilliseconds;

            TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1));
            var time = ts.TotalMilliseconds; // mili seconds
            // Create a request using a URL that can receive a post.  
            var uri= "http://localhost:50123/wwIntelligence/DashboardLicensing/ReleaseLicense?" + time;
            WebRequest request = WebRequest.Create(uri);
            // Set the Method property of the request to POST.  
            request.Method = "GET";

            // Create POST data and convert it to a byte array.  
            string postData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.  
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
            }

            // Close the response.  
            response.Close();
        }
        private void toolStripComboBox1_Enter(object sender, EventArgs e)
        {
           
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            myBrowser();
        }
    }
}
