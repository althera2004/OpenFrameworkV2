// --------------------------------
// <copyright file="Save.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.CRUD
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using OpenFramework.ItemManager;
    using OpenFramework.Security;
    using OpenFramework.Customer;

    /// <summary>Implements save actions for item into database</summary>
    public static class Save
    {
        /*/// <summary>
        /// Inserts item data into database
        /// </summary>
        /// <param name="itemBuilder">Item instance</param>
        /// <param name="fromImport">Indicates if insert is from import (not required)</param>
        /// <returns>Result of action</returns>
        public static ActionResult Insert(ItemBuilder itemBuilder,long userId, bool fromImport = false)
        {
            return Insert(itemBuilder, Basics.ActualInstance.Config.ConnectionString, userId, fromImport);
        }*/

        /// <summary>Inserts item data into database</summary>
        /// <param name="itemBuilder">Item instance</param>
        /// <param name="connectionString">String connection to database</param>
        /// <param name="userId">User identifier</param> 
        /// <param name="fromImport">Indicates if insert is from import (not required)</param>
        /// <returns>Result of action</returns>
        public static ActionResult Insert(ItemBuilder itemBuilder, string connectionString, long userId, bool fromImport = false)
        {
            if (itemBuilder == null)
            {
                return ActionResult.NoAction;
            }

            var res = ActionResult.NoAction;
            string query = SqlServerWrapper.InsertQuery(itemBuilder, userId);

            if (string.IsNullOrEmpty(query))
            {
                return ActionResult.NoAction;
            }

            // Prepare code sequence
            /*if (itemBuilder.Definition.Fields.Any(f => !string.IsNullOrEmpty(f.CodeSequence)))
            {
                foreach (ItemField field in itemBuilder.Definition.Fields.Where(f => !string.IsNullOrEmpty(f.CodeSequence)))
                {
                    if (itemBuilder[field.Name] != null)
                    {
                        var codeSequence = CodeSequencePersistence.GetByName(itemBuilder.InstanceName, field.CodeSequence);
                        itemBuilder[field.Name] = codeSequence.SetActualValue(userId, connectionString, itemBuilder.InstanceName);
                    }
                }
            }*/

            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();

                        // ExecuteScalar devuelve el Id del último insertado
                        var id = cmd.ExecuteScalar();

                        // PersistentData
                        if (itemBuilder.ContainsKey("Id"))
                        {
                            itemBuilder["Id"] = id;
                        }
                        else
                        {
                            itemBuilder.Add("Id", id);
                        }

                        itemBuilder["Active"] = true;
                        res.SetSuccess();
                        res.MessageError = itemBuilder.ItemName;
                        res.ReturnValue = string.Format(CultureInfo.InvariantCulture, "INSERT|{0}", id);
                        ActionLog.Trace(itemBuilder.ItemName + "_", itemBuilder.Id, itemBuilder.Json, itemBuilder.InstanceName, connectionString);
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "Insert:" + query);
                        res.SetFail(ex);
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

            return res;
        }

        /*/// <summary>
        /// Updates item data in database
        /// </summary>
        /// <param name="itemBuilder">Item instance</param>
        /// <param name="fromImport">Indicates if update is from import (not required)</param>
        /// <returns>Result of action</returns>
        public static ActionResult Update(ItemBuilder itemBuilder,long userId, bool fromImport = false)
        {
            return Update(itemBuilder, Basics.ActualInstance.Config.ConnectionString, userId, fromImport);
        }*/

        /// <summary>Updates item data in database</summary>
        /// <param name="itemBuilder">Item instance</param>
        /// <param name="connectionString">String connection to database</param>
        /// <param name="userId">User identifier</param>
        /// <param name="fromImport">Indicates if update is from import (not required)</param>
        /// <returns>Result of action</returns>
        public static ActionResult Update(ItemBuilder itemBuilder, string instanceName, long userId, bool fromImport = false)
        {
            var res = ActionResult.NoAction;
            if (itemBuilder == null)
            {
                res.SetFail("No itemBuilder defined");
                return res;
            }

            if (string.IsNullOrEmpty(instanceName))
            {
                res.SetFail("No connection string specified");
                return res;
            }

            string query = SqlServerWrapper.UpdateQuery(itemBuilder, userId);
            using (SqlCommand cmd = new SqlCommand(query))
            {
                try
                {
                    if (string.IsNullOrEmpty(query))
                    {
                        return ActionResult.NoAction;
                    }

                    CustomerFramework instance = new CustomerFramework() { Name = instanceName };
                    instance.LoadConfig();

                    using (SqlConnection cnn = new SqlConnection(instance.Config.ConnectionString))
                    {
                        var old = Read.ById(itemBuilder.Id, itemBuilder.Definition, instanceName);                        
                        cmd.Connection = cnn;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                        string differences = ItemBuilder.Differences(old, itemBuilder);
                        /* //Persistent data
                        res = DataPersistence.Update(itemBuilder);
                        if (!string.IsNullOrEmpty(itemBuilder.Definition.ParentItem))
                        {
                            res = DataPersistence.ReloadItem(itemBuilder.InstanceName, itemBuilder.Definition.ParentItem, itemBuilder.ItemName, true);
                        }

                        if (itemBuilder.Definition.InheritedItems.Count > 0)
                        {
                            res = DataPersistence.ReloadItem(itemBuilder.InstanceName, itemBuilder.ItemName, string.Empty, false);
                        }*/

                        if (res.Success)
                        {
                            ApplicationUser user = ApplicationUser.GetById(userId, instance.Config.ConnectionString);
                            ActionLog.Trace(itemBuilder.ItemName + "_", itemBuilder.Id, differences, itemBuilder.ItemName, user.TraceDescription);
                            res.ReturnValue = "UPDATE";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.LogException(ex as Exception, "Update:" + query);
                    res.SetFail(query + "<br />" + ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return res;
        }

        /// <summary>Activate an item in database</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="userDescription">User description for trace purposses</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <param name="connectionString">String connection to database</param>
        /// <returns>Result of action</returns>
        public static ActionResult Active(long itemId, long userId, string userDescription, string itemName, string instanceName, string connectionString)
        {
            var res = ActionResult.NoAction;
            string query = SqlServerWrapper.ActiveQuery(itemId, userId, itemName, instanceName);
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        ActionLog.Trace(itemName + "_", itemId, "ACTIVE", instanceName, userDescription);
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        /// <summary>Inactivate an item in database</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="userDescription">User description for trace purposses</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <param name="connectionString">String connection to database</param>
        /// <returns>Result of action</returns>
        public static ActionResult Inactive(long itemId, long userId, string userDescription, string itemName, string instanceName, string connectionString)
        {
            var res = ActionResult.NoAction;
            string query = SqlServerWrapper.InactiveQuery(itemId, userId, itemName, instanceName);
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(itemName);
                        //// DataPersistence.Delete(instanceName, itemName, itemId);
                        ActionLog.Trace(itemName + "_", itemId, "INACTIVE", instanceName, userDescription);
                        var item = new ItemBuilder(itemName, instanceName);
                        /*if (item.Definition.Removes != null)
                        {
                            if (item.Definition.Removes.Count > 0)
                            {
                                foreach (string removeItemName in item.Definition.Removes)
                                {
                                    var victims = DataPersistence.GetAllByField(instanceName, removeItemName, itemName + "Id", itemId);
                                    foreach (ItemBuilder victim in victims)
                                    {
                                        Inactive(victim.Id, userId, userDescription, removeItemName, instanceName, connectionString);
                                    }
                                }
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        /// <summary>Delete an item form database</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="userDescription">User description for trace purposses</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <param name="connectionString">String connection to database</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(long itemId, long userId, string userDescription, string itemName, string instanceName, string connectionString)
        {
            var res = ActionResult.NoAction;
            string query = SqlServerWrapper.DeleteQuery(itemId, userId, itemName, instanceName);
            var instance = new CustomerFramework() { Name = instanceName };
            instance.LoadConfig();
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlConnection cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(itemName);
                        ActionLog.Trace(itemName + "_", itemId, "DELETE", instanceName, userDescription);
                        var item = new ItemBuilder(itemName, instanceName);
                        /*if (item.Definition.Removes != null)
                        {
                            if (item.Definition.Removes.Count > 0)
                            {
                                foreach (string removeItemName in item.Definition.Removes)
                                {
                                    var victims = DataPersistence.GetAllByField(instanceName, removeItemName, itemName + "Id", itemId);
                                    foreach (ItemBuilder victim in victims)
                                    {
                                        Delete(victim.Id, userId, userDescription, removeItemName, instanceName, connectionString);
                                    }
                                }
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }
    }
}