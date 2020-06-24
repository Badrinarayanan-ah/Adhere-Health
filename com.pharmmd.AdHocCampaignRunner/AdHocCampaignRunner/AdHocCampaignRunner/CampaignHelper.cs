using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Data;


namespace AdHocCampaignRunner
{
    public class CampaignHelper
    {
        public static void RunCampaign(string campaignName, int isnewcluster)
        {
            //string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[archiveAdHocCallList] @campaignName = '{0}'", campaignName);
            //string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[archiveAdHocCallList] @campaignName = '{0}', @dialerTable = '{1}' , @dialerTableHx = '{2]'", new string[] { campaignName, dialertable, dialertablehx });
            string command = String.Format(@"EXECUTE [ODSBase].[AdHoc_RunCampaign] '" + campaignName + "','" + isnewcluster + "'");

            string showthiscommand = "";

            ExecuteComplianceQuery(command);

        }

        public static void ArchiveCampaign(string campaignName, string dialertable, string dialertablehx)
        {
            //string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[archiveAdHocCallList] @campaignName = '{0}'", campaignName);
            //string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[archiveAdHocCallList] @campaignName = '{0}', @dialerTable = '{1}' , @dialerTableHx = '{2]'", new string[] { campaignName, dialertable, dialertablehx });
            string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[archiveAdHocCallList] '" + campaignName + "','" + dialertable + "','" + dialertablehx + "'");

            string showthiscommand = "";

            ExecuteCallCampaignsQuery(command);

        }

        public static void ExecuteCallCampaignsQuery(string commandString)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CallCampaigns"].ToString().Trim();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.CommandTimeout = 1500;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

        }


        public static void StartLoggingExecutionStatus(string packagename, DateTime? successdate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ETLCore"].ToString().Trim();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"Utilities.[dbo].[UpsertSSISExecutionStatus]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@packagename", packagename);
                command.Parameters.AddWithValue("@successdate", successdate);
                command.CommandTimeout = 1500;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public static DateTime GetStartOfRunExecutionStatus(string packagename)
        {
            DateTime outputDT = Convert.ToDateTime("9/9/9999");

            string connectionString = ConfigurationManager.ConnectionStrings["ETLCore"].ToString().Trim();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"Utilities.[dbo].[SelectSSISExecutionsByPackageName]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@packagename", packagename);
                command.CommandTimeout = 1500;
                command.Connection.Open();

                var dataReader = command.ExecuteReader();

                var dataTable = new DataTable();
                dataTable.Load(dataReader);

                outputDT = Convert.ToDateTime(dataTable.Rows[0]["startofrun"]);
            }

