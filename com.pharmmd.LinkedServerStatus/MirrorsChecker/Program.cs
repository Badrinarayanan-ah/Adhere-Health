using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.Sql;

namespace MirrorsChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckLinkedServers();
        }

        static void CheckLinkedServers()
        {
            SQLDataActions sqlDA = new SQLDataActions();
            List<Environment> lstEnv = CommonFunctions.LoadEnvironmentFromDataTable(sqlDA.GetMedProEnvironmentProductionIDs());

            //testing failure on 5/23/2018

            lstEnv.Add(new Environment(4, "PG_ME", "PGUS", "JOE"));

            foreach (Environment e in lstEnv)
            {
                if (e.linkservername.Length > 0)
                {
                    if (e.linkservername.ToUpper() != "ALL")
                    {
                        try
                        {
                            string sql = "select * from " + e.medproconnection + "users";
                            sqlDA.ExecuteMirrorCheckSQL(sql);

                            sqlDA.InsertCheckerRecord(e.mirror_id, true);
                        }
                        catch (Exception ex)
                        {
                            string theproblem = ex.Message + " - " + ex.Message;

                            string body = e.medproconnection + " Connection Failure at " + DateTime.Now.ToString();

                            try
                            {
                                sqlDA.SendEmailAlert("brian.williams@pharmmd.com", body);
                            }
                            finally { }


                            sqlDA.InsertCheckerRecord(e.mirror_id, false);
                        }
                    }
                }
            }
        }
    }
}

