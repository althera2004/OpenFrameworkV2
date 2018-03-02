// --------------------------------
// <copyright file="CompanyMemberShip.cs" company="Sbrinna">
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
    using System.Text;
    using OpenFramework.DataAccess;
    using OpenFramework.ItemManager;

    public class CompanyMemberShip
    {
        public ApplicationUser Member { get; set; }

        public ItemBuilder Company { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public ApplicationUser ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Active { get; set; }

        public static CompanyMemberShip Empty(string multipleCompanyItem,string instanceName)
        {

            return new CompanyMemberShip()
            {
                Member = ApplicationUser.Empty,
                Company = ItemBuilder.Empty(multipleCompanyItem, false, instanceName),
                CreatedBy = ApplicationUser.Empty,
                ModifiedBy = ApplicationUser.Empty,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Active = false
            };
        }

        public static ReadOnlyCollection<CompanyMemberShip> GetAll(string instanceName,string multipleCompanyItem, string connectionString)
        {
            List<CompanyMemberShip> res = new List<CompanyMemberShip>();
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_GetAll"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
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
                               /* ApplicationUser member = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.MemberId), connectionString);
                                ItemBuilder company = DataPersistence.GetById(instanceName, multipleCompanyItem, rdr.GetInt64(ColumnsCompanyMembership.CompanyId), connectionString);
                                ApplicationUser createdBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.CreatedBy), connectionString);
                                ApplicationUser modifiedBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.ModifiedBy), connectionString);
                                res.Add(new CompanyMemberShip()
                                {
                                    Member = member,
                                    Company = company,
                                    CreatedBy = createdBy,
                                    CreatedOn = rdr.GetDateTime(ColumnsCompanyMembership.CreatedOn),
                                    ModifiedBy = modifiedBy,
                                    ModifiedOn = rdr.GetDateTime(ColumnsCompanyMembership.Modifiedon),
                                    Active = rdr.GetBoolean(ColumnsCompanyMembership.Active)
                                });*/
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

            return new ReadOnlyCollection<CompanyMemberShip>(res);
        }

        public static ReadOnlyCollection<CompanyMemberShip> GetActive(string instanceName, string multipleCompanyItem, string connectionString)
        {
            List<CompanyMemberShip> res = new List<CompanyMemberShip>();
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_GetActive"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
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
                               /* ApplicationUser member = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.MemberId), connectionString);
                                ItemBuilder company = DataPersistence.GetById(instanceName, multipleCompanyItem, rdr.GetInt64(ColumnsCompanyMembership.CompanyId), connectionString);
                                ApplicationUser createdBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.CreatedBy), connectionString);
                                ApplicationUser modifiedBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.ModifiedBy), connectionString);
                                res.Add(new CompanyMemberShip()
                                {
                                    Member = member,
                                    Company = company,
                                    CreatedBy = createdBy,
                                    CreatedOn = rdr.GetDateTime(ColumnsCompanyMembership.CreatedOn),
                                    ModifiedBy = modifiedBy,
                                    ModifiedOn = rdr.GetDateTime(ColumnsCompanyMembership.Modifiedon),
                                    Active = rdr.GetBoolean(ColumnsCompanyMembership.Active)
                                });*/
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

            return new ReadOnlyCollection<CompanyMemberShip>(res);
        }

        public static ReadOnlyCollection<CompanyMemberShip> GetByUserId(long userId, string instanceName,string multipleCompanyItem, string connectionString)
        {
            List<CompanyMemberShip> res = new List<CompanyMemberShip>();
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_GetByUserId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
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
                                /*ApplicationUser member = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.MemberId), connectionString);
                                ItemBuilder company = DataPersistence.GetById(instanceName, multipleCompanyItem, rdr.GetInt64(ColumnsCompanyMembership.CompanyId), connectionString);
                                ApplicationUser createdBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.CreatedBy), connectionString);
                                ApplicationUser modifiedBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.ModifiedBy), connectionString);
                                res.Add(new CompanyMemberShip()
                                {
                                    Member = member,
                                    Company = company,
                                    CreatedBy = createdBy,
                                    CreatedOn = rdr.GetDateTime(ColumnsCompanyMembership.CreatedOn),
                                    ModifiedBy = modifiedBy,
                                    ModifiedOn = rdr.GetDateTime(ColumnsCompanyMembership.Modifiedon),
                                    Active = rdr.GetBoolean(ColumnsCompanyMembership.Active)
                                });*/
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

            return new ReadOnlyCollection<CompanyMemberShip>(res);
        }

        public static ReadOnlyCollection<CompanyMemberShip> GetByCompanyId(long companyId, string instanceName, string multipleCompanyItem,string connectionString)
        {
            List<CompanyMemberShip> res = new List<CompanyMemberShip>();
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_GetByCompanyId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
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
                                /*ApplicationUser member = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.MemberId), connectionString);
                                ItemBuilder company = DataPersistence.GetById(instanceName, multipleCompanyItem, rdr.GetInt64(ColumnsCompanyMembership.CompanyId), connectionString);
                                ApplicationUser createdBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.CreatedBy), connectionString);
                                ApplicationUser modifiedBy = SecurityPersistence.UserById(instanceName, rdr.GetInt64(ColumnsCompanyMembership.ModifiedBy), connectionString);
                                res.Add(new CompanyMemberShip()
                                {
                                    Member = member,
                                    Company = company,
                                    CreatedBy = createdBy,
                                    CreatedOn = rdr.GetDateTime(ColumnsCompanyMembership.CreatedOn),
                                    ModifiedBy = modifiedBy,
                                    ModifiedOn = rdr.GetDateTime(ColumnsCompanyMembership.Modifiedon),
                                    Active = rdr.GetBoolean(ColumnsCompanyMembership.Active)
                                });*/
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

            return new ReadOnlyCollection<CompanyMemberShip>(res);
        }

        public static ActionResult Activate(long userId, long companyId, long applicationUserId, string connectionString)
        {
            var res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_Activate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        //SecurityPersistence.LoadCompanyMembership(instanceName);
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

        public static ActionResult Inactivate(long userId, long companyId, long applicationUserId, string connectionString)
        {
            var res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_Inactivate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        // SecurityPersistence.LoadCompanyMembership(customerConfig.Name);
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
        
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Member"": {0},
                        ""Company"": {1},
                        ""CreatedBy"": {2},
                        ""CreatedOn"": {3:dd/MM/yyyy},
                        ""ModifiedBy"": {4},
                        ""ModifiedOn"": {5},
                        ""Active"": {6}
                    }}",
                       this.Member.JsonKeyValue,
                       this.Company.JsonKeyValue,
                       this.CreatedBy,
                       this.CreatedOn,
                       this.ModifiedBy,
                       this.ModifiedOn,
                       this.Active ? ConstantValue.True : ConstantValue.False);
            }
        }

        public string JsonList(ReadOnlyCollection<CompanyMemberShip> list)
        {
            if(list == null){
                return "null";
            }

            if (list.Count == 0)
            {
                return "[]";
            }            

            bool first = true;
            StringBuilder res = new StringBuilder("[");
            foreach (CompanyMemberShip member in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(";");
                }

                res.Append(member.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public ActionResult Save(long applicationUserId, string connectionString)
        {
            var res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Core_CompanyMemberShip_Save"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Member.Id));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Company.Id));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        //// SecurityPersistence.LoadCompanyMembership(customerConfig.Name);
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
