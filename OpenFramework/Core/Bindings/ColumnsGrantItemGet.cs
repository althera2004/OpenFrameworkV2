// --------------------------------
// <copyright file="ColumnsGrantItemGet.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.Bindings
{
    /// <summary>
    /// Columns index of result to get item grants from database
    /// </summary>
    public static class ColumnsGrantItemGet
    {
        /// <summary>Index for column GroupId</summary>
        public const int GroupId = 0;

        /// <summary>Index for column ItemName</summary>
        public const int ItemName = 1;

        /// <summary>Index for column CanRead</summary>
        public const int CanRead = 2;

        /// <summary>Index for column CanWrite</summary>
        public const int CanWrite = 3;

        /// <summary>Index for column CanDelete</summary>
        public const int CanDelete = 4;
    }
}
