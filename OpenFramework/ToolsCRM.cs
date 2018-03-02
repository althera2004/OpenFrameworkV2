// --------------------------------
// <copyright file="ToolsCRM.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework
{
    using System;
    using System.Globalization;
    using CRM = Microsoft.Xrm.Sdk;

    public static class ToolsCRM
    {
        public static string GetReferenceValue(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            return ((CRM.EntityReference)entity[fieldName]).Name;
        }

        public static Guid GetReferenceId(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return Guid.Empty;
            }

            if (!entity.Contains(fieldName))
            {
                return Guid.Empty;
            }

            return ((CRM.EntityReference)entity[fieldName]).Id;
        }

        public static string GetAliasedValue(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            CRM.AliasedValue alias = (CRM.AliasedValue)entity[fieldName];

            if (alias.Value.GetType().ToString() == "System.Guid" || alias.Value.GetType().ToString() == "System.String" || alias.Value.GetType().ToString() == "System.Boolean" || alias.Value.GetType().ToString() == "System.Int32")
            {
                return alias.Value.ToString();
            }

            if (alias.Value.GetType().ToString() == "System.Double")
            {
                return Convert.ToDouble(alias.Value, CultureInfo.GetCultureInfo("en-us")).ToString("###0.000000", CultureInfo.GetCultureInfo("en-us")).Replace(',', '.');
            }

            CRM.EntityReference referencia = (CRM.EntityReference)alias.Value;

            return referencia.Name;
        }

        public static Guid GetAliasedId(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return Guid.Empty;
            }

            if (!entity.Contains(fieldName))
            {
                return Guid.Empty;
            }

            CRM.AliasedValue alias = (CRM.AliasedValue)entity[fieldName];

            switch (alias.Value.GetType().ToString())
            {
                case "Microsoft.Xrm.Sdk.EntityReference":
                    return ((CRM.EntityReference)alias.Value).Id;
                case "System.Guid":
                    return new Guid(alias.Value.ToString());
            }

            return Guid.Empty;
        }

        public static string GetOptionValue(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            CRM.OptionSetValue option = (CRM.OptionSetValue)entity[fieldName];

            return option.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetStringValue(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            return entity[fieldName].ToString();
        }

        public static decimal GetMoneyValue(CRM.Entity entity, string fieldName)
        {
            if (entity == null || string.IsNullOrEmpty(fieldName))
            {
                return 0;
            }

            if (!entity.Contains(fieldName))
            {
                return 0;
            }

            CRM.Money data = (CRM.Money)entity[fieldName];
            return Convert.ToDecimal(data.Value);
        }
    }
}
