using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerMatcher.Matchmaker
{
    public class MatchConstructor
    {     
        private PlayerMatcherEntities db;

        public MatchConstructor() : this(new PlayerMatcherEntities()) {}

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

            if ( numPlayers < 0 )
            {
                throw new ArgumentOutOfRangeException("numPlayers should be a positive integer");
            }
            if ( gameID < 0 )
            {
                throw new ArgumentException("invalid game id");
            }

            try
            {
                int numToTake = numPlayers;
                List<User> playersInMatch = new List<User>();
                List<int> listOfElos = new List<int>();

                var totalGamePlayers = (from players in db.Users
                                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                                        where ranks.Game_ID == gameID
                                        select players).Count();

                if (totalGamePlayers < numToTake) numToTake = totalGamePlayers;

                while (playersInMatch.Count < numToTake)
                {
                    if (numTaken != 0) numToTake = numToTake - numTaken;

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

                    foreach (var matchPlayer in GetPlayersQuery.Take(numToTake))
                    {
                        playersInMatch.Add(matchPlayer.Player);
                        listOfElos.Add(matchPlayer.EloRating);
                        numTaken++;
                    }

                    if (numTaken == 1)
                    {
                        minElo = listOfElos[0] - window;
                        maxElo = listOfElos[0] + window;
                    }
                    else
                    {
                        minElo = minElo - window;
                        maxElo = maxElo - window;
                    }
                }

                return playersInMatch;
            }
            catch (Exception)
            // TODO as a ROT you dont want to catch all expcetions, for example this would still catch a seg fault, or ENOT system exception lets change the type here to catch a specific exception type that you were encountering  or remvoe the try catch entirely and let the aprent handle the exception
            {
                Console.WriteLine("Caught exception. Suspect that Elo rating was missing");
                throw;
            }
        }
    }    
}
