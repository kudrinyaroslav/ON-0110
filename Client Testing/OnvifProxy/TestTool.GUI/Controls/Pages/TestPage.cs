using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TestTool.Device;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Controls
{
    internal partial class TestPage : BasePage, Views.ITestView
    {
        private Controllers.TestController _controller;

        public TestPage()
        {
            InitializeComponent();

            _controller = new TestController(this);
        }


        private void tsbStart_Click(object sender, EventArgs e)
        {
            _controller.Start();

        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            _controller.Stop();
        }




        #region IView Members

        public IController GetController()
        {
            return _controller;
        }
        
        #endregion

        #region IDiagnosticView Members
        
        public void Clear()
        {
            Invoke(new Action(() =>
                                  {
                                      lvRequests.Items.Clear();
                                      tbMessage.Clear();
                                  }));
        }

        public void DisplayNetworkEvent(NetworkEventData data)
        {
            Invoke(new Action(() =>
                                  {
                                      ListViewItem item =
                                          new ListViewItem(data.Timestamp.ToString("dd-MM-yy  HH-mm-ss.fff"));

                                      string type = string.Empty;
                                      string imageKey = string.Empty;
                                      switch (data.Type)
                                      {
                                          case NetworkEventType.ClientRequest:
                                              imageKey = "FromClient";
                                              type = "From Client";
                                              break;
                                          case NetworkEventType.ClientResponse:
                                              imageKey = "ToClient";
                                              type = "To Client";
                                              break;
                                          case NetworkEventType.DutRequest:
                                              imageKey = "ToDevice";
                                              type = "To Device";
                                              break;
                                          case NetworkEventType.DutResponse:
                                              imageKey = "FromDevice";
                                              type = "From Device";
                                              break;
                                      }
                                      item.ImageKey = imageKey;
                                      item.SubItems.Add(type);
                                      item.Tag = data;
                                      lvRequests.Items.Add(item);
                                  }));


        }

        public void SwitchToWorkingMode()
        {
            Invoke(new Action(() =>
                                  {
                                      tsbStop.Enabled = true;
                                      tsbStart.Enabled = false;
                                      tsbSave.Enabled = false;
                                      tsbClear.Enabled = false;
                                  }));
        }

        public void SwitchToIdleMode()
        {
            tsbStop.Enabled = false;
            tsbStart.Enabled = true;
            tsbSave.Enabled = true;
            tsbClear.Enabled = true;
        }

        #endregion

        internal TestController Controller
        {
            get { return _controller; }
        }

        private void lvRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRequests.SelectedItems.Count > 0)
            {
                ListViewItem item = lvRequests.SelectedItems[0];
                NetworkEventData data = (NetworkEventData)item.Tag;
                tbMessage.Text = data.Message;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void lvRequests_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvRequests.SelectedItems.Count > 0)
            {
                ListViewItem item = lvRequests.SelectedItems[0];
                NetworkEventData data = (NetworkEventData) item.Tag;
                NetworkEventType type = data.Type;

                SelectItems( ED => ED.Type == type);
            }
            else
            {
                SelectItems( ED => false);
            }
        }


        void SelectItems(Func<NetworkEventData, bool> selector)
        {
            foreach (ListViewItem item in lvRequests.Items)
            {
                NetworkEventData data = (NetworkEventData) item.Tag;

                if (selector(data))
                {
                    item.BackColor = Color.PowderBlue;
                }
                else
                {
                    item.BackColor = Color.White;
                }
            }
        }

        private void lvRequests_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SelectItems(ED => false);
            }
        }

    }
}
