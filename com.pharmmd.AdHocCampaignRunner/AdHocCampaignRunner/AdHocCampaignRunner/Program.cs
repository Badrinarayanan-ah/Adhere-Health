using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using CheckRunAdHocCampaigns;

namespace AdHocCampaignRunner
{
    class Program
    {
        //we do NOT want to refresh campaigns after 7 AM on Monday - Friday
        static bool overridestarttimecheck = false;

        static int goodexitcode = 0;
        static int badexitcode = 1;

        static int queuedstatusid = 1;
        static int processingstatusid = 2;
        static int completedstatusid = 3;

        static void NewMain(string[] args)
        {
            string singlecampaignname = "";
            bool usesinglecampaign = false;
            bool runqueue = false;

            try
            {
                //check for an argument that starts with "CAMPAIGNNAME"
                foreach (string s in args)
                {
                    if (s.ToUpper().Trim().StartsWith("/CAMPAIGNNAME:"))
                    {
                        singlecampaignname = s.ToUpper().Trim().Replace("/CAMPAIGNNAME:", "");
                        usesinglecampaign = true;
                    }
                    else if (s.ToUpper().StartsWith("/TIMEOVERRIDE:"))
                    {
                        overridestarttimecheck = true;
                    }
                    else if (s.ToUpper().StartsWith("/QUEUE:"))
                    {
                        runqueue = true;
                    }
                }

                int campaignsthatrancount = 0;

                string htmlOutput = string.Empty;

                if (!runqueue)
                {
                    //run if override is on OR the time is before 7 AM (call center hours)
                    if ((overridestarttimecheck == true) || (overridestarttimecheck == false && (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)) || (overridestarttimecheck == false && DateTime.Now.Hour < 7 && (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Friday)))
                    {
                        Console.Out.WriteLine("Starting rollup updates...");

                        CampaignHelper.ExecuteRollupUpdates();

                        //run all campaigns from master base table

                        DataTable dt = CheckRunAdHocCampaignsHelper.GetCampaignNamesList();

                        foreach (DataRow dr in dt.Rows)
                        {
                            bool isactive = Convert.ToBoolean(dr["isactive"].ToString());
                            string campaignname = dr["campaignname"].ToString();
                            string daystorun = dr["skipdays"].ToString();
                            string dialertable = dr["dialertable"].ToString();
                            string dialertablehx = dr["dialertablehx"].ToString();
                            bool donotrun = Convert.ToBoolean(dr["donotrun"].ToString());

                            if (donotrun)
                            {
                                //what are you dumb? this variable says DO NOT RUN!
                            }
                            else
                            {
                                //only run if active campaign (or it's a single use campaign from the parameter)
                                if (isactive || usesinglecampaign)
                                {
                                    //check the table configuration skip days from the C# enumerator (1-7  versus 0-6 cause I was lazy about it)
                                    //Sunday(1) to Saturday(7)
                                    int dayofweektoday = (int)DateTime.Now.DayOfWeek + 1;

                                    if (usesinglecampaign)
                                    {
                                        if (campaignname.ToLower().Trim() == singlecampaignname.ToLower().Trim())
                                        {
                                            htmlOutput = CampaignHelper.RunCampaign(campaignname, dialertable, dialertablehx);
                                            CampaignHelper.SaveHTMLOutput(htmlOutput);
                                            campaignsthatrancount++;
                                        }
                                    }
                                    else
                                    {
                                        if (daystorun.Contains(dayofweektoday.ToString()))
                                        {
                                            htmlOutput = CampaignHelper.RunCampaign(campaignname, dialertable, dialertablehx);
                                            CampaignHelper.SaveHTMLOutput(htmlOutput);
                                            campaignsthatrancount++;
                                        }
                                    }
                                }
                            }
                        }

                        //htmlOutput = CampaignHelper.RunCampaign(args[0]);
                        //CampaignHelper.SaveHTMLOutput(htmlOutput);

#if RELEASE
                        if (campaignsthatrancount > 0)
                        {
                            CampaignHelper.RunPostDeployActions();
                        }
#elif Release
                        if (campaignsthatrancount > 0)
                        {
                            CampaignHelper.RunPostDeployActions();
                        }
#endif

                        //attach this to this section
                        Console.Out.WriteLine("Starting email transmission...");

                        //CampaignHelper.SendEmail();
                    }
                    else
                    {
                        Console.Out.WriteLine("Start Time is too late (post 7 AM): " + DateTime.Now.ToString());
                    }
                }
                else
                {
                    //run only the queued campaigns....
                    Console.Out.WriteLine("Starting rollup updates...");

                    CampaignHelper.ExecuteRollupUpdates();

                    DataTable dt = CheckRunAdHocCampaignsHelper.GetQueuedCampaignNamesList();

                    //do some more stuff
                    foreach (DataRow dr in dt.Rows)
                    {
                        string campaignname = dr["campaignname"].ToString();
                        string dialertable = dr["dialertable"].ToString();
                        string dialertablehx = dr["dialertablehx"].ToString();
                        int campaignid = Convert.ToInt32(dr["campaignid"].ToString());
                        int id = Convert.ToInt32(dr["id"].ToString());

                        //mark campaign as processing...
                        CheckRunAdHocCampaignsHelper.UpdateCampaignQueueStatus(id, processingstatusid);

                        htmlOutput = CampaignHelper.RunCampaign(campaignname, dialertable, dialertablehx);
                        CampaignHelper.SaveHTMLOutput(htmlOutput);

                        //mark campaign as completed
                        CheckRunAdHocCampaignsHelper.UpdateCampaignQueueStatus(id, completedstatusid);

                        campaignsthatrancount++;
                    }

                    if (campaignsthatrancount > 0)
                    {
                        CampaignHelper.RunPostDeployActions();
                    }

                    //attach this to this section
                    Console.Out.WriteLine("Starting email transmission...");

                    CampaignHelper.SendEmail();


                }
                Console.WriteLine();
                Console.WriteLine();

                //Console.Write("Press any key to exit...");
                //Console.ReadKey();

                Console.Out.WriteLine(goodexitcode);
                System.Environment.Exit(goodexitcode);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("An error has occurred. Please check the message below.");
                Console.Out.WriteLine("");

                Console.Out.WriteLine(ex.Message + " - " + ex.StackTrace);
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");


                //display some help text
                HelpText();

                Console.Out.WriteLine("The application will exit in 30 seconds....");

                System.Threading.Thread.Sleep(30000);

                Console.Out.WriteLine(badexitcode);
                System.Environment.Exit(badexitcode);

            }
        }

