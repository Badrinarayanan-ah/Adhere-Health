using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using USPSAddressVerifier.Lib;
//using PF.Lib;

namespace USPSAddressVerifier
{
    class Program
    {
        static int? pmdclientid = null;
        static int? pmdpatientid = null;
        static int? singlepmdpatientid = null;

        static List<int> lstMTMClients = new List<int> { 15, 30, 54, 62, 70 };

        static void SingleAddressMode()
        {
            Processor p = new Processor();
            p.SingleAddressMode();
        }

        static string HelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("To use this application, you will need the following:");
            sb.AppendLine(@"A folder on the C drive called C:\USPS");
            sb.AppendLine("A file called either testaddressfile.csv or testaccountaddressfile.csv  ");
            sb.AppendLine("The Test Address File should have the following format (pipe-delimited)  ");
            sb.AppendLine("     Address1|Address2|City|State|Zip|Zip4");
            sb.AppendLine("The Test Account Address File should have the following format (pipe-delimited)");
            sb.AppendLine("     AccountID|Address1|Address2|City|State|Zip|Zip4");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("You can enter a single ClientID to use with this parameter syntax: /pmdclientid:45");
            sb.AppendLine("or use -1 to run all MTM clients");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("You can enter a single PMDPatientID through with this syntax: /pmdpatientid:11112223");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("MODES:1 - Single Address, 2 - From File, 3 From Database, 4 From Database (By PMDPatientID) ");
            sb.AppendLine("-------------");
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(HelpText());

                //filemode

                if (args.Length == 0)
                {
                    SingleAddressMode();
                }
                else
                {
                    string argumentValue = args[0];
                    string[] argumentValueArray = argumentValue.Split(new char[] { ':' });

                    foreach (string s in argumentValueArray)
                    {
                        if (s.Trim().ToLower().StartsWith("/pmdclientid:") || s.Trim().ToLower().StartsWith(@"\pmdclientid:"))
                        {
                            pmdclientid = Convert.ToInt32(s.Trim().ToLower().Replace("/pmdclientid:", "").Replace(@"\pmdclientid:", "").Trim());
                        }
                        if (s.Trim().ToLower().StartsWith("/pmdpatientid:") || s.Trim().ToLower().StartsWith(@"\pmdpatientid:"))
                        {
                            singlepmdpatientid = Convert.ToInt32(s.Trim().ToLower().Replace("/pmdpatientid:", "").Replace(@"\pmdpatientid:", "").Trim());
                        }
                    }

                    foreach (string s in args)
                    {
                        if (s.Trim().ToLower().StartsWith("/pmdclientid:") || s.Trim().ToLower().StartsWith(@"\pmdclientid:"))
                        {
                            pmdclientid = Convert.ToInt32(s.Trim().ToLower().Replace("/pmdclientid:", "").Replace(@"\pmdclientid:", "").Trim());
                        }
                        else if (s.Trim().ToLower().StartsWith("/pmdpatientid:") || s.Trim().ToLower().StartsWith(@"\pmdpatientid:"))
                        {
                            singlepmdpatientid = Convert.ToInt32(s.Trim().ToLower().Replace("/pmdpatientid:", "").Replace(@"\pmdpatientid:", "").Trim());
                        }
                    }

                    string usageValue = "";

                    Processor p = new Processor();
                    p.ValidateParameters(usageValue, argumentValueArray);
                    usageValue = p.usageValue;

                    if (p.usageValueError)
                    {
                        Console.Out.WriteLine(p.AddressCheckOutput);
                    }
                    else
                    {
                        switch (usageValue)
                        {
                            case "1":
                                Processor p1 = new Processor();
                                p1.SingleAddressMode();

                                Console.Out.WriteLine(p.AddressCheckOutput);
                                break;

                            case "2":
                                Processor p2 = new Processor();
                                p2.FileAddressMode();

                                Console.Out.WriteLine(p2.AddressCheckOutput);
                                break;
                            case "3":
                                Processor p3 = new Processor();

                                //pmdpatientid = 107488344;
                                pmdpatientid = singlepmdpatientid;

                                if (pmdclientid == -1)
                                {
                                    foreach (int i in lstMTMClients)
                                    {
                                        pmdclientid = i;
                                        p3.DatabaseLooper(pmdclientid, pmdpatientid);
                                    }
                                }
                                else
                                {
                                    p3.DatabaseLooper(pmdclientid, pmdpatientid);
                                }

                                Console.Out.WriteLine(p3.AddressCheckOutput);
                                break;
                            case "4":
                                Processor p4 = new Processor();

                                p4.DatabaseLooper(null, pmdpatientid);

                                Console.Out.WriteLine(p4.AddressCheckOutput);
                                break;
                            default:
                                Console.Out.WriteLine(p.AddressCheckOutput);
                                break;
                        }
                    }

                }


#if DEBUG
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");

                /*
                Console.Write("Press any key to exit....");

                Console.ReadKey();

                while (Console.KeyAvailable)
                {
                    System.Environment.Exit(0);
                }
                */

                Console.Out.WriteLine("The application will exit in 15 seconds...");
                System.Threading.Thread.Sleep(15000);

                System.Environment.Exit(0);
#else
                Console.Out.WriteLine("The application will exit in 15 seconds...");
                System.Threading.Thread.Sleep(15000);

                System.Environment.Exit(0);
#endif

            }
            catch (Exception ex)
            {

                Console.Out.WriteLine("");
                Console.Out.WriteLine("An unexpected error occurred. Please check the above help text and this error message: " + ex.Message + "-" + ex.StackTrace);
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("The application will exit in 5 seconds...");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");

                System.Threading.Thread.Sleep(5000);

                System.Environment.Exit(1);
            }
        }
    }
}
