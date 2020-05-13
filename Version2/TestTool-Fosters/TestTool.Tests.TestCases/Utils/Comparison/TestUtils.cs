using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TestTool.Tests.CommonUtils.Comparison
{
    /// <summary>
    /// Common utilities
    /// </summary>
    public static class TestUtils
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
                }
            }
            return tokensOk;
        }



        /// <summary>
        /// Check that both field are not null
        /// </summary>
        /// <param name="equal">True, if bith are null or noth are not null.</param>
        /// <param name="fieldName">Field namd</param>
        /// <param name="descr1">Description of first structure</param>
        /// <param name="descr2">Description of second structure</param>
        /// <param name="value1">Value of field in first structure</param>
        /// <param name="value2">Value of field in second structure</param>
        /// <param name="dump">StringBuilder to add log.</param>
        /// <returns></returns>
        public static bool BothNotNull(out bool equal,
            string fieldName,
            string descr1, string descr2,
            object value1, object value2,
            StringBuilder dump)
        {
            bool ret = false;
            if (value1 == null || value2 == null)
            {
                if (value1 == null && value2 == null)
                {
                    equal = true;
                }
                else
                {
                    equal = false;
                    if (value1 == null)
                    {
                        dump.AppendLine(string.Format("{0} field is empty in the structure {1}", fieldName, descr1));
                    }
                    else
                    {
                        dump.AppendLine(string.Format("{0} field is empty in the structure {1}", fieldName, descr2));
                    }
                }
            }
            else
            {
                equal = true;
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Class for holding validation task data (for boolean field validation). 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class CheckSettings<T>
        {
            /// <summary>
            /// Name of the field.
            /// </summary>
            public string FieldName { get; set; }
            /// <summary>
            /// Function to get value of the field
            /// </summary>
            public Func<T, bool> ValueSelector { get; set; }
            /// <summary>
            /// Function to check that value has been specified.
            /// </summary>
            public Func<T, bool> SpecifiedSelector { get; set; }
        }

        /// <summary>
        /// Checks Boolean fields for all fields in a list.
        /// </summary>
        /// <typeparam name="T">Structure type.</typeparam>
        /// <param name="s1">First structure</param>
        /// <param name="s2">Second structure</param>
        /// <param name="descr1">First structure description</param>
        /// <param name="descr2">Second structure description</param>
        /// <param name="batch">List of check settings.</param>
        /// <param name="structureName">Name of the structure</param>
        /// <param name="dump">Log</param>
        /// <returns></returns>
        public static bool BatchCheckBoolean<T>(T s1, T s2,
            string descr1, string descr2,
            List<CheckSettings<T>> batch, string structureName, StringBuilder dump)
        {
            bool equal = true;

            foreach (CheckSettings<T> settings in batch)
            {
                bool local = CheckBooleanField(s1, s2, settings.ValueSelector, settings.SpecifiedSelector,
                    string.IsNullOrEmpty(structureName) ? settings.FieldName : string.Format("{0}.{1}", structureName, settings.FieldName), 
                    descr1, descr2, dump);

                equal = equal && local;
            }

            return equal;
        }
        
        /// <summary>
        /// Checks boolean field (using selectors).
        /// </summary>
        /// <typeparam name="T">Structure type</typeparam>
        /// <param name="s1">First structure</param>
        /// <param name="s2">Second structure</param>
        /// <param name="valueSelector">Value selector</param>
        /// <param name="specifiedSelector">Selector for corresponding "specified" field.</param>
        /// <param name="name">Field name</param>
        /// <param name="descr1">First structure description.</param>
        /// <param name="descr2">Second structure description.</param>
        /// <param name="dump">Log</param>
        /// <returns></returns>
        public static bool CheckBooleanField<T>(T s1, T s2,
            Func<T, bool> valueSelector, Func<T, bool> specifiedSelector,
            string name, string descr1, string descr2, StringBuilder dump)
        {
            return CheckField(specifiedSelector(s1), valueSelector(s1),
                specifiedSelector(s2), valueSelector(s2),
                name, descr1, descr2, dump);
        }

        /// <summary>
        /// Checks boolean field using values.
        /// </summary>
        /// <param name="specified1">True, if first value is specified</param>
        /// <param name="field1">First value</param>
        /// <param name="specified2">True, if second value is specified</param>
        /// <param name="field2">Second value</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="descr1">First structure description</param>
        /// <param name="descr2">Second structure description</param>
        /// <param name="dump">Log</param>
        /// <returns></returns>
        public static bool CheckField(bool specified1, bool field1,
            bool specified2, bool field2,
            string fieldName, string descr1, string descr2, StringBuilder dump)
        {
            bool ok = false;
            // both bot specified is ok;
            if (specified1 || specified2)
            {
                if (specified1 && specified2)
                {
                    if (field1 != field2)
                    {
                        dump.AppendLine(string.Format("{0} field does not match ({1}: {2}, {3}: {4})", fieldName, descr1, field1, descr2, field2));
                    }
                    else
                    {
                        ok = true;
                    }
                }
                else
                {
                    if (!specified1)
                    {
                        if (!field2)
                        {
                            ok = true;
                        }
                        else
                        {
                            dump.AppendLine(string.Format("{0} field does not match ({1}: not specified, {2}: True)", 
                                fieldName, descr1, descr2));
                        }
                    }
                    if (!specified2)
                    {
                        if (!field1)
                        {
                            ok = true;
                        }
                        else
                        {
                            dump.AppendLine(string.Format("{0} field does not match ({1}: True, {2}: not specified)",
                                fieldName, descr1, descr2));
                        }
                    }
                }
            }
            else
            {
                ok = true;
            }
            return ok;
        }

        /// <summary>
        /// Checks int field.
        /// </summary>
        /// <typeparam name="T">Structure type</typeparam>
        /// <param name="s1">First structure</param>
        /// <param name="s2">Second structure</param>
        /// <param name="valueSelector">Value selector</param>
        /// <param name="specifiedSelector">Selector for corresponding "specified" field.</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="descr1">First structure description.</param>
        /// <param name="descr2">Second structure description.</param>
        /// <param name="dump">Log</param>
        /// <returns></returns>
        public static bool CheckIntField<T>(T s1, T s2,
            Func<T, long> valueSelector, Func<T, bool> specifiedSelector,
            string fieldName, string descr1, string descr2, StringBuilder dump)
        {
            bool ok = false;
            // both bot specified is ok;
            if (specifiedSelector(s1) || specifiedSelector(s2))
            {
                if (specifiedSelector(s1) && specifiedSelector(s2))
                {
                    if (valueSelector(s1) != valueSelector(s2))
                    {
                        dump.AppendLine(string.Format("{0} field is different", fieldName));
                    }
                    else
                    {
                        ok = true;
                    }
                }
                else
                {
                    if (!specifiedSelector(s1))
                    {
                        dump.AppendLine(string.Format("{0} field is not specified in the structure {1}", fieldName, descr1));
                    }
                    if (!specifiedSelector(s2))
                    {
                        dump.AppendLine(string.Format("{0} field is not specified in the structure {1}", fieldName, descr2));
                    }
                }
            }
            else
            {
                ok = true;
            }
            return ok;
        }

    }
}
