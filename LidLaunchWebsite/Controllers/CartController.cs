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
    public class CartController : Controller
    {
        public ActionResult Cart()
        {
            Cart cart = new Cart();
            List<Product> lstProducts;

            if (Session["Cart"] != null)
            {
                cart = (Cart)Session["Cart"];
                lstProducts = cart.lstProducts;
            }
            else
            {
                lstProducts = new List<Product>();
            }
            cart.lstProducts = lstProducts;

            //check if this product is setup for free shipping
            var productHasFreeShipping = false;

            OrderData orderData = new OrderData();

            cart.Total = 0.00M;

            foreach (Product prod in cart.lstProducts)
            {
                if (orderData.CheckProductHasFreeShipping(prod.Id))
                {
                    productHasFreeShipping = true;
                }
                if (prod.ApplyMethod.ToLower() == "embroidery")
                {
                    cart.Total += prod.Quantity * 19.99M;
                }
                if (prod.ApplyMethod.ToLower() == "leatherpatch")
                {
                    cart.Total += prod.Quantity * 29.99M;
                }
            }

            //add shipping
            if (!productHasFreeShipping)
            {
                cart.TotalWithShipping = cart.Total + 5;
                cart.Shipping = 5;
            }
            else
            {
                cart.TotalWithShipping = cart.Total;
                cart.Shipping = 0;
            }

            return View(cart);
        }

        public string AddItemToCart(string productId, string qty, string size, string typeId, string colorId)
        {
            Cart cart = new Cart();
            List<Product> lstProducts;

            if (Session["Cart"] != null)
            {
                cart = (Cart)Session["Cart"];
                lstProducts = cart.lstProducts;
            }
            else
            {
                lstProducts = new List<Product>();
            }

            ProductData productData = new ProductData();
            Product product = new Product();
            product = productData.GetProduct(Convert.ToInt32(productId), Convert.ToInt32(typeId), Convert.ToInt32(colorId));
            product.Quantity = Convert.ToInt32(qty);
            product.CartGuid = Guid.NewGuid().ToString();
            product.TypeId = Convert.ToInt32(typeId);
            product.ColorId = Convert.ToInt32(colorId);
            if (Convert.ToInt32(typeId) != 2)
            {
                size = "OSFA";
            }
            product.Size = size;

            lstProducts.Add(product);

            var totalProducts = 0;
            cart.Total = 0.00M;

            foreach (Product prod in lstProducts)
            {
                totalProducts += prod.Quantity;
                if(prod.ApplyMethod.ToLower() == "embroidery")
                {
                    cart.Total += prod.Quantity * 19.99M;
                }
                if(prod.ApplyMethod.ToLower() == "leatherpatch")
                {
                    cart.Total += prod.Quantity * 29.99M;
                }
            }

            //cart.Total = totalProducts * 19.99M;

            cart.lstProducts = lstProducts;
            Session["Cart"] = cart;
            Session["TotalCartQuantity"] = totalProducts;
            
            var json = new JavaScriptSerializer().Serialize(totalProducts);
            return json;
        }

        public string RemoveItemFromCart(string cartGuid)
        {
            Cart cart = new Cart();
            List<Product> lstProducts;

            if (Session["Cart"] != null)
            {
                cart = (Cart)Session["Cart"];
                lstProducts = cart.lstProducts;
            }
            else
            {
                lstProducts = new List<Product>();
            }

            cart.Total = 0.00M;

            var totalProducts = 0;
            var indexToRemove = -1;
            foreach (Product prod in lstProducts)
            {
                if (prod.CartGuid == cartGuid)
                {
                    indexToRemove = lstProducts.IndexOf(prod);
                }
                else
                {
                    totalProducts += prod.Quantity;
                }
                if (prod.ApplyMethod.ToLower() == "embroidery")
                {
                    cart.Total += prod.Quantity * 19.99M;
                }
                if (prod.ApplyMethod.ToLower() == "leatherpatch")
                {
                    cart.Total += prod.Quantity * 29.99M;
                }
            }

            if(indexToRemove != -1)
            {
                lstProducts.Remove(lstProducts[indexToRemove]);
            }

            //cart.Total = totalProducts * 19.99M;

            cart.lstProducts = lstProducts;
            Session["Cart"] = cart;
            Session["TotalCartQuantity"] = totalProducts;

            var json = new JavaScriptSerializer().Serialize(totalProducts);
            return json;
        }
        //public string SubmitOrder(string total, string firstName, string lastName, string email, string phone, string address, string city, string state, string zip, string addressBill, string cityBill, string stateBill, string zipBill)
        public string SubmitOrder(string total, string firstName, string lastName, string email, string phone)
        {
            OrderData orderData = new OrderData();
            Order order = new Order();

            Cart cart = new Cart();
            List<Product> lstProducts;

            if (Session["Cart"] != null)
            {
                cart = (Cart)Session["Cart"];
                lstProducts = cart.lstProducts;
            }
            else
            {
                lstProducts = new List<Product>();
            }



            decimal orderTotal = 0;
            orderTotal = cart.Total;

            //check if this product is setup for free shipping
            var productHasFreeShipping = false;

            foreach (Product prod in cart.lstProducts)
            {
                if (orderData.CheckProductHasFreeShipping(prod.Id))
                {
                    productHasFreeShipping = true;
                }
            }

            //add shipping
            if (!productHasFreeShipping)
            {
                orderTotal += 5;
            }            

            String paymentGuid = Guid.NewGuid().ToString();
            //var orderId = orderData.CreateOrder(orderTotal, firstName, lastName, email, phone, address, city, state, zip, addressBill, cityBill, stateBill, zipBill, paymentGuid, Convert.ToInt32(Session["UserID"]));
            var orderId = orderData.CreateOrder(orderTotal, firstName, lastName, email, phone, "", "", "", "", "", "", "", "", paymentGuid, Convert.ToInt32(Session["UserID"]));
            
            foreach (Product prod in cart.lstProducts)
            {
                var orderProductId = 0;
                if (prod.Quantity == 1)
                {
                    orderProductId = orderData.CreateOrderProduct(orderId, prod.Id, prod.Size, prod.TypeId);
                } else
                {
                    for(int i = 1; i <= prod.Quantity; i++)
                    {
                        orderProductId = orderData.CreateOrderProduct(orderId, prod.Id, prod.Size, prod.TypeId);
                    }
                }                
            }
            
            if (orderId > 0)
            {
                EmailFunctions emailFunc = new EmailFunctions();
                var emailSuccess = emailFunc.sendEmail(email, firstName + " " + lastName, emailFunc.orderEmail(cart.lstProducts, total, orderId.ToString()), "Lid Launch Order Confirmation", "");
            }

            var json = new JavaScriptSerializer().Serialize(new string[] {paymentGuid, orderId.ToString()});
            return json;
        }
        public virtual ActionResult Payment(string PaymentCode)
        {
            //GET APPOINTMENT INFO AND UPDATE HAS PAID FOR THE APPOINTMENT
            OrderData orderData = new OrderData();

            var success = orderData.UpdateOrderHasPaid(PaymentCode);

            Session["Cart"] = null;
            Session["TotalCartQuantity"] = null;

            var json = new JavaScriptSerializer().Serialize(success);

            return View();
        }
    }
}