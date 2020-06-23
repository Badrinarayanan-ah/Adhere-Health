using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WhitePagesReversePhoneAPI
{
    public class SQLDataActions: DataLayer
    {
        public DataTable ReturnWhitePagesListByClient(int pmdclientid)
        {
            Cmd.CommandText = "WhitePages.dbo.ReturnListByClient";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            return ExecuteTableQuery(Cmd);
        }
            

        public void InsertAPIReturnedData(string phonenumber, string inputname, string foundname, string linetype, bool? iscommercial, bool? isvalid, int? altphonescount, int? currentaddressescount, string request, string json, int sessionid)
        {
            Cmd.CommandText = "WhitePages.dbo.InsertAPIReturnedDataPhones";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@phonenumber", phonenumber);
            Cmd.Parameters.AddWithValue("@inputname", inputname);
            Cmd.Parameters.AddWithValue("@foundname", foundname);
            Cmd.Parameters.AddWithValue("@linetype", linetype);
            Cmd.Parameters.AddWithValue("@iscommercial", iscommercial);
            Cmd.Parameters.AddWithValue("@isvalid", isvalid);
            Cmd.Parameters.AddWithValue("@altphonescount", altphonescount);
            Cmd.Parameters.AddWithValue("@currentaddressescount", currentaddressescount);
            Cmd.Parameters.AddWithValue("@json", json);
            Cmd.Parameters.AddWithValue("@request", request);
            Cmd.Parameters.AddWithValue("@sessionid", sessionid);
            ExecuteNonQuery(Cmd);
        }
        
        public int InsertAPISession(DateTime startdate)
        {
            Cmd.CommandText = "WhitePages.dbo.InsertAPISessionPhones";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@startdate", startdate);
            return ExecuteScalarInt(Cmd);
        }

        public void UpdateAPISession(int sessionid, DateTime enddate)
        {
            Cmd.CommandText = "WhitePages.dbo.UpdateAPISessionPhones";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@sessionid", sessionid);
            Cmd.Parameters.AddWithValue("@enddate", enddate);
            ExecuteNonQuery(Cmd);
        }
    }
}
