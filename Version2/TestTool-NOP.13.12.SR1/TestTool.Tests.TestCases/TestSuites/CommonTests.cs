using System;
using System.Collections.Generic;
using System.Text;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Proxies.Onvif;
//using TestTool.Tests.Definitions.Enums;


namespace TestTool.Tests.TestCases.TestSuites
{
    public delegate void AssertDelegate(bool condition, string message, string stepName, string stepDetails);

    public delegate void RunStepDelegate(
        Action action, string stepName, string expectedFault, bool acceptOtherFaults, bool allowNoFault);
    
    public static class CommonTests
    {
        public static void InvalidTokenTestBody<T>(this BaseTest test,
            Func<string, T> method,
            RunStepDelegate runStepDelegate,
            string itemName,
            string expectedFault)
        {
            string token = Guid.NewGuid().ToString().Substring(0, 8);
            runStepDelegate(() => { T info = method(token); },
                string.Format("Get {0} with invalid token", itemName), expectedFault, true, false);

        }

        public static void InvalidTokenTestBody(this BaseTest test,
            Action<string> method,
            RunStepDelegate runStepDelegate,
            string itemName,
            string expectedFault)
        {
            string token = Guid.NewGuid().ToString().Substring(0, 8);
            runStepDelegate(() => { method(token); },
                string.Format("Get {0} with invalid token", itemName), expectedFault, true, false);

        }

        public static void InvalidTokenTestBody<T>(this BaseTest test,
            Action<string, T> method,
            T parameter,
            RunStepDelegate runStepDelegate,
            string message,
            string token,
            string expectedFault)
        {
            token = token ?? Guid.NewGuid().ToString().Substring(0, 8);
            runStepDelegate(() => { method(token, parameter); },
                message, expectedFault, true, false);
        }

        // !!!
        // Maybe it's need to send some addition parameter to method InvalidTokenTestBody to 
        // unite with InvalidTokenTestBody(.., Action<string, T> method, ..).
        // E.g. 
        //public static void InvalidTokenTestBody(this BaseTest test,
        //    Action<string> method,
        //    Feature operation,
        //    RunStepDelegate runStepDelegate,
        //    string itemName,
        //    string expectedFault)
        //{
        //    string token = Guid.NewGuid().ToString().Substring(0, 8);
        //    runStepDelegate(() => { method(token); },
        //        string.Format("{0} {1} with invalid token", operation, itemName), expectedFault, true, false);

        //}
        // Here operation is Feature.Get, Feature.Delete or something else.
        // Or maybe it's right to send something like "Get Recording Information" or "Delete Receiver" instead of itemName
         
        /// <summary>
        /// Checks that tokens in list are different
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="test">Class containing test method in which validation should be performed.</param>
        /// <param name="list">Items list</param>
        /// <param name="tokenSelector">Function to get token from Item</param>
        /// <param name="assert"></param>
        public static void ValidateTokensInList<T>(this BaseTest test,
            IEnumerable<T> list,
            Func<T, string> tokenSelector,
            AssertDelegate assert)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = TestUtils.ValidateTokens(list, tokenSelector, logger);
            assert(ok, logger.ToStringTrimNewLine(), "Check that tokens are unique", string.Empty);
        }
    }
}
