using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UkukhulaAPI.Controllers;
using UkukhulaAPI.Data;
using UkukhulaAPI.Data.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace UkukhulaAPI.Tests.Controllers
{
    public class RaceControllerTests
    {
        [Fact]
        public void GetRaces_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var mockContext = new Mock<UkukhulaContext>();
            var expectedRaces = new List<Race>
            {
                new Race { RaceId = 1, RaceName = "African" },
                new Race { RaceId = 2, RaceName = "Colored" },
                new Race { RaceId = 2, RaceName = "Indian" }

                // Add more mock data as needed
            };
            var mockDbSet = MockDbSet(expectedRaces);
            mockContext.Setup(c => c.Races).Returns(mockDbSet.Object);
            var controller = new RaceController(mockContext.Object);

            // Act
            var result = controller.GetRaces() as OkObjectResult;
            var actualRaces = result?.Value as List<Race>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(actualRaces);
            Assert.Equal(expectedRaces.Count, actualRaces!.Count);
        }


        private Mock<DbSet<T>> MockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            return mockSet;
        }
    }
}
