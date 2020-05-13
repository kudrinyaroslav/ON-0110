using System;

namespace CameraClient.Log
{
    public enum StepStatus
    {
        Passed,
        Failed
    }

    public class StepResult
    {
        public int Number { get; set; }
        public string StepName { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public StepStatus Status { get; set; }
        public Exception Exception { get; set; }
    }

    public delegate void StepCompleted(StepResult result);

}
