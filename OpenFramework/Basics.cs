namespace OpenFramework
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using OpenFramework.Customer;
    using OpenFramework.Security;

    /// <summary>
    /// Implements auxiliary functions for framework
    /// </summary>
    public static class Basics
    {
        public static CustomerFramework ActualInstance
        {
            get
            {
                /*if (HttpContext.Current.Session["FrameworkCustomer"] != null)
                {
                    return HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
                }
                else*/
                if (InstanceMode.OnPremise)
                {
                    HttpContext.Current.Session["FrameworkCustomer"] = InstanceMode.Instance;
                    return InstanceMode.Instance;
                }

                return CustomerFramework.Empty;
            }
        }

        public static CustomerFramework GetActualInstance()
        {
            CustomerFramework res = CustomerFramework.Empty;
            if (HttpContext.Current.Session["FrameworkCustomer"] != null)
            {
                res = HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
            }

            return res;
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static string MonthName(int index, string languageCode)
        {
            /*if (languageCode.Equals("ca", StringComparison.OrdinalIgnoreCase))
            {
                switch (index)
                {
                    case 1: return "gener";
                    case 2: return "febrer";
                    case 3: return "març";
                    case 4: return "abril";
                    case 5: return "maig";
                    case 6: return "juny";
                    case 7: return "juliol";
                    case 8: return "agost";
                    case 9: return "setembre";
                    case 10: return "octubre";
                    case 11: return "novembre";
                    case 12: return "desembre";
                }
            }*/

            switch (index)
            {
                case 1: return "enero";
                case 2: return "febrero";
                case 3: return "marzo";
                case 4: return "abril";
                case 5: return "mayo";
                case 6: return "junio";
                case 7: return "julio";
                case 8: return "agosto";
                case 9: return "septiembre";
                case 10: return "octubre";
                case 11: return "noviembre";
                case 12: return "diciembre";
            }

            return string.Empty;
        }

        public static string ServerName(string request)
        {
            Uri uri = new Uri(request);
            string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
            return requested;
        }

        public static bool ValidateDate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            string[] data = value.Split('/');
            if (data.Length != 3)
            {
                return false;
            }

            try
            {
                int day = Convert.ToInt32(data[0], CultureInfo.InvariantCulture);
                int month = Convert.ToInt32(data[1], CultureInfo.InvariantCulture);
                int year = Convert.ToInt32(data[2], CultureInfo.InvariantCulture);
                DateTime probeDate = new DateTime(year, month, day);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        /// <param name="emailAddress">Text to validate</param>
        /// <returns>Boolean indicating if text if a valid email</returns>
        public static bool EmailIsValid(string emailAddress)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase).IsMatch(emailAddress);
        }

        /// <summary>
        /// Clone an object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="source">Object to be cloned</param>
        /// <returns>Copy of source object</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        
        /// <summary>
        /// Extract characters from right of text
        /// </summary>
        /// <param name="text">Text where extract</param>
        /// <param name="length">Number of characters to extract</param>
        /// <returns>String value</returns>
        public static string Right(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(text.Length - length, length);
        }

        /// <summary>
        /// Extract characters from left of text
        /// </summary>
        /// <param name="text">Text where extract</param>
        /// <param name="length">Number of characters to extract</param>
        /// <returns>String value</returns>
        public static string Left(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>
        /// Calculate seconds in a string time formatted
        /// </summary>
        /// <param name="time">Time formatted</param>
        /// <returns>Number of seconds represented in time formatted</returns>
        public static int? ParseTime(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return null;
            }


            // Si no se especifica unidad se asumen que son segundos
            if (int.TryParse(time, out int value))
            {
                return value;
            }

            string timeUnit = Right(time, 1);
            string timeValue = time.Substring(0, time.Length - 1);
            if (!int.TryParse(timeValue, out value))
            {
                return null;
            }

            switch (timeUnit.ToUpperInvariant())
            {
                case "S":
                    return value;
                case "M":
                    return value * 60;
                case "H":
                    return value * 3600;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates month name from a date
        /// </summary>
        /// <param name="date">Date to get month name from</param>
        /// <returns>Name of month</returns>
        public static string MonthDate(DateTime date)
        {
            string res = string.Empty;

            switch (date.Month)
            {
                case 1:
                    res = "Enero";
                    break;
                case 2:
                    res = "Febrero";
                    break;
                case 3:
                    res = "Marzo";
                    break;
                case 4:
                    res = "Abril";
                    break;
                case 5:
                    res = "Mayo";
                    break;
                case 6:
                    res = "Junio";
                    break;
                case 7:
                    res = "Julio";
                    break;
                case 8:
                    res = "Agosto";
                    break;
                case 9:
                    res = "Septiembre";
                    break;
                case 10:
                    res = "Octubre";
                    break;
                case 11:
                    res = "Noviembre";
                    break;
                case 12:
                    res = "Diciembre";
                    break;
            }

            res = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}/{1}", res, date.Year);
            return res;
        }

        /// <summary>
        /// Calculates if two dates are in the same month of the same year
        /// </summary>
        /// <param name="firstDate">First date on comparison</param>
        /// <param name="secondDate">Second date on comparison</param>
        /// <returns>Indicates if two dates are in the same month of the same year</returns>
        public static bool SameMonth(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate.Month != secondDate.Month)
            {
                return false;
            }

            if (firstDate.Year != secondDate.Year)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a limited length string
        /// </summary>
        /// <param name="text">Original string</param>
        /// <param name="length">Maximum length</param>
        /// <returns>A limited length string</returns>
        public static string LimitedText(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>
        /// Gets a limited length string
        /// </summary>
        /// <param name="text">Original string</param>
        /// <param name="length">Maximum length</param>
        /// <param name="ellipsis">Shows ellipsis on reduce</param>
        /// <returns>A limited length string</returns>
        public static string LimitedText(string text, int length, bool ellipsis)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", text.Substring(0, length), ellipsis ? "..." : string.Empty);
        }

        public static string SpanishMoney(decimal value)
        {
            return string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:#,##0.00}", value);
        }

        public static string Ident(int length)
        {
            string res = string.Empty;
            length = length * 4;
            for (int x = 0; x < length; x++)
            {
                res += " ";
            }

            return res;
        }

        public static string HtmlLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
            {
                return string.Empty;
            }

            string res = label.Replace("Á", "&Aacute;");
            res = res.Replace("É", "&Eacute;");
            res = res.Replace("Í", "&Iacute;");
            res = res.Replace("Ó", "&Oacute;");
            res = res.Replace("Ú", "&Uacute;");
            res = res.Replace("á", "&aacute;");
            res = res.Replace("é", "&eacute;");
            res = res.Replace("í", "&iacute;");
            res = res.Replace("ó", "&oacute;");
            res = res.Replace("ú", "&uacute;");
            res = res.Replace("À", "&Agrave;");
            res = res.Replace("È", "&Egrave;");
            res = res.Replace("Ì", "&Igrave;");
            res = res.Replace("Ò", "&Ograve;");
            res = res.Replace("Ù", "&Ugrave;");
            res = res.Replace("à", "&agrave;");
            res = res.Replace("è", "&egrave;");
            res = res.Replace("ì", "&igrave;");
            res = res.Replace("ò", "&ograve;");
            res = res.Replace("ù", "&ugrave;");
            res = res.Replace("ñ", "&ntilde;");
            res = res.Replace("Ñ", "&Ntilde;");
            return res;
        }

        public static string Base64Encode(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                return string.Empty;
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }

            if (base64EncodedData.EndsWith("%3d", StringComparison.Ordinal))
            {
                base64EncodedData = base64EncodedData.Replace("%3d", "=");
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static DateTime? DateFromStringddMMyyy(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            string[] data = value.Split('/');

            if (data.Count() != 3)
            {
                return null;
            }

            int year = Convert.ToInt32(data[2], CultureInfo.InvariantCulture);
            int month = Convert.ToInt32(data[1], CultureInfo.InvariantCulture);
            int day = Convert.ToInt32(data[0], CultureInfo.InvariantCulture);
            return new DateTime(year, month, day);
        }

        public static DateTime? DateFromStringyyyyMMdd(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            int year = Convert.ToInt32(value.Substring(0, 4), CultureInfo.InvariantCulture);
            int month = Convert.ToInt32(value.Substring(4, 2), CultureInfo.InvariantCulture);
            int day = Convert.ToInt32(value.Substring(6, 2), CultureInfo.InvariantCulture);
            return new DateTime(year, month, day);
        }

        public static string Resume(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length) + "...";
        }

        public static string TimeAgo(DateTime date)
        {
            TimeSpan span = DateTime.Now - date;
            if (span.Days > 365)
            {
                int years = span.Days / 365;
                if (span.Days % 365 != 0)
                {
                    years += 1;
                }

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    years,
                    years == 1 ? "year" : "years");
            }

            if (span.Days > 30)
            {
                int months = span.Days / 30;
                if (span.Days % 31 != 0)
                {
                    months += 1;
                }

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    months,
                    months == 1 ? "month" : "months");
            }

            if (span.Days > 0)
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    span.Days,
                    span.Days == 1 ? "day" : "days");
            }

            if (span.Hours > 0)
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    span.Hours,
                    span.Hours == 1 ? "hour" : "hours");
            }

            if (span.Minutes > 0)
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    span.Minutes,
                    span.Minutes == 1 ? "minute" : "minutes");
            }

            if (span.Seconds > 5)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "about {0} seconds ago", span.Seconds);
            }

            if (span.Seconds <= 5)
            {
                return "just now";
            }

            return string.Empty;
        }

        public static string CapitalizePhrase(string text)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Return char and concat substring.
            return char.ToUpper(text[0], CultureInfo.GetCultureInfo("en-us")) + text.Substring(1);
        }

        // Cuts the date retrieved from the DB to the format DD/MM/YYYY
        public static string FormatDateDDMMYYYY(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return string.Empty;
            }

            if (date.Length > 9)
            {
                return date.Substring(0, 10);
            }

            return string.Empty;
        }

        public static string FormatDateDDMMYYYY(DateTime date)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0:ddMMyyyy}", date);
        }

        public static string FormatDateDDMMYYYY(DateTime? date)
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }

            return FormatDateDDMMYYYY(date.Value);
        }

        public static string FormatDayMonth(DateTime date)
        {
            string res = string.Format(CultureInfo.InvariantCulture, "{0}", date.Day) + " de ";
            switch (date.Month)
            {
                case 0:
                    res += "enero";
                    break;
                case 1:
                    res += "febrero";
                    break;
                case 2:
                    res += "marzo";
                    break;
                case 3:
                    res += "abril";
                    break;
                case 4:
                    res += "mayo";
                    break;
                case 5:
                    res += "junio";
                    break;
                case 6:
                    res += "julio";
                    break;
                case 7:
                    res += "agosto";
                    break;
                case 8:
                    res += "septiembre";
                    break;
                case 9:
                    res += "octubre";
                    break;
                case 10:
                    res += "noviembre";
                    break;
                case 11:
                    res += "diciembre";
                    break;
            }

            return res;
        }

        /// <summary>
        /// Converts string value on date for DatePicker control
        /// </summary>
        /// <param name="date">Text to convert</param>
        /// <returns>String value on date for DatePicker control</returns>
        public static string FormatDatePicker(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return string.Empty;
            }
            else
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{0:00}/{1:00}/{2:0000}",
                    Convert.ToInt32(date.Substring(0, 2), CultureInfo.GetCultureInfo("en-us")),
                    Convert.ToInt32(date.Substring(2, 2), CultureInfo.GetCultureInfo("en-us")),
                    Convert.ToInt32(date.Substring(6, 4), CultureInfo.GetCultureInfo("en-us")));
            }
        }

        public static string FormatDatePicker(DateTime date)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0:dd/MM/yyyy}", date);
        }

        /// <summary>
        /// Generates date value on text for DatePicker control
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Formatted text for DatePicker control</returns>
        public static string FormatDatePicker(DateTime? date)
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }

            return FormatDatePicker(date.Value);
        }

        public static string FormatTimePicker(DateTime date)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0:hh:MM}", date);
        }

        /// <summary>
        /// Generates date value on text for DatePicker control
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Formatted text for DatePicker control</returns>
        public static string FormatTimePicker(DateTime? date)
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }

            return FormatTimePicker(date.Value);
        }

        public static string SQLJSONStream(string storedName, string instanceName)
        {
            string res = "[]";
            if (!string.IsNullOrEmpty(storedName))
            {
                CustomerFramework instance = new CustomerFramework() { Name = instanceName };
                instance.LoadConfig();
                using (SqlCommand cmd = new SqlCommand(storedName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(instance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        res = SQLJSONStream(cmd);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Get JSON format data from SQL command
        /// </summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLJSONStream(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return "[]";
            }

            StringBuilder res = new StringBuilder("[");
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (rdr.Read())
                    {
                        //// if (!rdr.IsDBNull(0))
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                res.Append(",");
                            }

                            res.Append(Environment.NewLine);
                            res.Append(rdr.GetString(0));
                        }
                    }
                }

                res.Append("]");
            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.core.Tools.Basics.SQLJsoStream(" + cmd.CommandText + ")");
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        public static string SQLToJSON(string storedName)
        {
            if (string.IsNullOrEmpty(storedName))
            {
                return "[]";
            }

            string res = "[]";
            CustomerFramework customerConfig = HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
            string connectionString = customerConfig.Config.ConnectionString;
            res = SQLToJSON(storedName, connectionString);
            return res;
        }

        /// <summary>
        /// Obtains a JSON array structure of stored procedure results
        /// </summary>
        /// <param name="storedName">Stored procedure name</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>JSON array structure</returns>
        public static string SQLToJSON(string storedName, string connectionString)
        {
            if (string.IsNullOrEmpty(storedName) || string.IsNullOrEmpty(connectionString))
            {
                return "[]";
            }

            string res = "[]";
            using (SqlCommand cmd = new SqlCommand(storedName))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLToJSON(cmd);
                }
            }

            return res;
        }
        
         /// <summary>
         /// Get JSON format data from SQL command
         /// </summary>
         /// <param name="cmd">SQL command</param>
         /// <returns>An "application/json" format of data</returns>
        public static string SQLStreamSimple(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return "{}";
            }

            StringBuilder res = new StringBuilder();
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res.AppendFormat(CultureInfo.InvariantCulture, @"{{{0}}}", rdr.GetString(0));
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.Basics.SQLToJSONSimple(" + cmd.CommandText + ")");
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>
        /// Get JSON format data from SQL command
        /// </summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLStreamList(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return "[]";
            }

            StringBuilder res = new StringBuilder();
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        res.Append("[");
                        var first = true;
                        while (rdr.Read())
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                res.Append(",");
                            }

                            res.AppendFormat(CultureInfo.InvariantCulture, @"{{{0}}}", rdr.GetString(0));
                        }

                        res.Append("]");
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.Basics.SQLToJSONSimple(" + cmd.CommandText + ")");
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>
        /// Get JSON format data from SQL command
        /// </summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLToJSON(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return "[]";
            }

            StringBuilder res = new StringBuilder("[");
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (rdr.Read())
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        res.Append(Environment.NewLine);
                        res.Append("    {");

                        for (int x = 0; x < rdr.FieldCount; x++)
                        {
                            if (x > 0)
                            {
                                res.Append(",");
                            }

                            res.Append(Environment.NewLine);
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"        ""{0}"":",
                                rdr.GetName(x));

                            string fieldType = rdr.GetFieldType(x).ToString().ToUpperInvariant();

                            if (rdr.IsDBNull(x))
                            {
                                res.Append("null");
                            }
                            else
                            {
                                switch (fieldType)
                                {
                                    case "SYSTEM.BOOLEAN":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0}",
                                            rdr.GetBoolean(x) ? ConstantValue.True : ConstantValue.False);
                                        break;
                                    case "SYSTEM.INT16":
                                    case "SYSTEM.INT32":
                                    case "SYSTEM.INT64":
                                    case "SYSTEM.DECIMAL":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0}",
                                            rdr[x]);
                                        break;
                                    case "SYSTEM.STRING":
                                    default:
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"""{0}""",
                                            ToolsJson.JsonCompliant(rdr[x].ToString()));
                                        break;
                                }
                            }
                        }

                        res.Append(Environment.NewLine);
                        res.Append("    }");
                    }
                }

                res.Append("]");
            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.Basics.SQLToJSON(" + cmd.CommandText + ")");
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>
        /// Get CSV format data from SQL command
        /// </summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>Plain text in CSV format</returns>
        public static string SqlToCSV(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return string.Empty;
            }

            StringBuilder res = new StringBuilder();
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (rdr.Read())
                    {
                        if (first)
                        {
                            first = false;
                            for (int x = 0; x < rdr.FieldCount; x++)
                            {
                                res.AppendFormat(CultureInfo.InvariantCulture, @"{0};", rdr.GetName(x));
                            }
                        }

                        res.Append(Environment.NewLine);
                        for (int x = 0; x < rdr.FieldCount; x++)
                        {
                            string fieldType = rdr.GetFieldType(x).ToString().ToUpperInvariant();

                            if (rdr.IsDBNull(x))
                            {
                                res.Append(";");
                            }
                            else
                            {
                                switch (fieldType)
                                {
                                    case "SYSTEM.BOOLEAN":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0};",
                                            rdr.GetBoolean(x) ? ConstantValue.True : ConstantValue.False);
                                        break;
                                    case "SYSTEM.INT16":
                                    case "SYSTEM.INT32":
                                    case "SYSTEM.INT64":
                                    case "SYSTEM.DECIMAL":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0};",
                                            rdr[x]);
                                        break;
                                    case "SYSTEM.STRING":
                                    default:
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"""{0}"";",
                                            rdr[x]);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.core.Tools.SqlToCsv(" + cmd.CommandText + ")");
                return ex.Message;
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.core.Tools.SqlToCsv(" + cmd.CommandText + ")");
                return ex.Message;
            }
            catch (InvalidCastException ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.core.Tools.SqlToCsv(" + cmd.CommandText + ")");
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.LogException(ex, "OpenFramework.core.Tools.SqlToCsv(" + cmd.CommandText + ")");
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }
    }
}