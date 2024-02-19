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
    public class DepartmentControllerTests
    {
        [Fact]
        public void GetDepartments_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var mockContext = new Mock<UkukhulaContext>();
            var expectedDepartments = new List<Department>
            {
                new Department { DepartmentId = 1, DepartmentName = "ComputerScience" },
                new Department { DepartmentId = 2, DepartmentName = "GameDevelopment" },
                new Department { DepartmentId = 3, DepartmentName = "Engineering" }

            };
            var mockDbSet = MockDbSet(expectedDepartments);
            mockContext.Setup(c => c.Departments).Returns(mockDbSet.Object);
            var controller = new DepartmentController(mockContext.Object);

            // Act
            var result = controller.GetDepartments() as OkObjectResult;
            var actualDepartments = result?.Value as List<Department>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(actualDepartments);
            Assert.Equal(expectedDepartments.Count, actualDepartments!.Count);
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
