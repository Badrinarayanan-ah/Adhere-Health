using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Data;

namespace WhitePagesFindPersonAPI
{
    class Program
    {
        static int sleeptime = 2500;

        //static List<int> lstMTMClients = new List<int> { 15, 30, 54, 62, 70 };
        //removing Healthspring (April 2018 - BDW)
        static List<int> lstMTMClients = new List<int> { 15, 54, 62, 70, 79 };

        //file name format
        //id|addressable_id|pmd_patientid|organizationname|firstname|middleinitial|lastname|street|street2|city|state|postalcode|patientprimaryaddress|primary|addressid|maxaddressid|pmdclientid|lastenteredaddress|
        //0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17

        static List<Person> lstPerson = new List<Person>();

        static int totalcount = 0;
        static int errorcount = 0;
        static int foundcount = 0;
        static int notfoundcount = 0;
        static int recordcount = 0;
        static int baddemographicscount = 0;

        /*
        static int pmdClientID = 15; //scan
        static int pmdClientID = 30; //healthspringsaas
        static int pmdClientID = 54; //highmark
        static int pmdClientID = 62; //meridian
        static int pmdClientID = 70; //summacare
        */


        //static int pmdClientID = 62;
        //static int pmdClientID = 54;
        //static int pmdClientID = 70;
        //static int pmdClientID = 15;
        static int pmdClientID = 301; //PDP first //MAPD contract list second


        static string exceptiontype = string.Empty;

        static string apikey = "0d8ccfffebcc4ed2a84d2bc9579255b2";
        //static string whitepagesurl = "https://proapi.whitepages.com/3.0/person?name=Drama+Number&address.city=Ashland&address.state_code=MT&api_key=" + apikey;
        //static string basewhitepagesurl = "https://proapi.whitepages.com/3.0/person?";

        static string whitepagesurl = "https://api.ekata.com/3.0/person?name=Drama+Number&address.city=Ashland&address.state_code=MT&api_key=" + apikey;
        static string basewhitepagesurl = "https://api.ekata.com/3.0/person?";


        //static string fakebasewhitepagesurl = "https://proapi.whitepages.com/3.0/personabcd?";
        //static string truebasewhitepagesurl = "https://proapi.whitepages.com/3.0/person?";

        static List<UniquePerson> lstUniquePerson = new List<UniquePerson>();

        static int sessionid = 0;

