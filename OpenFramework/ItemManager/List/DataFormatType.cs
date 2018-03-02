// --------------------------------
// <copyright file="DataFormatType.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    /// <summary>Enumeration of data format types</summary>
    public enum DataFormatType
    {
        /// <summary>Undefined, showed as is</summary>
        Undefined = 0,

        /// <summary>Content showed as text</summary>
        Text = 1,

        /// <summary>Decimal type showed as money value</summary>
        Money = 2,

        /// <summary>Decimal type showed as percentage value</summary>
        Percentage = 3,

        /// <summary>Boolean value showed as text</summary>
        BooleanText = 4,

        /// <summary>Boolean value showed as icon</summary>
        BooleanIcon = 5,

        /// <summary>Showed as date day/month/year</summary>
        Date = 6,

        /// <summary>Showed as date with time</summary>
        DateTime = 7,

        /// <summary>Showed as date only with month and year information</summary>
        DateMonth = 8,

        /// <summary>Showed as date with the name of month nested month number</summary>
        DateText = 9,
        
        /// <summary>Showed as decimal value, with decimal separator</summary>        
        Decimal = 10,

        /// <summary>Showed as email address with mail to</summary>
        Email = 11,

        /// <summary>Showed as hyperlink</summary>
        Url = 12,

        /// <summary>Shows an image</summary>
        Image = 13,

        /// <summary>Shows a document path</summary>
        Document = 14,

        /// <summary>Shows a folder path</summary>
        Folder = 15,

        /// <summary>Shows simulating a check box</summary>
        BooleanCheck = 16,

        /// <summary>Shows time value</summary>
        Time = 17,

        /// <summary>Shows GPS value</summary>
        GPS = 18,

        /// <summary>Shows simulating a check box that only shows true values</summary>
        BooleanCheckNull = 18
    }
}