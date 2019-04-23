using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using PlayerMatcher.Controllers;
using PlayerMatcher;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using FluentAssertions;
using System.Net;

namespace ControllerTests
{
    public class UserControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Create_AddsNewUserAndRedirects()
        {
            var mockSet = new Mock<DbSet<User>>();
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users)
                .Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);
            var user = new User() { User_ID = 1, User_Name = "Test One" };

            var redirect = controller.Create(user);

            Assert.IsInstanceOf<RedirectToRouteResult>(redirect);
            Assert.AreEqual(((RedirectToRouteResult)redirect).RouteValues["action"], "SmartLogin");
            mockSet.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            mockdb.Verify(x => x.SaveChanges());
        }

        [Test]
        public void Details_ErrorsWhenIdNotProvided()
        {
            var data = new List<User>().AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => data.FirstOrDefault(d => d.User_ID == (int)ids[0]));
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            var response = controller.Details(1) as HttpStatusCodeResult;

            response.Should().BeOfType<HttpNotFoundResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Details_ReturnsWhenIdFound()
        {
            var data = new List<User> {
                new User(){ User_ID = 1, User_Name = "Test One" },
            }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => data.FirstOrDefault(d => d.User_ID == (int)ids[0]));
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            var response = controller.Details(1);

            response.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<User>();
            response.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<User>().Which.User_ID.Should().Be(1);
            response.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<User>().Which.User_Name.Should().Be("Test One");
        }

        [Test]
        public void Index_AnItemList_WithAListOfAllUsers()
        {
            // Arrange
            var data = new List<User> {
                new User(){ User_ID = 1, User_Name = "Test One" },
                new User(){ User_ID = 2, User_Name = "Test Two" },
            }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            // Act
            var view = controller.Index() as ViewResult;
            var users = (List<User>)view.Model;

            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(users, typeof(User));
            Assert.AreEqual(2, users.Count());
        }

        [Test]
        public void Edit_EditAnItem()
        {
            // Arrange
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.SetModified(It.IsAny<User>())).Returns<object>(u => u);
            var controller = new UsersController(mockdb.Object);

            // Act
            var redirect = controller.Edit(new User() { User_ID = 1, User_Name = "Test One Mutated" });
            
            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(redirect);
            Assert.AreEqual(((RedirectToRouteResult)redirect).RouteValues["action"], "Index");
            mockdb.Verify(x => x.SetModified(It.Is<User>(u=>u.User_Name == "Test One Mutated" && u.User_ID == 1)), Times.Once);
            mockdb.Verify(x => x.SaveChanges());
        }

        [Test]
        public void Delete_NotFound()
        {
            // Arrange
            var data = new List<User> {
                new User(){ User_ID = 2, User_Name = "Test Two" },
            }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => data.FirstOrDefault(d => d.User_ID == (int)ids[0]));
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            // Act
            var response = controller.Delete(1) as HttpStatusCodeResult;

            response.Should().BeOfType<HttpNotFoundResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void Delete_Null()
        {
            // Arrange
            var controller = new UsersController();

            // Act
            var response = controller.Delete(null) as HttpStatusCodeResult;

            response.Should().BeOfType<HttpStatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public void Delete_ReturnView()
        {
            // Arrange
            var data = new List<User> {
                new User(){ User_ID = 1, User_Name = "Test One" },
            }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.User_ID == (int)ids[0]));
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            // Act
            var view = controller.Delete(1) as ViewResult;

            // Assert
            view.Should().BeOfType<ViewResult>().Which.Model.Should().Be(new User() { User_ID = 1, User_Name = "Test One" });
        }

        [Test]
        public void DeleteConfirmed_RemoveAnItem()
        {
            // Arrange
            var data = new List<User> {
                new User(){ User_ID = 1, User_Name = "Test One" },
                new User(){ User_ID = 2, User_Name = "Test Two" },
            }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.User_ID == (int)ids[0]));
            var mockdb = new Mock<UsersController.PlayerMatcherEntitiesExtended>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            // Act
            var redirect = controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(redirect);
            Assert.AreEqual(((RedirectToRouteResult)redirect).RouteValues["action"], "Index");
            mockSet.Verify(x => x.Remove(It.Is<User>(u => u.User_ID == 1 && u.User_Name == "Test One")), Times.Once);
            mockdb.Verify(x => x.SaveChanges());
        }
    }
}
