using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEditor
{
    public partial class Main : Form
    {

        ClientTestTool.TestSet testSet { get; set; }

        public Main()
        {
            InitializeComponent();
            testSet = new ClientTestTool.TestSet();
        }

        private void bOpenReport_Click(object sender, EventArgs e)
        {
            if (ofdReport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbReport.Text = ofdReport.FileName;
                testSet = testSet.DeSerializeData(tbReport.Text);
                pgMain.SelectedObject = testSet;
                
            }
        }
    }
}
