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

        // put the final number of pages in a template
        private PdfTemplate template;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                var contentByte = writer.DirectContent;
                template = contentByte.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }

        }

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

            // page numbers begins
            var footerPageNumber = new PdfPTable(2);
            footerPageNumber.TotalWidth = pageSize.Width - document.LeftMargin - document.RightMargin;
            int pageNumber = writer.PageNumber;

            string pageNumberText = "Page " + pageNumber + " of ";
            PdfPCell pageNumberCell = new PdfPCell(new Phrase(pageNumberText, FontFactory.GetFont("Arial", 8)));
            pageNumberCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pageNumberCell.Border = Rectangle.NO_BORDER;
            footerPageNumber.AddCell(pageNumberCell);

            PdfPCell cell = new PdfPCell(Image.GetInstance(template));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            footerPageNumber.AddCell(cell);

            footerPageNumber.WriteSelectedRows(0, -1, pageSize.GetLeft(document.LeftMargin) + 15, pageSize.GetBottom(40), contentByte);
            // page numbers ends

            var foother = new PdfPTable(3);
            foother.TotalWidth = pageSize.Width - document.LeftMargin - document.RightMargin;

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
            foother.WriteSelectedRows(0, -1, pageSize.GetLeft(document.LeftMargin), pageSize.GetBottom(25), contentByte);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            
            Rectangle pageSize = document.PageSize;
            ColumnText.ShowTextAligned(template, Element.ALIGN_LEFT,
                new Phrase((writer.PageNumber - 1).ToString(), FontFactory.GetFont("Arial", 8)), 1, 40, 0);

        }

    }
}
