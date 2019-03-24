using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;

using PlayerMatcherService.Models;
using PlayerMatcherService.Controllers;


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
        public async Task Index_AnItemList_WithAListOfAllUsers()
        {
            // Arrange
            var mockRepo = new Mock<GamePlayerMatcherContext>();
            mockRepo.Setup(repo => repo.ToListAsync())
                .ReturnsAsync(GetTestUsers());
            var controller = new UserController(mockRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var users = Assert.IsType<IEnumerable<Users>>(result);
            Assert.Equal(2, users.Count());
        }

        private IEnumerable<Users> GetTestUsers()
        {
            var users = new List<Users>();
            users.Add(new Users()
            {
                UserId = 1,
                UserName = "Test One"
            });
            users.Add(new Users()
            {
                UserId = 2,
                UserName = "Test Two"
            });
            return users;
        }
    }
}