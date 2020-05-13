///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Devices.Definitions.FeatureList;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Definitions.Interfaces;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Utils;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Enums;
using ClientTestTool.GUI.Forms;
using ClientTestTool.GUI.Logging;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Parsers.NetworkTraceParser;
using ClientTestTool.Properties;

namespace ClientTestTool.GUI.Controls.Configuration
{
  /// <summary>
  /// Configuration page
  /// </summary>
  public partial class PageConfiguration : BaseView
  {
    /// <summary>
    /// ctor
    /// </summary>
    public PageConfiguration()
    {
      InitializeComponent();
    }

    #region EventHandlers

    private void ConfigurationPage_Load(object sender, EventArgs e)
    {
      PrepareGridView();

      NetworkTraceSet.OnTraceAdded   += NetworkTraces_ListChanged;
      NetworkTraceSet.OnTraceRemoved += NetworkTraces_ListChanged;
      UnitSet.OnUnitListChanged      += Units_ListChanged;
    }

    private void NetworkTraces_ListChanged(object sender, EventArgs e)
    {
      UpdateTracesList();
      btnStartTesting.Enabled = !NetworkTraceSet.IsEmpty;
    }

    private void Units_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (ListChangedType.ItemDeleted == e.ListChangedType)
        ClearDeviceInfo();

      UpdateUnitList();
    }

    /// <summary>
    /// Start Testing button handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnStartTesting_Click(object sender, EventArgs e)
    {
      if (NetworkTraceSet.LoadedTraces.All(item => NetworkTraceStatus.Parsed == item.Status))
        return;

      btnStartTesting.Enabled = false;

      var traces = NetworkTraceSet.LoadedTraces.Where(item => NetworkTraceStatus.Parsed != item.Status).ToList();

      await Task.Run(() =>
      {
        foreach (var trace in traces)
        {
          ApplicationStatus.SetProgress(0);
          trace.Status = NetworkTraceStatus.InProgress;
          UpdateTracesList();

          var parser = new NTParser(CTTSettings.GetProtocols(), trace);
          parser.OnProgressChanged += OnProgressChanged;

          trace.Parser = parser;

          parser.Run();

          trace.Status = NetworkTraceStatus.Parsed;
          UpdateTracesList();
          ApplicationStatus.SetProgress(100);
        }
      }, mTokenSource.Token);

      UpdateUnitList();
      ApplicationStatus.SetStatus("Parsing process has been completed.");

      await Task.Run(() =>
                     {
                       ConversationList.Instance.Validate();
                       ApplicationStatus.SetStatus("Done!");
                       StateManager.SetState(ApplicationState.Idle);
                     });
    }

    /// <summary>
    /// Add Network Trace button handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddNetworkTrace_Click(object sender, EventArgs e)
    {
      if (ApplicationState.Idle != StateManager.GetState())
        return;

      String pFilename = tBPathNetworkTrace.Text.Trim();
      if (File.Exists(pFilename) && NetworkTraceSet.LoadedTraces.All(item => item.FullName != pFilename))
      {
        NetworkTraceSet.Add(new NetworkTraceInfo(pFilename));
        tBPathNetworkTrace.Clear();

        return;
      }

      LoadNetworkTrace();
    }

    public bool LoadNetworkTrace()
    {
      if (ApplicationState.Idle != StateManager.GetState())
        return false;

      DialogResult dResult = oFDNetworkTrace.ShowDialog();

      if (DialogResult.OK != dResult)
        return false;

      foreach (String filename in oFDNetworkTrace.FileNames)
      {
        if (!File.Exists(filename) || NetworkTraceSet.LoadedTraces.Any(item => item.FullName == filename))
          continue;

        NetworkTraceSet.Add(new NetworkTraceInfo(filename));
      }

      tBPathNetworkTrace.Clear();

      return true;
    }

    /// <summary>
    /// Browse FeatureList button handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnBrowseFeatureList_Click(object sender, EventArgs e)
    {
      DialogResult result = oFDFeatureList.ShowDialog();

      if (DialogResult.OK != result)
        return;

      IUnit unit = UnitSet.GetUnitAt(mSelectedUnitIndex);

      var device = unit as Device;

      if (null == device)
        return;

      device.FeatureList = oFDFeatureList.FileName;
      var deviceInfo = new DeviceFeatureListParser(device).GetDeviceInformation();

      if (!String.IsNullOrEmpty(device.Info.Model) && deviceInfo.Model != device.Info.Model)
      {
        var warningResult = DialogHelper.ShowWarning(Resources.Message_Device_Info_Different);

        if (warningResult == DialogResult.Cancel)
        {
          device.FeatureList = String.Empty;
          return;
        }
      }

      device.SetInformation(deviceInfo);

      tBFeatureList.Text = device.FeatureList;

      UpdateUnitList();

      lVUnits.Focus();
      lVUnits.Items[mSelectedUnitIndex].Selected = true;
    }

