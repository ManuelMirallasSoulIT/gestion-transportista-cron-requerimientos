using Andreani.ARQ.Core.Interface;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using FluentAssertions;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.PersonOperation.Commands.Create;

namespace Application.Test.PersonOperation.Commands.Create
{
    public class CreatePersonCommandTest
    {
        private readonly Mock<ITransactionalRepository> _repository;
        private readonly Mock<ILogger<CreatePersonCommandHandler>> _logger;
        private CreatePersonCommandHandler _handler;
        private CancellationToken _cancellationToken;
        public CreatePersonCommandTest()
        {
            // Arrange
            _repository = new Mock<ITransactionalRepository>();
            _logger = new Mock<ILogger<CreatePersonCommandHandler>>();
            _cancellationToken = CancellationToken.None;

            _handler = new CreatePersonCommandHandler(_repository.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_CreatePerson_Success()
        {
            // Arrange
            var request = new Fixture().Create<CreatePersonCommand>();
            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.Content.Message.Should().Be("Success");
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handler_CreatePerson_UpdateDatabaseException()
        {
            // Arrange
            var request = new Fixture().Create<CreatePersonCommand>();

            _repository.Setup(_ => _.SaveChangeAsync()).ThrowsAsync(new DbUpdateException());

            // Act
            // Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(request, _cancellationToken));
        }

    }
}
