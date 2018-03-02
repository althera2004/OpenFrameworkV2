// --------------------------------
// <copyright file="Group.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
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
    using OpenFramework;
    using OpenFramework.Core.Bindings;
    using OpenFramework.DataAccess;
    using OpenFramework.ItemManager;
    using OpenFramework.Customer;

    /// <summary>
    /// Implements security group class
    /// </summary>
    public sealed class Group
    {
        /// <summary>Members of group</summary>
        private List<ApplicationUser> members;

        /// <summary>Grants of group</summary>
        private List<ItemGrant> grants;

        public static Group Empty
        {
            get
            {
                return new Group()
                {
                    Id = 0,
                    Description = string.Empty,
                    Deletable = true,
                    Active = false
                };
            }
        }

        public static Group Admin
        {
            get
            {
                return new Group()
                {
                    Id = 1,
                    Description = "admin",
                    Deletable = false,
                    Active = true
                };
            }
        }

        public long Id { get; set; }

        public string Description { get; set; }

        public string Observations { get; set; }

        public bool Deletable { get; set; }

        public bool Active { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public ApplicationUser ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Description"":""{1}"",""Observations"":""{5}"",""Deletable"":{2},""Active"":{3},""Members"":{4},""Grants"":{6}}}",
                    this.Id,
                    ToolsJson.JsonCompliant(this.Description),
                    ConstantValue.Value(this.Deletable),
                    ConstantValue.Value(this.Active),
                    ApplicationUser.JsonList(this.Members),
                    ToolsJson.JsonCompliant(this.Observations),
                    ItemGrant.JsonList(this.Grants));
            }
        }

        public ReadOnlyCollection<ItemGrant> Grants
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<ItemGrant>();
                }

                return new ReadOnlyCollection<ItemGrant>(this.grants);
            }
        }

        public string NormalizedDescription
        {
            get
            {
                return this.Description.Replace(" ", string.Empty).Trim();
            }
        }

        public ReadOnlyCollection<ApplicationUser> Members
        {
            get
            {
                this.PreventNullMembers();
                return new ReadOnlyCollection<ApplicationUser>(this.members);
            }
        }

        public static Group GetById(string instanceName, long id, string connectionString)
        {
            if (string.IsNullOrEmpty(instanceName) || id == 0)
            {
                return Group.Empty;
            }

            /* ALTER PROCEDURE [dbo].[Core_Group_GetById]
             *   GroupId bigint */
            Group res = Group.Empty;
            using (SqlCommand cmd = new SqlCommand("Core_Group_GetById"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@GroupId", id));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = id;
                                res.Description = rdr.GetString(1);
                                res.Deletable = rdr.GetBoolean(2);
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(3),
                                    FirstName = rdr.GetString(4),
                                    LastName = rdr.GetString(5)
                                };
                                res.CreatedOn = rdr.GetDateTime(6);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(7),
                                    FirstName = rdr.GetString(8),
                                    LastName = rdr.GetString(9)
                                };
                                res.ModifiedOn = rdr.GetDateTime(10);
                                res.Active = rdr.GetBoolean(11);
                                res.Observations = rdr.GetString(12);
                                res.GetMembers(connectionString);
                                res.GetGrants(connectionString);
                            }
                        }
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

        public static string JsonList(ReadOnlyCollection<Group> groups)
        {
            if (groups == null)
            {
                return ConstantValue.EmptyJsonList;
            }

            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Group group in groups)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(Environment.NewLine);
                res.Append(group.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static Group GetById(long groupId, string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Group::GetById({0})", groupId);
            Group res = Group.Empty;
            using (SqlCommand cmd = new SqlCommand("Core_Group_GetById"))
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
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = rdr.GetInt64(ColumnsGroupGet.Id);
                                res.Description = rdr.GetString(ColumnsGroupGet.Description);
                                res.Observations = rdr.GetString(ColumnsGroupGet.Observations);
                                res.Deletable = rdr.GetBoolean(ColumnsGroupGet.CanBeDeleted);
                                res.Active = rdr.GetBoolean(ColumnsGroupGet.Active);
                                res.CreatedOn = rdr.GetDateTime(ColumnsGroupGet.CreatedOn);
                                res.ModifiedOn = rdr.GetDateTime(ColumnsGroupGet.ModifiedOn);
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                    FirstName = rdr.GetString(ColumnsGroupGet.CreatedByFirstName),
                                    LastName = rdr.GetString(ColumnsGroupGet.CreatedByLastName)
                                };

                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                    FirstName = rdr.GetString(ColumnsGroupGet.ModifiedByFirstName),
                                    LastName = rdr.GetString(ColumnsGroupGet.ModifiedByLastName)
                                };

                                res.GetMembers(connectionString);
                            }
                        }
                    }
                    catch (NullReferenceException ex)
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

            return res;
        }

        /// <summary>
        /// Get all security groups
        /// </summary>
        /// <returns>Readonly collection of security groups</returns>
        public static ReadOnlyCollection<Group> GetAll(string connectionString)
        {
            string source = "Group::GetAll()";
            List<Group> res = new List<Group>();
            using (SqlCommand cmd = new SqlCommand("Core_Group_GetAll"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
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
                                Group newGroup = new Group()
                                {
                                    Id = rdr.GetInt64(ColumnsGroupGet.Id),
                                    Description = rdr.GetString(ColumnsGroupGet.Description),
                                    Observations = rdr.GetString(ColumnsGroupGet.Observations),
                                    Deletable = rdr.GetBoolean(ColumnsGroupGet.CanBeDeleted),
                                    CreatedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                        FirstName = rdr.GetString(ColumnsGroupGet.CreatedByFirstName),
                                        LastName = rdr.GetString(ColumnsGroupGet.CreatedByLastName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsGroupGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                        FirstName = rdr.GetString(ColumnsGroupGet.ModifiedByFirstName),
                                        LastName = rdr.GetString(ColumnsGroupGet.ModifiedByLastName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsGroupGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsGroupGet.Active)
                                };

                                newGroup.PreventNullMembers();
                                res.Add(newGroup);
                            }
                        }
                    }
                    catch (NullReferenceException ex)
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

            return new ReadOnlyCollection<Group>(res);
        }

        public static void ClearMembers(long groupId, string connectionString)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = string.Format(
                    CultureInfo.InvariantCulture,
                    @"DELETE FROM Core_Membership WHERE GroupId = {0}",
                    groupId);
                cmd.CommandType = CommandType.Text;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Updates users in security group
        /// </summary>
        /// <param name="groupId">Group identifier</param>
        /// <param name="users">Users to update</param>
        /// <returns>Result of action</returns>
        public static ActionResult UpdateUsers(long groupId, long[] users,long applicationUserId, string connectionString)
        {
            /* ALTER PROCEDURE Core_Group_AddMember
             *   @GroupId bigint,
             *   @UserId bigint,
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;

            ClearMembers(groupId, connectionString);

            if (1==2)
            {
                /*Parallel.ForEach(users,
                    () =>
                    {
                        var con = new SqlConnection(customerConfig.Config.ConnectionString);
                        var cmd = con.CreateCommand();
                        con.Open();
                        cmd.CommandText = "Core_Group_AddMember";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.BigInt));
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt));
                        cmd.Parameters.Add(new SqlParameter("@ApplicationUserId", SqlDbType.BigInt));
                        cmd.Prepare();
                        return new { Conn = con, Cmd = cmd };
                    },
                    (user, pls, localInit) =>
                    {
                        localInit.Cmd.Parameters["@GroupId"].Value = groupId;
                        localInit.Cmd.Parameters["@UserId"].Value = user;
                        localInit.Cmd.Parameters["@ApplicationUserId"].Value = actualUser.Id;
                        localInit.Cmd.ExecuteNonQuery();
                        return localInit;
                    },
                    (localInit) =>
                    {
                        localInit.Cmd.Dispose();
                        localInit.Conn.Dispose();
                    });
                res.SetSuccess();*/
            }
            else
            {
                string errorMessage = string.Empty;
                foreach (long userId in users)
                {
                    ActionResult t = Group.SetUser(groupId, userId, applicationUserId,connectionString);
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

            //// SecurityPersistence.LoadGroupMembers(customerConfig.Name, groupId);
            return res;
        }

        /// <summary>
        /// Adds a user to a security group
        /// </summary>
        /// <param name="groupId">Group identifier</param>
        /// <param name="userId">Identifier of user to be added</param>
        /// <param name="applicationUserId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetUser(long groupId, long userId, long applicationUserId,string connectionString)
        {
            string source = "ItemGrant::Save";
            /* ALTER PROCEDURE Core_Group_AddMember
             *   @GroupId bigint,
             *   @UserId bigint,
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Core_Group_AddMember"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@GroupId", groupId));
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        //// SecurityPersistence.LoadGroupMembers(HttpContext.Current.Session["InstanceName"] as string, groupId);
                        res.SetSuccess();
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex as Exception);
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex as Exception);
                    }
                    catch (NotSupportedException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                        res.SetFail(ex as Exception);
                    }
                }
            }

            return res;
        }

        public void EmptyMembers()
        {
            this.PreventNullMembers();
            this.members.Clear();
        }

        public void AddMember(ApplicationUser user)
        {
            this.PreventNullMembers();
            this.members.Add(user);
        }

        public void RemoveMember(long userId)
        {
            if (this.members != null)
            {
                if (this.members.Any(m => m.Id == userId))
                {
                    ApplicationUser victim = this.members.Where(m => m.Id == userId).First();
                    this.members.Remove(victim);
                }
            }
        }

        public void GetMembers(string connectionString)
        {
            string source = "Group::GetAll()";
            this.PreventNullMembers();
            using (SqlCommand cmd = new SqlCommand("Core_Group_GetUsers"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@GroupId", this.Id));
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
                                this.members.Add(new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(0),
                                    FirstName = rdr.GetString(1),
                                    LastName = rdr.GetString(2),
                                    Email = rdr.GetString(3)
                                });
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
                    catch (ArgumentOutOfRangeException ex)
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
        }

        /*public void GetGrants()
        {
            this.grants = ItemGrant.ByGroup(this.Id).ToList();
        }*/

        public void GetGrants(string connectionString)
        {
            this.grants = ItemGrant.ByGroup(this.Id, connectionString).ToList();
        }

        private void PreventNullMembers()
        {
            if (this.members == null)
            {
                this.members = new List<ApplicationUser>();
            }
        }

        public ActionResult Update(long applicationUserId, string connectionString)
        {
            var res = ActionResult.NoAction;
            /*CREATE PROCEDURE Core_Group_Update
             *   @Id bigint,
             *   @Description  nvarchar(100),
             *   @Observations nvarchar(150),
             *   @ApplicationUserId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_Group_Update"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations, 150));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        //// SecurityPersistence.Load(customerConfig.Name, customerConfig.Id);
                        res.SetSuccess("UPDATE");
                    }
                    catch (Exception ex)
                    {
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

        public ActionResult Insert(long applicationUserId, string connectionString)
        {
            var res = ActionResult.NoAction;
            /*CREATE PROCEDURE Core_Group_Update
             *   @Id bigint,
             *   @Description  nvarchar(100),
             *   @Observations nvarchar(150),
             *   @ApplicationUserId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_Group_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations, 150));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString(), CultureInfo.InvariantCulture);
                        //// SecurityPersistence.Load(customerConfig.Name, customerConfig.Id);
                        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "INSERT|{0}", this.Id));
                    }
                    catch (Exception ex)
                    {
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
    }
}