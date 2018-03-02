// --------------------------------
// <copyright file="UserProfile.cs" company="Sbrinna">
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
    using System.Text;
    using System.Web;
    using OpenFramework.Core;
    using OpenFramework.Core.Bindings;
    using OpenFramework.DataAccess;
    using OpenFramework.Multilanguage;

    /// <summary>
    /// Implements user profile
    /// </summary>
    public sealed class UserProfile
    {
        /// <summary>
        /// Gets a empty instance of user profile
        /// </summary>
        public static UserProfile Empty
        {
            get
            {
                return new UserProfile()
                {
                    Language = Language.SimpleEmpty,
                    LinkedIn = string.Empty,
                    ShowHelp = false,
                    Id = 0,
                    JobPosition = string.Empty,
                    Mobile = string.Empty,
                    Phone = string.Empty,
                    UserId = 0
                };
            }
        }

        /// <summary>
        /// Gets all user profiles
        /// </summary>
        public static ReadOnlyCollection<UserProfile> All
        {
            get
            {
                string source = "UserProfile::GetAll";
                List<UserProfile> res = new List<UserProfile>();
                /*using (SqlCommand cmd = new SqlCommand("UserProfile_GetAll"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    CustomerFramework customerConfig = HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
                    using (SqlConnection cnn = new SqlConnection(customerConfig.Config.ConnectionString))
                    {
                        try
                        {
                            cmd.Connection = cnn;
                            cnn.Open();
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new UserProfile()
                                    {
                                        Id = rdr.GetInt64(ColumnsUserProfileGet.Id),
                                        UserId = rdr.GetInt64(ColumnsUserProfileGet.UserId),
                                        FirstName = rdr.GetString(ColumnsUserProfileGet.FirstName),
                                        LastName = rdr.GetString(ColumnsUserProfileGet.LastName),
                                        Phone = rdr.GetString(ColumnsUserProfileGet.Phone),
                                        Mobile = rdr.GetString(ColumnsUserProfileGet.Mobile),
                                        LinkedIn = rdr.GetString(ColumnsUserProfileGet.LinkedIn),
                                        JobPosition = rdr.GetString(ColumnsUserProfileGet.JobPosition),
                                        ShowHelp = rdr.GetBoolean(ColumnsUserProfileGet.ShowHelp),
                                        Language = new Language()
                                        {
                                            Id = rdr.GetInt64(ColumnsUserProfileGet.Language),
                                            Name = rdr.GetString(ColumnsUserProfileGet.LastName),
                                            RightToLeft = false
                                        }
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
                }*/

                return new ReadOnlyCollection<UserProfile>(res);
            }
        }

        /// <summary>
        /// Gets or sets user profile identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets user identifier
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets job position of user
        /// </summary>
        public string JobPosition { get; set; }

        /// <summary>
        /// Gets or sets linkedIn profile
        /// </summary>
        public string LinkedIn { get; set; }

        /// <summary>
        /// Gets or sets user phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets user mobile number
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets user layout language
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user actives online help
        /// </summary>
        public bool ShowHelp { get; set; }

        /// <summary>
        /// Gets a JSON structure of user profile
        /// </summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""UserId"":{1}, ""Language"":{2}, ""ShowHelp"":{3}, ""Phone"":""{4}"", ""Mobile"":""{5}"", ""LinkedIn"":""{6}""}}",
                    this.Id,
                    this.UserId,
                    this.Language.Json,
                    ConstantValue.Value(this.ShowHelp),
                    ToolsJson.JsonCompliant(this.Phone),
                    ToolsJson.JsonCompliant(this.Mobile),
                    ToolsJson.JsonCompliant(this.LinkedIn));
            }
        }

        /// <summary>
        /// Gets a JSON list of user profiles
        /// </summary>
        /// <param name="usersProfiles">List of profiles</param>
        /// <returns>A JSON list of user profiles</returns>
        public static string JsonList(ReadOnlyCollection<UserProfile> usersProfiles)
        {
            if (usersProfiles == null)
            {
                return ConstantValue.EmptyJsonList;
            }

            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (UserProfile user in usersProfiles)
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
                res.Append(user.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Gets profile by user identifier
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Profile of user</returns>
        public static UserProfile GetByUserId(long userId, string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "UserProfile::GetUserById({0})", userId);
            UserProfile res = UserProfile.Empty;
            using (SqlCommand cmd = new SqlCommand("UserProfile_GetByUserId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    try
                    {
                        cmd.Connection = cnn;
                        cnn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = rdr.GetInt64(ColumnsUserProfileGet.Id);
                                res.UserId = rdr.GetInt64(ColumnsUserProfileGet.UserId);
                                res.Phone = rdr.GetString(ColumnsUserProfileGet.Phone);
                                res.Mobile = rdr.GetString(ColumnsUserProfileGet.Mobile);
                                res.LinkedIn = rdr.GetString(ColumnsUserProfileGet.LinkedIn);
                                res.JobPosition = rdr.GetString(ColumnsUserProfileGet.JobPosition);
                                res.ShowHelp = rdr.GetBoolean(ColumnsUserProfileGet.ShowHelp);
                                res.Language = new Language()
                                {
                                    Id = rdr.GetInt64(ColumnsUserProfileGet.Language),
                                    Name = rdr.GetString(ColumnsUserProfileGet.LastName),
                                    RightToLeft = false
                                };
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

            return res;
        }
    }
}