using System;
namespace POS.Models
{
    public class PaymentDetails
    {
        public string CardNumber { get; set; }          // Card number for payment
        public string CardHolderName { get; set; }      // Name of the cardholder
        public DateTime ExpirationDate { get; set; }    // Expiration date of the card
        public string CVV { get; set; }                 // Card Verification Value (CVV)
        public decimal Amount { get; set; }             // Amount to be paid
        public string PaymentMethod { get; set; }       // Payment method (e.g., Credit Card, Debit Card, etc.)
    }

}

