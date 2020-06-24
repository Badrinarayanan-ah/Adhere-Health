using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileLastWriteTimeChecker
{
    public class Arguments
    {
        public string FileFilter = "*.*";
        public string FileFolder = "";
        public bool HasError = false;
        public string DaysSinceLastFile = "10";

        public void ProcessArguments(string[] args)
        {
            if (args.GetLength(0) > 0)
            {
                for (int i = 0; i < args.GetLength(0); i++)
                {
                    int substring = args[i].IndexOf(':');

                    if (substring == -1)
                    {
                        substring = args[i].Length;
                    }

                    switch (args[i].Substring(0, substring).Trim().ToLower())
                    {
                        case "/?":
                            Console.WriteLine(HelpText());
                            HasError = true;
                            break;
                        case "/directoryname":
                            FileFolder = args[i].Replace(@"/directoryname:", String.Empty);
                            break;
                        case "/filefilter":
                            FileFilter = args[i].Replace(@"/filefilter:", String.Empty);
                            break;
                        case "/dayssincelastfile":
                            DaysSinceLastFile = args[i].Replace(@"/dayssincelastfile:", String.Empty);
                            break;
                        default:
                            Console.WriteLine("You entered an incorrect argument.");
                            HasError = true;
                            break;
                    }
                }

                if (!(System.IO.Directory.Exists(FileFolder)))
                {
                    Console.WriteLine("The directory you entered does not exist or you do not have access to it.");
                    HasError = true;
                }

                int outnew;

                if(!(Int32.TryParse(DaysSinceLastFile, out outnew)))
                {
                    Console.WriteLine("The Days Since Last File value you entered is non-numeric");
                    HasError = true;
                }
                
            }
            else
            {
                HasError = true;
                Console.WriteLine("You did not enter any arguments");
                Console.WriteLine(HelpText());
            }
        }
        


        public string HelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage: FileLastWriteTimeChecker.exe /directoryname:directoryname [/texttoremove:texttoremove]");
            sb.AppendLine("");
            sb.AppendLine("/?\t\t\tDisplays this message.");
            sb.AppendLine("DirectoryName\t\t The directory that should be analyzed");
            sb.AppendLine("FileFilter\t\t If nothing is entered, all files will be checked (*.*). You can check for text files with .txt, Excel files with .xls or .xlsx, etc)");
            sb.AppendLine("DaysSinceLastFile\t\t The date range to search for files for - the default is 10");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
