using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Data;

namespace DataBaseTools.NetCore.SQLServer
{
    /// <summary>
    /// Objeto para gestionar la interacción de base de datos de SQLServer.
    /// </summary>
    public interface ISQLServerDataBase : General.IDataBaseGeneral
    {
        /// <summary>
        /// Crear una conexión a SQLServer utilizando la cadena de conexión proporcionada en la propiedad ConnectionString.
        /// </summary>
        /// <returns>Conexión de SQLServer configurada.</returns>
        public SqlConnection CreateSqlConnection();

        /// <summary>
        /// Crear comando de SQLServer utilizando el tipo de comando, la conexión y la consulta proporcionados.
        /// </summary>
        /// <param name="commandType">Tipo de comando a ejecutar (StoredProcedure, Text, etc.).</param>
        /// <param name="sqlConnection">Conexión de SQLServer.</param>
        /// <param name="query">Consulta o procedimiento almacenado a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales para el comando.</param>
        /// <returns>Comando de SQLServer configurado.</returns>
        public SqlCommand CreateSqlCommand(CommandType commandType, SqlConnection sqlConnection, string query, Dictionary<string, object> parameters = null);

        /// <summary>
        /// DataSet con el resultado de la ejecución del comando de SQLServer proporcionado.
        /// </summary>
        /// <param name="cmd">Comando de SQLServer a ejecutar.</param>
        /// <returns>DataSet con los resultados de la ejecución del comando.</returns>
        public DataSet ExecuteDataSet(SqlCommand cmd);

        /// <summary>
        /// Ejecuta un commando de SQLServer con una cuenta dominio
        /// </summary>
        /// <param name="safeAccessTokenHandle">Manejador de token de acceso seguro para la cuenta de dominio.</param>
        /// <param name="cmd">Comando de SQLServer a ejecutar.</param>
        /// <returns>DataSet con los resultados de la ejecución del comando.</returns>
        public DataSet ExecuteDataSet(SafeAccessTokenHandle safeAccessTokenHandle, SqlCommand cmd);

        /// <summary>
        /// Executes a query against the specified SQL connection and returns the results as a DataSet. 
        /// Ejecuta un query con la configuración que se proporcionó en las propiedades de ConnectionString y devuelve los resultados como un DataSet.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta la cadena de comando, como Text o StoredProcedure.</param>
        /// <param name="query">La consulta SQL o el procedimiento almacenado a ejecutar.</param>
        /// <param name="safeAccessTokenHandle">Un token de acceso de Windows opcional para la suplantación durante la ejecución de la consulta.</param>
        /// <param name="parameters">Parámetros opcionales para el comando.</param>
        /// <returns>Un DataSet que contiene los resultados de la consulta ejecutada.</returns>
        public DataSet ExecuteQuery(CommandType commandType, string query, SafeAccessTokenHandle safeAccessTokenHandle = null, Dictionary<string, object> parameters = null);
    }
}
