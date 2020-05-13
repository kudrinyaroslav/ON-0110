using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PDFforONVIFxample
{
    public partial class Form1 : Form
    {
        private PDFCreater m_PDFCreater = null;

        public Form1()
        {
            InitializeComponent();

            m_PDFCreater = new PDFCreater();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_PDFCreater.m_logoPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "logo.gif");
            m_PDFCreater.Save(textBoxFileName.Text);
        }


    }
}
