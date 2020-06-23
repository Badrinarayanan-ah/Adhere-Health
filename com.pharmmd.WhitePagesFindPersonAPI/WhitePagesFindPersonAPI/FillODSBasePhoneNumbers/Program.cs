using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;

namespace FillODSBasePhoneNumbers
{
    class Program
    {
        //static List<int> lstMTMClients = new List<int> { 15, 30, 54, 62, 70 };
        //removing Healthspring (April 2018 - BDW)
        static List<int> lstMTMClients = new List<int> { 15, 54, 62, 70, 79 };

        static void Main(string[] args)
        {
            SQLDataActions sda = new SQLDataActions();
            //DataTable dt = sda.GetAllClients();

            //overwrite the client list from the whitepages table;
            //BW 10/24/2018
            DataTable dt = sda.GetWhitePagesClients();

            lstMTMClients.Clear();

            foreach (DataRow row in dt.Rows)
            {
                lstMTMClients.Add(Convert.ToInt32(row["clientid"]));
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Arguments a = new Arguments();
            a.ProcessArguments(args);

            if (a.Errored == false)
            {
                SQLDataActions sqlDA = new SQLDataActions();

                if (a.inputPMDClientID == "-1")
                {
                    foreach (int i in lstMTMClients)
                    {
                        int insertcount = sqlDA.FillODSBasePhoneNumbersFromWhitePages(i);

                        Console.Out.WriteLine(insertcount + " record(s) inserted from WhitePages API for PMDClientID: " + i);
                    }

                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("The application will exit in 5 seconds...");
                    System.Threading.Thread.Sleep(5000);
                    Console.Out.WriteLine("");
                }
                else
                {
                    int insertcount = sqlDA.FillODSBasePhoneNumbersFromWhitePages(Convert.ToInt32(a.inputPMDClientID));

                    Console.Out.WriteLine(insertcount + " record(s) inserted from WhitePages API for PMDClientID: " + a.inputPMDClientID);

                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("The application will exit in 5 seconds...");
                    System.Threading.Thread.Sleep(5000);
                    Console.Out.WriteLine("");
                }

                System.Environment.Exit(0);
            }
            else
            {
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");

#if DEBUG
                Console.Out.WriteLine("Press Enter to exit");
                string s = Console.ReadLine();
#endif

                //string s = Console.ReadKey();


                System.Environment.Exit(1);
            }
        }
    }
}
