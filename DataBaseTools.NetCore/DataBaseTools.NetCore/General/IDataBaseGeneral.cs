using Microsoft.Win32.SafeHandles;
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DataBaseTools.NetCore.General
{
    /// <summary>
    /// Objeto general para la conexion a base de datos, se pueden agregar mas propiedades o metodos generales para las conexiones a base de datos.
    /// </summary>
    public interface IDataBaseGeneral
    {
        /// <summary>
        /// Cadena de conexión de la base de datos.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Dominio de la cuenta de usuario.
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Inicia sesion y genera el token de una cuenta de windows.
        /// </summary>
        /// <param name="lpszUsername"></param>
        /// <param name="lpszDomain"></param>
        /// <param name="lpszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        /// <summary>
        /// Obtiene el token de una cuena de windows
        /// </summary>
        /// <returns></returns>
        public SafeAccessTokenHandle GetTokenDomainConnection();

        /// <summary>
        /// Obtener un valor string de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor string de la fila y columna especificada.</returns>
        public string GetStringFromDataRow(DataRow dataRow, int column);

        /// <summary>
        /// Obtener un valor char de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor char de la fila y columna especificada.</returns>
        public char GetCharFromDataRow(DataRow dataRow, int column);

        /// <summary>
        /// Obtener un valor int de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor int de la fila y columna especificada.</returns>
        public int GetIntFromDataRow(DataRow dataRow, int column);

        /// <summary>
        /// Obtener un valor Decimal de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor Decimal de la fila y columna especificada.</returns>
        public Decimal GetDecimalFromDataRow(DataRow dataRow, int column);

        /// <summary>
        /// Obtener un valor byte de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor byte de la fila y columna especificada.</returns>
        public byte GetByteFromDataRow(DataRow dataRow, int column);

        /// <summary>
        /// Obtener un valor bool de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor bool de la fila y columna especificada.</returns>
        public bool GetBooleanFromDataRow(DataRow dataRow, int column);

        /// <summary>
        /// Obtener un valor DateTime de una columna en especifica de una fila en especifica.
        /// </summary>
        /// <param name="dataRow">Fila de la cual se quiere obtener el valor.</param>
        /// <param name="column">Columna de la cual se quiere obtener el valor.</param>
        /// <returns>Valor DateTime de la fila y columna especificada.</returns>
        public DateTime GetDateTimeFromDataRow(DataRow dataRow, int column);
    }
}
