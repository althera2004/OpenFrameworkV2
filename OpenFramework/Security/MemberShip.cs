// --------------------------------
// <copyright file="MemberShip.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// Implements security groups membership
    /// </summary>
    public sealed class Membership
    {
        /// <summary>
        /// Gets all memberships
        /// </summary>
        public static ReadOnlyCollection<Membership> All(string instanceName, string connectionString)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "MemeberShip::All==>{0}", instanceName);
            List<Membership> res = new List<Membership>();
            using (SqlCommand cmd = new SqlCommand("Core_Group_GetMembers"))
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
                                res.Add(new Membership()
                                {
                                    GroupId = rdr.GetInt64(0),
                                    UserId = rdr.GetInt64(1)
                                });
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

            return new ReadOnlyCollection<Membership>(res);
        }

        /// <summary>Gets or sets group identifier</summary>
        public long GroupId { get; set; }

        /// <summary>Gets or sets user identifier</summary>
        public long UserId { get; set; }
    }
}