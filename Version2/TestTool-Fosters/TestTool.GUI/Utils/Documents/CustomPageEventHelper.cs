using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TestTool.GUI.Utils
{
    public class CustomPageEventHelper : PdfPageEventHelper
    {
        #region Properties

        public Image HeaderLogo { get; set; }

        public string HeaderText { get; set; }

        public Font HeaderFont { get; set; }

        public string FootherFirst { get; set; }

        public string FootherSecond { get; set; }

        public string FootherThird { get; set; }

        public Font FooterFont { get; set; }

        #endregion

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            var contentByte = writer.DirectContent;
            Rectangle pageSize = document.PageSize;

            var header = new PdfPTable(2);
            header.TotalWidth = pageSize.Width - document.LeftMargin - document.RightMargin;
            header.SetWidthPercentage(new float[] {20, 80}, pageSize);

            var logoCell = new PdfPCell(HeaderLogo, true);
            logoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            logoCell.Border = Rectangle.NO_BORDER;
            header.AddCell(logoCell);

            var headerText = new PdfPCell(new Phrase(5, HeaderText, HeaderFont));
            headerText.HorizontalAlignment = Element.ALIGN_LEFT;
            headerText.VerticalAlignment = Element.ALIGN_BOTTOM;
            headerText.Border = Rectangle.NO_BORDER;
            header.AddCell(headerText);

            contentByte.SetRGBColorFill(0, 0, 0);
            header.WriteSelectedRows(0, -1, pageSize.GetLeft(document.LeftMargin), pageSize.GetTop(40), contentByte);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            var contentByte = writer.DirectContent;
            Rectangle pageSize = document.PageSize;

            var foother = new PdfPTable(3);
            foother.TotalWidth = pageSize.Width - document.LeftMargin - document.RightMargin;
            //header.SetWidthPercentage(new float[] { 20, 80 }, pageSize);

            var firstCell = new PdfPCell(new Phrase(FootherFirst, FooterFont));
            firstCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firstCell.Border = Rectangle.NO_BORDER;
            foother.AddCell(firstCell);

            var secondCell = new PdfPCell(new Phrase(FootherSecond, FooterFont));
            secondCell.HorizontalAlignment = Element.ALIGN_CENTER;
            secondCell.Border = Rectangle.NO_BORDER;
            foother.AddCell(secondCell);

            var thirdCell = new PdfPCell(new Phrase(FootherThird, FooterFont));
            thirdCell.HorizontalAlignment = Element.ALIGN_CENTER;
            thirdCell.Border = Rectangle.NO_BORDER;
            foother.AddCell(thirdCell);

            contentByte.SetRGBColorFill(0, 0, 0);
            foother.WriteSelectedRows(0, -1, pageSize.GetLeft(document.LeftMargin), pageSize.GetBottom(40), contentByte);
        }

        //public override void OnCloseDocument(PdfWriter writer, Document document)
        //{
        //    base.OnCloseDocument(writer, document);

        //    template.BeginText();
        //    template.SetFontAndSize(bf, 8);
        // //   template.SetTextMatrix(0, 0);
        //    template.ShowText("OnCloseDocument");
        //    template.EndText();
        //}

    }
}
