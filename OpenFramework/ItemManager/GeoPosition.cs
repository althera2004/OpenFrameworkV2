// --------------------------------
// <copyright file="GeoPosition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;

    /// <summary>Defines a data structure with latitude an longitude</summary>
    public sealed class GeoPosition
    {
        /// <summary>Field latitude</summary>
        private decimal latitude;

        /// <summary>Field longitude</summary>
        private decimal longitude;

        /// <summary>Initializes a new instance of the GeoPosition class.</summary>
        public GeoPosition()
        {
            this.Intialize(0, 0);
        }

        /// <summary>Initializes a new instance of the GeoPosition class.</summary>
        /// <param name="latitude">Latitude value</param>
        /// <param name="longitude">Longitude value</param>
        public GeoPosition(decimal latitude, decimal longitude)
        {
            this.Intialize(latitude, longitude);
        }

        /// <summary>Gets or sets latitude</summary>
        public decimal Latitude
        {
            get
            {
                return decimal.Round(this.latitude, 6, MidpointRounding.AwayFromZero);
            }

            set
            {
                this.latitude = decimal.Round(value, 6, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>Gets or sets longitude</summary>
        public decimal Longitude
        {
            get
            {
                return decimal.Round(this.longitude, 6, MidpointRounding.AwayFromZero);
            }

            set
            {
                this.longitude = decimal.Round(value, 6, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>Initialize a new instance of the GeoPosition class</summary>
        /// <param name="latitudeValue">Latitude value</param>
        /// <param name="longitudeValue">Longitude value</param>
        private void Intialize(decimal latitudeValue, decimal longitudeValue)
        {
            this.latitude = decimal.Round(latitudeValue, 6, MidpointRounding.AwayFromZero);
            this.longitude = decimal.Round(longitudeValue, 6, MidpointRounding.AwayFromZero);
        }
    }
}
