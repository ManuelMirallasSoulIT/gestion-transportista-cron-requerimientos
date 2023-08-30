using Andreani.ARQ.Core.Interface;
using AutoFixture;
using FluentAssertions;
using Moq;
using System.Net;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.PersonOperation.Queries.GetList;
using gestion_transportista_cron_requerimientos.Domain.Dtos;

namespace Application.Test.PersonOperation.Queries.GetList
{
    public class ListPersonTest
    {
        private readonly Mock<IReadOnlyQuery> _query;
        private ListPersonHandler _handler;
        private CancellationToken _cancellationToken;
        public ListPersonTest()
        {
            _query = new Mock<IReadOnlyQuery>();
            _cancellationToken = CancellationToken.None;
            _handler = new ListPersonHandler(_query.Object);
        }

        [Fact]
        public async Task Handle_GetListUser_Success()
        {
            // Arrange
            var request = new ListPerson();
            var resonse = new Fixture().CreateMany<PersonDto>();
            _query.Setup(_ => _.GetAllAsync<PersonDto>(It.IsAny<string>()))
                .ReturnsAsync(resonse);
            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.Content.Should().Equal(resonse);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_GetListUser_ThrowException()
        {
            // Arrange
            var request = new ListPerson();
            _query.Setup(_ => _.GetAllAsync<PersonDto>(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, _cancellationToken));

        }
    }
}
