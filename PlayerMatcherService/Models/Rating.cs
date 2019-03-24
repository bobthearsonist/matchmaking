using System;
using System.Collections.Generic;

namespace PlayerMatcherService.Models
{
    public partial class Rating
    {
        public int UserRatingId { get; set; }
        public int? UserRating { get; set; }
        public int? UserId { get; set; }
        public int? GameId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Users User { get; set; }
    }
}
