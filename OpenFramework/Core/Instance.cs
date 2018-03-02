namespace OpenFramework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using OpenFramework.Core.Bindings;
    using OpenFramework.DataAccess;
    using OpenFramework.Security;
    using System.IO;
    using OpenFramework.Core;

    /// <summary>
    /// Implements OpenFramework instance
    /// </summary>
    public sealed class Instance
    {
        private List<Language> languages;
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public Language DefaultLanguage { get; set; }
        public int SubscriptionType { get; set; }
        public int UsersLimit { get; set; }
        public DateTime SubscriptionStart { get; set; }
        public DateTime SubscriptionEnd { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool MultipleCore { get; set; }
        public bool Active { get; set; }

        public static string Path(string instanceName)
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}CustomersFramework\{1}\",
                    path, instanceName);
            }
            else
            {
                path = string.Format(
                       CultureInfo.InvariantCulture,
                       @"\{0}CustomersFramework\{1}\",
                       path, instanceName);
            }

            return path;
        }

        public static bool Exists(string instanceName)
        {
            bool res = false;
            string path = Path(instanceName);

            if (File.Exists(path + "customer.cfx"))
            {
                res = true;
            }

            return res;
        }

        public ReadOnlyCollection<Language> Languages
        {
            get
            {
                if (this.languages == null)
                {
                    this.languages = new List<Language>();
                }

                return new ReadOnlyCollection<Language>(this.languages);
            }
        }

        public static Instance Empty
        {
            get
            {
                return new Instance()
                {
                    Id = 0,
                    Name = string.Empty,
                    DefaultLanguage = Language.Empty,
                    SubscriptionType = 0,
                    SubscriptionStart = DateTime.Now,
                    SubscriptionEnd = DateTime.Now,
                    UsersLimit = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    MultipleCore = false,
                    Active = false,
                    languages = new List<Language>()
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""Key"":""{1}"",
                        ""Name"":""{2}"",
                        ""DefaultLanguage"":{3},
                        ""SubcriptionType"":{4},
                        ""UsersLimit"":{5},
                        ""SubscriptionStart"":""{6:yyyyMMdd}"",
                        ""SubscriptionEnd"":""{7:yyyyMMdd}"",
                        ""CreatedBy"":{8},
                        ""CreatedOn"":""{9:yyyyMMdd}"",
                        ""ModifiedBy"":{10},
                        ""ModifiedOn"":{11:yyyyMMdd}"",
                        ""MultiCore"":{12},
                        ""Active"":{13}
                    }}",
                    this.Id,
                    this.Key,
                    ToolsJson.JsonCompliant(this.Name),
                    this.DefaultLanguage.Json,
                    this.SubscriptionType,
                    this.UsersLimit,
                    this.SubscriptionStart,
                    this.SubscriptionEnd,
                    this.CreatedBy,
                    this.CreatedOn,
                    this.ModifiedBy,
                    this.ModifiedOn,
                    ConstantValue.Value(this.MultipleCore),
                    ConstantValue.Value(this.Active));
            }
        }

        public static string ListJson(ReadOnlyCollection<Instance> instances)
        {
            if (instances == null)
            {
                return ConstantValue.EmptyJsonList;
            }

            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Instance instance in instances)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(instance.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static Instance ById(long instanceId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Instance::ById({0})", instanceId);
            Instance res = Instance.Empty;
            using (SqlCommand cmd = new SqlCommand("Instance_GetById"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@InstanceId", instanceId));
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = rdr.GetInt64(ColumnsInstanceGet.Id);
                                res.Key = rdr.GetString(ColumnsInstanceGet.Key);
                                res.Name = rdr.GetString(ColumnsInstanceGet.Name);
                                res.SubscriptionType = rdr.GetInt32(ColumnsInstanceGet.SubscriptionType);
                                res.SubscriptionStart = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionStart);
                                res.SubscriptionEnd = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionEnd);
                                res.UsersLimit = rdr.GetInt32(ColumnsInstanceGet.UsersLimit);
                                res.DefaultLanguage = new Language()
                                {
                                    Id = rdr.GetInt64(ColumnsInstanceGet.DefaultLanguage)
                                };
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsInstanceGet.CreatedBy),
                                    FirstName = rdr.GetString(ColumnsInstanceGet.CreatedByFirstName),
                                    LastName = rdr.GetString(ColumnsInstanceGet.CreatedByLastName)

                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsInstanceGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsInstanceGet.ModifiedBy),
                                    FirstName = rdr.GetString(ColumnsInstanceGet.ModifiedByFirstName),
                                    LastName = rdr.GetString(ColumnsInstanceGet.ModifiedByLastName)

                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsInstanceGet.ModifiedOn);
                                res.MultipleCore = rdr.GetBoolean(ColumnsInstanceGet.MultipleCore);
                                res.Active = rdr.GetBoolean(ColumnsInstanceGet.Active);
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

        public static ReadOnlyCollection<Instance> All
        {
            get
            {
                string source = string.Format(CultureInfo.InvariantCulture, "Instance::All");
                List<Instance> res = new List<Instance>();
                using (SqlCommand cmd = new SqlCommand("Instance_GetAll"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    Instance newInstance = new Instance()
                                    {
                                        Id = rdr.GetInt64(ColumnsInstanceGet.Id),
                                        Key = rdr.GetString(ColumnsInstanceGet.Key),
                                        Name = rdr.GetString(ColumnsInstanceGet.Name),
                                        SubscriptionType = rdr.GetInt32(ColumnsInstanceGet.SubscriptionType),
                                        SubscriptionStart = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionStart),
                                        SubscriptionEnd = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionEnd),
                                        UsersLimit = rdr.GetInt32(ColumnsInstanceGet.UsersLimit),
                                        DefaultLanguage = LanguagePersistence.LanguageById(rdr.GetInt64(ColumnsInstanceGet.DefaultLanguage)),
                                        CreatedBy = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt64(ColumnsInstanceGet.CreatedBy),
                                            FirstName = rdr.GetString(ColumnsInstanceGet.CreatedByFirstName),
                                            LastName = rdr.GetString(ColumnsInstanceGet.CreatedByLastName)
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsInstanceGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt64(ColumnsInstanceGet.ModifiedBy),
                                            FirstName = rdr.GetString(ColumnsInstanceGet.ModifiedByFirstName),
                                            LastName = rdr.GetString(ColumnsInstanceGet.ModifiedByLastName)
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsInstanceGet.ModifiedOn),
                                        MultipleCore = rdr.GetBoolean(ColumnsInstanceGet.MultipleCore),
                                        Active = rdr.GetBoolean(ColumnsInstanceGet.Active),
                                    };

                                    res.Add(newInstance);
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

                    return new ReadOnlyCollection<Instance>(res);
                }
            }
        }

        public void ObtainInstanceLanguages()
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Instance::ObtainInstanceLanguages({0})", this.Id);
            this.languages = new List<Language>();
            using (SqlCommand cmd = new SqlCommand("Instance_GetLanguages"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@InstanceId", this.Id));
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                this.languages.Add(LanguagePersistence.LanguageById(rdr.GetInt64(1)));
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
    }
}
