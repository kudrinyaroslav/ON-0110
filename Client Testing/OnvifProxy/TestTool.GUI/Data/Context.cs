using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Data
{
    class Context
    {
        public Environment Environment { get; private set; }
        public Credentials Credentials { get; private set; }

        private Context()
        {
            Environment = new Environment();
            Credentials = new Credentials();
        }

        private static Context _instance;

        public static Context Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new Context();
                }

                return _instance;
            }
        }

    }
}
