using DataBaseTools.NetCore.General;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace DataBaseTools.NetCore.Oracle
{
    /// <summary>
    /// Objeto para gestionar la interacción de base de datos de Oracle.
    /// </summary>
    public class OracleDataBase : DataBaseGeneral, IOracleDataBase
    {
        public OracleDataBase(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Crear una conexión a Oracle utilizando la cadena de conexión proporcionada en la propiedad ConnectionString.
        /// </summary>
        /// <returns>Conexión de Oracle configurada.</returns>
        public OracleConnection CreateOracleConnection() => new OracleConnection(ConnectionString);

        /// <summary>
        /// Crear comando de Oracle utilizando el tipo de comando, la conexión y la consulta proporcionados.
        /// </summary>
        /// <param name="commandType">Tipo de comando a ejecutar (StoredProcedure, Text, etc.).</param>
        /// <param name="oracleConnection">Conexión de oracle.</param>
        /// <param name="query">Consulta o procedimiento almacenado a ejecutar.</param>
        /// <returns>Comando de Oracle configurado.</returns>
        public OracleCommand CreateOracleCommand(CommandType commandType, OracleConnection oracleConnection, string query)
        {
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnection;
            oracleCommand.CommandText = query;
            oracleCommand.CommandType = commandType;

            return oracleCommand;
        }

        /// <summary>
        /// DataSet con el resultado de la ejecución del comando de Oracle proporcionado.
        /// </summary>
        /// <param name="cmd">Comando de Oracle a ejecutar.</param>
        /// <returns>DataSet con los resultados de la ejecución del comando.</returns>
        public DataSet ExecuteDataSet(OracleCommand cmd)
        {
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            try
            {
                cmd.CommandTimeout = 0;
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
        /// Ejecuta una consulta o procedimiento almacenado con la configuración que se proporcionó en las propiedades de ConnectionString.
        /// </summary>
        /// <param name="commandType">Tipo de comando a ajecutar (StoredProcedure, Text, etc.</param>
        /// <param name="query">Consulta o procedmiento almacenado a ejecutar.</param>
        /// <returns>DataSet con los resultados de la ejecución del comando.</returns>
        public DataSet ExecuteQuery(CommandType commandType, string query)
        {
            using (OracleCommand command = CreateOracleCommand(commandType, CreateOracleConnection(), query))
            {
                return ExecuteDataSet(command);
            }
        }
    }
}