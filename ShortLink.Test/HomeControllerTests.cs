using Microsoft.AspNetCore.Mvc;
using Moq;
using ShortLink.Controllers;
using ShortLink.Interfaces;
using ShortLink.Models;
using Xunit;

namespace ShortLink.Test
{
    public class HomeControllerTests
    {
        [Fact]
        public void CreateUrlReturnsViewResultWithUrlModel()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Url url = new Url
            {
                LongUrl = "https://www.google.com/webhp?hl=ru&sa=X&ved=0ahUKEwjRrqyBwurkAhWDAxAIHYsVAeAQPAgH"
            };

            // Act
            IActionResult result = controller.Create(url);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(url, viewResult?.Model);
        }

        [Fact]
        public void DeleteUrlReturnsBadRequestResultWhenIdIsNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);

            // Act
            IActionResult result = controller.Delete(null);

            // Arrange
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EditUrlReturnsNotFoundResultWhenUrlNotFound()
        {
            // Arrange
            int testUrlId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetById(testUrlId))
                .Returns(null as Url);
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Edit(testUrlId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteUrlReturnsNotFoundResultWhenUrlNotFound()
        {
            // Arrange
            int testUrlId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetById(testUrlId))
                .Returns(null as Url);
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Delete(testUrlId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateNewUrlReturnsARedirectAndCreateUrl()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            Url newUrl = new Url
            {
                LongUrl = "https://www.google.com/webhp?hl=ru&sa=X&ved=0ahUKEwjRrqyBwurkAhWDAxAIHYsVAeAQPAgH"
            };

            // Act
            var result = controller.Create(newUrl);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Create(newUrl));
        }

    }
}
