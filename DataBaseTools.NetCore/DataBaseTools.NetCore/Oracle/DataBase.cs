using DataBaseTools.NetCore.General;
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
    public class DataBase : DataBaseGeneral
    {
        public DataBase(string connectionString) : base(connectionString)
        {
        }

        public OracleConnection CreateOracleConnection() => new OracleConnection(ConnectionString);

        public OracleCommand CreateOracleCommand(CommandType commandType, OracleConnection oracleConnection, string query)
        {
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnection;
            oracleCommand.CommandText = query;
            oracleCommand.CommandType = commandType;

            return oracleCommand;
        }

        public DataSet ExecuteDataSet(OracleCommand cmd)
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
        public DataSet ExecuteDataSet(SafeAccessTokenHandle safeAccessTokenHandle, OracleCommand cmd)
        {
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            try
            {
                WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () => {
                    cmd.CommandTimeout = 0;
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

        public DataSet ExecuteQuery(CommandType commandType, string query, SafeAccessTokenHandle safeAccessTokenHandle = null)
        {
            using (OracleCommand command = CreateOracleCommand(commandType, CreateOracleConnection(), query))
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
    }
}
