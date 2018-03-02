// --------------------------------
// <copyright file="InstanceMode.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Security
{
    using OpenFramework.Customer;
    using System;
    using System.Configuration;

    public static class InstanceMode
    {
        public static Instance Product
        {
            get
            {
                var instanct = InstanceMode.Instance;
                return new Instance()
                {
                    Active = true,
                    CreatedBy = ApplicationUser.OpenFramework,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.OpenFramework,
                    ModifiedOn = DateTime.Now,
                    DefaultLanguage = new Core.Language()
                    {
                        Active = true,
                        ISO = instanct.Config.DefaultLanguage,
                        LocaleName = string.Empty
                    },
                    Name = instanct.Name,
                    SubscriptionType = 4,
                    Id = instanct.Id,
                    Key = string.Empty,
                    MultipleCore = false,
                    UsersLimit = 1000
                };
            }
        }

        public static string Connection
        {
            get
            {
                string cns = ConfigurationManager.ConnectionStrings["Security"].ConnectionString;
                if (ConfigurationManager.AppSettings["OnPremiseInstance"] != null)
                {
                    var instance = new CustomerFramework() { Name = ConfigurationManager.AppSettings["OnPremiseInstance"] };
                    instance.LoadConfig();
                    cns = instance.Config.ConnectionString;
                }

                return cns;
            }
        }

        public static CustomerFramework Instance
        {
            get
            {
                var res = CustomerFramework.Empty;
                if (ConfigurationManager.AppSettings["OnPremiseInstance"] != null)
                {
                    res = new CustomerFramework() { Name = ConfigurationManager.AppSettings["OnPremiseInstance"] };
                    res.LoadConfig();
                }

                return res;
            }
        }

        public static bool OnPremise
        {
            get
            {
                if (ConfigurationManager.AppSettings["OnPremiseInstance"] != null)
                {
                    return true;

                }

                return false;
            }
        }
    }
}