    /// <summary>
    /// Delete button handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (ApplicationState.Idle != StateManager.GetState())
        return;

      if (-1 == e.RowIndex)
        return;

      if ((int) eGridViewColumns.Delete != e.ColumnIndex)
        return;

      NetworkTraceSet.RemoveAt(e.RowIndex);
    }

    /// <summary>
    ///  Preventing gridView item selection
    /// </summary>
    private void dGVNetworkTraces_SelectionChanged(object sender, EventArgs e)
    {
      dGVNetworkTraces.ClearSelection();
    }

    /// <summary>
    /// Called when rows are added to DataGridView control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      var gvSender = sender as DataGridView;

      if (null == gvSender)
        return;

      VScrollBar vScroll = gvSender.Controls.OfType<VScrollBar>().First();

      if (vScroll.Visible && !mIsGridViewWidthChanged)
      {
        gvSender.Columns[(int) eGridViewColumns.Filename].Width -= vScroll.Width;
        mIsGridViewWidthChanged = !mIsGridViewWidthChanged;
      }
    }  

    /// <summary>
    ///   Called when rows are removed from DataGridView control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      var gvSender = sender as DataGridView;

      if (null == gvSender)
        return;

      VScrollBar vScroll = gvSender.Controls.OfType<VScrollBar>().First();

      if (!vScroll.Visible && mIsGridViewWidthChanged)
      {
        gvSender.Columns[(int) eGridViewColumns.Filename].Width += vScroll.Width;
        mIsGridViewWidthChanged = !mIsGridViewWidthChanged;
      }

      RecountTracesRows(gvSender);
    }

    /// <summary>
    ///   Called when selected index of lVUnits changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lVUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (0 == lVUnits.SelectedIndices.Count)
      {
        ClearDeviceInfo();
        btnBrowseFeatureList.Enabled = false;
        mSelectedUnitIndex = -1;
        return;
      }

      mSelectedUnitIndex = lVUnits.SelectedIndices[0];
      var selectedUnit = UnitSet.GetUnitAt(mSelectedUnitIndex) as BaseUnit;

      if (null == selectedUnit)
        return;

      bool isDevice = UnitType.Device == selectedUnit.Type;

      btnBrowseFeatureList.Enabled = isDevice;
      tBFeatureList.Enabled        = isDevice;

      tBNetworkTrace.Text = selectedUnit.GetTracesString();

      FillClientInfo(selectedUnit as Client);
      FillDeviceInfo(selectedUnit as Device);
    }

    /// <summary>
    ///  Called when item in the lVUnits checked
    /// </summary>
    private void lVUnits_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (mIsUnitListUpdating)
        return;

      if (e.Item.Index >= UnitSet.Count)
        return;

      IUnit selectedUnit = UnitSet.GetUnits()[e.Item.Index];
      selectedUnit.IsIgnored = !e.Item.Checked;

      UpdateUnitList();
    }

    private void lVUnits_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (mPreventItemChecking)
        if (e.CurrentValue != e.NewValue)
          e.NewValue = e.CurrentValue;
    }

    private void lVUnits_MouseUp(object sender, MouseEventArgs e)
    {
      if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control)
        mPreventItemChecking = false;
    }

    private void lVUnits_MouseDown(object sender, MouseEventArgs e)
    {
      if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control)
        mPreventItemChecking = true;
    }

    private void lVUnits_MouseClick(object sender, MouseEventArgs e) // TODO
    {
      if (MouseButtons.Right != e.Button)
        return;

      ListView lVSender = sender as ListView;

      if (null == lVSender)
        return;

      var selectedUnits = (from int index in lVSender.SelectedIndices
                           select UnitSet.GetUnitAt(index)).ToList();

      if (selectedUnits.All(item => UnitType.Client == item.Type) && selectedUnits.Count > 1)
      {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Merge");
        menu.Items[0].Click += (o, args) => new SystemCreatorForm(selectedUnits.Cast<Client>()).ShowDialog();
        menu.Show(sender as Control, e.Location);
      }
      else if (selectedUnits.All(item => UnitType.System == item.Type))
      {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Split");
        menu.Items[0].Click += (o, args) => selectedUnits.Cast<ClientSystem>().ToList().ForEach(UnitSet.Split);
        menu.Show(sender as Control, e.Location);
      }
    }

    private void sCMain_SplitterMoved(object sender, SplitterEventArgs e)
    {
      HookUI();
    }

    #endregion

    #region Helpers

    protected override void HookUI()
    {
      UpdateUnitList();
      ResizeListViews();
      ResizeGridViews();
    }

    private void ClearDeviceInfo()
    {
      tBFeatureList.Text  = String.Empty;
      tBNetworkTrace.Text = String.Empty;
      tBManufacturer.Text = String.Empty;
      tBModel.Text        = String.Empty;
      tBSerialNumber.Text = String.Empty;
      tBFirmware.Text     = String.Empty;
    }

    private void FillClientInfo(Client client)
    {
      if (null == client)
        return;

      tBFeatureList.Text  = String.Empty;
      tBSerialNumber.Text = String.Empty;
      tBManufacturer.Text = client.Brand;
      tBModel.Text        = client.Model;
      tBFirmware.Text     = client.FirmwareVersion;
    }

    private void FillDeviceInfo(Device selectedDevice)
    {
      if (null == selectedDevice)
        return;

      tBFeatureList.Text  = selectedDevice.FeatureList;
      tBManufacturer.Text = selectedDevice.Info.Manufacturer;
      tBModel.Text        = selectedDevice.Info.Model;
      tBSerialNumber.Text = selectedDevice.Info.SerialNumber;
      tBFirmware.Text     = selectedDevice.Info.FirmwareVersion;
    }

    private void UpdateUnitList()
    {
      if (!IsHandleCreated)
        return;

      mIsUnitListUpdating = true;
      
      this.InvokeIfRequired((() =>
      {
        lVUnits.Items.Clear();

        foreach (Unit unit in UnitSet.GetUnits())
        {
          var item = new ListViewItem();
          item.SubItems.Add((lVUnits.Items.Count + 1).ToString(CultureInfo.InvariantCulture));

          item.SubItems.Add(unit.Name);
          item.SubItems.Add(unit.Mac);
          item.SubItems.Add(unit.Ip);
          item.SubItems.Add(GetFeatureListString(unit));
          item.SubItems.Add(unit.Type.GetDescription());

          item.Checked     = !unit.IsIgnored;
          item.ToolTipText = TooltipHelper.CreateDeviceTooltip(unit as Device);

          bool isChecked = item.Checked;
          item.ForeColor = isChecked ? Settings.Default.CheckedUnitColor : Settings.Default.UnitColor;

          lVUnits.Items.Add(item);
        }
      }));

      mIsUnitListUpdating = false;
    }

    private String GetFeatureListString(Unit unit)
    {
      var device = unit as Device;

      if (null == device)
        return String.Empty;

      return device.FeatureList;
    }

    private void UpdateTracesList()
    {
      this.InvokeIfRequired(() =>
      {
        dGVNetworkTraces.Rows.Clear();
        foreach (NetworkTraceInfo trace in NetworkTraceSet.LoadedTraces)
        {
          int index = dGVNetworkTraces.Rows.Count + 1;

          dGVNetworkTraces.Rows.Add(index.ToString(CultureInfo.InvariantCulture), trace.Filename, trace.Size,
            trace.Status.GetDescription());
          dGVNetworkTraces.Rows[index - 1].Cells[1].ToolTipText = trace.FullName;
        }
      });
    }

    private void RecountTracesRows(DataGridView gridView)
    {
      int index = 1;
      foreach (DataGridViewRow row in gridView.Rows)
      {
        DataGridViewCell cell = row.Cells[(int) eGridViewColumns.Number];

        if (int.Parse(cell.Value.ToString()) != index)
          cell.Value = index.ToString(CultureInfo.InvariantCulture);

        ++index;
      }
    }

    private void PrepareGridView()
    {
      dGVNetworkTraces.Columns[0].Tag = 5;
      dGVNetworkTraces.Columns[1].Tag = 25;
      dGVNetworkTraces.Columns[2].Tag = 15;
      dGVNetworkTraces.Columns[3].Tag = 15;
      dGVNetworkTraces.Columns[4].Tag = 7;

      ResizeGridViews();
    }

    private enum eGridViewColumns : int
    {
      Number   = 0,
      Filename = 1,
      Size     = 2,
      Status   = 3,
      Delete   = 4,
    }

    private enum eListViewColumns
    {
      Enabled      = 0,
      Number       = 1,
      DeviceName   = 2,
      NetworkTrace = 3,
      FeatureList  = 4,
      Type         = 5
    }

    private readonly CancellationTokenSource mTokenSource = new CancellationTokenSource(); //TODO handle task cancel (CancellationToken)

    private bool mIsGridViewWidthChanged;
    private int  mSelectedUnitIndex   = -1;
    private bool mPreventItemChecking = false;
    private bool mIsUnitListUpdating  = false;
    #endregion
  }
}