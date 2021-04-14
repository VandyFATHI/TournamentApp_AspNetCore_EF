using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationUser application = new ApplicationUser();

        public AdminController() { }
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Create()
        {
            return View(new ApplicationUser());
        }

        [HttpPost]
        public async Task<ActionResult> Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await UserManager.CreateAsync(user, "motdepasse");
                if (user.FirstName == "admin")
                {
                    await UserManager.AddToRoleAsync(user.Id, "Admin");
                }
                else
                {
                    await UserManager.AddToRoleAsync(user.Id, "User");
                }

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Pour plus d'informations sur l'activation de la confirmation de compte et de la réinitialisation de mot de passe, visitez https://go.microsoft.com/fwlink/?LinkID=320771
                    // Envoyer un message électronique avec ce lien
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirmez votre compte", "Confirmez votre compte en cliquant <a href=\"" + callbackUrl + "\">ici</a>");
                    return RedirectToAction("Index");
                }

            }

            // Si nous sommes arrivés là, un échec s’est produit. Réafficher le formulaire
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            List<Tournament> tournaments = db.Tournaments.Where(a => a.ApplicationUserId == id).ToList();
            foreach (Tournament tournament in tournaments)
            {
                foreach (Team t in tournament.teams.ToList())
                {
                    foreach (Player p in t.players.ToList())
                    {
                        db.Players.Remove(p);
                    }
                    db.SaveChanges();

                    foreach (Game g in t.games.ToList())
                    {
                        db.Games.Remove(g);
                    }
                    db.SaveChanges();

                    db.Teams.Remove(t);
                }
                db.SaveChanges();
                db.Games.RemoveRange(tournament.games);
                db.SaveChanges();
                db.Tournaments.Remove(tournament);
                db.SaveChanges();
            }

            db.Users.Remove(user);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

    }
}
