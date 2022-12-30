using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Properties_to_Rent_Client
{
    internal class AuthConfig
    {
        public string Instance { get; set; }
        public string TenandId { get; set; }
        public string ClientId { get; set; }
        public string Authority { 
            get 
            {
                return string.Format(CultureInfo.InvariantCulture, Instance, TenandId);
            }
        }
        public string ClientSecret { get; set; }
        public string BaseAddress { get; set; }
        public string ResourceId { get; set; }


        public static AuthConfig ReadJsonFromFile(string path)
        {
            IConfiguration configuration;

            var builder = new ConfigurationBuilder()
                //GetCurrentDirectory()
                .SetBasePath(Directory.GetDirectoryRoot("Properties_to_Rent_Client"))
                .AddJsonFile(path);

            configuration = builder.Build();
            return configuration.Get<AuthConfig>();
        }


    }
}
