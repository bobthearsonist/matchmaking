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
    public class SessionUsersController : Controller
    {
        private PlayerMatcherEntities db = new PlayerMatcherEntities();

        // GET: SessionUsers
        public ActionResult Index()
        {
            var session_Users = db.Session_Users.Include(s => s.Game_Sessions).Include(s => s.User);
            return View(session_Users.ToList());
        }

        // GET: SessionUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session_Users session_Users = db.Session_Users.Find(id);
            if (session_Users == null)
            {
                return HttpNotFound();
            }
            return View(session_Users);
        }

        // GET: SessionUsers/Create
        public ActionResult Create()
        {
            ViewBag.Game_Session_ID = new SelectList(db.Game_Sessions, "Game_Session_ID", "Game_Session_ID");
            ViewBag.Users_ID = new SelectList(db.Users, "User_ID", "User_Name");
            return View();
        }

        // POST: SessionUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Game_Session_ID,Users_ID")] Session_Users session_Users)
        {
            if (ModelState.IsValid)
            {
                db.Session_Users.Add(session_Users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Game_Session_ID = new SelectList(db.Game_Sessions, "Game_Session_ID", "Game_Session_ID", session_Users.Game_Session_ID);
            ViewBag.Users_ID = new SelectList(db.Users, "User_ID", "User_Name", session_Users.Users_ID);
            return View(session_Users);
        }

        // GET: SessionUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session_Users session_Users = db.Session_Users.Find(id);
            if (session_Users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Game_Session_ID = new SelectList(db.Game_Sessions, "Game_Session_ID", "Game_Session_ID", session_Users.Game_Session_ID);
            ViewBag.Users_ID = new SelectList(db.Users, "User_ID", "User_Name", session_Users.Users_ID);
            return View(session_Users);
        }

        // POST: SessionUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Game_Session_ID,Users_ID")] Session_Users session_Users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session_Users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Game_Session_ID = new SelectList(db.Game_Sessions, "Game_Session_ID", "Game_Session_ID", session_Users.Game_Session_ID);
            ViewBag.Users_ID = new SelectList(db.Users, "User_ID", "User_Name", session_Users.Users_ID);
            return View(session_Users);
        }

        // GET: SessionUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session_Users session_Users = db.Session_Users.Find(id);
            if (session_Users == null)
            {
                return HttpNotFound();
            }
            return View(session_Users);
        }

        // POST: SessionUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session_Users session_Users = db.Session_Users.Find(id);
            db.Session_Users.Remove(session_Users);
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
