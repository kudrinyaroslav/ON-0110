///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Utils;

namespace ClientTestTool.Data.Definitions.Conversation.Base
{
  public abstract class ValidatableItem 
  {
    protected ValidatableItem()
    {
      ValidationStatus = ValidationStatus.Pending;
    }

    public abstract void Validate();

    public String GetStatusString()
    {
      return ValidationStatus.GetDescription().ToUpper();
    }

    #region Properties

    public ValidationStatus ValidationStatus
    {
      get;
      protected set;
    }

    public bool Validated
    {
      get;
      protected set;
    }

    public String ValidationError
    {
      get;
      protected set;
    }

    #endregion
  }
}
