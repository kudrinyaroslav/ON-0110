using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PDFforONVIFxample
{
    class PDFCreater
    {

        public PDFCreater()
        {
        }

        private Document m_Document;
        public string m_logoPath;

        public void Save(string in_FileName)
        {
            //Создаем документ формата A4
            m_Document = new Document(PageSize.A4);

            // create a new instance of the results file
            PdfWriter.GetInstance(m_Document, new FileStream(in_FileName, FileMode.Create));

            m_Document.Open();

            //Создаем Footer и присваиваем (аналогично можно сделать Header)
            m_Document.Footer = HeaderAndFooterCreater();

            //Добавление стартовой страницы
            BuildCoverPage();
            
            //Добавление суммарной информации
            BuildSummary();

            //Дабавление содержания
            BuildIndex();

            //Добавление тестов
            AddTestGroupInfo();

            m_Document.Close();
                
        }

        private HeaderFooter HeaderAndFooterCreater()
        {
            //Создаем заголовок с номером страницы и добавляем туда текст
            //Заголовок появиться со второй страницы
            HeaderFooter footer = new HeaderFooter(new Phrase("Device - " + "<DEVICE_MODEL>" + " " + "<DATA_AND_TIME_OF_TEST>" + " ONVIF Test Report Page: "), true);
            //колонтитул без границы
            footer.Border = Rectangle.NO_BORDER;
            //Выравнен по центру
            footer.Alignment = Element.ALIGN_CENTER;

            return footer;
        }

        private void BuildCoverPage()
        {

            //Добавление логотипа
            Image logo = Image.GetInstance(m_logoPath);
            logo.Alignment = Element.ALIGN_CENTER;
            m_Document.Add(logo);

            //Добавление пустых строчек
            m_Document.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
            m_Document.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
            m_Document.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

            //Создание нового параграфа (позже он добавиться в документ)
            //Chunk - это кусок текста с заданым форматированием
            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p0.Add(new Chunk("ONVIF Conformance Test", FontFactory.GetFont(FontFactory.TIMES_BOLD, 20)));
            p0.Add("\n\n");

            p0.Add(new Chunk("Performed by", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");

            p0.Add(String.Format("Operator - {0}\n", "<OPERATOR>"));
            p0.Add(String.Format("Organization - {0}\n", "<OrganizationName>"));
            p0.Add(String.Format("Address - {0}\n", "<OrganizationAddress>"));

            //Так добавляются пустые строчки
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add(new Chunk("Device Under Test", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");
            p0.Add(String.Format("Brand - {0}\n", "<Device_Brand>"));
            p0.Add(String.Format("Model - {0}\n", "<Device_Model>"));
            p0.Add(String.Format("Serial Number - {0}\n", "<Device_SerialNumber>"));
            p0.Add(String.Format("Firmware Version - {0}\n", "<Device_FWversion>"));
            p0.Add(String.Format("Other - {0}\n", "<Device_Other>"));

            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            // add the ONVIF information

            p0.Add(String.Format("{0}", "<ToolVersion>"));
            p0.Add(Environment.NewLine);
            p0.Add(String.Format("{0}", "<TestVersion>"));
            p0.Add(Environment.NewLine);
            p0.Add(String.Format("{0}", "<CoreVersion>"));
            p0.Alignment = Element.ALIGN_CENTER;


            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add("Test Date and Time - " + "<TestDateAndTime>");


            m_Document.Add(p0);

        }

        private void BuildSummary()
        {
            m_Document.NewPage();
            int testCount, optional_Skipped, manditory_Skipped;
            int testsRan, testsPassed, testsFailed;

            int optionalFailed, optionalPassed;

            testCount = 12;
            optional_Skipped = 1;
            manditory_Skipped = 3;

            testsRan = 4;
            testsPassed = 4;
            testsFailed = 5;

            optionalFailed = 1;
            optionalPassed = 1;

            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("ONVIF Test Summary\n");
            p0.Alignment = Element.ALIGN_CENTER;
            m_Document.Add(p0);

            Paragraph p1 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Add("THIS IS NOT A VALID ONVIF CONFORMANCE TEST\n\n");
            p1.Alignment = Element.ALIGN_CENTER;
            m_Document.Add(p1);

            Paragraph p2 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p2.Add(String.Format("Test Count: {0}\n", testCount));
            p2.Add(String.Format("Manditory Tests Skipped: {0}\n", manditory_Skipped));
            p2.Add(String.Format("Optional Tests Skipped: {0}\n", optional_Skipped));
            p2.Add(String.Format("Tests Executed: {0}\n", testsRan));
            p2.Add(String.Format("Tests Passed:  {0}\n", testsPassed + optionalPassed));
            p2.Add(String.Format("Tests Failed:  {0}\n", testsFailed));


            if (optionalFailed > 0)
            {
                p2.Add(String.Format("Optional Tests Failed: {1}\n", "", optionalFailed));
            }

            p2.Add("\n");

            m_Document.Add(p2);

            // if not all tests were run this is not a valid conformance test
            if (testCount != (testsRan + optional_Skipped))
            {
                Paragraph p3 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                p3.Add("NOT ALL TESTS RUN, NOT A VALID ONVIF CONFORMANCE TEST\n\n");
                p3.Alignment = Element.ALIGN_CENTER;
                m_Document.Add(p3);
            }

            Paragraph p4 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
             p4.Add("THIS IS NOT A VALID ONVIF CONFORMANCE TEST\n\n");
                p4.Alignment = Element.ALIGN_CENTER;
                m_Document.Add(p4);
            



        }

        private void BuildIndex()
        {
            //Добавить новую страницу
            m_Document.NewPage();

            //Добавить заголовок по середине
            Paragraph p1 = new Paragraph(new Chunk("Tests\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Alignment = Element.ALIGN_CENTER;
            m_Document.Add(p1);

            //Добавить ссылку на тест (если не добавить потом точку куда оно должно переходить)
            string line = String.Format("{0, -13} {1}\n", "8.1.1", "MULTICAST NVT HELLO MESSAGE");
            Anchor anchor = new Anchor(line, FontFactory.GetFont(FontFactory.TIMES, 12));
            anchor.Reference = "#MULTICAST NVT HELLO MESSAGE";
            m_Document.Add(anchor);

            //Добавить ссылку на тест (если не добавить потом точку куда оно должно переходить)
            line = String.Format("{0, -13} {1}\n", "8.1.2", "MULTICAST NVT HELLO MESSAGE VALIDATION");
            anchor = new Anchor(line, FontFactory.GetFont(FontFactory.TIMES, 12));
            anchor.Reference = "#MULTICAST NVT HELLO MESSAGE VALIDATION";
            m_Document.Add(anchor);

        }

        private void AddTestGroupInfo()
        {
            m_Document.NewPage();

            //Добавление основного заголовка ONVIF TEST
            Paragraph title = new Paragraph("ONVIF TEST", FontFactory.GetFont(FontFactory.TIMES, 18));
            Chapter chapter2 = new Chapter(title, 2);
            //Для отсутсвия нумерации
            chapter2.NumberDepth = 0;
            Paragraph someText = new Paragraph("\n\n");
            chapter2.Add(someText);

            //Добавление группы тестов
            Paragraph title1 = new Paragraph("Device Discovery Test Cases", FontFactory.GetFont(FontFactory.TIMES, 18));
            Section section1 = chapter2.AddSection(title1);
            section1.NumberDepth = 0;
            Paragraph someSectionText = new Paragraph("\n\n");
            section1.Add(someSectionText);

            //Добавление теста
            Paragraph title11 = new Paragraph("8.1.1 - MULTICAST NVT HELLO MESSAGE");
            //Добавление конечной точки для ссылки из содержания
            Anchor anc11 = new Anchor(".");
            anc11.Name = "MULTICAST NVT HELLO MESSAGE";
            title11.Add(anc11);
            Section section11 = section1.AddSection(title11);
            section11.NumberDepth = 0;
            Paragraph someSectionText11 = new Paragraph("Test Results\nTest Marked as Skipped\nSkipping all test steps\nTest complete\nTest SKIPPED\n\n");
            section11.Add(someSectionText11);

            //Добавление теста
            Paragraph title12 = new Paragraph("8.1.2 - MULTICAST NVT HELLO MESSAGE VALIDATION");
            Anchor anc12 = new Anchor(".");
            anc12.Name = "MULTICAST NVT HELLO MESSAGE VALIDATION";
            title12.Add(anc12);
            Section section12 = section1.AddSection(title12);
            section12.NumberDepth = 0;
            Paragraph someSectionText12 = new Paragraph("Test Results\nTest Marked as Skipped\nSkipping all test steps\nTest complete\nTest SKIPPED\n\n");
            section12.Add(someSectionText12);


            m_Document.Add(chapter2);

        }
    }
}
