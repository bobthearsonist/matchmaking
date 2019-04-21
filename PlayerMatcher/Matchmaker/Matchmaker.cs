using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayerMatcher.Matchmaker
{
    /*public class PlayerData
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

    }*/

    public class Matchmaker
    {
        public Matchmaker()
        {

        }

        private PlayerMatcherEntities db = new PlayerMatcherEntities();
        public List<User> ConstructMatch(int gameID, int numPlayers)
        {
            try
            {
                int numTaken = 0;
                int numToTake = 1;
                int minElo = 0;
                int maxElo = 10000;
                List<User> playersInMatch = new List<User>();
                List<int> listOfElos = new List<int>();

                while (numTaken != numPlayers)
                {
                    if (numTaken != 0)
                    {
                        numToTake = numPlayers - numTaken;
                    }
                    var GetPlayersQuery =
                        from players in db.Users
                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                        where ranks.Game_ID == gameID && ranks.User_Rating > minElo && ranks.User_Rating < maxElo
                        orderby ranks.User_Rating ascending
                        select new
                        {
                            Player = players,
                            EloRating = ranks.User_Rating.GetValueOrDefault()
                        };
                    foreach (var matchPlayer in GetPlayersQuery)
                    {
                        playersInMatch.Add(matchPlayer.Player);
                        listOfElos.Add(matchPlayer.EloRating);
                        numTaken++;
                        if (playersInMatch.Count == numToTake)
                        {
                            break;
                        }
                    }
                    if (numTaken == 1)
                    {
                        minElo = listOfElos[0] - 12;
                        maxElo = listOfElos[0] + 12;
                    }
                    else
                    {
                        minElo = minElo - 12;
                        maxElo = maxElo - 12;
                    }
                }
                return playersInMatch;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception. Suspect that Elo rating was missing");
                return null;
            }
        }
    }    
}
