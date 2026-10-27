using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.Service;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;
using Moq;
using Xunit;

namespace CarRental.Automotor.Tests.Services
{
    public class AutomobileServiceTests
    {
        private readonly Mock<IAutomobileRepository> _repository = new();
        private readonly AutomobileService _service;

        public AutomobileServiceTests()
        {
            _service = new AutomobileService(_repository.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenPlateAlreadyExists_ThrowsBusinessExceptionAndDoesNotSave()
        {
            _repository.Setup(r => r.ExistsByPlateAsync("QRT1F92")).ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(
                new Automobile("Cruze", "2016", "QRT1F92", "Flex", "Prata")));

            _repository.Verify(r => r.CreateAsync(It.IsAny<Automobile>()), Times.Never);
        }
    }
}