        static void HelpText()
        {
            string output = "";

            Console.Out.WriteLine("Allowable campaigns are: ");

            DataTable dt = CheckRunAdHocCampaignsHelper.GetCampaignNamesList();

            foreach (DataRow row in dt.Rows)
            {
                output += "Campaign:" + row["campaignname"].ToString() + "/Active: " + row["isactive"].ToString();
                output += System.Environment.NewLine;
            }

            output += System.Environment.NewLine;
            output += System.Environment.NewLine;
            output += "Available Parameters are: ";
            output += System.Environment.NewLine;
            output += "/TIMEOVERRIDE: (by default, we will not let this process run after 7 AM (Monday - Friday) as it could affect operations. This flag overrides that setting (use with caution)";
            output += System.Environment.NewLine;

            output += System.Environment.NewLine;
            output += System.Environment.NewLine;

            Console.Out.WriteLine(output);
            Console.Out.WriteLine();
            Console.Out.WriteLine();

        }
        static void Main(string[] args)
        {
            string packagename = System.Configuration.ConfigurationSettings.AppSettings["PackageName"].ToString();

            //string packagename="MTM Adhoc Campaign Runner";

            /*
#if RELEASE
        */

            //CampaignHelper.StartLoggingExecutionStatus(packagename, null);
            /*
#elif Release
        */

            //CampaignHelper.StartLoggingExecutionStatus(packagename, null);
            /** 
#endif
            */

            //passing in a campaign name will bypass this
            NewMain(args);


            //CampaignHelper.StartLoggingExecutionStatus(packagename, DateTime.Now);
        }


        /*
        static void OldMain(string[] args)
        {
            string htmlOutput = string.Empty;

            if ((args.Length > 0) && (args[0] != @"SendMail") && (args[0] != @"UpdateRollups"))
            {
                htmlOutput = CampaignHelper.RunCampaign(args[0], "", "");
                CampaignHelper.SaveHTMLOutput(htmlOutput);

            }

            if ((args.Length > 0) && (args[0] == @"SendMail"))
            {
                CampaignHelper.SendEmail();
            }

            if ((args.Length > 0) && (args[0] == @"UpdateRollups"))
            {
                CampaignHelper.ExecuteRollupUpdates();
            }




            Console.WriteLine();
            Console.WriteLine();

            //Console.Write("Press any key to exit...");
            //Console.ReadKey();

        }
        */
    }


}
