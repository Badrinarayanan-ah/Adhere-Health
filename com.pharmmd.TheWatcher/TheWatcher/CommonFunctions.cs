using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TheWatcher
{
    public static class CommonFunctions
    {
        public static List<int> DaysToSkipFromSkipDays(string skipdays)
        {
            List<int> lst = new List<int>();

            foreach (char s in skipdays)
            {
                lst.Add(Convert.ToInt32(s.ToString()));
            }

            return lst;
        }


        public static Dictionary<string, int> TheseDays()
        {
            Dictionary<string, int> lst = new Dictionary<string, int>();

            lst.Add("Sunday", 1);
            lst.Add("Monday", 2);
            lst.Add("Tuesday", 3);
            lst.Add("Wednesday", 4);
            lst.Add("Thursday", 5);
            lst.Add("Friday", 6);
            lst.Add("Saturday", 7);

            return lst;
        }

        public static List<TableTable> LoadTableDataTableItoList(DataTable dt)
        {
            List<TableTable> lst = new List<TableTable>();

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(LoadTableRowIntoObject(row));
            }

            return lst;
        }

        public static string ReturnFormattedColumnName(string servername, string databasename, string schemaname, string tablename, string columnname)
        {
            return "[" + servername + "].[" + databasename + "].[" + schemaname + "].[" + tablename + "].[" + columnname + "]";
        }
        public static TableTable LoadTableRowIntoObject(DataRow row)
        {
            TableTable t = new TableTable();

            t.DaysToSkip = row["skipdays"].ToString();

            if (row["dateinterval"] != DBNull.Value)
            {
                t.DateInterval = Convert.ToInt32(row["dateinterval"]);
            }
            else
            {
                t.DateInterval = -1;
            }

            t.TableName = row["tablename"].ToString();
            t.ColumnName = row["columnname"].ToString();
            t.RowID = Convert.ToInt32(row["rowid"]);
            t.Active = Convert.ToBoolean(row["active"]);

            t.DatabaseName = row["databasename"].ToString();
            t.SchemaName = row["schemaname"].ToString();
            t.ServerName = row["servername"].ToString();
            t.SQL = row["sql"].ToString();

            t.CreatedDate = Convert.ToDateTime(row["createddate"]);

            if (row["lastchecked"] != DBNull.Value)
            {
                t.LastChecked = Convert.ToDateTime(row["lastchecked"]);
            }

            if (row["dayssincelastverified"] != DBNull.Value)
            {
                t.DaysSinceLastVerified = Convert.ToInt32(row["dayssincelastverified"]);
            }

            if (row["updateddate"] != DBNull.Value)
            {
                t.UpdatedDate = Convert.ToDateTime(row["updateddate"]);
            }

            t.ModeID = Convert.ToInt32(row["modeid"]);

            t.CreatedBy = row["createdby"].ToString();
            t.UpdatedBy = row["updatedby"].ToString();
            return t;
        }
    }
}
