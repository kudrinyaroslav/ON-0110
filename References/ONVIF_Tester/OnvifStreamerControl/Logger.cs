/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Onvif
{
    public class Logger
    {

        public enum LogLevels {None,Error,Info,Debug};

        private const string _logFolderBase = "LogFiles";
        private const string _logNameBase = "OnvifStreamerControl";

        private const int _logFileCount = 5;
        private static object _logSync = new object();
        private static LogLevels _level = LogLevels.Info;
        private static ArrayList _logOnceList = new ArrayList();

        private static string _filename;

        public static LogLevels LogLevel
        {
            get { return _level; }
            set { _level = value; }
        }

        internal class Debug
        {
            internal static void Write(string s)
            {
                if (_level >= LogLevels.Debug) Logger.Write(s);

                System.Diagnostics.Debug.Write(s);

            }
            internal static void WriteLine(string s)
            {
                if (_level >= LogLevels.Debug) Logger.WriteLine(s);

                System.Diagnostics.Debug.WriteLine(s);
            }
        }
        internal class Info
        {
            internal static void Write(string s)
            {
                if (_level >= LogLevels.Info) Logger.Write(s);

                System.Diagnostics.Debug.Write(s);

            }
            internal static void WriteLine(string s)
            {
                if (_level >= LogLevels.Info) Logger.WriteLine(s);

                System.Diagnostics.Debug.WriteLine(s);
            }
            internal static void WriteOnce(string s)
            {
                string check = "Error " + s;

                lock (_logOnceList.SyncRoot)
                {
                    if (_logOnceList.Contains(check)) return;
                }

                if (_level >= LogLevels.Info) Logger.WriteLine(s);

                System.Diagnostics.Debug.WriteLine(s);

                lock (_logOnceList.SyncRoot)
                {
                    _logOnceList.Add(check);
                }

            }
        }
        internal class Error
        {
            internal static void Write(string s)
            {
                if (_level >= LogLevels.Error) Logger.Write(s);

                System.Diagnostics.Debug.Write(s);
            }
            internal static void WriteLine(string s)
            {
                if (_level >= LogLevels.Error) Logger.WriteLine(s);

                System.Diagnostics.Debug.WriteLine(s);

            }

            internal static void WriteOnce(string s)
            {
                string check = "Error " + s;

                lock (_logOnceList.SyncRoot)
                {
                    if (_logOnceList.Contains(check)) return;
                }

                //add code to check if we wrote this string from the calling class already once
                if (_level >= LogLevels.Info) Logger.WriteLine(s);

                System.Diagnostics.Debug.WriteLine(s);

                lock (_logOnceList.SyncRoot)
                {
                    _logOnceList.Add(check);
                }

            }

        }


        private static void WriteLine(string s)
        {

            if (!s.EndsWith("\r\n")) s += "\r\n";

            Write(s);
        }

        private static void RenameLogFiles()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.System) + string.Format("\\{0}\\{1}\\{2}.", _logFolderBase, _logNameBase, _logNameBase);
            FileInfo fi1 = new FileInfo(path + (_logFileCount - 1).ToString("0#") + ".log");
            if (fi1.Exists) fi1.Delete();

            for (int i = (_logFileCount - 2); i >= 0; i--)
            {
                fi1 = new FileInfo(path+i.ToString("0#")+".log");
                if (fi1.Exists) fi1.MoveTo(path+(i+1).ToString("0#")+".log");
            }

        }

        private static void Write(string s)
        {
            lock (_logSync)
            {
                try
                {
                    if (_filename == null)
                    {
                        RenameLogFiles();

                        _filename = Environment.GetFolderPath(Environment.SpecialFolder.System) + string.Format("\\{0}\\{1}", _logFolderBase, _logNameBase);
                        if (!Directory.Exists(_filename)) Directory.CreateDirectory(_filename);
                        _filename += string.Format("\\{0}.00.log", _logNameBase);
                    }


                    StreamWriter sw = new StreamWriter(_filename, true);

                    string lm = string.Format("[{0}] {1} {2}", Thread.CurrentThread.ManagedThreadId.ToString("X2"), DateTime.Now.ToString("MM/dd/yy HH:mm:ss.fff"), s);
                    sw.Write(lm);
                    sw.Close();
                }
                catch (Exception e)
                {
                    String strError = "Error writing log message to file: " + e.ToString();
                    System.Diagnostics.Debug.WriteLine(strError);
                }
            }
        }
    }
}
