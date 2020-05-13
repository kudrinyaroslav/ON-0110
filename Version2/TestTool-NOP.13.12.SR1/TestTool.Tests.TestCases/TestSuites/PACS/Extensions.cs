using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.TestBase;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    public delegate string GetListMethod<T>(int? limit, string offset, out T[] list);

    public delegate string GetListStepMethod<T>(int? limit, string offset, out T[] list, string stepName);

    public static class Extensions
    {
        public delegate void AssertDelegate(bool condition, string message, string stepName, string stepDetails);

        public delegate void RunStepDelegate(
            Action action, string stepName, string expectedFault, bool acceptOtherFaults, bool allowNoFault);


        /// <summary>
        /// Checks that list contains exactly one item with specified token
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="test">Class containing test method in which validation should be performed.</param>
        /// <param name="list">Items list</param>
        /// <param name="token">Token of desired item</param>
        /// <param name="tokenSelector">Function to get token from Item</param>
        /// <param name="itemName">Name of item for eror description</param>
        /// <param name="assert"></param>
        public static void CheckRequestedInfo<T>(this BaseTest test, IEnumerable<T> list,
            string token,
            Func<T, string> tokenSelector,
            string itemName,
            AssertDelegate assert)
        {
            string error = string.Empty;

            int count = 0;
            if (list != null)
            {
                count = list.Count();
            }

            if (count == 0)
            {
                error = string.Format("No {0} information returned", itemName);
            }
            else
            {
                if (count > 1)
                {
                    error = "More than one entry returned";
                }
                else
                {
                    T item = list.First();
                    if (tokenSelector(item) != token)
                    {
                        error = "Entry for other token returned";
                    }
                }
            }
            assert(string.IsNullOrEmpty(error), error, "Check response", string.Empty);

        }
        
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

        /// <summary>
        /// Checks that list contains only one item and this item holds expected information
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="test">Class containing test method in which validation should be performed.</param>
        /// <param name="list">Items list</param>
        /// <param name="expected"></param>
        /// <param name="tokenSelector">Function to get token from Item</param>
        /// <param name="itemComparison">Method for comparing single items.</param>
        /// <param name="itemName">Name of item for eror description</param>
        /// <param name="assert"></param>
        public static void CheckRequestedInfo<T>(this BaseTest test, IEnumerable<T> list,
            T expected,
            Func<T, string> tokenSelector,
            Func<T, T, StringBuilder, bool> itemComparison,
            string itemName,
            AssertDelegate assert)
        {
            string error = string.Empty;
            string token = tokenSelector(expected);

            int count = 0;
            if (list != null)
            {
                count = list.Count();
            }

            if (count == 0)
            {
                error = string.Format("No {0} information returned", itemName);
            }
            else
            {
                if (count > 1)
                {
                    error = "More than one entry returned";
                }
                else
                {
                    T item = list.First();
                    if (tokenSelector(item) != token)
                    {
                        error = "Entry for other token returned";
                    }
                    else
                    {
                        StringBuilder dump =new StringBuilder("Information is different: " + Environment.NewLine);
                        bool ok = itemComparison(expected, item, dump);
                        if (!ok)
                        {
                            error = dump.ToStringTrimNewLine();
                        }
                    }
                }
            }
            assert(string.IsNullOrEmpty(error), error, "Check response", string.Empty);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="test"></param>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="itemName"></param>
        /// <param name="tokenSelector"></param>
        /// <param name="itemComparer"></param>
        /// <param name="assert"></param>
        public static void ValidateSubset<T>(this BaseTest test, IEnumerable<T> actual,
            IEnumerable<T> expected,
            string itemName,
            Func<T, string> tokenSelector,
            Func<T, T, StringBuilder, bool> itemComparer,
            AssertDelegate assert)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            /****************/

            foreach (T info in expected)
            {
                string token = tokenSelector(info);
                T[] foundItems =
                    actual.Where(I => tokenSelector(I) == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found {2}",
                            itemName, token, Environment.NewLine);
                    ok = false;
                }
                else
                {
                    common.Add(token);
                }
            }

            foreach (T info in actual)
            {
                string token = tokenSelector(info);
                T[] foundItems =
                    expected.Where(I => tokenSelector(I) == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not expected {2}",
                            itemName, token, Environment.NewLine);
                    ok = false;
                }
            }

            /****************/

            // for common only
            if (itemComparer != null)
            {
                bool local = ArrayUtils.CompareListItems(actual, expected, tokenSelector, common, itemName, logger,
                                                         itemComparer);
                ok = ok && local;
            }

            assert(ok, logger.ToStringTrimNewLine(), "Check that lists are the same", null);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="test"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="itemName"></param>
        /// <param name="tokenSelector"></param>
        /// <param name="description1"></param>
        /// <param name="description2"></param>
        /// <param name="itemComparer"></param>
        /// <param name="assert"></param>
        public static void ValidateOrderedLists<T>(this BaseTest test, IList<T> list1,
            IList<T> list2,
            string itemName,
            Func<T, string> tokenSelector,
            string description1,
            string description2,
            Func<T, T, StringBuilder, bool> itemComparer,
            AssertDelegate assert)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            /****************/

            foreach (T info in list1)
            {
                string token = tokenSelector(info);
                T[] foundItems =
                    list2.Where(I => tokenSelector(I) == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found in list {2}{3}",
                            itemName, tokenSelector(info), description2, Environment.NewLine);
                    ok = false;
                }
                else
                {
                    common.Add(token);
                }
            }

            foreach (T info in list2)
            {
                string token = tokenSelector(info);
                T[] foundItems =
                    list1.Where(I => tokenSelector(I) == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found in {2}{3}",
                            itemName, tokenSelector(info), description1, Environment.NewLine);
                    ok = false;
                }
            }

            /****************/

            bool orderOk = true;
            // only if sets match
            if (ok)
            {
                for (int i = 0; i< list1.Count; i++)
                {
                    string token1 = tokenSelector(list1[i]);
                    string token2 = tokenSelector(list2[i]);
                    if (token1 != token2)
                    {
                        orderOk = false;
                        break;
                    }
                }
            }
            
            if (!orderOk)
            {
                logger.AppendLine(string.Format("Order of {0} is different{1}", itemName, Environment.NewLine));
                ok = false;
            }

            /****************/

            // for common only
            if (itemComparer != null)
            {
                bool local = ArrayUtils.CompareListItems(list1, list2, tokenSelector, common, itemName, logger,
                                                         itemComparer);
                ok = ok && local;
            }

            assert(ok, logger.ToStringTrimNewLine(), "Check that lists are the same", null);
        }

        public static void ValidateNextPart(this BaseTest test,
            IEnumerable<string> gotByThisMoment,
            IEnumerable<string> newItems,
            string itemName,
            AssertDelegate assert)
        {
            List<string> newNotUnique = new List<string>();     // intersection within new part
            List<string> duplicated = new List<string>();       // intersection with received previously

            foreach (string token in newItems)
            {
                if (newItems.Count( S => S == token) > 1)
                {
                    newNotUnique.Add(token);
                }
                if (gotByThisMoment.Count(S => S == token) > 0)
                {
                    duplicated.Add(token);
                }
            }

            bool ok = true;
            StringBuilder error = new StringBuilder();
            if (newNotUnique.Count >0)
            {
                ok = false;
                error.AppendLine("The following tokens are not unique: " + string.Join(", ", newNotUnique.ToArray()));
            }
            if (duplicated.Count > 0)
            {
                ok = false;
                error.AppendLine("The following tokens have been already received: " + string.Join(", ", duplicated.ToArray()));
            }

            assert(ok, error.ToStringTrimNewLine(), "Validate response received", string.Empty);

        }
        
        public static List<T> GetFullList<T>(
            GetListMethod<T> getList,
            int? chunk,
            string itemName,
            AssertDelegate assert)
        {
            List<T> fullList = new List<T>();
            if (chunk > 0 || !chunk.HasValue)
            {
                string currentOffset = null;
                while (true)
                {
                    T[] portion = null;
                    currentOffset = getList(chunk, currentOffset, out portion);

                    if (portion != null)
                    {
                        if (chunk.HasValue)
                        {
                            assert(portion.Length <= chunk,
                                string.Format("{0} {1}s returned", portion.Length, itemName),
                                "Check that MaxLimit parameter is not exceeded",
                                string.Empty);
                        }

                        fullList.AddRange(portion);
                    }
                    if (string.IsNullOrEmpty(currentOffset))
                    {
                        break;
                    }
                }
            }
            return fullList;
        }

        #region TestBody

        public static void InvalidTokenTestBody<T>(this BaseTest test,
            Func<string[], T[]> listSelector,
            RunStepDelegate runStepDelegate,
            string itemName)
        {
            InvalidTokenTestBody(test, listSelector, runStepDelegate, itemName, OnvifFaults.NotFound );
        }

        public static void InvalidTokenTestBody<T>(this BaseTest test, 
            Func<string[], T[]> listSelector,
            RunStepDelegate runStepDelegate, 
            string itemName,
            string expectedFault)
        {
            string token = Guid.NewGuid().ToString().Substring(0, 8);
            T[] infosList = null;
            runStepDelegate(() => { infosList = listSelector(new string[] { token }); }, 
                string.Format("Get {0} with invalid token", itemName), expectedFault, true, false);

        }

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

        public static void CommonGetListByTokenListTestBody<T>(this BaseTest test, 
            Func<int> maxLimitSelector,
            Func<List<T>> fullListSelector,
            Func<string[], T[]> listSelector,
            Func<T, string> tokenSelector,
            Action<IEnumerable<T>, IEnumerable<T>, string, string> listComparisonAction,
            string itemName,
            string itemInfoName,
            RunStepDelegate runStepDelegate,
            AssertDelegate assert)
        {

            List<T> infos = fullListSelector();

            if (infos == null || infos.Count == 0)
            {
                return;
            }

            ValidateTokensInList(test, infos, tokenSelector, assert);
            
            //
            int maxLimit = maxLimitSelector();

            List<string> tokens = new List<string>();
            foreach (T info in infos)
            {
                tokens.Add(tokenSelector(info));
            }
            
            List<T> expected = new List<T>();
            List<string> tokensForSubsetTest = new List<string>();

            // create list of randomly selected tokens.
            // create "expected" list
            {
                List<T> fullListCopy = new List<T>(infos);

                for (int i = 0; i< maxLimit; i++)
                {
                    Random rnd = new Random();
                    int idx = rnd.Next(0, fullListCopy.Count - 1);
                    tokensForSubsetTest.Add(tokenSelector(fullListCopy[idx]));
                    expected.Add(fullListCopy[idx]);
                    fullListCopy.RemoveAt(idx);
                    if (fullListCopy.Count == 0)
                    {
                        break;
                    }
                }
            }

            // test for random subset
            T[] actual = listSelector(tokensForSubsetTest.ToArray());

            assert(actual != null && actual.Length > 0, 
                "Empty list returned", "Check that the list is not empty", "");

            ValidateTokensInList(test, actual, tokenSelector, assert);

            test.ValidateSubset(actual, expected, itemName, tokenSelector, null, assert);
            
            // Get by one

            foreach (string token in tokens)
            {
                T[] shortList = listSelector(new string[] { token });

                CheckRequestedInfo(test, shortList, token, tokenSelector, itemName, assert);
            }
        }

        public static void CommonGetListStartReferenceTestBody<T>(this BaseTest test,
            Func<int> maxLimitSelector,
            Func<int, List<T>> fullListSelector, 
            Func<int?, int?, string, T[]> listSelector,
            Func<T, string> tokenSelector,
            Action<IEnumerable<T>, IEnumerable<T>, string, string> listComparisonAction,
            Func<T, T, StringBuilder, bool> itemComparison,
            string itemName,
            RunStepDelegate runStepDelegate,
            AssertDelegate assert)
        {
            int maxLimit = maxLimitSelector();

            List<T> infos = fullListSelector(maxLimit);

            if (infos == null || infos.Count == 0)
            {
                return;
            }

            test.ValidateTokensInList(infos, tokenSelector, assert);

            int count = infos.Count;

            // one last item

            if (count > 1)
            {
                T[] shortList = listSelector(maxLimit, count - 1, string.Format("Get {0} list with offset = {1}", itemName, count - 1));

                test.CheckRequestedInfo<T>(shortList, infos[count - 1], tokenSelector, itemComparison, itemName, assert);
            }

            // first "maxLimit" items
            {
                List<T> expected = infos.Take(maxLimit).ToList(); 

                T[] actual = listSelector(maxLimit, 0, string.Format("Get {0} list with offset = 0", itemName));

                test.ValidateTokensInList(actual, tokenSelector, assert);

                listComparisonAction(expected, actual, string.Format("of first {0} {1}", maxLimit, itemName),
                             "received when passing offset=0");
            }


            if (count > 2)
            {
                int offset = count / 2; // 3 => 1, 4 => 2, 5 => 2, 6 => 3 

                System.Diagnostics.Debug.WriteLine(string.Format("Get {0} with offset = {1}", itemName, offset));

                // expected list

                T[] infosArray = infos.ToArray();

                int cnt = Math.Min(maxLimit, count - offset);

                T[] expected = new T[cnt];

                Array.Copy(infosArray, offset, expected, 0, cnt);


                T[] actual = listSelector(maxLimit, offset, string.Format("Get {0} list with offset = {1}", itemName, offset));

                test.ValidateTokensInList(actual, tokenSelector, assert);

                test.ValidateSubset(actual, expected, itemName, tokenSelector, itemComparison, assert);
            }

        }

        public static void CommonGetListLimitTestBody<T>(this BaseTest test,
            Func<int> maxLimitSelector,
            Func<List<T>> fullListSelector,
            GetListStepMethod<T> listSelector,
            Func<T, string> tokenSelector,
            string itemName,
            AssertDelegate assert)
        {
            // Get max limit
            int maxLimit = maxLimitSelector();

            // Get full list. Exit test, if no items of interest registered
            List<T> infos = fullListSelector();

            if (infos == null || infos.Count == 0)
            {
                return;
            }

            test.ValidateTokensInList(infos, tokenSelector, assert);

            int count = infos.Count;

            // Get one item
            {
                T[] shortList = null;
                listSelector(1, null, out shortList, string.Format("Get {0} list with limit = 1", itemName));

                assert(shortList != null, string.Format("No {0}s returned", itemName), "Check that result is not null", string.Empty);

                assert(shortList.Length <= 1,
                       string.Format("{0} {1}s returned when limit is {2}", shortList.Length, itemName, 1), 
                       "Check that limit is not exceeded",
                       string.Empty);

                assert(shortList.Length <= maxLimit,
                       string.Format("{0} {1}s returned when limit is {2}", shortList.Length, itemName, maxLimit),
                       "Check that MaxLimit is not exceeded",
                       string.Empty);
            }

            // maxLimit
            if (count > 1)
            {
                T[] actual = null;

                listSelector(maxLimit, null, out actual, string.Format("Get {0} list with limit = {1}", itemName, maxLimit));

                assert(actual != null, string.Format("No {0}s returned", itemName), "Check that result is not null", string.Empty);

                assert(actual.Length <= maxLimit,
                       string.Format("{0} {1}s returned when limit is {2}", actual.Length, itemName, maxLimit),
                       "Check that limit is not exceeded",
                       string.Empty);


                test.ValidateTokensInList(actual, tokenSelector, assert);
            }
            
            
            int cnt = Math.Min(count, maxLimit);

            int limit = cnt / 2 + 1;  // 3 => 2, 4=> 3, 5=> 3,

            if (limit != maxLimit && limit != 1)
            {
                T[] infosArray = infos.ToArray();

                System.Diagnostics.Debug.WriteLine(string.Format("Get {0} with limit = {1}", itemName, limit));

                T[] expected = new T[limit];

                Array.Copy(infosArray, 0, expected, 0, limit);

                T[] actual = null;
                listSelector(limit, null, out actual, string.Format("Get {0} list with limit = {1}", itemName, limit));
                
                assert(actual != null, string.Format("No {0}s returned", itemName), "Check that result is not null", string.Empty);

                assert(actual.Length <= limit,
                       string.Format("{0} {1}s returned when limit is {2}", actual.Length, itemName, limit),
                       "Check that limit is not exceeded",
                       string.Empty);


                test.ValidateTokensInList(actual, tokenSelector, assert);

            }
        }

        public static void CommonGetListStartReferenceLimitTestBody<T>(this BaseTest test,
            Func<int> maxLimitSelector,
            GetListStepMethod<T> listSelector,
            Func<T, string> tokenSelector,
            Func<T, T, StringBuilder, bool> itemComparison,
            string itemName,
            RunStepDelegate runStepDelegate,
            AssertDelegate assert)
        {
            // Get maxLimit
            
            int maxLimit = maxLimitSelector();

            Func<int, List<T> > getList = (limit) =>
                               {
                                   List<T> fullList = new List<T>();
                                   
                                   List<string> gotByThisMoment = new List<string>();
                                   string offset = null;
                                   while (true)
                                   {
                                       T[] nextPart = null;
                                       offset =  listSelector(limit, offset, out nextPart, string.Format("Get {0} list with limit = {1} and start reference ='{2}'", itemName, limit, offset));

                                       //assert(nextPart != null, string.Format("No {0}s returned", itemName), "Check that result is not null", string.Empty);

                                       int count = nextPart == null ? 0 : nextPart.Length;

                                       assert(count <= limit,
                                              string.Format("{0} {1}s returned when limit is {2}", count,
                                                            itemName, limit), "Check that limit is not exceeded",
                                              string.Empty);

                                       if (limit > maxLimit)
                                           assert(count <= maxLimit,
                                                  string.Format("{0} {1}s returned when limit is {2}", count, itemName, maxLimit),
                                                  "Check that MaxLimit is not exceeded",
                                                  string.Empty);

                                       if (count > 0)
                                       {
                                           List<string> newTokens = nextPart.Select(tokenSelector).ToList();
                                          
                                           test.ValidateNextPart(gotByThisMoment, newTokens, itemName, assert);

                                           gotByThisMoment.AddRange(newTokens);

                                           fullList.AddRange(nextPart);
                                       }

                                       if (string.IsNullOrEmpty(offset))
                                       {
                                           break;
                                       }                                       
                                   }

                                   return fullList;
                               };

            // get full list by maxLimit chunks
            
            List<T> gotByMaxLimit = getList(maxLimit);

            // get full list by 1 item

            List<T> gotByOne = getList(1);

            // check order
            // compare items

            //test.ValidateOrderedLists(gotByMaxLimit, gotByOne, itemName, tokenSelector,
            //                          string.Format("received with limit ={0}", maxLimit), "received with limit=1",
            //                          itemComparison, assert);
            test.ValidateSubset(gotByOne, gotByMaxLimit, itemName, tokenSelector, itemComparison, assert);


            if (maxLimit > 2)
            {
                int middle = maxLimit/2 + 1; // 3=>2, 4=>3, 5=>3

                List<T> gotByMiddle = getList(middle);

                // check order
                // compare items

                //test.ValidateOrderedLists(gotByOne, gotByMiddle, itemName, tokenSelector,
                //                          "received with limit=1",
                //                          string.Format("received with limit ={0}", middle), 
                //                          itemComparison, assert);
                test.ValidateSubset(gotByMiddle, gotByMaxLimit, itemName, tokenSelector, itemComparison, assert);

            }
        }

        public static void CommonGetListNoLimitTestBody<T>(this BaseTest test,
            Func<int> maxLimitSelector,
            GetListStepMethod<T> listSelector,
            Func<T, string> tokenSelector,
            string itemName,
            AssertDelegate assert)
        {
            // Get maxLimit

            int maxLimit = maxLimitSelector();

            List<T> fullList = new List<T>();

            List<string> gotByThisMoment = new List<string>();
            string offset = null;
            while (true)
            {
                T[] nextPart = null;
                offset = listSelector(null, offset, out nextPart, string.Format("Get {0} list without limit and start reference ='{1}'", itemName, offset));

                int count = nextPart == null ? 0 : nextPart.Length;

                assert(count <= maxLimit,
                       string.Format("{0} {1}s returned when MaxLimit is {2}", count,
                                     itemName, maxLimit), "Check that limit is not exceeded",
                       string.Empty);

                if (count > 0)
                {
                    List<string> newTokens = nextPart.Select(tokenSelector).ToList();

                    test.ValidateNextPart(gotByThisMoment, newTokens, itemName, assert);

                    gotByThisMoment.AddRange(newTokens);

                    fullList.AddRange(nextPart);
                }

                if (string.IsNullOrEmpty(offset))
                {
                    break;
                }
            }
        }


        public static void CommonGetInfoTestBody<T>(this BaseTest test,
            Func<List<T>> fullListSelector,
            Func<string, T> infoSelector,
            Func<T, string> tokenSelector,
            Func<T, T, StringBuilder, bool> itemComparison,
            string itemName,
            string getInfoMethodName,
            string getListMethodName,
            AssertDelegate assert)
        {
            // Get valid tokens
            List<T> infos = fullListSelector();
            
            foreach (T info in infos)
            {
                string token = tokenSelector(info);
                T itemInfo = infoSelector(token);

                StringBuilder logger = new StringBuilder("Information is different:" + Environment.NewLine);
                bool tokenOk = true;
                if (token != tokenSelector(itemInfo))
                {
                    tokenOk = false;
                    logger.AppendLine("   Token is different");
                }

                bool ok = itemComparison(info, itemInfo, logger);
                ok = ok && tokenOk;

                string stepName = string.Format("Compare {0} got via {1} and via {2}", itemName, getInfoMethodName, getListMethodName);
                assert(ok, logger.ToStringTrimNewLine(),stepName, string.Empty);
            }

        }


        #endregion

    }
}
