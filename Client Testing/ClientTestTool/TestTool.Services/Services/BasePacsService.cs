using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.Common.Configuration;

namespace TestTool.Services
{
    public abstract class BasePacsService : BaseService
    {

        protected T GetInfo<T>(string token, Func<SimulatorConfiguration, Dictionary<string, T>> listSelector)
        {
            T res = default(T);

            Dictionary<string, T> list = listSelector(SimulatorConfiguration);

            if (list.ContainsKey(token))
            {
                res = list[token];
            }
            else
            {
                string message = string.Format("Token {0} does not exist", token);
                Transport.CommonUtils.ReturnFault("Sender", "InvalidArg", "TokenNotFound");
            }

            return res;
        }

        protected T GetInfo<T>(string token,
            Func<T, string> tokenSelector,
            Func<SimulatorConfiguration, IEnumerable<T> > listSelector)
        {
            T res;

            IEnumerable<T> list = listSelector(SimulatorConfiguration);
            res = list.FirstOrDefault(I => tokenSelector(I) == token);

            if (res == null)
            {
                string message = string.Format("Token {0} does not exist", token);
                Transport.CommonUtils.ReturnFault("Sender", "InvalidArg", "TokenNotFound");
            }

            return res;
        }

        protected T[] GetListByTokenList<T>(string[] tokens,
            Func<T, string> tokenSelector,
            Func<SimulatorConfiguration, IEnumerable<T>> listSelector)
        {
            List<T> res = new List<T>(); 

            List<T> tempRes = new List<T>(listSelector(SimulatorConfiguration));

            if ((tokens != null) && (tokens.Count() != 0))
            {
                foreach (string token in tokens)
                {
                    T info = tempRes.FirstOrDefault(I => tokenSelector(I) == token);

                    if (info != null)
                    {
                        if (!res.Contains(info))
                        {
                            res.Add(info);
                        }
                    }
                    else
                    {
                        string message = string.Format("Token {0} does not exist", token);
                        Transport.CommonUtils.ReturnFault("Sender", "InvalidArg", "TokenNotFound");
                    }
                }
            }
            else
            {
                res = tempRes;
            }

            return res.ToArray();
        }

        protected T[] GetList<T>(int offset, bool offsetSpecified,
            int limit, bool limitSpecified,
            Func<T, string> tokenSelector,
            Func<SimulatorConfiguration, IEnumerable<T>> listSelector)
        {
            List<T> res = new List<T>(listSelector(SimulatorConfiguration));

            if (offsetSpecified)
            {
                res = res.Skip(offset).ToList();
            }

            if (limitSpecified)
            {
                res = res.Take(limit).ToList();
            }


            return res.ToArray();
        }


    }
}
