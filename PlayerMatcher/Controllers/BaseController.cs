using PlayerMatcher.Models;
using System.Web.Mvc;

namespace PlayerMatcher.Controllers
{
    public class BaseController : Controller
    {
        protected PlayerMatcherEntitiesExtended db;

        public BaseController() : this(new PlayerMatcherEntitiesExtended()) { }

        public BaseController(PlayerMatcherEntities db)
        {
            this.@db = @db as PlayerMatcherEntitiesExtended;
        }
    }
}