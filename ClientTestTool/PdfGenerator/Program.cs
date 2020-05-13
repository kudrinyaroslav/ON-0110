using System;
using System.IO;

namespace PdfGenerator
{
  class Program
  {
    static void Main(string[] args)
    {
      foreach (var foFile in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.fo"))
      {
        Console.WriteLine("Processing {0}", Path.GetFileName(foFile));

        DoCGenerator generator = new DoCGenerator(foFile);
        generator.Generate(null, String.Format("{0}_{1}.pdf", Path.GetFileNameWithoutExtension(foFile), DateTime.Now.ToLongTimeString().Replace(':', '_')));

        Console.WriteLine("Completed");
      }

      Console.Read();

    }
  }
}
