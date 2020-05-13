///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.ComponentModel;
using ClientTestTool.GUI.Logging;

namespace ClientTestTool.Data.Conformance.DoC.Base.Pdf
{
  public static class PdfViewer
  {
    public static void View(String pdfPath)
    {
      try
      {
        System.Diagnostics.Process.Start(pdfPath);
      }
      catch (Win32Exception e)
      {
        Logger.LogException("Exception in PdfViewer:", e);
      }
    }
  }
}
