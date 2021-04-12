using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Security.Claims;
using System.Net;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public HomeController()
        {

        }

        // GET: tournaments
        public ActionResult Index()
        {
            return View(db.Tournaments.ToList());
        }
        [HttpGet]
        public ActionResult Details(long? id)
        {
            return RedirectToAction("Details", "Tournaments",new { id = id });
        }

        public ActionResult Create()
        {
            return RedirectToAction("Create", "Tournaments");
        }

        // GET: tournaments/Delete/5
        public ActionResult Delete(long? id)
        {
            return RedirectToAction("Delete", "Tournaments", new { id = id });
        }

        public ActionResult addTeam(long? id)
        {

            return RedirectToAction("addTeam", "Tournaments", new { id = id });

        }

        public ActionResult Edit(long? id)
        {

            return RedirectToAction("Edit", "Tournaments", new { id = id });
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
            String userID = User.Identity.GetUserId();
  
            return View(db.Tournaments.Where(x => x.ApplicationUserId.Equals(userID)));
            
        }

        /* Si l'utilisateur n'est pas un admin, il sera redirigé vers la page login pour qu'il se connecte en tant qu'admin */
        [Authorize (Roles = "Admin")]
        public ActionResult Admin_page()
        {
            return View(db.Tournaments.ToList());
        }
    }
}