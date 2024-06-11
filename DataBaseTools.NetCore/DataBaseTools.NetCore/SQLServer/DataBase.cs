using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.NetCore.SQLServer
{
    public static class DataBase
    {
        public static SqlConnection CreateSqlConnection(string stringConnection) => new SqlConnection(stringConnection);

        public static SqlCommand CreateSqlCommand(CommandType commandType, SqlConnection sqlConnection, string query)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;

            return sqlCommand;
        }

        public static DataSet ExecuteDataSet(SqlCommand cmd)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                cmd.CommandTimeout = 600;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        /// <summary>
        /// Ejecuta un commando de SQLServer con una cuenta dominio
        /// </summary>
        /// <param name="safeAccessTokenHandle"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(SafeAccessTokenHandle safeAccessTokenHandle, SqlCommand cmd)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () => {
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public static DataSet ExecuteQuery(CommandType commandType, SqlConnection connection, string query, SafeAccessTokenHandle safeAccessTokenHandle = null)
        {
            using (SqlCommand command = CreateSqlCommand(commandType, connection, query))
            {
                if (safeAccessTokenHandle == null)
                {
                    return ExecuteDataSet(command);
                }
                else
                {
                    return ExecuteDataSet(safeAccessTokenHandle, command);
                }
            }
        }

        public static DataSet ExecuteQuery(CommandType commandType, SqlConnection connection, string query, string domainDB = null, string userNameDB = null, string passWordDB = null)
        {
            using (SqlCommand command = CreateSqlCommand(commandType, connection, query))
            {
                if (string.IsNullOrEmpty(domainDB) || string.IsNullOrEmpty(userNameDB) || string.IsNullOrEmpty(passWordDB))
                {
                    return ExecuteDataSet(command);
                }
                else
                {
                    SafeAccessTokenHandle safeAccessTokenHandle = General.DataBase.GetTokenDomainConnection(domainDB, userNameDB, passWordDB);
                    return ExecuteDataSet(safeAccessTokenHandle, command);
                }
            }
        }
    }
}
