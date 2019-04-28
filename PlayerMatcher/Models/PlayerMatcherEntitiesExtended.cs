using System.Data.Entity;

namespace PlayerMatcher.Models
{
    public class PlayerMatcherEntitiesExtended : PlayerMatcherEntities
    {
        public virtual object SetModified(object entity) //this method is not otherwise mockable
        {
            Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}