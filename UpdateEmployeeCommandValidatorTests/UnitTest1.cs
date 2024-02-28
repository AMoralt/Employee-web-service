using Application.Commands.Employees;
using Application.Validators;

namespace UpdateEmployeeCommandValidatorTests;

public class UnitTest1
{
    [Theory]
    [InlineData("+1234567890123", true)] // Валидный номер
    [InlineData("123456", false)] // Невалидный номер
    public void Phone_Validation_Should_Validate_Correctly(string phone, bool isValid)
    {
        // Arrange
        var validator = new UpdateEmployeeCommandValidator();
        var command = new UpdateEmployeeCommand { Phone = phone };

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.Equal(isValid, result.IsValid);
    }
}