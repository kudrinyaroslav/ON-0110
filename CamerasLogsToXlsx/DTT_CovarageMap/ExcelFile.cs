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


namespace DTT_CovarageMap
{
    public class ExcelFile
    {
        private FileInfo newFile;
        private ExcelPackage pck;
        private int currentRow_NotMappedTestCases = 1;
        private int currentRow_OtherTestCases = 1;

        public ExcelFile(String ExcelPath)
        {
            newFile = new FileInfo(ExcelPath);
            pck = new ExcelPackage(newFile);
        }

        public void AddTestCasesListWS()
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Profile Spec"))
            {
                foreach (var cell in pck.Workbook.Worksheets["Profile Spec"].Cells[3, 4, 300, 5])
                {
                    cell.Value = "";
                }
                
                pck.Workbook.Worksheets["Profile Spec"].Column(1).Width = 35;
                pck.Workbook.Worksheets["Profile Spec"].Column(2).Width = 10;
                pck.Workbook.Worksheets["Profile Spec"].Column(3).Width = 35;
                pck.Workbook.Worksheets["Profile Spec"].Column(4).Width = 75;
                pck.Workbook.Worksheets["Profile Spec"].Column(5).Width = 75;
            }
        }

        public List<ProfileFeature> GetProfileFeatureList()
        {
            List<ProfileFeature> res = null;
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Profile Spec"))
            {
                res = new List<ProfileFeature>();

                foreach (var cell in pck.Workbook.Worksheets["Profile Spec"].Cells[3, 3, 300, 3])
                {
                    if (cell.Text == null || cell.Text == "")
                    {
                        res.Add(new ProfileFeature(cell.Start.Row, pck.Workbook.Worksheets["Profile Spec"].Cells[cell.Start.Row, 1].Text));
                    }
                    else
                    {
                        res.Add(new ProfileFeature(cell.Start.Row, cell.Text));
                    }
                }
            }

            return res;
        }

        public void AddTestCases2Feature(ProfileFeature profileFeature, String testCases, bool conformanceMode)
        {
            if (conformanceMode)
            {
                pck.Workbook.Worksheets["Profile Spec"].Cells[profileFeature.Row, 4].Value = testCases;
                pck.Workbook.Worksheets["Profile Spec"].Cells[profileFeature.Row, 4].Style.WrapText = true;
            }
            else
            {
                pck.Workbook.Worksheets["Profile Spec"].Cells[profileFeature.Row, 5].Value = testCases;
                pck.Workbook.Worksheets["Profile Spec"].Cells[profileFeature.Row, 5].Style.WrapText = true;
            }
        }

        public void AddTestCasesWithoutFunctionality()
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Not Mapped Test Cases"))
            {
                pck.Workbook.Worksheets.Delete("Not Mapped Test Cases");
            }
            pck.Workbook.Worksheets.Add("Not Mapped Test Cases");
        }

        public void AddTestCasesOther()
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Other Test Cases"))
            {
                pck.Workbook.Worksheets.Delete("Other Test Cases");
            }
            pck.Workbook.Worksheets.Add("Other Test Cases");
        }

        public void AddTestCaseWithoutFunctionality(String testName)
        {
            pck.Workbook.Worksheets["Not Mapped Test Cases"].Cells[currentRow_NotMappedTestCases, 1].Value = testName;
            currentRow_NotMappedTestCases++;
        }

        public void AddTestCaseOther(String testName)
        {
            pck.Workbook.Worksheets["Other Test Cases"].Cells[currentRow_OtherTestCases, 1].Value = testName;
            currentRow_OtherTestCases++;
        }

        public void Save()
        {
            pck.Save();
        }
    }
}
