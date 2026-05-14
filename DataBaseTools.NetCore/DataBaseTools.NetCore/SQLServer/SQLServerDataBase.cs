using DataBaseTools.NetCore.General;
using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;

namespace DataBaseTools.NetCore.SQLServer
{
    /// <summary>
    /// Objeto para gestionar la interacción de base de datos de SQLServer.
    /// </summary>
    public class SQLServerDataBase : DataBaseGeneral, ISQLServerDataBase
    {
        public SQLServerDataBase(string connectionString) : base(connectionString)
        {
        }

        public SQLServerDataBase(string connectionString, string domain, string userName, string password) : base(connectionString, domain, userName, password)
        {
        }

        /// <summary>
        /// Crear una conexión a SQLServer utilizando la cadena de conexión proporcionada en la propiedad ConnectionString.
        /// </summary>
        /// <returns>Conexión de SQLServer configurada.</returns>
        public SqlConnection CreateSqlConnection() => new SqlConnection(ConnectionString);

        /// <summary>
        /// Crear comando de SQLServer utilizando el tipo de comando, la conexión y la consulta proporcionados.
        /// </summary>
        /// <param name="commandType">Tipo de comando a ejecutar (StoredProcedure, Text, etc.).</param>
        /// <param name="sqlConnection">Conexión de SQLServer.</param>
        /// <param name="query">Consulta o procedimiento almacenado a ejecutar.</param>
        /// <returns>Comando de SQLServer configurado.</returns>
        public SqlCommand CreateSqlCommand(CommandType commandType, SqlConnection sqlConnection, string query, Dictionary<string, object> parameters = null)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
                }
            }

            return sqlCommand;
        }

        /// <summary>
        /// DataSet con el resultado de la ejecución del comando de SQLServer proporcionado.
        /// </summary>
        /// <param name="cmd">Comando de SQLServer a ejecutar.</param>
        /// <returns>DataSet con los resultados de la ejecución del comando.</returns>
        public DataSet ExecuteDataSet(SqlCommand cmd)
        {
            DataSet ds = new DataSet();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
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
            }

            return ds;
        }

        /// <summary>
        /// Ejecuta un commando de SQLServer con una cuenta dominio
        /// </summary>
        /// <param name="safeAccessTokenHandle">Manejador de token de acceso seguro para la cuenta de dominio.</param>
        /// <param name="cmd">Comando de SQLServer a ejecutar.</param>
        /// <returns>DataSet con los resultados de la ejecución del comando.</returns>
        public DataSet ExecuteDataSet(SafeAccessTokenHandle safeAccessTokenHandle, SqlCommand cmd)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                try
                {
                    WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () =>
                    {
                        cmd.CommandTimeout = 0;
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return ds;
        }

        /// <summary>
        /// Executes a query against the specified SQL connection and returns the results as a DataSet. 
        /// Ejecuta un query con la configuración que se proporcionó en las propiedades de ConnectionString y devuelve los resultados como un DataSet.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta la cadena de comando, como Text o StoredProcedure.</param>
        /// <param name="query">La consulta SQL o el procedimiento almacenado a ejecutar.</param>
        /// <param name="safeAccessTokenHandle">Un token de acceso de Windows opcional para la suplantación durante la ejecución de la consulta.</param>
        /// <returns>Un DataSet que contiene los resultados de la consulta ejecutada.</returns>
        public DataSet ExecuteQuery(CommandType commandType, string query, SafeAccessTokenHandle safeAccessTokenHandle = null, Dictionary<string, object> parameters = null)
        {
            DataSet ds = new DataSet();
            SqlConnection connection = null;

            try
            {
                using (connection = CreateSqlConnection())
                {
                    using (SqlCommand command = CreateSqlCommand(commandType, connection, query, parameters))
                    {
                        if (safeAccessTokenHandle == null)
                        {
                            ds = ExecuteDataSet(command);
                        }
                        else
                        {
                            ds = ExecuteDataSet(safeAccessTokenHandle, command);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return ds;
        }
    }
}
