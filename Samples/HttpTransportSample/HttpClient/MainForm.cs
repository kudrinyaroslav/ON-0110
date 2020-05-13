using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ServiceModel;
using TestTool.HttpTransport;

namespace HttpClient
{
    public partial class MainForm : Form
    {
        private TrafficListener _listener;
        public MainForm()
        {
            InitializeComponent();
            _listener = new TrafficListener();
            _listener.OnRequest += new Action<string>(_listener_OnRequest);
            _listener.OnResponse += new Action<string>(_listener_OnResponse);
        }

        void _listener_OnResponse(string data)
        {
            BeginInvoke(new Action(() => {
                                             WriteLine("\nResponse: "); 
                                             WriteLine(data); 
                                            }));
        }

        void _listener_OnRequest(string data)
        {
            BeginInvoke(new Action(() =>
            {
                WriteLine("\nRequest: ");
                WriteLine(data);
            }));
        }

        void WriteLine(string message)
        {
            tbConsole.AppendText(string.Format("{0} {1}", message, Environment.NewLine));
            tbConsole.ScrollToCaret();
        }

        void WriteLine(string message, System.Drawing.Color color)
        {
            int start = tbConsole.Text.Length;
            WriteLine(message);
            int end = tbConsole.Text.Length;
            HighLite(start, end, color);
            
        }

        private void HighLite(int start, int length, System.Drawing.Color color)
        {
            tbConsole.Select(start, length);
            tbConsole.SelectionColor = color;
            tbConsole.Invalidate();
            tbConsole.Select(start, 0);
        }

        private void ReportError(Exception ex)
        {
            tbConsole.AppendText(Environment.NewLine);
            MessageBox.Show(ex.StackTrace);

            StringBuilder sb = new StringBuilder(ex.Message);
            Exception innerException = ex.InnerException;
            string offset = "   ";
            while(innerException != null)
            {
                sb.AppendFormat("{0}{1}{2}", Environment.NewLine, offset, innerException.Message);
                offset = "   " + offset;
                innerException = innerException.InnerException;
            }
            string report = sb.ToString();
            WriteLine(report);
            int selStart = tbConsole.Text.Length - report.Length - 2;
            HighLite(selStart, report.Length, System.Drawing.Color.DeepPink);
            tbConsole.ScrollToCaret();
        }


        private void btnBackgroundOperation_Click(object sender, EventArgs e)
        {
            RunInBackground(GetHostname);
        }

        void RunInBackground(Action action)
        {
            _listener.Reset();
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            System.Threading.Thread thread = new Thread(new ThreadStart(action));
            thread.Start();

        }

        void GetHostname()
        {
            try
            {
                DeviceClient client = new DeviceClient(new HttpBinding(new IChannelController[] { _listener }),
                                                       new EndpointAddress(tbAddress.Text));

                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, int.Parse(tbTimeout.Text));

                HostnameInformation info = client.GetHostname();
            }
            catch (Exception exc)
            {
                BeginInvoke(new Action(() => { ReportError(exc);}));
            }
            finally
            {
                StopAnimation();
            }
        }

        void GetNTP()
        {
            DeviceClient client = null;
            try
            {
                client = new DeviceClient(new HttpBinding(new IChannelController[] { _listener }),
                                       new EndpointAddress(tbAddress.Text));

                try
                {
                    NTPInformation ntp = client.GetNTP();
                }
                catch (FaultException exc)
                {
                    BeginInvoke(new Action(() =>
                    {
                        WriteLine("Fault!", System.Drawing.Color.BlueViolet);
                        WriteLine(exc.Code.Name, System.Drawing.Color.Green);
                        System.ServiceModel.FaultCode code = exc.Code.SubCode;
                        string offset = "  ";
                        while (code != null)
                        {
                            WriteLine(string.Format("{0}Subcode: {1}", offset, code.Name), System.Drawing.Color.Green);
                            offset = "  " + offset;
                            code = code.SubCode;
                        }
                    }));

                }

                BeginInvoke(new Action( ()=> WriteLine("OK")));
            }
            catch (Exception exc)
            {
                if (client != null)
                {
                    client.Abort();
                }
                BeginInvoke(new Action( ()=> ReportError(exc)));
                //throw;
            }
            finally
            {
                StopAnimation();
            }
        }

        void GetHostname2()
        {
            try
            {
                DeviceClient client = new DeviceClient(new HttpBinding(new IChannelController[] { _listener }),
                                                       new EndpointAddress(tbAddress.Text));

                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, int.Parse(tbTimeout.Text));

                HostnameInformation info = null;
                try
                {
                    info = client.GetHostname();
                }
                catch (Exception exc)
                {
                    BeginInvoke(new Action(() => { ReportError(exc); }));
                }
                try
                {
                    info = client.GetHostname();
                }
                catch (Exception exc)
                {
                    BeginInvoke(new Action(() => { ReportError(exc); }));
                }
                info = client.GetHostname();
            }
            catch (Exception exc)
            {
                BeginInvoke(new Action(() => { ReportError(exc); }));
            }
            finally
            {
                StopAnimation();
            }
        }

        private void btnHalt_Click(object sender, EventArgs e)
        {
            _listener.Stop();
        }

        private void btnGetNtpBackground_Click(object sender, EventArgs e)
        {
            RunInBackground(GetNTP);
        }

        void StopAnimation()
        {
            BeginInvoke(new Action(() =>
            {
                toolStripProgressBar.Style = ProgressBarStyle.Blocks;
            }));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbConsole.Clear();
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            RunInBackground(GetHostname2);
        }

        void SendBrokenMessage()
        {
            MessageSpoiler behaviour = new MessageSpoiler();

            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
            namespaces.Add("onvif", "http://www.onvif.org/ver10/device/wsdl");

            Dictionary<string, string> replacements = new Dictionary<string, string>();
            replacements.Add("/s:Envelope/s:Body/onvif:GetCapabilities/onvif:Category", "XYZ");

            behaviour.Namespaces = namespaces;
            behaviour.NodesToReplace = replacements;

            try
            {
                DeviceClient client = new DeviceClient(new HttpBinding(new IChannelController[] { _listener, behaviour }),
                                                       new EndpointAddress(tbAddress.Text));

                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, int.Parse(tbTimeout.Text));

                client.GetCapabilities(new CapabilityCategory[] { CapabilityCategory.All });
            }
            catch (Exception exc)
            {
                BeginInvoke(new Action(() => { ReportError(exc); }));
            }
            finally
            {
                StopAnimation();
            }
        }

        private void btnBreakMessage_Click(object sender, EventArgs e)
        {
            RunInBackground(SendBrokenMessage);
        }
    }
}
