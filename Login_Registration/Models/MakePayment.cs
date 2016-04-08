using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using Login_Registration.Models;
using System.Net;

namespace Login_Registration.Models
{
    public class MakePayment
    {
        public Item item { get; set; }
        public Address address { get; set; }
        public CreditCard creditcard { get; set; }
        public Details details { get; set; }
        public Amount amount { get; set; }
        public Transaction transaction { get; set; }
        public FundingInstrument funding { get; set; }
        public Payment pay { get; set; }
        public Payer payer { get; set; }

        // Variable payment
        private PayPal.Api.Payment payment;

        // Function of ExecutePayment
        public Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        // Function of CreatePayment
        public Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = item.name,
                currency = "USD",
                price = item.price,
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = (double.Parse(item.price)*0.13).ToString(),
                shipping = "0",
                subtotal = item.price
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (double.Parse(details.tax) +double.Parse(details.shipping)+double.Parse(details.subtotal)).ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            Random randon = new Random();
            int i = randon.Next(0, 10000);
            int invoice = i;
            transactionList.Add(new Transaction()
            {
                description = "Pay membership with Paypal",
                invoice_number = (++invoice).ToString(),
                amount = amount,
                item_list = itemList
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return payment.Create(apiContext);
        }// end of CreatePayment
    }
}