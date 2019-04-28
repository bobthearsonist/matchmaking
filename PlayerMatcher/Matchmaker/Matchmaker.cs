using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerMatcher.Matchmaker
{
    public class MatchConstructor
    {
        private class PlayerData
        {
            public User user;
            public int elo;
        }

        private PlayerMatcherEntities db;

        public MatchConstructor() : this(new PlayerMatcherEntities()) { }

        public MatchConstructor(PlayerMatcherEntities db)
        {
            this.db = db;
        }

        readonly int window = 12;        

        public List<User> ConstructMatch(int gameID, int numPlayers, bool UseBehaviorScore)
        {
            int numTaken = 0;
            int minElo = 0;
            int maxElo = 10000;
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

                try
                {
                    PlayerData initialPlayer = new PlayerData();
                    initialPlayer = GetInitialPlayer(gameID);
                    playersInMatch.Add(initialPlayer.user);
                    listOfElos.Add(initialPlayer.elo);
                    numTaken++;
                    minElo = listOfElos[0] - window;
                    if (minElo < 0) minElo = 0;
                    maxElo = listOfElos[0] + window;
                }
                catch (NoPlayersFoundException ex)
                {
                }

                IEnumerable<PlayerData> playersSelectedByQuery;

                if (UseBehaviorScore)
                {
                    var GetPlayersQuery =
                        from players in db.Users
                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                        where ranks.Game_ID == gameID && ranks.User_Rating > minElo && ranks.User_Rating < maxElo
                        orderby ranks.Behavior_Score ascending
                        select new PlayerData
                        {
                            user = players,
                            elo = ranks.User_Rating.GetValueOrDefault()
                        };
                    playersSelectedByQuery = GetPlayersQuery.Take(totalGamePlayers);

                }
                else
                {
                    var GetPlayersQuery =
                        from players in db.Users
                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                        where ranks.Game_ID == gameID && ranks.User_Rating > minElo && ranks.User_Rating < maxElo
                        orderby Guid.NewGuid()
                        select new PlayerData
                        {
                            user = players,
                            elo = ranks.User_Rating.GetValueOrDefault()
                        };
                        playersSelectedByQuery = GetPlayersQuery.Take(totalGamePlayers);
                }

                while (playersInMatch.Count < numToTake)
                {
                    foreach (var matchPlayer in playersSelectedByQuery)
                    {
                        if (!playersInMatch.Contains(matchPlayer.user) && numTaken < numToTake)
                        {
                            playersInMatch.Add(matchPlayer.user);
                            listOfElos.Add(matchPlayer.elo);
                            numTaken++;
                        }
                        if (numTaken == numToTake) break;
                    }                    
                    minElo = minElo - window;
                    maxElo = maxElo + window;
                    
                    if (minElo < 0) minElo = 0;
                }

                return playersInMatch;
            }
        }

        private PlayerData GetInitialPlayer(int gameID)
        {
            int minElo = 0;
            int maxElo = 10000;
            bool playerActuallyFound = false;
            PlayerData playerToBeReturned = new PlayerData();

            IEnumerable<PlayerData> playerSelectedByQuery;
            var InitialQuery =
                        from players in db.Users
                        join ranks in db.Ratings on players.User_ID equals ranks.User_ID
                        where ranks.Game_ID == gameID && ranks.User_Rating > minElo && ranks.User_Rating < maxElo
                        orderby Guid.NewGuid()
                        select new PlayerData
                        {
                            user = players,
                            elo = ranks.User_Rating.GetValueOrDefault()
                        };
            playerSelectedByQuery = InitialQuery.Take(1);

            foreach (var matchPlayer in playerSelectedByQuery)
            {
                playerToBeReturned = matchPlayer;
                playerActuallyFound = true;
            }
            if (playerActuallyFound)
            {
                return playerToBeReturned;
            }
            else throw new NoPlayersFoundException("No players found for game");
        }
    }
    public class NoPlayersFoundException : System.Exception
    {
        public NoPlayersFoundException() : base() { }
        public NoPlayersFoundException(string message) : base(message) { }
        public NoPlayersFoundException(string message, System.Exception inner) : base(message, inner) { }

        protected NoPlayersFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
