﻿namespace SiRFLive.Datalogging
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class DatabaseManager
    {
        private string m_ConnStr = "Data Source=GORGON;Initial Catalog=ReceiverDB_Test;Integrated Security=True;Min Pool Size=5;Max Pool Size=100;Connect Timeout=10";
        private string m_ConnStr_NONPOOLED = "Data Source=GORGON;Initial Catalog=ReceiverDB_Test;Integrated Security=True;Pooling=false;Connect Timeout=10";

        private void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        ~DatabaseManager()
        {
        }

        private SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = this.m_ConnStr;
                connection.Open();
            }
            catch (Exception)
            {
                try
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    connection.ConnectionString = this.m_ConnStr_NONPOOLED;
                    connection.Open();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return connection;
        }

        public string QueryRxType(string rxID)
        {
            string str;
            try
            {
                using (SqlConnection connection = this.OpenConnection())
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT evrType FROM RxData WHERE rxName = '" + rxID + "'";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (string) reader["evrType"];
                        }
                        str = "No Record";
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }
    }
}

