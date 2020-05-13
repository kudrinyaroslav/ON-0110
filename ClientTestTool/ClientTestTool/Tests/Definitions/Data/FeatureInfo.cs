///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Utils;
using ClientTestTool.Properties;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Data
{
  public class FeatureInfo : INotifyPropertyChanged
  {

    #region Builder

    internal static class Builder
    {
      public static FeatureInfo Build(Feature feature, FeatureType type)
      {
        var obj = new FeatureInfo(feature, type);

        var requirements = new Dictionary<Profile, RequirementLevel>();
        var attributes   = feature.GetAttributes<ProfileAttribute>();
        foreach (Profile profile in Enum.GetValues(typeof(Profile)))
        {
          var requirement = feature.GetRequirementLevel(profile);
          requirements.Add(profile, requirement);
        }

        obj.Requirement = new ReadOnlyDictionary<Profile, RequirementLevel>(requirements);

        return obj;
      }
    }

    #endregion

    #region FeatureInfo

    private FeatureInfo(Feature feature, FeatureType type)
    {
      Name        = feature.GetDisplayName();
      Feature     = feature;
      Type        = type;
      Status      = FeatureStatus.Undefined;
    }

    #region Properties

    public String Name
    {
      get;
      private set;
    }

    public Feature Feature
    {
      get;
      private set;
    }

    internal FeatureType Type
    {
      get;
      private set;
    }

    internal ReadOnlyDictionary<Profile, RequirementLevel> Requirement
    {
      get;
      private set;
    }

    private FeatureStatus mStatus;

    public FeatureStatus Status
    {
      get
      {
        return mStatus;
      }
      set
      {
        if (value == mStatus)
          return;

        mStatus = value;

        OnPropertyChanged();
      }
    }

    #endregion

    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #endregion
  }
}
