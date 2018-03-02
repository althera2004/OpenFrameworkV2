// --------------------------------
// <copyright file="Command.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.DataAccess
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Implementation of Command class.</summary>
    public static class Command
    {
        /// <summary>Gets a SQL command that calls a stored procedure</summary>
        /// <param name="storedName">Stored procedure name</param>
        /// <param name="connectionString">Connection string of command</param>
        /// <returns>A SQL command</returns>
        public static SqlCommand Stored(string storedName, string connectionString)
        {
            SqlCommand res = new SqlCommand();
            if (!string.IsNullOrEmpty(storedName))
            {
                res.CommandType = CommandType.StoredProcedure;
                res.CommandText = storedName;
                res.Connection = new SqlConnection(connectionString);
            }

            return res;
        }
    }
}