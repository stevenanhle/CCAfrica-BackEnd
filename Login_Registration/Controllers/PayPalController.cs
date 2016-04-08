using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using Login_Registration.Models;
using System.Net;
using MySql.Data.MySqlClient;

namespace Login_Registration.Controllers
{
    public class PayPalController : Controller
    {
        //
        // GET: /PayPal/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithCreditCard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PaymentWithCreditCard(MakePayment model)
        {

            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item();
            item.name = model.item.name;
            item.currency = "USD";
            item.price = model.item.price;
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = model.address.city;
            billingAddress.country_code = model.address.country_code;
            billingAddress.line1 = model.address.line1;
            billingAddress.postal_code = model.address.postal_code;
            billingAddress.state = model.address.state;


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "111";  //card cvv2 number
            crdtCard.expire_month = 12; //card expire date
            crdtCard.expire_year = 2018; //card expire year
            crdtCard.first_name = "Besty";
            crdtCard.last_name = "Buyer";
            crdtCard.number = "5500005555555559"; //enter your credit card number here
            crdtCard.type = "mastercard"; //credit card type here paypal allows 4 types
            
            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "0";
            details.subtotal = item.price;
            details.tax =(double.Parse(item.price)*0.13).ToString(); // Total must be equal to sum of shipping, tax and subtotal.;

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = (double.Parse(details.tax) + double.Parse(details.shipping) + double.Parse(details.subtotal)).ToString(); // Total must be equal to sum of shipping, tax and subtotal.;
            amnt.details = details;

            // Now make a transaction object and assign the Amount object
            Random rand = new Random();
            int ran= rand.Next(0, 10000);
            int invoice = ran;// By this way, invoice id is increased by 1 with each transaction done
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = model.transaction.description;
            tran.item_list = itemList;
            tran.invoice_number = (++invoice).ToString();

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

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
            
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.DefaultConnectionLimit = 9999;
            APIContext apiContext = MyPaypal.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
                else
                {
                    Addmembership(Session["userID"].ToString());
                    return View("SuccessView");
                }
        }

     public ActionResult PaymentWithPaypal()
      {
            return View();
      }

     public ActionResult Finish(MakePayment model)
     {
         ServicePointManager.Expect100Continue = true;
         ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
         ServicePointManager.DefaultConnectionLimit = 9999;
         APIContext apiContext = MyPaypal.GetAPIContext();
         string payerId = Request.Params["PayerID"];
         var guid = Request.Params["guid"];

         var executedPayment = model.ExecutePayment(apiContext, payerId, Session[guid] as string);

         if (executedPayment.state.ToLower() != "approved")
         {
             return View("FailureView");
         }
         Addmembership(Session["userID"].ToString());
         return View("SuccessView");
     }

     [HttpPost]
     public ActionResult PaymentWithPaypal(MakePayment model)
     {
     //getting the apiContext as earlier
         ServicePointManager.Expect100Continue = true;
         ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
         ServicePointManager.DefaultConnectionLimit = 9999;
         APIContext apiContext = MyPaypal.GetAPIContext();

         string payerId = Request.Params["PayerID"];
          if (string.IsNullOrEmpty(payerId))
          {
            //this section will be executed first because PayerID doesn't exist
            //it is returned by the create function call of the payment class

            // Creating a payment
            // baseURL is the url on which paypal sendsback the data.
            // So we have provided URL of this controller only
            string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + 
                        "/Paypal/Finish?";

            //guid we are generating for storing the paymentID received in session
            //after calling the create function and it is used in the payment execution

            var guid = Convert.ToString((new Random()).Next(100000));

            //CreatePayment function gives us the payment approval url
            //on which payer is redirected for paypal account payment

            var createdPayment = model.CreatePayment(apiContext, baseURI + "guid=" + guid);

            //get links returned from paypal in response to Create function call

            var links = createdPayment.links.GetEnumerator();

            string paypalRedirectUrl = null;
            
            while (links.MoveNext())
            {
                 Links lnk = links.Current;

                 if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                 {
                       //saving the payapalredirect URL to which user will be redirected for payment
                       paypalRedirectUrl = lnk.href;
                 }
            }

            // saving the paymentID in the key guid
            Session.Add(guid, createdPayment.id);

            return Redirect(paypalRedirectUrl); //WHY ????????????????????
         }
     return View("SuccessView");
   }

     public void Addmembership(string UserId)
     {
          
                MySqlConnection connection = new MySqlConnection("server=localhost; user id=root; database=demo; password=1234");
                connection.Open();
                String text = "UPDATE members SET membership=@membership, sincedate=@sincedate where email=@email";
                MySqlCommand command = new MySqlCommand(text, connection);
                command.Parameters.AddWithValue("@email", UserId);
                command.Parameters.AddWithValue("@membership", "True");
                command.Parameters.AddWithValue("@sincedate", DateTime.Now);
                command.ExecuteNonQuery();
                connection.Close();
     }

    
   }
}