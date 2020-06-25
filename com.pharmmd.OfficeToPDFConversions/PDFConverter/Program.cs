using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelToPDF;
using WordToPDF;

namespace PDFConverter
{
    class Program
    {
        static public string FileName = "";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Out.WriteLine("No arguments given.");
                System.Environment.Exit(1);
            }
            else
            {
                int exitcode = 0;

                string inputfilename = args[0].ToString();
                FileName = inputfilename;

                if(FileName=="")
                {
                    exitcode = 1;
                    Console.Out.WriteLine("No File Name was given. Please try again.");
                    System.Environment.Exit(exitcode);
                }
                    
                if(!(System.IO.File.Exists(FileName)))
                {
                    exitcode = 1;
                    Console.Out.WriteLine("The input file (" + FileName + ") does not exist or you do not have access to thsi file.");
                    System.Environment.Exit(exitcode);
                }

                
                string extension = new System.IO.FileInfo(FileName).Extension;

                switch (extension.ToUpper())
                {
                    case ".DOC":
                    case ".DOCX":
                        Console.Out.WriteLine("Started at: " + DateTime.Now.ToString());
                        WordToPDF.MasterClass wmc = new WordToPDF.MasterClass();
                        wmc.WordFileName = FileName;
                        wmc.RunConverter();
                        Console.Out.WriteLine("The PDF File (" + wmc.OutputFileName + ") has been created.");
                        Console.Out.WriteLine("Ended at: " + DateTime.Now.ToString());
                        exitcode = 0;
                        break;
                    case ".XLS":
                    case ".XLSX":
                        Console.Out.WriteLine("Started at: " + DateTime.Now.ToString());
                        ExcelToPDF.MasterClass emc = new ExcelToPDF.MasterClass();
                        emc.ExcelFileName = FileName;
                        emc.RunConverter();
                        Console.Out.WriteLine("The PDF File (" + emc.OutputFileName + ") has been created.");
                        Console.Out.WriteLine("Ended at: " + DateTime.Now.ToString());
                        exitcode = 0;
                        break;
                    default:
                        exitcode = 1;
                        break;

                }

                if(exitcode==1)
                {
                    Console.Out.Write("The extension on the file (" + FileName + " is not .doc, .docx, .xls or .xlsx");
                }

                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("The application will exit in 5 minutes.");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");

                System.Threading.Thread.Sleep(5000);
                System.Environment.Exit(exitcode);
            }
        }
    }
}
