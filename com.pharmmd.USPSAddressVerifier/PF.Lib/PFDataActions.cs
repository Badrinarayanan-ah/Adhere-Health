using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Lib
{
    public class PFDataActions: SQLDataLayer
    {
        public DataTable SelectSentAddresses()
        {
            Cmd.CommandText = "SelectSentAddresses";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            return ExecuteTableQuery(Cmd);
        }

        public DataTable SelectSentAddressesVerified()
        {
            Cmd.CommandText = "SelectSentAddressesVerified";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            return ExecuteTableQuery(Cmd);
        }


        public DataTable SelectSentAddressesUnVerified()
        {
            Cmd.CommandText = "SelectSentAddressesUnVerified";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            return ExecuteTableQuery(Cmd);
        }

        public void VerifyAddress(string address)
        {
            Cmd.CommandText = "VerifyAddress";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@address", address);

            ExecuteNonQuery(Cmd);
        }

        public void UpdateAddressReturnedAddress(string address, string returnedaddress)
        {
            Cmd.CommandText = "UpdateAddressReturnedAddress";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@address", address);
            Cmd.Parameters.AddWithValue("@returnedaddress", returnedaddress);

            ExecuteNonQuery(Cmd);
        }

        public void InsertAddress(string address, string address1, string address2, string city,string state, string zip, string returnedaddress, bool verified, string returnedaddress1, string returnedaddress2, string returnedcity, string returnedstate, string returnedzip, long accountid)
        {
            Cmd.CommandText = "InsertAddress";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@address", address);
            Cmd.Parameters.AddWithValue("@address1", address1);
            Cmd.Parameters.AddWithValue("@address2", address2);
            Cmd.Parameters.AddWithValue("@city", city);
            Cmd.Parameters.AddWithValue("@state", state);
            Cmd.Parameters.AddWithValue("@zip", zip);
            Cmd.Parameters.AddWithValue("@returnedaddress", returnedaddress);
            Cmd.Parameters.AddWithValue("@verified", verified);
            Cmd.Parameters.AddWithValue("@returnedaddress1", returnedaddress1);
            Cmd.Parameters.AddWithValue("@returnedaddress2", returnedaddress2);
            Cmd.Parameters.AddWithValue("@returnedcity", returnedcity);
            Cmd.Parameters.AddWithValue("@returnedstate", returnedstate);
            Cmd.Parameters.AddWithValue("@returnedzip", returnedzip);
            Cmd.Parameters.AddWithValue("@accountid", accountid);
            ExecuteNonQuery(Cmd);
        }

        public void ClearSentAddressesUnverified()
        {
            Cmd.CommandText = "ClearSentAddressesUnverified";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            ExecuteNonQuery(Cmd);
        }

        public void ClearSentAddresses()
        {
            Cmd.CommandText = "ClearSentAddresses";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();

            ExecuteNonQuery(Cmd);
        }

        
    }
}
