///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClientTestTool.GUI.Extensions
{
  internal static class RichTextBoxExtension
  {
    #region Text

    /// <summary>
    /// Appends the line to textBox
    /// </summary>
    public static void AppendLine(this RichTextBox textBox, String text = "")
    {
      textBox.AppendText(String.Format("{0}{1}", text, Environment.NewLine));
    }

    /// <summary>
    /// Appends the formatted line to textBox
    /// </summary>
    public static void AppendLine(this RichTextBox textBox, String format, params object[] args)
    {
      textBox.AppendLine(String.Format(format, args));
    }

    /// <summary>
    /// Appends the line to textBox with specified color and font
    /// </summary>
    public static void AppendLine(this RichTextBox textBox, String text, Color color, FontStyle fontStyle)
    {
      textBox.Select(textBox.TextLength, 0);

      textBox.SelectionColor = color;
      textBox.SelectionFont  = new Font(textBox.SelectionFont, FontStyle.Bold | FontStyle.Underline);

      AppendLine(textBox, text);
    }

    #endregion

    #region Scrolling

    public static void ScrollToEnd(this RichTextBox textBox)
    {
      textBox.SelectionStart = textBox.TextLength;
      textBox.ScrollToCaret();
    }

    public static void ScrollToChar(this RichTextBox textBox, int index)
    {
      if (index < 0)
        return;

      textBox.SelectionStart = index;
      textBox.ScrollToCaret();
    }

    #endregion

    #region Highlighting

    /// <summary>
    /// Highlights all occurences of text in the textBox
    /// </summary>
    public static void HighlightAllText(this RichTextBox textBox, String text)
    {
      if (null == text)
        return;

      String textBoxText = textBox.Text;

      int length = text.Length;
      int firstIndex = textBoxText.IndexOf(text, StringComparison.Ordinal);

      if (-1 == firstIndex)
        return;

      textBox.ScrollToChar(firstIndex);
      HighlightText(textBox, firstIndex, length, Properties.Settings.Default.DefaultTextColor, SystemColors.Highlight);

      for (int i = textBoxText.IndexOf(text, firstIndex + 1); i > -1; i = textBoxText.IndexOf(text, i + 1))
        HighlightText(textBox, i, length, Properties.Settings.Default.DefaultTextColor, SystemColors.Highlight);
    }

    /// <summary>
    /// Highlights specified range in the textBox
    /// </summary>
    public static void HighlightText(this RichTextBox textBox, int start, int length)
    {
      HighlightText(textBox, start, length, Properties.Settings.Default.SelectedTextColor);
    }

    /// <summary>
    /// Highlights specified range in the textBox with forecolor specified
    /// </summary>
    public static void HighlightText(this RichTextBox textBox, int start, int length, Color fColor)
    {
      HighlightText(textBox, start, length, fColor, Properties.Settings.Default.SelectedTextBackColor);
    }

    /// <summary>
    /// Highlights specified range in the textBox with forecolor and backcolor specified
    /// </summary>
    public static void HighlightText(this RichTextBox textBox, int start, int length, Color fColor, Color bColor)
    {
      if (start < 0)
        return;

      if (0 == length)
        return;

      textBox.Select(start, length);
      textBox.SelectionColor     = fColor;
      textBox.SelectionBackColor = bColor;
    }

    /// <summary>
    /// Clears the selection.
    /// </summary>
    public static void ClearSelection(this RichTextBox textBox)
    {
      String text = textBox.Text;

      textBox.Clear();

      textBox.Text = text;
    }

    #endregion

  }
}
