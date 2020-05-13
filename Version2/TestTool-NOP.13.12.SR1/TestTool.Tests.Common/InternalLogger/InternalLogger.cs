using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTool.Tests.Common.InternalLogger
{
    public class InternalLogger: IDisposable
    {
        #region Singleton implementation

        private static InternalLogger m_Instance;
        public static InternalLogger GetInstance()
        {
           if (null == m_Instance) 
               m_Instance = new InternalLogger();

            return m_Instance;
        }

        #endregion

        private InternalLogger()
        {}

        ~InternalLogger()
        {
            Dispose();
        }

        public void SwitchOffFor(int threadID)
        {
            lock (m_NotActiveFor)
            {
                if (LogOffForCurrentThread)
                    return;

                m_NotActiveFor.Add(Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void SwitchOffForCurrentThread()
        {
            SwitchOffFor(Thread.CurrentThread.ManagedThreadId);
        }

        public void SwitchOnFor(int threadID)
        {
            lock (m_NotActiveFor)
            {
                if (LogOffForCurrentThread)
                    m_NotActiveFor.Remove(Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void SwitchOnForCurrentThread()
        {
            SwitchOnFor(Thread.CurrentThread.ManagedThreadId);
        }

        public void LogMessage(string msg)
        {
            lock (m_NotActiveFor)
            {
                if (!LogOffForCurrentThread)
                    lock (m_Log)
                    { m_Log.AppendLine(msg); }
            }
        }

        private void Flush()
        {
            lock (m_NotActiveFor)
            {
                if (m_Log.ToString().Any() && EnsureStorageExists())
                {
                    var writer = new StreamWriter(m_Out);
                    writer.Write(m_Log.ToString());
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private bool EnsureStorageExists()
        {
            if (null == m_Out)
            {
                try
                {
                    m_Out = new FileStream(StoragePath, FileMode.Create, FileAccess.Write);
                }
                catch (Exception)
                {
                    m_Out = null;
                    return false;
                }
            }
            return true;
        }

        public void Dispose()
        {
            Flush();
        }

        private string StoragePath
        {
            get
            {
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return directory + @"\Internal.log";
            }
        }

        private bool LogOffForCurrentThread
        {
            get { return m_NotActiveFor.Contains(Thread.CurrentThread.ManagedThreadId); }
        }

        private readonly HashSet<int> m_NotActiveFor = new HashSet<int>();
        private readonly StringBuilder m_Log = new StringBuilder();
        private FileStream m_Out;
    }
}
