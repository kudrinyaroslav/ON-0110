using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.GUI
{
    public partial class CompactProcessingForm : Form
    {
        public CompactProcessingForm()
        {
            InitializeComponent();
        }

        public void FeatureDefinitionCompleted(int total)
        {
            Invoke(new Action(()=>
                                  {
                                      lblFeatureDefinitionStatus.Text = "Feature definition completed";
                                      lblTotalCount.Text = total.ToString();
                                  }));

        }

        public void ReportProgress(int completed, int failed)
        {
            Invoke(new Action(()=>
                                  {
                                      lblCompletedCount.Text = completed.ToString();
                                      lblFailedCount.Text = failed.ToString();
                                  }));
        }

    }
}
