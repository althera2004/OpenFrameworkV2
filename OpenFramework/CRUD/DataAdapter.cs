// --------------------------------
// <copyright file="DataAdapter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.CRUD
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Implements DataAdapter class</summary>
    [Serializable]
    public sealed class DataAdapter
    {
        /// <summary>Stored procedure to inactivate</summary>
        [JsonProperty("Inactive")]
        private StoredProcedure inactive;

        /// <summary>Stored procedure to activate</summary>
        [JsonProperty("Active")]
        private StoredProcedure active;

        /// <summary>Stored procedure to insert an item</summary>
        [JsonProperty("Insert")]
        private StoredProcedure insert;

        /// <summary>Stored procedure to update an item data</summary>
        [JsonProperty("Update")]
        private StoredProcedure update;

        /// <summary>Stored procedure to get all items</summary>
        [JsonProperty("GetAll")]
        private StoredProcedure getAll;

        /// <summary>Stored procedure to get by item identifier</summary>
        [JsonProperty("GetById")]
        private StoredProcedure getById;

        /// <summary>Stored procedure to get item's key/value used in ReadAllKeyValue</summary>
        [JsonProperty("GetKeyValue")]
        private StoredProcedure getKeyValue;

        /// <summary>Gets an empty data adapter</summary>
        [JsonIgnore]
        public static DataAdapter Empty
        {
            get
            {
                return new DataAdapter()
                {
                    inactive = new StoredProcedure() { StoredName = string.Empty },
                    active = new StoredProcedure() { StoredName = string.Empty },
                    insert = new StoredProcedure() { StoredName = string.Empty },
                    update = new StoredProcedure() { StoredName = string.Empty },
                    getAll = new StoredProcedure() { StoredName = string.Empty },
                    getById = new StoredProcedure() { StoredName = string.Empty },
                    getKeyValue = new StoredProcedure() { StoredName = string.Empty }
                };
            }
        }

        /// <summary>Gets stored procedure to inactivate</summary>
        [JsonIgnore]
        public StoredProcedure Inactive
        {
            get
            {
                if (this.inactive == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.inactive;
            }
        }

        /// <summary>Gets stored procedure to activate</summary>
        [JsonIgnore]
        public StoredProcedure Active
        {
            get
            {
                if (this.active == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.active;
            }
        }

        /// <summary>Gets stored procedure to insert an item</summary>
        [JsonIgnore]
        public StoredProcedure Insert
        {
            get
            {
                if (this.insert == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.insert;
            }
        }

        /// <summary>Gets stored procedure to update an item data</summary>
        [JsonIgnore]
        public StoredProcedure Update
        {
            get
            {
                if (this.update == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.update;
            }
        }

        /// <summary>Gets stored procedure to get all items</summary>
        [JsonIgnore]
        public StoredProcedure GetAll
        {
            get
            {
                if (this.getAll == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.getAll;
            }
        }

        /// <summary>Gets stored procedure to get an item by identifier</summary>
        [JsonIgnore]
        public StoredProcedure GetById
        {
            get
            {
                if (this.getById == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.getById;
            }
        }

        /// <summary>Gets stored procedure to get item's key/value used in ReadAllKeyValue</summary>
        [JsonIgnore]
        public StoredProcedure GetKeyValue
        {
            get
            {
                if (this.getKeyValue == null)
                {
                    return StoredProcedure.Empty;
                }

                return this.getKeyValue;
            }
        }
    }
}