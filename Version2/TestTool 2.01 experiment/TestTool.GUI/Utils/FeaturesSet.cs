using System.Collections.Generic;
using TestTool.Tests.Common.Enums;

namespace TestTool.GUI.Utils
{

    /// <summary>
    /// Set of all features
    /// </summary>
    class FeaturesSet
    {
        public FeaturesSet()
        {
            Nodes = new List<FeatureNode>();
        }

        public List<FeatureNode> Nodes { get; private set; }


        /// <summary>
        /// Sets up features accordingly to services selection.
        /// </summary>
        /// <param name="types"></param>
        public void SetupFeatureSet(DeviceType types)
        {
            foreach (FeatureNode node in Nodes)
            {
                if (node.Feature == Feature.ManagementService || node.Feature == Feature.EventsService)
                {
                    node.State = (types != DeviceType.None) ? FeatureState.Mandatory : FeatureState.Optional;
                    node.Check(types != DeviceType.None, node.CheckedManually);
                }

                if (node.Feature == Feature.MediaService)
                {
                    node.State = ((types & DeviceType.NVT) != 0) ? FeatureState.Mandatory : FeatureState.Optional;
                    if ((types & DeviceType.NVT) != 0)
                    {
                        node.Check(true, node.CheckedManually);
                    }
                    else if (types == DeviceType.None)
                    {
                        node.Check(false, false);
                    }
                    else
                    {
                        if (!node.CheckedManually)
                        {
                            node.Check(false, false);
                        }
                    }
                    EnablePTZ();
                }

                UpdateChildFeatures(node);
            }
        }

        /// <summary>
        /// Enables/disabled PTZ in feature set.
        /// </summary>
        public void EnablePTZ()
        {
            bool mediaSelected = false;
            foreach (FeatureNode n in Nodes)
            {
                if (n.Feature == Feature.MediaService)
                {
                    mediaSelected = n.Checked;
                    break;
                }
            }
            foreach (FeatureNode node in Nodes)
            {
                if (node.Feature == Feature.PTZ)
                {
                    if (!mediaSelected)
                    {
                        node.Check(false, false);
                        node.Enabled = false;
                        node.State = FeatureState.Undefined;
                    }
                    else
                    {
                        node.State = FeatureState.Optional;
                        node.Enabled = true;
                    }
                    UpdateChildFeatures(node);
                    break;
                }
            }
        }

        /// <summary>
        /// Propogate changes to child features.
        /// </summary>
        /// <param name="node"></param>
        public static void UpdateChildFeatures(FeatureNode node)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Feature: {0}, Mandatory: {1}, Enabled: {2}",
                    child.Feature, child.Mandatory, node.Checked));

                child.Enabled = node.Checked;
                if (node.Checked)
                {
                    child.State = child.Mandatory ? FeatureState.Mandatory : FeatureState.Optional;
                    if (child.Mandatory)
                    {
                        child.Check(true, false);
                    }
                }
                else
                {
                    child.Check(false, false);
                    child.State = FeatureState.Undefined;
                }
                UpdateChildFeatures(child);
            }
        }

        /// <summary>
        /// Selects nodes
        /// </summary>
        /// <param name="services"></param>
        /// <param name="features"></param>
        public void SelectNodes(IList<Service> services,
            IList<Feature> features)
        {
            Dictionary<Service, FeatureNode> nodes = new Dictionary<Service, FeatureNode>();
            foreach (FeatureNode n in Nodes)
            {
                Service service = Service.Device;
                switch (n.Feature)
                {
                    case Feature.ManagementService:
                        service = Service.Device;
                        break;
                    case Feature.EventsService:
                        service = Service.Events;
                        break;
                    case Feature.MediaService:
                        service = Service.Media;
                        break;
                    case Feature.PTZ:
                        service = Service.PTZ;
                        break;
                }
                nodes.Add(service, n);
            }

            foreach (Service service in nodes.Keys)
            {
                nodes[service].Check(services.Contains(service), false);
                UpdateChildFeatures(nodes[service]);
                if (service == Service.Media)
                {
                    EnablePTZ();
                }
                foreach (FeatureNode child in nodes[service].Nodes)
                {
                    DefineState(nodes[service], child);
                    SelectFeatures(child, features);
                }
            }

        }

        /// <summary>
        /// Selects child features.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="features"></param>
        static void SelectFeatures(FeatureNode node, IList<Feature> features)
        {
            node.Check(features.Contains(node.Feature), false);
            foreach (FeatureNode child in node.Nodes)
            {
                DefineState(node, child);
                SelectFeatures(child, features);
            }
        }

        /// <summary>
        /// Defines node's state.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="child"></param>
        static void DefineState(FeatureNode node, FeatureNode child)
        {
            child.Enabled = node.Checked;
            if (node.Checked)
            {
                child.State = child.Mandatory ? FeatureState.Mandatory : FeatureState.Optional;
                if (child.Mandatory)
                {
                    child.Check(true, false);
                }
            }
            else
            {
                child.Check(false, false);
                child.State = FeatureState.Undefined;
            }

        }

    }
}
