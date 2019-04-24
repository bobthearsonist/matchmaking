using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Data.Entity;
using PlayerMatcher.Matchmaker;
using FluentAssertions;

namespace PlayerMatcher.Tests.Matchmaker
{
    public class MatchmakerTests
    {
        [Test]
        public void ConstructMatch_AListOfUsersIsResturned()
        {
            // Arrange
            var userData = new List<User> {
                new User(){ User_ID = 1, User_Name = "One" },
                new User(){ User_ID = 2, User_Name = "Two" },
            }.AsQueryable();
            var mockSetUsers = new Mock<DbSet<User>>();
            mockSetUsers.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockSetUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockSetUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockSetUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            var ratingsData = new List<Rating> {
                new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 0 },
                new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 0 }
            }.AsQueryable();
            var mockSetRatings = new Mock<DbSet<Rating>>();
            mockSetRatings.As<IQueryable<Rating>>().Setup(m => m.Provider).Returns(ratingsData.Provider);
            mockSetRatings.As<IQueryable<Rating>>().Setup(m => m.Expression).Returns(ratingsData.Expression);
            mockSetRatings.As<IQueryable<Rating>>().Setup(m => m.ElementType).Returns(ratingsData.ElementType);
            mockSetRatings.As<IQueryable<Rating>>().Setup(m => m.GetEnumerator()).Returns(ratingsData.GetEnumerator());

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);
            
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 2);

            match.Should().AllBeOfType<User>().And.HaveCount(2);
            match.Should().ContainEquivalentOf(
                new User(){ User_ID = 1, User_Name = "One" }
            );
            match.Should().AllBeOfType<User>().And.ContainEquivalentOf(
                new User() { User_ID = 2, User_Name = "Two" }
            );
        }
    }
}
