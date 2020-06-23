using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;


namespace MirrorsChecker
{

    public class DataLayer
    {
        public static string HSConnString = "server=SQLDW;database=Compliance;uid=masterConfigApp;password=configApp;";
        public static string PMDConnString = "server=SQLDW;database=Compliance;uid=masterConfigApp;password=configApp;";
        public static string SCANConnString = "server=SQLDW;database=Compliance;uid=masterConfigApp;password=configApp;";

        SqlConnection conn;
        SqlCommand cmd;

        public SqlConnection Conn
        {
            get
            {
                string value = string.Empty;

                if (conn == null)
                {

#if DEBUG
                    conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PharmMDConfigurationConnectionString"].ToString());
#elif RELEASE
                    conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PharmMDConfigurationConnectionString"].ToString());
#endif

                   //conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PharmMDConfigurationConnectionString"].ToString());
                }


                return conn;
            }

            set
            {
                conn = value;
            }
        }

        public SqlCommand Cmd
        {
            get
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                    cmd.CommandTimeout = 60;
                    cmd.Connection = Conn;
                    Conn.Open();
                }
                else
                {
                    if (Conn.State == ConnectionState.Closed)
                    {
                        Conn.Open();
                    }
                }

                return cmd;
            }
            set
            {
                cmd = value;
            }
        }

        public SqlDataReader ExecuteReaderQuery(SqlCommand Cmd)
        {
            SqlDataReader reader = Cmd.ExecuteReader();
            Conn.Close();
            return reader;
        }

        public DataTable ExecuteTableQuery(SqlCommand Cmd)
        {
            DataTable returnTable = new DataTable();
            SqlDataReader rdr = Cmd.ExecuteReader();
            returnTable.Load(rdr);
            Conn.Close();
            return returnTable;
        }

        public long ExecuteScalarQueryLong(SqlCommand cmd)
        {
            long returnValue = Convert.ToInt64(Cmd.ExecuteScalar());
            Conn.Close();
            return returnValue;
        }

        public DateTime? ExecuteScalarQueryWithDate(SqlCommand cmd)
        {
            try
            {
                DateTime returnValue = Convert.ToDateTime(Cmd.ExecuteScalar());
                Conn.Close();
                return returnValue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int ExecuteScalarQuery(SqlCommand Cmd)
        {
            int returnValue = Convert.ToInt32(Cmd.ExecuteScalar());
            Conn.Close();
            return returnValue;
        }

        public bool ExecuteScalarQueryWithBoolean(SqlCommand Cmd)
        {
            bool returnValue = Convert.ToBoolean(Cmd.ExecuteScalar());
            Conn.Close();
            return returnValue;
        }

        public string ExecuteScalarQueryWithString(SqlCommand Cmd)
        {
            string returnValue = Cmd.ExecuteScalar().ToString();
            Conn.Close();
            return returnValue;
        }

        public void ExecuteNonQuery(SqlCommand Cmd)
        {
            Cmd.ExecuteNonQuery();
            Conn.Close();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
