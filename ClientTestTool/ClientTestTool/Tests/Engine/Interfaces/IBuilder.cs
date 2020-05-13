///
/// @Author Matthew Tuusberg
///

﻿
namespace ClientTestTool.Tests.Engine.Interfaces
{
  interface IBuilder<out TResult>
  {
    TResult Build();
  }

  interface IBuilder<out TResult, in TArg1>
  {
    TResult Build(TArg1 arg1);
  }

  interface IBuilder<out TResult, in TArg1, in TArg2>
  {
    TResult Build(TArg1 arg1, TArg2 arg2);
  }
}
