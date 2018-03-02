// --------------------------------
// <copyright file="ToolsXlsx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework
{
    using System;
    using System.Globalization;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>Tools to operate in an Excel document</summary>
    public static class ToolsXlsx
    {
        /// <summary>Creates a new row</summary>
        /// <param name="sheet">Sheet to create row</param>
        /// <param name="rowIndex">Index of row</param>
        public static void CreateRow(XSSFSheet sheet, int rowIndex)
        {
            if (sheet == null)
            {
                return;
            }

            if (sheet.GetRow(rowIndex) == null)
            {
                sheet.CreateRow(rowIndex);
            }
        }

        /// <summary>Creates a new row</summary>
        /// <param name="sheet">Sheet to create row</param>
        /// <param name="rowIndex">Index of row</param>
        public static void CreateRow(HSSFSheet sheet, int rowIndex)
        {
            if (sheet == null)
            {
                return;
            }

            if (sheet.GetRow(rowIndex) == null)
            {
                sheet.CreateRow(rowIndex);
            }
        }

        /// <summary>Gets the string value of cell</summary>
        /// <param name="row">Row of cell</param>
        /// <param name="cellIndex">Index of cell in row</param>
        /// <returns>String value of cell</returns>
        public static string GetString(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return string.Empty;
            }

            if (row.GetCell(cellIndex) == null)
            {
                return string.Empty;
            }

            return GetString(row.GetCell(cellIndex));
        }

        /// <summary>Gets value from a cell</summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="cell">Excel cell</param>
        /// <returns>Value of cell</returns>
        public static T GetValue<T>(ICell cell)
        {
            if (cell == null)
            {
                return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
            }

            object cellValue = null;

            switch (typeof(T).FullName.ToUpperInvariant())
            {
                case "SYSTEM.STRING":
                    cellValue = GetString(cell);
                    break;
                case "SYSTEM.DATETIME":
                case "SYSTEM.DATETIME?":
                    cellValue = GetDateTimeNullable(cell);
                    break;
                case "SYSTEM.BOOL":
                case "SYSTEM.BOOL?":
                    cellValue = GetBooleanValue(cell);
                    break;
                case "SYSTEM.DECIMAL":
                case "SYSTEM.DECIMAL?":
                    cellValue = GetDecimalValue(cell);
                    break;
            }

            if (cellValue == null)
            {
                if (typeof(T).FullName.ToUpperInvariant().IndexOf("SYSTEM.DATETIME", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    cellValue = GetDateTimeNullable(cell);
                }

                if (typeof(T).FullName.ToUpperInvariant().IndexOf("SYSTEM.BOOL", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    cellValue = GetBooleanValue(cell);
                }

                if (typeof(T).FullName.ToUpperInvariant().IndexOf("SYSTEM.DECIMAL", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    cellValue = GetDecimalValue(cell);
                }
            }

            Type t = typeof(T);
            t = Nullable.GetUnderlyingType(t) ?? t;
            return (cellValue == null || DBNull.Value.Equals(cellValue)) ? default(T) : (T)Convert.ChangeType(cellValue, t, CultureInfo.InvariantCulture);

            //// return (T)Convert.ChangeType(cellValue, typeof(T));
        }

        /// <summary>Gets string value of a cell</summary>
        /// <param name="cell">Excel cell</param>
        /// <returns>String value of cell</returns>
        public static string GetString(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            return cell.ToString();
        }

        public static DateTime? GetDateTimeNullable(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }

            return GetDateTimeNullable(row.GetCell(cellIndex));
        }

        public static DateTime? GetDateTimeNullable(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.String:
                    return Basics.DateFromStringddMMyyy(cell.StringCellValue);
            }

            return cell.DateCellValue;
        }

        public static decimal? GetDecimalValue(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }

            return GetDecimalValue(row.GetCell(cellIndex));
        }

        public static decimal? GetDecimalValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.String:
                    decimal res = 0;
                    if (decimal.TryParse(cell.StringCellValue, out res))
                    {
                        return res;
                    }

                    return null;
            }

            double? doubleRes = cell.NumericCellValue as double?;

            if (doubleRes.HasValue)
            {
                return Convert.ToDecimal(doubleRes, CultureInfo.InvariantCulture);
            }

            return null;
        }

        public static float? GetFloatValue(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }

            return GetFloatValue(row.GetCell(cellIndex));
        }

        public static float? GetFloatValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.String:
                    float res = 0;
                    if (float.TryParse(cell.StringCellValue, out res))
                    {
                        return res;
                    }

                    return null;
            }

            double? doubleRes = cell.NumericCellValue as double?;

            if (doubleRes.HasValue)
            {
                return Convert.ToSingle(doubleRes, CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>Gets a boolean value from a cell</summary>
        /// <param name="cell">Cell to extract value</param>
        /// <returns>Boolean value or null</returns>
        public static bool? GetBooleanValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            bool? res = null;
            switch (cell.CellType)
            {
                //// case CellType.Blank:
                ////    return null;
                case CellType.Boolean:
                    res = cell.BooleanCellValue;
                    break;
                case CellType.String:
                    string data = cell.StringCellValue.ToUpperInvariant();

                    if (data == "TRUE")
                    {
                        res = true;
                    }

                    if (data == "FALSE")
                    {
                        res = false;
                    }

                    res = null;
                    break;
            }

            return res;
        }
    }
}