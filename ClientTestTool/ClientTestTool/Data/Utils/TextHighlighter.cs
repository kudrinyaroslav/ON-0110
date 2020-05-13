///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ClientTestTool.GUI.Extensions;

namespace ClientTestTool.Data.Utils
{
  public class TextHighlighter
  {
    public TextHighlighter(RichTextBox textBox)
    {
      if (null == textBox)
        throw new ArgumentNullException("textBox");

      mTextBox = textBox;
    }

    private String mSearchPattern;

    public String SearchPattern
    {
      get
      {
        return mSearchPattern;
      }
      set
      {
        mSearchPattern = String.IsNullOrEmpty(value) ? String.Empty : value.ToLower();
      }
    }

    public bool HighlightNext()
    {
      String text = mTextBox.Text.ToLower();

      if (String.IsNullOrEmpty(text))
        return false;

      int len = SearchPattern.Length;

      mLastIndex = -1 == mLastIndex ? text.IndexOf(SearchPattern, StringComparison.Ordinal) : text.IndexOf(SearchPattern, mLastIndex + 1, StringComparison.Ordinal);

      if (-1 == mLastIndex)
        return false;

      HighlightText(mLastIndex, len);
      return true;
    }

    public bool HighlightPrev()
    {
      String text = mTextBox.Text.ToLower();

      if (String.IsNullOrEmpty(text))
        return false;

      int len = SearchPattern.Length;

      mLastIndex = -1 == mLastIndex ? text.LastIndexOf(SearchPattern, StringComparison.Ordinal) :
                                      text.Substring(0, mLastIndex).LastIndexOf(SearchPattern, StringComparison.Ordinal);

      if (-1 == mLastIndex)
        return false;

      HighlightText(mLastIndex, len);
      return true;
    }

    private void HighlightText(int start, int len)
    {
      mTextBox.ClearSelection();
      mTextBox.ScrollToChar(mLastIndex);
      mTextBox.HighlightText(mLastIndex, len, SystemColors.HighlightText, SystemColors.Highlight);
    }

    private int mLastIndex = -1;
    private readonly RichTextBox mTextBox;
  }
}
