using System;
using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class Order 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PaypalTransactionId { get; set; }        
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZip { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string Phone { get; set; }
        public decimal Total { get; set; }
        public bool HasPaid { get; set; }
        public string PaymentGuid { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool CollectIllinoisTax { get; set; }
    }
}