using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class PaypalAuthResponse
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public string expires_in { get; set; }
    }
    
    public class AccessToken
    {
        public string client_token { get; set; }
        public string expires_in { get; set; }
    }
}