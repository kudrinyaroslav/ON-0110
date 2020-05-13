using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMC.Pages
{
    public class BaseSmcControl : UserControl
    {
        public BaseSmcControl()
        {
            SmcContext.General.Modified += new EventHandler(General_Modified);
        }


        internal  SmcData.Context SmcContext
        {
            get
            {
                return SmcData.Context.Instance;
            }
        }
        
        private System.ServiceModel.BasicHttpBinding _serviceBinding;
        protected System.ServiceModel.BasicHttpBinding GetBinding()
        {
            if (_serviceBinding == null)
            {
                _serviceBinding = new System.ServiceModel.BasicHttpBinding();
            }

            return _serviceBinding;   
        }

        protected void SafeInvoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        protected void ShowError(Exception exc)
        {
            Invoke( new Action(() => { MessageBox.Show(exc.Message);}));
        }
    

        void General_Modified(object sender, EventArgs e)
        {
            UpdateAddress();
        }

        protected virtual void UpdateAddress()
        {

        }

        public delegate string MyDelegate<T>(int? limit, string reference, out T[] output);

        public List<T> GetList<T>(MyDelegate<T> myFunc)
        {
            List<T> list = new List<T>();
            T[] tempArray;
            string temp = null;
            do
            {
                temp = myFunc(null, temp, out tempArray);
                foreach (var tempItem in tempArray)
                {
                    list.Add(tempItem);
                }

            } while (!string.IsNullOrEmpty(temp));

            return list;
        }
    }
}
