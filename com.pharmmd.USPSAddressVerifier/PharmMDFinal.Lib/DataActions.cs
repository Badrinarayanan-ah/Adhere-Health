using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PharmMDFinal.Lib
{
        public class DataActions : DataLayer
        {

            //builds dynamic sql string
            public DataTable GetETLCoreAddresses(string sql)
            {
                PmdCmd.CommandText = sql;
                PmdCmd.CommandType = CommandType.Text;
                PmdCmd.Parameters.Clear();

                return ExecuteETLCoreTableQuery(PmdCmd);
            }

             public DataTable SelectUsedPatientRecord()
            {
                Cmd.CommandText = "SelectUsedPatientRecord";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Clear();

                return ExecuteTableQuery(Cmd);
            }
      

         public DataTable WrongNumberPatientsReportWithDemographics(int? pmdclientid,int reportingyear, DateTime startdate, DateTime stopdate)
        {
            PmdCmd.CommandText = "WrongNumberPatientsReportWithDemographics";
            PmdCmd.CommandType = CommandType.StoredProcedure;
            PmdCmd.Parameters.Clear();
            PmdCmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
            PmdCmd.Parameters.AddWithValue("@reportingyear", reportingyear);
            PmdCmd.Parameters.AddWithValue("@reportrunstartdate", startdate);
            PmdCmd.Parameters.AddWithValue("@reportrunenddate", stopdate);

            return ExecuteETLCoreTableQuery(PmdCmd);
        }

        public DataTable ReturnETLCoreUSPSAPIEligibleList(int? pmdclientid,int? top, int? pmdpatientid)
            {
                PmdCmd.CommandText = "ReturnUSPSAPIEligibleList";
                PmdCmd.CommandType = CommandType.StoredProcedure;
                PmdCmd.Parameters.Clear();
                PmdCmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
                PmdCmd.Parameters.AddWithValue("@top", top);
                PmdCmd.Parameters.AddWithValue("@pmdpatientid", pmdpatientid);

                return ExecuteETLCoreTableQuery(PmdCmd);
            }

            public DataTable ReturnMirrorsUSPSAPIEligibleList(int? pmdclientid, int? top, int? pmdpatientid)
            {
                MirrorsCmd.CommandText = "ReturnUSPSAPIEligibleList";
                MirrorsCmd.CommandType = CommandType.StoredProcedure;
                MirrorsCmd.Parameters.Clear();
                MirrorsCmd.Parameters.AddWithValue("@pmdclientid", pmdclientid);
                MirrorsCmd.Parameters.AddWithValue("@top", top);
                MirrorsCmd.Parameters.AddWithValue("@pmdpatientid", pmdpatientid);

                return ExecuteMirrorsTableQuery(MirrorsCmd);
            }


        public DataTable SelectUsedPatientRecordByPatientID(long patientid)
            {
                Cmd.CommandText = "SelectUsedPatientRecordByPatientID";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Clear();
                Cmd.Parameters.AddWithValue("@patientid", patientid);
                return ExecuteTableQuery(Cmd);
            }

            public int InsertSentPatientOld(long patientid)
            {
                Cmd.CommandText = "InsertUsedPatientRecord";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Clear();
                Cmd.Parameters.AddWithValue("@patientid", patientid);
                return ExecuteScalarInt(Cmd);
            }

            public int InsertSentPatient(long patientid, string address1)
            {
                PmdCmd.CommandText = "InsertUsedPatientRecord";
                PmdCmd.CommandType = CommandType.StoredProcedure;
                PmdCmd.Parameters.Clear();
                PmdCmd.Parameters.AddWithValue("@patientid", patientid);
                PmdCmd.Parameters.AddWithValue("@address1", address1);
                return ExecuteETLCoreScalarInt(PmdCmd);
            }

            public void ClearSentPatients()
            {
                Cmd.CommandText = "ClearUsedPatients";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Clear();

                ExecuteNonQuery(Cmd);
            }

            public int InsertUSPSReturnVerification(long pmdpatientid, string inputfulladdress, string returnedfulladdress, bool? verified, bool? match, string differences, bool? soundexmatch, string address1, string address2, string city, string state, string zip, string newaddress1, string newaddress2, string newcity, string newstate, string newzip)
            {
                PmdCmd.CommandText = "InsertUSPSReturnVerification";
                PmdCmd.CommandType = CommandType.StoredProcedure;
                PmdCmd.Parameters.Clear();
                PmdCmd.Parameters.AddWithValue("@pmdpatientid", pmdpatientid);
                PmdCmd.Parameters.AddWithValue("@inputfulladdress", inputfulladdress);
                PmdCmd.Parameters.AddWithValue("@returnedfulladdress", returnedfulladdress);
                PmdCmd.Parameters.AddWithValue("@verified", verified);
                PmdCmd.Parameters.AddWithValue("@match", match);
                PmdCmd.Parameters.AddWithValue("@differences", differences);
                PmdCmd.Parameters.AddWithValue("@soundexmatch", soundexmatch);

                PmdCmd.Parameters.AddWithValue("@address1", address1);
                PmdCmd.Parameters.AddWithValue("@address2", address2);
                PmdCmd.Parameters.AddWithValue("@city", city);
                PmdCmd.Parameters.AddWithValue("@state", state);
                PmdCmd.Parameters.AddWithValue("@zip", zip);

                PmdCmd.Parameters.AddWithValue("@newaddress1", newaddress1);
                PmdCmd.Parameters.AddWithValue("@newaddress2", newaddress2);
                PmdCmd.Parameters.AddWithValue("@newcity", newcity);
                PmdCmd.Parameters.AddWithValue("@newstate", newstate);
                PmdCmd.Parameters.AddWithValue("@newzip", newzip);

                return ExecuteETLCoreScalarInt(PmdCmd);
            }


            public void ClearReturnedUSPSAddresses()
            {
                PmdCmd.CommandText = "ClearReturnedUSPSAddresses";
                PmdCmd.CommandType = CommandType.StoredProcedure;
                PmdCmd.Parameters.Clear();

                ExecuteETLCoreNonQuery(PmdCmd);
            }

    }
    }
