using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using PlayerMatcher.Matchmaker;
using FluentAssertions;

namespace PlayerMatcher.Tests.Matchmaker
{
    public class MatchmakerTests
    {
        [Test]
        public void ConstructMatch_AListOfUsersIsReturned()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One" },
                    new User(){ User_ID = 2, User_Name = "Two" }
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 0 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 0 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 2);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(2);
            match.Should().ContainEquivalentOf( new User(){ User_ID = 1, User_Name = "One" } );
            match.Should().ContainEquivalentOf( new User() { User_ID = 2, User_Name = "Two" } );
        }

        [Test]
        public void ConstructMatch_AMatchOfTwoIsCreatedFromThree()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One" },
                    new User(){ User_ID = 2, User_Name = "Two" },
                    new User(){ User_ID = 3, User_Name = "Three"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 0 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 0 },
                    new Rating(){ User_ID = 3, Game_ID = 1, User_Rating = 0 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 2);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(2);
        }

        [Test]
        public void ConstructMatch_GameTypeIsUsedProperly()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One" },
                    new User(){ User_ID = 2, User_Name = "Two" },
                    new User(){ User_ID = 3, User_Name = "Three"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 0 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 0 },
                    new Rating(){ User_ID = 3, Game_ID = 2, User_Rating = 0 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 3);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(2);
        }

        [Test]
        public void ConstructMatch_CanCallMultipleTimes()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One" },
                    new User(){ User_ID = 2, User_Name = "Two" },
                    new User(){ User_ID = 3, User_Name = "Three"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 12 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 12 },
                    new Rating(){ User_ID = 3, Game_ID = 1, User_Rating = 0 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match1 = new MatchConstructor(mockdb.Object).ConstructMatch(1, 2);
            var match2 = new MatchConstructor(mockdb.Object).ConstructMatch(1, 2);

            // Assert
            match1.Should().AllBeOfType<User>().And.HaveCount(2);
            match2.Should().AllBeOfType<User>().And.HaveCount(2);
            match1.Should().ContainEquivalentOf( new User() { User_ID = 1, User_Name = "One" } );
            match1.Should().ContainEquivalentOf( new User() { User_ID = 2, User_Name = "Two" } );
            match2.Should().ContainEquivalentOf( new User() { User_ID = 1, User_Name = "One" } );
            match2.Should().ContainEquivalentOf( new User() { User_ID = 2, User_Name = "Two" } );
        }
    }
}
