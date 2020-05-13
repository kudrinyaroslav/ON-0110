using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatedTesting.GUI.Context
{
    internal class AppContext
    {
        private AppContext()
        {
            _treeState = new TreeState();
        }

        private static AppContext _instance;

        public static AppContext Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new AppContext();
                }
                return _instance;
            }
        }
        
        private TreeState _treeState;
        
        public TreeState TreeState 
        {
            get
            {
                return _treeState;
            }
        }

        public void AttachTreeState(TreeState state)
        {
            _treeState = state;
        }

    }
}
