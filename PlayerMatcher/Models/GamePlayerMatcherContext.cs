using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
namespace PlayerMatcher.Models
{
    public partial class GamePlayerMatcherContext : DbContext
    {
        public GamePlayerMatcherContext()
        {
        }

        public GamePlayerMatcherContext(DbContextOptions<GamePlayerMatcherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameSessions> GameSessions { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("Game_ID");

                entity.Property(e => e.GameName)
                    .HasColumnName("Game_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaxPlayerCount).HasColumnName("Max_Player_Count");
            });

            modelBuilder.Entity<GameSessions>(entity =>
            {
                entity.HasKey(e => e.GameSessionId)
                    .HasName("PK__Game_Ses__95A581309179863C");

                entity.ToTable("Game_Sessions");

                entity.Property(e => e.GameSessionId).HasColumnName("Game_Session_ID");

                entity.Property(e => e.GameId).HasColumnName("Game_ID");

                entity.Property(e => e.SessionEnd)
                    .HasColumnName("Session_End")
                    .HasColumnType("datetime");

                entity.Property(e => e.SessionStart)
                    .HasColumnName("Session_Start")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameSessions)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Game_Sess__Game___34C8D9D1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameSessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Game_Sess__User___33D4B598");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => e.UserRatingId)
                    .HasName("PK__Rating__75451B757AC32280");

                entity.Property(e => e.UserRatingId).HasColumnName("User_Rating_ID");

                entity.Property(e => e.GameId).HasColumnName("Game_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.UserRating).HasColumnName("User_Rating");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Rating__Game_ID__30F848ED");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Rating__User_ID__300424B4");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__206D9190457C9878");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.IsOnline)
                    .HasColumnName("Is_Online")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserName)
                    .HasColumnName("User_Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasColumnName("User_Password")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
        }
    }
}
