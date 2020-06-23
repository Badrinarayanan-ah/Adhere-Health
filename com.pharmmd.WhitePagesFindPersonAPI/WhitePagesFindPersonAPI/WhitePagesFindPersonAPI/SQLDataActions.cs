using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WhitePagesFindPersonAPI
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

        public DataTable ReturnWhitePagesListByClient(int pmdclientid)
        {
            Cmd.CommandText = "WhitePages.dbo.ReturnListByClient";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            return ExecuteTableQuery(Cmd);
        }


        public void InsertAPIReturnedData(string pmdpatientid, string pmdclientid, string dob, string name, string address1, string address2, string city, string state, string postalcode, string phonenumber, string linetype, string json, string request, string contractnumber, int sessionid)
        {
            if (phonenumber.Length > 0)
            {
                phonenumber = phonenumber.Replace("+", "").Replace("-", "").Replace(".", "");
            }

            if(phonenumber.Length>0)
            {
                if(phonenumber.Substring(1,1)=="1")
                {
                    phonenumber = phonenumber.TrimStart(new char[] { '1' });
                }
            }


            Cmd.CommandText = "WhitePages.dbo.InsertAPIReturnedData";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdpatientid", pmdpatientid);
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
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
            Cmd.Parameters.AddWithValue("@contractnumber", contractnumber);
            Cmd.Parameters.AddWithValue("@sessionid", sessionid);
            ExecuteNonQuery(Cmd);
        }

        public DataTable ReturnDemographicsDataPostGres(int pmdclientid)
        {
            Cmd.CommandText = "Compliance.dbo.usp_rpt_PatientDemographicsPostGres";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            Cmd.Parameters.AddWithValue("@ExcludeIfInWhitePagesFeed", "true");
            Cmd.Parameters.AddWithValue("@HasCMRThisYear", "true");
            return ExecuteTableQuery(Cmd);
        }

        public DataTable ReturnDemographicsDataODS(int pmdclientid)
        {
            Cmd.CommandText = "Compliance.dbo.usp_rpt_PatientDemographicsODSBase";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            Cmd.Parameters.AddWithValue("@ExcludeIfInWhitePagesFeed", "true");
            Cmd.Parameters.AddWithValue("@HasCMRThisYear", "true");
            //turning wrong number stuff off for now
            //Cmd.Parameters.AddWithValue("@IncludeWrongNumberPeople", "true");
            Cmd.Parameters.AddWithValue("@IncludeWrongNumberPeople", "false");
            Cmd.Parameters.AddWithValue("@IncludeETL", "true");
            //optional and stuff
            //Cmd.Parameters.AddWithValue("@ClearBaseCMR", "true");
            return ExecuteTableQuery(Cmd);
        }


        public DataTable ReturnPatientsWithoutPhoneNumbers(int pmdclientid, bool? ismtm)
        {
            Cmd.CommandText = "Compliance.Reporting.PatientsInActivePopulationWithoutPhoneNumbers";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            Cmd.Parameters.AddWithValue("@inwhitepages", false);
            Cmd.Parameters.AddWithValue("@ismtm", ismtm);
            return ExecuteTableQuery(Cmd);
        }

        public DataTable ReturnPatientsManualListCreatedToday(int pmdclientid)
        {
            
            Cmd.CommandText = "WhitePages.dbo.ReturnWhitePagesManualListPatients";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            return ExecuteTableQuery(Cmd);
        }

        public DataTable ReturnETLCoreWithoutPhoneNumbers(int pmdclientid)
        {
            Cmd.CommandText = "Compliance.Reporting.PatientsInActivePopulationWithoutPhoneNumbers";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            Cmd.Parameters.AddWithValue("@inwhitepages", false);
            return ExecuteTableQuery(Cmd);
        }

        public void InsertReturnedRecord()
        {
            //Cmd.CommandText = "usp_rpt_PatientDemographicsPostGres";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            //Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
        }

        public int InsertAPISession(DateTime startdate, int pmdclientid)
        {
            Cmd.CommandText = "WhitePages.dbo.InsertAPISession";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@startdate", startdate);
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            return ExecuteScalarInt(Cmd);
        }

        public void UpdateAPISession(int sessionid, DateTime enddate)
        {
            Cmd.CommandText = "WhitePages.dbo.UpdateAPISession";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandTimeout = 1000;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@sessionid", sessionid);
            Cmd.Parameters.AddWithValue("@enddate", enddate);
            ExecuteNonQuery(Cmd);
        }
    }
}
