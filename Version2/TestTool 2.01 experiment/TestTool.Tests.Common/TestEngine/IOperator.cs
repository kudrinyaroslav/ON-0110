///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Interface for getting operator's feedback.
    /// </summary>
    public interface IOperator
    {
        bool GetYesNoAnswer(string question);
        bool GetOkCancelAnswer(string question);
        string GetVideoConfigurationToken(List<string> tokens);
        string GetSecureAPI();

        bool GetEventsTopic(List<EventsTopicInfo> predefinedFilters, out EventsTopicInfo topic);
        bool GetSubscriptionTimeout(string prompt, string eventAction, ref int timeout);
    }

    public class EventsTopicInfo
    {
        public string Topic { get; set; }
        public string NamespacesDefinition { get; set; }
    }

}
