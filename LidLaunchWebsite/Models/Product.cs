using System;
using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DesignId   { get; set; }
        public int DesignerId { get; set; }
        public DateTime DateAdded { get; set; }
        public Design Design { get; set; }
        public int Quantity { get; set; }
        public string CartGuid { get; set; }
        public string Size { get; set; }
        public int CategoryId { get; set; }
        public bool Private { get; set; }
        public bool Hidden { get; set; }
        public int TypeId { get; set; }
        public int ColorId { get; set; }
        public string TypeText { get; set; }
        public int ParentProductId { get; set; }
        public int ParentCount { get; set; }
        public string ApplyMethod { get; set; }
    }
    public class ProductPageProduct
    {
        public Product Product { get; set; }
        public Designer Designer { get; set; }
        public List<HatType> lstHatTypes { get; set; }
        public List<Product> lstChildProducts { get; set; }
    }
    public class UpdateProduct
    {
        public Product Product { get; set; }
        public List<Category> lstCategories { get; set; }
        public List<Product> lstParentProducts { get; set; }
    }
}