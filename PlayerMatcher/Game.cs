//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlayerMatcher
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game
    {
        public Game()
        {
            this.Game_Sessions = new HashSet<Game_Sessions>();
            this.Ratings = new HashSet<Rating>();
        }
    
        public int Game_ID { get; set; }
        public string Game_Name { get; set; }
        public Nullable<int> Max_Player_Count { get; set; }
    
        public virtual ICollection<Game_Sessions> Game_Sessions { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
