// --------------------------------
// <copyright file="ExecuteQuery.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using OpenFramework.Core;
    using OpenFramework.Customer;

    public class ExecuteQuery
    {
        private List<SqlParameter> parameters;
        public string QueryText { get; set; }
        public string ConnectionString { get; set; }

        public void AddParameter(string name, object value)
        {
            if (this.parameters == null)
            {
                this.parameters = new List<SqlParameter>();
            }

            switch (value.GetType().ToString().ToUpperInvariant())
            {
                case "INT":
                    int valueInteger = 0;
                    int.TryParse(value as string, out valueInteger);
                    this.parameters.Add(DataParameter.Input(name, valueInteger));
                    break;
                case "LONG":
                    int valueLong = 0;
                    int.TryParse(value as string, out valueInteger);
                    this.parameters.Add(DataParameter.Input(name, valueLong));
                    break;
                case "FLOAT":
                    int valueFloat = 0;
                    int.TryParse(value as string, out valueInteger);
                    this.parameters.Add(DataParameter.Input(name, valueFloat));
                    break;
                case "DECIMAL":
                    int valueDecimal = 0;
                    int.TryParse(value as string, out valueInteger);
                    this.parameters.Add(DataParameter.Input(name, valueDecimal));
                    break;
                case "BOOLEAN":
                    bool valueBoolean = false;
                    int.TryParse(value as string, out valueInteger);
                    this.parameters.Add(DataParameter.Input(name, valueBoolean));
                    break;
                case "DATETIME":
                    DateTime valueDateTime = DateTime.MinValue;
                    int.TryParse(value as string, out valueInteger);
                    this.parameters.Add(DataParameter.Input(name, valueDateTime));
                    break;
                default:
                    this.parameters.Add(DataParameter.Input(name, value as string));
                    break;
            }

        }

        public void AddParameter(string name, string value, int length)
        {
            if (this.parameters == null)
            {
                this.parameters = new List<SqlParameter>();
            }

            this.parameters.Add(DataParameter.Input(name, value, length));
        }

        public ActionResult ExecuteProcedure
        {
            get
            {
                return LaunchQuery(CommandType.StoredProcedure);
            }
        }

        public ActionResult ExecuteCommand
        {
            get
            {
                return LaunchQuery(CommandType.Text);
            }
        }

        private ActionResult LaunchQuery(CommandType commandType)
        {

            var res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand(this.QueryText))
            {
                cmd.CommandType = commandType;
                using (SqlConnection cnn = new SqlConnection(this.ConnectionString))
                {
                    if (this.parameters != null)
                    {
                        foreach (SqlParameter parameter in this.parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        cmd.Connection = cnn;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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