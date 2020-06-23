using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WhitePagesFindBusinessAPI
{
    public class SQLDataActions: DataLayer
    {
        public DataTable ReturnWhitePagesListByProvider()
        {
            Cmd.CommandText = "WhitePages.dbo.ReturnListByProvider";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            return ExecuteTableQuery(Cmd);
        }
            

        public void InsertAPIReturnedData(string dob, string name, string address1, string address2, string city, string state, string postalcode, string phonenumber, string linetype,string json,string request, int sessionid, string npi)
        {
            Cmd.CommandText = "WhitePages.dbo.InsertAPIReturnedDataProviders";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@dob", dob);
            Cmd.Parameters.AddWithValue("@name", name);
            Cmd.Parameters.AddWithValue("@address1", address1);
            Cmd.Parameters.AddWithValue("@address2", address2);
            Cmd.Parameters.AddWithValue("@city", city);
            Cmd.Parameters.AddWithValue("@state", state);
            Cmd.Parameters.AddWithValue("@postalcode", postalcode);
            Cmd.Parameters.AddWithValue("@phonenumber", phonenumber);
            Cmd.Parameters.AddWithValue("@linetype", linetype);
            Cmd.Parameters.AddWithValue("@json", json);
            Cmd.Parameters.AddWithValue("@request", request);
            Cmd.Parameters.AddWithValue("@sessionid", sessionid);
            Cmd.Parameters.AddWithValue("@npi", npi);
            ExecuteNonQuery(Cmd);
        }


        public int InsertAPISession(DateTime startdate, int pmdclientid)
        {
            Cmd.CommandText = "WhitePages.dbo.InsertAPISessionProviders";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@startdate", startdate);
            return ExecuteScalarInt(Cmd);
        }

        public void UpdateAPISession(int sessionid, DateTime enddate)
        {
            Cmd.CommandText = "WhitePages.dbo.UpdateAPISessionProviders";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@sessionid", sessionid);
            Cmd.Parameters.AddWithValue("@enddate", enddate);
            ExecuteNonQuery(Cmd);
        }
    }
}
