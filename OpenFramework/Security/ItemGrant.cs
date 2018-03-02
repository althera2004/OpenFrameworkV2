// --------------------------------
// <copyright file="ItemGrant.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFramework.Core.Bindings;
    using OpenFramework.DataAccess;

    public sealed class ItemGrant
    {
        private List<FieldGrant> fieldsGrant;

        public static ItemGrant Empty
        {
            get
            {
                return new ItemGrant()
                {
                    ItemName = string.Empty,
                    Read = false,
                    Write = false,
                    Delete = false
                };
            }
        }

        public string ItemName { get; set; }

        public string ItemLabel { get; set; }

        public bool Read { get; set; }

        public bool Write { get; set; }

        public bool Delete { get; set; }

        public ReadOnlyCollection<FieldGrant> FieldsGrant
        {
            get
            {
                this.PreventNullFieldsGrant();
                return new ReadOnlyCollection<FieldGrant>(this.fieldsGrant);
            }
        }

        /// <summary>Gets a JSON structure of ItemGrant</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""ItemName"":""{0}"", ""ItemLabel"":""{1}"" ,""Read"":{2}, ""Write"":{3}, ""Delete"":{4}}}",
                    this.ItemName,
                    ToolsJson.JsonCompliant(this.ItemLabel),
                    ConstantValue.Value(this.Read),
                    ConstantValue.Value(this.Write),
                    ConstantValue.Value(this.Delete));
            }
        }

        /// <summary>
        /// Gets a JSON array of item grants
        /// </summary>
        /// <param name="grants">Collection of item grants</param>
        /// <returns>String with JSON array of item grants</returns>
        public static string JsonList(ReadOnlyCollection<ItemGrant> grants)
        {
            if (grants == null)
            {
                return ConstantValue.EmptyJsonList;
            }

            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (ItemGrant grant in grants)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(grant.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Gets grants for a security group
        /// </summary>
        /// <param name="group">Security group</param>
        /// <returns>Readonly collection of group grants</returns>
        public static ReadOnlyCollection<ItemGrant> ByGroup(Group group, string connectionString)
        {
            if (group == null)
            {
                return new ReadOnlyCollection<ItemGrant>(new List<ItemGrant>());
            }

            return ByGroup(group.Id, connectionString);
        }

        /*/// <summary>
        /// Gets grants for a security group
        /// </summary>
        /// <param name="groupId">Security group identifier</param>
        /// <returns>Readonly collection of group grants</returns>
        public static ReadOnlyCollection<ItemGrant> ByGroup(long groupId)
        {
            CustomerFramework customerConfig = HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
            return ByGroup(groupId, customerConfig.Config.ConnectionString);
        }*/

        /// <summary>
        /// Gets grants for a security group
        /// </summary>
        /// <param name="groupId">Security group identifier</param>
        /// <param name="connectionString">String connection to data base</param>
        /// <returns>Readonly collection of group grants</returns>
        public static ReadOnlyCollection<ItemGrant> ByGroup(long groupId, string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "ItemGrant::ByGroup({0})", groupId);
            List<ItemGrant> res = new List<ItemGrant>();
            using (SqlCommand cmd = new SqlCommand("Grant_Item_ByGroup"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@GroupId", groupId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    try
                    {
                        cmd.Connection = cnn;
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ItemGrant newItemGrant = new ItemGrant()
                                {
                                    ItemName = rdr.GetString(ColumnsGrantItemGet.ItemName).ToUpperInvariant(),
                                    Read = rdr.GetBoolean(ColumnsGrantItemGet.CanRead),
                                    Write = rdr.GetBoolean(ColumnsGrantItemGet.CanWrite),
                                    Delete = rdr.GetBoolean(ColumnsGrantItemGet.CanDelete)
                                };

                                newItemGrant.PreventNullFieldsGrant();
                                res.Add(newItemGrant);
                            }
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (NotSupportedException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                }
            }

            return new ReadOnlyCollection<ItemGrant>(res);
        }

        public static ActionResult Save(ItemGrant grant, long groupId, string connectionString)
        {
            if (grant == null)
            {
                return ActionResult.NoAction;
            }

            string source = "ItemGrant::Save";
            /* CREATE PROCEDURE Core_ItemGrant_Save
             *   @GroupId bigint,
             *   @ItemName nvarchar(50),
             *   @CanRead bit,
             *   @CanWrite bit,
             *   @CanDelete bit */
            var res = ActionResult.NoAction;

            using (SqlCommand cmd = new SqlCommand("Core_ItemGrant_Save"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@GroupId", groupId));
                cmd.Parameters.Add(DataParameter.Input("@ItemName", grant.ItemName, 50));
                cmd.Parameters.Add(DataParameter.Input("@CanRead", grant.Read));
                cmd.Parameters.Add(DataParameter.Input("@CanWrite", grant.Write));
                cmd.Parameters.Add(DataParameter.Input("@CanDelete", grant.Delete));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        ////SecurityPersistence.LoadGroupGrant(HttpContext.Current.Session["InstanceName"] as string, groupId);
                        res.SetSuccess();
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        public static ActionResult Save(ItemGrant grant, long groupId, string connectionString, string instanceName)
        {
            if (grant == null || string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(instanceName))
            {
                return ActionResult.NoAction;
            }

            string source = "ItemGrant::Save";
            /* CREATE PROCEDURE Core_ItemGrant_Save
             *   @GroupId bigint,
             *   @ItemName nvarchar(50),
             *   @CanRead bit,
             *   @CanWrite bit,
             *   @CanDelete bit */
            var res = ActionResult.NoAction;

            using (SqlCommand cmd = new SqlCommand("Core_ItemGrant_Save"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@GroupId", groupId));
                cmd.Parameters.Add(DataParameter.Input("@ItemName", grant.ItemName, 50));
                cmd.Parameters.Add(DataParameter.Input("@CanRead", grant.Read));
                cmd.Parameters.Add(DataParameter.Input("@CanWrite", grant.Write));
                cmd.Parameters.Add(DataParameter.Input("@CanDelete", grant.Delete));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandTimeout = 120;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        public static ItemGrant ByItem(ApplicationUser user, string itemName)
        {
            ItemGrant res = ItemGrant.Empty;
            if (user.Core)
            {
                res.Delete = true;
                res.Write = true;
                res.Read = true;
                res.ItemName = itemName;
                res.ItemLabel = itemName;
            }
            else if (user.Grants.Any(g => g.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                res = user.Grants.Where(g => g.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)).First();
            }

            return res;
        }

        /*public static ItemGrant ByItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return ItemGrant.Empty;
            }

            if (SecurityPersistence.UserEffectiveGrants(HttpContext.Current.Session["InstanceName"] as string, HttpContext.Current.Session["User"] as ApplicationUser).Any(g => g.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                return SecurityPersistence.UserEffectiveGrants(HttpContext.Current.Session["InstanceName"] as string, HttpContext.Current.Session["User"] as ApplicationUser).Where(g => g.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)).First();
            }

            return ItemGrant.Empty;
        }*/

        public static ActionResult SetGroupGrant(ReadOnlyCollection<ItemGrant> grants, long groupId, string connectionString)
        {
            var res = ActionResult.NoAction;

            if (grants == null)
            {
                return res;
            }

            //// Instance instance = HttpContext.Current.Session["Instance"] as Instance;
            //// CustomerFramework customerConfig = HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
            if (1 == 2)
            {
                /*using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = string.Format(
                        CultureInfo.InvariantCulture,
                        @"DELETE FROM Grant_Item WHERE GroupId = {0}",
                        groupId);
                    cmd.CommandType = CommandType.Text;
                    using (SqlConnection cnn = new SqlConnection(customerConfig.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }

                Parallel.ForEach(grants,
                () =>
                {
                    var con = new SqlConnection(customerConfig.Config.ConnectionString);
                    var cmd = con.CreateCommand();
                    con.Open();
                    cmd.CommandText = "Core_ItemGrant_Save";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@ItemName", SqlDbType.NVarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@CanRead", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@CanWrite", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@CanDelete", SqlDbType.Bit));
                    cmd.Prepare();
                    return new { Conn = con, Cmd = cmd };
                },
                (grant, pls, localInit) =>
                {
                    localInit.Cmd.Parameters["@GroupId"].Value = groupId;
                    localInit.Cmd.Parameters["@ItemName"].Value = grant.ItemName;
                    localInit.Cmd.Parameters["@CanRead"].Value = grant.Read;
                    localInit.Cmd.Parameters["@CanWrite"].Value = grant.Write;
                    localInit.Cmd.Parameters["@CanDelete"].Value = grant.Delete;
                    localInit.Cmd.ExecuteNonQuery();
                    return localInit;
                },
                (localInit) =>
                {
                    localInit.Cmd.Dispose();
                    localInit.Conn.Dispose();
                    SecurityPersistence.LoadGroupGrant(instance.Name, groupId);
                });

                res.SetSuccess();*/
            }
            else
            {
                string errorMessage = string.Empty;
                foreach (ItemGrant grant in grants)
                {
                    ActionResult t = ItemGrant.Save(grant, groupId, connectionString);
                    if (!t.Success)
                    {
                        errorMessage += t.MessageError;
                    }
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    res.SetFail(errorMessage);
                }
                else
                {
                    res.SetSuccess();
                }
            }

            return res;
        }

        public void AddFieldGrant(FieldGrant fieldGrant)
        {
            this.PreventNullFieldsGrant();
            this.fieldsGrant.Add(fieldGrant);
        }

        private void PreventNullFieldsGrant()
        {
            if (this.fieldsGrant == null)
            {
                this.fieldsGrant = new List<FieldGrant>();
            }
        }
    }
}