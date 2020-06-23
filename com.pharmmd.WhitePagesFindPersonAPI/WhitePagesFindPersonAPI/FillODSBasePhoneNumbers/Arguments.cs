using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FillODSBasePhoneNumbers
{
    class Arguments
    {
        public string inputPMDClientID = "";
        public bool Errored = false;

        public string HelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage: FillODSBasePhoneNumbers.exe /pmdclientid:pmdclientid");
            sb.AppendLine("");
            sb.AppendLine("/?\t\t\tDisplays this message.");
            sb.AppendLine("PmdClientID\t\t The PMDClientID from the Client table on SQLETL/ETLCORE for the client we want to process");
            sb.AppendLine("-1 will run for all MTM Clients");
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
                            inputPMDClientID = args[i].Replace(@"/pmdclientid:", String.Empty);
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
