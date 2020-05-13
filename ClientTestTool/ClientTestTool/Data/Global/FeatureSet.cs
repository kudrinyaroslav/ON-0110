///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.FeatureSet.Base;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Global
{
  /// <summary>
  /// Hierarchycal set of all features
  /// </summary>
  public class FeatureSet : BaseFeatureSet, IEnumerable<FeatureNode>
  {
    #region Singleton

    private static FeatureSet mInstance;

    public static FeatureSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new FeatureSet());
      }
    }

    static FeatureSet()
    {}

    #endregion

    private FeatureSet() : base()
    {
    }

    public void ClearTestResults()
    {
      var featureInfos = mPlainFeatures.Value.SelectMany(item => new List<FeatureInfo>(item.Nodes.Select(node => ((FeatureNode)node).Info)) { item.Info });

      foreach (var info in featureInfos)
        info.Status = FeatureStatus.Undefined;
    }

    public List<FeatureNode> GetFeatures(Profile profile)
    {
      return mProfilesMap.Value[profile].ToList();
    }

    public FeatureInfo GetInfo(Feature feature)
    {
      foreach (var pNode in Nodes)
      {
        if (pNode.Feature == feature)
          return pNode.Info;

        var cNode = pNode.Nodes.Cast<FeatureNode>().FirstOrDefault(item => item.Feature == feature);

        if (null != cNode)
          return cNode.Info;
      }

      throw new ArgumentException();
    }

    #region Properties

    /// <summary>
    /// Root nodes
    /// </summary>
    public List<FeatureNode> Nodes
    {
      get
      {
        return mPlainFeatures.Value.ToList();
      }
    }

    #endregion

    public IEnumerator<FeatureNode> GetEnumerator()
    {
      return Nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
