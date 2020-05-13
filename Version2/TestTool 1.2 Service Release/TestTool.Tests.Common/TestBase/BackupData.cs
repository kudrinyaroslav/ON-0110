///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;

namespace TestTool.Tests.Common.TestBase
{
    /// <summary>
    /// Dictionary-like object to store backup information during the test.
    /// </summary>
    public class BackupData 
    {
        private Dictionary<string, object> _data;

        public BackupData()
        {
            _data = new Dictionary<string, object>();
        }
        

        #region IDictionary<string,object> Members

        public void Add(string key, object value)
        {
            _data.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _data.Keys; }
        }

        public bool Remove(string key)
        {
            return _data.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _data.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _data.Values; }
        }

        public object this[string key]
        {
            get
            {
                return _data[key];
            }
            set
            {
                _data[key] = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Template of a method used to backup device information before the test. 
    /// </summary>
    /// <typeparam name="T">Type of the class/structure to be retrieved and then passed to 
    /// method for restoring.</typeparam>
    /// <returns>Data to be used to restore device state.</returns>
    public delegate T Backup<T>();

}
