// --------------------------------
// <copyright file="DataOrigin.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.List
{
    /// <summary>Enumeration of data origin</summary>
    public enum DataOrigin
    {
        /// <summary>Undefined, showed as is</summary>
        Undefined = 0,

        /// <summary>Native of open framework data repository</summary>
        Native = 1,

        /// <summary>Synchronized from Navision</summary>
        Navision = 2,

        /// <summary>Synchronized from CRM</summary>
        CRM = 3,

        /// <summary>Synchronized from SAP</summary>
        SAP = 4
    }
}