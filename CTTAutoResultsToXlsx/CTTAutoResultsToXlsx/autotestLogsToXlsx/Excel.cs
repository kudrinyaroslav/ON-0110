using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
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
        public int ColumnNumber = 6;

        public String cellNumber;

        public FileInfo newFile;
        public ExcelPackage pck;
        public ExcelWorksheet ws;

        public Excel(String ExcelPath)
        {
            newFile = new FileInfo(ExcelPath);
            pck = new ExcelPackage(newFile);
        }

        private String getNextCellColumn(String cellColumn)
        {
            char col = cellColumn[0];
            col++;
            return (col + cellColumn.Substring(1, cellColumn.Length - 1));
        }
        public void InitializeExcelWorksheet()
        {
            ws = pck.Workbook.Worksheets.Add("Test Report");
            ws.View.ShowGridLines = true;
            char col = 'A';
            cellNumber = "" + col + 1;
            //Headers
            ws.Cells[cellNumber].Value = "ID";
            col++; cellNumber = "" + col + 1;
            ws.Cells[cellNumber].Value = "Name";
            col++; cellNumber = "" + col + 1;
            ws.Cells[cellNumber].Value = "Description";
            col++; cellNumber = "" + col + 1;
            ws.Cells[cellNumber].Value = "RelatedItems";
            col++; cellNumber = "" + col + 1;
            ws.Cells[cellNumber].Value = "Expected Result";
            col++; cellNumber = "" + col + 1;
            ws.Cells[cellNumber].Value = "Result";
            ws.Cells["A1:" + cellNumber].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A1:" + cellNumber].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells["A1:" + cellNumber].Style.Font.Bold = true;
        }

        public void LoadWorksheet()
        {
            pck = new ExcelPackage(newFile);
            ws = pck.Workbook.Worksheets[1];
        }

        public void DefineColumn(String columnName)
        {
            ColumnNumber = 0;
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
                if (ws.Cells["E" + i].Text == "" && ws.Cells["A" + i].Text == "")//id поле и поле expected result
                {
                    LastRow = i - 1;
                    break;
                }
            }
        }

        public void FillTestCasesResults(XmlDoc myXml)//записываем все что есть в xml файле в excel
        {
            RowNumber = LastRow + 1;
            cellNumber = "A" + RowNumber;
            WriteInCell(cellNumber, myXml.GetXmlName());//записываем имя файла авто-тест репорта
            ws.Cells[cellNumber].Style.Font.Bold = true;
            ws.Cells[(cellNumber + ":F" + RowNumber)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[(cellNumber + ":F" + RowNumber)].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);//TODO "F" заменить на (column) после вызова defineColumn(result) ;
            ws.Cells[(cellNumber + ":F" + RowNumber)].Merge = true;

            RowNumber++;
            foreach (XmlNode TestResult in myXml.TestResults)
            {
                FillColumns(TestResult, RowNumber);
            }
        }
        private void mergeCells(string nameOfColumn, int firstRow,int LastRow)
        {
            DefineColumn(nameOfColumn);
            cellNumber = "" + Column + firstRow;
            ws.Cells[(cellNumber + ":" + Column + LastRow)].Merge = true;
            ws.Cells[(cellNumber + ":" + Column + LastRow)].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        }
        private void FillColumns(XmlNode TestResult, int row)
        {
            int firstNumOfTestRow = row;
            cellNumber = "A" + row;
            WriteInCell(cellNumber, TextChildNode(TestResult, "id"));
            cellNumber = getNextCellColumn(cellNumber);
            WriteInCell(cellNumber, TextChildNode(TestResult, "name"));
            cellNumber = getNextCellColumn(cellNumber);
            WriteInCell(cellNumber, TextChildNode(TestResult, "description").Trim());
            cellNumber = getNextCellColumn(cellNumber);
            WriteInCell(cellNumber, TextChildNode(TestResult, "relatedItems/string"));
            cellNumber = getNextCellColumn(cellNumber);
            FillTestStatus(TestResult);

            DefineLastRow();
            mergeCells("ID", firstNumOfTestRow, LastRow);
            mergeCells("Name", firstNumOfTestRow,LastRow);
            mergeCells("Description", firstNumOfTestRow, LastRow);
            mergeCells("RelatedItems", firstNumOfTestRow, LastRow);
        }

        private String TextChildNode(XmlNode result, String xPath)
        {
            return result.SelectSingleNode(xPath).InnerText;
        }

        public String DefineCellNumber()
        {
            return "" + Column + RowNumber;
        }
        public void compareResults(XmlNode result, String checkNode, Color relatedColor)//функция, которая заполнаяет последние два столбца
        {
            if (result.SelectSingleNode(checkNode).HasChildNodes)
            {
                foreach (XmlNode childNode in result.SelectNodes(checkNode + "/LogData"))
                {
                    String expectedResult = childNode.SelectSingleNode("expectedResult").InnerText;
                    String stringResult = childNode.SelectSingleNode("result").InnerText;
                    DefineColumn("Expected Result");
                    ws.Cells[DefineCellNumber()].Value = expectedResult;
                    ws.Cells[DefineCellNumber()].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[DefineCellNumber()].Style.Fill.BackgroundColor.SetColor(relatedColor);
                    DefineColumn("Result");
                    if (stringResult.Equals("PASSED"))
                    {
                        ws.Cells[DefineCellNumber()].Value = "PASSED";
                        ws.Cells[DefineCellNumber()].Style.Font.Color.SetColor(Color.Green);
                    }
                    else
                    {
                        ws.Cells[DefineCellNumber()].Value = "FAILED";
                        ws.Cells[DefineCellNumber()].Style.Font.Color.SetColor(Color.Red);
                    }
                    ws.Cells[DefineCellNumber()].Style.Font.Bold = true;
                    RowNumber++;
                }
            }
        }
        public void FillTestStatus(XmlNode result)
        {
            String checkNode = "check";
            compareResults(result, checkNode, Color.LightSkyBlue);
            checkNode = "checkFeatures";
            compareResults(result, checkNode, Color.LightCyan);
            checkNode = "checkProfiles";
            compareResults(result, checkNode, Color.LightYellow);
        }

        public void FillExplanation(XmlNode result)
        {
            XmlNodeList StepsPotentiallyFailed = result.SelectNodes("./Log/Steps/StepResult");
            String Explanation = "";

            XmlNode FailedStep = StepsPotentiallyFailed.Item(0);

            foreach (XmlNode Step in StepsPotentiallyFailed)
            {
                if (Step["Status"].InnerText == "Failed")
                {
                    FailedStep = Step;
                    Explanation += "STEP " + FailedStep["Number"].InnerText + " " + FailedStep["StepName"].InnerText + " " + FailedStep["Message"].InnerText + " " + FailedStep["Status"].InnerText + "; ";
                }
            }

            //ws.Cells[cellNumber].Value += " " + Explanation;
            if (Explanation != "")
            {
                ws.Cells[DefineCellNumber()].AddComment(Explanation, "Auto");
            }
        }

        public void FinalFormat()
        {
            DefineColumn("Result");
            ExcelRange all = ws.Cells["A1:" + Column + LastRow];

            all.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            all.Style.Border.Bottom.Color.SetColor(Color.Black);
            all.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            all.Style.Border.Right.Color.SetColor(Color.Black);
            all.Style.Border.BorderAround(ExcelBorderStyle.Thick, Color.Black);

            for (int i = 0; i < ColumnNumber; i++)//
            {
                ws.Column(i + 1).Width = 30;
                ws.Column(i + 1).Style.WrapText = true;
            }
            ws.Column(2).Width = 60;
            ws.Column(3).Width = 60;
            //ws.Column(ColumnNumber).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
    }
}
