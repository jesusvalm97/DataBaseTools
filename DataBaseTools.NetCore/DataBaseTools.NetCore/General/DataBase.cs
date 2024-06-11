using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.NetCore.General
{
    public static class DataBase
    {
        #region Conexion por cuenta dominio

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
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SafeAccessTokenHandle GetTokenDomainConnection(string domain, string userName, string password)
        {
            const int LOGON32_LOGON_INTERACTIVE = 2;
            const int LOGON32_PROVIDER_DEFAULT = 0;

            bool returnValue = LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out SafeAccessTokenHandle safeAccessTokenHandle);

            if (!returnValue)
            {
                int ret = Marshal.GetLastWin32Error();
                throw new Win32Exception(ret);
            }

            return safeAccessTokenHandle;
        }

        #endregion

        public static string GetStringFromDataRow(DataRow dataRow, int column) => dataRow[column] == DBNull.Value ? string.Empty : Convert.ToString(dataRow[column]);

        public static char GetCharFromDataRow(DataRow dataRow, int column) => dataRow[column] == DBNull.Value ? ' ' : Convert.ToChar(dataRow[column]);

        public static int GetIntFromDataRow(DataRow dataRow, int column) => dataRow[column] == DBNull.Value ? 0 : Convert.ToInt32(dataRow[column]);

        public static byte GetByteFromDataRow(DataRow dataRow, int column)
        {
            var columnValue = dataRow[column];

            if (columnValue == DBNull.Value)
                return 0;
            else
                return Convert.ToByte(columnValue);
        }

        public static bool GetBooleanFromDataRow(DataRow dataRow, int column) => dataRow[column] == DBNull.Value ? false : Convert.ToBoolean(dataRow[column]);

        public static DateTime GetDateTimeFromDataRow(DataRow dataRow, int column) => dataRow[column] == DBNull.Value ? new DateTime() : Convert.ToDateTime(dataRow[column]);
    }
}
