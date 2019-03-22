using System;
using System.Collections.Generic;

namespace PlayerMatcher.Models
{
    public partial class Game
    {
        public Game()
        {
            GameSessions = new HashSet<GameSessions>();
            Rating = new HashSet<Rating>();
        }

        public int GameId { get; set; }
        public string GameName { get; set; }
        public int? MaxPlayerCount { get; set; }

        public virtual ICollection<GameSessions> GameSessions { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}
