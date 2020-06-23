using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace WhitePagesReversePhoneAPI
{
    class Program
    {
        static int sleeptime = 2500;

        static List<Phones> lstPhones = new List<Phones>();

        static string exceptiontype = string.Empty;

        static int totalcount = 0;
        static int errorcount = 0;
        static int foundcount = 0;
        static int notfoundcount = 0;
        static int recordcount = 0;
        static int baddemographicscount = 0;

        static int sessionid = 0;

        static string apikey = "b19cdfe38e604bd2baa4f42695109b42";

        //static string basewhitepagesurl = "https://proapi.whitepages.com/3.0/phone?";
        static string basewhitepagesurl = "https://api.ekata.com/3.0/phone?";

        static List<UniquePhones> lstUniquePhone = new List<UniquePhones>();

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string outputmessage = "";

            try
            {
                SQLDataActions sqlDA = new SQLDataActions();
                sessionid = sqlDA.InsertAPISession(DateTime.Now);

                FillPhones();

                //query the ODS data store for patient information by client
                //CreatePhoneListFromDataBase();

                Console.Out.WriteLine("Starting at:" + DateTime.Now.ToString());

                CallAPI();


                //output messages and stuff
                outputmessage = lstPhones.Count() + " Total Phones";
                outputmessage += System.Environment.NewLine + foundcount + " API Identified Phone Numbers";
                outputmessage += System.Environment.NewLine + notfoundcount + " API Non-Identified Phone Numbers";
                outputmessage += System.Environment.NewLine + errorcount + " Errored  Phone Numbers";


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

        private static void CreatePhoneListFromDataBase()
        {
            lstPhones.Clear();

            SQLDataActions sqlDA = new SQLDataActions();
            //DataTable dt = sqlDA.ReturnProviderList();
            DataTable dt = new DataTable();

            int originalcount = dt.Rows.Count;

            foreach (DataRow dr in dt.Rows)
            {
                //postgres
                //lstProvider.Add(new Provider(dr["firstname"].ToString().Trim(), dr["lastname"].ToString().Trim(), dr["firstname"].ToString().Trim() + " " + dr["middleinitial"].ToString().Trim() + " " + dr["lastname"].ToString().Trim(), dr["street"].ToString().Trim(), "", dr["city"].ToString().Trim(), dr["zipcode"].ToString().Trim(), dr["state"].ToString().Trim(), null, false, false, dr["dob"].ToString()));
                lstPhones.Add(new Phones(dr["phonenumber"].ToString(), dr["name"].ToString()));
            }


            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniquePhone = dlg.ReturnUniqueList(lstPhones);

            int uniquecount = lstUniquePhone.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }

        private static void FillPhones()
        {
            /*
            lstPhones.Add(new Phones("6623773000", "Robert Kirksey"));
            */

            int originalcount = lstPhones.Count;

            DistinctListGenerator dlg = new DistinctListGenerator();
            lstUniquePhone = dlg.ReturnUniqueList(lstPhones);

            int uniquecount = lstUniquePhone.Count;

            Console.Out.WriteLine(originalcount.ToString() + " Original Count/" + uniquecount.ToString() + " Unique Count");
        }

        private static void CallAPI()
        {
            string simpleurl = "";

            recordcount = 0;

            foreach (UniquePhones p in lstUniquePhone)
            {
                simpleurl = basewhitepagesurl;
                recordcount++;

                if (p.PhoneNumber.Length > 0)
                {
                    //stop at 5 people 
                    //stop at 2 people
                    //if (recordcount < 5)
                    //if (recordcount < 25)
                    {
                        var parameters = HttpUtility.ParseQueryString(string.Empty);
                        parameters.Add("api_key", apikey);
                        parameters.Add("phone", p.PhoneNumber);

                        simpleurl += parameters;

                        //call the GET function
                        string returnValue = GET(simpleurl);

                        //testing JSON object
                        //string returnValue = "{\"id\":\"Phone.623b6fef-a2e1-4b08-cfe3-bc7128b6dd92.Durable\",\"phone_number\":\"6153646365\",\"is_valid\":true,\"country_calling_code\":\"1\",\"line_type\":\"Mobile\",\"carrier\":\"AT&T\",\"is_prepaid\":null,\"is_commercial\":false,\"belongs_to\":[{\"id\":\"Person.799df873-d21a-34f8-8632-11050dbf42f4.Ephemeral\",\"name\":\"Brian Williams\",\"firstname\":\"Brian\",\"middlename\":null,\"lastname\":\"Williams\",\"age_range\":null,\"gender\":null,\"type\":\"Person\",\"link_to_phone_start_date\":null}],\"current_addresses\":[{\"id\":\"Location.4b3eb4ca-8d0d-435e-bcdc-2c36286b4144.Durable\",\"location_type\":\"ZipPlus4\",\"street_line_1\":null,\"street_line_2\":null,\"city\":\"Thompsons Station\",\"postal_code\":\"37179\",\"zip4\":\"9707\",\"state_code\":\"TN\",\"country_code\":\"US\",\"lat_long\":{\"latitude\":35.778226,\"longitude\":-86.883111,\"accuracy\":\"Street\"},\"is_active\":null,\"delivery_point\":null,\"link_to_person_start_date\":null}],\"historical_addresses\":[],\"associated_people\":[],\"alternate_phones\":[],\"error\":null,\"warnings\":[]}";

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

        static void OutputDatabaseSingle(UniquePhones p, string simpleurl)
        {
            SQLDataActions sqlDA = new SQLDataActions();

            if (p.Names.Count == 0)
            {
                sqlDA.InsertAPIReturnedData(p.PhoneNumber, p.InputName, "", null, null, null, null, null, simpleurl, p.JSON, sessionid);
            }
            else
            {
                for (int i = 0; i < p.Names.Count; i++)
                {
                    sqlDA.InsertAPIReturnedData(p.PhoneNumber, p.InputName, p.Names[i], p.LineType, p.IsCommercial, p.IsValid, p.AltPhonesCount, p.CurrentAddressesCount, simpleurl, p.JSON, sessionid);
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



        static void ParseResponse(string returnValue, UniquePhones p)
        {
            try
            {
                int smartcounter = 0;
                bool? isvalid = null;
                bool? iscommercial = null;
                string linetype = null;

                dynamic stuff = JsonConvert.DeserializeObject(returnValue);

                int personcount = stuff.belongs_to.Count;

                int i = 0;

                if (personcount > 0)
                {
                    foundcount++;

                    isvalid = Convert.ToBoolean(stuff["is_valid"]);
                    iscommercial = Convert.ToBoolean(stuff["is_commercial"]);
                    linetype = stuff["line_type"];

                    p.IsValid = isvalid;
                    p.IsCommercial = iscommercial;
                    p.LineType = linetype;
                }
                else
                {
                    notfoundcount++;
                }

                for (i = 0; i < personcount; i++)
                {
                    string name = stuff.belongs_to[i].name;
                    string nametype = stuff.belongs_to[i].type;

                    if (name.Length > 0)
                    {
                        p.Names.Add(name);
                    }
                }

                int addresscount = stuff.current_addresses.Count;

                for (int j = 0; j < addresscount; j++)
                {
                    string address = stuff.current_addresses[j].street_line_1 + " " + stuff.current_addresses[j].street_line_2 + " " + stuff.current_addresses[j].city + " " + stuff.current_addresses[j].state_code + " " + stuff.current_addresses[j].postal_code;
                    string myaddress = address;
                }

                int alternatephonescount = stuff.alternate_phones.Count;

                for (int k = 0; k < alternatephonescount; k++)
                {
                    string altphonenumber = stuff.alternate_phones[k];
                    string myaltphone = altphonenumber;
                }

                p.AltPhonesCount = alternatephonescount;
                p.CurrentAddressesCount = addresscount;

            }
            catch (Exception ex)
            {
                exceptiontype += "*Error occured on Record #" + recordcount.ToString() + "/" + ex.Message + "-" + ex.StackTrace;
                errorcount++;
            }
        }
    }
}