            return outputDT;
        }


        public static DataTable GetAdHocLog()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();

            var dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select rowid, description from ODSBase.AdHocLog", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Clear();
                command.CommandTimeout = 1500;
                command.Connection.Open();

                var dataReader = command.ExecuteReader();

                dataTable.Load(dataReader);
            }

            return dataTable;
        }

        public static void ExecuteRollupUpdates()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"ODSBase.AdHoc_RollupUpdateAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@checkpreviousrun", "true");
                command.CommandTimeout = 1500;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public static void ExecuteComplianceQuery(string commandString)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(commandString, connection);
                command.CommandTimeout = 1500;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public static getAdHocMostRecentSession_Result GetMostRecentSession()
        {
            CallCampaignsEntities cce = new CallCampaignsEntities();
            return cce.getAdHocMostRecentSession().FirstOrDefault();
        }



        public static void CreateCallList(string campaignName, int sessionId)
        {
            string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[createAdHocCallList]  @campaignName = '{0}', @callListSessionId = {1}", campaignName, sessionId);
            ExecuteCallCampaignsQuery(command);
        }

        public static void RunPostDeployActions()
        {
            Console.WriteLine("Starting Post-Deploy Cleanup Actions");
            string command = String.Format(@"EXECUTE [CallCampaigns].[dbo].[CallListPostDeployActions]  null");
            ExecuteCallCampaignsQuery(command);

        }

        //original did not have the 2 additional parameters
        public static string RunCampaign(string campaignName)
        {
            return "original";
        }

        public static string RunCampaign(string campaignName, string dialertable, string dialertablehx)
        {
            Console.WriteLine(campaignName);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            CampaignHelper.ArchiveCampaign(campaignName, dialertable, dialertablehx);
            ComplianceEntities ce = new ComplianceEntities();


            int isnewcluster = 1;
            CampaignHelper.RunCampaign(campaignName, isnewcluster);


            getAdHocMostRecentSession_Result item = CampaignHelper.GetMostRecentSession();

            CampaignHelper.CreateCallList(campaignName, item.CallListSessionId);

            StringBuilder sb = new StringBuilder();

            List<AdHocLog> logs = CampaignHelper.GetLog();

            foreach (AdHocLog logItem in logs)
            {
                if (logItem.description != null)
                {
                    if (logItem.description.Trim().Length == 0)
                    {
                        sb.AppendLine("&nbsp;<br>");
                    }
                    else
                    {
                        sb.AppendLine(String.Format("{0}<br>", logItem.description.Trim()));
                    }
                }
            }

            sb.AppendLine(@"</p>");

            //Console.WriteLine(CampaignHelper.CreateFile(CampaignDefs.Campaigns[campaignName].DialerTable, url));


            Console.WriteLine(sb.ToString());
            File.AppendAllText(@"C:\temp\output.txt", sb.ToString());

            File.AppendAllText(@"C:\temp\output.txt", "");
            File.AppendAllText(@"C:\temp\output.txt", "");
            File.AppendAllText(@"C:\temp\output.txt", "");
            File.AppendAllText(@"C:\temp\output.txt", "");

            //CreateFile(CampaignDefs.Campaigns[campaignName].DialerTable, excelFilePath);
            return sb.ToString();
        }

        public static List<AdHocLog> GetLog()
        {
            List<AdHocLog> lst = new List<AdHocLog>();

            DataTable dt = CampaignHelper.GetAdHocLog();

            foreach (DataRow dr in dt.Rows)
            {
                AdHocLog al = new AdHocLog();

                al.rowId = Convert.ToInt32(dr["rowid"]);
                al.description = dr["description"].ToString();

                lst.Add(al);
            }


            return lst;
        }

        public static void SaveHTMLOutput(string output)
        {
            string filePath = ConfigurationManager.AppSettings["EmailOutputBufferFilePath"];

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath))
            {
                DateTime fileDate = File.GetLastWriteTime(filePath).Date;

                if (fileDate != DateTime.Now.Date)
                    File.Delete(filePath);

            }

            File.AppendAllText(filePath, output);
        }

        public static List<string> ReadHTMLOutputFile()
        {
            List<string> result = new List<string>();
            string filePath = ConfigurationManager.AppSettings["EmailOutputBufferFilePath"];

            if (File.Exists(filePath))
            {
                result = File.ReadAllLines(filePath).ToList();
            }

            return result;
        }

        public static void SendEmail()
        {
            string emailContent = string.Empty;
            string recipientsTo = string.Empty;
            string recipientsCC = string.Empty;
            MailMessage mail = null;
            SmtpClient client = null;
            List<string> values = null;

            StringBuilder sb = new StringBuilder();

            foreach (string s in ReadHTMLOutputFile())
                sb.Append(s);

            emailContent = sb.ToString();

            recipientsTo = ConfigurationManager.AppSettings["RecipientTo"].ToString().Trim();
            recipientsCC = ConfigurationManager.AppSettings["RecipientCC"].ToString().Trim();
            bool runInDebugMode = (ConfigurationManager.AppSettings["RunInDebugMode"].ToString().Trim() == "true") ? true : false;


            //mail = new MailMessage("no-reply@adherehealth.com", "brian.williams@adherehealth.com", String.Format("MTM 'New Edition' Call Lists for {0}", DateTime.Now.ToString("MM-dd")), emailContent);
            mail = new MailMessage("noreply@pharmmd.com", "brian.williams@adherehealth.com", String.Format("MTM 'New Edition' Call Lists for {0}", DateTime.Now.ToString("MM-dd")), emailContent);
            //mail.From = new MailAddress("noreply@pharmmd.com");

            mail.IsBodyHtml = true;


            if (runInDebugMode)
            {
                Console.WriteLine("In debug mode.");
            }
            else
            {
                foreach (string s in recipientsTo.Split(','))
                {
                    if (s != null)
                    {
                        if (s.Length > 0)
                        {
                            mail.To.Add(s);
                        }
                    }
                }

                foreach (string s in recipientsCC.Split(','))
                {
                    if (s != null)
                    {
                        if (s.Length > 0)
                        {
                            mail.CC.Add(s);
                        }
                    }
                }
            }

            SmtpClient sc = new SmtpClient();
            sc.Send(mail);
        }


    }


}
