using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using PlayerMatcher.Controllers;
using PlayerMatcher;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void Create_AddNewUser()
        {
            var mockSet = new Mock<DbSet<User>>();
            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users)
                .Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);
            var user = new User() { User_ID = 1, User_Name = "Test One" };

            var view = controller.Create(user);
            var newUser = ((ViewResult)view).Model;

            //Assert.IsInstanceOf<System.Web.Mvc.RedirectToRouteResult>(redirect);
            Assert.AreEqual(newUser, user);
            mockSet.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            mockdb.Verify(x => x.SaveChanges());
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
            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.Setup(db => db.Users).Returns(mockSet.Object);
            var controller = new UsersController(mockdb.Object);

            // Act
            var view = (ViewResult)controller.Index();
            var users = (List<User>)view.Model;

            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(users, typeof(User));
            Assert.AreEqual(2, users.Count());
        }
    }
}
