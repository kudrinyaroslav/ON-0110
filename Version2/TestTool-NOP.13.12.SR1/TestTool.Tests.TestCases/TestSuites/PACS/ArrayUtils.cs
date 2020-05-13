using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.TestCases.TestSuites
{
    class ArrayUtils
    {
        /// <summary>
        /// Checks that for all items in first list item with the same token is presented in 
        /// second list and vice versa.
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="tokenSelector">Function for tokens retrieval</param>
        /// <param name="common">List of common tokens (actually out parameters)</param>
        /// <param name="itemName">Name if item</param>
        /// <param name="description1">Description of the first list</param>
        /// <param name="description2">Description of the second list</param>
        /// <param name="logger">Logger to append error description, if needed.</param>
        /// <returns>True if set of tokens is the same in both lists.</returns>
        public static bool CompareLists<T>(IEnumerable<T> list1,
            IEnumerable<T> list2,
            Func<T, string> tokenSelector,
            List<string> common,
            string itemName,
            string description1,
            string description2,
            StringBuilder logger)
        {
            bool ok = true;

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

            return ok;
        }

        /// <summary>
        /// Compares common items (with the same token) in two lists.
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="tokenSelector">Function for tokens retrieval</param>
        /// <param name="common">List of common tokens</param>
        /// <param name="itemName">Name if item</param>
        /// <param name="logger">Logger to append error description, if needed.</param>
        /// <param name="comparer">Method for comparing single items</param>
        /// <returns>True if imformation for all items is the same.</returns>
        public static bool CompareListItems<T>(IEnumerable<T> list1,
            IEnumerable<T> list2,
            Func<T, string> tokenSelector,
            List<string> common,
            string itemName,
            StringBuilder logger,
            Func<T, T, StringBuilder, bool> comparer)
        {
            bool ok = true;

            foreach (T info1 in list1)
            {
                string token = tokenSelector(info1);
                if (!common.Contains(token))
                {
                    continue;
                }

                T info2 = list2.Where(I => tokenSelector(I) == token).FirstOrDefault();

                StringBuilder dump =
                    new StringBuilder(string.Format("Information for {0} with token '{1}' is different:{2}",
                        itemName, token, Environment.NewLine));

                bool localOk = comparer(info1, info2, dump);

                if (!localOk)
                {
                    logger.Append(dump.ToString());
                    ok = false;
                }
            }

            return ok;
        }
    }
}
