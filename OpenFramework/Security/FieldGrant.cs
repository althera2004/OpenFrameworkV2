// --------------------------------
// <copyright file="FieldGrant.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Security
{
    /// <summary>
    /// Implements FieldGrant class
    /// </summary>
    public struct FieldGrant
    {
        /// <summary>
        /// Gets an empty field grant
        /// </summary>
        public static FieldGrant Empty
        {
            get
            {
                return new FieldGrant()
                {
                    FieldName = string.Empty,
                    Read = false,
                    Write = false
                };
            }
        }

        /// <summary>
        /// Gets or sets the name of field
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether field content is readable
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether field content is writable
        /// </summary>
        public bool Write { get; set; }
    }
}
