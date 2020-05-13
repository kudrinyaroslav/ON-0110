///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ClientTestTool.Data.Extensions
{
  public static class BindingListExtension
  {
    public static void Sort<T>(this BindingList<T> bindingList, IComparer<T> comparer)
    {
      List<T> list = bindingList.ToList();
      list.Sort(comparer);

      bindingList.RaiseListChangedEvents = false;
      bindingList.Clear();
      list.ForEach(bindingList.Add);
      bindingList.RaiseListChangedEvents = true;
    }

    public static void AddRange<T>(this BindingList<T> bindingList, IEnumerable<T> values)
    {
      foreach (T value in values)
        bindingList.Add(value);
    }
  }
}
