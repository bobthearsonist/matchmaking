using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerMatcher.Matchmaker
{
    public class MatchConstructor
    {
        private int numTaken;        
        private int minElo;
        private int maxElo;
        private PlayerMatcherEntities db;

        public MatchConstructor() : this(new PlayerMatcherEntities()) {}

        public MatchConstructor(PlayerMatcherEntities db)
        {
            numTaken = 0;
            minElo = 0;
            maxElo = 10000;
            this.db = db;
        }

        
        public List<User> ConstructMatch(int gameID, int numPlayers)
        {
            try
            {
                int numToTake = numPlayers;
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
