using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesFindPersonAPI
{
    public class Arguments
    {
        public string inputPMDClientID = "";
        public bool Errored = false;
        public string inputPMDMode = "1";
        public string inputType = "0";

        public string HelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage: WhitePagesFindPersonAPI.exe /pmdclientid:pmdclientid [mode:mode]");
            sb.AppendLine("");
            sb.AppendLine("/?\t\t\tDisplays this message.");
            sb.AppendLine("PmdClientID\t\t The PMDClientID from the Client table on SQLETL/ETLCORE for the client we want to process");
            sb.AppendLine("Mode\t\t The Can be blank. Mode 1 is the default in pulling from the DB. Mode 2 is pulling from a text file");
            sb.AppendLine();

            return sb.ToString();
        }

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
                            Errored = true;
                            break;
                        case "/pmdclientid":
                            inputPMDClientID = args[i].ToLower().Replace(@"/pmdclientid:", String.Empty);
                            break;
                        case "/mode":
                            inputPMDMode = args[i].ToLower().Replace(@"/mode:", String.Empty);
                            break;
                        case "/inputType":
                        case "/type":
                            inputType = args[i].ToLower().Replace(@"/type:", String.Empty).Replace(@"/inputtype:",string.Empty);
                            break;
                        default:
                            Console.WriteLine("You entered an incorrect argument.");
                            Console.WriteLine(HelpText());
                            Errored = true;
                            break;
                    }
                }

                if (inputPMDClientID == "")
                {
                    Console.WriteLine("You must enter a PMDClientID parameter");
                    Errored = true;
                }

                int outtest;

                if (!(Int32.TryParse(inputPMDClientID, out outtest)))
                {
                    Console.WriteLine("You must enter a numeric PMDClientID parameter");
                    Errored = true;
                }

                if (!(Int32.TryParse(inputPMDMode, out outtest)))
                {
                    inputPMDMode = "1";
                }

                if (!(Int32.TryParse(inputType, out outtest)))
                {
                    inputType = "0";
                }
            }
            else
            {
                Errored = true;
                Console.WriteLine("You did not enter any arguments");
                Console.WriteLine(HelpText());
            }

        }
    }
}
