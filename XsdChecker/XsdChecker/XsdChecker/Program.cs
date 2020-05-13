using System;
using System.Linq;
using XsdCheckerLib;


namespace XsdChecker
{
    class Program
    {
        private static void PrintHelpMessage() 
        {
            string[] helpMessage = new[] {
                "\nUSAGE:",
                "\n\tXsdChecker.exe [/? | (filename | directorypath)]",
                "\nOptions:",
                "\n\t/?\t\tDisplay this help message",
                "\n\tfilename\tSpecifies a file for XsdChecker to process.",
                "\n\tdirectorypath\tSpecifies a path to a folder contains XSD and WSDL files to be validated.",
                "\nExamples:",
                "\n\t> XsdChecker.exe schema.xsd\tValidates schema.xsd file",
                "\n\t> XsdChecker.exe ..\\schemas\tValidates all XSD and WSDL files in the \"schemas\" folder",
            };

            foreach (string line in helpMessage)
            {
                Console.WriteLine(line);
            }
        }

        private static int ProcessFiles(string filePath)
        {
            try
            {
                XsdCheckResult[] results = XsdCheckerProcessor.CheckXsdFilesMinOccursAtrribute(filePath);
                PrintResults(results);
                return results.Aggregate(0, (sum, res) => sum + res.ErrorCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private static void PrintResults(XsdCheckResult[] results)
        {
            foreach(var res in results.Where(result => result.ErrorCount > 0))
            {
                Console.WriteLine("FILE: " + res.FilePath);
                Console.WriteLine();
                Console.WriteLine("ERRORS: " + res.ErrorCount);
                Console.WriteLine();
                Console.WriteLine(res.ErrorMessage);
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();
            }
        }

        static int Main(string[] args)
        {
            int res = -1;
            if (args.Length == 1)
            {
                if (args[0]=="/?")
                {
                    PrintHelpMessage();                    
                }                
                else
                {
                    res = ProcessFiles(args[0]);
                }                
            }
            else 
            {
                Console.WriteLine("Incorrect filename");
                PrintHelpMessage();
            }

            return res;
        }
    }
}
