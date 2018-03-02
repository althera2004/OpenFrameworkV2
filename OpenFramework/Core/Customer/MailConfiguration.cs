namespace OpenFramework.Customer
{
    using Newtonsoft.Json;

    /// <summary>Customer mail configuration</summary>
    public sealed class MailConfiguration
    {
        /// <summary>Gets an empty instance of MailConfiguration class</summary>
        [JsonIgnore]
        public static MailConfiguration Empty
        {
            get
            {
                return new MailConfiguration()
                {
                    Mode = string.Empty,
                    Server = string.Empty,
                    User = string.Empty,
                    Password = string.Empty,
                    UserName = string.Empty,
                    MailSender = string.Empty,
                    Port = 0
                };
            }
        }

        /// <summary>Gets or sets mode</summary>
        [JsonProperty("Mode")]
        public string Mode { get; set; }

        /// <summary>Gets or sets mail server</summary>
        [JsonProperty("Server")]
        public string Server { get; set; }

        /// <summary>Gets or sets user account</summary>
        [JsonProperty("User")]
        public string User { get; set; }

        /// <summary>Gets or sets password account</summary>
        [JsonProperty("Password")]
        public string Password { get; set; }

        /// <summary>Gets or sets user name</summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>Gets or sets mail address</summary>
        [JsonProperty("MailSender")]
        public string MailSender { get; set; }

        /// <summary>Gets or sets mail server port</summary>
        [JsonProperty("Port")]
        public int Port { get; set; }
    }
}