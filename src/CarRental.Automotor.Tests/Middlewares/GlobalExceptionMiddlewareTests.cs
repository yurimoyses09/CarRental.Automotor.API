using CarRental.Automotor.Domain.Exceptions;
using CarRental.Automotor.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Xunit;

namespace CarRental.Automotor.Tests.Middlewares
{
    // Testa como cada tipo de exceção vira uma resposta HTTP (ProblemDetails).
    // Regra de negócio (BusinessException) vira 422 com a própria mensagem no title.
    public class GlobalExceptionMiddlewareTests
    {
        private static async Task<(int Status, JsonElement Body)> InvokeAsync(Exception exception)
        {
            var context = new DefaultHttpContext { TraceIdentifier = "trace-test" };
            context.Response.Body = new MemoryStream();

            var middleware = new GlobalExceptionMiddleware(
                _ => throw exception,
                NullLogger<GlobalExceptionMiddleware>.Instance);

            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var document = await JsonDocument.ParseAsync(context.Response.Body);

            return (context.Response.StatusCode, document.RootElement.Clone());
        }

        [Fact]
        public async Task BusinessException_ReturnsUnprocessableEntityWithMessage()
        {
            var (status, body) = await InvokeAsync(new BusinessException("This rental has already been paid."));

            Assert.Equal(422, status);
            Assert.Equal("This rental has already been paid.", body.GetProperty("title").GetString());
        }

        [Fact]
        public async Task KeyNotFoundException_ReturnsNotFound()
        {
            var (status, body) = await InvokeAsync(new KeyNotFoundException("Rental 99 was not found."));

            Assert.Equal(404, status);
            Assert.Equal("Resource not found.", body.GetProperty("title").GetString());
        }

        [Fact]
        public async Task ArgumentException_ReturnsBadRequest()
        {
            var (status, _) = await InvokeAsync(new ArgumentException("bad value"));

            Assert.Equal(400, status);
        }

        [Fact]
        public async Task UnexpectedException_ReturnsInternalErrorWithFriendlyMessageAndTraceId()
        {
            var (status, body) = await InvokeAsync(new InvalidOperationException("detalhe interno"));

            Assert.Equal(500, status);
            Assert.Equal("Ocorreu um erro inesperado no servidor. Tente novamente em instantes.",
                body.GetProperty("title").GetString());
            Assert.Equal("trace-test", body.GetProperty("traceId").GetString());
            // O detalhe técnico não deve vazar para o cliente.
            Assert.DoesNotContain("detalhe interno", body.ToString());
        }
    }
}
