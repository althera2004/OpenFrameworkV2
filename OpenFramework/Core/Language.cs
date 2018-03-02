namespace OpenFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using OpenFramework.Core.Bindings;
    using OpenFramework.Security;

    /// <summary>
    /// Implements Language class
    /// </summary>
    public sealed class Language
    {
        /// <summary>
        /// Gets a simple empty language object
        /// </summary>
        public static Language SimpleEmpty
        {
            get
            {
                return new Language()
                {
                    Id = 0,
                    Name = string.Empty,
                    LocaleName = string.Empty,
                    ISO = string.Empty,
                    Active = false
                };
            }
        }

        /// <summary>
        /// Gets an empty language object
        /// </summary>
        public static Language Empty
        {
            get
            {
                return new Language()
                {
                    Id = 0,
                    Name = string.Empty,
                    LocaleName = string.Empty,
                    ISO = string.Empty,
                    RightToLeft = false,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        /// <summary>
        /// Gets all available languages
        /// </summary>
        public static ReadOnlyCollection<Language> All
        {
            get
            {
                string source = "Language::All";
                List<Language> res = new List<Language>();
                using (SqlCommand cmd = new SqlCommand("Core_Language_GetAll"))
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
                                    res.Add(new Language()
                                    {
                                        Id = rdr.GetInt64(ColumnsLanguageGet.Id),
                                        Name = rdr.GetString(ColumnsLanguageGet.Name),
                                        LocaleName = rdr.GetString(ColumnsLanguageGet.LocaleName),
                                        ISO = rdr.GetString(ColumnsLanguageGet.Iso),
                                        RightToLeft = rdr.GetBoolean(ColumnsLanguageGet.RightToLeft),
                                        CreatedBy = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt64(ColumnsLanguageGet.CreatedBy),
                                            FirstName = rdr.GetString(ColumnsLanguageGet.CreatedByFirstName),
                                            LastName = rdr.GetString(ColumnsLanguageGet.CreatedByLastName)
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsLanguageGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt64(ColumnsLanguageGet.ModifiedBy),
                                            FirstName = rdr.GetString(ColumnsLanguageGet.ModifiedByFirstName),
                                            LastName = rdr.GetString(ColumnsLanguageGet.ModifiedByLastName)
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsLanguageGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsLanguageGet.Active)
                                    });
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.LogException(ex, source);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.LogException(ex, source);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.LogException(ex, source);
                        }
                    }
                }

                return new ReadOnlyCollection<Language>(res);
            }
        }

        /// <summary> Gets or sets language identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets language name</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets locale language name</summary>
        public string LocaleName { get; set; }

        /// <summary>Gets or sets language ISO code</summary>
        public string ISO { get; set; }

        /// <summary>Gets or sets a value indicating whether direction of writing language</summary>
        public bool RightToLeft { get; set; }

        /// <summary>Gets or sets a value indicating whether language is active or not</summary>
        public bool Active { get; set; }

        /// <summary>Gets user that created language</summary>
        public ApplicationUser CreatedBy { get; private set; }

        /// <summary>
        /// Gets user that make last modification of language
        /// </summary>
        public ApplicationUser ModifiedBy { get; private set; }

        /// <summary>Gets date of creation</summary>
        public DateTime CreatedOn { get; private set; }

        /// <summary>Gets date of last modification</summary>
        public DateTime ModifiedOn { get; private set; }

        /// <summary>
        /// Gets basic structure JSON of language data
        /// </summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Value"":""{1}""}}", this.Id, this.Name);
            }
        }

        /// <summary>
        /// Gets structure JSON of language data
        /// </summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0}, ""Name"":""{1}"", ""LocaleName"":""{2}"", ""ISO"":""{3}"", ""RightToLeft"":{4},  ""Active"": {5}}}",
                    this.Id,
                    this.Name,
                    this.LocaleName,
                    this.ISO,
                    this.RightToLeft ? ConstantValue.True : ConstantValue.False,
                    this.Active ? ConstantValue.True : ConstantValue.False);
            }
        }
    }
}