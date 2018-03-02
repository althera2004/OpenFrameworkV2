// --------------------------------
// <copyright file="Csv.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.Export
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using OpenFramework.Core;
    using OpenFramework.ItemManager;
    using OpenFramework.CRUD;

    /// <summary>Implements class to export item data to CSV file</summary>
    public sealed class Csv
    {
        /// <summary>ItemBuilder target of exportation</summary>
        private ItemBuilder itemBuilder;

        /// <summary>Create a list of ItemBuilder objects and export them in a CSV file</summary>
        /// <param name="itemName">Item name</param>
        /// <returns>Result of action</returns>
        public ActionResult List(string itemName,string instanceName, string connectionString)
        {
            this.itemBuilder = new ItemBuilder(itemName, instanceName);
            StringBuilder fileContent = new StringBuilder(this.CreateHeader());
            var res = ActionResult.NoAction;

            ItemDefinition definition = ItemDefinition.Load(itemName, instanceName);
            var list = Read.All(definition, instanceName);
            foreach (dynamic data in list)
            {
                if (data == null)
                {
                    continue;
                }

                fileContent.Append(this.CreateRow(data));
            }

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}\{2}\{1}.{3}", path, this.itemBuilder.Definition.Layout.LabelPlural, ConstantValue.TemporalFolder, ConstantValue.CommaSeparatedValueExtension);
            string link = string.Format(CultureInfo.InvariantCulture, @"/{1}/{0}.{2}", this.itemBuilder.Definition.Layout.LabelPlural, ConstantValue.TemporalFolder, ConstantValue.CommaSeparatedValueExtension);

            using (StreamWriter fs = new StreamWriter(path, false))
            {
                fs.Write(fileContent);
            }

            res.SetSuccess(link);
            return res;
        }

        /// <summary>
        /// Gets the row header for CSV file
        /// </summary>
        /// <returns>A string with the row header for CSV file</returns>
        public string CreateHeader()
        {
            StringBuilder res = new StringBuilder();
            foreach (ItemField field in this.itemBuilder.Definition.Fields)
            {
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "{0};", field.Label);
            }

            return res.ToString();
        }

        /// <summary>
        /// Gets a row data for CSV file
        /// </summary>
        /// <param name="data">Data to show in row</param>
        /// <returns>A string with row data for CSV file</returns>
        public string CreateRow(dynamic data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            StringBuilder res = new StringBuilder();
            var values = (IDictionary<string, object>)data;
            foreach (ItemField field in this.itemBuilder.Definition.Fields)
            {
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "{0};", values[field.Name].ToString());
            }

            return res.ToString();
        }
    }
}
