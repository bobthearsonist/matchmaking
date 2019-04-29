using System.Linq;
using System.Net;
using System.Web.Mvc;
using PlayerMatcher.Matchmaker;

namespace PlayerMatcher.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController() : base() { }
        public UsersController(PlayerMatcherEntities db) : base(db) { }

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public User UserLoggedInSmart(User user) { 
        
            var employeeLoggedIn = db.Users.Where(x => x.User_Name == user.User_Name && x.User_Password == user.User_Password).FirstOrDefault();
            return employeeLoggedIn;
        }

        [AllowAnonymous]
        public ActionResult SmartLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var employeeSession = (User)Session["user"];

            if (employeeSession != null)
            {
                return RedirectToAction("WelcomePage", "Employee", new { employee = Session["employee"] });
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SmartLogin(User user)
        {
            var userLoggedIn = UserLoggedInSmart(user);
            if (userLoggedIn != null)
            {
                ViewBag.message = "loggedin";
                ViewBag.triedOnce = "yes";

                Session["user"] = userLoggedIn;

                return RedirectToAction("Welcome", "Users", new { user = Session["user"] });
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }

        }
        public ActionResult login()
        {
            var userSession = (User)Session["user"];

            if (userSession != null)
            {
                return RedirectToAction("Welcome", "Users", new { user = Session["user"] });
            }
            else
            {
                return View();
            }
        }
        public ActionResult logout()
        {
            Session["user"] = null;
            return RedirectToAction("SmartLogin", "Users");
        }
        [HttpPost]
        public ActionResult login(User user)
        {
            var userSM = UserLoggedInSmart(user);
            if (userSM != null)
            {
                ViewBag.message = "loggedin";
                ViewBag.triedOnce = "yes";


                Session["user"] = userSM;

                var userS = (User)Session["user"];
                ViewBag.employeeTitle = userS.Is_Online;

                return RedirectToAction("Welcome", "Users", new { username = userSM?.User_Name });
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }
        // GET: Users/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Welcome() {
            return View();
        }

        // POST: Users/Create.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Name,User_Password,Is_Online")] User user)
        {
            if(user is null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //TODO this wont get hit beacasue of the create empty view controller GET: Users/Create

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("SmartLogin");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_ID,User_Name,User_Password,Is_Online")] User user)
        {
            if (ModelState.IsValid)
            {
                db.SetModified(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchConstructor mm = new MatchConstructor();
            return View(mm.ConstructMatch(25, id.Value, false));
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
