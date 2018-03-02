// --------------------------------
// <copyright file="FieldDataType.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    /// <summary>Enumeration of item field data types</summary>
    public enum FieldDataType
    {
        /// <summary>Undefined type</summary>
        Undefined = 0,

        /// <summary>Text or string</summary>
        Text = 1,

        /// <summary>Integer number</summary>
        Integer = 2,

        /// <summary>Decimal number</summary>
        Decimal = 3,

        /// <summary>Long number</summary>
        Long = 4,

        /// <summary>Float number</summary>
        Float = 5,

        /// <summary>Email address</summary>
        Email = 6,

        /// <summary>Date and time</summary>
        DateTime = 7,

        /// <summary>Only time</summary>
        Time = 8,

        /// <summary>Nullable integer</summary>
        NullableInteger = 9,

        /// <summary>Nullable decimal</summary>
        NullableDecimal = 10,

        /// <summary>Nullable long</summary>
        NullableLong = 11,

        /// <summary>Nullable float</summary>
        NullableFloat = 12,

        /// <summary>Nullable date and time</summary>
        NullableDateTime = 13,

        /// <summary>Nullable time</summary>
        NullableTime = 14,

        /// <summary>Boolean value</summary>
        Boolean = 15,

        /// <summary>Nullable boolean</summary>
        NullableBoolean = 16,

        /// <summary>Latitude and longitude</summary>
        Geoposition = 17,

        /// <summary>Nullable latitude and longitude</summary>
        NullableGeoposition = 18,

        /// <summary>Guid value</summary>
        Guid = 19,

        /// <summary>Nullable guid</summary>
        NullableGuid = 20,

        /// <summary>Text area</summary>
        Textarea = 21,

        /// <summary>Fixed List</summary>
        FixedList = 22,

        /// <summary>Fixed List linked</summary>
        FixedListLinked = 23,

        /// <summary>Url site</summary>
        Url = 24,

        /// <summary>Image file</summary>
        Image = 25,

        /// <summary>Document file</summary>
        DocumentFile = 26,

        /// <summary>Three state variable</summary>
        TriState = 27,

        /// <summary>Folder path</summary>
        Folder = 28,

        /// <summary>Fixed image on CDN server</summary>
        CDNImage = 29,

        /// <summary>Gallery of images</summary>
        ImageGallery = 30,

        /// <summary>Gallery of documents</summary>
        DocumentGallery = 31
    }
}
