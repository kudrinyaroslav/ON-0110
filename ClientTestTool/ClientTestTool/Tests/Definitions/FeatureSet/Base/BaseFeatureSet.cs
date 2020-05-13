///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Engine.Builder;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.FeatureSet.Base
{
  public abstract class BaseFeatureSet
  {
    protected BaseFeatureSet()
    {
      mPlainFeatures = new Lazy<List<FeatureNode>>(() => new FeatureNodeListBuilder().Build());
      mProfilesMap   = new Lazy<Dictionary<Profile, List<FeatureNode>>>(() => new ProfileMapBuilder(mPlainFeatures.Value).Build());
    }

    protected readonly Lazy<List<FeatureNode>>                      mPlainFeatures;
    protected readonly Lazy<Dictionary<Profile, List<FeatureNode>>> mProfilesMap;
  }
}
