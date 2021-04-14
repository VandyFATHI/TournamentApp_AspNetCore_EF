using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public TournamentsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public TournamentsController()
        {

        }
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: tournaments
        public ActionResult Index()
        {
            return View(db.Tournaments.ToList());
        }

        // Get : Search?search= {searchRequest}
        public ActionResult Search(string searchRequest)
        {
            if(searchRequest is null || searchRequest == "")
            {
                return View(db.Tournaments.ToList());
            }
            else {
                return View(db.Tournaments.Where(a => a.name.Contains(searchRequest) || a.name == null).ToList());
            }
            }
            

        public ActionResult View_Team(long? id)
        {
            return RedirectToAction("Index", "Teams", new { id = id });
            
        }

        // GET: tournaments/Details/5
        public ActionResult Details(long? id)
        {
            System.Diagnostics.Debug.WriteLine(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            String userID = User.Identity.GetUserId();
            if (tournament.ApplicationUserId.Equals(userID))
            {
                ViewBag.isOwner = true;
            }
            return View(tournament);
        }

        [Authorize]
        // GET: tournaments/Create
        public ActionResult Create()
        {
            return View(new Tournament());
        }


        // POST: tournaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nb_participants,description,game,name,start_date,team_size")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                
                ClaimsPrincipal currentUser = (ClaimsPrincipal)this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                tournament.ApplicationUserId = currentUserID;
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Details", new {id = tournament.id });
            }


            return View(tournament);
        }

        [Authorize]
        public ActionResult CreateGame([Bind(Include = "id,nb_participants,description,game,name,start_date,team_size")] long? id)
        {

            Tournament t = db.Tournaments.Find(id);
            return RedirectToAction("Create", "games", new { id = id });
        }

        [Authorize]
        // GET: tournaments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            return View(tournament);
        }

        // POST: tournaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, nb_participants,description,game,name,start_date,team_size")] Tournament tournament)
        {
            String userID = User.Identity.GetUserId();
            tournament.ApplicationUserId = userID;
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
             
                db.SaveChanges();
                return RedirectToAction("Details", new {id = tournament.id });
            }
            return View(tournament);
        }

        
        public ActionResult Edit_Game(long? id)
        {
            return RedirectToAction("Edit", "games", new { id = id });
        }

        public ActionResult Delete_Game(long? id)
        {
            return RedirectToAction("Delete", "games", new { id = id });
        }

        public ActionResult Details_Game(long? id)
        {
            return RedirectToAction("Details", "games", new { id = id });
        }





        public ActionResult addTeam(long? id)
        {

            Tournament t = db.Tournaments.Find(id);
            return RedirectToAction("Create", "teams", new { id = id});

        }

        // GET: tournaments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Tournament tournament = db.Tournaments.Find(id);
            foreach (Team t in tournament.teams.ToList())
            {
                foreach (Player p in t.players.ToList())
                {
                    db.Players.Remove(p);
                }
                db.SaveChanges();

                foreach(Game g in t.games.ToList())
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
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
