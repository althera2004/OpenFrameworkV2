﻿// --------------------------------
// <copyright file="CustomerFramework.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Customer
{
    using System;
    using System.Configuration;
    using System.Globalization;

    /// <summary>Implements customer framework class</summary>
    public sealed class CustomerFramework
    {
        /// <summary>Gets an empty instance of customer framework</summary>
        public static CustomerFramework Empty
        {
            get
            {
                return new CustomerFramework
                {
                    Id = 0,
                    Name = string.Empty,
                    Config = Config.Empty
                };
            }
        }

        /// <summary>Gets or sets the customer framework identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the customer framework name</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the customer framework configuration</summary>
        public Config Config { get; set; }

        /// <summary>Loads customer framework configuration</summary>
        public void LoadConfig()
        {
            this.Config = Config.Load(this.Name);
            this.Id = this.Config.Id;
            this.Name = this.Config.Name;
        }

        public static CustomerFramework Load(string instanceName)
        {
            var result = new CustomerFramework
            {
                Name = instanceName
            };

            result.LoadConfig();
            return result;
        }

        public string DefinitionPath
        {
            get
            {
                string path = string.Format(
                    CultureInfo.InvariantCulture,
                    ConfigurationManager.AppSettings["ItemsDefinitionPath"],
                    this.Name);

                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                }
                else
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}", path);
                }

                return path.ToUpperInvariant();
            }
        }
    }
}