        static void MTMPiece(string[] args)
        {
            pmdClientID = -4;

            Arguments a = new Arguments();
            a.ProcessArguments(args);

            if (a.inputType == "0" || a.inputType == "1")
            {
                SQLDataActions sda = new SQLDataActions();

                //overwrite the client list from the whitepages table;
                //BW 10/24/2018
                DataTable dt = sda.GetWhitePagesClients();

                lstMTMClients.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    if (row["producttype"].ToString() == "MTM")
                    {
                        lstMTMClients.Add(Convert.ToInt32(row["clientid"]));
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;



                if (a.Errored == false)
                {
                    pmdClientID = Convert.ToInt32(a.inputPMDClientID);

                    if (pmdClientID == -1)
                    {
                        foreach (int i in lstMTMClients)
                        {
                            foundcount = notfoundcount = errorcount = baddemographicscount = 0;

                            int pmdClientID = i;

                            Console.Out.WriteLine("PMDClientID: " + pmdClientID.ToString());
                            Console.Out.WriteLine("Starting at:" + DateTime.Now.ToString());

                            string outputmessage = "";

                            try
                            {
                                if (a.inputPMDMode == "1")
                                {
                                    CreatePersonListFromDataBase(true, pmdClientID);
                                }
                                else if (a.inputPMDMode == "2")
                                {
                                    CreatePersonFileFromManualList();
                                }

                                /*
                                if (pmdClientID == 54)
                                {
                                    RemovePreviouslyCalledPeople(pmdClientID);
                                }
                                */

                                SQLDataActions sqlDA = new SQLDataActions();
                                sessionid = sqlDA.InsertAPISession(DateTime.Now, pmdClientID);

                                CallAPI();

                                //output messages and stuff
                                outputmessage = lstPerson.Count() + " Total People";
                                outputmessage = lstUniquePerson.Count() + " Total Unique People";
                                outputmessage += System.Environment.NewLine + foundcount + " API Identified People";
                                outputmessage += System.Environment.NewLine + notfoundcount + " API Non-Identified People";
                                outputmessage += System.Environment.NewLine + errorcount + " Errored People";
                                outputmessage += System.Environment.NewLine + baddemographicscount + " Bad Demographics People (address1 vs address 2)";

                                Console.Out.WriteLine(outputmessage);

                                Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                                Console.Out.WriteLine("");

                                sqlDA.UpdateAPISession(sessionid, DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                Console.Out.WriteLine("An error occurred: " + ex.Message + " - " + ex.StackTrace);
                                Console.Out.WriteLine("");

                                Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                                Console.Out.WriteLine("");

                                Console.Out.WriteLine("Press Enter to exit");

                                //string s = Console.ReadKey();

#if DEBUG
                                string s = Console.ReadLine();
#endif
                                SQLDataActions sqlDA = new SQLDataActions();
                                sqlDA.UpdateAPISession(sessionid, DateTime.Now);

                                System.Environment.Exit(1);
                            }
                        }
                    }
                    else
                    {
                        Console.Out.WriteLine("Starting at:" + DateTime.Now.ToString());

                        string outputmessage = "";

                        try
                        {
                            //temp for cynthia's meridian person
                            //FillPerson();

                            //Highmark Formulary
                            //CreatePersonListFromFile();

                            //query the ODS data store for patient information by client
                            if (a.inputPMDMode == "1")
                            {
                                CreatePersonListFromDataBase(true, Convert.ToInt32(a.inputPMDClientID)   );
                            }
                            else if (a.inputPMDMode == "2")
                            {
                                CreatePersonFileFromManualList();
                            }

                            //Highmark Formulary - remove people we have already used for that special project
                            /*
                            if (pmdClientID == 54)
                            {
                                RemovePreviouslyCalledPeople(pmdClientID);
                            }
                            */

                            SQLDataActions sqlDA = new SQLDataActions();
                            sessionid = sqlDA.InsertAPISession(DateTime.Now, pmdClientID);

                            //THE call to the White Pages API


                            CallAPI();

                            //output messages and stuff
                            outputmessage = lstPerson.Count() + " Total People";
                            outputmessage = lstUniquePerson.Count() + " Total Unique People";
                            outputmessage += System.Environment.NewLine + foundcount + " API Identified People";
                            outputmessage += System.Environment.NewLine + notfoundcount + " API Non-Identified People";
                            outputmessage += System.Environment.NewLine + errorcount + " Errored People";
                            outputmessage += System.Environment.NewLine + baddemographicscount + " Bad Demographics People (address1 vs address 2)";

                            Console.Out.WriteLine(outputmessage);

                            //OutputFinals();

                            //OutputToDatabase();



                            Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                            Console.Out.WriteLine("");



                            sqlDA.UpdateAPISession(sessionid, DateTime.Now);
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine("An error occurred: " + ex.Message + " - " + ex.StackTrace);
                            Console.Out.WriteLine("");

                            Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                            Console.Out.WriteLine("");

#if DEBUG
                            string s = Console.ReadLine();
                            Console.Out.WriteLine("Press Enter to exit");
#endif

                            //string s = Console.ReadKey();


                            SQLDataActions sqlDA = new SQLDataActions();
                            sqlDA.UpdateAPISession(sessionid, DateTime.Now);

                            System.Environment.Exit(1);
                        }

                    }





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

        static void ComplexCasePiece(string[] args)
        {
            pmdClientID = -4;
            
               Arguments a = new Arguments();
            a.ProcessArguments(args);


            if (a.inputType == "0" || a.inputType == "2")
            {
                SQLDataActions sda = new SQLDataActions();

                //overwrite the client list from the whitepages table;
                //BW 10/24/2018
                DataTable dt = sda.GetWhitePagesClients();

                lstMTMClients.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    if (row["producttype"].ToString() == "ComplexCase")
                    {
                        lstMTMClients.Add(Convert.ToInt32(row["clientid"]));
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                if (a.Errored == false)
                {
                    pmdClientID = Convert.ToInt32(a.inputPMDClientID);

                    if (pmdClientID == -1)
                    {
                        foreach (int i in lstMTMClients)
                        {
                            foundcount = notfoundcount = errorcount = baddemographicscount = 0;

                            int pmdClientID = i;

                            Console.Out.WriteLine("PMDClientID: " + pmdClientID.ToString());
                            Console.Out.WriteLine("Starting at:" + DateTime.Now.ToString());

                            string outputmessage = "";

                            try
                            {
                                if (a.inputPMDMode == "1")
                                {
                                    CreatePersonListFromDataBase(false, pmdClientID);
                                }
                                SQLDataActions sqlDA = new SQLDataActions();
                                sessionid = sqlDA.InsertAPISession(DateTime.Now, pmdClientID);

                                CallAPI();

                                //output messages and stuff
                                outputmessage = lstPerson.Count() + " Total People";
                                outputmessage = lstUniquePerson.Count() + " Total Unique People";
                                outputmessage += System.Environment.NewLine + foundcount + " API Identified People";
                                outputmessage += System.Environment.NewLine + notfoundcount + " API Non-Identified People";
                                outputmessage += System.Environment.NewLine + errorcount + " Errored People";
                                outputmessage += System.Environment.NewLine + baddemographicscount + " Bad Demographics People (address1 vs address 2)";

                                Console.Out.WriteLine(outputmessage);

                                Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                                Console.Out.WriteLine("");

                                sqlDA.UpdateAPISession(sessionid, DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                Console.Out.WriteLine("An error occurred: " + ex.Message + " - " + ex.StackTrace);
                                Console.Out.WriteLine("");

                                Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                                Console.Out.WriteLine("");

                                Console.Out.WriteLine("Press Enter to exit");

                                //string s = Console.ReadKey();

#if DEBUG
                                string s = Console.ReadLine();
#endif
                                SQLDataActions sqlDA = new SQLDataActions();
                                sqlDA.UpdateAPISession(sessionid, DateTime.Now);

                                System.Environment.Exit(1);
                            }
                        }
                    }
                    else
                    {
                        Console.Out.WriteLine("Starting at:" + DateTime.Now.ToString());

                        string outputmessage = "";

                        try
                        {
                            //temp for cynthia's meridian person
                            //FillPerson();


                            //query the ODS data store for patient information by client
                            if (a.inputPMDMode == "1")
                            {
                                CreatePersonListFromDataBase(false, Convert.ToInt32(a.inputPMDClientID));
                            }



                            SQLDataActions sqlDA = new SQLDataActions();
                            sessionid = sqlDA.InsertAPISession(DateTime.Now, pmdClientID);

                            //THE call to the White Pages API


                            CallAPI();

                            //output messages and stuff
                            outputmessage = lstPerson.Count() + " Total People";
                            outputmessage = lstUniquePerson.Count() + " Total Unique People";
                            outputmessage += System.Environment.NewLine + foundcount + " API Identified People";
                            outputmessage += System.Environment.NewLine + notfoundcount + " API Non-Identified People";
                            outputmessage += System.Environment.NewLine + errorcount + " Errored People";
                            outputmessage += System.Environment.NewLine + baddemographicscount + " Bad Demographics People (address1 vs address 2)";

                            Console.Out.WriteLine(outputmessage);

                            //OutputFinals();

                            //OutputToDatabase();



                            Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                            Console.Out.WriteLine("");



                            sqlDA.UpdateAPISession(sessionid, DateTime.Now);
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine("An error occurred: " + ex.Message + " - " + ex.StackTrace);
                            Console.Out.WriteLine("");

                            Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                            Console.Out.WriteLine("");

#if DEBUG
                            string s = Console.ReadLine();
                            Console.Out.WriteLine("Press Enter to exit");
#endif

                            //string s = Console.ReadKey();


                            SQLDataActions sqlDA = new SQLDataActions();
                            sqlDA.UpdateAPISession(sessionid, DateTime.Now);

                            System.Environment.Exit(1);
                        }

                    }





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

        static void Main(string[] args)
        {

            MTMPiece(args);

#if COMPLEXCASE
            ComplexCasePiece(args);
#endif

            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.Write("The application will exit in 5 seconds...");
            System.Threading.Thread.Sleep(5000);

            System.Environment.Exit(0);

        }

        static void RemovePreviouslyCalledPeople(int pmdClientID)
        {
            SQLDataActions sqlDA = new SQLDataActions();
            DataTable dt = sqlDA.ReturnWhitePagesListByClient(pmdClientID);

            int removedcount = 0;

            Console.Out.WriteLine("");
            Console.Out.WriteLine("******");
            Console.Out.WriteLine("Before Previously Called Removal: " + lstUniquePerson.Count.ToString());

            foreach (DataRow dr in dt.Rows)
            {
                //string pmdpatientid = dr["pmd_patient_id"].ToString();
                string pmdpatientid = dr["pmdpatientid"].ToString();

                lstUniquePerson.RemoveAll(x => x.PMDPatientID == pmdpatientid);
            }

            Console.Out.WriteLine("After Previously Called Removal: " + lstUniquePerson.Count.ToString());
            Console.Out.WriteLine("******");
            Console.Out.WriteLine("");

        }

        static void CreatePersonFileFromManualList()
        {
            lstUniquePerson.Clear();
            lstPerson.Clear();

            int counter = 0;

            counter++;

            //insert list here
            /*
            lstPerson.Add(new Person("Sally", "Maneval", "Sally Maneval", "215 High Blvd Apt 617", "", "Wilkes Barre", "18702", "PA", "", false, false, "54", "105598423", "", ""));
            */

            lstPerson.RemoveAll(x => x.PMDClientID.ToString() != pmdClientID.ToString());

            int originalcount = lstPerson.Count;

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniquePerson = dlg.ReturnUniqueList(lstPerson);

            int uniquecount = lstUniquePerson.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }

        static void CreatePersonListFromFile()
        {
            lstUniquePerson.Clear();
            lstPerson.Clear();

            string thisfilename = "meridianwhitepages.txt";
            thisfilename = "summacarewhitepages.txt";
            thisfilename = "healthspringwhitepages.txt";
            thisfilename = "scanwhitepages.txt";
            thisfilename = "highmarkwhitepages.txt";

            string filename = @"c:\whitepagesapi\" + thisfilename;

            StreamReader sr = new StreamReader(filename);
            string line;
            int counter = 0;

            //lstPerson.Add(new Person("Cynthia", "Sandahl", "Cynthia Sandahl", "7412 Rolling River Pkwy ", "","Nashvile", "", "TN", null, false, false,""));
            //lstPerson.Add(new Person("Arlenda", "Flannel", "Arlenda Flannel", "5089 Welbourne Cove", "", "Arlington", "38002", "TN", null, false, false));

            while ((line = sr.ReadLine()) != null)
            {
                string[] lineParser = line.Split(new char[] { '}', '\t' });
                string firstname = lineParser[4].Trim();
                string middleinitial = lineParser[5].Trim();
                string lastname = lineParser[6].Trim();
                string street = lineParser[7].Trim();
                string street2 = lineParser[8].Trim();
                string city = lineParser[9].Trim();
                string state = lineParser[10].Trim();
                string postalcode = lineParser[11].Trim();
                string pmdclientid = lineParser[16].Trim();
                string pmdpatientid = lineParser[2].Trim();
                string dob = lineParser[17].Trim();

                //placeholder for contract number
                string contractnumber = "";
                counter++;

                lstPerson.Add(new Person(firstname, lastname, (firstname + " " + middleinitial + " " + lastname).Replace("  ", " "), street, street2, city, postalcode, state, null, true, false, pmdclientid, pmdpatientid, dob, contractnumber));

            }



            int originalcount = counter;

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniquePerson = dlg.ReturnUniqueList(lstPerson);

            int uniquecount = lstUniquePerson.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }


        static void CreatePersonListFromDataBase(bool ismtm, int pmdClientID)
        {
            lstUniquePerson.Clear();
            lstPerson.Clear();

            SQLDataActions sqlDA = new SQLDataActions();

            int originalcount = 0;

            //taking out as of 8/20/19 BDW

            /*
            //DataTable dt = sqlDA.ReturnDemographicsDataPostGres(pmdClientID);
            DataTable dt = sqlDA.ReturnDemographicsDataODS(pmdClientID);

            originalcount = dt.Rows.Count;

            foreach (DataRow dr in dt.Rows)
            {
                //postgres
                //lstPerson.Add(new Person(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zipcode"].ToString().Trim(), dr["state"].ToString().Trim(), null, false, false, dr["dob"].ToString()));
                lstPerson.Add(new Person(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zip_code"].ToString().Trim(), dr["state"].ToString().Trim(), null, true, false, pmdClientID.ToString(), dr["pmd_patient_id"].ToString(), dr["dob"].ToString(), dr["contractnumber"].ToString()));
            }
            */

            //section new 12/20/2017
            //use the missing numbers query as well
            DataTable dt2 = sqlDA.ReturnPatientsWithoutPhoneNumbers(pmdClientID, ismtm);

            originalcount += dt2.Rows.Count;

            foreach (DataRow dr in dt2.Rows)
            {
                //postgres
                //lstPerson.Add(new Person(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zipcode"].ToString().Trim(), dr["state"].ToString().Trim(), null, false, false, dr["dob"].ToString()));
                lstPerson.Add(new Person(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(),dr["secondary_address"].ToString(), dr["city"].ToString().Trim(), dr["zip_code"].ToString().Trim(), dr["state"].ToString().Trim(), null, true, false, pmdClientID.ToString(), dr["pmd_patient_id"].ToString(), dr["dob"].ToString(), dr["contractnumber"].ToString()));
            }


            /*
            //section new 7/25/2019 BW
            //add in specific people i want to find numbers for
            if (DateTime.Now.Month >= 10)
            { 
                DataTable dt3 = sqlDA.ReturnPatientsManualListCreatedToday(pmdClientID);

                originalcount += dt3.Rows.Count;

                foreach (DataRow dr in dt3.Rows)
                {
                    //postgres
                    //lstPerson.Add(new Person(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zipcode"].ToString().Trim(), dr["state"].ToString().Trim(), null, false, false, dr["dob"].ToString()));
                    lstPerson.Add(new Person(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zip_code"].ToString().Trim(), dr["state"].ToString().Trim(), null, true, false, pmdClientID.ToString(), dr["pmd_patient_id"].ToString(), dr["dob"].ToString(), dr["contractnumber"].ToString()));
                }
            }
            */

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniquePerson = dlg.ReturnUniqueList(lstPerson);

            int uniquecount = lstUniquePerson.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");


        }

        static void FillPerson()
        {
            //ability to send across a single person
            //lstPerson.Add(new Person("Brian","Williams","Brian Williams", "2788 Aston Woods Lane", "", "Thompsons Station", "37179", "TN", null, false, false));
            //lstPerson.Add(new Person("Rebecca","Williams","Rebecca Williams", "2788 Aston Woods Lane", "", "Thompsons Station", "37179", "TN", null, false, false));
            //lstPerson.Add(new Person("Arlenda","Flannel","Arlenda Flannel", "5089 Welbourne Cove", "", "Arlington", "38002", "TN", null, false, false));

            //11.28.17 - cynthia's manual search
            lstPerson.Add(new Person("Jimmy", "Graham", "Jimmy Graham", "39 Frelinghuysen Ave", "Apt 6", "Battle Creek", "49017", "MI", "", false, false, "62", "107488413", "", ""));
            //lstPerson.Add(new Person("Jimmy", "Graham", "Jimmy Graham", "140 W MICHIGAN", "", "Battle Creek", "49017", "MI", "", true, false, "62", "107488413", "", ""));

            lstPerson.Add(new Person("Dewayne", "Harris", "Dewayne Harris", "15600 Honore Ave", "", "Harvey", "60426", "IL", "", false, false, "62", "107496440", "", ""));


            int originalcount = lstPerson.Count;

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniquePerson = dlg.ReturnUniqueList(lstPerson);

            int uniquecount = lstUniquePerson.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }

        /*
        static void InsertIntoDatabase(string sql)
        {

        }
        */

        static void CallAPI()
        {
            string simpleurl = "";

            recordcount = 0;

            foreach (UniquePerson p in lstUniquePerson)
            {
                simpleurl = basewhitepagesurl;
                recordcount++;

                //only run if one of the addresses has a value - don't want too many people

                if (p.Address.Length > 0 || p.Address2.Length > 0)
                {
                    //stop at 5 people 
                    //stop at 2 people
                    //if (recordcount < 5)
                    //if (recordcount < 25)
                    {
                        var parameters = HttpUtility.ParseQueryString(string.Empty);
                        parameters.Add("api_key", apikey);
                        parameters.Add("name", p.Name);
                        parameters.Add("address.street_line_1", p.Address);
                        parameters.Add("address.street_line_2", p.Address2);
                        parameters.Add("address.city", p.City);
                        parameters.Add("address.state_code", p.State);
                        //parameters.Add("address.postalcode", p.PostalCode);
                        parameters.Add("address.postal_code", p.PostalCode);

                        simpleurl += parameters;

                        //call the GET function
                        string returnValue = GET(simpleurl);

                        p.JSON = returnValue;
                        p.Request = simpleurl;

                        System.Threading.Thread.Sleep(sleeptime);

                        //adding due to WP request - Too Many Requests was error message. 4 per second are allowed
                        //BW 4/8/2019 
                        ParseResponse(returnValue, p);

                        OutputDatabaseSingle(p, simpleurl);

                        //Console.Out.WriteLine(returnValue);
                    }
                }
                else
                {
                    baddemographicscount++;
                }
            }
        }

        static void OutputDatabaseSingle(UniquePerson p, string simpleurl)
        {
            SQLDataActions sqlDA = new SQLDataActions();

              sqlDA.InsertAPIReturnedData(p.PMDPatientID, p.PMDClientID, p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumber, p.LineType, p.JSON, simpleurl, p.ContractNumber, sessionid);

            if (p.PhoneNumber2.Length > 0)
            {
                sqlDA.InsertAPIReturnedData(p.PMDPatientID, p.PMDClientID, p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumber2, p.LineType2, p.JSON, simpleurl, p.ContractNumber, sessionid);
            }

            if (p.PhoneNumber3.Length > 0)
            {
                sqlDA.InsertAPIReturnedData(p.PMDPatientID, p.PMDClientID, p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumber3, p.LineType3, p.JSON, simpleurl, p.ContractNumber, sessionid);
            }
        }

        //static void ParseResponse(string returnValue, Person p)
        static void ParseResponse(string returnValue, UniquePerson p)
        {
            try
            {
                int smartcounter = 0;

                dynamic stuff = JsonConvert.DeserializeObject(returnValue);

                int personcount = stuff.person.Count;
                int i = 0;

                if (personcount > 0)
                {
                    foundcount++;
                }
                else
                {
                    notfoundcount++;
                }

                for (i = 0; i < personcount; i++)
                {
                    string name = stuff.person[i].name;

                    int phonescount = stuff.person[i].phones.Count;

                    int j = 0;


                    string firstphonenumber = String.Empty;
                    string secondphonenumber = String.Empty;

                    for (j = 0; j < phonescount; j++)
                    {
                        string phonenumber = stuff.person[i].phones[j].phone_number;
                        string linetype = stuff.person[i].phones[j].line_type;


                        if (phonenumber.Length > 0)
                        {
                            smartcounter++;

                            if (smartcounter == 3)
                            {
                                p.PhoneNumber3 = phonenumber;

                                if (linetype.Length > 0)
                                {
                                    p.LineType3 = linetype;
                                }
                            }
                            else if (smartcounter == 2)
                            {
                                p.PhoneNumber2 = phonenumber;

                                if (linetype.Length > 0)
                                {
                                    p.LineType2 = linetype;
                                }
                            }
                            else if (smartcounter == 1)
                            {
                                p.PhoneNumber = phonenumber;

                                if (linetype.Length > 0)
                                {
                                    p.LineType = linetype;
                                }
                            }
                        }


                    }

                }


            }
            catch (Exception ex)
            {
                exceptiontype += "*Error occured on Record #" + recordcount.ToString() + "/" + ex.Message + "-" + ex.StackTrace;
                errorcount++;
            }
        }

        static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("****Response Error: " + ex.Message + "-" + ex.StackTrace + "******");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");

                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText


                }
                throw;
            }
        }

        public static void OutputToDatabase()
        {
            SQLDataActions sqlDA = new SQLDataActions();

            foreach (UniquePerson p in lstUniquePerson)
            {
                sqlDA.InsertAPIReturnedData(p.PMDPatientID, p.PMDClientID, p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumber, p.LineType, p.JSON, p.Request, p.ContractNumber, sessionid);

                if (p.PhoneNumber2.Length > 0)
                {
                    sqlDA.InsertAPIReturnedData(p.PMDPatientID, p.PMDClientID, p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumber2, p.LineType2, p.JSON, p.Request, p.ContractNumber, sessionid);
                }

                if (p.PhoneNumber3.Length > 0)
                {
                    sqlDA.InsertAPIReturnedData(p.PMDPatientID, p.PMDClientID, p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumber3, p.LineType3, p.JSON, p.Request, p.ContractNumber, sessionid);
                }

            }
        }

        public static void OutputFinals()
        {

            string outputfilename = @"c:\whitepagesapi\output\" + pmdClientID.ToString() + "_returnedData_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".txt";

            if (System.IO.Directory.Exists(new System.IO.FileInfo(outputfilename).Directory.FullName))
            {
            }
            else
            {
                System.IO.Directory.CreateDirectory(new System.IO.FileInfo(outputfilename).Directory.FullName);
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter(outputfilename);

            //sw.WriteLine("PMDPatientID|DOB|Name|Address1|Address2|City|State|PostalCode|PhoneNumber|LineType|AdditionalPhoneNumber|AdditionalLineType|VerifiedDate|JSON");
            sw.WriteLine("PMDPatientID|PMDClientID|DOB|Name|Address1|Address2|City|State|PostalCode|PhoneNumber|LineType|VerifiedDate|JSON");

            foreach (UniquePerson p in lstUniquePerson)
            {
                //sw.WriteLine(p.PMDPatientID + "|" + p.DOB + "|" + p.Name + "|" + p.Address + "|" + p.Address2 + "|" + p.City + "|" + p.State + "|" + p.PostalCode + "|" + p.PhoneNumber + "|" + p.LineType + "|" + p.PhoneNumber2 + "|" + p.LineType2 + "|" + DateTime.Now.ToShortDateString() + "|" + p.JSON);

                sw.WriteLine(p.PMDPatientID + "|" + p.PMDClientID + "|" + p.DOB + "|" + p.Name + "|" + p.Address + "|" + p.Address2 + "|" + p.City + "|" + p.State + "|" + p.PostalCode + "|" + p.PhoneNumber + "|" + p.LineType + "|" + DateTime.Now.ToShortDateString() + "|" + p.JSON);

                if (p.PhoneNumber2.Length > 0)
                {
                    sw.WriteLine(p.PMDPatientID + "|" + p.PMDClientID + "|" + p.DOB + "|" + p.Name + "|" + p.Address + "|" + p.Address2 + "|" + p.City + "|" + p.State + "|" + p.PostalCode + "|" + p.PhoneNumber2 + "|" + p.LineType2 + "|" + DateTime.Now.ToShortDateString() + "|" + p.JSON);
                }

                if (p.PhoneNumber3.Length > 0)
                {
                    sw.WriteLine(p.PMDPatientID + "|" + p.PMDClientID + "|" + p.DOB + "|" + p.Name + "|" + p.Address + "|" + p.Address2 + "|" + p.City + "|" + p.State + "|" + p.PostalCode + "|" + p.PhoneNumber3 + "|" + p.LineType3 + "|" + DateTime.Now.ToShortDateString() + "|" + p.JSON);
                }
            }

            sw.Close();
        }
    }




}
