using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.GUI.SilentProcessing
{
    class SilentOperator : IOperator
    {
        #region IOperator Members

        public bool GetYesNoAnswer(string question)
        {
            return true;
        }

        public bool GetOkCancelAnswer(string question)
        {
            return true;
        }
        
        #endregion
    }
}
