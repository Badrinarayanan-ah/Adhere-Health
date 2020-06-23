using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TwilioPhoneNumberLookup
{
    public class SQLDataActions: DataLayer
    {
        public DataTable ReturnPhoneNumberData(int pmdclientid)
        {
            Cmd.CommandText = "usp_rptPatientPhoneNumbersODSBase";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            return ExecuteTableQuery(Cmd);
        }

        public void InsertReturnedRecord()
        {
            //Cmd.CommandText = "usp_rpt_PatientDemographicsPostGres";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Clear();
            //Cmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
        }
    }
}
