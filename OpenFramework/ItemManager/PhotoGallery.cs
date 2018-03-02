// --------------------------------
// <copyright file="PhotoGallery.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFramework.Core;
    using OpenFramework.DataAccess;
    using OpenFramework.Security;

    /// <summary>Implements PhotoGallery class</summary>
    public class PhotoGallery
    {
        public long Id { get; set; }
        public string ItemName { get; set; }
        public string ItemField { get; set; }
        public long ItemId { get; set; }
        public DateTime? Date { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string GPS { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        /// <summary>Gets an empty instance of PhotoGallery object</summary>
        public static PhotoGallery Empty
        {
            get
            {
                return new PhotoGallery()
                {
                    Id = 0,
                    ItemName = string.Empty,
                    ItemField = string.Empty,
                    ItemId = 0,
                    Order = 0,
                    Description = string.Empty,
                    GPS = string.Empty,
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        /// <summary>Gets a Json structure of photo gallery</summary>
        public string Json
        {
            get
            {
                string fecha = "null";

                if (this.Date.HasValue)
                {
                    fecha = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Date.Value);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""ItemName"":""{1}"",
                        ""ItemField"":""{2}"",
                        ""ItemId"":{3},
                        ""Order"": {12},
                        ""Description"":""{4}"",
                        ""Date"":{5},
                        ""GPS"":""{6}"",
                        ""CreatedBy"":{7},
                        ""CreatedOn"":""{8:dd/MM/yyyy}"",
                        ""ModifiedBy"":{9},
                        ""ModifiedOn"":""{10:dd/MM/yyyy}"",
                        ""Active"":{11}
                    }}",
                       this.Id,
                       this.ItemName,
                       this.ItemField,
                       this.ItemId,
                       ToolsJson.JsonCompliant(this.Description),
                       fecha,
                       this.GPS,
                       this.CreatedBy,
                       this.CreatedOn,
                       this.ModifiedBy,
                       this.ModifiedOn,
                       this.Active ? ConstantValue.True : ConstantValue.False,
                       this.Order);
            }
        }

        public static string JsonList(ReadOnlyCollection<PhotoGallery> photos)
        {
            if (photos == null || photos.Count == 0)
            {
                return "[]";
            }

            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (PhotoGallery photo in photos)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(photo.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<PhotoGallery> GetByItemId(string itemName, long itemId, string connectionString)
        {
            List<PhotoGallery> res = new List<PhotoGallery>();
            
            /* CREATE PROCEDURE Core_PhotoGallery_GetByItemId
             *   @ItemName nvarchar(50),
             *   @ItemId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_PhotoGallery_GetByItemId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ItemName", itemName));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));

                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PhotoGallery newPhoto = PhotoGallery.Empty;
                                newPhoto.Id = rdr.GetInt64(0);
                                newPhoto.ItemName = rdr.GetString(1);
                                newPhoto.ItemField = rdr.GetString(2);
                                newPhoto.ItemId = rdr.GetInt64(3);
                                newPhoto.Description = rdr.GetString(5);
                                newPhoto.GPS = rdr.GetString(6);
                                newPhoto.CreatedBy = rdr.GetInt64(7);
                                newPhoto.CreatedOn = rdr.GetDateTime(8);
                                newPhoto.ModifiedBy = rdr.GetInt64(9);
                                newPhoto.ModifiedOn = rdr.GetDateTime(10);
                                newPhoto.Active = rdr.GetBoolean(11);
                                newPhoto.Order = rdr.GetInt32(12);

                                if (!rdr.IsDBNull(4))
                                {
                                    newPhoto.Date = rdr.GetDateTime(4);
                                }

                                res.Add(newPhoto);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "PhotoGallery GetByItemId");
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<PhotoGallery>(res);
        }

        public static ReadOnlyCollection<PhotoGallery> GetByItemFieldId(string itemName, string itemFieldName, long itemId, string connectionString)
        {
            List<PhotoGallery> res = new List<PhotoGallery>();

            /* CREATE PROCEDURE Core_PhotoGallery_GetByItemId
             *   @ItemName nvarchar(50),
             *   @ItemId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_PhotoGallery_GetByItemFieldId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ItemName", itemName));
                cmd.Parameters.Add(DataParameter.Input("@ItemField", itemFieldName));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));

                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            PhotoGallery newPhoto = PhotoGallery.Empty;
                            newPhoto.Id = rdr.GetInt64(0);
                            newPhoto.ItemName = rdr.GetString(1);
                            newPhoto.ItemField = rdr.GetString(2);
                            newPhoto.ItemId = rdr.GetInt64(4);
                            newPhoto.Description = rdr.GetString(5);
                            newPhoto.GPS = rdr.GetString(7);
                            newPhoto.CreatedBy = rdr.GetInt64(8);
                            newPhoto.CreatedOn = rdr.GetDateTime(9);
                            newPhoto.ModifiedBy = rdr.GetInt64(10);
                            newPhoto.ModifiedOn = rdr.GetDateTime(11);
                            newPhoto.Active = rdr.GetBoolean(12);
                            newPhoto.Order = rdr.GetInt32(13);

                            if (!rdr.IsDBNull(6))
                            {
                                newPhoto.Date = rdr.GetDateTime(6);
                            }

                            res.Add(newPhoto);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "PhotoGallery GetByItemFieldId");
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<PhotoGallery>(res);
        }
    }
}
