///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Conformance;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Utils;
using ClientTestTool.GUI.Utils.Validation;

namespace ClientTestTool.GUI.Controls.Conformance
{
  public partial class ConformanceInfoView : BaseView
  {
    public ConformanceInfoView()
    {
      InitializeComponent();

      mModel = ConformanceInfo.Create();

      mRequiredFields = new[]
      {
        (Control) tBMemberName,
        tBMemberAddress,
        tBProductName,
        tBVersion,
        cLBProductType,
        tBOther,
        tBSupportWebsite,
        tBInternationalSupportAddress
      };

      SetTablePadding(tLPInformation);
      SetBindings();
    }

    #region Bindings

    private void SetBindings()
    {
      tBProductName                .DataBindings.Add("Text", mModel, "ProductName"                , false, DataSourceUpdateMode.OnPropertyChanged);
      tBBrand                      .DataBindings.Add("Text", mModel, "Brand"                      , false, DataSourceUpdateMode.OnPropertyChanged);
      tBModel                      .DataBindings.Add("Text", mModel, "Model"                      , false, DataSourceUpdateMode.OnPropertyChanged);
      tBVersion                    .DataBindings.Add("Text", mModel, "Version"                    , false, DataSourceUpdateMode.OnPropertyChanged);
      tBOtherInformation           .DataBindings.Add("Text", mModel, "OtherInformation"           , false, DataSourceUpdateMode.OnPropertyChanged);
      tBMemberName                 .DataBindings.Add("Text", mModel, "MemberName"                 , false, DataSourceUpdateMode.OnPropertyChanged);
      tBMemberAddress              .DataBindings.Add("Text", mModel, "MemberAddress"              , false, DataSourceUpdateMode.OnPropertyChanged);
      tBTestOperatorName           .DataBindings.Add("Text", mModel, "TestOperatorName"           , false, DataSourceUpdateMode.OnPropertyChanged);
      tBOrganizationName           .DataBindings.Add("Text", mModel, "OrganizationName"           , false, DataSourceUpdateMode.OnPropertyChanged);
      tBOrganizationAddress        .DataBindings.Add("Text", mModel, "OrganizationAddress"        , false, DataSourceUpdateMode.OnPropertyChanged);
      tBSupportWebsite             .DataBindings.Add("Text", mModel, "TechSupportWebsite"         , false, DataSourceUpdateMode.OnPropertyChanged);
      tBSupportEmail               .DataBindings.Add("Text", mModel, "TechSupportEmail"           , false, DataSourceUpdateMode.OnPropertyChanged);
      tBSupportPhone               .DataBindings.Add("Text", mModel, "TechSupportPhone"           , false, DataSourceUpdateMode.OnPropertyChanged);
      tBInternationalSupportAddress.DataBindings.Add("Text", mModel, "InternationalSupportAddress", false, DataSourceUpdateMode.OnPropertyChanged);
      tBRegionalSupportAddress     .DataBindings.Add("Text", mModel, "RegionalSupportAddress"     , false, DataSourceUpdateMode.OnPropertyChanged);
    }

    public new void ResetBindings()
    {
      tBProductName                  .DataBindings["Text"].ReadValue();
      tBBrand                        .DataBindings["Text"].ReadValue();
      tBModel                        .DataBindings["Text"].ReadValue();
      tBVersion                      .DataBindings["Text"].ReadValue();
      tBOtherInformation             .DataBindings["Text"].ReadValue();
      tBMemberName                   .DataBindings["Text"].ReadValue();
      tBMemberAddress                .DataBindings["Text"].ReadValue();
      tBTestOperatorName             .DataBindings["Text"].ReadValue();
      tBOrganizationName             .DataBindings["Text"].ReadValue();
      tBOrganizationAddress          .DataBindings["Text"].ReadValue();
      tBSupportWebsite               .DataBindings["Text"].ReadValue();
      tBSupportEmail                 .DataBindings["Text"].ReadValue();
      tBSupportPhone                 .DataBindings["Text"].ReadValue();
      tBInternationalSupportAddress  .DataBindings["Text"].ReadValue();
      tBRegionalSupportAddress       .DataBindings["Text"].ReadValue();
    }

