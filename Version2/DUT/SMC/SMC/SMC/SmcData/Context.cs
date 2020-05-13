using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.SmcData
{
    internal class Context
    {
        private Context()
        {
            _general = new General();
            _doorsControl = new DoorsControl();
        }

        private static Context _instance;

        public static Context Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Context();
                }

                return _instance;
            }
        }

        private General _general;

        public General General
        {
            get { return _general; }
        }

        private DoorsControl _doorsControl;

        public DoorsControl DoorsControl
        {
            get { return _doorsControl; }
        }
    }
}
