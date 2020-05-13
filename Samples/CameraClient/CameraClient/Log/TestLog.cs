using System.Collections.Generic;

namespace CameraClient.Log
{
    public class TestLog
    {
        private List<StepResult> _steps = new List<StepResult>();
        public List<StepResult> Steps
        {
            get { return _steps; }
        }

        void AddStepResult(StepResult result)
        {
            _steps.Add(result);
        }

    }
        
    public delegate void TestCompleted(TestLog log);
}
