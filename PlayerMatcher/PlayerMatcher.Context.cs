﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PlayerMatcherEntities : DbContext
    {
        public PlayerMatcherEntities()
            : base("name=PlayerMatcherEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Game_Sessions> Game_Sessions { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Session_Users> Session_Users { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
