using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlayerMatcher;

namespace PlayerMatcher.Controllers
{
    public class Game_SessionsController : Controller
    {
        private PlayerMatcherEntities db = new PlayerMatcherEntities();

        // GET: Game_Sessions
        public ActionResult Index()
        {
            var game_Sessions = db.Game_Sessions.Include(g => g.Game).Include(g => g.User);
            return View(game_Sessions.ToList());
        }

        // GET: Game_Sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game_Sessions game_Sessions = db.Game_Sessions.Find(id);
            if (game_Sessions == null)
            {
                return HttpNotFound();
            }
            return View(game_Sessions);
        }

        // GET: Game_Sessions/Create
        public ActionResult Create()
        {
            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name");
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name");
            return View();
        }

        // POST: Game_Sessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Game_Session_ID,Session_Start,Session_End,Game_ID,User_ID")] Game_Sessions game_Sessions)
        {
            if (ModelState.IsValid)
            {
                db.Game_Sessions.Add(game_Sessions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name", game_Sessions.Game_ID);
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name", game_Sessions.User_ID);
            return View(game_Sessions);
        }

        // GET: Game_Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game_Sessions game_Sessions = db.Game_Sessions.Find(id);
            if (game_Sessions == null)
            {
                return HttpNotFound();
            }
            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name", game_Sessions.Game_ID);
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name", game_Sessions.User_ID);
            return View(game_Sessions);
        }

        // POST: Game_Sessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Game_Session_ID,Session_Start,Session_End,Game_ID,User_ID")] Game_Sessions game_Sessions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game_Sessions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name", game_Sessions.Game_ID);
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name", game_Sessions.User_ID);
            return View(game_Sessions);
        }

        // GET: Game_Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game_Sessions game_Sessions = db.Game_Sessions.Find(id);
            if (game_Sessions == null)
            {
                return HttpNotFound();
            }
            return View(game_Sessions);
        }

        // POST: Game_Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game_Sessions game_Sessions = db.Game_Sessions.Find(id);
            db.Game_Sessions.Remove(game_Sessions);
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
