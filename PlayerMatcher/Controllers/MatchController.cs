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
        public ActionResult SubsetPlayers(int? numPlayers)
        {
            if (numPlayers == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<User> userList = db.Users.ToList();
            List<User> newList = new List<User>();
            int n = 0;
            while (n < numPlayers.Value && n < userList.Count -1)
            {
                newList[n] = userList[n];
                n++;
            }
            JavaScriptResult jsRes = new JavaScriptResult();
            var jsonS = new JavaScriptSerializer();
            var jsonList = jsonS.Serialize(newList);
            jsRes.Script = jsonList;
            return jsRes;
        }

        // Incomplete
        // GET: Match/RandomMatch/5
        public ActionResult RandomMatch(int? numPlayers)
        {
            if (numPlayers == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List < User > userList = db.Users.ToList();
            List<User> matchPlayers = new List<User>();
            int n = 0;
            while (n < numPlayers)
            {
                matchPlayers[n] = userList[rng.Next(userList[0].User_ID, userList.Count+userList[0].User_ID)];
                n++;
            }

            return View(matchPlayers);
        }
    }

}
