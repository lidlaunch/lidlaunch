using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

        public ActionResult ProcessPayment()
        {
            return View();
        }

        public string PaymentWithCreditCard(string creditCard, string cartItems, string billingAddress, string shippingAddress, string shippingRecipient, int shippingPrice, string email,  string isBulkOrder, string orderNotes, string artworkPlacement)
        {            

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Item>>(cartItems);
            CreditCard cc = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditCard>(creditCard);
            Address billAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<Address>(billingAddress);
            Address shipAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<Address>(shippingAddress);

            bool bulkOrder = Convert.ToBoolean(isBulkOrder);

            ShippingAddress shippingAddressNew = new ShippingAddress();

            shippingAddressNew.state = shipAddress.state;
            shippingAddressNew.line1 = shipAddress.line1;
            shippingAddressNew.postal_code = shipAddress.postal_code;
            shippingAddressNew.city = shipAddress.city;
            if(shipAddress.phone == "")
            {
                shippingAddressNew.phone = null;
            }         
            shippingAddressNew.recipient_name = shippingRecipient;
            shippingAddressNew.country_code = "US";
            

            if(billAddress.phone == "")
            {
                billAddress.phone = null;
            }
            billAddress.country_code = "US";

            ItemList itemList = new ItemList();
            itemList.items = items;
            itemList.shipping_address = shippingAddressNew;


            //remove spaces from credit card number...
            cc.number = cc.number.Replace(" ", "");

            cc.type = FindType(cc.number);

            cc.billing_address = billAddress;

            var subtotal = items.Sum(i => Convert.ToDecimal(i.price) * Convert.ToInt32(i.quantity));
            var total = shippingPrice + subtotal;

            // Specify details of your payment amount.
            var details = new Details();
            details.shipping = shippingPrice.ToString();
            details.subtotal = subtotal.ToString();
            details.tax = "0";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = total.ToString();
            amnt.details = details;

            string invoiceNumber = "";

            string paymentGuid = Guid.NewGuid().ToString();
            

            HttpPostedFileBase fileContent = null;
            if (Request.Files.Count > 0)
            {
                fileContent = Request.Files[0];
            }

            //check if the credit card type is valid
            if (cc.type == "unknown")
            {
                return "ccerror";
            }

            // Now make a trasaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Lid Launch Order Payment.";
            tran.item_list = itemList;
            if(bulkOrder)
            {
                tran.invoice_number = "Bulk:" + paymentGuid;
            } else
            {
                tran.invoice_number = "Web:" + paymentGuid;
            }            

            // Now, we have to make a list of trasaction and add the trasactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = cc;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";            

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                //getting context from the paypal, basically we are sending the clientID and clientSecret key in this function 
                //to the get the context from the paypal API to make the payment for which we have created the object above.

                //Code for the configuration class is provided next

                // Basically, apiContext has a accesstoken which is sent by the paypal to authenticate the payment to facilitator account. An access token could be an alphanumeric string

                APIContext apiContext = PaypalConfiguration.GetAPIContext();

                // Create is a Payment class function which actually sends the payment details to the paypal API for the payment. The function is passed with the ApiContext which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.State is "approved" it means the payment was successfull else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return "ccerror";
                }
                else
                {
                    string phone = billAddress.phone == null ? "" : billAddress.phone;
                    if (bulkOrder)
                    {
                        BulkController bc = new BulkController();
                        string orderResult = bc.CreateBulkOrder(cartItems, shippingRecipient, email, phone, artworkPlacement, orderNotes, total.ToString(), paymentGuid, fileContent);                                              
                    }
                    else
                    {                        
                        string orderResult = SubmitOrder(total.ToString(), cc.first_name, cc.last_name, email, phone, shipAddress.line1, shipAddress.city, shipAddress.state, shipAddress.postal_code, billAddress.line1, billAddress.city, billAddress.state, billAddress.postal_code, paymentGuid);                                             
                    }
                }
            }
            catch (PayPal.PayPalException ex)
            {
                Logger.Log("Error: " + ex.Message);
                return "ccerror";
            }

            return paymentGuid;
        }

        public string FindType(string cardNumber)
        {
            //https://www.regular-expressions.info/creditcard.html
            if (Regex.Match(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
            {
                return "visa";
            }

            if (Regex.Match(cardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
            {
                return "mastercard";
            }

            if (Regex.Match(cardNumber, @"^3[47][0-9]{13}$").Success)
            {
                return "amex";
            }

            if (Regex.Match(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$").Success)
            {
                return "discover";
            }

            return "unknown";
        }


        private PayPal.Api.Payment payment;


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
        public string SubmitOrder(string total, string firstName, string lastName, string email, string phone, string address, string city, string state, string zip, string addressBill, string cityBill, string stateBill, string zipBill, string paymentGuid)        
        {
            OrderData orderData = new OrderData();
            Models.Order order = new Models.Order();

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

            
            var orderId = orderData.CreateOrder(orderTotal, firstName, lastName, email, phone, address, city, state, zip, addressBill, cityBill, stateBill, zipBill, paymentGuid, Convert.ToInt32(Session["UserID"]));
            //var orderId = orderData.CreateOrder(orderTotal, firstName, lastName, email, phone, "", "", "", "", "", "", "", "", paymentGuid, Convert.ToInt32(Session["UserID"]));
            
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

            //var json = new JavaScriptSerializer().Serialize(new string[] {paymentGuid, orderId.ToString()});
            return orderId.ToString();
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