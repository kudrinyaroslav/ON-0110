using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchitectureSample.Data
{
    static class GlobalOptions
    {
        static FirstTabOptions _firstTabOptions = new FirstTabOptions();

        public static FirstTabOptions FirstTabOptions
        {
            get { return _firstTabOptions; }
        }

        public static void UpdateFirstTabOptions(FirstTabOptions options)
        {
            _firstTabOptions = options;
        }

        private static SecondTabOptions _secondTabOptions = new SecondTabOptions();

        public static SecondTabOptions SecondTabOptions
        {
            get { return _secondTabOptions; }
        }

        public static void UpdateSecondTabOptions(SecondTabOptions options)
        {
            _secondTabOptions = options;
        }
    }
}
