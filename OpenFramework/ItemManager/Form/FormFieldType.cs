// --------------------------------
// <copyright file="FormFieldType.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.Form
{
    /// <summary>Enumeration of type of fields in form</summary>
    public enum FormFieldType
    {
        /// <summary>Input control</summary>
        Input = 0,

        /// <summary>Button to do action</summary>
        Button = 1,

        /// <summary>Empty space to fill row</summary>
        Break = 2,

        /// <summary>List (filtering purpose)</summary>
        List = 3,

        /// <summary>Place holder for custom controls and messages</summary>
        PlaceHolder = 4
    }
}
