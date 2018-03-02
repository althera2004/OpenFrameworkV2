// --------------------------------
// <copyright file="ExceptionManager.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;

    /// <summary>Implements the ExceptionManager class</summary>
    public static class ExceptionManager
    {
        /// <summary>Write a trace line on a log daily file</summary>
        /// <param name="ex">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        public static void Trace(Exception ex, string source)
        {
            Trace(ex, source, string.Empty);
        }

        /// <summary>Trace a exception into log file</summary>
        /// <param name="ex">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        /// <param name="extraData">Data extra of exception</param>
        public static void Trace(Exception ex, string source, string extraData)
        {
            int pos = ex.StackTrace.LastIndexOf('\n');

            if (pos > 0)
            {
                source = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0} - {1}", source, ex.StackTrace.Substring(pos));
            }

            string message = string.Empty;
            if (ex != null)
            {
                message = ex.Message;
            }

            if (string.IsNullOrEmpty(source))
            {
                source = string.Empty;
            }

            if (string.IsNullOrEmpty(extraData))
            {
                extraData = string.Empty;
            }

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.Ordinal))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\Log\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}Log\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
            }

            string line = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}::{1}::{2}::{3}", DateTime.Now.ToString("hh:mm:ss", CultureInfo.GetCultureInfo("es-es")), ex.Message, source, extraData);

            using (StreamWriter output = new StreamWriter(path, true))
            {
                output.WriteLine(line);
                output.Flush();
            }
        }

        /// <summary>Log exception</summary>
        /// <param name="exception">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        public static void LogException(Exception exception, string source)
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.Ordinal))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\Log\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}Log\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
            }

            // Open the log file for append and write the log
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("********** {0} **********", DateTime.Now);
                if (exception.InnerException != null)
                {
                    writer.Write("Inner Exception Type: ");
                    writer.WriteLine(exception.InnerException.GetType().ToString());
                    writer.Write("Inner Exception: ");
                    writer.WriteLine(exception.InnerException.Message);
                    writer.Write("Inner Source: ");
                    writer.WriteLine(exception.InnerException.Source);
                    if (exception.InnerException.StackTrace != null)
                    {
                        writer.WriteLine("Inner Stack Trace: ");
                        writer.WriteLine(exception.InnerException.StackTrace);
                    }
                }

                writer.Write("Exception Type: ");
                writer.WriteLine(exception.GetType().ToString());
                writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", exception.Message));
                writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Source: {0}", source));
                writer.WriteLine("Stack Trace: ");
                if (exception.StackTrace != null)
                {
                    writer.WriteLine(exception.StackTrace);
                    writer.WriteLine();
                }
            }
        }

        /// <summary>Notify to system operators an exception occurred</summary>
        /// <param name="exception">Exception occurred</param>
        public static void NotifySystemOps(Exception exception)
        {
            // Include code for notifying IT system operators
        }
    }
}