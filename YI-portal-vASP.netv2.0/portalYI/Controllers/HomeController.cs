using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using portalYI.Models; 

namespace portalYI.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Downloads()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(user u)
        {
            if(ModelState.IsValid)
            {
                using (DatabaseEntities dc = new DatabaseEntities())
                {
                    var v = dc.users.Where(a => a.email.Equals(u.email) && a.password.Equals(u.password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.id.ToString();
                        Session["LogedUserName"] = v.name.ToString();
                        return RedirectToAction("Index","Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }

            }
            return View(u);
        }


    }
}