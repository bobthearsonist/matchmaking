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
    public class RatingsController : Controller
    {
        private PlayerMatcherEntities db = new PlayerMatcherEntities();

        // GET: Ratings
        public ActionResult Index()
        {
            var ratings = db.Ratings.Include(r => r.Game).Include(r => r.User);
            return View(ratings.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Ratings/Create
        public ActionResult Create()
        {
            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name");
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name");
            return View();
        }

        // POST: Ratings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Skill, int Friendliness, int Team, int Rate, int? Game_ID)
        {
            int skillLevel = (int)((10*(Skill + Friendliness + Team + Rate))/16);
            var userLogedIn = (User)Session["user"];
            var employeeLoggedIn = db.Ratings.Where(x => x.User_ID == userLogedIn.User_ID && x.Game_ID == Game_ID).FirstOrDefault();

                Rating rating = new Rating();
                rating.Game_ID = Game_ID;
                rating.User_ID = userLogedIn.User_ID;
                rating.User_Rating = skillLevel;
            if (employeeLoggedIn == null)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                return RedirectToAction("Index");
            }  
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name", rating.Game_ID);
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name", rating.User_ID);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Rating_ID,User_Rating,User_ID,Game_ID")] Rating rating)
        {
            var userLogedIn = (User)Session["user"];
            rating.User_ID = userLogedIn.User_ID;
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Game_ID = new SelectList(db.Games, "Game_ID", "Game_Name", rating.Game_ID);
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "User_Name", rating.User_ID);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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
