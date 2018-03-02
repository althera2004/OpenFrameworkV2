// --------------------------------
// <copyright file="EditMode.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    /// <summary>Enumeration of available button actions</summary>
    public enum EditMode
    {
        /// <summary>Not defined</summary>
        Undefined = 0,

        /// <summary>Show form edition in popup</summary>
        Popup = 1,

        /// <summary>Edition of row inline</summary>
        Inline = 2,

        /// <summary>Show content data in custom form on another page</summary>
        Custom = 3,

        /// <summary>Show content data as a part of another ItemBuilder that contains it</summary>
        Inform = 4,

        /// <summary>Button for capture existent data</summary>
        Capture = 5,

        /// <summary>Read only table</summary>
        ReadOnly = 6
    }
}
