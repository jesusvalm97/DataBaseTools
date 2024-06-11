using Microsoft.Win32.SafeHandles;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.NetCore.Oracle
{
    public static class DataBase
    {
        public static OracleConnection CreateOracleConnection(string stringConnection) => new OracleConnection(stringConnection);

        public static OracleCommand CreateOracleCommand(CommandType commandType, OracleConnection oracleConnection, string query)
        {
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnection;
            oracleCommand.CommandText = query;
            oracleCommand.CommandType = commandType;

            return oracleCommand;
        }

        public static DataSet ExecuteDataSet(OracleCommand cmd)
        {
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
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
        public static DataSet ExecuteDataSet(SafeAccessTokenHandle safeAccessTokenHandle, OracleCommand cmd)
        {
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
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

        public static DataSet ExecuteQuery(CommandType commandType, OracleConnection connection, string query, SafeAccessTokenHandle safeAccessTokenHandle = null)
        {
            using (OracleCommand command = CreateOracleCommand(commandType, connection, query))
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

        public static DataSet ExecuteQuery(CommandType commandType, OracleConnection connection, string query, string domainDB = null, string userNameDB = null, string passWordDB = null)
        {
            using (OracleCommand command = CreateOracleCommand(commandType, connection, query))
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
