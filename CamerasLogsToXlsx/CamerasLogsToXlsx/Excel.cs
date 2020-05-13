using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using System.Xml;

namespace CamerasLogsToXlsx
{
    public class Excel
    {
        public int RowNumber = 1;
        public int LastRow = 1;
        public char Column = 'Z';
        public int ColumnNumber = 1;

        public String cellNumberColumnA;
        public String cellNumberColumnB;
        public String cellNumberColumnC;
        public String cellNumberColumnX;

        public FileInfo newFile;
        public ExcelPackage pck;
        public ExcelWorksheet ws;

        List<String> testIds;

        public Excel(String ExcelPath)
        {
            newFile = new FileInfo(ExcelPath);
            pck = new ExcelPackage(newFile);
        }

        public void InitializeExcelWorksheet()
        {
            ws = pck.Workbook.Worksheets.Add("Cameras");
            ws.View.ShowGridLines = true;

            cellNumberColumnA = "A1";
            cellNumberColumnB = "B1";
            cellNumberColumnC = "C1";

            //Headers
            ws.Cells[cellNumberColumnA].Value = "TestGroup";
            ws.Cells[cellNumberColumnA].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnA].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnA].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[cellNumberColumnB].Value = "Test ID";
            ws.Cells[cellNumberColumnB].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnB].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnB].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[cellNumberColumnC].Value = "Test Name";
            ws.Cells[cellNumberColumnC].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnC].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnC].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
        }

        public void LoadWorksheet()
        {
            pck = new ExcelPackage(newFile);
            ws = pck.Workbook.Worksheets[1];
        }

        public void DefineColumn(String columnName)
        {
            for (char col = 'A'; col < 'Z'; col++)
            {
                if (ws.Cells["" + col + 1].Text == columnName)
                {
                    Column = col;

                    break;
                }
                ColumnNumber++;
            }
        }

        public void DefineColumn()
        {
            DefineColumn("");
        }

        

        protected void WriteInCell(String cellNumber, String text)
        {
            ws.Cells[cellNumber].Value = text;
        }

        public void Save()
        {
            pck.Save();
        }

        public void DefineLastRow()
        {
            for (int i = 1; i < 1000; i++)
            {

                if (ws.Cells["B" + i].Text == "")
                {
                    LastRow = i - 1;
                    break;
                }
            }

        }

        public void TestIdsList()
        {
            testIds = new List<String>();
            for (int j = 2; j <= LastRow; j++)
            {
                testIds.Add(ws.Cells["B" + j].Value.ToString());
            }
        }

        public void CameraHeader(String name)
        {
            String cellNumberColumnX = "" + Column + "1";
            ws.Cells[cellNumberColumnX].Value = name;
            ws.Cells[cellNumberColumnX].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnX].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnX].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
        }

        public void FillAllFirstThreeColumns(XmlDoc myXml)
        {
            foreach (XmlNode TestResult in myXml.TestResults)
            {
                RowNumber++;
                FillThreeColumns(TestResult, RowNumber);
            }
        }

        private void FillThreeColumns(XmlNode TestResult, int row)
        {
            cellNumberColumnA = "A" + row;
            cellNumberColumnB = "B" + row;
            cellNumberColumnC = "C" + row;

            //ws.Cells[cellNumberColumnA].Value = TestResult.SelectSingleNode("./TestInfo/Group").InnerText;
            //ws.Cells[cellNumberColumnB].Value = TestResult.SelectSingleNode("./TestInfo/Category").InnerText + "-" + TestResult.SelectSingleNode("./TestInfo/Id").InnerText;
            //ws.Cells[cellNumberColumnC].Value = TestResult.SelectSingleNode("./TestInfo/Name").InnerText;

            WriteInCell(cellNumberColumnA, TextChildNode(TestResult, "./TestInfo/Group"));
            WriteInCell(cellNumberColumnB, TextChildNode(TestResult, "./TestInfo/Category") + "-" + TextChildNode(TestResult, "./TestInfo/Id"));
            WriteInCell(cellNumberColumnC, TextChildNode(TestResult, "./TestInfo/Name"));
        }

        public void DefineCurrentRowNumber(XmlNode TestResult)
        {
            String id = TextChildNode(TestResult, "./TestInfo/Category") + "-" + TextChildNode(TestResult, "./TestInfo/Id");

            bool idFound = testIds.Contains(id);
            if (idFound)
            {
                RowNumber = testIds.IndexOf(id) + 2;
            }
            else
            {
                RowNumber = LastRow + 1;
                FillThreeColumns(TestResult, RowNumber);
                testIds.Add(ws.Cells[cellNumberColumnB].Value.ToString());
                LastRow++;
            }
        }

        private String TextChildNode(XmlNode result, String xPath)
        {
            return result.SelectSingleNode(xPath).InnerText;
        }

        public String DefineCellNumber()
        {
            return "" + Column + RowNumber;
        }

        public void FillTestStatus(XmlNode result)
        {
            String state = TextChildNode(result, "./Log/TestStatus");

            ws.Cells[DefineCellNumber()].Value = state.ToUpper();
            ws.Cells[DefineCellNumber()].Style.Font.Bold = true;

            if (state.Equals("Passed"))
            {
                //ws.Cells[cellNumber].Value = "PASSED";
                ws.Cells[DefineCellNumber()].Style.Font.Color.SetColor(Color.Green);
            }
            else
            {
                ws.Cells[DefineCellNumber()].Style.Font.Color.SetColor(Color.Red);
            }
        }

        public void FillExplanation(XmlNode result)
        {
            XmlNodeList StepsPotentiallyFailed = result.SelectNodes("./Log/Steps/StepResult");
            String Explanation = "";

            XmlNode FailedStep = StepsPotentiallyFailed.Item(0);

            //foreach (XmlNode Step in StepsPotentiallyFailed)
            //{
            //    if (Step["Status"].InnerText == "Failed")
            //    {
            //        FailedStep = Step;
            //        Explanation += "STEP " + FailedStep["Number"].InnerText + " " + FailedStep["StepName"].InnerText + " " + FailedStep["Message"].InnerText + " " + FailedStep["Status"].InnerText + "; ";
            //    }
            //}


            //ws.Cells[cellNumber].Value += " " + Explanation;
            if (Explanation != "")
            {
                ws.Cells[DefineCellNumber()].AddComment(Explanation, "Auto");
            }
        }

        public void FinalFormat()
        {
            ExcelRange all = ws.Cells["A1:" + Column + LastRow];

            all.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            all.Style.Border.Bottom.Color.SetColor(Color.Black);
            all.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            all.Style.Border.Right.Color.SetColor(Color.Black);
            all.Style.Border.BorderAround(ExcelBorderStyle.Thick, Color.Black);


            ws.Column(1).AutoFit();
            ws.Column(2).AutoFit();
            ws.Column(3).Width = 90.0;
            ws.Column(3).Style.WrapText = true;
            ws.Column(ColumnNumber).Width = 15.0;
            ws.Column(ColumnNumber).Style.WrapText = true;
            ws.Column(ColumnNumber).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
    }
}