    #endregion

    #region Properties

    public Client ClientUnderTest
    {
      get
      {
        return Info.ClientUnderTest;
      }

      set
      {
        Info.ClientUnderTest = value;

        this.InvokeIfRequired(HookUI);
      }
    }

    public ConformanceInfo Info
    {
      get
      {
        return mModel;
      }
    }

    public bool IsInputValid
    {
      get
      {
        return HasValidationErrors();
      }
    }

    #endregion

    #region Event Handlers

    private void Clear_OnClick(object sender, EventArgs e)
    {
      var btnSender = sender as Button;

      if (null == btnSender)
        return;

      var groupBox = btnSender.Parent as GroupBox;

      if (null == groupBox)
        return;

      foreach (var listBox in groupBox.Controls.OfType<CheckedListBox>())
      {
        listBox.ClearSelected();
        listBox.UncheckAllItems();
        SetFieldError(listBox, String.Empty);
      }

      foreach (var textBox in groupBox.Controls.OfType<TextBox>())
      {
        textBox.Text = String.Empty;

        if (tBOther == textBox)
          tBOther.Enabled = false; // HACK

        SetFieldError(textBox, String.Empty);
      }

      foreach (var textBox in groupBox.Controls.OfType<TableLayoutPanel>()
        .SelectMany(tablePanel => tablePanel.Controls.OfType<TextBoxBase>()))
      {
        textBox.Text = String.Empty;
        SetFieldError(textBox, String.Empty);
      }
    }

    private void requiredTextBox_Validating(object sender, CancelEventArgs e)
    {
      var tBSender = sender as TextBox;

      if (null == tBSender)
        throw new ArgumentException();

      if (!String.IsNullOrEmpty(tBSender.Text))
        return;

      e.Cancel = true;
      SetFieldError(tBSender, ERROR_EMPTY_FIELD);
    }

    private void tBSupportWebsite_Validating(object sender, CancelEventArgs e)
    {
      var tBSender = sender as TextBox;

      if (null == tBSender)
        return;

      if (!StringValidator.IsValidUri(tBSender.Text))
      {
        e.Cancel = true;
        tBSender.Select(0, tBSender.Text.Length);

        SetFieldError(tBSender, ERROR_INVALID_URL);
      }
    }

    private void tBSupportEmail_Validating(object sender, CancelEventArgs e)
    {
      var tBSender = sender as TextBox;

      if (null == tBSender)
        return;

      if (String.IsNullOrEmpty(tBSender.Text))
        return;

      if (StringValidator.IsValidEmailAddress(tBSender.Text))
        return;

      e.Cancel = true;
      tBSender.Select(0, tBSender.Text.Length);
      SetFieldError(tBSender, ERROR_INVALID_EMAIL);
    }

    private void tBSupportPhone_Validating(object sender, CancelEventArgs e)
    {
      var tBSender = sender as TextBox;

      if (null == tBSender)
        return;

      for (char c = 'a'; c < 'z'; c++)
      {
        char cUpper = c.ToString(CultureInfo.InvariantCulture).ToUpper().First();

        if (tBSender.Text.ToUpper().Contains(cUpper))
        {
          e.Cancel = true;
          tBSender.Select(0, tBSender.Text.Length);
          SetFieldError(tBSender, ERROR_INVALID_PHONE);
          return;
        }
      }

      SetFieldError(tBSender, String.Empty);
    }

    private void textBox_Validated(object sender, EventArgs e)
    {
      var senderControl = sender as Control;

      if (null == senderControl)
        return;

      SetFieldError(senderControl, String.Empty);
    }

    private void tBOther_Validated(object sender, EventArgs e)
    {
      textBox_Validated(sender, e);

      RefreshProductType();
    }

    private void cLBProductType_Validating(object sender, CancelEventArgs e)
    {
      var cLBSender = sender as CheckedListBox;

      if (null == cLBSender)
        return;

      IsListBoxEmpty(cLBSender, e);
    }

