using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Data
{
    class Context
    {
        public ServicesEnvironment ServicesEnvironment { get; private set; }

        private Context()
        {
            ServicesEnvironment = new ServicesEnvironment();
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
