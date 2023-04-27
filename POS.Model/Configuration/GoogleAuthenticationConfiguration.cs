using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model.Configuration
{
    public class GoogleAuthenticationConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string LoginPath { get; set; }
    }
}
