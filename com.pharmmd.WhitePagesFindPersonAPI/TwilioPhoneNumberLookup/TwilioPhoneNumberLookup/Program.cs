using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Lookups.V1;
using Twilio.Types;

namespace TwilioPhoneNumberLookup
{
    class Program
    {
        static int totalcount = 0;
        static int mobilecount = 0;
        static int badnumbercount = 0;
        static int invalidnumbercount = 0;

        static List<string> phoneNumberList = new List<string>();
        static List<PhoneNumberClass> lstReturnClass = new List<PhoneNumberClass>();

        static void Main(string[] args)
        {
            string accountSid = "AC8c739ae32c0c508876745f04379bf91f";
            string authToken = "your_auth_token";

            //mysection
            accountSid = "AC8c739ae32c0c508876745f04379bf91f";
            authToken = "6078ad0727713f2cc10075aa21e0982c";

            TwilioClient.Init(accountSid, authToken);

            //manual from static
            //var phoneNumber = PhoneNumberResource.Fetch(new PhoneNumber("+15108675309"), "US", new List<string> { "carrier" });

            /*
                       var phoneNumber1 = PhoneNumberResource.Fetch(new PhoneNumber("(615) 364-6365"), "US");

                       Console.WriteLine(phoneNumber1.PhoneNumber + "-" + phoneNumber.Carrier);

                       var phoneNumber2 = PhoneNumberResource.Fetch(new PhoneNumber("(901) 364-6365"), "US");

                       Console.WriteLine(phoneNumber2.PhoneNumber + "-" + phoneNumber.Carrier);
                       */

            BuildList();

            foreach (string pnc in phoneNumberList)
            {
                string looppnc = pnc.Trim();

                if (looppnc.Length > 0)
                {
                    looppnc = looppnc.Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace("-", "").Trim();


                    if (looppnc.Substring(0, 1) == "+")
                    {

                    }
                    else
                    {
                        looppnc = "+" + looppnc;
                    }

                    if (looppnc.Substring(1, 2) == "1")
                    {

                    }
                    else
                    {
                        looppnc = looppnc.Substring(0, 1) + "1" + looppnc.Substring(1, looppnc.Length - 1);
                    }
                }

                looppnc = looppnc.Trim();
                //looppnc = "+15108675309";

                string trimtoverify = looppnc.TrimStart('+').TrimStart('1');

                Match m = Regex.Match(trimtoverify, @"[\s\(]*((?!1)\d{1})((?!11)\d{2})[-\s\).,]*((?!1)\d{1})\d{2}[-\s.,]*\d{4}[\s]*");
                string pattern = "000%";

                bool ismatch = m.Success;

                if (!ismatch)
                {
                    lstReturnClass.Add(new PhoneNumberClass(looppnc, false, null));
                    invalidnumbercount++;
                }
                else
                {
                    if (looppnc.Length == 12)
                    {
                        if (looppnc == "+10000000000" || looppnc == "+11111111111" || looppnc == "+12222222222" || looppnc == "+13333333333" || looppnc == "+14444444444" || looppnc == "+15555555555" || looppnc == " + 16666666666" || looppnc == " +17777777777" || looppnc == "+18888888888" || looppnc == "+19999999999")
                        //|| looppnc.Contains("000%"))
                        {
                            var phoneNumber = PhoneNumberResource.Fetch(new PhoneNumber(looppnc), type: new List<string> { "caller_name", "caller-name", "carrier" });

                            string carriername = string.Empty;
                            string carriertype = String.Empty;


                            if (phoneNumber.Carrier != null)
                            {
                                carriername = phoneNumber.Carrier["name"];
                                carriertype = phoneNumber.Carrier["type"];
                            }

                            string callername = String.Empty;

                            if (phoneNumber.CallerName != null)
                            {
                                callername = phoneNumber.CallerName["caller_name"];
                            }
                            totalcount++;

                            //Console.WriteLine(phoneNumber.PhoneNumber + "-" + carriername + "-" + carriertype);

                            if (carriertype.ToLower() == "mobile")
                            {
                                lstReturnClass.Add(new PhoneNumberClass(phoneNumber.PhoneNumber.ToString(), true, true, callername));
                                mobilecount++;
                            }
                            else
                            {
                                lstReturnClass.Add(new PhoneNumberClass(phoneNumber.PhoneNumber.ToString(), true, false, callername));
                            }
                        }
                        else
                        {
                            lstReturnClass.Add(new PhoneNumberClass(looppnc, false, null));
                            badnumbercount++;
                        }
                    }
                    else
                    {
                        lstReturnClass.Add(new PhoneNumberClass(looppnc, false, null));
                        badnumbercount++;
                    }
                }
            }
            

           

            Console.Out.WriteLine();
            Console.Out.WriteLine("Total Numbers: " + totalcount.ToString() + "/" + "Mobile Numbers: " + mobilecount.ToString() + "/" + "Bad Numbers (Too Short): " + badnumbercount.ToString() + "/Invalid Numbers: " + invalidnumbercount.ToString());
            Console.Out.WriteLine();
            Console.Out.WriteLine();
            Console.Out.WriteLine();

            OutputFinals();

            Console.Out.WriteLine("Press Enter to exit");

            //string s = Console.ReadKey();
            string s = Console.ReadLine();
            System.Environment.Exit(0);
        }

        static void OutputFinals()
        {
            string outputfilename = @"c:\twilio\returnedData_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".txt";

            if (System.IO.Directory.Exists(new System.IO.FileInfo(outputfilename).Directory.FullName))
            {
            }
            else
            {
                System.IO.Directory.CreateDirectory(new System.IO.FileInfo(outputfilename).Directory.FullName);
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter(outputfilename);

            sw.WriteLine("Number|Caller|Mobile|Valid");

            foreach(PhoneNumberClass pnc in lstReturnClass)
            {
                sw.WriteLine(pnc.Number + "|" + pnc.Name + "|" + pnc.IsMobile + "|" + pnc.IsValid);
            }

            sw.Close();
        }

        static void BuildList()
        {
            phoneNumberList.Add("6153646365");
            phoneNumberList.Add("9018679055");
            phoneNumberList.Add("9013646365");
            phoneNumberList.Add("9118679055");
            phoneNumberList.Add("3646365");
            phoneNumberList.Add("4113646365");
        }


       
    }
}
