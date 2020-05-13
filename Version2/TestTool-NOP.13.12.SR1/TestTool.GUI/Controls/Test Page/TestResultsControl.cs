using System;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Data;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions.Trace;

namespace TestTool.GUI.Controls
{
    public partial class TestResultsControl : UserControl, ITestResultView
    {
        public TestResultsControl()
        {
            InitializeComponent();
        }

        public void BeginTest()
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        ClearTestInfo();
                    }));
        }

        // mark failed step by red color
        private void HighlightFailedSteps(string PlainLog)
        {
            int posFailed = -1;
            int startPos = 0;

            while ((posFailed = rtbTestResult.Find(stepFailedStr, posFailed + 1, RichTextBoxFinds.None)) >= 0)
            {
                int stepFailedTotalLength = 0;

                // get line number of failed step
                int lineNumber = rtbTestResult.GetLineFromCharIndex(posFailed);
                stepFailedTotalLength += (rtbTestResult.Lines[lineNumber].Length + 1);

                // looking for beginning of failed step on previous lines 
                for (lineNumber -= 1; lineNumber >= 0; lineNumber--)
                {
                    stepFailedTotalLength += (rtbTestResult.Lines[lineNumber].Length + 1);

                    string currentLine = rtbTestResult.Lines[lineNumber];

                    if (currentLine.StartsWith("   STEP")
                        && !currentLine.Contains("STEP PASSED")
                        && !currentLine.Contains("STEP FAILED"))
                    {
                        // get first char index of found line
                        startPos = rtbTestResult.GetFirstCharIndexFromLine(lineNumber);

                        lstFailedStepPos.Add(startPos);
                        lstStepFailedLength.Add(stepFailedTotalLength);

                        break;
                    }
                }

                rtbTestResult.Select(startPos, stepFailedTotalLength);
                rtbTestResult.SelectionColor = System.Drawing.Color.Red;
            }
        }

        public void DisplayTestResults(TestResult log)
        {
            if (log.Log != null)
            {
                foreach (StepResult result in log.Log.Steps.OrderBy(s => s.Number))
                {
                    AddStepResult(result);
                }
            }
            
            rtbTestResult.Text = log.PlainTextLog;

            // highlight failed steps in new log with red color
            if (rtbTestResult.TextLength != 0)
            {
                HighlightFailedSteps(rtbTestResult.Text);
            }
        }

        // disable search functionality if test(s) is(are) running 
        public void DisableSearch()
        {
            scOutputResultBorder.Panel2.Enabled = false;
            scOutputResultBorder.Panel2.Invalidate();
        }

        // enable search functionality if test(s) is(are) stopped 
        public void EnableSearch()
        {
            scOutputResultBorder.Panel2.Enabled = true;
            scOutputResultBorder.Panel2.Invalidate();
        }

        public void DisplayProfileTestLog(string dump)
        {
            ClearTestInfo();
            rtbTestResult.Text = dump;
        }

        public void ClearTestInfo()
        {
            lvStepDetails.Items.Clear();
            tbResponse.Clear();
            tbRequest.Clear();

            rtbTestResult.Clear();

            FailedStepCurrentFoundPos = -1;

            // reset search results for new test log
            lstFindPos.Clear();
            FindPos = -1;
            FindPosIndex = 0;
            searchTextLength = 0;

            // reset "failed steps" for new test log
            lstFailedStepPos.Clear();
            lstStepFailedLength.Clear();
            FailedStepPosIndex = 0;
            FailedStepPos = -1;
            FailedStepSearchDone = false;
        }

        /// <summary>
        /// Adds line to test log.
        /// </summary>
        /// <param name="logEntry"></param>
        public void WriteLine(string logEntry)
        {
            Invoke(
                new Action(
                    () =>
                    {
                        InternalWriteLine(logEntry);
                    }));


        }

        // list that store failed step positions
        private System.Collections.Generic.List<int> lstFailedStepPos = new System.Collections.Generic.List<int>();

        // list that store length of failed step
        private System.Collections.Generic.List<int> lstStepFailedLength = new System.Collections.Generic.List<int>();

        //
        private int FailedStepCurrentFoundPos = -1;

        private string stepFailedStr = "   STEP FAILED";

        /// <summary>
        /// Adds line to log.
        /// </summary>
        /// <param name="logEntry">Log entry</param>
        void InternalWriteLine(string logEntry)
        {
            rtbTestResult.SuspendLayout();
            rtbTestResult.AppendText(string.Format("{0}{1}", logEntry, Environment.NewLine));

            // mark failed step by red color          
            if (logEntry == stepFailedStr)
            {
                int posFailed = -1;
                posFailed = rtbTestResult.Find(stepFailedStr, FailedStepCurrentFoundPos + 1, RichTextBoxFinds.None);
                FailedStepCurrentFoundPos = posFailed;

                int stepFailedTotalLength = 0;
                int startPos = 0;

                if (posFailed >= 0)
                {
                    // get line number of failed step
                    int lineNumber = rtbTestResult.GetLineFromCharIndex(posFailed);
                    stepFailedTotalLength += (rtbTestResult.Lines[lineNumber].Length + 1);

                    // looking for beginning of failed step on previous lines 
                    for (lineNumber -= 1; lineNumber >= 0; lineNumber--)
                    {
                        stepFailedTotalLength += (rtbTestResult.Lines[lineNumber].Length + 1);

                        string currentLine = rtbTestResult.Lines[lineNumber];

                        if (currentLine.StartsWith("   STEP") 
                            && !currentLine.Contains("STEP PASSED") 
                            && !currentLine.Contains("STEP FAILED"))
                        {
                            // get first char index of found line
                            startPos = rtbTestResult.GetFirstCharIndexFromLine(lineNumber);

                            lstFailedStepPos.Add(startPos);
                            lstStepFailedLength.Add(stepFailedTotalLength);

                            break;
                        }
                    }

                    rtbTestResult.Select(startPos, stepFailedTotalLength);
                    rtbTestResult.SelectionColor = System.Drawing.Color.Red;
                }
            }

            rtbTestResult.SelectionStart = rtbTestResult.Text.Length;
            rtbTestResult.ScrollToCaret();

            rtbTestResult.ResumeLayout();

        }

        /// <summary>
        /// Displays step result.
        /// </summary>
        /// <param name="result"></param>
        public void DisplayStepResult(StepResult result)
        {
            BeginInvoke(new Action(() => AddStepResult(result)));
        }
        
        /// <summary>
        /// Adds list item representing step results.
        /// </summary>
        /// <param name="result">Step information.</param>
        public void AddStepResult(StepResult result)
        {
            ListViewItem stepItem = new ListViewItem(result.Number.ToString());
            stepItem.Tag = result;
            stepItem.SubItems.Add(result.Status.ToString());
            stepItem.SubItems.Add(result.Message);
            stepItem.ImageKey = string.IsNullOrEmpty(result.Request) && string.IsNullOrEmpty(result.Response) ? 
                string.Empty : "NetworkOperation";
            lvStepDetails.Items.Add(stepItem);
        }
        
        /// <summary>
        /// Displays step details when a step is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvStepDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStepDetails.SelectedItems.Count > 0)
            {
                ListViewItem item = lvStepDetails.SelectedItems[0];
                StepResult result = (StepResult)item.Tag;
                tbRequest.Text = result.Request;
                tbResponse.Text = result.Response;
            }
            else
            {
                tbRequest.Text = string.Empty;
                tbResponse.Text = string.Empty;
            }
        }

        public void EnableControl(bool bEnable)
        {
        }

        // list that store indexes of found strings
        private System.Collections.Generic.List<int> lstFindPos = new System.Collections.Generic.List<int>();

        // length of searched text
        private int searchTextLength = 0;

        // function that scroll to invisible search result
        private void scrollToInvisibleSearchResult(int resultPosition)
        {
            System.Drawing.Point ptFirstFindResult = rtbTestResult.GetPositionFromCharIndex(resultPosition);
            System.Drawing.Rectangle rectClient = rtbTestResult.ClientRectangle;

            if (ptFirstFindResult.Y > rectClient.Height || ptFirstFindResult.Y < 0)
            {
                rtbTestResult.SelectionStart = resultPosition;
                rtbTestResult.ScrollToCaret();
            }
        }

        void GoToNext()
        {
            if (lstFindPos.Count != 0)
            {
                // after last found result go to first found result
                if (FindPosIndex == lstFindPos.Count - 1)
                {
                    FindPosIndex = -1;
                }

                FindPos = lstFindPos[++FindPosIndex];

                // highlighted next found value
                rtbTestResult.Select(FindPos, searchTextLength);
                rtbTestResult.SelectionBackColor = System.Drawing.Color.Orange;

                int findPosPrevIndex = -1;

                // return previous color for previous found value
                if ((FindPosIndex - 1 >= 0 || FindPosIndex == 0) && lstFindPos.Count != 1)
                {
                    findPosPrevIndex = FindPosIndex == 0 ? lstFindPos.Count - 1 : FindPosIndex - 1;
                    rtbTestResult.Select(lstFindPos[findPosPrevIndex], searchTextLength);
                    rtbTestResult.SelectionBackColor = System.Drawing.Color.Yellow;
                }

                scrollToInvisibleSearchResult(FindPos);
            }
        }


        private void tbFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GoToNext();
            }
        }

        // position of found result
        private int FindPos = -1;

        // index of found position in lstFindPos
        private int FindPosIndex = 0;

        private void bNext_Click(object sender, EventArgs e)
        {
            GoToNext();
        }

        void GoToPrevious()
        {
            if (lstFindPos.Count != 0)
            {
                // after first found result go to last found result
                if (FindPosIndex == 0 || FindPosIndex == -1)
                {
                    FindPosIndex = lstFindPos.Count;
                }

                FindPos = lstFindPos[--FindPosIndex];

                // highlighted previous found value
                rtbTestResult.Select(FindPos, searchTextLength);
                rtbTestResult.SelectionBackColor = System.Drawing.Color.Orange;

                int findPosNextIndex = -1;

                // return next color for next found value
                if ((FindPosIndex + 1 <= lstFindPos.Count - 1 || FindPosIndex == lstFindPos.Count - 1) && lstFindPos.Count != 1)
                {
                    findPosNextIndex = FindPosIndex == lstFindPos.Count - 1 ? 0 : FindPosIndex + 1;
                    rtbTestResult.Select(lstFindPos[findPosNextIndex], searchTextLength);
                    rtbTestResult.SelectionBackColor = System.Drawing.Color.Yellow;
                }

                scrollToInvisibleSearchResult(FindPos);
            }
        }

        private void bPrev_Click(object sender, EventArgs e)
        {
            GoToPrevious();
        }

        // index of failed step position in lstFailedStepPos
        private int FailedStepPosIndex = 0;

        // position of failed step
        private int FailedStepPos = -1;

        // if there was search already done or not
        bool FailedStepSearchDone = false;

        // length of failed step string
        //private string FailedStepLength = stepFailedStr.Length;

        // "Next failed step" functionality makes both search and navigation 
        // to next failed step
        private void bNextFailed_Click(object sender, EventArgs e)
        {
            if (lstFailedStepPos.Count != 0)
            {
                ///////////////////////////////////////////////////////////////////////////////
                // This code block goes to first/last or nearest failed step. 
                // This is part of search functionality.

                if (!FailedStepSearchDone)
                {
                    int CursorPosition = rtbTestResult.SelectionStart;

                    // reset all backcolor highlights after usual search
                    rtbTestResult.Select(0, rtbTestResult.Text.Length);
                    rtbTestResult.SelectionBackColor = System.Drawing.SystemColors.Control;

                    // forget about search results
                    lstFindPos.Clear();
                    FindPos = -1;
                    FindPosIndex = 0;
                    searchTextLength = 0;

                    // if cursor is in the end of text (by default)
                    // then navigates to first failed step
                    if (CursorPosition == rtbTestResult.TextLength)
                    {
                        // scroll to first failed step if it's invisible
                        scrollToInvisibleSearchResult(lstFailedStepPos[0]);

                        // first search result marks with Next/Previous color
                        rtbTestResult.Select(lstFailedStepPos[0], lstStepFailedLength[0]);
                        rtbTestResult.SelectionBackColor = System.Drawing.Color.LightPink;

                        FailedStepPosIndex = 0;
                    }
                    // if cursor is in the other location
                    // then navigates to nearest failed step after cursor location 
                    else
                    {
                        for (int i = 0; i < lstFailedStepPos.Count; i++)
                        {
                            if (lstFailedStepPos[i] >= CursorPosition)
                            {
                                FailedStepPosIndex = i;
                                break;
                            }

                            // if we don't find failed step after cursor location
                            // then take last failed step
                            if (i == lstFailedStepPos.Count - 1)
                            {
                                FailedStepPosIndex = i;
                            }
                        }

                        // scroll to found line if it's invisible
                        scrollToInvisibleSearchResult(lstFailedStepPos[FailedStepPosIndex]);

                        // search result marks with Next/Previous color
                        rtbTestResult.Select(lstFailedStepPos[FailedStepPosIndex], lstStepFailedLength[FailedStepPosIndex]);
                        rtbTestResult.SelectionBackColor = System.Drawing.Color.LightPink;
                    }

                    FailedStepSearchDone = true;
                }
                ///////////////////////////////////////////////////////////////////////////////
                // This code block navigates to next failed step. 
                // This is part of navigate functionality.
                else
                {
                    // after last failed step go to first failed step
                    if (FailedStepPosIndex == lstFailedStepPos.Count - 1)
                    {
                        FailedStepPosIndex = -1;
                    }

                    FailedStepPos = lstFailedStepPos[++FailedStepPosIndex];

                    // highlight next failed step
                    rtbTestResult.Select(FailedStepPos, lstStepFailedLength[FailedStepPosIndex]);
                    rtbTestResult.SelectionBackColor = System.Drawing.Color.LightPink;

                    int failedStepPosPrevIndex = -1;

                    // return previous color for previous failed step
                    if ((FailedStepPosIndex - 1 >= 0 || FailedStepPosIndex == 0) && lstFailedStepPos.Count != 1)
                    {
                        failedStepPosPrevIndex = FailedStepPosIndex == 0 ? lstFailedStepPos.Count - 1 : FailedStepPosIndex - 1;
                        rtbTestResult.Select(lstFailedStepPos[failedStepPosPrevIndex], lstStepFailedLength[failedStepPosPrevIndex]);
                        rtbTestResult.SelectionBackColor = System.Drawing.SystemColors.Control;
                    }

                    scrollToInvisibleSearchResult(FailedStepPos);
                }
            }
        }

        private void bPrevFailed_Click(object sender, EventArgs e)
        {
            if (lstFailedStepPos.Count != 0)
            {
                ///////////////////////////////////////////////////////////////////////////////
                // This code block goes to first/last or nearest failed step. 
                // This is part of search functionality.

                if (!FailedStepSearchDone)
                {
                    int CursorPosition = rtbTestResult.SelectionStart;

                    // reset all backcolor highlights after usual search
                    rtbTestResult.Select(0, rtbTestResult.Text.Length);
                    rtbTestResult.SelectionBackColor = System.Drawing.SystemColors.Control;

                    // forget about search results
                    lstFindPos.Clear();
                    FindPos = -1;
                    FindPosIndex = 0;
                    searchTextLength = 0;

                    // if cursor is in the end of text (by default)
                    // then navigates to first failed step
                    if (CursorPosition == rtbTestResult.TextLength)
                    {
                        // scroll to first failed step if it's invisible
                        scrollToInvisibleSearchResult(lstFailedStepPos[0]);

                        // first search result marks with Next/Previous color
                        rtbTestResult.Select(lstFailedStepPos[0], lstStepFailedLength[0]);
                        rtbTestResult.SelectionBackColor = System.Drawing.Color.LightPink;

                        FailedStepPosIndex = 0;
                    }
                    // if cursor is in the other location
                    // then navigates to nearest failed step after cursor location 
                    else
                    {
                        for (int i = 0; i < lstFailedStepPos.Count; i++)
                        {
                            if (lstFailedStepPos[i] >= CursorPosition)
                            {
                                FailedStepPosIndex = i;
                                break;
                            }

                            // if we don't find failed step after cursor location
                            // then take last failed step
                            if (i == lstFailedStepPos.Count - 1)
                            {
                                FailedStepPosIndex = i;
                            }
                        }

                        // scroll to found line if it's invisible
                        scrollToInvisibleSearchResult(lstFailedStepPos[FailedStepPosIndex]);

                        // search result marks with Next/Previous color
                        rtbTestResult.Select(lstFailedStepPos[FailedStepPosIndex], lstStepFailedLength[FailedStepPosIndex]);
                        rtbTestResult.SelectionBackColor = System.Drawing.Color.LightPink;
                    }

                    FailedStepSearchDone = true;
                }
                ///////////////////////////////////////////////////////////////////////////////
                // This code block navigates to previous failed step. 
                // This is part of navigate functionality.
                else
                {
                    // after last failed step go to first failed step
                    if (FailedStepPosIndex == 0 || FailedStepPosIndex == -1)
                    {
                        FailedStepPosIndex = lstFailedStepPos.Count;
                    }

                    FailedStepPos = lstFailedStepPos[--FailedStepPosIndex];

                    // highlight previous failed step
                    rtbTestResult.Select(FailedStepPos, lstStepFailedLength[FailedStepPosIndex]);
                    rtbTestResult.SelectionBackColor = System.Drawing.Color.LightPink;

                    int findPosNextIndex = -1;

                    // return next color for next failde step
                    if ((FailedStepPosIndex + 1 <= lstFailedStepPos.Count - 1 || FailedStepPosIndex == lstFailedStepPos.Count - 1) && lstFailedStepPos.Count != 1)
                    {
                        findPosNextIndex = FailedStepPosIndex == lstFailedStepPos.Count - 1 ? 0 : FailedStepPosIndex + 1;
                        rtbTestResult.Select(lstFailedStepPos[findPosNextIndex], lstStepFailedLength[findPosNextIndex]);
                        rtbTestResult.SelectionBackColor = System.Drawing.SystemColors.Control;
                    }

                    scrollToInvisibleSearchResult(FailedStepPos);
                }
            }

        }

        private void cmsTestLogMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // "Select All" is available when there is some text
            if (rtbTestResult.TextLength != 0)
                cmsTestLogMenu.Items[2].Enabled = true;
            else
                cmsTestLogMenu.Items[2].Enabled = false;

            // "Copy" is avaiable when some text is selected
            if (rtbTestResult.SelectedText == "")
                cmsTestLogMenu.Items[0].Enabled = false;
            else
                cmsTestLogMenu.Items[0].Enabled = true;
        }

        private void cmsTestLogMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Copy
            if (e.ClickedItem == cmsTestLogMenu.Items[0])
            {
                rtbTestResult.Copy();
            }
            // Select All
            else if (e.ClickedItem == cmsTestLogMenu.Items[2])
            {
                rtbTestResult.Focus();
                rtbTestResult.SelectAll();
            }
        }

        private void tbFind_TextChanged(object sender, EventArgs e)
        {
            // return to default colors
            tbFind.BackColor = System.Drawing.SystemColors.Window;
            tbFind.ForeColor = System.Drawing.SystemColors.WindowText;

            int CursorPosition = rtbTestResult.SelectionStart;

            string searchText = tbFind.Text;
            searchTextLength = searchText.Length;

            // we should reset previous serach results before new search
            rtbTestResult.Select(0, rtbTestResult.Text.Length);
            rtbTestResult.SelectionBackColor = System.Drawing.SystemColors.Control;
            lstFindPos.Clear();
            FindPosIndex = 0;
            FindPos = -1;

            FailedStepSearchDone = false;

            if (searchText != "")
            {
                int posFind = rtbTestResult.Find(searchText, RichTextBoxFinds.None);

                while (posFind >= 0)
                {
                    // add to list found position
                    lstFindPos.Add(posFind);

                    rtbTestResult.Select(posFind, searchTextLength);
                    rtbTestResult.SelectionBackColor = System.Drawing.Color.Yellow;

                    posFind = rtbTestResult.Find(searchText, posFind + 1, RichTextBoxFinds.None);
                }

                if (lstFindPos.Count > 0)
                {
                    // if cursor is in the end of text (by default)
                    // then navigates to first search result
                    if (CursorPosition == rtbTestResult.TextLength)
                    {
                        // scroll to first found line if it's invisible
                        scrollToInvisibleSearchResult(lstFindPos[0]);

                        // first search result marks with Next/Previous color
                        rtbTestResult.Select(lstFindPos[0], searchTextLength);
                        rtbTestResult.SelectionBackColor = System.Drawing.Color.Orange;

                        FindPosIndex = 0;
                    }
                    // if cursor is in the other location
                    // then navigates to nearest search result after cursor location 
                    else
                    {
                        for (int i = 0; i < lstFindPos.Count; i++)
                        {
                            if (lstFindPos[i] >= CursorPosition)
                            {
                                FindPosIndex = i;
                                break;
                            }

                            // if we don't find search result after cursor location
                            // then take last found result
                            if (i == lstFindPos.Count - 1)
                            {
                                FindPosIndex = i;
                            }
                        }

                        // scroll to found line if it's invisible
                        scrollToInvisibleSearchResult(lstFindPos[FindPosIndex]);

                        // search result marks with Next/Previous color
                        rtbTestResult.Select(lstFindPos[FindPosIndex], searchTextLength);
                        rtbTestResult.SelectionBackColor = System.Drawing.Color.Orange;
                    }
                }

                // if there is nothing found then marks search field as red
                if (lstFindPos.Count == 0)
                {
                    tbFind.BackColor = System.Drawing.Color.FromArgb(255, 102, 102);
                    tbFind.ForeColor = System.Drawing.Color.White;
                }
            }
        }

        private void tbFind_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFind.Text))
            {
                // return to default colors
                tbFind.BackColor = System.Drawing.SystemColors.Window;
                tbFind.ForeColor = System.Drawing.SystemColors.WindowText;

                string text = tbFind.Text;
                tbFind.Text = "";
                tbFind.Text = text;
            }
        }

    }
}
