using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTool.Tests.Common.CommonUtils
{
    public class TabulatedStringBuilder
    {
        private StringBuilder m_StringBuilder = new StringBuilder();
        private int m_TabSize;
        public int TabStep { get; protected set; }
        public int StepCount { get { return m_TabSize/TabStep; } }

        protected string GetPrefix()
        {
            return new string(' ', m_TabSize);
        }

        public TabulatedStringBuilder(int tabStep = 4)
        { m_TabSize = 0; TabStep = tabStep; }

        public TabulatedStringBuilder(int tabStep, int tabCount)
        { m_TabSize = tabStep*tabCount; TabStep = tabStep; }

        public void IncreaseTabSize()
        {
            m_TabSize += TabStep;
        }

        public void DecreaseTabSize()
        {
            if (0 != m_TabSize)
                m_TabSize -= TabStep;
        }

        public void AppendLine(string s)
        {
            m_StringBuilder.Append(GetPrefix());
            m_StringBuilder.AppendLine(s);
        }

        public void AppendFormat(String format, params Object[] args)
        {
            var s = string.Format(format, args);

            var lines = s.Split(new [] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                m_StringBuilder.Append(GetPrefix());
                m_StringBuilder.AppendLine(line);
            }
        }

        public void Append(TabulatedStringBuilder sb)
        {
            m_StringBuilder.AppendLine(sb.ToString());
        }

        public void Clear()
        {
            m_StringBuilder.Clear();
        }

        public new string ToString()
        {
            return m_StringBuilder.ToString();
        }

        public string ToStringTrimNewLine()
        {
            return ToString().TrimEnd();
        }
    }
}
