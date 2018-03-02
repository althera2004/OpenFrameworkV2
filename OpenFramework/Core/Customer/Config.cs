namespace OpenFramework.Customer
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Implements configuration of framework customer
    /// </summary>
    public sealed class Config
    {
        /// <summary>Mail configuration</summary>
        [JsonProperty("Mail")]
        private MailConfiguration mail;

        /// <summary>Session timeout</summary>
        [JsonProperty("SessionTimeout")]
        private string sessionTimeout;

        /// <summary>Default language of instance</summary>
        [JsonProperty("DefaultLanguage")]
        private string defaultLanguage;

        /// <summary>Action of delete</summary>
        [JsonProperty("DeleteAction")]
        private string deleteAction;

        /// <summary>Action of save</summary>
        [JsonProperty("SaveAction")]
        private string saveAction;

        /// <summary>Indicates if instance has multiple company configuration</summary>
        [JsonProperty("MultiCompany")]
        private string multiCompany;

        [JsonProperty("ListNnumRows")]
        private int listNumRows;

        /// <summary>Gets an empty customer config</summary>
        [JsonIgnore]
        public static Config Empty
        {
            get
            {
                return new Config()
                {
                    Id = -1,
                    Name = string.Empty,
                    ConnectionString = string.Empty,
                    mail = MailConfiguration.Empty,
                    GoogleAPIKey = string.Empty
                };
            }
        }

        /// <summary>Gets or sets key of Google maps API</summary>
        [JsonProperty("GoogleAPIKey")]
        public string GoogleAPIKey { get; set; }

        /// <summary>Gets or sets the connection string</summary>
        [JsonProperty("ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>Gets or sets the item that decides company membership</summary>
        [JsonProperty("MultiCompanyItem")]
        public string MultipleCompanyItem { get; set; }

        /// <summary>Gets or sets the customer identifier</summary>
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonIgnore]
        public int ListNumRows
        {
            get
            {
                if (this.listNumRows == 0)
                {
                    return 10;
                }

                return this.listNumRows;
            }
        }

        /// <summary>Gets the action in database for delete</summary>
        [JsonIgnore]
        public DeleteAction DeleteAction
        {
            get
            {
                if (string.IsNullOrEmpty(this.deleteAction))
                {
                    return DeleteAction.Inactive;
                }

                switch (this.deleteAction.ToUpperInvariant())
                {
                    case "DELETE":
                        return DeleteAction.Delete;
                    case "INACTIVE":
                    default:
                        return DeleteAction.Inactive;
                }
            }
        }

        /// <summary>Gets behavior for save button</summary>
        [JsonIgnore]
        public SaveAction SaveAction
        {
            get
            {
                if (string.IsNullOrEmpty(this.saveAction))
                {
                    return SaveAction.SaveAndClose;
                }

                switch (this.saveAction.ToUpperInvariant())
                {
                    case "SAVEONLY":
                        return SaveAction.SaveOnly;
                    case "BOTH":
                        return SaveAction.Both;
                    case "SAVEANDNEW":
                        return SaveAction.SaveAndNew;
                    case "SAVEANDCLOSE":
                    default:
                        return SaveAction.SaveAndClose;
                }
            }
        }

        /// <summary>Gets mail configuration of instance</summary>
        [JsonIgnore]
        public MailConfiguration Mail
        {
            get
            {
                if (this.mail == null)
                {
                    return MailConfiguration.Empty;
                }

                return this.mail;
            }
        }

        /// <summary>Gets a value indicating whether instance has multiple company configuration</summary>
        [JsonIgnore]
        public bool MultiCompany
        {
            get
            {
                if (string.IsNullOrEmpty(this.multiCompany))
                {
                    return false;
                }

                if (this.multiCompany.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets or sets the customer name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets a test result</summary>
        public ActionResult TestConfig
        {
            get
            {
                ActionResult res = ActionResult.NoAction;
                using (SqlConnection cnn = new SqlConnection(this.ConnectionString))
                {
                    try
                    {
                        cnn.Open();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        // Could not convert.  Pass back default value...
                        ExceptionManager.Trace(ex as Exception, "Connect DataBase --> " + this.ConnectionString);
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        // Could not convert.  Pass back default value...
                        ExceptionManager.Trace(ex as Exception, "Connect DataBase --> " + this.ConnectionString);
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
                        // Could not convert.  Pass back default value...
                        ExceptionManager.Trace(ex as Exception, "Connect DataBase --> " + this.ConnectionString);
                        res.SetFail(ex);
                    }
                    catch (ArgumentNullException ex)
                    {
                        // Could not convert.  Pass back default value...
                        ExceptionManager.Trace(ex as Exception, "Connect DataBase --> " + this.ConnectionString);
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        // Could not convert.  Pass back default value...
                        ExceptionManager.Trace(ex as Exception, "Connect DataBase --> " + this.ConnectionString);
                        res.SetFail(ex);
                    }
                }

                return res;
            }
        }

        /// <summary>Gets the session timeout</summary>
        [JsonIgnore]
        public int SessionTimeout
        {
            get
            {
                if (string.IsNullOrEmpty(this.sessionTimeout))
                {
                    return 900;
                }

                int? extractTime = Basics.ParseTime(this.sessionTimeout);
                return extractTime.HasValue ? extractTime.Value : 900;
            }
        }

        /// <summary>Gets the default language</summary>
        [JsonIgnore]
        public string DefaultLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(this.defaultLanguage))
                {
                    return "es";
                }

                return this.defaultLanguage;
            }
        }

        /// <summary>
        /// Load customer framework configuration
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <returns>Customer framework configuration</returns>
        public static Config Load(string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                return Config.Empty;
            }

            string source = string.Format(CultureInfo.InvariantCulture, @"OpenFramework.Core.Config.Load(""{0}"")", customerName);
            Config res = Config.Empty;
            string path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\", path);
            }

            path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}Instances\{1}\Customer.cfx", path, customerName);

            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader input = new StreamReader(path))
                    {
                        var json = input.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                        dynamic data = serializer.Deserialize(json, typeof(object));
                        res = new Config()
                        {
                            Id = data["Id"],
                            ConnectionString = data["ConnectionString"],
                            Name = data["Name"],
                            deleteAction = data["DeleteAction"],
                            sessionTimeout = data["SessionTimeout"],
                            multiCompany = data["MultiCompany"],
                            MultipleCompanyItem = data["MultiCompanyItem"],
                            defaultLanguage = data["DefaultLanguage"],
                            GoogleAPIKey = data["GoogleAPIKey"],
                            mail = MailConfiguration.Empty
                        };

                        if (data["ListNumRows"] != null)
                        {
                            res.listNumRows = data["ListNumRows"];
                        }

                        if (data["Mail"] != null)
                        {
                            res.Mail.Mode = data["Mail"]["Mode"];
                            res.Mail.Server = data["Mail"]["Server"];
                            res.Mail.User = data["Mail"]["User"];
                            res.Mail.Password = data["Mail"]["Password"];
                            res.Mail.UserName = data["Mail"]["UserName"];
                            res.Mail.MailSender = data["Mail"]["MailSender"];
                            res.Mail.Port = data["Mail"]["Port"];
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                ExceptionManager.LogException(ex as Exception, source);
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.LogException(ex as Exception, source);
            }
            catch (JsonSerializationException ex)
            {
                ExceptionManager.LogException(ex as Exception, source);
            }
            catch (JsonException ex)
            {
                ExceptionManager.LogException(ex as Exception, source);
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.LogException(ex as Exception, source);
            }

            return res;
        }
    }
}