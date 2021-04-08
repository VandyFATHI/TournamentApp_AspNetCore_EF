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
    public class TeamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: teams
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.player).Include(t => t.tournament);
            return View(teams.ToList());
        }

        // GET: teams/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: teams/Create
        public ActionResult Create()
        {
            ViewBag.captain_id = new SelectList(db.Players, "id", "name");
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name");
            return View();
        }

        // POST: teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name, nb_members, captain_id,tournament_id")] Team team)
        {
            if (ModelState.IsValid)
            {
                Tournament t = db.Tournaments.Find(team.tournament_id);
                team.nb_members = t.team_size;
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.captain_id = new SelectList(team.players, "id", "name", team.captain_id);
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name", team.tournament_id);
            return View(team);
        }

        public ActionResult addPlayer(long? id)
        {
            Team t = db.Teams.Find(id);
            return RedirectToAction("Create", "players", new Player(teamId: t.id));
        }

        // GET: teams/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.captain_id = new SelectList(team.players, "id", "name", team.captain_id);
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name", team.tournament_id);
            return View(team);
        }

        // POST: teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, name,nb_members,captain_id,tournament_id")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.captain_id = new SelectList(team.players, "id", "name", team.captain_id);
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name", team.tournament_id);
            return View(team);
        }

        // GET: teams/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Team team = db.Teams.Find(id);

            foreach (Player p in team.players.ToList())
            {
                db.Players.Remove(p);
            }
            db.SaveChanges();

            db.Teams.Remove(team);
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
