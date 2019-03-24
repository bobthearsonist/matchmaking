using System;
using System.Collections.Generic;

namespace PlayerMatcherService.Models
{
    public partial class Users
    {
        public Users()
        {
            GameSessions = new HashSet<GameSessions>();
            Rating = new HashSet<Rating>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool? IsOnline { get; set; }

        public virtual ICollection<GameSessions> GameSessions { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}
