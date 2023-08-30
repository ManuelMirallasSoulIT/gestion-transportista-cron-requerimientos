using AutoFixture;
using FluentAssertions;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.PersonOperation.Commands.Update;

namespace Application.Test.PersonOperation.Commands.Update
{
    public  class UpdatePersonValidationTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Create<UpdatePersonCommand>();
            request.PersonId = "1";
            var validator = new UpdatePersonValidation();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();

        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Validation_WithPropertyIncorrect_IsValidFalse(string personId, string nombre, string apellido)
        {
            // Arrange
            var request = new UpdatePersonCommand
            {
                PersonId = personId,
                Apellido = apellido,
                Nombre = nombre
            };
            var validator = new UpdatePersonValidation();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { "", "", "" },
                new object[] { null, null, null },
                new object[] { null, null, "Test" },
                new object[] { null, "Test", "Test" },
                new object[] { null, "Test", null },
                new object[] { "Test", "Test", null },
                new object[] { "Test", null, null }, 
                new object[] { "Test", null, "Test" }, 
                new object[] { "Test", "", "" }, 
                new object[] { "Test", "Test", "" }, 
                new object[] { "", "Test", "Test" }, 
                new object[] { "Test", "", "Test" }, 

            };
    }
}
