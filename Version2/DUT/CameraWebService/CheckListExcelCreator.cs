using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;

//this file helps to create check list report in folder specified in excelPath in this file
namespace DUT.CameraWebService
{
    public class CheckListExcelCreator
    {
        public int RowNumber = 1;
        public int LastRow = 1;
        public char Column = 'C';
        public int ColumnNumber = 1;

        public String cellNumberColumnA;
        public String cellNumberColumnB;
        public String cellNumberColumnC;
        public String cellNumberColumnX;

        public FileInfo newFile;
        public ExcelPackage pck;
        public ExcelWorksheet ws;

        List<String> testIds;
        List<String> testNames;
        String wbName;
        public bool FileRedyForWriting = true;
        public String errorMessage = "ERROR.";
       
        String excelPath = "C:\\Users\\Maria.Verkina\\Desktop\\ONVIF\\CheckList_DTT\\#";//chose your own folder where you place all other check list (use double slash '\\')
    
        public CheckListExcelCreator(String ticketId)
        {
            excelPath += ticketId + "_CheckList_" + DateTime.Now.Date.ToShortDateString().Replace(".", "-") + ".xlsx";
            newFile = new FileInfo(excelPath);

            try
            {
                if (File.Exists(excelPath))
                {
                    using (var fs = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        Console.WriteLine("файл свободен");
                    }
                }
                else
                {
                    using (var fs = File.Create(excelPath))
                    {
                        Console.WriteLine("Файл можно создать");
                    }
                    File.Delete(excelPath);
                }
                pck = new ExcelPackage(newFile);
            }
            catch (IOException ioex)
            {
                errorMessage = ioex.Message;
                FileRedyForWriting = false;
            }
        }

        public void InitializeExcelWorksheet()
        {
            wbName = testIds != null ? testIds.Last().Substring(0, testIds.Last().LastIndexOf('.')) : "Test List";
            ws = pck.Workbook.Worksheets[wbName] == null ? pck.Workbook.Worksheets.Add(wbName) : pck.Workbook.Worksheets[wbName];
            ws.View.ShowGridLines = true;

            cellNumberColumnA = "A1";
            cellNumberColumnB = "B1";
            cellNumberColumnC = "C1";

            //Headers
            ws.Cells[cellNumberColumnA].Value = "Test ID";
            ws.Cells[cellNumberColumnA].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnA].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnA].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[cellNumberColumnB].Value = "Test Name";
            ws.Cells[cellNumberColumnB].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnB].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnB].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[cellNumberColumnC].Value = "Test result";
            ws.Cells[cellNumberColumnC].Style.Font.Bold = true;
            ws.Cells[cellNumberColumnC].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[cellNumberColumnC].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
        }
        
        public void LoadTestsName(String[] tests)
        {
            int i=0;
            int position = 0;
            testIds = new List<String>();
            testNames = new List<String>();
            // Extract testIds and testNames from the tests array.
            while(tests[i]!="")
            {
                position = tests[i].IndexOf(' ');
                if (position >= 0)
                {
                    if (!tests[i].ToUpper().StartsWith("FEATURE")) //not add features for test case from check list
                    {
                        testIds.Add(tests[i].Substring(0, position));
                        testNames.Add(tests[i].Substring(position + 1));    
                    }
                    i++;
                }
            }
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
        public String CreateReport(String[] tests)
        {
            //if (!File.Exists(excelPath))
            
               LoadTestsName(tests);
               InitializeExcelWorksheet();
               FillThreeColumns(tests);
               Save();
            

            //myExcel.LoadWorksheet();

            //Define Last row
            DefineLastRow();
            //DefineColumn();
            //Create list of Ids in excel
            //myExcel.TestIdsList();

            //myExcel.DefineColumn("");

            FinalFormat();

            Save();
            return " Check list created. " + excelPath;
        }

        protected void WriteInCell(String cellNumber, String text)
        {
            ws.Cells[cellNumber].Value = text;
        }

        public void Save()
        {
            try
            {
                pck.Save();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw;
            }
            
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

        private void FillThreeColumns(String[] tests)
        {
            RowNumber = 2;
            foreach (String test in testIds)
            {
                cellNumberColumnA = "A" + RowNumber;
                WriteInCell(cellNumberColumnA, test);
                RowNumber++;
            }
            RowNumber = 2;
            foreach (String name in testNames)
            {
                cellNumberColumnB = "B" + RowNumber;
                cellNumberColumnC = "C" + RowNumber;
                WriteInCell(cellNumberColumnB, name);
                WriteInCell(cellNumberColumnC, "passed");
                ws.Cells[cellNumberColumnC].Style.Font.Color.SetColor(Color.Green);
                RowNumber++;
            }
        }

        public String DefineCellNumber()
        {
            return "" + Column + RowNumber;
        }

        public void FinalFormat()
        {
            ExcelRange all = ws.Cells["A1:" + Column + LastRow];

            all.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            all.Style.Border.Bottom.Color.SetColor(Color.Black);
            all.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            all.Style.Border.Right.Color.SetColor(Color.Black);


            ws.Column(1).Width = 25.0;
            ws.Column(3).Width = 20.0;
            ws.Column(2).Width = 150.0;
            //ws.Column(3).Style.WrapText = true;            
            ws.Column(ColumnNumber).Style.WrapText = true;
            ws.Column(ColumnNumber).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        }
    }
}
