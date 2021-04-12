using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: games
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.team).Include(g => g.team1).Include(g => g.team2).Include(g => g.tournament);
            return View(games.ToList());
        }
        /*      /*public ActionResult Index(long? tournament_id)
                {
                    var games = db.games.Include(g => g.team).Include(g => g.team1).Include(g => g.team2).Include(g => g.tournament);
                    return View(games.ToList());
                }*/

        // GET: games/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: games/Create
        public ActionResult Create(long? id)
        {
           
            var teams = db.Teams.Where(x => x.tournament_id == id);
            Tournament t = db.Tournaments.Find(id);


            ViewBag.rteam_id = new SelectList(teams, "id", "name");
            ViewBag.winner_id = new SelectList(teams, "id", "name");
            ViewBag.bteam_id = new SelectList(teams, "id", "name");
            ViewBag.tournament_id = id;
            return View(new Game(t: t));
        }


        // POST: games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(long? id , [Bind(Include = "id,date,bteam_id,rteam_id,tournament_id,winner_id")] Game game)
        {
            game.tournament_id = id;
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Details", "Tournaments", new { id = id });
            }

            var teams = db.Teams.Where(x => x.tournament_id == game.tournament_id);

            ViewBag.rteam_id = new SelectList(teams, "id", "name", game.rteam_id);
            ViewBag.winner_id = new SelectList(teams, "id", "name", game.winner_id);
            ViewBag.bteam_id = new SelectList(teams, "id", "name", game.bteam_id);
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name", game.tournament_id);
            return View(game);
        }

        // GET: games/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            var teams = db.Teams.Where(x => x.tournament_id == game.tournament_id);

            Tournament t = db.Tournaments.Find(game.tournament_id);
            ViewBag.winner_id = new SelectList(teams, "id", "name", game.winner_id);
            ViewBag.rteam_id = new SelectList(teams, "id", "name", game.rteam_id);
            ViewBag.bteam_id = new SelectList(teams, "id", "name", game.bteam_id);
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name", game.tournament_id);
            return View(game);
        }

        // POST: games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date,bteam_id,rteam_id,tournament_id,winner_id")] Game game)
        {
     
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tournaments", new { id = game.tournament_id });
            }
            var teams = db.Teams.Where(x => x.tournament_id == game.tournament_id);
            ViewBag.rteam_id = new SelectList(teams, "id", "name", game.rteam_id);
            ViewBag.tournament_id = new SelectList(db.Tournaments, "id", "name", game.tournament_id);
            ViewBag.bteam_id = new SelectList(teams, "id", "name", game.bteam_id);
            ViewBag.winner_id = new SelectList(teams, "id", "name", game.winner_id);
            return View(game);
        }

        // GET: games/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Game game = db.Games.Find(id);
            long? tid = game.tournament_id;
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Details", "Tournaments", new { id = tid});
        }

        public ActionResult BacktoTournament(long? id)
        {
            
            return RedirectToAction("Details", "Tournaments", new { id = id });
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