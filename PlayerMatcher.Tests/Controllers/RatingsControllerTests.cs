using FluentAssertions;
using Moq;
using NUnit.Framework;
using PlayerMatcher.Controllers;
using PlayerMatcher.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PlayerMatcher.Tests.Controllers
{
    public class RatingsControllerTests
    {
        [Test]
        public void Index_AnItemList_WithAListOfAllRatings()
        {
            // Arrange
            var user1 = new User() { User_ID = 1, User_Name = "One" };
            var userData = new List<User> {
                user1
            };
            var game1 = new Game() { Game_ID = 1, Game_Name = "One" };
            var gameData = new List<Game> {
                new Game(){ Game_ID = 1, Game_Name = "One" },
                new Game(){ Game_ID = 2, Game_Name = "Two" },
            };
            var ratingData = new List<Rating> {
                new Rating(){ User_Rating_ID = 1, User_ID = 1, User_Rating = 1, Game_ID = 1, User = user1, Game = game1},
                new Rating(){ User_Rating_ID = 2, User_ID = 1, User_Rating = 1, Game_ID = 2 }
            };

            var mockdb = new Mock<PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(Mock.CreateMockSet(userData).Object);
            mockdb.Setup(db => db.Games).Returns(Mock.CreateMockSet(gameData).Object);
            mockdb.Setup(db => db.Ratings).Returns(Mock.CreateMockSet(ratingData).Object);
            var controller = new RatingsController(mockdb.Object);

            // Act
            var view = controller.Index() as ViewResult;
            var ratings = (List<Rating>)view.Model;

            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(ratings, typeof(Rating));
            Assert.AreEqual(2, ratings.Count());
        }

        [Test]
        public void Details_ErrorsWhenIdNotProvided()
        {
            var controller = new RatingsController();

            var response = controller.Details(null);

            response.Should().BeOfType<HttpStatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public void Details_ErrorsWhenIdNotFound()
        {
            var data = new List<Rating>();
            var mockSet = Mock.CreateMockSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => data.FirstOrDefault(d => d.User_Rating_ID == (int)ids[0]));
            var mockdb = new Mock<PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Ratings).Returns(mockSet.Object);
            var controller = new RatingsController(mockdb.Object);

            var response = controller.Details(1) as HttpStatusCodeResult;

            response.Should().BeOfType<HttpNotFoundResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Edit_ErrorsWhenIdNotProvided()
        {
            var controller = new RatingsController();

            var response = controller.Details(null);

            response.Should().BeOfType<HttpStatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public void Edit_ErrorsWhenIdNotFound()
        {
            var data = new List<Rating>();
            var mockSet = Mock.CreateMockSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => data.FirstOrDefault(d => d.User_Rating_ID == (int)ids[0]));
            var mockdb = new Mock<PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Ratings).Returns(mockSet.Object);
            var controller = new RatingsController(mockdb.Object);

            var response = controller.Edit(1) as HttpStatusCodeResult;

            response.Should().BeOfType<HttpNotFoundResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Delete_ErrorsWhenIdNotProvided()
        {
            var controller = new RatingsController();

            var response = controller.Delete(null);

            response.Should().BeOfType<HttpStatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public void Delete_ErrorsWhenIdNotFound()
        {
            var data = new List<Rating>();
            var mockSet = Mock.CreateMockSet(data);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => data.FirstOrDefault(d => d.User_Rating_ID == (int)ids[0]));
            var mockdb = new Mock<PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Ratings).Returns(mockSet.Object);
            var controller = new RatingsController(mockdb.Object);

            var response = controller.Edit(1) as HttpStatusCodeResult;

            response.Should().BeOfType<HttpNotFoundResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteConfirmed_RemoveAnItem()
        {
            // Arrange
            var ratingData = new List<Rating> {
                new Rating(){ User_Rating_ID = 1, User_ID = 1, User_Rating = 1, Game_ID = 1, User = new User() { User_ID = 1, User_Name = "One" }, Game = new Game() { Game_ID = 1, Game_Name = "One" }
                },
                new Rating(){ User_Rating_ID = 2, User_ID = 1, User_Rating = 1, Game_ID = 2 }
            };

            var mockSet = Mock.CreateMockSet(ratingData);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => ratingData.FirstOrDefault(d => d.User_Rating_ID == (int)ids[0]));
            var mockdb = new Mock<PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Ratings).Returns(Mock.CreateMockSet(ratingData).Object);

            var controller = new RatingsController(mockdb.Object);

            // Act
            var redirect = controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(redirect);
            Assert.AreEqual(((RedirectToRouteResult)redirect).RouteValues["action"], "Index");
            mockSet.Verify(x => x.Remove(It.IsAny<Rating>()),Times.AtMostOnce);
            mockdb.Verify(x => x.SaveChanges());
        }
    }
}
