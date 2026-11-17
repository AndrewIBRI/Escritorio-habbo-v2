namespace MyAzure_Administration.Kernel
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;

    internal class MySQL
    {
        private string Database = "butterfix";
        private string Host = "92.60.116.105";
        private static MySqlCommand mCommand = null;
        private static MySqlConnection mConnection = null;
        private string Password = "Windows8";
        private int Port = 0xcea;
        private string Username = "azure";

        internal MySQL()
        {
        }

        internal void EndConnection()
        {
            mConnection.Close();
            mConnection = null;
            mCommand = null;
        }

        internal void ExecuteQuery(string Query)
        {
            try
            {
                mCommand.CommandText = Query;
                mCommand.Parameters.Add(new MySqlParameter(Query, Query));
                mConnection.Open();
                mCommand.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                mCommand.CommandText = "";
                mCommand.Parameters.Clear();
                mConnection.Close();
            }
        }

        internal void InitConnection()
        {
            string connectionString = string.Concat(new object[] { "Database=", this.Database, ";Data Source=", this.Host, ";User Id=", this.Username, ";Password=", this.Password, ";Port=", this.Port });
            try
            {
                mConnection = new MySqlConnection(connectionString);
                mConnection.Open();
                mCommand = new MySqlCommand();
                mCommand.Connection = mConnection;
            }
            catch
            {
                Console.WriteLine("Error al conectar con la base de datos.");
            }
            finally
            {
                mConnection.Close();
            }
        }

        internal DataRow ReadDataRow(string Query)
        {
            DataSet dataSet = new DataSet();
            DataRow row = new DataTable().NewRow();
            try
            {
                mCommand.CommandText = Query;
                mConnection.Open();
                new MySqlDataAdapter(mCommand).Fill(dataSet);
                row = dataSet.Tables[0].Rows[0];
            }
            catch
            {
            }
            finally
            {
                mCommand.CommandText = "";
                mCommand.Parameters.Clear();
                mConnection.Close();
            }
            return row;
        }

        internal int ReadInt32(string Query)
        {
            int num;
            try
            {
                mConnection.Open();
                mCommand.CommandText = Query;
                num = (int) mCommand.ExecuteScalar();
            }
            catch
            {
                num = -1;
            }
            finally
            {
                mCommand.CommandText = "";
                mCommand.Parameters.Clear();
                mConnection.Close();
            }
            return num;
        }

        internal string ReadString(string Query)
        {
            string str;
            try
            {
                mConnection.Open();
                mCommand.CommandText = Query;
                str = (string) mCommand.ExecuteScalar();
            }
            catch
            {
                str = "";
            }
            finally
            {
                mCommand.CommandText = "";
                mCommand.Parameters.Clear();
                mConnection.Close();
            }
            return str;
        }
    }
}

