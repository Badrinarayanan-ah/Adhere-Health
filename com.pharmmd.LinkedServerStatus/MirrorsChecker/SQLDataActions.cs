using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;


namespace MirrorsChecker
{
    public class SQLDataActions : DataLayer
    {
        public DataTable LinkedServerCleaner()
        {
            Cmd.Connection = Conn;
            Cmd.CommandText = "Utility.dbo.LinkedServerCleaner";
            Cmd.Parameters.Clear();
            Cmd.CommandType = CommandType.StoredProcedure;

            return ExecuteTableQuery(Cmd);
        }

        public DataTable GetMedProEnvironmentProductionIDs()
        {
            Cmd.Connection = Conn;
             Cmd.CommandText = "Utility.dbo.GetMedProEnvironmentIDs";
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@prodordev", 1);
            Cmd.CommandType = CommandType.StoredProcedure;

            return ExecuteTableQuery(Cmd);
        }


        public void SendEmailAlert(string toemailaddress, string body)
        {
            Cmd.Connection = Conn;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = "EXECUTE msdb.[dbo].[sp_send_dbmail]  @profile_name = 'SQLDW Peak 10'  ,  @recipients =  '" + toemailaddress + "',  @from_address='noreply@pharmmdm.com', @subject='SQLDW Linked Server Issue',@body='" + body + "'";
            Cmd.Parameters.Clear();
            ExecuteNonQuery(Cmd);
        }
        public void InsertCheckerRecord(int medproenvironmentid, bool successorfailure)
        {
            Cmd.Connection = Conn;
            Cmd.CommandText = "Utility.dbo.InsertLinkedServerTableSCANS";
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@medproenvironmentid", medproenvironmentid);
            Cmd.Parameters.AddWithValue("@successorfailure", successorfailure);
            Cmd.CommandType = CommandType.StoredProcedure;
            ExecuteNonQuery(Cmd);
        }

        public void ExecuteMirrorCheckSQL(string sqltext)
        {
            Cmd.Connection = Conn;
            Cmd.CommandText = sqltext;
            Cmd.Parameters.Clear();
            Cmd.CommandType = CommandType.Text;
            Cmd.ExecuteNonQuery();
        }
    }
}
