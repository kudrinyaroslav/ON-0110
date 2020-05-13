///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Threading;
using TestTool.Tests.Definitions.Data;

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

        WaitHandle ShowMessage(string message);
        void HideMessage();

        WaitHandle ShowCountdownMessage(int timeout, DoorSelectionData data);
        void HideCountdownMessage();

        WaitHandle ShowDoorSelectionMessage(DoorSelectionData data);
        void HideDoorSelectionMessage();
    }

    public class EventsTopicInfo
    {
        public string Topic { get; set; }
        public string NamespacesDefinition { get; set; }
    }

}
