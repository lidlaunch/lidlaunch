using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class Category 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }        

    }
}