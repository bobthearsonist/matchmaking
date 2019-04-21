using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayerMatcher.Matchmaker
{
    public class PlayerData
    {
        private int playerRating;
        private int userID;
        private List<int> behaviorAttributes;

        public PlayerData(int id, int rating)
        {
            this.userID = id;
            this.playerRating = rating;
            this.behaviorAttributes = new List<int>();
        }
        public PlayerData(int id, int rating, List<int> attributes)
        {
            this.userID = id;
            this.playerRating = rating;
            this.behaviorAttributes = attributes;
        }

    }

    public class Matchmaker
    {
        private PlayerMatcherEntities db = new PlayerMatcherEntities();

        public List<PlayerData> ConstructMatch(int gameID, int numPlayers)
        {
            int numTaken = 0; 

            return null;
        }
    }

    
}
