using System;
using System.Windows.Forms;

namespace TSharkHelperTool
{
  public partial class DecoderForm : Form
  {
    public DecoderForm()
    {
      InitializeComponent();
    }

    public void ShowText(String text)
    {
      tBOutput.Text = text;
      Show();
    }
  }
}
