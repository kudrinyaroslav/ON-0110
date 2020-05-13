///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Devices;

namespace ClientTestTool.Data.Conformance
{
  public class ConformanceInfo
  {
    public static ConformanceInfo Create()
    {
      return new ConformanceInfo()
      {
        MemberName                  = String.Empty,
        MemberAddress               = String.Empty,
        TestOperatorName            = String.Empty,
        OrganizationName            = String.Empty,
        OrganizationAddress         = String.Empty,
        TechSupportWebsite          = String.Empty,
        TechSupportEmail            = String.Empty,
        TechSupportPhone            = String.Empty,
        InternationalSupportAddress = String.Empty,
        RegionalSupportAddress      = String.Empty
      };
    }

    #region Properties

    public Client ClientUnderTest
    {
      get;
      set;
    }

    #region Product Under Test

    public String ProductName
    {
      get
      {
        if (null == ClientUnderTest)
          return String.Empty;

        return ClientUnderTest.Name;
      }
      set
      {
        if (null != ClientUnderTest)
          ClientUnderTest.Name = value;
      }
    }

    public String Brand
    {
      get
      {
        if (null == ClientUnderTest)
          return String.Empty;

        return ClientUnderTest.Brand;
      }
      set
      {
        if (null != ClientUnderTest)
          ClientUnderTest.Brand = value;
      }
    }

    public String Model
    {
      get
      {
        if (null == ClientUnderTest)
          return String.Empty;

        return ClientUnderTest.Model;
      }
      set
      {
        if (null != ClientUnderTest)
          ClientUnderTest.Model = value;
      }
    }

    public String Version
    {
      get
      {
        if (null == ClientUnderTest)
          return String.Empty;

        return ClientUnderTest.FirmwareVersion;
      }
      set
      {
        if (null != ClientUnderTest)
          ClientUnderTest.FirmwareVersion = value;
      }
    }

    public String OtherInformation
    {
      get
      {
        if (null == ClientUnderTest)
          return String.Empty;

        if (null == ClientUnderTest.OtherInformation)
          return String.Empty;

        return ClientUnderTest.OtherInformation;
      }
      set
      {
        if (null != ClientUnderTest)
          ClientUnderTest.OtherInformation = value;
      }
    }

    public String ProductType
    {
      get
      {
        if (null == ClientUnderTest)
          return String.Empty;

        return ClientUnderTest.ProductType;
      }
      set
      {
        if (null != ClientUnderTest)
          ClientUnderTest.ProductType = value;
      }
    }

    #endregion

    #region Responsible Member

    public String MemberName
    {
      get;
      set;
    }

    public String MemberAddress
    {
      get;
      set;
    }

    #endregion

    #region Test Execution Information

    public String TestOperatorName
    {
      get;
      set;
    }

    public String OrganizationName
    {
      get;
      set;
    }

    public String OrganizationAddress
    {
      get;
      set;
    }

    #endregion

    #region Technical Support Information

    public String TechSupportWebsite
    {
      get;
      set;
    }

    public String TechSupportEmail
    {
      get;
      set;
    }

    public String TechSupportPhone
    {
      get;
      set;
    }

    public String InternationalSupportAddress
    {
      get;
      set;
    }

    public String RegionalSupportAddress
    {
      get;
      set;
    }

    #endregion

    #endregion
  }
}
