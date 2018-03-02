// --------------------------------
// <copyright file="Excel.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.Export
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json.Linq;
    using NPOI.XSSF.UserModel;
    using OpenFramework.Core;
    using OpenFramework.CRUD;
    using OpenFramework.Customer;

    /// <summary>Implements class to export item data to Excel file</summary>    
    public sealed class Excel
    {
        /// <summary>ItemBuilder target of exportation</summary>
        private ItemBuilder itemBuilder;

        /// <summary> Sheet of work book destination of exported data</summary>
        private XSSFSheet sheet;

        /// <summary> Work book of excel destination of exported data</summary>
        private XSSFWorkbook workBook;

        /// <summary>Gets the row header for CSV file</summary>
        public void CreateHeader()
        {
            if (this.sheet.GetRow(0) == null)
            {
                this.sheet.CreateRow(0);
            }

            int cont = 0;
            foreach (ItemField field in this.itemBuilder.HumanFields)
            {
                this.sheet.GetRow(0).CreateCell(cont);
                this.sheet.GetRow(0).GetCell(cont).SetCellValue(field.Label);
                cont++;
            }
        }

        /// <summary>
        /// Gets a row data for excel file
        /// </summary>
        /// <param name="data">Data to show in row</param>
        /// <param name="index">Index of row in sheet</param>
        public void CreateRow(dynamic data, int index)
        {
            if (data == null)
            {
                return;
            }

            if (this.sheet.GetRow(index) == null)
            {
                this.sheet.CreateRow(index);
            }

            var values = (IDictionary<string, object>)data;
            int cont = 0;
            foreach (ItemField field in this.itemBuilder.Definition.Fields)
            {
                this.sheet.GetRow(index).CreateCell(cont);
                this.sheet.GetRow(index).GetCell(cont).SetCellValue(values[field.Name].ToString());
                cont++;
            }
        }

        public ActionResult ExportHuman(string itemName, string instanceName)
        {
            CustomerFramework instance = new CustomerFramework() { Name = instanceName };
            instance.LoadConfig();
            return ExportHuman(itemName, instanceName, instance.Config.ConnectionString);
        }

        /// <summary>
        /// Create a list of ItemBuilder objects and export them in a Excel file
        /// </summary>
        /// <param name="itemName">Item name</param>
        /// <returns>Result of action</returns>
        public ActionResult ExportHuman(string itemName,string instanceName,string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Excel:List({0})", itemName);
            this.itemBuilder = new ItemBuilder(itemName, instanceName);
            var res = ActionResult.NoAction;
            try
            {
                this.workBook = new XSSFWorkbook();
                this.sheet = (XSSFSheet)this.workBook.CreateSheet(this.itemBuilder.Definition.Layout.LabelPlural);

                // HEADER
                int countCells = 0;
                this.CreateHeader();

                ReadOnlyCollection<ItemBuilder> list = Read.Active(this.itemBuilder.Definition, instanceName);
                int countRows = 1;
                foreach (ItemBuilder item in list)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    ToolsXlsx.CreateRow(this.sheet, countRows);
                    countCells = 0;
                    foreach (ItemField field in this.itemBuilder.HumanFields)
                    {
                        this.sheet.GetRow(countRows).CreateCell(countCells);
                        if (item.ContainsKey(field.Name) && item[field.Name] != null)
                        {
                            if (item[field.Name].GetType().Name.ToUpperInvariant() == "JOBJECT")
                            {
                                string data = item[field.Name].ToString();
                                JObject x = item[field.Name] as JObject;
                                if (x["Description"] != null)
                                {
                                    data = x["Description"].ToString();
                                }

                                this.sheet.GetRow(countRows).GetCell(countCells).SetCellValue(data);
                            }
                            else
                            {
                                string fieldName = string.Empty;
                                if (item.Definition.Fields.Any(f => f.Name == field.Name))
                                {
                                    fieldName = field.Name;
                                }
                                else
                                {
                                    if (item.Definition.Fields.Any(f => f.Name == field.Name + "Id"))
                                    {
                                        fieldName = string.Format(CultureInfo.InvariantCulture, "{0}Id", field.Name);
                                    }

                                    fieldName = field.Name;
                                }

                                if (!string.IsNullOrEmpty(fieldName))
                                {
                                    ItemField fieldDefinition = item.Definition.Fields.Where(f => f.Name == fieldName).First();
                                    this.SetCellValue(countRows, countCells, item[fieldName], fieldDefinition.DataType);
                                }
                            }
                        }

                        countCells++;
                    }

                    countRows++;
                }

                string path = HttpContext.Current.Request.PhysicalApplicationPath;
                if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
                }

                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\Temp\{1}.{2}", path, this.itemBuilder.Definition.Layout.LabelPlural, ConstantValue.Excel2007Extension);
                string link = string.Format(CultureInfo.GetCultureInfo("en-us"), @"/Temp/{0}.{1}", this.itemBuilder.Definition.Layout.LabelPlural, ConstantValue.Excel2007Extension);

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    this.workBook.Write(fs);
                }

                res.SetSuccess(link);
            }
            catch (IOException ex)
            {
                res.SetFail(ex);
                ExceptionManager.Trace(ex, source);
            }
            catch (NullReferenceException ex)
            {
                res.SetFail(ex);
                ExceptionManager.Trace(ex, source);
            }
            catch (NotSupportedException ex)
            {
                res.SetFail(ex);
                ExceptionManager.Trace(ex, source);
            }

            return res;
        }
        /// <summary>
        /// Create a list of ItemBuilder objects and export them in a Excel file in order to be reimported
        /// </summary>
        /// <param name="itemName">Item name</param>
        /// <returns>Result of action</returns>
        public ActionResult ExportToImport(string itemName, string instanceName)
        {
            CustomerFramework instance = new CustomerFramework() { Name = instanceName };
            instance.LoadConfig();
            return ExportToImport(itemName, instanceName, instance.Config.ConnectionString);
        }

        /// <summary>
        /// Create a list of ItemBuilder objects and export them in a Excel file in order to be reimported
        /// </summary>
        /// <param name="itemName">Item name</param>
        /// <returns>Result of action</returns>
        public ActionResult ExportToImport(string itemName, string instanceName, string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Excel:List({0})", itemName);
            this.itemBuilder = new ItemBuilder(itemName, instanceName);
            var res = ActionResult.NoAction;
            try
            {
                this.workBook = new XSSFWorkbook();
                this.sheet = (XSSFSheet)this.workBook.CreateSheet(this.itemBuilder.Definition.Layout.LabelPlural);

                // HEADER
                int countCells = 0;
                this.CreateHeader();

                ReadOnlyCollection<ItemBuilder> list = Read.Active(this.itemBuilder.Definition, instanceName);
                int countRows = 1;
                foreach (ItemBuilder item in list)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    ToolsXlsx.CreateRow(this.sheet, countRows);
                    countCells = 0;
                    foreach (ItemField field in this.itemBuilder.HumanFields)
                    {
                        this.sheet.GetRow(countRows).CreateCell(countCells);
                        
                        // Detectar si es FK
                        if (item.ContainsKey(field.Name) && item[field.Name] != null)
                        {
                            if (this.itemBuilder.Definition.ForeignValues.Any(fv => fv.LocalName.Equals(field.Name, StringComparison.OrdinalIgnoreCase) || fv.LocalName.Equals(field.Name + "Id", StringComparison.OrdinalIgnoreCase)))
                            {
                                ForeignList foreignValue = this.itemBuilder.Definition.ForeignValues.Where(fv => fv.LocalName.Equals(field.Name, StringComparison.OrdinalIgnoreCase) || fv.LocalName.Equals(field.Name + "Id", StringComparison.OrdinalIgnoreCase)).First();

                                long foreignId = 0;
                                if (item[field.Name].GetType().Name.ToUpperInvariant() == "JOBJECT")
                                {
                                    foreignId = (long)item[field.Name + "Id"];
                                }
                                else
                                {
                                    foreignId = (long)item[field.Name];
                                }

                                string foreignItem = foreignValue.ItemName;
                                ItemBuilder foreignItemFound = Read.ById(foreignId, this.itemBuilder.Definition, instanceName);

                                if (foreignItemFound != null)
                                {
                                    this.SetCellValue(countRows, countCells, foreignItemFound[foreignValue.ImportReference], FieldDataType.Text);
                                }
                                else
                                {
                                    this.SetCellValue(countRows, countCells, null, FieldDataType.Text);
                                }
                            }
                            else
                            {
                                if (item[field.Name].GetType().Name.ToUpperInvariant() == "JOBJECT")
                                {
                                    string data = item[field.Name].ToString();
                                    JObject x = item[field.Name] as JObject;
                                    if (x["Description"] != null)
                                    {
                                        data = x["Description"].ToString();
                                    }

                                    this.sheet.GetRow(countRows).GetCell(countCells).SetCellValue(data);
                                }
                                else
                                {
                                    string fieldName = string.Empty;
                                    if (item.Definition.Fields.Any(f => f.Name == field.Name))
                                    {
                                        fieldName = field.Name;
                                    }
                                    else
                                    {
                                        if (item.Definition.Fields.Any(f => f.Name == field.Name + "Id"))
                                        {
                                            fieldName = string.Format(CultureInfo.InvariantCulture, "{0}Id", field.Name);
                                        }

                                        fieldName = field.Name;
                                    }

                                    if (!string.IsNullOrEmpty(fieldName))
                                    {
                                        ItemField fieldDefinition = item.Definition.Fields.Where(f => f.Name == fieldName).First();
                                        this.SetCellValue(countRows, countCells, item[fieldName], fieldDefinition.DataType);
                                    }
                                }
                            }
                        }

                        countCells++;
                    }

                    countRows++;
                }

                string path = HttpContext.Current.Request.PhysicalApplicationPath;
                if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
                }

                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\Temp\ToImport_{1}.{2}", path, this.itemBuilder.Definition.Layout.LabelPlural, ConstantValue.Excel2007Extension);
                string link = string.Format(CultureInfo.GetCultureInfo("en-us"), @"/Temp/ToImport_{0}.{1}", this.itemBuilder.Definition.Layout.LabelPlural, ConstantValue.Excel2007Extension);

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    this.workBook.Write(fs);
                }

                res.SetSuccess(link);
            }
            catch (IOException ex)
            {
                res.SetFail(ex);
                ExceptionManager.Trace(ex, source);
            }
            catch (NullReferenceException ex)
            {
                res.SetFail(ex);
                ExceptionManager.Trace(ex, source);
            }
            catch (NotSupportedException ex)
            {
                res.SetFail(ex);
                ExceptionManager.Trace(ex, source);
            }

            return res;
        }

        /// <summary>
        /// Gets final cell value
        /// </summary>
        /// <param name="rowIndex">Index of row</param>
        /// <param name="cellIndex">Index of cell</param>
        /// <param name="value">Original value</param>
        /// <param name="dataType">Value type</param>
        private void SetCellValue(int rowIndex, int cellIndex, object value, FieldDataType dataType)
        {
            switch (dataType)
            {
                case FieldDataType.Boolean:
                case FieldDataType.NullableBoolean:
                    this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
                    break;
                case FieldDataType.Integer:
                case FieldDataType.NullableInteger:
                    this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(Convert.ToInt32(value, CultureInfo.InvariantCulture));
                    break;
                case FieldDataType.Long:
                case FieldDataType.NullableLong:
                    this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
                    break;
                case FieldDataType.Float:
                case FieldDataType.NullableFloat:
                    this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(Convert.ToSingle(value, CultureInfo.InvariantCulture));
                    break;
                case FieldDataType.Decimal:
                case FieldDataType.NullableDecimal:
                    this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
                    break;
                case FieldDataType.DateTime:
                case FieldDataType.NullableDateTime:
                case FieldDataType.Time:
                case FieldDataType.NullableTime:
                    this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
                    break;
                case FieldDataType.Guid:
                case FieldDataType.Url:
                case FieldDataType.Email:
                case FieldDataType.Text:
                case FieldDataType.Textarea:
                default:
                    if (value == null)
                    {
                        this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(string.Empty);
                    }
                    else
                    {
                        this.sheet.GetRow(rowIndex).GetCell(cellIndex).SetCellValue(value.ToString());
                    }

                    break;
            }
        }
    }
}