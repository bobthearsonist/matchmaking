using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerMatcher.Matchmaker
{
    public class MatchConstructor
    {
        private PlayerMatcherEntities db;

        public MatchConstructor() : this(new PlayerMatcherEntities()) { }

        public MatchConstructor(PlayerMatcherEntities db)
        {
            this.db = db;
        }

        readonly int window = 12;

        public List<User> ConstructMatch(int gameID, int numPlayers)
        {
            int numTaken = 0;
            int minElo = 0;
            int maxElo = 10000;
            int maxRuns = 100;
            int runs = 0;

            if (numPlayers < 0)
            {
                throw new ArgumentOutOfRangeException("numPlayers should be a positive integer");
            }
            if (gameID < 0)
            {
                throw new ArgumentException("invalid game id");
            }
            {
                int numToTake = numPlayers;
                int numForQuery = 1;
                List<User> playersInMatch = new List<User>();
                List<int> listOfElos = new List<int>();

                var totalGamePlayers = (from players in db.Users
                                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                                        where ranks.Game_ID == gameID
                                        select players).Count();

                if (totalGamePlayers < numToTake) numToTake = totalGamePlayers;

                while (playersInMatch.Count < numToTake && runs < maxRuns)
                {
                    if (numTaken != 0) numForQuery = numToTake - numTaken;
                    runs++;

                    var GetPlayersQuery =
                        from players in db.Users
                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                        where ranks.Game_ID == gameID && ranks.User_Rating > minElo && ranks.User_Rating < maxElo
                        orderby Guid.NewGuid()
                        select new
                        {
                            Player = players,
                            EloRating = ranks.User_Rating.GetValueOrDefault()
                        };                   

                    foreach (var matchPlayer in GetPlayersQuery.Take(numForQuery))
                    {
                        if (!playersInMatch.Contains(matchPlayer.Player))
                        {
                            playersInMatch.Add(matchPlayer.Player);
                            listOfElos.Add(matchPlayer.EloRating);
                            numTaken++;
                        }
                    }

                    if (runs == 1)
                    {
                        minElo = listOfElos[0] - window;
                        maxElo = listOfElos[0] + window;
                    }
                    else if (runs % 3 == 0)
                    {
                        minElo = minElo - window * 2;
                        maxElo = maxElo + window * 2;
                    }
                    else
                    {
                        minElo = minElo - window;
                        maxElo = maxElo - window;
                    }
                    if (minElo < 0) minElo = 0;                    
                }

                return playersInMatch;

            }
        }
    }
}
