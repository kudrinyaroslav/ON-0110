using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;

namespace ProfilesTestLibrary.Tests
{

    /*[TestClass]*/
    public class BaseDummyTest : BaseTest
    {
        public BaseDummyTest(TestLaunchParam param)
            : base(param)
        {
        }


        /// <summary>
        /// Perform common initialization/finalization. Run test action.
        /// </summary>
        /// <param name="action">Test "body"</param>
        protected void RunTest(Action action)
        {
            _halted = false;
            Exception exc = null;

            try
            {
                ResetLog();

                action();

                EndTest(TestStatus.Passed);
            }
            catch (StopEventException)
            {
                LogStepEvent("Halted");
                _halted = true;
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
                return;

            }
        }

        /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        protected void EndTest(TestStatus status)
        {
            SetTestStatus(status);
            TestCompleted();
        }


    }
}
