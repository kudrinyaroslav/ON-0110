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


namespace CTT_CovarageMap
{
    public class ExcelFile
    {
        private FileInfo newFile;
        private ExcelPackage pck;
        private int currentRow = 1;

        public ExcelFile(String ExcelPath)
        {
            newFile = new FileInfo(ExcelPath);
            pck = new ExcelPackage(newFile);
        }

        public void AddFeatureListWS()
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Test Cases"))
            {
                pck.Workbook.Worksheets.Delete("Test Cases");
            }
            pck.Workbook.Worksheets.Add("Test Cases");
            pck.Workbook.Worksheets["Test Cases"].OutLineSummaryBelow = false;

            if (pck.Workbook.Worksheets.Any(C => C.Name == "Profile Spec"))
            {
                foreach (var cell in pck.Workbook.Worksheets["Profile Spec"].Cells[2, 3, 300, 4])
                {
                    cell.Value = "";
                }
            }
        }

        public void AddHeader_FeatureList()
        {
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 1].Value = "Id";
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 2].Value = "Name";
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 3].Value = "Requirement Level";
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 4].Value = "Feature Under Test";
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 5].Value = "Link to Profile";
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Font.Size = 14;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Font.Bold = true;
            currentRow++;
        }

        public void AddSection(string sectionName)
        {
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 1].Value = sectionName;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Font.Bold = true;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Font.Size = 14;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Fill.PatternType = ExcelFillStyle.Solid;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Fill.BackgroundColor.SetColor(Color.DarkGray); ;
            currentRow++;
        }

        public void AddFeature(Feature feature)
        {
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 1].Value = feature.Id;
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 2].Value = feature.Name;
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 3].Value = feature.RequirementLevel.ToString();
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 5].Value = feature.LinkToProfile;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).OutlineLevel = 1;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Font.Bold = true;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Fill.PatternType = ExcelFillStyle.Solid;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            AddLinkToProfile(feature);
            currentRow++;
        }

        public void AddTestCase(TestCase testCase)
        {
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 1].Value = testCase.Id;
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 2].Value = testCase.Name;
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 3].Value = testCase.RequirementLevel.ToString();
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 4].Value = testCase.Subfeature;
            pck.Workbook.Worksheets["Test Cases"].Cells[currentRow, 5].Value = testCase.LinkToProfile;
            pck.Workbook.Worksheets["Test Cases"].Row(currentRow).OutlineLevel = 2;
            AddLinkToProfile(testCase);
            currentRow++;
        }

        public void AddLinkToProfile(Feature feature)
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Profile Spec") & feature.LinkToProfile != null)
            {
                var linksList = feature.LinkToProfile.Split(';');
                var profileFeatureList = pck.Workbook.Worksheets["Profile Spec"].Cells[1, 1, 300, 1].Where(C => linksList.Contains(C.Value));
                foreach (var cell in profileFeatureList)
                {
                    var oldText = pck.Workbook.Worksheets["Profile Spec"].Cells[cell.Start.Row, 3].Text;

                    pck.Workbook.Worksheets["Profile Spec"].Cells[cell.Start.Row, 3].Value = oldText == "" ? feature.Id : oldText + "; " + feature.Id;
                }
            }
        }

        public void AddLinkToProfile(TestCase testCase)
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Profile Spec") & testCase.LinkToProfile != null)
            {
                var linksList = testCase.LinkToProfile.Split(';');
                foreach (var cell in pck.Workbook.Worksheets["Profile Spec"].Cells[1, 1, 300, 1].Where(C => linksList.Contains(C.Value)))
                {
                    var oldText = pck.Workbook.Worksheets["Profile Spec"].Cells[cell.Start.Row, 4].Text;
                    pck.Workbook.Worksheets["Profile Spec"].Cells[cell.Start.Row, 4].Value = oldText == "" ? testCase.Id : oldText + "; " + testCase.Id;
                }
            }
        }

        public void AddEventsNotes()
        {
            if (pck.Workbook.Worksheets.Any(C => C.Name == "Profile Spec"))
            {
                foreach (var cell in pck.Workbook.Worksheets["Profile Spec"].Cells[1, 1, 300, 1].Where(C => C.Value.ToString().StartsWith("tns1:")))
                {
                    pck.Workbook.Worksheets["Profile Spec"].Cells[cell.Start.Row, 4].Value = "To be covered by checkbox.";
                }
            }
        }

        public void Save()
        {
            pck.Workbook.Worksheets["Test Cases"].Cells.AutoFitColumns();
            pck.Save();
        }
    }
}
