using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login_Registration.Models;
using System.Web.Security;
using System.Drawing;
using System.Text;

namespace Login_Registration.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/


        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "This can be viewed only by authenticated users only";
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }
            public ViewResult LogOn()
            {
                return View();
            }
            public ViewResult Register()
            {
                return View();
            }

            [HttpPost, ValidateInput(false)]
            public ActionResult LogOn(Login model)
            {
                if (ModelState.IsValid)
                {
                    if (model.IsUserExist(model.email, model.password))
                    {
                       
                        ViewBag.UserName = model.email;
                        Session["userID"] = model.email;
                        FormsAuthentication.RedirectFromLoginPage(model.email, false);
                        FormsAuthentication.SetAuthCookie(model.email, false);
                        return RedirectToAction("viewProfile", "Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email or Password Incorrect.");
                    }
                } 
                return View(model);
            }

            
            public ActionResult LogOut()
            {
                FormsAuthentication.SignOut();
                Session.Remove("userID");
                Session.RemoveAll();
                return RedirectToAction("LogOn", "Home");
            }

            [HttpPost, ValidateInput(false)]
            public ActionResult Register(Register model, string ispay)
            {

                if (ModelState.IsValid)
                {
                    string realCaptcha = Session["captcha"].ToString();
                    Session["userID"] = model.email;
                    if (model.Captcha == realCaptcha)
                    {
                        if (model.Insert())
                        {
                            if(ispay=="false")
                            return RedirectToAction("viewProfile", "Profile");
                             if(ispay=="true")
                            return RedirectToAction("Index", "Paypal");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email  Already Exist");
                        }
                    }
                    else
                        ModelState.AddModelError("", "Verification Code is Incorrect");
                }
                return View(model);
            }

            public CaptchImageAction Image()
            {
                string randomText = SelectRandomWord(6);
                Session["captcha"] = randomText;
                HttpContext.Session["RandomText"] = randomText;
                return new CaptchImageAction()
                {
                    BackgroundColor = Color.LightGray,
                    RandomTextColor = Color.Black,
                    RandomText = randomText
                };
            }

            private string SelectRandomWord(int numberOfChars)
            {
                if (numberOfChars > 36)
                {
                    throw new InvalidOperationException("Random Word Characters cannot be greater than 36");
                }
                char[] columns = new char[36];
                for (int charPos = 65; charPos < 65 + 26; charPos++)
                    columns[charPos - 65] = (char)charPos;
                for (int intPos = 48; intPos <= 57; intPos++)
                    columns[26 + (intPos - 48)] = (char)intPos;
                StringBuilder randomBuilder = new StringBuilder();
                Random randomSeed = new Random();
                for (int incr = 0; incr < numberOfChars; incr++)
                {
                    randomBuilder.Append(columns[randomSeed.Next(36)].ToString());
                }
                return randomBuilder.ToString();
            }
        }

    }

