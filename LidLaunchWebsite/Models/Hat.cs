using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class HatCreationHats {
        public List<HatType> lstHatTypes { get; set; }
    }
    public class HatType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePreview { get; set; }
        public string ProductImage { get; set; }
        public string ProductIdentifier { get; set; }
        public int ManufacturerId { get; set; }
        public decimal BasePrice { get; set; }
        public List<HatColor> lstColors { get; set; }
    }
    public class CompleteModel
    {
        public List<HatType> lstHatTypes { get; set; }        
        public List<Category> lstCategories { get; set; }
        public List<Product> lstParentProducts { get; set; }
    }
}