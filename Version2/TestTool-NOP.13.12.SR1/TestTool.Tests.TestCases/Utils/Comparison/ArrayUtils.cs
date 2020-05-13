﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.TestCases.Utils.Comparison
{
    public static class ArrayUtils
    {
        public static bool ValidateTokens<T>(
            IEnumerable<T> list,
            Func<T, string> tokenSelector,
            StringBuilder logger)
        {
            bool tokensOk = true;
            List<string> logged = new List<string>(); 
            foreach (T item in list)
            {
                string token = tokenSelector(item);
                int count = list.Where(I => tokenSelector(I) == token).Count();
                if (count != 1)
                {
                    tokensOk = false;
                    if (!logged.Contains(token))
                    {
                        logger.AppendFormat("Token '{0}' is not unique{1}", token, Environment.NewLine);
                        logged.Add(token);
                    }
                    //break;
                }
            }

            return tokensOk;
        }

        public static string GetTokensList<T>(
            IEnumerable<T> list,
            Func<T, string> tokenSelector,
            string separator)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;

            foreach (T item in list)
            {
                string token = tokenSelector(item);
                if (!first)
                {
                    sb.Append(separator);
                }
                sb.AppendFormat(" {0}", token);
                first = false;
            }

            return sb.ToString();
        }


        public static bool CompareTokensLists<T1, T2>(
            IEnumerable<T1> list1,
            Func<T1, string> tokenSelector1,
            string list1Description,
            IEnumerable<T2> list2,
            Func<T2, string> tokenSelector2,
            string list2Description,
            string itemName,
            StringBuilder logger)
        {
            bool ok = true;

            foreach (T1 item in list1)
            {
                string token = tokenSelector1(item);
                int count = list2.Count(I => tokenSelector2(I) == token);

                if (count == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found in {2}{3}",
                            itemName, token, list2Description, Environment.NewLine);
                    ok = false;
                }
            }

            foreach (T2 item in list2)
            {
                string token = tokenSelector2(item);
                int count = list1.Count(I => tokenSelector1(I) == token);

                if (count == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found in {2}{3}",
                            itemName, token, list1Description, Environment.NewLine);
                    ok = false;
                }
            }

            return ok;
        }

        public static bool CheckTokensInclusion<T1, T2>(
            IEnumerable<T1> fullList,
            Func<T1, string> tokenSelector1,
            string fullListDescription,
            IEnumerable<T2> shortList,
            Func<T2, string> tokenSelector2,
            string itemName,
            StringBuilder logger)
        {
            bool ok = true;

            foreach (T2 item in shortList)
            {
                string token = tokenSelector2(item);
                int count = fullList.Count(I => tokenSelector1(I) == token);

                if (count == 0)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found in {2}{3}",
                            itemName, token, fullListDescription, Environment.NewLine);
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
