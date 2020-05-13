using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection; 
using System.Xml;

namespace ONVIFTestTool
{
    /// <summary>
    /// Class for gettint specification from xml test suit file
    /// </summary>
    class NVTTestDescription
    {
        #region Constructors

        public NVTTestDescription(TestSuit testSuit)
        {
            m_testSuit = testSuit;
        }

        #endregion //Constructors

        #region Methods

        public string CreateExcelDescription()
        {
            string result = null;
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
                oSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove;

                for (int i = 2; oWB.Sheets.Count > 1; i++)
                {
                    ((Excel._Worksheet)oWB.Sheets[2]).Delete();
                }

                //Add Info about test suit
                oSheet.Name = "Info";

                oSheet.Cells[1, 1] = "Name:";
                oSheet.Cells[1, 2] = m_testSuit.Name;
                oSheet.get_Range("B1", "F1").Merge(Type.Missing);
                oSheet.get_Range("A1", "F1").Font.Bold = true;
                oSheet.get_Range("A1", "F1").Font.Size = 16;
                oSheet.get_Range("A1", "F1").Font.Size = 16;
                oSheet.get_Range("A1", "F1").Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                oSheet.get_Range("A1", "F1").Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                oSheet.get_Range("A1", "F1").Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                oSheet.get_Range("A1", "F1").Interior.TintAndShade = -0.349986266670736;
                oSheet.get_Range("A1", "F1").Interior.PatternTintAndShade = 0;

                oSheet.Cells[2, 1] = "Version:";
                oSheet.Cells[2, 2] = m_testSuit.Version;
                oSheet.get_Range("B2", "F2").Merge(Type.Missing);

                oSheet.Cells[3, 1] = "Date:";
                oSheet.Cells[3, 2] = m_testSuit.Date;
                oSheet.get_Range("B3", "F3").Merge(Type.Missing);

                oSheet.Cells[4, 1] = "Camera Model:";
                oSheet.Cells[4, 2] = m_testSuit.CamraModel;
                oSheet.get_Range("B4", "F4").Merge(Type.Missing);

                oSheet.get_Range("A2", "A4").Font.Bold = true;
                oSheet.get_Range("B2", "F4").HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                oRng = oSheet.get_Range("A1", "F1");
                oRng.EntireColumn.AutoFit();
                
                XmlNode xmlTestList = m_testSuit.XMLDocument.SelectSingleNode("/TestSuit/TestList");

                //Add servises
                foreach (XmlNode service in xmlTestList.ChildNodes)
                {
                    oSheet = (Excel._Worksheet)oWB.Sheets.Add(Type.Missing, oSheet, Type.Missing, Excel.XlSheetType.xlWorksheet);
                    oSheet.Name = service.Attributes.GetNamedItem("name").InnerText;
                    oSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove;

                    //Add service caption
                    oSheet.Cells[1, 1] = "[" + service.Attributes.GetNamedItem("id").InnerText + "]";
                    oSheet.Cells[1, 2] = service.Attributes.GetNamedItem("name").InnerText;
                    oSheet.get_Range("B1", "F1").Merge(Type.Missing);
                    oSheet.get_Range("A1", "F1").Font.Bold = true;
                    oSheet.get_Range("A1", "F1").Font.Size = 16;
                    oSheet.get_Range("A1", "F1").Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                    oSheet.get_Range("A1", "F1").Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                    oSheet.get_Range("A1", "F1").Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    oSheet.get_Range("A1", "F1").Interior.TintAndShade = -0.349986266670736;
                    oSheet.get_Range("A1", "F1").Interior.PatternTintAndShade = 0;

                    //Add table header
                    oSheet.Cells[3, 1] = "ID";
                    oSheet.Cells[3, 2] = "Command under test";
                    oSheet.Cells[3, 3] = "Title";
                    oSheet.Cells[3, 4] = "Implemented";
                    oSheet.get_Range("A3", "F3").Font.Bold = true;
                    oSheet.get_Range("A3", "F3").Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                    oSheet.get_Range("A3", "F3").Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                    oSheet.get_Range("A3", "F3").Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    oSheet.get_Range("A3", "F3").Interior.TintAndShade = -0.349986266670736;
                    oSheet.get_Range("A3", "F3").Interior.PatternTintAndShade = 0;

                    int i = 4;

                    foreach (XmlNode testGroup in service.ChildNodes)
                    {
                        if (testGroup.NodeType != XmlNodeType.Comment)
                        {
                            oSheet.Cells[i, 1] = "[" + testGroup.Attributes.GetNamedItem("id").InnerText + "]";
                            oSheet.Cells[i, 2] = testGroup.Attributes.GetNamedItem("command").InnerText;
                            oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Font.Bold = true;
                            oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                            oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                            oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                            oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.TintAndShade = -0.149998474074526;
                            oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.PatternTintAndShade = 0;
                            i++;

                            int testGroupBegin = i;
                            foreach (XmlNode test in testGroup.ChildNodes)
                            {
                                oSheet.Cells[i, 1] = "[" + test.Attributes.GetNamedItem("id").InnerText + "]";
                                oSheet.Cells[i, 2] = testGroup.Attributes.GetNamedItem("command").InnerText;
                                if (test.FirstChild.Name == "Name")
                                {
                                    oSheet.Cells[i, 3] = test.FirstChild.InnerText;
                                }
                                oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Font.Bold = true;
                                oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                                oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;
                                oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent1;
                                oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.TintAndShade = 0.799981688894314;
                                oSheet.get_Range(oSheet.Cells[i, 1], oSheet.Cells[i, 6]).Interior.PatternTintAndShade = 0;

                                i++;
                                int testBegin = i;
                                oSheet.Cells[i, 2] = "Description:";
                                oSheet.get_Range(oSheet.Cells[i, 2], oSheet.Cells[i, 2]).Font.Bold = true;
                                oSheet.get_Range(oSheet.Cells[i, 2], oSheet.Cells[i, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                oSheet.get_Range(oSheet.Cells[i, 2], oSheet.Cells[i, 2]).VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                                oSheet.Cells[i, 3] = test.SelectSingleNode("Description").InnerText;
                                i++;

                                foreach (XmlNode step in test.SelectNodes("Step"))
                                {
                                    oSheet.Cells[i, 2] = "Step " + step.Attributes.GetNamedItem("id").InnerText;
                                    oSheet.Cells[i, 3] = step.PreviousSibling.InnerText;
                                    i++;
                                    oSheet.Cells[i, 2] = "Request file:";
                                    oSheet.Cells[i, 3] = step.Attributes.GetNamedItem("fileRequest").InnerText;
                                    i++;
                                    oSheet.Cells[i, 2] = "Response file:";
                                    oSheet.Cells[i, 3] = step.Attributes.GetNamedItem("fileAnswer").InnerText;
                                    i++;
                                    oSheet.get_Range(oSheet.Cells[i - 3, 3], oSheet.Cells[i - 3, 3]).Font.Bold = true;
                                    oSheet.get_Range(oSheet.Cells[i - 3, 2], oSheet.Cells[i - 1, 2]).Font.Bold = true;
                                    oSheet.get_Range(oSheet.Cells[i - 2, 1], oSheet.Cells[i - 1, 1]).Rows.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                                }

                                oSheet.get_Range(oSheet.Cells[testBegin, 1], oSheet.Cells[i - 1, 1]).Rows.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            }
                            oSheet.get_Range(oSheet.Cells[testGroupBegin, 1], oSheet.Cells[i - 1, 1]).Rows.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        }

                    }

                    
                    oRng = oSheet.get_Range("A1", "F1000");
                    oRng.EntireColumn.AutoFit();
                    oRng.EntireRow.AutoFit();
                    oSheet.Outline.ShowLevels(3, Type.Missing);
                    oSheet.Outline.ShowLevels(2, Type.Missing);
                    oSheet.Outline.ShowLevels(1, Type.Missing);

                }

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        #endregion //Methods

        #region Fields

        private TestSuit m_testSuit = null;

        #endregion //Fields
    }
}
