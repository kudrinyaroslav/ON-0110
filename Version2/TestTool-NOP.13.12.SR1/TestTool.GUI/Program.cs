using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TestTool.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                System.Globalization.CultureInfo.InvariantCulture;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0 || args.Contains("-v"))
            {
                Application.Run(new MainForm());
            }
            else
            {
                CompactProcessingForm emptyForm = new CompactProcessingForm();
                emptyForm.ShowInTaskbar = false;
                System.Threading.Thread thread =
                    new Thread(new ThreadStart(new Action(() =>
                    {
                        new SilentProcessingController(emptyForm).Run(args);
                        if (emptyForm.Created)
                        {
                            emptyForm.Invoke(new Action(emptyForm.Close));
                        }
                    }) ));
                emptyForm.Shown +=
                     (s, e) => { thread.Start();  };

                Application.Run(emptyForm);
            }
        }
        
    }
}
