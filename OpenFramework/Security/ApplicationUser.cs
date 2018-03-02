// --------------------------------
// <copyright file="ApplicationUser.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFramework.Core.Bindings;
    using OpenFramework.DataAccess;
    using OpenFramework.Core;
    using OpenFramework.Customer;

    /// <summary>Implements class to manage application user</summary>
    public sealed class ApplicationUser
    {
        private const string OpenFrameworkMail = "openframework@openframework.es";

        /// <summary>Instance where user is part</summary>
        private List<Instance> instances;

        /// <summary>Groups of user</summary>
        private List<long> groups;

        /// <summary>Grants of user</summary>
        private List<ItemGrant> grants;

        private List<long> scopeView;

        public string TraceDescription
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}::{1}", this.Id, this.FullName);
            }
        }

        public static ApplicationUser OpenFramework
        {
            get
            {
                return new ApplicationUser()
                {
                    Id = 0,
                    UserInstanceId = 0,
                    Email = OpenFrameworkMail,
                    Profile = UserProfile.Empty,
                    Core = true,
                    Active = true,
                    FirstName = "Open",
                    LastName = "Framework",
                    FailedSignIn = 0,
                    MustResetPassword = false,
                    Locked = false,
                    IMEI = string.Empty,
                    scopeView = new List<long>(),
                    grants = new List<ItemGrant>(),
                    groups = new List<long>(),
                    instances = new List<Instance>()
                };
            }
        }

        /// <summary>Gets an empty user</summary>
        public static ApplicationUser Empty
        {
            get
            {
                return new ApplicationUser()
                {
                    Id = -1,
                    UserInstanceId = -1,
                    Email = string.Empty,
                    Profile = UserProfile.Empty,
                    instances = new List<Instance>(),
                    groups = new List<long>(),
                    scopeView = new List<long>(),
                    grants = new List<ItemGrant>()
                };
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

        public ReadOnlyCollection<long> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<long>();
                }

                return new ReadOnlyCollection<long>(this.groups);
            }
        }

        public ReadOnlyCollection<long> ScopeView
        {
            get
            {
                if (this.scopeView == null)
                {
                    this.scopeView = new List<long>();
                }

                return new ReadOnlyCollection<long>(this.scopeView);
            }
        }

        public string Password { get; set; }
        /*{
            get
            {
                string res = string.Empty;

                * CREATE PROCEDURE [dbo].[User_GetPassword]
                 *   @UserId bigint *
                using (SqlCommand cmd = Command.Stored("User_GetPassword", InstanceMode.Connection))
                {
                    cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res = rdr.GetString(0);
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
                return res;
            }
        }*/

        public string ScopeViewList
        {
            get
            {
                if (this.scopeView == null)
                {
                    this.scopeView = new List<long>();
                    return string.Empty;
                }

                var res = new StringBuilder();
                bool first = true;
                foreach (long scopeViewId in this.scopeView)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.AppendFormat(CultureInfo.InvariantCulture, "{0}", scopeViewId);
                }

                return res.ToString();
            }
        }

        /// <summary>Gets or sets user identifier on OpenFramework</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets user identifier on Instance</summary>
        public long UserInstanceId { get; set; }

        /// <summary>Gets or sets user email address</summary>
        public string Email { get; set; }

        /// <summary>Gets or sets user profile</summary>
        public UserProfile Profile { get; set; }

        /// <summary>Gets or sets the number of previous failed logon attempts</summary>
        public int FailedSignIn { get; set; }

        /// <summary>Gets or sets a value indicating whether user must reset password at next logon</summary>
        public bool MustResetPassword { get; set; }

        /// <summary>Gets or sets a value indicating whether user is locked</summary>
        public bool Locked { get; set; }

        /// <summary>Gets or sets a value indicating whether user is active</summary>
        public bool Active { get; set; }

        /// <summary>Gets or sets a value indicating whether user is member of core</summary>
        public bool Core { get; set; }

        /// <summary>Gets or sets the user that creates this</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets the date of creation</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the user that make the last modification</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the date of last modification</summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>Gets or sets the first name of user</summary>
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name of user</summary>
        public string LastName { get; set; }

        /// <summary>Gets the full name of user</summary>
        public string FullName
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    ToolsJson.JsonCompliant(this.FirstName),
                    ToolsJson.JsonCompliant(this.LastName)).Trim();
            }
        }

        /// <summary>Gets or sets IMEI code inorder to acceso by app</summary>
        public string IMEI { get; set; }

        /// <summary>Gets the frameworks on user has access</summary>
        public ReadOnlyCollection<Instance> Frameworks
        {
            get
            {
                if (this.instances == null)
                {
                    this.instances = new List<Instance>();
                }

                return new ReadOnlyCollection<Instance>(this.instances);
            }
        }

        /// <summary>Gets user description</summary>
        public string Description
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FullName))
                {
                    return this.FullName;
                }

                return this.Email;
            }
        }

        /// <summary>Gets the JSON structure of user</summary>
        public string Json
        {
            get
            {
                var groupsJson = new StringBuilder();
                if (this.groups != null)
                {
                    if (this.groups.Count > 0)
                    {
                        bool first = true;
                        foreach (long groupId in this.groups)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                groupsJson.Append(",");
                            }

                            groupsJson.Append(groupId);
                        }
                    }
                }

                var scopeViewJson = new StringBuilder();
                if (this.scopeView != null)
                {
                    if (this.scopeView.Count > 0)
                    {
                        if (Basics.ActualInstance.Config.MultiCompany)
                        {
                            bool first = true;
                            foreach (long scopeViewId in this.scopeView)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    scopeViewJson.Append(",");
                                }

                                scopeViewJson.Append(scopeViewId);
                            }
                        }
                    }
                }

                var grants = new StringBuilder("[");
                if (this.grants != null)
                {
                    bool first = true;
                    foreach(var grant in this.grants)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            grants.Append(",");
                        }

                        grants.Append(grant.Json);
                    }

                }
                grants.Append("]");

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""UserInstanceId"":{11}, ""Email"":""{1}"",""IMEI"":""{10}"", ""FirstName"":""{2}"", ""LastName"":""{3}"", ""FullName"":""{4}"", ""Core"":{5}, ""Active"":{6},  ""Locked"":{7}, ""Groups"":[{8}], ""ScopeView"":[{9}],""Grants"":{13}, ""TraceDescription"":""{12}""}}",
                    this.Id,
                    this.Email,
                    ToolsJson.JsonCompliant(this.FirstName),
                    ToolsJson.JsonCompliant(this.LastName),
                    ToolsJson.JsonCompliant(this.FullName),
                    ConstantValue.Value(this.Core),
                    ConstantValue.Value(this.Active),
                    ConstantValue.Value(this.Locked),
                    groupsJson,
                    scopeViewJson,
                    this.IMEI,
                    this.UserInstanceId,
                    ToolsJson.JsonCompliant(this.TraceDescription),
                    grants);
            }
        }

        /// <summary>
        /// Gets the JSON key/value structure of user
        /// </summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"), 
                    @"{{""Id"":{0},""UserInstanceId"":{2},""Value"":""{1}""}}",
                    this.Id,
                    this.Description,
                    this.UserInstanceId);
            }
        }

        private static ReadOnlyCollection<UserRelation> Relation
        {
            get
            {
                var res = new List<UserRelation>();
                using (SqlCommand cmd = new SqlCommand("Core_User_GetFrameworkRelation"))
                {
                    using (SqlConnection cnn = new SqlConnection(Basics.ActualInstance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new UserRelation()
                                    {
                                        Id = rdr.GetInt64(0),
                                        Email = rdr.GetString(1)
                                    });
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

                return new ReadOnlyCollection<UserRelation>(res);
            }
        }

        /// <summary>
        /// Gets a JSON list of all users in the current instance
        /// </summary>
        /// <param name="instanceId">Instance identifier</param>
        /// <returns>A JSON list of all users in the current instance</returns>
        public static string GetAllJson(string instanceName)
        {
            return JsonList(GetAll(instanceName));
        }

        /// <summary>
        /// Gets all users of current instance
        /// </summary>
        /// <param name="instanceId">Instance identifier</param>
        /// <returns>A list of all users of current instance</returns>
        public static ReadOnlyCollection<ApplicationUser> GetAll(string instanceName)
        {
            var res = new List<ApplicationUser>();
            var relation = new ReadOnlyCollection<UserRelation>(new List<UserRelation>());

            CustomerFramework instance = new CustomerFramework() { Name = instanceName };
            instance.LoadConfig();
            using (SqlCommand cmd = Command.Stored("User_GetByInstanceId", instance.Config.ConnectionString))
            {
                cmd.Parameters.Add(DataParameter.Input("@InstanceId", instance.Id));
                try
                {
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string email = rdr.GetString(ColumnsApplicationUserGetAll.Email);

                            long userInstanceId = -1;
                            if (InstanceMode.OnPremise)
                            {
                                userInstanceId = rdr.GetInt64(ColumnsApplicationUserGetAll.OpenFrameworkId);
                            }
                            else
                            {
                                if (relation.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                                {
                                    userInstanceId = relation.First(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).Id;
                                }
                            }

                            res.Add(new ApplicationUser()
                            {
                                Id = rdr.GetInt64(ColumnsApplicationUserGetAll.Id),
                                UserInstanceId = userInstanceId,
                                Email = email,
                                FirstName = rdr.GetString(ColumnsApplicationUserGetAll.FirstName),
                                LastName = rdr.GetString(ColumnsApplicationUserGetAll.LastName),
                                Core = rdr.GetBoolean(ColumnsApplicationUserGetAll.Core),
                                Locked = rdr.GetBoolean(ColumnsApplicationUserGetAll.Locked),
                                Active = rdr.GetBoolean(ColumnsApplicationUserGetAll.Active)/*,
                                CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGetAll.CreatedBy),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGetAll.CreatedByFirstName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGetAll.CreatedByLastName)
                                },
                                CreatedOn = rdr.GetDateTime(ColumnsApplicationUserGetAll.CreatedOn),
                                ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGetAll.ModifiedBy),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGetAll.ModifiedByFirstName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGetAll.ModifiedByLastName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsApplicationUserGetAll.ModifiedOn)*/
                            });
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

            return new ReadOnlyCollection<ApplicationUser>(res);
        }

        /// <summary>
        /// Gets a JSON list of users collection
        /// </summary>
        /// <param name="list">Collection of users</param>
        /// <returns>JSON list of users collection</returns>
        public static string JsonList(ReadOnlyCollection<ApplicationUser> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            if (list != null)
            {
                foreach (ApplicationUser user in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(user.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Gets all groups identifiers of user
        /// </summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <returns>A list of all groups identifiers of user</returns>
        public static ReadOnlyCollection<long> GroupsId(long applicationUserId, string instanceName)
        {
            var res = new List<long>();
            CustomerFramework instance = new CustomerFramework() { Name = instanceName };
            instance.LoadConfig();
            using (SqlCommand cmd = new SqlCommand("Core_User_GetGroupsId"))
            {
                using (SqlConnection cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(rdr.GetInt64(0));
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "GroupsId(" + applicationUserId + ")");
                    }
                }
            }

            return new ReadOnlyCollection<long>(res);
        }

        /// <summary>
        /// Try to logon
        /// </summary>
        /// <param name="email">Email that identifies user</param>
        /// <param name="password">User password</param>
        /// <returns>Result of action</returns>
        public static ActionResult LogOn(string email, string password, string connectionString)
        {
            var res = ActionResult.NoAction;

            /* CREATE PROCEDURE [dbo].[Login]
             *   @email nvarchar(150),
             *   @pass nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("Login"))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@email", email, 150));
                    cmd.Parameters.Add(DataParameter.Input("@pass", password, 50));
                    try
                    {
                        var user = Empty;
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                user = GetById(rdr.GetInt64(0), connectionString);
                                user.GetInstances();
                                user.ObtainGroups(connectionString);
                                user.ObtainScopeView(connectionString);
                                user.ObtainsGrants(connectionString);
                                res.SetSuccess(user);                                
                            }
                            else
                            {
                                res.SetFail("NOLOGIN");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "Login(" + email + ")");
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Try to logon
        /// </summary>
        /// <param name="email">Email that identifies user</param>
        /// <param name="password">User password</param>
        /// <returns>Result of action</returns>
        public static ActionResult LogOnV3(string email, string password, string connectionString)
        {
            var res = ActionResult.NoAction;

            /* CREATE PROCEDURE [dbo].[Login]
             *   @email nvarchar(150),
             *   @pass nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("Login"))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@email", email, 150));
                    cmd.Parameters.Add(DataParameter.Input("@pass", password, 50));
                    try
                    {
                        var user = Empty;
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                if (rdr.GetString(2).Equals("No login", StringComparison.OrdinalIgnoreCase))
                                {
                                    res.SetFail("NOLOGIN");
                                }
                                else
                                {
                                    res.SetSuccess(GetById(rdr.GetInt64(0), connectionString));
                                }
                            }
                            else
                            {
                                res.SetFail("NOLOGIN");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "Login(" + email + ")");
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        public static long GetIdByEmail(string email)
        {
            long res = -1;
            /* CREATE PROCEDURE GetUserIdByEmail
             *   @Email vnarchar(100) */
            using (SqlCommand cmd = new SqlCommand("GetUserIdByEmail"))
            {
                using (SqlConnection cnn = new SqlConnection(Basics.ActualInstance.Config.ConnectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@Email", email));
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = rdr.GetInt64(0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        res = -1;
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

        /// <summary>
        /// Gets an user for current instance by identifier
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Application user of current instance if exists</returns>
        public static ApplicationUser GetById(long userId, string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "ApplicationUser::GetById({0})", userId);
            var res = Empty;

            /* CREATE PROCEDURE Core_User_GetById
             *   @UserId bigint */
            using (SqlCommand cmd = new SqlCommand("User_GetById"))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt64(ColumnsApplicationUserGet.Id);
                                res.Core = rdr.GetBoolean(ColumnsApplicationUserGet.Core);
                                res.FirstName = rdr.GetString(ColumnsApplicationUserGet.FirstName);
                                res.LastName = rdr.GetString(ColumnsApplicationUserGet.LastName);
                                res.Email = rdr.GetString(ColumnsApplicationUserGet.Email);
                                res.MustResetPassword = rdr.GetBoolean(ColumnsApplicationUserGet.MustResetPassword);
                                res.FailedSignIn = rdr.GetInt32(ColumnsApplicationUserGet.FailedSignIn);
                                res.Locked = rdr.GetBoolean(ColumnsApplicationUserGet.Locked);
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGet.CreatedBy),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGet.FirstName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGet.LastName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsApplicationUserGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGet.ModifiedBy),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGet.ModifiedByFirstName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGet.ModifiedByLastName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsApplicationUserGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsApplicationUserGet.Active);
                                res.UserInstanceId = GetIdByEmail(res.Email);
                                res.ObtainsGrants(connectionString);
                                res.ObtainGroups(connectionString);
                                res.ObtainScopeView(connectionString);
                                res.GetInstances();
                            }                            
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                }
            }

            return res;
        }

        public void AddGroup(long groupId)
        {
            if (this.groups == null)
            {
                this.groups = new List<long>();
            }

            this.groups.Add(groupId);
        }

        public void AddScopeView(long scopeViewId)
        {
            if (this.scopeView == null)
            {
                this.scopeView = new List<long>();
            }

            this.scopeView.Add(scopeViewId);
        }

        /// <summary>Gets all groups identifiers of user</summary>
        public void GetGroupsId(string instanceName)
        {
            this.groups = GroupsId(this.Id, instanceName).ToList();
        }

        /// <summary>Load profile associated to user in the current instance</summary>
        public void LoadProfile(string connectionString)
        {
            this.Profile = UserProfile.GetByUserId(this.Id, connectionString);
        }

        /// <summary>Obtain all instances of framework</summary>
        public void GetInstances()
        {
            if(ConfigurationManager.ConnectionStrings["security"] == null)
            {
                return;
            }

            string source = string.Format(CultureInfo.InvariantCulture, "ApplicationUser::GetInstances({0})", this.Id);
            this.instances = new List<Instance>();
            using (SqlCommand cmd = new SqlCommand("User_GetInstances"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newInstance = new Instance()
                                {
                                    Id = rdr.GetInt64(ColumnsInstanceGet.Id),
                                    Key = rdr.GetString(ColumnsInstanceGet.Key),
                                    MultipleCore = rdr.GetBoolean(ColumnsInstanceGet.MultipleCore),
                                    Active = rdr.GetBoolean(ColumnsInstanceGet.Active),
                                    DefaultLanguage = LanguagePersistence.LanguageById((long)rdr.GetInt32(ColumnsInstanceGet.DefaultLanguage)),
                                    Name = rdr.GetString(ColumnsInstanceGet.Name),
                                    UsersLimit = rdr.GetInt32(ColumnsInstanceGet.UsersLimit),
                                    SubscriptionStart = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionStart),
                                    SubscriptionEnd = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionEnd),
                                    SubscriptionType = rdr.GetInt32(ColumnsInstanceGet.SubscriptionType)
                                };

                                if (Instance.Exists(newInstance.Name))
                                {
                                    newInstance.ObtainInstanceLanguages();
                                    this.instances.Add(newInstance);
                                }
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
        }

        public ActionResult UpdateGroups(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            if (this.groups == null)
            {
                res.SetSuccess();
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("Core_User_CleanGroups"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(Basics.ActualInstance.Config.ConnectionString))
                    {
                        cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "Core_MemberShip_Insert";
                            foreach (long groupId in this.groups)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                                cmd.Parameters.Add(DataParameter.Input("@GroupId", groupId));
                                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                                cmd.ExecuteNonQuery();
                            }

                            res.SetSuccess();
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }
                }
            }

            return res;
        }

        public ActionResult UpdateScope(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            if (this.scopeView == null)
            {
                res.SetSuccess();
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("Core_User_CleanScopeView"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(Basics.ActualInstance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "Core_CompanyMemberShip_Insert";
                            foreach (long scopeViewId in this.scopeView)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                                cmd.Parameters.Add(DataParameter.Input("@CompanyId", scopeViewId));
                                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                                cmd.ExecuteNonQuery();
                            }

                            res.SetSuccess();
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }
                }
            }

            return res;
        }

        public void ObtainScopeView(string connectionString)
        {
            this.scopeView = new List<long>();
            using (SqlCommand cmd = new SqlCommand("Core_User_GetScopeView"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                this.scopeView.Add(rdr.GetInt64(0));
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
        }

        public void ObtainGroups(string connectionString)
        {
            this.groups = new List<long>();
            using (SqlCommand cmd = new SqlCommand("Core_User_GetGroups"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    this.groups.Add(rdr.GetInt64(0));
                                }
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
        }

        public void ObtainsGrants(string connectionString)
        {
            this.grants = new List<ItemGrant>();
            using (SqlCommand cmd = new SqlCommand("Core_User_GetGrants"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                this.grants.Add(new ItemGrant()
                                {
                                    ItemName = rdr.GetString(1),
                                    Read = rdr.GetBoolean(2),
                                    Write = rdr.GetBoolean(3),
                                    Delete = rdr.GetBoolean(4),
                                    ItemLabel = string.Empty
                                });
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
        }

        /// <summary>
        /// Insert a new user in instance
        /// </summary>
        /// <param name="applicationUserId">Identifier of user that perfomrs action</param>
        /// <returns>Result of action</returns>
        public ActionResult InsertInstance(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            CustomerFramework instance = new CustomerFramework() { Name = instanceName };
            instance.LoadConfig();
            /* CREATE PROCEDURE Core_User_Insert
             *   @Id bigint output,
             *   @Email nvarchar(10),
             *   @FirstName nvarchar(10),
             *   @LastName nvarchar(10),
             *   @ApplicationUserId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_User_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@Email", this.Email));
                cmd.Parameters.Add(DataParameter.Input("@IMEI", this.IMEI, 20));
                cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName));
                cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        this.UserInstanceId = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());

                        var groupResult = this.UpdateGroups(applicationUserId);
                        if (groupResult.Success == false)
                        {
                            res.SetFail(groupResult.MessageError);
                        }
                        else
                        {
                            var scopeResult = this.UpdateScope(applicationUserId);
                            if (scopeResult.Success == false)
                            {
                                res.SetFail(scopeResult.MessageError);
                            }
                            else
                            {
                                res.SetSuccess();
                            }
                        }
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

        public static ActionResult ChangePassword(long frameworkId, string instanceName, string newPassword)
        {
            var res = ActionResult.NoAction;
            CustomerFramework instance = new CustomerFramework() { Name = instanceName };
                instance.LoadConfig();
                using (SqlCommand cmdFramework = new SqlCommand("Core_User_ChangePassword"))
                {
                    cmdFramework.CommandType = CommandType.StoredProcedure;
                    cmdFramework.Parameters.Add(DataParameter.Input("@Id", frameworkId));
                    cmdFramework.Parameters.Add(DataParameter.Input("@Password", newPassword, 50));

                    string cnsFrameowrk = ConfigurationManager.ConnectionStrings["security"].ConnectionString;
                    try
                    {
                        using (SqlConnection cnnFramework = new SqlConnection(cnsFrameowrk))
                        {
                            cmdFramework.Connection = cnnFramework;
                            cmdFramework.Connection.Open();
                            cmdFramework.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {

                        if (cmdFramework.Connection.State != ConnectionState.Closed)
                        {
                            cmdFramework.Connection.Close();
                        }
                    }
                }

            if (res.Success)
            {
                using (SqlCommand cmdInstance = new SqlCommand("Core_User_ChangePassword"))
                {
                    cmdInstance.CommandType = CommandType.StoredProcedure;
                    cmdInstance.Parameters.Add(DataParameter.Input("@Id", instance.Id));
                    cmdInstance.Parameters.Add(DataParameter.Input("@Password", newPassword, 50));

                    try
                    {
                        using (SqlConnection cnnInstance = new SqlConnection(Basics.ActualInstance.Config.ConnectionString))
                        {
                            cmdInstance.Connection = cnnInstance;
                            cmdInstance.Connection.Open();
                            cmdInstance.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {

                        if (cmdInstance.Connection.State != ConnectionState.Closed)
                        {
                            cmdInstance.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Insert a new user in OpenFramework securoty database
        /// </summary>
        /// <param name="applicationUserId">Identifier of user that perfomrs action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_User_Insert
             *  @Id bigint output,
             *  @Email nvarchar(100),
             *  @IMEI nvarchar(20),
             *  @FirstName nvarchar(50),
             *  @LastName nvarchar(50),
             *  @InstanceId bigint,
             *  @ApplicationUserId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_User_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@Email", this.Email));
                cmd.Parameters.Add(DataParameter.Input("@IMEI", this.IMEI, 20));
                cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName));
                cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName));
                cmd.Parameters.Add(DataParameter.Input("@InstanceId", Basics.ActualInstance.Id));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());

                        var groupResult = this.UpdateGroups(applicationUserId);
                        if (groupResult.Success == false)
                        {
                            res.SetFail(groupResult.MessageError);
                        }
                        else
                        {
                            var scopeResult = this.UpdateScope(applicationUserId);
                            if (scopeResult.Success == false)
                            {
                                res.SetFail(scopeResult.MessageError);
                            }
                            else
                            {
                                res.SetSuccess();
                            }
                        }
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

        public ActionResult Update(long applicationUserId, string connectionString)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_User_Update
             *   @Id bigint,
             *   @Email nvarchar(100),
             *   @IMEI nvarchar(20),
             *   @FirstName nvarchar(50),
             *   @LastName nvarchar(50),
             *   @ApplicationUserId bigint */
            using (SqlCommand cmd = new SqlCommand("Core_User_Update"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 100));
                cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 50));
                cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 50));
                cmd.Parameters.Add(DataParameter.Input("@IMEI", this.IMEI, 20));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();

                        var groupResult = this.UpdateGroups(applicationUserId);
                        if (groupResult.Success == false)
                        {
                            res.SetFail(groupResult.MessageError);
                        }
                        else
                        {
                            var scopeResult = this.UpdateScope(applicationUserId);
                            if (scopeResult.Success == false)
                            {
                                res.SetFail(scopeResult.MessageError);
                            }
                            else
                            {
                                // var customerConfig = Basics.ActualInstance;
                                // SecurityPersistence.Load(customerConfig.Name, customerConfig.Id);
                                res.SetSuccess();
                            }
                        }
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

        private struct UserRelation
        {
            public long Id { get; set; }

            public string Email { get; set; }
        }
    }
}