using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using PlayerMatcher;

namespace PlayerMatcher.Controllers
{
    public class MatchController : Controller
    {
        private PlayerMatcherEntities db = new PlayerMatcherEntities();
        private Random rng = new Random();

        // GET: Match/AllPlayerMatch
        public ActionResult AllPlayerMatch()
        {
            List<User> userList = db.Users.ToList();
            JavaScriptResult jsRes = new JavaScriptResult();            
            var jsonS = new JavaScriptSerializer();
            var jsonList = jsonS.Serialize(userList);
            jsRes.Script = jsonList;
            return jsRes;
        }

        // Incomplete
        // GET: Match/SubsetPlayers
        public ActionResult SubsetPlayers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<User> userList = db.Users.Take(id.Value).ToList();
            JavaScriptResult jsRes = new JavaScriptResult();
            var jsonS = new JavaScriptSerializer();
            var jsonList = jsonS.Serialize(userList);
            jsRes.Script = jsonList;
            return jsRes;
        }

        // Incomplete
        // GET: Match/RandomMatch/5
        public ActionResult RandomMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<User> userList = db.Users.OrderBy(o => Guid.NewGuid()).Take(id.Value).ToList();
            JavaScriptResult jsRes = new JavaScriptResult();
            var jsonS = new JavaScriptSerializer();
            var jsonList = jsonS.Serialize(userList);
            jsRes.Script = jsonList;
            return jsRes;
        }
    }

}
