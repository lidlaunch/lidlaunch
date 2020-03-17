using System;
using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class WebsiteProduct 
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int DesignerId { get; set; }
        public string DesignerName { get; set; }
        public string ApplyMethod { get; set; }
        public DateTime AdddedDate { get; set; }
        public int ParentCount { get; set; }
    }
}