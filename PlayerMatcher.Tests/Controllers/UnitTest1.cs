using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using PlayerMatcher.Controllers;
using PlayerMatcher;
using System.Linq;
using System.Web.Mvc;

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
        public void Index_AnItemList_WithAListOfAllUsers()
        {
            // Arrange
            var mockdb = new Mock<PlayerMatcherEntities>();
            mockdb.SetupGet(repo => repo)
            var mockUsers = new Mock<User>();
//            mockUsers.As<IEnumerable<User>>();
//            mockUsers.Setup(user => user.ToList()).Returns(GetTestUsers().ToList());
            mockdb.Setup(repo => repo.Users.ToList())
                .Returns(GetTestUsers().ToList());
            var controller = new UsersController(mockdb.Object);

            // Act
            var result = (ViewResult)controller.Index();

            // Assert
            Assert.IsInstanceOf<User>(result);
            var users = (List<User>)result.Model;
            Assert.Equals(2, users.Count());
        }

        private IEnumerable<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User()
            {
                User_ID = 1,
                User_Name = "Test One"
            });
            users.Add(new User()
            {
                User_ID = 2,
                User_Name = "Test Two"
            });
            return users;
        }
    }
}
