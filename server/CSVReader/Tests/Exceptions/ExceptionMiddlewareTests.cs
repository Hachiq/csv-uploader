using Core.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.ExceptionHandling
{
    public class ExceptionMiddlewareTests
    {
        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        private static async Task<string> GetResponseBodyAsync(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            return await reader.ReadToEndAsync();
        }

        [Fact]
        public async Task InvokeAsync_WhenNoException_ShouldCallNextAndNotChangeResponse()
        {
            // Arrange
            var logger = new Mock<ILogger<ExceptionMiddleware>>();
            var context = CreateHttpContext();

            var middleware = new ExceptionMiddleware(
                next: (ctx) => Task.CompletedTask,
                logger: logger.Object
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            Assert.Equal(0, context.Response.Body.Length); // no body written
        }

        [Fact]
        public async Task InvokeAsync_WhenJsonException_ShouldReturnBadRequest()
        {
            // Arrange
            var logger = new Mock<ILogger<ExceptionMiddleware>>();
            var context = CreateHttpContext();

            var middleware = new ExceptionMiddleware(
                next: (ctx) => throw new JsonException("Bad JSON"),
                logger: logger.Object
            );

            // Act
            await middleware.InvokeAsync(context);
            var body = await GetResponseBodyAsync(context);
            var result = JsonSerializer.Deserialize<HandleExceptionModel>(body);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result!.Code);
            Assert.Equal("Invalid JSON format.", result.Message);
            Assert.Contains("Bad JSON", result.Details);
        }

        [Fact]
        public async Task InvokeAsync_WhenUnhandledException_ShouldReturnInternalServerError()
        {
            // Arrange
            var logger = new Mock<ILogger<ExceptionMiddleware>>();
            var context = CreateHttpContext();

            var middleware = new ExceptionMiddleware(
                next: (ctx) => throw new Exception("Unexpected failure"),
                logger: logger.Object
            );

            // Act
            await middleware.InvokeAsync(context);
            var body = await GetResponseBodyAsync(context);
            var result = JsonSerializer.Deserialize<HandleExceptionModel>(body);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.InternalServerError, result!.Code);
            Assert.Equal("Internal Server Error. Please try again later.", result.Message);
            Assert.Contains("Unexpected failure", result.Details);
        }
    }
}
