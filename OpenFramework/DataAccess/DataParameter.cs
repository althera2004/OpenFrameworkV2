// --------------------------------
// <copyright file="DataParameter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.DataAccess
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    /// <summary>Create a SQL Parameter</summary>
    public static class DataParameter
    {
        /// <summary>Gets a input SQL Parameter with null value</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter InputNull(string name)
        {
            return new SqlParameter(SetName(name), SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = DBNull.Value
            };
        }

        /// <summary>Gets a input SQL Parameter with string value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">string value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, string value)
        {
            SqlParameter res = new SqlParameter(SetName(name), SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input
            };

            if (string.IsNullOrEmpty(value))
            {
                res.Value = string.Empty;
            }
            else
            {
                res.Value = value;
            }

            return res;
        }

        /// <summary>Gets a input SQL Parameter with string value with length limit</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">String value</param>
        /// <param name="length">Limit length of string value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            return new SqlParameter(SetName(name), SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = Basics.LimitedText(value, length)
            };
        }

        /// <summary>Gets a input SQL Parameter with integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Integer value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, int value)
        {
            return new SqlParameter(name, SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SQL Parameter with nullable integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Integer nullable value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, int? value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Int)
            {
                Direction = ParameterDirection.Input
            };

            if (value.HasValue)
            {
                res.Value = value.Value;
            }
            else
            {
                res.Value = DBNull.Value;
            }

            return res;
        }

        /// <summary>Gets a input SQL Parameter with nullable integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Decimal nullable value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, decimal? value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Input
            };

            if (value.HasValue)
            {
                res.Value = value.Value;
            }
            else
            {
                res.Value = DBNull.Value;
            }

            return res;
        }

        /// <summary>Gets a input SQL Parameter with float value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Float value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, float value)
        {
            return new SqlParameter(name, SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SQL Parameter with long value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Long value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, long value)
        {
            return new SqlParameter(name, SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SQL Parameter with decimal value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Decimal value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, decimal value)
        {
            return new SqlParameter(name, SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input parameter with boolean value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Boolean value</param>
        /// <returns>Command parameter</returns>
        public static SqlParameter Input(string name, bool value)
        {
            return new SqlParameter(name, SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input parameter with date/time value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime value</param>
        /// <returns>Command parameter</returns>
        public static SqlParameter Input(string name, DateTime value)
        {
            return new SqlParameter(name, SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input parameter with nullable DateTime value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime nullable value</param>
        /// <returns>Command parameter</returns>
        public static SqlParameter InputDate(string name, DateTime value)
        {
            return Input(name, string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:yyyyMMdd}", value));
        }

        /// <summary>Gets a input parameter with nullable DateTime value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime nullable value</param>
        /// <returns>Command parameter</returns>
        public static SqlParameter InputDate(string name, DateTime? value)
        {
            if (!value.HasValue)
            {
                return InputNull(name);
            }

            return Input(name, string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:yyyyMMdd}", value.Value));
        }

        /// <summary>Gets a input SQL Parameter with nullable DateTime value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime nullable value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter Input(string name, DateTime? value)
        {
            if (!value.HasValue)
            {
                return InputNull(name);
            }

            return Input(name, value.Value);
        }

        /// <summary>Gets a input SQL Parameter with text value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Text value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter InputText(string name, string value)
        {
            return new SqlParameter(name, SqlDbType.Text)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a output SQL Parameter with string value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="size">Size of returned value</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter OutputString(string name, int size)
        {
            return new SqlParameter(SetName(name), SqlDbType.NVarChar, size)
            {
                Direction = ParameterDirection.Output,
                Value = DBNull.Value
            };
        }

        /// <summary>Gets a output SQL Parameter with integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter OutputInt(string name)
        {
            return new SqlParameter(name, SqlDbType.Int)
            {
                Direction = ParameterDirection.Output,
                Value = DBNull.Value
            };
        }

        /// <summary>Gets a output SQL Parameter with long value</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>SQL parameter</returns>
        public static SqlParameter OutputLong(string name)
        {
            return new SqlParameter(name, SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output,
                Value = DBNull.Value
            };
        }

        /// <summary>Determines if parameter name starts with "@", else adds "@" before name</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>normalized parameter name</returns>
        private static string SetName(string name)
        {
            if (!name.StartsWith("@", StringComparison.OrdinalIgnoreCase))
            {
                name = "@" + name;
            }

            return name;
        }
    }
}