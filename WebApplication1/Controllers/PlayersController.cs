using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PlayersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: players
        public ActionResult Index(long? id)
        {
            if (id != null)
            {
                ViewBag.ID = id;
     
                var tournament = db.Teams.Where(x => x.id == id);
                var tournament_id = tournament.FirstOrDefault().tournament_id;
                ViewBag.TID = tournament_id;
                var aPlayers = db.Players.Where(x => x.team_id == id);
                ViewBag.nbPlayer = aPlayers.Count();

                var t = db.Tournaments.Where(x => x.id == tournament_id).FirstOrDefault().team_size;
                ViewBag.tSize = t;

                System.Diagnostics.Debug.WriteLine(aPlayers.Count());
                System.Diagnostics.Debug.WriteLine(t);

                return View(aPlayers);
            }


            var players = db.Players.Include(p => p.team);
            return View(players.ToList());
        }

        // GET: players/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        [Authorize]
        // GET: players/Create
        public ActionResult Create(long? id)
        {
            ViewBag.team_id = new SelectList(db.Teams, "id", "name");
            ViewBag.teamid = id;
            Team t = db.Teams.Find(id);
        
            return View(new Player(teamId: t.id));
        }

        // POST: players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(long? id, [Bind(Include = "id,name,team_id")] Player player)
        {
            
            player.team_id = (long) id;
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = player.team_id } );
            }

            ViewBag.team_id = new SelectList(db.Teams, "id", "name", player.team_id);
            return View(player);
        }

        [Authorize]
        // GET: players/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.team_id = new SelectList(db.Teams, "id", "name", player.team_id);
            return View(player);
        }

        // POST: players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, name , team_id")] Player player)
        {
            if (ModelState.IsValid)
            {


                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = player.team_id });
            }
            ViewBag.team_id = new SelectList(db.Teams, "id", "name", player.team_id);
            return View(player);
        }

        [Authorize]
        // GET: players/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Player player = db.Players.Find(id);
            long? tid = player.team_id;
            if (player.team.captain_id == id)
            {
                Team t = player.team;
                t.captain_id = null;
                db.Entry(t).State = EntityState.Modified;

            }
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = tid });
        }

        public ActionResult BacktoTournament(long? id)
        {
            return RedirectToAction("Details", "Tournaments", new { id = id });
        }

        public ActionResult BacktoTeams(long? id)
        {
            return RedirectToAction("Index", "Teams", new { id = id });
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