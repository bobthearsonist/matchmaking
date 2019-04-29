using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PlayerMatcher.Tests
{
    public static class Mock
    {
        public static Mock<DbSet<T>> CreateMockSet<T>(IEnumerable<T> userData) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(userData.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(userData.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(userData.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            return mockSet;
        }
    }
}
