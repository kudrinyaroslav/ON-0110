using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Base class for PDF documents generation
    /// </summary>
    class PdfDocGenerator : IReportGenerator
    {
        private Document _document;

        protected Document Document
        {
            get { return _document; }
        }
        
        private Data.TestLogFull _log;
        protected Data.TestLogFull Log
        {
            get { return _log; }
        }

        protected PdfWriter Writer { get; private set; }

        /// <summary>
        /// Is raised when an exception occurs.
        /// </summary>
        public event Action<Exception> OnException;

        /// <summary>
        /// Is raised when report is saved.
        /// </summary>
        public event Action OnReportSaved;

        /// <summary>
        /// Creates report.
        /// </summary>
        /// <param name="fileName">Path to save report.</param>
        /// <param name="log">Test execution information.</param>
        public void CreateReport(string fileName, Data.TestLogFull log)
        {
            _log = log;
            bool ok = false;
            try
            {
                _document = new Document(PageSize.A4);
                Writer = PdfWriter.GetInstance(_document, new FileStream(fileName, FileMode.Create));

                CustomDocumentInitialization();

                _document.Open();

                GenerateDocumentContent();

                ok = true;
            }
            catch (Exception ex)
            {
                RaiseOnException(ex);
            }
            finally
            {
                if (_document.IsOpen())
                {
                    _document.Close();
                }
            }
            if (ok)
            {
                RaiseDocumentSaved();
            }
        }

        protected void RaiseOnException(Exception exc)
        {
            if (OnException != null)
            {
                OnException(exc);
            }
        }

        protected void RaiseDocumentSaved()
        {
            if (OnReportSaved != null)
            {
                OnReportSaved();
            }
        }

        protected virtual void CustomDocumentInitialization()
        {
        }

        protected virtual void GenerateDocumentContent()
        {

        }

    }
}
