using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FillODSBasePhoneNumbers
{
    public class SQLDataActions : DataLayer
    {
        public DataTable GetWhitePagesClients()
        {
            Cmd.CommandText = "WhitePages.dbo.GetWhitePagesClients";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            return ExecuteTableQuery(Cmd);
        }

        public int FillODSBasePhoneNumbersFromWhitePages(int pmdclientid)
        {
            Cmd.CommandText = "WhitePages.dbo.FillODSBasePhoneNumbersFromWhitePages";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            return ExecuteScalarInt(Cmd);
        }

        public DataTable GetAllClients()
        {
            Cmd.CommandText = "WhitePages.dbo.GetWhitePagesClients";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            return ExecuteTableQuery(Cmd);
        }
    }
}
