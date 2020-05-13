///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Engine
{
  /// <summary>
  /// Hierarchycal set of all features
  /// </summary>
  public class FeaturesSet
  {
    private FeaturesSet()
    {
      Nodes = new List<FeatureNode>();
    }

    /// <summary>
    /// Root nodes
    /// </summary>
    public List<FeatureNode> Nodes
    {
      get;
      private set;
    }

    /// <summary>
    /// Finds node for feature specified
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    public FeatureNode FindNode(Feature feature)
    {
      foreach (FeatureNode node in Nodes)
      {
        if (node.Feature == feature)
        {
          return node;
        }
        else
        {
          FeatureNode n = FindNode(node, feature);
          if (n != null)
          {
            return n;
          }
        }
      }
      return null;
    }

    private FeatureNode FindNode(FeatureNode node, Feature feature)
    {
      foreach (FeatureNode child in node.Nodes)
      {
        if (child.Feature == feature)
        {
          return child;
        }
        else
        {
          FeatureNode n = FindNode(child, feature);
          if (n != null)
          {
            return n;
          }
        }
      }
      return null;
    }

    public static FeaturesSet CreateFeaturesSet()
    {
      FeaturesSet featuresSet = new FeaturesSet();

      foreach (Feature group in FeatureHelper.GetFeaturesWithType(NodeType.Parent))
      {
        FeatureNode groupNode = new FeatureNode {
                                                  Name = group.GetDisplayName(),
                                                  Feature = group,
                                                  State = FeatureState.Undefined,
                                                  Type = NodeType.Parent
                                                };

        foreach (Feature feature in group.GetChildFeatures())
          groupNode.Nodes.Add(new FeatureNode {
                                                Name = feature.GetDisplayName(),
                                                Feature = feature,
                                                State = FeatureState.Undefined,
                                                Type = NodeType.Feature
                                              });

        featuresSet.Nodes.Add(groupNode);
      }

      return featuresSet;
    }
  }
}
