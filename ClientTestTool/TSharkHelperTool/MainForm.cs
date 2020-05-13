using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSharkHelperTool.Decoders;
using TSharkHelperTool.TShark;
using TSharkHelperTool.TShark.Query;

namespace TSharkHelperTool
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    #region Event Handlers

    private void Form1_Load(object sender, EventArgs e)
    {
      tBArgs.Text = Properties.Settings.Default.LastArgs;
      tBPath.Text = Properties.Settings.Default.PathTShark;

      progressBar.MarqueeAnimationSpeed = 0;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      SaveSettings();
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      var result = openFileDialog1.ShowDialog();

      if (DialogResult.OK != result)
        return;

      Properties.Settings.Default.PathTShark = openFileDialog1.FileName;
      tBPath.Text                            = Properties.Settings.Default.PathTShark;
      SaveSettings();
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
      progressBar.Style = ProgressBarStyle.Marquee;
      progressBar.MarqueeAnimationSpeed = 50;
      progressBar.Value = 0;

      await RunTShark();

      progressBar.Style = ProgressBarStyle.Blocks;
      progressBar.MarqueeAnimationSpeed = 0;
      progressBar.Value = 0;
    }

    private void tBOutput_MouseDown(object sender, MouseEventArgs e)
    {
      if (MouseButtons.Right != e.Button)
        return;

      if (0 == tBOutput.SelectedText.Length)
        return;

      var menu = new ContextMenuStrip();
      menu.Items.Add("Decode Hex");
      menu.Items[0].Click += (o, args) => ShowDecodedHexString();
      menu.Show(sender as Control, e.Location);
    }

    private void tBArgs_KeyDown(object sender, KeyEventArgs e)
    {
      if (Keys.Return == e.KeyData)
        RunTShark();
    }

    #endregion

    #region Helpers

    private async Task RunTShark()
    {
      String args = tBArgs.Text.Trim();
      Properties.Settings.Default.LastArgs = args;
      SaveSettings();

      String output = String.Empty;
      if (! await Task.Run(() => TSharkHelper.RunTShark(args, out output)))
        return;

      tBOutput.Clear();
      tBOutput.Text = output;
    }

    private void LoadSettings()
    {
  
    }

    private void SaveSettings()
    {
      Properties.Settings.Default.Save();
    }

    private void ShowDecodedHexString()
    {
      try
      {
        String text = HexToASCIIConverter.Convert(tBOutput.SelectedText.Trim());
        var decoderForm = new DecoderForm();
        decoderForm.ShowText(text);
      }
      catch (Exception e)
      {
        MessageBox.Show(String.Format("Exception: {0}", e.Message));
      }
    }

    #endregion

    private void btnFeatureList_Click(object sender, EventArgs e)
    {
      using (var process = new TSharkProcess(new FrameListQuery(@"d:\!Work\Network Traces\ONVIF_traces_from_Bosch\ONVIF-Test\Hikvision.pcapng",
          new[]
          {
            "http", "rtsp"
          })))
      {

        tBOutput.Clear();

        tBOutput.AppendText(process.StandartOutput.ReadToEnd());
      }
    }

    private void btnFrames_Click(object sender, EventArgs e)
    {
      using (var process = new TSharkProcess(new FramesQuery(@"d:\!Work\Network Traces\ONVIF_traces_from_Bosch\ONVIF-Test\Hikvision.pcapng")))
      {
        tBOutput.Clear();

        foreach (var frame in GetFrames(process.StandartOutput))
        {
          tBOutput.AppendText(frame);
          tBOutput.AppendText(Environment.NewLine);
        }
      }
    }

    public static System.Collections.Generic.IEnumerable<String> GetFrames(StreamReader reader)
    {
      String result = "";
      String line = "";
      var frameBuilder = new StringBuilder();

      while ((line = reader.ReadLine()) != null)
      {
        if (String.Empty == line && 0 != frameBuilder.Length)
        {
          result = frameBuilder.ToString();
          frameBuilder.Clear();
          line = "";
          yield return result;
        }

        frameBuilder.AppendLine(line.Trim().Replace("\\r\\n", String.Empty));
      }
    }
  }
}
