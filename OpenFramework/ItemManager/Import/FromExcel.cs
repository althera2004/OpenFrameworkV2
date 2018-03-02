// --------------------------------
// <copyright file="FromExcel.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.Import
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using NPOI.XSSF.UserModel;
    using OpenFramework.Core;
    using OpenFramework.ItemManager;

    /// <summary>
    /// Implements importation of data from excel files
    /// </summary>
    public sealed class FromExcel
    {
        /// <summary> Work book of excel destination of exported data</summary>
        private XSSFWorkbook workBook;

        /// <summary> Sheet of work book destination of exported data</summary>
        private XSSFSheet sheet;

        /// <summary>Gets or sets the item builder to import data</summary>
        public ItemBuilder ItemBuilder { get; set; }

        /// <summary>
        /// Generates a empty excel file with header as template for import process
        /// </summary>
        /// <returns>Link to generated template</returns>
        public ActionResult Template()
        {
            var res = ActionResult.NoAction;
            this.workBook = new XSSFWorkbook();
            this.sheet = (XSSFSheet)this.workBook.CreateSheet(this.ItemBuilder.Definition.Layout.LabelPlural);

            // HEADER
            int contCell = 0;
            this.CreateSecureRow(0);

            foreach (ItemField field in this.ItemBuilder.Definition.Fields.Where(f => !f.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
            {
                this.sheet.GetRow(0).CreateCell(contCell);
                this.sheet.GetRow(0).GetCell(contCell).SetCellValue(field.LabelExcel);
                contCell++;
            }

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}\\", path);
            }

            path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\Temp\{1}.{2}", path, this.ItemBuilder.Definition.Layout.LabelPlural, ConstantValue.Excel2007Extension);
            string link = string.Format(CultureInfo.GetCultureInfo("en-us"), @"/Temp/{0}.{1}", this.ItemBuilder.Definition.Layout.LabelPlural, ConstantValue.Excel2007Extension);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                this.workBook.Write(fs);
            }

            res.SetSuccess(link);
            return res;
        }

        /// <summary>
        /// Creates a row into sheet
        /// </summary>
        /// <param name="index">Row index</param>
        private void CreateSecureRow(int index)
        {
            if (this.sheet.GetRow(index) == null)
            {
                this.sheet.CreateRow(index);
            }
        }
    }
}
