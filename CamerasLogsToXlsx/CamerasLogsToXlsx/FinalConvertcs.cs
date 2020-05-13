using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CamerasLogsToXlsx
{
    public class FinalConvertcs
    {
        XmlDoc myDoc;
        Excel myExcel;

        public FinalConvertcs()
        { }

        public void Convert(List<String> pathList, String excelPath)
        {
            foreach (String path in pathList)
            {
                myDoc = new XmlDoc(path);
                myExcel = new Excel(excelPath);

                if (!File.Exists(excelPath))
                {
                    myExcel.InitializeExcelWorksheet();
                    myExcel.FillAllFirstThreeColumns(myDoc);
                    myExcel.Save();
                }

                myExcel.LoadWorksheet();

                //Define Column
                if (myDoc.isAppend())
                {
                    myExcel.DefineColumn(myDoc.GetXmlName());
                }
                else 
                {
                    myExcel.DefineColumn();
                }

                //Define Last row
                myExcel.DefineLastRow();

                //Create list of Ids in excel
                myExcel.TestIdsList();

                //Fill Header for chosen camera (one xml - one camera)
                myExcel.CameraHeader(myDoc.GetXmlName());


                foreach (XmlNode TestResult in myDoc.TestResults)
                {
                    myExcel.DefineCurrentRowNumber(TestResult);

                    myExcel.FillTestStatus(TestResult);

                    myExcel.FillExplanation(TestResult);
                }

                myExcel.FinalFormat();

                myExcel.Save();
            }
        }
    }
}
