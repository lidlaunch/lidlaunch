using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LidLaunchWebsite.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index(int id)
        {
            ProductData productData = new ProductData();

            Product product = productData.GetProductForProductPage(Convert.ToInt32(id));
            ProductPageProduct productPageProduct = new ProductPageProduct();
            productPageProduct.Product = product;
            Designer designer = new Designer();
            DesignerData designerData = new DesignerData();
            List<HatType> lstHatType = new List<HatType>();
            List<Product> lstChildProducts = new List<Product>();
            lstHatType = productData.GetProductHatTypes(Convert.ToInt32(id));

            product.TypeId = lstHatType.FirstOrDefault().Id;
            product.TypeText = lstHatType.FirstOrDefault().Name;
            product.ColorId = lstHatType.FirstOrDefault().lstColors.FirstOrDefault().colorId;
            product.Design = new Design();
            product.Design.PreviewImage = lstHatType.FirstOrDefault().ProductImage;

            if (product.ParentProductId == 0)
            {
                lstChildProducts = productData.GetChildHatList(product.Id);
            }
            else
            {
                lstChildProducts = productData.GetChildHatList(product.ParentProductId);
                //Product parentProduct = productData.GetProduct(product.ParentProductId);
                //lstChildProducts.Add(parentProduct);
            }
            
            productPageProduct.lstHatTypes = lstHatType;
            productPageProduct.lstChildProducts = lstChildProducts;
            designer = designerData.GetDesignerByDesignerId(product.DesignerId);
            productPageProduct.Designer = designer;
            if (product.Hidden)
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                return View(productPageProduct);
            }
            
        }
    }
}