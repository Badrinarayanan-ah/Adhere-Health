using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace MirrorsChecker
{
    public static class CommonFunctions
    {
        public static Environment LoadEnvironmentFromRow(DataRow row)
        {
            Environment e = new Environment();

            e.mirror_id = Convert.ToInt32(row["mirror_id"]);
            e.dst_db_name_final = row["dst_db_name_final"].ToString();
            e.medproconnection = row["medproconnection"].ToString();
            e.linkservername = row["linkservername"].ToString();


            return e;
        }

        public static List<Environment> LoadEnvironmentFromDataTable(DataTable dt)
        {
            List<Environment> lstEnv = new List<Environment>();

            foreach (DataRow dr in dt.Rows)
            {
                lstEnv.Add(LoadEnvironmentFromRow(dr));

            }

            return lstEnv;
        }
    }
}