    private void cLBProductType_SelectedIndexChanged(object sender, EventArgs e)
    {
      var cLBSender = sender as CheckedListBox;

      if (null == cLBSender)
        return;

      bool isOtherSelected = cLBSender.CheckedIndices.Contains(cLBSender.Items.Count - 1);
      tBOther.Enabled = isOtherSelected;

      if (isOtherSelected)
        SetFieldError(tBOther, IsTextBoxEmpty(tBOther) ? ERROR_EMPTY_FIELD : String.Empty);
      else
        SetFieldError(tBOther, String.Empty);
      
      RefreshProductType();
    }

    private void cLBProductType_Validated(object sender, EventArgs e)
    {
      var cLBSender = sender as CheckedListBox;

      if (null == cLBSender)
        return;

     errorProvider.SetError(cLBSender, String.Empty);
    }

    #endregion

    #region Helpers

    public override void Clear()
    {
      this.InvokeIfRequired(() =>
      {
        this.AllControlsOfType<TextBoxBase>().ToList().ForEach(item => item.Clear());

        cLBProductType.ClearSelected();
        cLBProductType.UncheckAllItems();

        var fields = mRequiredFields.Union(this.AllControlsOfType<TextBoxBase>()).ToList();

        foreach (var field in fields)
          errorProvider.SetError(field, String.Empty);
      });
    }

    protected override void HookUI()
    {
      bool clientIsNull = null == ClientUnderTest;

      gBProductInfo.Enabled = !clientIsNull;

      if (clientIsNull)
      {
        cLBProductType.ClearSelected();
        cLBProductType.UncheckAllItems();
        tBOther.Enabled = false;
        tBOther.Clear();
      }
    }

    private bool IsTextBoxEmpty(TextBoxBase control, CancelEventArgs e = null)
    {
      String text = control.Text;
      if (!control.Enabled || (!String.IsNullOrEmpty(text) && !String.IsNullOrWhiteSpace(text)))
        return false;

      SetFieldError(control, ERROR_EMPTY_FIELD);

      if (null != e)
        e.Cancel = true;

      return true;
    }

    private bool IsListBoxEmpty(CheckedListBox control, CancelEventArgs e = null)
    {
      if (0 != control.CheckedItems.Count)
        return false;

      SetFieldError(control, ERROR_EMPTY_FIELD);

      if (null != e)
        e.Cancel = true;

      return true;
    }

    private bool HasValidationErrors()
    {
      return !Controls.HasValidationErrors();
    }

    private void RefreshProductType()
    {
      var checkedItems = cLBProductType.CheckedItems.Cast<object>().Select(item => item.ToString()).ToList();

      int indexOfOther = checkedItems.IndexOf("Other");
      if (-1 != indexOfOther)
        checkedItems[indexOfOther] = tBOther.Text;

      Info.ProductType = String.Join(", ", checkedItems);
    }

    private void SetFieldError(Control control, String message)
    {
      errorProvider.SetError(control, message);
      errorProvider.SetIconPadding(control, -errorProvider.Icon.Size.Width - 2);
    }

    private void SetTablePadding(TableLayoutPanel tPLSender)
    {
      var currentPadding = tPLSender.Padding;
      tPLSender.Padding = new Padding(currentPadding.Left, currentPadding.Top,
        SystemInformation.VerticalScrollBarWidth,
        currentPadding.Bottom);
    }

    #endregion

    private readonly ConformanceInfo mModel;
    private readonly Control[]       mRequiredFields;

    private const String ERROR_EMPTY_FIELD   = "This field is required!";
    private const String ERROR_INVALID_EMAIL = "e-mail address must be valid e-mail address format:\n" + "For example 'someone@example.com' ";
    private const String ERROR_INVALID_URL   = "URL address must be in valid format";
    private const String ERROR_INVALID_PHONE = "Phone number must include only numbers or special characters:\n" + "For example '+1 (418) 000-00-00' ";
  }
}
