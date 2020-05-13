using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection; 

namespace ONVIFTestTool
{
    /// <summary>
    /// Class for test log operation
    /// </summary>
    class TestLog
    {
        #region Methods

        /// <summary>
        /// Add test result to test log
        /// </summary>
        /// <param name="testResult">Test result</param>
        public void AddTestResult(TestResult testResult)
        {
            m_testResultArray.Add(testResult);
        }

        /// <summary>
        /// Save log to Excel file
        /// </summary>
        /// <param name="pathToLog">Path to log file</param>
        public void SaveAsExcel(string pathToLog)
        {
            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                oSheet.Name = "TestResults";

                oSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove;

                for (int j = 2; oWB.Sheets.Count > 1; j++)
                {
                    ((Excel._Worksheet)oWB.Sheets[2]).Delete();
                }

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Service";
                oSheet.Cells[1, 2] = "Group";
                oSheet.Cells[1, 3] = "Test ID";
                oSheet.Cells[1, 4] = "Test Name";
                oSheet.Cells[1, 5] = "Result";
                oSheet.Cells[1, 6] = "Message";

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "F1").Font.Bold = true;
                oSheet.get_Range("A1", "F1").VerticalAlignment =
                    Excel.XlVAlign.xlVAlignCenter;

                oSheet.get_Range("A1", "F1").Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                oSheet.get_Range("A1", "F1").Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                oSheet.get_Range("A1", "F1").Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                oSheet.get_Range("A1", "F1").Interior.TintAndShade = -0.349986266670736;
                oSheet.get_Range("A1", "F1").Interior.PatternTintAndShade = 0;

                int i = 2;

                foreach (TestResult testResult in m_testResultArray)
                {
                    oSheet.Cells[i, 1] = testResult.Service;
                    oSheet.Cells[i, 2] = testResult.Group;
                    oSheet.Cells[i, 3] = testResult.TestId;
                    oSheet.Cells[i, 4] = testResult.TestName;
                    oSheet.Cells[i, 5] = testResult.ResultString;
                    oSheet.Cells[i, 6] = testResult.Message;

                    oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Font.Bold = true;
                    oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                    oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                    oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.TintAndShade = -0.149998474074526;
                    oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.PatternTintAndShade = 0;

                    oSheet.get_Range(oSheet.Cells[i, 3], oSheet.Cells[i, 3]).Font.Bold = true;
                    oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]).Font.Bold = true;

                    if (!testResult.Result)
                    {
                        oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]).Font.Color = -16776961;
                        oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]).Font.TintAndShade = 0;
                    }

                    i++;

                    int groupBegin = i;

                    oSheet.Cells[i, 4] = testResult.TestDescription;
                    i++;

                    foreach (TestStepResult testStepResult in testResult.m_testStepResultArray)
                    {
                        oSheet.Cells[i, 2] = testStepResult.Time.ToString() + " ms";
                        oSheet.Cells[i, 3] = testStepResult.StepId;
                        oSheet.Cells[i, 4] = testStepResult.StepDescription;
                        oSheet.Cells[i, 5] = testStepResult.ResultString;
                        oSheet.Cells[i, 6] = testStepResult.Message;

                        if (!testStepResult.Result)
                        {
                            oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]).Font.Color = -16776961;
                            oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]).Font.TintAndShade = 0;
                        }

                        i++;

                        oSheet.Hyperlinks.Add(oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]), testStepResult.PathToRequestFile, "", "", "Request File");
                        i++;
                        if (testStepResult.PathToRealResponseFile != "")
                        {
                            oSheet.Hyperlinks.Add(oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]), testStepResult.PathToRealResponseFile, "", "", "Response File");
                        }
                        else
                        {
                            oSheet.Cells[i, 5] = "No response";
                        }
                        i++;

                        if (testStepResult.PathToRealResponseFileFull != "")
                        {
                            oSheet.Hyperlinks.Add(oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]), testStepResult.PathToRealResponseFileFull, "", "", "Response File (Full)");
                        }
                        else
                        {
                            oSheet.Cells[i, 5] = "No response";
                        }
                        i++;


                        if (testStepResult.PathToCompareFile != "")
                        {
                            oSheet.Hyperlinks.Add(oSheet.get_Range(oSheet.Cells[i, 5], oSheet.Cells[i, 5]), testStepResult.PathToCompareFile, "", "", "Compare File");
                        }
                        else
                        {
                            oSheet.Cells[i, 5] = "No compare file";
                        }
                        i++;

                        oSheet.get_Range(oSheet.Cells[i - 4, 1], oSheet.Cells[i - 1, 1]).Rows.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    oSheet.get_Range(oSheet.Cells[groupBegin, 1], oSheet.Cells[i - 1, 1]).Rows.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                }
                
                //AutoFit columns A:D.
                oRng = oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[i, 6]);
                oRng.EntireColumn.AutoFit();
                oRng.EntireRow.AutoFit();
                oSheet.Outline.ShowLevels(2, Type.Missing);
                oSheet.Outline.ShowLevels(1,Type.Missing);

//                oWB.Save();
                oWB.SaveAs(pathToLog, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;

                
            }
            catch
            {
                //throw theException;
            }
        }
    

        #endregion //Methods

        #region Fields

        private List<TestResult> m_testResultArray = new List<TestResult>();

        #endregion //Fields
    }
}
