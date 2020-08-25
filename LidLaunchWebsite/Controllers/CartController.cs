using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;

using PayPal.Payments.Common;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
//using PayPal.Payments.Samples.CS.DataObjects.Recurring;
using PayPal.Payments.Transactions;

using System.Collections.Generic;
using System.Linq;
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

        public string PaymentWithCreditCard(string cartItems, string ccNumber, string ccExpMM, string ccExpYY, string ccV, string orderTotal, string billingAddress, string shippingAddress, string email,  string isBulkOrder, string orderNotes, string artworkPlacement, string shippingCost)
        {
            BillTo billAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<BillTo>(billingAddress);
            ShipTo shipAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<ShipTo>(shippingAddress);


            UserInfo User = new UserInfo("lidlaunch", "lidlaunch", "PayPal", "Z06Corvette90!");

            // Create the Payflow  Connection data object with the required connection details.
            // Values of connection details can also be passed in the constructor of 
            // PayflowConnectionData. This will override the values passed in the App config file.
            // Example values passed below are as follows:
            // Payflow Pro Host address : pilot-payflowpro.verisign.com 
            // Payflow Pro Host Port : 443
            // Timeout : 45 ( in seconds )
            //TESTING ENDPOINT
            //PayflowConnectionData Connection = new PayflowConnectionData("pilot-payflowpro.paypal.com", 443, 45);
            //PRODUCTION ENDPOINT
            PayflowConnectionData Connection = new PayflowConnectionData("payflowpro.paypal.com", 443, 45);

            // Create a new Invoice data object with the Amount, Billing Address etc. details.
            Invoice Inv = new Invoice();

            // Set Amount.
            Currency Amt = new Currency(Convert.ToDecimal(orderTotal));
            Inv.Amt = Amt;
            // Truncate the Amount value using the truncate feature of 
            // the Currency Data Object.
            // Note: Currency Data Object also has the Round feature
            // which will round the amount value to desired number of decimal
            // digits ( default 2 ). However, round and truncate cannot be used 
            // at the same time. You can set one of round or truncate true.
            Inv.Amt.Truncate = true;
            // Set the truncation decimal digit to 2.
            Inv.Amt.NoOfDecimalDigits = 2;

            HttpPostedFileBase fileContent = null;
            if (Request.Files.Count > 0)
            {
                fileContent = Request.Files[0];
            }
            string paymentGuid = Guid.NewGuid().ToString();
            bool bulkOrder = Convert.ToBoolean(isBulkOrder);


            if (bulkOrder)
            {
                Inv.PoNum = "Bulk:PO-" + paymentGuid;
                Inv.InvNum = "Bulk:INV-" + paymentGuid;
            }
            else
            {
                Inv.PoNum = "Web:PO-" + paymentGuid;
                Inv.InvNum = "Web:INV-" + paymentGuid;
            }



            Inv.BillTo = billAddress;
            Inv.ShipTo = shipAddress;

            

            // Create a new Payment Device - Credit Card data object.
            // The input parameters are Credit Card Number and Expiration Date of the Credit Card.
            CreditCard CC = new CreditCard(ccNumber, ccExpMM + ccExpYY);
            CC.Cvv2 = ccV;

            if (FindType(ccNumber) == "unknown")
            {
                return "ccerror";
            }

            // Create a new Tender - Card Tender data object.
            CardTender Card = new CardTender(CC);
            ///////////////////////////////////////////////////////////////////

            // Create a new Base Transaction.
            BaseTransaction Trans = new BaseTransaction("S",
                User, Connection, Inv, Card, PayflowUtility.RequestId);

            // Submit the Transaction
            Response Resp = Trans.SubmitTransaction();

            // Display the transaction response parameters.
            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                TransactionResponse TrxnResponse = Resp.TransactionResponse;

                if (TrxnResponse != null)
                {
                    Console.WriteLine("RESULT = " + TrxnResponse.Result);
                    Console.WriteLine("PNREF = " + TrxnResponse.Pnref);
                    Console.WriteLine("RESPMSG = " + TrxnResponse.RespMsg);
                    Console.WriteLine("AUTHCODE = " + TrxnResponse.AuthCode);
                    Console.WriteLine("AVSADDR = " + TrxnResponse.AVSAddr);
                    Console.WriteLine("AVSZIP = " + TrxnResponse.AVSZip);
                    Console.WriteLine("IAVS = " + TrxnResponse.IAVS);
                    Console.WriteLine("CVV2MATCH = " + TrxnResponse.CVV2Match);
                }

                // Get the Fraud Response parameters.
                FraudResponse FraudResp = Resp.FraudResponse;
                // Display Fraud Response parameter
                if (FraudResp != null)
                {
                    Console.WriteLine("PREFPSMSG = " + FraudResp.PreFpsMsg);
                    Console.WriteLine("POSTFPSMSG = " + FraudResp.PostFpsMsg);
                }

                // Display the response.
                Console.WriteLine(Environment.NewLine + PayflowUtility.GetStatus(Resp));


                // Get the Transaction Context and check for any contained SDK specific errors (optional code).
                Context TransCtx = Resp.TransactionContext;
                if (TransCtx != null && TransCtx.getErrorCount() > 0)
                {
                    Console.WriteLine(Environment.NewLine + "Transaction Errors = " + TransCtx.ToString());
                    return "ccerror";
                } else
                {
                    if (Resp.TransactionResponse.Result == 0)
                    {
                        string phone = billAddress.PhoneNum == null ? "" : billAddress.PhoneNum;
                        if (bulkOrder)
                        {
                            BulkController bc = new BulkController();
                            string orderResult = bc.CreateBulkOrder(cartItems, shipAddress.ShipToFirstName + " " + shipAddress.ShipToLastName, email, phone, artworkPlacement, orderNotes, orderTotal, paymentGuid, shippingCost, fileContent);
                        }
                        else
                        {
                            string orderResult = SubmitOrder(orderTotal, shipAddress.ShipToFirstName, shipAddress.ShipToLastName, email, phone, shipAddress.ShipToStreet, shipAddress.ShipToCity, shipAddress.ShipToState, shipAddress.ShipToZip, billAddress.Street, billAddress.City, billAddress.State, billAddress.Zip, paymentGuid);
                        }
                    } else
                    {
                        return "ccerror";
                    }                                     

                }
            } else
            {
                return ("ccerror");

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

            if(paymentGuid == "")
            {
                paymentGuid = Guid.NewGuid().ToString();
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
            return paymentGuid;
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