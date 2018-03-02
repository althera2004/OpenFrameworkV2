namespace OpenFramework.Customer
{
    /// <summary>Implements customer framework class</summary>
    public sealed class CustomerFramework
    {
        /// <summary>Gets an empty instance of customer framework</summary>
        public static CustomerFramework Empty
        {
            get
            {
                return new CustomerFramework()
                {
                    Id = 0,
                    Name = string.Empty,
                    Config = Config.Empty
                };
            }
        }

        /// <summary>Gets or sets the customer framework identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the customer framework name</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the customer framework configuration</summary>
        public Config Config { get; set; }

        /// <summary>Loads customer framework configuration</summary>
        public void LoadConfig()
        {
            this.Config = Config.Load(this.Name);
            this.Id = this.Config.Id;
            this.Name = this.Config.Name;
        }

        public static CustomerFramework Load(string instanceName)
        {
            CustomerFramework res = new CustomerFramework();
            res.Name = instanceName;
            res.LoadConfig();
            return res;
        }
    }
}