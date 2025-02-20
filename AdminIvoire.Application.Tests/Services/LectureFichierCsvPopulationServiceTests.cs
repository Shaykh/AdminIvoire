using AdminIvoire.Application.Command;
using AdminIvoire.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Services;

public class LectureFichierCsvPopulationServiceTests
{
    private static LectureFichierCsvPopulationService MakeSut()
    {
        return new LectureFichierCsvPopulationService(Mock.Of<ILogger<LectureFichierCsvPopulationService>>(),
            Mock.Of<ISender>());
    }

    [Theory]
    [InlineData("ZANZAN ,BOUNKANI ,TEHINI,GOGO,HOMME,12202")]
    [InlineData("ABIDJAN,ABIDJAN,ABIDJAN,BROFODOUME,FEMME,9734")]
    public void GivenGetCommandFromLine_WhenValidLine_ThenReturnCommand(string line)
    {
        // Arrange
        var sut = MakeSut();

        // Act
        var result = sut.GetCommandFromLine(line);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AjoutLigneSousPrefecture.Command>(result, exactMatch: true);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("FEMME")]
    [InlineData("ZANZAN ,BOUNKANI")]
    [InlineData("GOGO,HOMME,12202")]
    [InlineData("ABIDJAN,ABIDJAN,BROFODOUME,FEMME")]
    [InlineData("ZANZAN ,BOUNKANI ,TEHINI,GOGO,MENAGE,12202")]
    [InlineData("DISTRICT,REGION,DEPARTEMENT,SOUS-PREFECTURE OU COMMUNE,CATEGORIE,EFFECTIF")]
    public void GivenGetCommandFromLine_WhenInvalidLine_ThenReturnNull(string line)
    {
        // Arrange
        var sut = MakeSut();

        // Act
        var result = sut.GetCommandFromLine(line);

        // Assert
        Assert.Null(result);
    }
}
