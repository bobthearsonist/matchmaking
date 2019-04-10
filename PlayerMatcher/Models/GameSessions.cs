using System;
using System.Collections.Generic;

namespace PlayerMatcherService.Models
{
    public partial class GameSessions
    {
        public int GameSessionId { get; set; }
        public DateTime? SessionStart { get; set; }
        public DateTime? SessionEnd { get; set; }
        public int? GameId { get; set; }
        public int? UserId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Users User { get; set; }
    }
}
