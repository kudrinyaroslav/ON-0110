using System;

namespace TestTool.Tests.Definitions.Attributes
{
    /// <summary>
    /// This attribute marks controls to be added to "Miscellaneoue settings" tabs.
    /// "ParametersType" maps controls to types.
    /// Each test which needs additional parameters has types in "ParametersType" fields.
    /// When tests are loaded, all controls necessary for entering additional values are 
    /// created.
    /// Settings control MUST derive from TestTool.Tests.Definitions.UI.SettingsTabPage class.
    /// Parameters returned MUST be of type specified in attrbute!
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsControlAttribute : Attribute
    {
        public Type ParametersType { get; set; }
    }
}
