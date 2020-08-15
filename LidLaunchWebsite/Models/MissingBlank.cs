using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class MissingBlank
    {
        public int Id { get; set; }
        public string MissingBlankName { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}