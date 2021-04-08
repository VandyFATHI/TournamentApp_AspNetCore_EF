using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Security.Claims;


namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public HomeController()
        {

        }

        public ActionResult Index()
        {
            /*
            ClaimsPrincipal currentUser = (ClaimsPrincipal)this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.msg = currentUserID;*/
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize (Roles = "User")]
        public ActionResult User_page()
        {
            return View();
        }

        /* Si l'utilisateur n'est pas un admin, il sera redirigé vers la page login pour qu'il se connecte en tant qu'admin */
        [Authorize (Roles = "Admin")]
        public ActionResult Admin_page()
        {
            return View();
        }
    }
}