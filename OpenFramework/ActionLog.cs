// --------------------------------
// <copyright file="ActionLog.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;

    public static class ActionLog
    {
        /// <summary>Gets application user data for trace</summary>
        private static string ApplicationUserTrace(string userDescription)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:dd/MM/yyyy - hh:mm:ss} - {1}",
                DateTime.Now, userDescription);
        }

        /// <summary>Inserts a trace for an item action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="itemId">Item identificer</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Name of instance</param>
        /// <param name="userDescription">User description for trace purposses</param>
        public static void Trace(string action, object itemId, string data, string instanceName, string userDescription)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, "{0}{1}", action, itemId.ToString()), data, instanceName, userDescription);
        }

        /// <summary>Inserts a trace for an item action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="itemId">Item identificer</param>
        /// <param name="data">Data of action</param>
        public static void Trace(string action, long itemId, string data,string instanceName, string userDescription)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, "{0}{1}", action, itemId), data, instanceName, userDescription);
        }

        /// <summary>Inserts a trace for an item action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="itemId">Item identificer</param>
        /// <param name="data">Data of action</param>
        public static void Trace(string action, string itemId, string data, string instanceName, string userDescription)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, "{0}{1}", action, itemId), data,instanceName, userDescription);
        }

        /// <summary>Inserts a trace for an action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="data">Data of action</param>
        public static void Trace(string action, string data, string instanceName, string userDescription)
        {
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}.log",
                instanceName,
                action);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if(!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\", path);
            }

            string completeFileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}Log\\{1}",
                path,
                fileName);

            using (StreamWriter output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                output.WriteLine(ApplicationUserTrace(userDescription));
                output.WriteLine(data);
            }
        }

        /// <summary>Inserts a trace with actual date for an action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="data">Data of action</param>
        public static void TraceDated(string action, string data, string instanceName, string userDescription)
        {
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}_{2:yyyyMMdd}.log",
                instanceName,
                action,
                DateTime.Now);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\", path);
            }

            string completeFileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}Log\\{1}",
                path,
                fileName);

            using (StreamWriter output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                output.WriteLine(ApplicationUserTrace(userDescription));
                output.Write("\t");
                output.WriteLine(data);
            }
        }
    }
}