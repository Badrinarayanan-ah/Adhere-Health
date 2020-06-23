using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace WhitePagesFindBusinessAPI
{
    class Program
    {

        static int sleeptime = 2500;
        static List<Providers> lstProviders = new List<Providers>();

        static string exceptiontype = string.Empty;

        static int totalcount = 0;
        static int errorcount = 0;
        static int foundcount = 0;
        static int notfoundcount = 0;
        static int recordcount = 0;
        static int baddemographicscount = 0;

        static int sessionid = 0;

        static string apikey = "da6bb4f69f2846de9be3dfbbceeb35ff";

        //static string basewhitepagesurl = "https://proapi.whitepages.com/3.0/business?";
        static string basewhitepagesurl = "https://api.ekata.com/3.0/business?";

        static List<UniqueProvider> lstUniqueProvider = new List<UniqueProvider>();

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                Console.Out.WriteLine("Starting at:" + DateTime.Now.ToString());

                string outputmessage = "";

                SQLDataActions sqlDA = new SQLDataActions();
                sessionid = sqlDA.InsertAPISession(DateTime.Now, -1);

                //CreateProviderListFromDataBase();

                FillProvider();

                CallAPI();

                //output messages and stuff
                outputmessage = lstProviders.Count() + " Total Providers";
                outputmessage = lstProviders.Count() + " Total Unique Providers";
                outputmessage += System.Environment.NewLine + foundcount + " API Identified Providers";
                outputmessage += System.Environment.NewLine + notfoundcount + " API Non-Identified Providers";
                outputmessage += System.Environment.NewLine + errorcount + " Errored Providers";
                outputmessage += System.Environment.NewLine + baddemographicscount + " Bad Demographics Providers (address1 vs address 2)";

                Console.Out.WriteLine(outputmessage);

                //OutputFinals();

                //OutputToDatabase();

                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.Write("The application will exit in 5 seconds...");
                System.Threading.Thread.Sleep(5000);

                Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                Console.Out.WriteLine("");

                sqlDA.UpdateAPISession(sessionid, DateTime.Now);

                System.Environment.Exit(0);

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("An error occurred: " + ex.Message + " - " + ex.StackTrace);
                Console.Out.WriteLine("");

                Console.Out.WriteLine("Ending at:" + DateTime.Now.ToString());
                Console.Out.WriteLine("");

#if DEBUG
                Console.Out.WriteLine("Press Enter to exit");
                string s = Console.ReadLine();
#endif

                //string s = Console.ReadKey();


                SQLDataActions sqlDA = new SQLDataActions();
                sqlDA.UpdateAPISession(sessionid, DateTime.Now);

                System.Environment.Exit(1);
            }
        }

        private static void CreateProviderListFromDataBase()
        {
            lstUniqueProvider.Clear();
            lstProviders.Clear();

            SQLDataActions sqlDA = new SQLDataActions();
            //DataTable dt = sqlDA.ReturnProviderList();
            DataTable dt = new DataTable();

            int originalcount = dt.Rows.Count;

            foreach (DataRow dr in dt.Rows)
            {
                //postgres
                //lstProvider.Add(new Provider(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zipcode"].ToString().Trim(), dr["state"].ToString().Trim(), null, false, false, dr["dob"].ToString()));
                lstProviders.Add(new Providers(dr["name"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zip_code"].ToString().Trim(), dr["state"].ToString().Trim(), null, true, false, null, dr["npi"].ToString()));
            }

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniqueProvider = dlg.ReturnUniqueList(lstProviders);


            int uniquecount = lstUniqueProvider.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }

        static void CallAPI()
        {
            string simpleurl = "";

            recordcount = 0;

            foreach (UniqueProvider p in lstUniqueProvider)
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

                        //test JSON object
                        //string returnValue = "{\"count_business\":3,\"business\":[{\"id\":\"Business.36d4fb7b-6b6f-426e-b08d-41b4393c2906.Durable\",\"name\":\"Kendrick Jim DDS\",\"found_at_address\":{\"id\":\"Location.2505995b-e3ff-4c24-b7a5-840436c2b708.Durable\",\"location_type\":\"Address\",\"street_line_1\":\"5518 Old Hickory Blvd Ste A\",\"street_line_2\":null,\"city\":\"Hermitage\",\"postal_code\":\"37076\",\"zip4\":\"2584\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":36.180487,\"longitude\":-86.606455,\"accuracy\":\"RoofTop\"},\"is_active\":true,\"delivery_point\":\"MultiUnit\",\"link_to_business_start_date\":null,\"link_to_business_end_date\":null},\"current_addresses\":[{\"id\":\"Location.2505995b-e3ff-4c24-b7a5-840436c2b708.Durable\",\"location_type\":\"Address\",\"street_line_1\":\"5518 Old Hickory Blvd Ste A\",\"street_line_2\":null,\"city\":\"Hermitage\",\"postal_code\":\"37076\",\"zip4\":\"2584\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":36.180487,\"longitude\":-86.606455,\"accuracy\":\"RoofTop\"},\"is_active\":true,\"delivery_point\":\"MultiUnit\",\"link_to_business_start_date\":null}],\"historical_addresses\":[],\"phones\":[{\"id\":\"Phone.a1eb6fef-a2e1-4b08-cfe3-bc7128b6de32.Durable\",\"phone_number\":\"+16158897397\",\"line_type\":\"FixedVOIP\"}],\"associated_people\":[],\"associated_businesses\":[{\"id\":\"Business.534c4ee3-6140-439a-b60c-c3798077e611.Durable\",\"name\":\"James L Kendrick DDS\",\"relation\":\"Household\"},{\"id\":\"Business.d5c2772d-0586-48ff-89d8-5abcd27b5a91.Durable\",\"name\":\"Daniel Eric Oxford DDS\",\"relation\":\"Household\"},{\"id\":\"Business.e076da27-7e25-4636-b1bd-7756e30a3d40.Durable\",\"name\":\"Kendrick Dental\",\"relation\":\"Household\"}]},{\"id\":\"Business.534c4ee3-6140-439a-b60c-c3798077e611.Durable\",\"name\":\"James L Kendrick DDS\",\"found_at_address\":{\"id\":\"Location.2505995b-e3ff-4c24-b7a5-840436c2b708.Durable\",\"location_type\":\"Address\",\"street_line_1\":\"5518 Old Hickory Blvd Ste A\",\"street_line_2\":null,\"city\":\"Hermitage\",\"postal_code\":\"37076\",\"zip4\":\"2584\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":36.180487,\"longitude\":-86.606455,\"accuracy\":\"RoofTop\"},\"is_active\":true,\"delivery_point\":\"MultiUnit\",\"link_to_business_start_date\":null,\"link_to_business_end_date\":null},\"current_addresses\":[{\"id\":\"Location.2505995b-e3ff-4c24-b7a5-840436c2b708.Durable\",\"location_type\":\"Address\",\"street_line_1\":\"5518 Old Hickory Blvd Ste A\",\"street_line_2\":null,\"city\":\"Hermitage\",\"postal_code\":\"37076\",\"zip4\":\"2584\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":36.180487,\"longitude\":-86.606455,\"accuracy\":\"RoofTop\"},\"is_active\":true,\"delivery_point\":\"MultiUnit\",\"link_to_business_start_date\":null}],\"historical_addresses\":[],\"phones\":[{\"id\":\"Phone.a1a76fef-a2e1-4b08-cfe3-bc7128b6de32.Durable\",\"phone_number\":\"+16158897363\",\"line_type\":\"FixedVOIP\"}],\"associated_people\":[],\"associated_businesses\":[{\"id\":\"Business.36d4fb7b-6b6f-426e-b08d-41b4393c2906.Durable\",\"name\":\"Kendrick Jim DDS\",\"relation\":\"Household\"},{\"id\":\"Business.d5c2772d-0586-48ff-89d8-5abcd27b5a91.Durable\",\"name\":\"Daniel Eric Oxford DDS\",\"relation\":\"Household\"},{\"id\":\"Business.e076da27-7e25-4636-b1bd-7756e30a3d40.Durable\",\"name\":\"Kendrick Dental\",\"relation\":\"Household\"}]},{\"id\":\"Business.e076da27-7e25-4636-b1bd-7756e30a3d40.Durable\",\"name\":\"Kendrick Dental\",\"found_at_address\":{\"id\":\"Location.2505995b-e3ff-4c24-b7a5-840436c2b708.Durable\",\"location_type\":\"Address\",\"street_line_1\":\"5518 Old Hickory Blvd Ste A\",\"street_line_2\":null,\"city\":\"Hermitage\",\"postal_code\":\"37076\",\"zip4\":\"2584\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":36.180487,\"longitude\":-86.606455,\"accuracy\":\"RoofTop\"},\"is_active\":true,\"delivery_point\":\"MultiUnit\",\"link_to_business_start_date\":null,\"link_to_business_end_date\":null},\"current_addresses\":[{\"id\":\"Location.2505995b-e3ff-4c24-b7a5-840436c2b708.Durable\",\"location_type\":\"Address\",\"street_line_1\":\"5518 Old Hickory Blvd Ste A\",\"street_line_2\":null,\"city\":\"Hermitage\",\"postal_code\":\"37076\",\"zip4\":\"2584\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":36.180487,\"longitude\":-86.606455,\"accuracy\":\"RoofTop\"},\"is_active\":true,\"delivery_point\":\"MultiUnit\",\"link_to_business_start_date\":null}],\"historical_addresses\":[],\"phones\":[{\"id\":\"Phone.a1a76fef-a2e1-4b08-cfe3-bc7128b6de32.Durable\",\"phone_number\":\"+16158897363\",\"line_type\":\"FixedVOIP\"},{\"id\":\"Phone.a1eb6fef-a2e1-4b08-cfe3-bc7128b6de32.Durable\",\"phone_number\":\"+16158897397\",\"line_type\":\"FixedVOIP\"}],\"associated_people\":[],\"associated_businesses\":[{\"id\":\"Business.36d4fb7b-6b6f-426e-b08d-41b4393c2906.Durable\",\"name\":\"Kendrick Jim DDS\",\"relation\":\"Household\"},{\"id\":\"Business.534c4ee3-6140-439a-b60c-c3798077e611.Durable\",\"name\":\"James L Kendrick DDS\",\"relation\":\"Household\"},{\"id\":\"Business.d5c2772d-0586-48ff-89d8-5abcd27b5a91.Durable\",\"name\":\"Daniel Eric Oxford DDS\",\"relation\":\"Household\"}]}],\"warnings\":[\"Missing unit/apt/suite number\"],\"error\":null}";

                        p.JSON = returnValue;
                        p.Request = simpleurl;

                        //adding due to WP request - Too Many Requests was error message. 4 per second are allowed
                        //BW 4/8/2019 
                        System.Threading.Thread.Sleep(sleeptime);

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

        static void ParseResponse(string returnValue, UniqueProvider p)
        {
            try
            {
                int smartcounter = 0;

                dynamic stuff = JsonConvert.DeserializeObject(returnValue);

                int personcount = stuff.business.Count;
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
                    string name = stuff.business[i].name;

                    int phonescount = stuff.business[i].phones.Count;

                    int j = 0;


                    string firstphonenumber = String.Empty;
                    string secondphonenumber = String.Empty;

                    for (j = 0; j < phonescount; j++)
                    {
                        string phonenumber = stuff.business[i].phones[j].phone_number;
                        string linetype = stuff.business[i].phones[j].line_type;


                        if (phonenumber.Length > 0)
                        {
                            smartcounter++;

                            p.PhoneNumbers.Add(phonenumber);
                            p.LineTypes.Add(linetype);
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


        static void OutputDatabaseSingle(UniqueProvider p, string simpleurl)
        {
            SQLDataActions sqlDA = new SQLDataActions();

            if (p.PhoneNumbers.Count == 0)
            {
                sqlDA.InsertAPIReturnedData(p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, "", "", p.JSON, simpleurl, sessionid, p.NPI);
            }
            else
            {
                for (int i = 0; i < p.PhoneNumbers.Count; i++)
                {
                    sqlDA.InsertAPIReturnedData(p.DOB, p.Name, p.Address, p.Address2, p.City, p.State, p.PostalCode, p.PhoneNumbers[i], p.LineTypes[i], p.JSON, simpleurl, sessionid, p.NPI);
                }
            }

        }

        static void FillProvider()
        {
            //ability to send across a single person

            /*
            lstProviders.Add(new Providers("Michael Khadavi", "315 Business Loop 70 W", "", "Columbia", "652033248", "MO", "", false, false, "", "1003050204"));
            */


            int originalcount = lstProviders.Count;

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniqueProvider = dlg.ReturnUniqueList(lstProviders);

            int uniquecount = lstUniqueProvider.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }
    }
}
