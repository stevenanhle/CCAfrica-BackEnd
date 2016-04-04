using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login_Registration.Models;

namespace Login_Registration.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ViewResult viewProfile(Profile model)
        {
            model.Retrieve(Session["userID"].ToString());
            return View(model);
        }
        public ViewResult editProfile(Profile model)
        {
            model.Retrieve(Session["userID"].ToString());
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult saveProfile(Profile model)
        {

            if ( model.Edit(Session["userID"].ToString())==true)
            {
                return RedirectToAction("viewProfile", "Profile"); 
            }
            return View(model);
        }

    }
}
