using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace WhitePagesFindPersonAPI
{
    public class DataLayer
    {
        private SqlConnection conn;
        private SqlCommand cmd;


        public SqlConnection Conn
        {
            get
            {
                if (null == conn)
                    //conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DWConnectionString"].ToString());
                    conn = new SqlConnection(Properties.Settings1.Default.DWConnectionStringText);
                    //conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["WhitePagesFindPersonAPI.Properties.Settings1.DWConnectionStringText"].ToString());

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
                }

                return cmd;
            }
            set
            {
                cmd = value;
            }

        }
        
        

        public DataTable ExecuteTableQuery(System.Data.Common.DbCommand command)
        {
            DataTable returnTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command);

            command.Connection = Conn;


            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                adapter.Fill(returnTable);
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnTable;
        }

        /// <summary>
        /// Executes a query with no return value.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        public void ExecuteNonQuery(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                command.ExecuteNonQuery();
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }
        }

        public int ExecuteNonQuerySproc(DbCommand command)
        {
            command.Connection = Conn;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                return command.ExecuteNonQuery();
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <returns>The return value as an object.</returns>
        public int ExecuteScalarInt(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            int returnVal = 0;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                returnVal = Convert.ToInt32(command.ExecuteScalar());
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnVal;
        }
        

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <returns>The return value as an object.</returns>
        public long ExecuteScalarLong(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            long returnVal = 0;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                returnVal = Convert.ToInt64(command.ExecuteScalar());
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnVal;
        }

        public int ExecuteScalarQuery(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            int returnVal = 0;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                returnVal = Convert.ToInt32(command.ExecuteScalar());
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnVal;
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <returns>The return value as an object.</returns>
        public string ExecuteScalarString(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            string returnVal = "";
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                returnVal = command.ExecuteScalar().ToString();
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnVal;
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <returns>The return value as an object.</returns>
        public string ExecuteScalarQueryToString(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            string returnVal = "";
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                returnVal = command.ExecuteScalar().ToString();
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnVal;
        }
        /// <summary>
        /// Executes a query and delegates the handling of the reader to an external method.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <param name="processor">The external method that processes the reader.</param>
        public SqlDataReader ExecuteReaderQuery(SqlCommand command)
        {
            command.Connection = Conn;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                SqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <returns>The return value as an object.</returns>
        public bool ExecuteScalarBoolean(System.Data.Common.DbCommand command)
        {
            command.Connection = Conn;
            bool returnVal = false;
            command.CommandTimeout = 0;

            try
            {
                if (ConnectionState.Closed == Conn.State)
                    Conn.Open();

                returnVal = Convert.ToBoolean(command.ExecuteScalar());
            }
            finally
            {
                if (ConnectionState.Open == Conn.State)
                    Conn.Close();
            }

            return returnVal;
        }
    }
}
