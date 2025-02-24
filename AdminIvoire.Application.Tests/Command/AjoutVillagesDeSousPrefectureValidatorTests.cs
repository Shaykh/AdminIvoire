using AdminIvoire.Application.Command;

namespace AdminIvoire.Application.Tests.Command;

public class AjoutVillagesDeSousPrefectureValidatorTests
{
    [Fact]
    public void GivenValidator_WhenCommandIsValid_ThenValidationShouldPass()
    {
        // Arrange
        var validator = new AjoutVillagesDeSousPrefecture.Validator();
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.NewGuid(), ["Village1", "Village2"]);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void GivenValidator_WhenInvalidSousPrefectureId_ThenValidationShouldFail()
    {
        // Arrange
        var validator = new AjoutVillagesDeSousPrefecture.Validator();
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.Empty, ["Village1", "Village2"]);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenValidator_WhenEmptyVillages_ThenValidationShouldFail()
    {
        // Arrange
        var validator = new AjoutVillagesDeSousPrefecture.Validator();
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.NewGuid(), []);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenValidator_WhenEmptyVillage_ThenValidationShouldFail()
    {
        // Arrange
        var validator = new AjoutVillagesDeSousPrefecture.Validator();
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.NewGuid(), ["Village1", ""]);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }
}
