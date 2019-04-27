using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using PlayerMatcher.Matchmaker;
using FluentAssertions;
using System.Linq;
using System;

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
                    new User(){ User_ID = 1, User_Name = "One"},
                    new User(){ User_ID = 2, User_Name = "Two"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 1 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 1 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 2);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(2);
            match.Should().ContainEquivalentOf( new User(){ User_ID = 1, User_Name = "One"} );
            match.Should().ContainEquivalentOf( new User() { User_ID = 2, User_Name = "Two"} );
        }

        [Test]
        public void ConstructMatch_AMatchOfTwoIsCreatedFromThree()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One"},
                    new User(){ User_ID = 2, User_Name = "Two"},
                    new User(){ User_ID = 3, User_Name = "Three"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 1 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 1 },
                    new Rating(){ User_ID = 3, Game_ID = 1, User_Rating = 1 }
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
                    new User(){ User_ID = 1, User_Name = "One"},
                    new User(){ User_ID = 2, User_Name = "Two"},
                    new User(){ User_ID = 3, User_Name = "Three"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 1 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 1 },
                    new Rating(){ User_ID = 3, Game_ID = 2, User_Rating = 1 }
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
        //[Ignore("this should work once we update the algorithm")]
        public void ConstructMatch_GroupsBySkill()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One"},
                    new User(){ User_ID = 2, User_Name = "Two"},
                    new User(){ User_ID = 3, User_Name = "Three"},
                    new User(){ User_ID = 4, User_Name = "Four"},
                    new User(){ User_ID = 5, User_Name = "Five"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 12 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 10 },
                    new Rating(){ User_ID = 3, Game_ID = 1, User_Rating = 100 },
                    new Rating(){ User_ID = 4, Game_ID = 1, User_Rating = 3 },
                    new Rating(){ User_ID = 5, Game_ID = 1, User_Rating = 0 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 3);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(3);
            match.Select(x => x.User_ID).Should().Contain(1);
            match.Select(x => x.User_ID).Should().Contain(2);
            //match.Select(x => x.User_ID).Should().Contain(4);
            //match.Select(x => x.User_ID).Should().NotContain(3);
            match.Select(x => x.User_ID).Should().NotContain(5);
        }

        [Test]
        public void ConstructMatch_GroupsByRankDescending()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One" },
                    new User(){ User_ID = 2, User_Name = "Two" },
                    new User(){ User_ID = 3, User_Name = "Three"},
                    new User(){ User_ID = 4, User_Name = "Four"},
                    new User(){ User_ID = 5, User_Name = "Five"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 12 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 10 },
                    new Rating(){ User_ID = 3, Game_ID = 1, User_Rating = 100 },
                    new Rating(){ User_ID = 4, Game_ID = 1, User_Rating = 3 },
                    new Rating(){ User_ID = 5, Game_ID = 1, User_Rating = 0 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 3);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(3);
            match.Select(x => x.User_ID).Should().Contain(1);
            match.Select(x => x.User_ID).Should().Contain(2);
            //match.Select(x => x.User_ID).Should().Contain(3);
        }

        [Test]
        public void ConstructMatch_IsIdempotent()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One"},
                    new User(){ User_ID = 2, User_Name = "Two"},
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
            match1.Select(x => x.User_ID).Should().Contain( 1);
            match1.Select(x => x.User_ID).Should().Contain( 2 );
            match2.Select(x => x.User_ID).Should().Contain( 1 );
            match2.Select(x => x.User_ID).Should().Contain( 2 );
        }

        [Test]
        public void ConstructMatch_WorksWithNoPlayers()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(new List<User>());
            var mockSetRatings = Mock.CreateMockSet(new List<Rating>());

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(0, 0);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(0);
        }

        [Test]
        public void ConstructMatch_WorksWhenNoneFound()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(new List<User>());
            var mockSetRatings = Mock.CreateMockSet(new List<Rating>());

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 4);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(0);
        }

        [Test]
        public void ConstructMatch_AssertsOnInvalidGameId()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(new List<User>());
            var mockSetRatings = Mock.CreateMockSet(new List<Rating>());
            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);
            var matcher = new MatchConstructor(mockdb.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => matcher.ConstructMatch(-1,4));
        }

        [Test]
        public void ConstructMatch_AssertsOnInvalidNumberOfPlayers()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(new List<User>());
            var mockSetRatings = Mock.CreateMockSet(new List<Rating>());
            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);
            var matcher = new MatchConstructor(mockdb.Object);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => matcher.ConstructMatch(1, -4));
        }

        [Test]
        public void ConstructMatch_NoDuplicatePlayers()
        {
            // Arrange
            var mockSetUsers = Mock.CreateMockSet(
                new List<User> {
                    new User(){ User_ID = 1, User_Name = "One"},
                    new User(){ User_ID = 2, User_Name = "Two"},
                    new User(){ User_ID = 3, User_Name = "Three"}
                }
            );

            var mockSetRatings = Mock.CreateMockSet(
                new List<Rating> {
                    new Rating(){ User_ID = 1, Game_ID = 1, User_Rating = 100 },
                    new Rating(){ User_ID = 2, Game_ID = 1, User_Rating = 120 },
                    new Rating(){ User_ID = 3, Game_ID = 1, User_Rating = 1 }
                }
            );

            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSetUsers.Object);
            mockdb.Setup(db => db.Ratings).Returns(mockSetRatings.Object);

            // Act
            var match = new MatchConstructor(mockdb.Object).ConstructMatch(1, 3);

            // Assert
            match.Should().AllBeOfType<User>().And.HaveCount(3);
            match.Select(x => x.User_ID).Should().Contain(1);
            match.Select(x => x.User_ID).Should().Contain(2);
            match.Select(x => x.User_ID).Should().Contain(3);
        }
    }
}
