using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;

namespace TheWatcher
{
    public class SQLActions : DataLayer
    {
        public bool CheckDataValues(string tablename, string columnname, string databasename, string schemaname, string servername, int? dateinterval, int modeid, string sql)
        {
            Cmd.CommandText = "Watcher.CheckTableValues";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@tablename", tablename);
            Cmd.Parameters.AddWithValue("@columnname", columnname);
            Cmd.Parameters.AddWithValue("@databasename", databasename);
            Cmd.Parameters.AddWithValue("@schemaname", schemaname);
            Cmd.Parameters.AddWithValue("@servername", servername);
            Cmd.Parameters.AddWithValue("@dateinterval", dateinterval);
            Cmd.Parameters.AddWithValue("@modeid", modeid);
            Cmd.Parameters.AddWithValue("@sql", sql);

            DataTable dt = ExecuteTableQuery(Cmd);

            //if we do not have rows within this time range, then FAIL!
            if (modeid == 1 || modeid == 2)
            {
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    //int returncount = Convert.ToInt32(dt.Rows[0]["reportcount"]);
                    int returncount = Convert.ToInt32(dt.Rows[0]["outputcount"]);

                    if (returncount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (modeid == 3)
            {
                int returncount = Convert.ToInt32(dt.Rows[0]["outputcount"]);

                if (returncount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        public void SendEmail(string fromemailaddress, string toemailaddress, string body, string subject)
        {
            Cmd.CommandText = "msdb.[dbo].[sp_send_dbmail]";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            body += System.Environment.NewLine + System.Environment.NewLine + EndingBody() + System.Environment.NewLine + System.Environment.NewLine;

            Cmd.Parameters.AddWithValue("@profile_name", "SQLDW Peak 10");
            Cmd.Parameters.AddWithValue("@recipients", toemailaddress);
            Cmd.Parameters.AddWithValue("@from_address", fromemailaddress);
            Cmd.Parameters.AddWithValue("@subject", subject);
            Cmd.Parameters.AddWithValue("@body", body);


            ExecuteNonQuery(Cmd);
        }

        public string EndingBody()
        {
            string output = string.Empty;

            output = "This alert could mean that the Mirrors completed later than the time 'The Watcher' was run for this day. It also could mean that a piece of the MTM Event Chain (UpdateComplianceBaseTables, Offers, etc) completed later than normal. This will take some Operational knowledge and research to troubleshoot but hopefully this can be shared throughout the Data Team.";
            return output;
        }
        public void UpdateLastChecked(int rowid)
        {
            Cmd.CommandText = "Watcher.UpdateLastChecked";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@rowid", rowid);

            ExecuteNonQuery(Cmd);
        }

        public int InsertConfiguration(string tablename, string columnname)
        {
            Cmd.CommandText = "Watcher.InsertConfiguration";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@tablename", tablename);
            Cmd.Parameters.AddWithValue("@columnname", columnname);

            return ExecuteScalarInt(Cmd);
        }

        public DataTable SelectActiveConfigurations()
        {
            Cmd.CommandText = "Watcher.GetActiveConfigurations";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            return ExecuteTableQuery(Cmd);
        }
    }
}
