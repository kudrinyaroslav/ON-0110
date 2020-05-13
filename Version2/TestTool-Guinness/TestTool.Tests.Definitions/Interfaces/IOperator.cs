///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;

namespace TestTool.Tests.Definitions.Interfaces
{
    /// <summary>
    /// Interface for getting operator's feedback.
    /// </summary>
    public interface IOperator
    {
        bool GetYesNoAnswer(string question);
        bool GetOkCancelAnswer(string question);
        string GetVideoConfigurationToken(List<string> tokens);

        bool GetDelayTime(string prompt, ref int timeout);
    }

    public class EventsTopicInfo
    {
        public string Topic { get; set; }
        public string NamespacesDefinition { get; set; }
    }

}
