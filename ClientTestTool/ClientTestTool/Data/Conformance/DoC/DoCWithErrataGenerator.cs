///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClientTestTool.Data.Conformance.DoC.Base;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Utils;

namespace ClientTestTool.Data.Conformance.DoC
{
  public class DoCWithErrataGenerator : BaseDoCGenerator
  {
    public DoCWithErrataGenerator(List<Tuple<String, String, String>> erratumTable, ConformanceInfo info, String templatePath) : base(info, templatePath)
    {
      mErratumTable = erratumTable;
    }

    public override void Generate(String pdfFilename)
    {
      var doc = XElement.Load(mTemplatePath);

      ReplaceAttributeValue(doc, ID_ONVIF_LOGO, "src", Path.Combine(CTTSettings.GetStylesheetsDir(), "onvif_doc_template_logo.jpg")); // TODO resource

      ReplaceIds(doc, Info);

      AddDevicesList(doc);
      FillDevicesList(doc);

      AddErratumList(doc, mErratumTable.Count);
      FillErratumList(doc, mErratumTable);

      RenderPdf(doc, Path.Combine(CTTSettings.GetOutputDir(), pdfFilename));
    }

    #region Helpers

    private void AddErratumList(XElement doc, int count)
    {
      String headerId = GetIdWithPostfix(ID_ERRATA_TABLE, 1);

      var tableRowTemplate = XmlUtil.GetElementById(doc, headerId);

      XElement lastElement = tableRowTemplate;

      for (int i = 1; i < count; ++i)
      {
        var newTableRow = new XElement(tableRowTemplate);

        ProcessErrataTableRow(newTableRow, i + 1);

        lastElement.AddAfterSelf(newTableRow);
        lastElement = newTableRow;
      }
    }

    private void FillErratumList(XElement doc, List<Tuple<String, String, String>> values)
    {
      for (int i = 0; i < values.Count; ++i)
      {
        int rowNumber = i + 1;
        ReplaceElementValue(doc, GetIdWithPostfix(ID_ERRATA_TEST, rowNumber), values[i].Item1);
        ReplaceElementValue(doc, GetIdWithPostfix(ID_ERRATA_NUMBER, rowNumber), values[i].Item2);
        ReplaceElementValue(doc, GetIdWithPostfix(ID_ERRATA_DESCRIPTION, rowNumber), values[i].Item3);
      }
    }

    private void ProcessErrataTableRow(XElement tableRowElement, int erratumNumber)
    {
      String[] ids = { ID_ERRATA_TABLE, ID_ERRATA_TEST, ID_ERRATA_NUMBER, ID_ERRATA_DESCRIPTION };

      foreach (String id in ids)
        XmlUtil.ReplaceId(tableRowElement, GetIdWithPostfix(id, 1), GetIdWithPostfix(id, erratumNumber));
    }

    #endregion

    #region IDs

    private const String ID_ERRATA_TABLE                  = "row_errataNumber";      
    private const String ID_ERRATA_TEST                   = "failedTest";      
    private const String ID_ERRATA_NUMBER                 = "errataNumber";      
    private const String ID_ERRATA_DESCRIPTION            = "errataDescription";

    #endregion

    private readonly List<Tuple<String, String, String>> mErratumTable;
  }
}
