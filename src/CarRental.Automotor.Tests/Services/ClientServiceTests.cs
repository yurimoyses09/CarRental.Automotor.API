using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.Service;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;
using Moq;
using Xunit;

namespace CarRental.Automotor.Tests.Services
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _repository = new();
        private readonly ClientService _service;

        public ClientServiceTests()
        {
            _service = new ClientService(_repository.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenDocumentAlreadyExists_ThrowsBusinessExceptionAndDoesNotSave()
        {
            _repository.Setup(r => r.ExistsByDocumentAsync("123")).ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(new Client("Ana", 30, "123")));

            _repository.Verify(r => r.CreateAsync(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenDocumentIsNew_SavesClient()
        {
            _repository.Setup(r => r.ExistsByDocumentAsync("456")).ReturnsAsync(false);
            _repository.Setup(r => r.CreateAsync(It.IsAny<Client>())).ReturnsAsync((Client c) => c);

            var result = await _service.CreateAsync(new Client("Bruno", 40, "456"));

            Assert.Equal("456", result.Document);
            _repository.Verify(r => r.CreateAsync(It.IsAny<Client>()), Times.Once);
        }
    }
}
