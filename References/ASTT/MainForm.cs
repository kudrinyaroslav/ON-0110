using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;



namespace ONVIFTestTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonRequestFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxRequestFile.Text = openFileDialog.FileName;
                // Read content from file
                string content = "";
                using (StreamReader reader = new StreamReader(textBoxRequestFile.Text))
                {
                    content = reader.ReadToEnd();
                }
                textBoxRequest.Text = content;
            }
        }

        private void buttonResponseFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxResponseFile.Text = saveFileDialog.FileName;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text,Convert.ToInt16(textBoxPort.Text),textBoxAddressPath.Text);
            string resultMessage;
            double time = 0;
            SoapClient.SendSoapRequestString(textBoxRequest.Text, textBoxResponseFile.Text, "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxResponse.Text = resultMessage;
        }

        private void buttonTestSuitFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxTestSuitFile.Text = openFileDialog.FileName;

                m_testSuit = new TestSuit(textBoxLogFile.Text);
                string result = m_testSuit.Open(textBoxTestSuitFile.Text);

                if (result == null)
                {
                    textBoxLog.Text = m_testSuit.TestSuitInfo;
                }
                else
                {
                    textBoxLog.Text = result;
                }
            }


        }

        private void buttonLogFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            
            if (dialogResult == DialogResult.OK)
            {
                textBoxLogFile.Text = folderBrowserDialog.SelectedPath;
            }
        }


        #region Fields

        private TestSuit m_testSuit = null;
        private TestSuit m_testSuitForTest = null;
        private Test m_currentTest = null;
        private TestResult m_currentTestResult = null;
        private XmlDocument m_Options = null;

        #endregion //Fields

        private void buttonStart_Click(object sender, EventArgs e)
        {
            
            textBoxLog.Text = "";
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),textBoxUser.Text,textBoxPassword.Text,"/");
            TestLog testLog = new TestLog();

            if (!checkBoxCloseConnection.Checked)
            {
                SoapClient.OpenConnection(serviceInfo);
            }
            foreach (Test test in m_testSuit)
            {
                TestResult testResult = test.Run(serviceInfo, checkBoxCloseConnection.Checked);
                testLog.AddTestResult(testResult);
                textBoxLog.Text = textBoxLog.Text + testResult.MessageForShortLog + "\r\n";
                textBoxLog.Refresh();
                System.Threading.Thread.Sleep(50);
            }
            if (!checkBoxCloseConnection.Checked)
            {
                SoapClient.CloseConnection();
            }

            testLog.SaveAsExcel(textBoxLogFile.Text + m_testSuit.XMLDocument.SelectSingleNode("TestSuit/Info/ReportName").InnerText);
        }

        private void buttonNVTTestSuitFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxNVTTestSuitFile.Text = openFileDialog.FileName;

            }
        }

        private void buttonNVTDescription_Click(object sender, EventArgs e)
        {
            TestSuit testSuit = new TestSuit(null);
            string result = testSuit.Open(textBoxNVTTestSuitFile.Text);

            if (result == null)
            {
                NVTTestDescription nvtTestDescription = new NVTTestDescription(testSuit);
                nvtTestDescription.CreateExcelDescription();
            }
            else
            {
                MessageBox.Show(result, "Error");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Create file tree with requests
            string pathToTestFiles = Path.Combine(Application.StartupPath,"TestSuits\\Tests\\");
            using (TextWriter streamWriter = new StreamWriter("CommandList.csv"))
            {

                foreach (string service in Directory.GetDirectories(pathToTestFiles))
                {
                    TreeNode nodeService = treeViewFiles.Nodes.Add(Path.GetFileName(service));
                    string pathToService = Path.Combine(pathToTestFiles, service);
                    foreach (string commandId in Directory.GetDirectories(pathToService))
                    {
                        string pathToCommand = Path.Combine(pathToService, commandId);
                        string[] commandNameFile = Directory.GetFiles(pathToCommand, "!*.txt");
                        string commandName = Path.GetFileName(commandNameFile[0]);
                        commandName = commandName.Substring(1, commandName.Length - 5);

                        TreeNode nodeCommand = nodeService.Nodes.Add(Path.GetFileName(commandId) + " (" + commandName + ")");

                        streamWriter.Write(Path.GetFileName(service) + ";");
                        streamWriter.Write(Path.GetFileName(commandId) + ";");
                        streamWriter.Write(commandName);
                        streamWriter.WriteLine();

                        foreach (string request in Directory.GetFiles(pathToCommand, "TC.*_REQ_*.xml"))
                        {
                            TreeNode nodeRequest = nodeCommand.Nodes.Add(Path.GetFileName(request));
                            nodeRequest.Tag = request;
                        }
                    }
                }
                streamWriter.Flush();

            }

            //Put all testsuits files to combobox
            string pathToTestSuitFiles = Path.Combine(Application.StartupPath,"TestSuits\\");
            
            foreach(string testSuit in Directory.GetFiles(pathToTestSuitFiles, "TestSuit*.xml"))
            {
                comboBoxTestSuitTest.Items.Add(testSuit);
            }


            //Load options from file
            XmlReader reader = null;

            try
            {
                reader = XmlReader.Create("Options.xml");
                m_Options = new XmlDocument();
                m_Options.Load(reader);

                textBoxAddress.Text = m_Options.SelectSingleNode("/Options/IP").InnerText;
                textBoxPort.Text = m_Options.SelectSingleNode("/Options/Port").InnerText;
                textBoxUser.Text = m_Options.SelectSingleNode("/Options/User").InnerText;
                textBoxPassword.Text = m_Options.SelectSingleNode("/Options/Password").InnerText;

                foreach (XmlNode service in m_Options.SelectSingleNode("/Options/Services").ChildNodes)
                {
                    comboBoxService.Items.Add(service.Name);
                }
                ServiceInfo.Service = m_Options.SelectSingleNode("/Options/Services");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private void treeViewFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                textBoxRequestFile.Text = (string)e.Node.Tag;
                // Read content from file
                string content = "";
                using (StreamReader reader = new StreamReader(textBoxRequestFile.Text))
                {
                    content = reader.ReadToEnd();
                }

                //Not universal but work for now
                if (textBoxUser.Text!="")
                {
                    string header = "<s:Header>\r\n";
                    header = header + "    <wsse:Security xmlns:wsse = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n";
                    header = header + "      <wsse:UsernameToken>\r\n";
                    header = header + "        <wsse:Username>" + textBoxUser.Text + "</wsse:Username>\r\n";
                    header = header + "        <wsse:Password>" + textBoxPassword.Text + "</wsse:Password>\r\n";
                    header = header + "      </wsse:UsernameToken>\r\n";
                    header = header + "    </wsse:Security>\r\n";
                    header = header + "  </s:Header>\r\n";
                    header = header + "  <s:Body>";
                    content = content.Replace("<s:Body>", header);
                    
                    header = "<soap:Header>\r\n";
                    header = header + "    <wsse:Security xmlns:wsse = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n";
                    header = header + "      <wsse:UsernameToken>\r\n";
                    header = header + "        <wsse:Username>" + textBoxUser.Text + "</wsse:Username>\r\n";
                    header = header + "        <wsse:Password>" + textBoxPassword.Text + "</wsse:Password>\r\n";
                    header = header + "      </wsse:UsernameToken>\r\n";
                    header = header + "    </wsse:Security>\r\n";
                    header = header + "  </soap:Header>\r\n";
                    header = header + "  <soap:Body>";
                    content = content.Replace("<soap:Body>", header);

                    header = "<SOAP-ENV:Header>\r\n";
                    header = header + "    <wsse:Security xmlns:wsse = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n";
                    header = header + "      <wsse:UsernameToken>\r\n";
                    header = header + "        <wsse:Username>" + textBoxUser.Text + "</wsse:Username>\r\n";
                    header = header + "        <wsse:Password>" + textBoxPassword.Text + "</wsse:Password>\r\n";
                    header = header + "      </wsse:UsernameToken>\r\n";
                    header = header + "    </wsse:Security>\r\n";
                    header = header + "  </SOAP-ENV:Header>\r\n";
                    header = header + "  <SOAP-ENV:Body>";
                    content = content.Replace("<SOAP-ENV:Body>", header);
                }


                textBoxRequest.Text = content;

                //Select path to service
                string fileName = Path.GetFileNameWithoutExtension(textBoxRequestFile.Text);
                foreach (XmlNode service in m_Options.SelectSingleNode("/Options/Services").ChildNodes)
                {
                    if (fileName.Contains(service.SelectSingleNode("FileName").InnerText))
                    {
                        textBoxAddressPath.Text = service.SelectSingleNode("Path").InnerText;
                        comboBoxService.Text = service.Name;
                    }
                }
            }
        }

        private void buttonSelectTestSuitForTest_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (!(comboBoxTestSuitTest.Items.Contains(openFileDialog.FileName)))
                {
                    comboBoxTestSuitTest.Items.Add(openFileDialog.FileName);
                }
                comboBoxTestSuitTest.SelectedItem = openFileDialog.FileName;
                OpenTestSuitForTest();
            }
        }

        private void OpenTestSuitForTest()
        {
            m_testSuitForTest = new TestSuit(textBoxTestResult.Text);
            string result = m_testSuitForTest.Open(comboBoxTestSuitTest.Text);
            treeViewTests.Nodes.Clear();

            if (result == null)
            {
                foreach (XmlNode service in m_testSuitForTest.XMLDocument.SelectNodes("TestSuit/TestList/Service[@enabled=\"true\"]"))
                {
                    TreeNode treeNodeService = new TreeNode();
                    treeNodeService.Text = "[" + service.Attributes.GetNamedItem("id").InnerText + "] " +
                                    service.Attributes.GetNamedItem("name").InnerText;

                    treeViewTests.Nodes.Add(treeNodeService);
                    foreach (XmlNode command in service.SelectNodes("TestGroup[@enabled=\"true\"]"))
                    {
                        TreeNode treeNodeCommand = new TreeNode();
                        treeNodeCommand.Text = "[" + command.Attributes.GetNamedItem("id").InnerText + "] " +
                                        command.Attributes.GetNamedItem("command").InnerText;

                        treeNodeService.Nodes.Add(treeNodeCommand);
                        foreach (XmlNode test in command.SelectNodes("Test[@enabled=\"true\"]"))
                        {
                            TreeNode treeNodeTest = new TreeNode();
                            treeNodeTest.Text = "[" + test.Attributes.GetNamedItem("id").InnerText + "] " +
                                            test.FirstChild.InnerText;
                            treeNodeTest.Tag = test;
                            treeNodeCommand.Nodes.Add(treeNodeTest);

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(result, "Error");
            }
        }

        private void comboBoxTestSuitTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenTestSuitForTest();
        }

        private void treeViewTests_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listViewSteps.Items.Clear();
            m_currentTest = null;
            if (e.Node.Level == 2)
            {
                XmlNode test = (XmlNode)(e.Node.Tag);
                m_currentTest = new Test(test, textBoxTestResult.Text + "TestFiles\\", textBoxTestResult.Text);
                textBoxTestID.Text = m_currentTest.TestId;
                textBoxTestName.Text = m_currentTest.Name;
                textBoxCommandName.Text = m_currentTest.Command;
                foreach (XmlNode step in test.SelectNodes("Step"))
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = step.Attributes.GetNamedItem("id").InnerText;
                    listViewItem.SubItems.Add(step.PreviousSibling.InnerText);
                    listViewItem.SubItems.Add("");
                    listViewItem.SubItems.Add("");
                    listViewItem.SubItems.Add("");
                    listViewItem.Tag = step;
                    listViewSteps.Items.Add(listViewItem);
                }

            }
            else
            {
                textBoxTestID.Text = "";
                textBoxTestName.Text = "";
                textBoxCommandName.Text = "";
            }
        }

        private void listViewSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSteps.SelectedItems.Count > 0)
            {
                if (m_currentTestResult != null)
                {
                    if (listViewSteps.SelectedItems[0].Index < m_currentTestResult.m_testStepResultArray.Count)
                    {
                        string content;
                        TestStepResult testStepResult = m_currentTestResult.m_testStepResultArray[listViewSteps.SelectedItems[0].Index];
                        if (testStepResult.PathToRealResponseFile != "")
                        {
                            // Read content from file
                            content = "";
                            using (StreamReader reader = new StreamReader(testStepResult.PathToRealResponseFile))
                            {
                                content = reader.ReadToEnd();
                            }
                            content = content.Replace("\r\n", "\n");
                            content = content.Replace("\n", "\r\n");
                            content = content.Replace("\t", "  ");
                            textBoxRes.Text = content;
                        }
                        else
                        {
                            textBoxRes.Text = testStepResult.Message;
                        }

                        if (testStepResult.PathToCompareFile != "")
                        {
                            webBrowserDif.Navigate(new Uri(testStepResult.PathToCompareFile));
                        }
                        else
                        {
                            webBrowserDif.Url = null;
                        }

                        // Read content from file
                        content = "";
                        using (StreamReader reader = new StreamReader(testStepResult.PathToRequestFile))
                        {
                            content = reader.ReadToEnd();
                        }
                        textBoxReq.Text = content;
                    }
                }
            }
        }

        private void buttonStartTest_Click(object sender, EventArgs e)
        {
            if (m_currentTest != null)
            {

                for (int j = 0; j < listViewSteps.Items.Count; j++)
                {
                    listViewSteps.Items[j].SubItems[2].Text = "";
                    listViewSteps.Items[j].SubItems[3].Text = "";
                    listViewSteps.Items[j].SubItems[4].Text = "";
                }

                textBoxReq.Text = "";
                textBoxRes.Text = "";
                webBrowserDif.Url = null;
                ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),textBoxUser.Text,textBoxPassword.Text,"/");
                m_currentTestResult = m_currentTest.Run(serviceInfo,true);
                int i = 0;
                foreach (TestStepResult testStepResult in m_currentTestResult.m_testStepResultArray)
                {
                    listViewSteps.Items[i].SubItems[2].Text = testStepResult.ResultString;
                    listViewSteps.Items[i].SubItems[3].Text = testStepResult.Time.ToString();
                    listViewSteps.Items[i].SubItems[4].Text = testStepResult.Message;
                    i++;
                }
            }
        }

        private void trackBarAbsoluteX_Scroll(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                AbsoluteMoveZoom(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value, trackBarAbsoluteZoom.Value);
            }
            else
            {
                AbsoluteMove(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value);
            }
        }

        private void AbsoluteMove(int x, int y)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.07\\TC.PTZ.NVT.07_REQ_02.xml"))
            {
                content = reader.ReadToEnd();
            }
            double xd = (double)x / (double)100 - 1;
            double yd = (double)y / (double)100 - 1;

            content = content.Replace("XXX", (xd.ToString()).Replace(",","."));
            content = content.Replace("YYY", (yd.ToString()).Replace(",", "."));

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void AbsoluteZoom(int z)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.07\\TC.PTZ.NVT.07_REQ_03.xml"))
            {
                content = reader.ReadToEnd();
            }
            double zd = (double)z / (double)100;

            content = content.Replace("ZZZ", (zd.ToString()).Replace(",", "."));

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void AbsoluteMoveZoom(int x,int y,int z)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text), "/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.07\\TC.PTZ.NVT.07_REQ_04.xml"))
            {
                content = reader.ReadToEnd();
            }
            double xd = (double)x / (double)100 - 1;
            double yd = (double)y / (double)100 - 1;
            double zd = (double)z / (double)100;

            content = content.Replace("ZZZ", (zd.ToString()).Replace(",", "."));
            content = content.Replace("XXX", (xd.ToString()).Replace(",", "."));
            content = content.Replace("YYY", (yd.ToString()).Replace(",", "."));

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void trackBarAbsoluteY_Scroll(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                AbsoluteMoveZoom(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value, trackBarAbsoluteZoom.Value);
            }
            else
            {
                AbsoluteMove(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value);
            }
        }


        private void buttonAllX_Click(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                for (int i = 0; i < 200; i++)
                {
                    Thread.Sleep(500);
                    AbsoluteMoveZoom(i, trackBarAbsoluteY.Value, trackBarAbsoluteZoom.Value);
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    Thread.Sleep(500);
                    AbsoluteMove(i, trackBarAbsoluteY.Value);
                }
            }
        }

        private void buttonAllY_Click(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                for (int i = 0; i < 200; i++)
                {
                    Thread.Sleep(500);
                    AbsoluteMoveZoom(trackBarAbsoluteX.Value, i, trackBarAbsoluteZoom.Value);
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    Thread.Sleep(500);
                    AbsoluteMove(trackBarAbsoluteX.Value, i);
                }
            }
        }


        private void trackBarAbsoluteZoom_Scroll(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                AbsoluteMoveZoom(trackBarAbsoluteX.Value,trackBarAbsoluteY.Value,trackBarAbsoluteZoom.Value);
            }
            else
            {
                AbsoluteZoom(trackBarAbsoluteZoom.Value);
            }
        }

        private void buttonAllZoom_Click(object sender, EventArgs e)
        {
            if (checkBoxBoth.Checked)
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(500);
                    AbsoluteMoveZoom(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value, i);
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(500);
                    AbsoluteZoom(i);
                }
            }
        }

        private void buttonAllXYZoom_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(500);
                AbsoluteMoveZoom(2 * i, 2 * i, i);
            }
        }

        private void buttonXMin_Click(object sender, EventArgs e)
        {
            AbsoluteMoveZoom(0, trackBarAbsoluteY.Value, trackBarAbsoluteZoom.Value);
        }

        private void buttonXMax_Click(object sender, EventArgs e)
        {
            AbsoluteMoveZoom(100, trackBarAbsoluteY.Value, trackBarAbsoluteZoom.Value);
        }

        private void buttonYMax_Click(object sender, EventArgs e)
        {
            AbsoluteMoveZoom(trackBarAbsoluteX.Value, 200,  trackBarAbsoluteZoom.Value);
        }

        private void buttonYMin_Click(object sender, EventArgs e)
        {
            AbsoluteMoveZoom(trackBarAbsoluteX.Value, 0, trackBarAbsoluteZoom.Value);
        }

        private void buttonZoomMin_Click(object sender, EventArgs e)
        {
            AbsoluteMoveZoom(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value, 0);
        }

        private void buttonZoomMax_Click(object sender, EventArgs e)
        {
            AbsoluteMoveZoom(trackBarAbsoluteX.Value, trackBarAbsoluteY.Value, 100);
        }

        private void buttonStopAbsoluteZoom_Click(object sender, EventArgs e)
        {
            StopZoom();
        }


        private void StopMoveZoom()
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.09\\TC.PTZ.NVT.09_REQ_01.xml"))
            {
                content = reader.ReadToEnd();
            }

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void StopZoom()
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.09\\TC.PTZ.NVT.09_REQ_03.xml"))
            {
                content = reader.ReadToEnd();
            }

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void StopMove()
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.09\\TC.PTZ.NVT.09_REQ_02.xml"))
            {
                content = reader.ReadToEnd();
            }

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void buttonStopAbsoluteMove_Click(object sender, EventArgs e)
        {
            StopMove();
        }

        private void buttonStopAbsoluteAll_Click(object sender, EventArgs e)
        {
            StopMoveZoom();
        }

        private void buttonStartMove_Click(object sender, EventArgs e)
        {
            ContinuousMove(numericUpDownX.Value, numericUpDownY.Value);
        }

        private void ContinuousMoveZoom(decimal x, decimal y, decimal z)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.11\\TC.PTZ.NVT.11_REQ_02.xml"))
            {
                content = reader.ReadToEnd();
            }
            content = content.Replace("ZZZ", (z.ToString()).Replace(",", "."));
            content = content.Replace("XXX", (x.ToString()).Replace(",", "."));
            content = content.Replace("YYY", (y.ToString()).Replace(",", "."));

            if (checkBoxUseTimeout.Checked)
            {
                content = content.Replace("TIMEOUT", "<tt:Timeout>P0.3S</tt:Timeout>");
            }
            else 
            {
                content = content.Replace("TIMEOUT", "");
            }

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void ContinuousMove(decimal x, decimal y)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text), "/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.11\\TC.PTZ.NVT.11_REQ_03.xml"))
            {
                content = reader.ReadToEnd();
            }
            content = content.Replace("XXX", (x.ToString()).Replace(",", "."));
            content = content.Replace("YYY", (y.ToString()).Replace(",", "."));
            if (checkBoxUseTimeout.Checked)
            {
                content = content.Replace("TIMEOUT", "<tt:Timeout>P0.3S</tt:Timeout>");
            }
            else
            {
                content = content.Replace("TIMEOUT", "");
            }

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void ContinuousZoom(decimal z)
        {
            ServiceInfo serviceInfo = new ServiceInfo(textBoxAddress.Text, Convert.ToInt16(textBoxPort.Text),"/onvif/ptz");
            string resultMessage;
            double time = 0;
            StringBuilder soapRequest = new StringBuilder();


            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader("TestSuits\\Tests\\PTZ\\TC.PTZ.NVT.11\\TC.PTZ.NVT.11_REQ_04.xml"))
            {
                content = reader.ReadToEnd();
            }
            content = content.Replace("ZZZ", (z.ToString()).Replace(",", "."));
            if (checkBoxUseTimeout.Checked)
            {
                content = content.Replace("TIMEOUT", "<tt:Timeout>P0.3S</tt:Timeout>");
            }
            else
            {
                content = content.Replace("TIMEOUT", "");
            }

            SoapClient.SendSoapRequestString(content, "", "", serviceInfo, out resultMessage, out time);
            resultMessage = "TIME: " + time.ToString() + " ms\r\n\r\n" + resultMessage;
            resultMessage = resultMessage.Replace("\r\n", "\n");
            resultMessage = resultMessage.Replace("\n", "\r\n");
            resultMessage = resultMessage.Replace("\t", "  ");
            textBoxPTZRes.Text = resultMessage;
        }

        private void buttonStartZoom_Click(object sender, EventArgs e)
        {
            ContinuousZoom(numericUpDownZ.Value);
        }

        private void buttonStartZoomMove_Click(object sender, EventArgs e)
        {
            ContinuousMoveZoom(numericUpDownX.Value, numericUpDownY.Value, numericUpDownZ.Value);
        }

        private void buttonStopAbsoluteMove_Click_1(object sender, EventArgs e)
        {
            StopMove();
        }

        private void buttonStopAbsoluteZoom_Click_1(object sender, EventArgs e)
        {
            StopZoom();
        }

        private void buttonStopAbsoluteAll_Click_1(object sender, EventArgs e)
        {
            StopMoveZoom();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save options for connection
            m_Options.SelectSingleNode("/Options/IP").InnerText = textBoxAddress.Text;
            m_Options.SelectSingleNode("/Options/Port").InnerText = textBoxPort.Text;
            m_Options.SelectSingleNode("/Options/User").InnerText = textBoxUser.Text;
            m_Options.SelectSingleNode("/Options/Password").InnerText = textBoxPassword.Text;
            m_Options.Save("Options.xml");
        }

        private void comboBoxService_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAddressPath.Text = m_Options.SelectSingleNode("/Options/Services/" + comboBoxService.Text + "/Path").InnerText;
        }


    }
}
