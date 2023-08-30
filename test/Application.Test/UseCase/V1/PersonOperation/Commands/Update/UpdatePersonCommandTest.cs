using Andreani.ARQ.Core.Interface;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.PersonOperation.Commands.Update;
using gestion_transportista_cron_requerimientos.Domain.Entities;

namespace Application.Test.PersonOperation.Commands.Update
{
    public class UpdatePersonCommandTest
    {
        private readonly Mock<ITransactionalRepository >_repository;
        private readonly Mock<IReadOnlyQuery >_query;
        private readonly Mock<ILogger<UpdatePersonHandler>> _logger;

        private readonly UpdatePersonHandler _handler;
        private CancellationToken _cancellationToken;

        public UpdatePersonCommandTest()
        {
            // Arrange
            _repository = new Mock<ITransactionalRepository>();
            _query = new Mock<IReadOnlyQuery>();
            _logger = new Mock<ILogger<UpdatePersonHandler>>();
            _cancellationToken = CancellationToken.None;

            _handler = new UpdatePersonHandler(_repository.Object, _query.Object, _logger.Object);
        }
        [Fact]
        public async Task Handler_UpdatePerson_Success()
        {
            // Arrange
            var request = new Fixture().Create<UpdatePersonCommand>();
            var person = new Fixture().Create<Person>();
            _query.Setup(_ => _.GetByIdAsync<Person>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(person);

            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Handler_UpdatePerson_PersonNotExist()
        {
            // Arrange
            var request = new Fixture().Create<UpdatePersonCommand>();
            _query.Setup(_ => _.GetByIdAsync<Person>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Person)null);

            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_UpdatePerson_ThrowUpdateDatabase()
        {
            // Arrange
            var request = new Fixture().Create<UpdatePersonCommand>();
            var person = new Fixture().Create<Person>();
            _query.Setup(_ => _.GetByIdAsync<Person>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(person);
            _repository.Setup(_ => _.SaveChangeAsync()).ThrowsAsync(new DbUpdateException());

            // Act
            // Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(request, _cancellationToken));

        }
    }
}
