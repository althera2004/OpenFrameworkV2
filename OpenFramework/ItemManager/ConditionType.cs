// --------------------------------
// <copyright file="ConditionType.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    /// <summary>List of types of condition</summary>
    public enum ConditionType
    {
        /// <summary>Condition refers to a field value</summary>
        Field = 0,

        /// <summary>Condition refers to a global value</summary>
        Global = 1
    }
}