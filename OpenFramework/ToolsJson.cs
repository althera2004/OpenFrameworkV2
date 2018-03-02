// --------------------------------
// <copyright file="ToolsJson.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework
{
    using System;
    using System.Globalization; 
    using OpenFramework.Security;

    public static class ToolsJson
    {
        public static string JsonCompliant(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.Replace("\n", "\\n");
            return value.Replace("\"", "\\\"");
        }

        public static string JsonCompliant(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string text = value.ToString();
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("\n", "\\n").Replace("\"", "\\\"");
        }

        public static string JsonNull(string key)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": null", key);
        }

        public static string Json(string key, string value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": ""{1}""", key, JsonCompliant(value));
        }

        public static string Json(string key, int value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value);
        }

        public static string Json(string key, int? value)
        {
            if (!value.HasValue)
            {
                return JsonNull(key);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value);
        }

        public static string Json(string key, DateTime? value)
        {
            if (!value.HasValue)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": null", key);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": ""{1:dd/MM/yyyy}""", key, value.Value);
        }

        public static string Json(string key, decimal value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value).Replace(',', '.');
        }

        public static string Json(string key, long value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value);
        }

        public static string Json(string key, long? value)
        {
            if (!value.HasValue)
            {
                return JsonNull(key);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value);
        }

        public static string Json(string key, ApplicationUser value)
        {
            if (value == null)
            {
                return JsonNull(key);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value.JsonKeyValue);
        }

        public static string Json(string key, bool value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1}", key, value ? "true" : "false");
        }        
    }
}
