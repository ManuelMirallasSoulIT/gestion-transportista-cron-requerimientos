using AutoFixture;
using FluentAssertions;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.PersonOperation.Commands.Create;

namespace Application.Test.PersonOperation.Commands.Create
{
    public class CreatePersonValidationTest
    {
        
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Create<CreatePersonCommand>();
            var validator = new CreatePersonValidation();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();

        }

        [Theory]
        [InlineData("", "test")]
        [InlineData("test", "")]
        [InlineData(null, "test")]
        [InlineData("test",null)]
        [InlineData("","")]
        [InlineData(null,null)]
        public async Task Validation_WithPropertyIncorrect_IsValidFalse(string nombre, string apellido)
        {
            // Arrange
            var request = new CreatePersonCommand
            {
                Apellido = apellido,
                Nombre = nombre
            };
            var validator = new CreatePersonValidation();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
