// --------------------------------
// <copyright file="SqlStream.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.DataAccess
{
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using OpenFramework;
    using OpenFramework.ItemManager;
    using System.Collections.Generic;

    public static class SqlStream
    {
        public static string GetSqlStream(string storedName, ReadOnlyCollection<SqlParameter> parameters, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(storedName))
            {
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        foreach (SqlParameter p in parameters)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }
                }

                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string GetFKStream(ItemBuilder item, Dictionary<string,string> parameters, string connectionString)
        {
            if (item.Definition.FKList == null)
            {
                return GetSqlStreamNoParams("Item_FK_" + item.ItemName, connectionString);
            }

            return GetSqlQueryStreamNoParams(ItemTools.QueryFKList(item, parameters), connectionString);
        }

        public static string GetSqlQueryStreamNoParams(string query, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string GetSqlStreamNoParams(string storedName, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(storedName))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string GetSqlStream(string storedName, bool core, string scopeViewList, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(storedName))
            {
                if (core)
                {
                    cmd.Parameters.Add(DataParameter.InputNull("@AdjudicacionesId"));
                }
                else
                {
                    cmd.Parameters.Add(DataParameter.Input("@AdjudicacionesId", scopeViewList));
                }

                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string GetbyStored(string storedName, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(storedName))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLToJSON(cmd);
                }
            }

            return res;
        }

        public static string GetByQuerySimple(string query, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLStreamSimple(cmd);
                }
            }

            // Si no existe se devuelve un item vacío con id -1
            if (string.IsNullOrEmpty(res))
            {
                res = "{Id:-1}";
            }

            return res;
        }

        public static string GetByQuery(string query, string connectionString)
        {
            string res = ConstantValue.EmptyJsonList;
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = Basics.SQLStreamList(cmd);
                }
            }

            return res;
        }
    }
}
