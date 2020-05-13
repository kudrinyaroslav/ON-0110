///
/// @Author Matthew Tuusberg
///

﻿
namespace ClientTestTool.Data.Definitions.Interfaces
{
  interface ISystem<in T>
              where T : class
  {
    void Add   (T item);
    void Remove(T item);
  }
}
