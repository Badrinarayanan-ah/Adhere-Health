using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CheckRunAdHocCampaigns
{
    public class CheckRunAdHocCampaignsHelper
    {

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



        public static DateTime GetLastReadyForCampaignRun()
        {
            string connString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();

            DateTime dt = DateTime.MinValue;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("ODSBase.AdHoc_CheckLastReadyForCampaignRun", myConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        myConnection.Open();

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            if (DateTime.TryParse(result.ToString(), out dt))
                            {
                                return dt;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
            return dt;
        }

        public static DataTable GetCampaignNamesList()
        {
            string connString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();

            DataTable dt = new DataTable();

            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                myConnection.Open();

                using (SqlCommand cmd = new SqlCommand("ODSBase.GetAdhocCampaignNamesMaster", myConnection))
                {
                    var dataReader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dataReader);
                }

                myConnection.Close();
            }

            return dt;
        }

        public static DateTime GetLastRun()
        {
            DateTime lastRun = DateTime.MaxValue;
            string lastRunFilePath = ConfigurationManager.AppSettings["LastRunFilePath"].ToString().Trim();

            if (File.Exists(lastRunFilePath))
            {
                string content = File.ReadAllText(lastRunFilePath).Trim();
                DateTime tempDateTime = DateTime.MaxValue;
                if (DateTime.TryParse(content, out tempDateTime))
                {
                    return tempDateTime;
                }
            }
            return lastRun;
        }


        public static void SaveLastRun(DateTime valueToSave)
        {
            string lastRunFilePath = ConfigurationManager.AppSettings["LastRunFilePath"].ToString().Trim();

            if (!Directory.Exists(Path.GetDirectoryName(lastRunFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(lastRunFilePath));

            File.WriteAllText(lastRunFilePath, valueToSave.ToString());
        }

        public static bool CheckCampaignsShouldRun()
        {
            DateTime lastReady = GetLastReadyForCampaignRun();
            DateTime lastRun = GetLastRun();
            if (lastReady > lastRun)
                return true;
            else
                return false;
        }


        //public static void UpdateCampaignQueueStatus(int campaignid, int statusid)
        public static void UpdateCampaignQueueStatus(int id, int statusid)
        {
            string connString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();

            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                myConnection.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.spUpdateCampaignQueueStatus", myConnection))
                {
                    cmd.Parameters.Clear();
                    //cmd.Parameters.AddWithValue("@campaignid", campaignid);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@statusid", statusid);
                    cmd.ExecuteNonQuery();
                }

                myConnection.Close();
            }
        }
        public static DataTable GetQueuedCampaignNamesList()
        {
            string connString = ConfigurationManager.ConnectionStrings["Compliance"].ToString().Trim();

            DataTable dt = new DataTable();

            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                myConnection.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.qryAdhocCampaignQueueForProcessing", myConnection))
                {
                    var dataReader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dataReader);
                }

                myConnection.Close();
            }

            return dt;
        }


    }
}
