// --------------------------------
// <copyright file="ItemUpload.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OpenFramework;
using OpenFramework.Core;
using OpenFramework.Core.ItemManager.List;
using OpenFramework.ItemManager;
using OpenFramework.Security;
using OpenFramework.Customer;

public partial class Data_ItemUpload : Page
{
    private CustomerFramework instance;

    public ItemBuilder Item { get; private set; }

    private List<ItemBuilder> itemsReaded;

    /// <summary>Import identifier</summary>
    private Guid importId;

    private List<DataLine> dataFile;

    private List<Error> errors;

    private List<int> requiredIndex;

    private List<FieldDataType> typeIndex;

    private List<ItemField> itemFields;

    /// <summary>Load event of page</summary>
    /// <param name="sender">Page loaded</param>
    /// <param name="e">Arguments of event</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Go();
    }

    /// <summary>Continues PageLoad execution if session is alive</summary>
    private void Go()
    {        
        string res = string.Empty;
        if (this.Request.QueryString.Count == 0)
        {
            string instanceName = string.Empty;
            if (this.Request.Form["InstanceName"] != null)
            {
                instanceName = this.Request.Form["InstanceName"];
            }

            this.instance = CustomerFramework.Load(instanceName);
            res = "No Action";
            this.errors = new List<Error>();
            this.dataFile = new List<DataLine>();
            if (!IsPostBack)
            {
                if (this.Request.Form["itemName"] != null)
                {
                    this.Item = new ItemBuilder(this.Request.Form["itemName"], this.instance.Name);
                }

                string file = SaveUploadedFile(Request.Files);

                if (!string.IsNullOrEmpty(file))
                {
                    file = Path.GetFileName(file);
                }

                this.dataFile = this.dataFile.OrderBy(d => d.Line).ToList();
                this.errors = this.errors.OrderBy(d => d.Linea).ToList();

                res = "{\"file\":\"" + file + "\",\"errors\":[";
                bool first = true;
                foreach (var er in errors)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res += ",";
                    }

                    res += string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""Type"":""{0}"",""Line"":{1},""Message"":""{2}""}}",
                        er.ErrorType,
                        er.Linea,
                        ToolsJson.JsonCompliant(er.Message));
                }

                res += "],\"data\":[";

                if (this.errors.Count == 0)
                {
                    first = true;
                    foreach (var dl in this.dataFile)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res += ",";
                        }

                        res += string.Format(
                            CultureInfo.InvariantCulture,
                            @"{{""Type"":""Data"",""Line"":{0},""Message"":{1}}}",
                            dl.Line,
                            dl.Data);
                    }
                }

                res += string.Format(CultureInfo.InvariantCulture, @"],""ImportId"":""{0}""}}", new Guid());
            }

            if (this.errors.Count == 0)
            {
                HttpContext.Current.Session["Import" + this.importId] = this.itemsReaded;
            }

            this.Response.Clear();
            this.Response.Write(res);
            this.Response.Flush();
            this.Response.SuppressContent = true;
            this.ApplicationInstance.CompleteRequest();
        }
        else
        {
            var codedQuery = new CodedQuery();
            codedQuery.SetQuery(this.Request.QueryString);
            string file = codedQuery.GetByKey<string>("file");
            string itemName = codedQuery.GetByKey<string>("item");
            string importId = codedQuery.GetByKey<string>("importId");
            string instanceName = codedQuery.GetByKey<string>("InstanceName");
            string resImport = ImportXlsx(file, importId, itemName);
            res = "Upload<br />" + file + "<br />" + itemName;

            this.Response.Clear();
            this.Response.Write(resImport);
            this.Response.Flush();
            this.Response.SuppressContent = true;
            this.ApplicationInstance.CompleteRequest();
        }
    }

    public string SaveUploadedFile(HttpFileCollection httpFileCollection)
    {
        foreach (string fileName in httpFileCollection)
        {
            var file = httpFileCollection.Get(fileName);

            // Save file content goes here
            if (file == null || file.ContentLength == 0)
            {
                continue;
            }

            string path = this.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\"))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\Temp\", path);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}Temp\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, "{0}{1}", path, file.FileName);
            file.SaveAs(path);

            this.ReadXlsx(path, this.instance.Config.ConnectionString);
            return path;
        }

        return string.Empty;
    }

    public bool ParseHeader(IRow header)
    {
        var dataHeader = new DataLine() { Line = 0, Data = string.Empty };
        var requiredNotFound = new List<string>();
        var fileFields = new List<string>();
        this.itemFields = new List<ItemField>();
        var headerNotField = new List<string>();

        var data = new StringBuilder("[");
        bool firstLabel = true;
        foreach (ICell cell in header.Cells)
        {
            string fieldLabel = cell.ToString().Replace("*", string.Empty);
            fileFields.Add(fieldLabel);

            if (this.Item.Definition.Fields.Any(f => f.Label.ToUpperInvariant() == fieldLabel.ToUpperInvariant() && f.Name != "Id"))
            {
                if (firstLabel)
                {
                    firstLabel = false;
                }
                else
                {
                    data.Append(",");
                }

                data.AppendFormat(CultureInfo.InvariantCulture, @"""{0}""", ToolsJson.JsonCompliant(fieldLabel));
                itemFields.Add(this.Item.Definition.Fields.First(f => f.Label.ToUpperInvariant() == fieldLabel.ToUpperInvariant() && f.Name != "Id" && f.Name != "CompanyId"));
            }
        }

        data.Append("]");
        dataHeader.Data = data.ToString();
        if (this.dataFile != null)
        {
            this.dataFile.Add(dataHeader);
        }

        bool errors = false;
        bool allRequired = true;
        foreach (var field in this.Item.Definition.Fields.Where(f => f.Required))
        {
            if (field.Name != "Id" && field.Name != "CompanyId")
            {
                if (!fileFields.Any(s => s == field.Label))
                {
                    allRequired = false;
                    errors = true;
                    requiredNotFound.Add(field.Label);
                }
            }
        }

        if (!allRequired)
        {
            var error = new Error { ErrorType = "Structure", Linea = 0 };
            var res = new StringBuilder("Los siguientes campos obligatorios no están en el fichero:");
            bool first = true;
            foreach (string r in requiredNotFound)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(r);
            }

            error.Message = res.ToString();
            this.errors.Add(error);
        }

        bool allHeadersAreFields = true;
        foreach (string h in fileFields)
        {
            if (this.Item.Definition.Fields.Any(f => f.Label == h))
            {
                continue;
            }

            allHeadersAreFields = false;
            headerNotField.Add(h);
            errors = true;
        }

        if (!allHeadersAreFields)
        {
            var error = new Error() { ErrorType = "Structure", Linea = 0 };
            var res = new StringBuilder();
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                "Los siguientes datos del fichero no son campos de {0}:",
                this.Item.Definition.Layout.Label);
            bool first = true;
            foreach (string r in headerNotField)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(r);
            }

            error.Message = res.ToString();
            this.errors.Add(error);
        }

        if (!errors)
        {
            int contCell = 0;
            foreach (var field in itemFields)
            {
                this.typeIndex.Add(field.DataType);
                if (field.Required)
                {
                    this.requiredIndex.Add(contCell);
                }

                contCell++;
            }
        }

        return errors;
    }

    public bool ReadXlsx(string fileName, string connectionString)
    {
        this.itemsReaded = new List<ItemBuilder>();
        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
        {
            path += "\\LoadTemp\\";
        }
        else
        {
            path += "LoadTemp\\";
        }

        var errorMessages = new List<string>();
        this.requiredIndex = new List<int>();
        this.typeIndex = new List<FieldDataType>();

        string headerJson = string.Empty;
        XSSFWorkbook hssfwb;
        using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            hssfwb = new XSSFWorkbook(file);
            var sheet = hssfwb.GetSheetAt(0);
            bool headerError = ParseHeader(sheet.GetRow(0));
            var rows = new List<IRow>();

            for (int contRows = sheet.FirstRowNum + 1; contRows < sheet.LastRowNum + 1; contRows++)
            {
                rows.Add( sheet.GetRow(contRows));
            }

            int parsedRows = sheet.FirstRowNum + 1;
            var primaryKeys = new List<string>();
            try
            {
                foreach(var currentRow in rows)
                {
                    var data = Parse(currentRow, parsedRows, this.Item.Definition, primaryKeys);
                    parsedRows++;
                }
            }
            catch (Exception ex)
            {
                this.errors.Add(new Error()
                {
                    Linea = parsedRows,
                    ErrorType = "Data",
                    Message = "La fila no es válida"
                });
            }

            /*Parallel.ForEach(rows, (currentRow)=>{
                dataLine data = Parse(currentRow, parsedRows, this.item.Definition, this.item.InstanceName);
                parsedRows++;
            });*/
        }   

        return true;
    }

    public string ImportXlsx(string fileName, string importId, string itemName)
    {
        var res = new StringBuilder();
        var d0 = DateTime.Now;
        this.Item = new ItemBuilder(itemName, this.instance.Name);

        if (HttpContext.Current.Session["Import" + importId] == null)
        {
            return "No hay fichero de carga.";
        }

        var items = HttpContext.Current.Session["Import" + importId] as List<ItemBuilder>;

        int inserted = 0;
        int updated = 0;
        string errors = string.Empty;

        //CustomerFramework customerConfig = Basics.ActualInstance;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;

        res.AppendFormat(
            CultureInfo.InvariantCulture,
            "<h1>Resultado de la importaci&oacute;n</h1>Insertados:&nbsp<strong>{0}</strong><br />Actualizados:&nbsp;<strong>{1}</strong><hr /><a href=\"#\" title=\"\" onclick=\"GoImportList('SupportedEnergyInvoiceLine');ClearAllUnsavedData();\"><i class=\"fa fa-lg fa-fw fa-{2}\"></i><span class=\"menu-item-parent\">{3}</span></a><hr />{5}",
            inserted,
            updated,
            this.Item.Definition.Layout.Icon,
            this.Item.Definition.Layout.LabelPlural,
            errors);

        HttpContext.Current.Session["Import" + importId] = null;
        return res.ToString();
    }

    private DataLine Parse(IRow data, int index, ItemDefinition definition, List<string> primaryKeys)
    {
        var d0 = DateTime.Now;
        var res = new DataLine() { Line = index, Data = string.Empty };
        if (data.Cells.Count > this.typeIndex.Count)
        {
            this.errors.Add(new Error()
            {
                Linea = index,
                ErrorType = "Data",
                Message = "El número de celdas no es correcto"
            });
        }
        else
        {
            var message = new StringBuilder("[");
            int contCell = 0;
            var itemData = new ItemBuilder(this.Item.ItemName, definition, this.instance.Name);
            foreach (var field in itemFields)
            {
                var cell = data.GetCell(contCell);
                string cellValue = "null";
                string testValue = string.Empty;
                if (cell != null)
                {
                    cellValue = GetCellValueForJson(this.typeIndex[contCell], cell, field.Length);
                    testValue = cellValue;
                    
                    // GES-129 Eliminar comillas del inicio y final
                    if (testValue.StartsWith("\"", StringComparison.OrdinalIgnoreCase))
                    {
                        testValue = testValue.Substring(1);
                    }

                    if (testValue.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                    {
                        testValue = testValue.Substring(0, testValue.Length - 1);
                    }

                    // GES-238 los mails y urls deben cumplir el formato
                    if (field.DataType == FieldDataType.Email)
                    {
                        if (field.Required || !string.IsNullOrEmpty(testValue))
                        {
                            if (!Basics.EmailIsValid(testValue))
                            {
                                this.errors.Add(new Error()
                                {
                                    Linea = index,
                                    ErrorType = "Data",
                                    Message = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "El email del campo {0} no es valido ({1}).",
                                        field.Label,
                                        testValue)
                                });
                            }
                        }
                    }

                    if (field.DataType == FieldDataType.Url)
                    {
                        if (field.Required || !string.IsNullOrEmpty(testValue))
                        {
                            Uri uriResult;
                            bool result = Uri.TryCreate(testValue, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                            if (!result)
                            {
                                testValue = "http://" + testValue;
                                result = Uri.TryCreate(testValue, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                                if (!result)
                                {
                                    this.errors.Add(new Error()
                                    {
                                        Linea = index,
                                        ErrorType = "Data",
                                        Message = string.Format(
                                            CultureInfo.InvariantCulture,
                                            "La dirección URL del campo {0} no es valida ({1}).",
                                            field.Label,
                                            testValue)
                                    });
                                }
                            }
                        }
                    }

                    if (this.typeIndex[contCell] == FieldDataType.Text || this.typeIndex[contCell] == FieldDataType.Textarea)
                    {
                        if (field.Length.HasValue)
                        {
                            if (testValue.Length > field.Length)
                            {
                                this.errors.Add(new Error()
                                {
                                    Linea = index,
                                    ErrorType = "Data",
                                    Message = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "El campo {0} tiene una longitud superior a {1}.",
                                        field.Label,
                                        field.Length.Value)
                                });
                            }
                        }
                    }
                    //// END GES-129

                    // Sólo se comprueba el FK si el campo está informado
                    if (!string.IsNullOrEmpty(testValue))
                    {
                        // GES-130 Se comprueba que si es un foreignlist haya valor en la tabla referenciada
                        /*if (definition.ForeignValues.Any(fv => fv.LocalName == field.Name))
                        {
                            ForeignList fl = definition.ForeignValues.Where(fv =>  fv.LocalName.Equals(field.Name, StringComparison.OrdinalIgnoreCase)).First();
                            if (DataPersistence.GetAllByField(Item.InstanceName, fl.ItemName, fl.ImportReference, testValue).Count == 0)
                            {
                                errors.Add(new Error()
                                {
                                    ErrorType = "Data",
                                    Linea = index,
                                    Message = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "El campo <strong>{0}</strong> no encuentra referencia sobre el valor &quot;<strong>{1}</strong>&quot;",
                                        field.Label,
                                        testValue)
                                });
                            }
                        }*/
                    }
                    
                    if (cellValue.Equals("\"FixedList\"", StringComparison.OrdinalIgnoreCase))
                    {
                        string dataCell = cell.StringCellValue;
                        var fixedItem = new FixedListItem();// DataPersistence.FixedListItemGetById(itemData.InstanceName, field.FixedListId, dataCell);

                        if (!string.IsNullOrEmpty(cell.StringCellValue) && fixedItem == null)
                        {
                            errors.Add(new Error()
                                {
                                    ErrorType = "Data",
                                    Linea = index,
                                    Message = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "El campo <strong>{0}</strong> tiene el valor &quot;<strong>{1}</strong>&quot; que no está en la lista de valores aceptados",
                                        field.Label,
                                        dataCell)
                                });
                        }
                        else
                        {
                            itemData[field.Name] = fixedItem.Id;
                            cellValue = string.Format(CultureInfo.InvariantCulture, @"""{0}""", fixedItem.Description);
                            testValue = cellValue.Replace("\"", string.Empty);
                        }
                    }
                    else
                    {
                        if (this.typeIndex[contCell] == FieldDataType.ImageGallery)
                        {
                            itemData.Add(field.Name, string.Empty);
                            testValue = string.Empty;
                        }
                        else if (field.DataType == FieldDataType.Boolean || field.DataType == FieldDataType.NullableBoolean)
                        {
                            if (!string.IsNullOrEmpty(testValue))
                            {
                                itemData.Add(field.Name, Convert.ToBoolean(testValue));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(testValue))
                            {
                                itemData.Add(field.Name, testValue.Replace(@"\""", @""""));
                            }
                            else
                            {
                                itemData.Add(field.Name, cellValue);
                            }
                        }
                    }
                }
                
                message.AppendFormat(CultureInfo.InvariantCulture, @"{0}""{1}""", contCell > 0 ? "," : string.Empty, testValue);
                contCell++;
            }

            // Juan Castilla - Comprobar que la PK no esté ya en la carga
            string primaryKeyData = itemData.PrimaryKeyData;
            if (primaryKeys.Contains(primaryKeyData))
            {
                this.errors.Add(new Error()
                {
                    Linea = index,
                    ErrorType = "Data",
                    Message = "La clave ya aparece en otro registro de esta carga"
                });
            }
            else
            {
                primaryKeys.Add(primaryKeyData);
            }

            // Cofirmar que los campos obligatorios están rellenados
            foreach (var field in Item.Definition.Fields.Where(f => f.Required))
            {
                if (field.Name != "Id" && field.Name != "CompanyId")
                {
                    if (!itemData.ContainsKey(field.Name))
                    {
                        this.errors.Add(new Error()
                        {
                            Linea = index,
                            ErrorType = "Data",
                            Message = string.Format(CultureInfo.InvariantCulture, "El campo {0} es obligatorio", field.Label)
                        });
                    }
                    else if (itemData[field.Name] == null)
                    {
                        this.errors.Add(new Error()
                        {
                            Linea = index,
                            ErrorType = "Data",
                            Message = "El campo " + field.Label + " es obligatorio"
                        });
                    }
                }
            }

            if (itemData.Definition.ItemRules.Count > 0)
            {
                foreach (var rule in itemData.Definition.ItemRules)
                {
                    var complains = new SpecialRule(itemData, rule).Complains;
                    if (!complains.Success)
                    {
                        this.errors.Add(new Error()
                        {
                            Linea = index,
                            ErrorType = "Data",
                            Message = complains.MessageError
                        });
                    }
                }
            }
            
            message.Append("]");

            if (res.Data.Count() < 21)
            {
                res.Data = message.ToString();
            }

            this.dataFile.Add(res);
            this.itemsReaded.Add(itemData);
        }

        return res;
    }

    private static string GetCellValueForJson(FieldDataType dataType, ICell cell, int? length)
    {
        if (cell == null || cell.CellType == CellType.Blank)
        {
            return string.Empty;
        }

        string cellValue = string.Empty;
        switch (dataType)
        {
            case FieldDataType.Text:
            case FieldDataType.Textarea:
                cellValue = string.Format(@"""{0}""", ToolsJson.JsonCompliant(ToolsXlsx.GetValue<string>(cell)));
                break;
            case FieldDataType.Decimal:
            case FieldDataType.NullableDecimal:
                var decimalValue = ToolsXlsx.GetValue<decimal?>(cell);

                if (decimalValue.HasValue)
                {
                    cellValue = string.Format(CultureInfo.InvariantCulture, "{0:################0.##################}", decimalValue);
                }
                else
                {
                    cellValue = ConstantValue.Null;
                }
                break;
            case FieldDataType.Long:
            case FieldDataType.NullableLong:
            case FieldDataType.Integer:
            case FieldDataType.NullableInteger:
            case FieldDataType.Float:
            case FieldDataType.NullableFloat:
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        cellValue = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                        break;
                    default:
                        cellValue = cell.StringCellValue;
                        break;
                }

                break;
            case FieldDataType.Time:
            case FieldDataType.NullableTime:
                var timeValue = ToolsXlsx.GetValue<DateTime?>(cell);
                if (timeValue.HasValue)
                {
                    cellValue = string.Format(CultureInfo.InvariantCulture, @"""{0:hh:mm}""", timeValue);
                }
                else
                {
                    cellValue = ConstantValue.Null;
                }

                break;
            case FieldDataType.DateTime:
            case FieldDataType.NullableDateTime:
                var dateTimeValue = ToolsXlsx.GetValue<DateTime?>(cell);
                if (dateTimeValue.HasValue)
                {
                    cellValue = string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", dateTimeValue);
                }
                else
                {
                    cellValue = ConstantValue.Null;
                }

                break;
            case FieldDataType.Boolean:
            case FieldDataType.NullableBoolean:
                if (cell.CellType == CellType.String)
                {
                    string cellData = cell.StringCellValue.ToUpperInvariant();

                    if (string.IsNullOrEmpty(cellData))
                    {
                        cellValue = string.Empty;
                        break;
                    }

                    cellValue = cellData == ConstantValue.True.ToUpperInvariant() ? ConstantValue.True : ConstantValue.False;
                    break;
                }

                var booleanValue = ToolsXlsx.GetBooleanValue(cell);
                cellValue = booleanValue.HasValue ? (booleanValue.Value ? ConstantValue.True : ConstantValue.False) : string.Empty;
                break;
            case FieldDataType.Url:
            case FieldDataType.Email:
                cellValue = ToolsXlsx.GetValue<string>(cell);
                break;
            default:
                cellValue = string.Format(CultureInfo.InvariantCulture, @"""{0}""", dataType.ToString());
                break;
        }

        return cellValue;
    }

    private static object GetCellValueForDataBase(ItemField field, ICell cell)
    {
        if (cell == null || cell.CellType == CellType.Blank)
        {
            return null;
        }

        object cellValue = null;
        switch (field.DataType)
        {
            case FieldDataType.Text:
                cellValue = ToolsXlsx.GetValue<string>(cell);
                break;
            case FieldDataType.Decimal:
            case FieldDataType.NullableDecimal:
                cellValue = ToolsXlsx.GetValue<decimal?>(cell);
                break;
            case FieldDataType.Long:
            case FieldDataType.NullableLong:
            case FieldDataType.Integer:
            case FieldDataType.NullableInteger:
            case FieldDataType.Float:
            case FieldDataType.NullableFloat:
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        cellValue = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                        break;
                    default:
                        cellValue = cell.StringCellValue;
                        break;
                }

                break;
            case FieldDataType.DateTime:
            case FieldDataType.NullableDateTime:
                var dateTimeValue = ToolsXlsx.GetValue<DateTime?>(cell);
                if (dateTimeValue.HasValue)
                {
                    cellValue = string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}", dateTimeValue);
                }

                break;
            case FieldDataType.Boolean:
            case FieldDataType.NullableBoolean:
                if (cell.CellType == CellType.String)
                {
                    string cellData = cell.StringCellValue.ToUpperInvariant();

                    if (string.IsNullOrEmpty(cellData))
                    {
                        cellValue = null;
                        break;
                    }

                    cellValue = cellData == ConstantValue.True.ToUpperInvariant() ? ConstantValue.True : ConstantValue.False;
                    break;
                }

                cellValue = ToolsXlsx.GetBooleanValue(cell);
                break;

            default:
                cellValue = null;
                break;
        }

        return cellValue;
    }

    private struct Error
    {
        public string ErrorType { get; set; }

        public int Linea { get; set; }

        public string Message { get; set; }
    }

    private struct DataLine
    {
        public int Line { get; set; }

        public string Data { get; set; }
    }
}