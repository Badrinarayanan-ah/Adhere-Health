using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;

namespace TheWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            bool dorun = true;

            if (args.Length == 0)
            {
            }
            else if (args.Length > 0)
            {
                foreach (string s in args)
                {
                    if (s == "?")
                    {
                        dorun = false;
                    }
                }
            }

            if (dorun)
            {
                //string emailtonotify = System.Configuration.ConfigurationManager.AppSettings["TheWatcher.Settings1.EmailNotificationAddress"].ToString();
                string emailtonotify = Settings1.Default.EmailNotificationAddress.ToString();

                try
                {
                    Emailing eme = new Emailing();

                    Console.Out.WriteLine("Started at: " + DateTime.Now.ToString());

                    SQLActions sa = new SQLActions();
                    DataTable activeTables = sa.SelectActiveConfigurations();

                    int totalcounter = activeTables.Rows.Count;

                    int goodcount = 0;
                    int badcount = 0;

                    foreach (DataRow row in activeTables.Rows)
                    {
                        TableTable t = CommonFunctions.LoadTableRowIntoObject(row);
                        Console.Out.WriteLine("Processing Configuration Row# " + t.RowID.ToString());
                        Console.Out.WriteLine("");


                        List<int> lstSKipDays = CommonFunctions.DaysToSkipFromSkipDays(t.DaysToSkip);

                        int day1 = (int)DateTime.Now.DayOfWeek;

                        if (lstSKipDays.Count == 0 || !(lstSKipDays.Contains(day1)))
                        {
                            bool didwork = sa.CheckDataValues(t.TableName, t.ColumnName, t.DatabaseName, t.SchemaName, t.ServerName, t.DateInterval, t.ModeID, t.SQL);

                            if (didwork)
                            {
                                goodcount++;
                                sa.UpdateLastChecked(t.RowID);
                            }
                            else
                            {
                                badcount++;

                                try
                                {
                                    string body = string.Empty;

                                    //mode switching!
                                    if (t.ModeID == 1)
                                    {
                                        body = "Table Last Update Failure: " + CommonFunctions.ReturnFormattedColumnName(t.ServerName, t.DatabaseName, t.SchemaName, t.TableName, t.ColumnName) + " has not been updated in the last ";
                                        body += t.DateInterval.ToString() == "1" ? "1 day" : t.DateInterval.ToString() + "+ days";


                                        if (t.DaysSinceLastVerified != null)
                                        {
                                            body += System.Environment.NewLine;
                                            body += System.Environment.NewLine;

                                            int dayssincelastverified = Convert.ToInt32(t.DaysSinceLastVerified) + 1;

                                            /*
                                            if (dayssincelastverified == 1)
                                            {
                                                body += "The last record update was " + dayssincelastverified.ToString() + " day ago";
                                            }
                                            else
                                            {
                                                body += "The last record update was " + dayssincelastverified.ToString() + " days ago";
                                            }
                                            */
                                        }

                                    }
                                    else if(t.ModeID==2)
                                    {
                                        body = "Table Data Check Failure: " + CommonFunctions.ReturnFormattedColumnName(t.ServerName, t.DatabaseName, t.SchemaName, t.TableName, t.ColumnName) + " does not have a valid data record count.";
                                    }
                                    else if (t.ModeID==3)
                                    {
                                        body = "Table Data Alert (invalid data in a data column/row): " + CommonFunctions.ReturnFormattedColumnName(t.ServerName, t.DatabaseName, t.SchemaName, t.TableName, t.ColumnName) + " - SQL Statement: " + t.SQL;
                                        //body = "Table Data Alert (invalid data in a data column/row). Check configuration table for SQL statement. Configuration Row ID:" + t.RowID.ToString() + " - " + CommonFunctions.ReturnFormattedColumnName(t.ServerName, t.DatabaseName, t.SchemaName, t.TableName, t.ColumnName);
                                    }

                                    body += System.Environment.NewLine;
                                    body += System.Environment.NewLine;
                                    body += System.Environment.NewLine;

                                    Console.Out.WriteLine(body);
                                    Console.Out.WriteLine("");

                                    eme.SendEmail("noreply@pharmmd.com", emailtonotify, "PharmMD Watcher Email Failure", body);
                                }
                                catch (Exception ex)
                                {
                                    Console.Out.WriteLine("Email notification failed.." + ex.Message + " - " + ex.StackTrace);
                                }
                            }
                        }
                        else
                        {
                            //it's a skip day
                        }

                    }

                    Console.Out.WriteLine("Ended at: " + DateTime.Now.ToString());

                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine(totalcounter.ToString() + " total configurations checked...");
                    Console.Out.WriteLine(goodcount.ToString() + " valid configurations found...");
                    Console.Out.WriteLine(badcount.ToString() + " invalid configurations found...");
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("");

                    if (badcount > 0)
                    {
                        Console.Out.WriteLine("The application will exit in 15 seconds...");
                        Console.Out.WriteLine("");

                        System.Threading.Thread.Sleep(15000);
                    }
                    else
                    {
                        Console.Out.WriteLine("The application will exit in 5 seconds...");
                        Console.Out.WriteLine("");

                        System.Threading.Thread.Sleep(5000);
                    }

                    System.Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("An error occurred:" + ex.Message + " - " + ex.StackTrace);
                    System.Threading.Thread.Sleep(5000);

                    System.Environment.Exit(1);
                }
            }
            else
            {
                Console.Out.Write(HelpText());

                System.Environment.Exit(0);
            }
        }


        static string HelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This application does not take input parameters. It does utilize the Utilities DW on SQLDW");
            sb.AppendLine("The whole point of this program is to check date values on certain key tables within the enterprise to use the _createdat or _updatedat (or other similary named columns) on tables to make sure they are getting loaded as they should.");
            sb.AppendLine(@"You can pass in a parameter of ? to get to this help feature");
            sb.AppendLine("-------------");
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
