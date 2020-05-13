using System;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Http;

namespace CameraClient
{
    public partial class MainForm
    {
        #region SOAP

        private void btnSend_Click(object sender, EventArgs e)
        {
            // does not work with HTTP 400
            try
            {
                WebRequest request = WebRequest.Create(tbAddress.Text);
                //set the properties
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Timeout = 30 * 1000;
                request.ContentLength = tbRequest.Text.Length;

                //open the pipe?
                Stream request_stream = request.GetRequestStream();
                //write the XML to the open pipe (e.g. stream)
                TextWriter tw = new StreamWriter(request_stream);
                tw.Write(tbRequest.Text);
                tw.Flush();
                //CLOSE THE PIPE !!! Very important or next step will time out!!!!
                request_stream.Close();
                
                //get the response from the webservice
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream r_stream = response.GetResponseStream();
                //convert it
                StreamReader response_stream = new
                    StreamReader(r_stream, System.Text.Encoding.GetEncoding("utf-8"));
                string sOutput = response_stream.ReadToEnd();

                //display it
                this.tbResponse.Text = sOutput;
                ShowStatusMessage("Done");
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                MessageBox.Show(ex.Message);
            }
        }

        /*
<s:Envelope xmlns:s="http://www.w3.org/2003/05/soap-envelope">
  <s:Header>
    <Action s:mustUnderstand="1" xmlns="http://schemas.microsoft.com/ws/2005/05/addressing/none">http://www.onvif.org/ver10/device/wsdl/GetNTP</Action>
  </s:Header>
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <GetNTP xmlns="http://www.onvif.org/ver10/device/wsdl" />
  </s:Body>
</s:Envelope>         
         */

        private void buttonRest_Click(object sender, EventArgs e)
        {
            // this works with HTTP 400
            // and does not work with HTTPS 
            try
            {
                using (HttpClient client = new HttpClient(tbAddress.Text))
                {
                    System.Xml.XmlDocument doc = new XmlDocument();
                    doc.LoadXml(tbRequest.Text);

                    System.Xml.Linq.XElement element = XElement.Parse(tbRequest.Text);
                    HttpContent content = HttpContentExtensions.Create(element);

                    using (HttpResponseMessage response = client.Post("/Dut.asmx", content))
                    {
                        response.Content.LoadIntoBuffer();
                        Console.WriteLine("  Status Code: {0}", response.StatusCode);
                        string cnt = response.Content.ReadAsString();
                        Console.WriteLine("  Content: {0}", cnt);
                        tbResponse.Text = cnt;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        #endregion

    }
}
