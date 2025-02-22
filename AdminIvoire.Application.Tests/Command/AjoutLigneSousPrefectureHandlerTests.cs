using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Command;

public class AjoutLigneSousPrefectureHandlerTests
{
    [Fact]
    public async Task GivenHandler_WhenCommand_ThenCommit()
    {
        //Arrange
        var sut = MakeSut(out var districtFactoryMock, out var regionFactoryMock, out var departementFactoryMock, out var sousPrefectureFactoryMock, out var unitOfWorkMock);
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = 100
        };
        var district = new District { Nom = command.DistrictNom };
        districtFactoryMock.Setup(x => x.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        var region = new Region { Nom = command.RegionNom, District = district };
        regionFactoryMock.Setup(x => x.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<District>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        var departement = new Departement { Nom = command.DepartementNom, Region = region };
        departementFactoryMock.Setup(x => x.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Region>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(departement);
        var sousPrefecture = new SousPrefecture { Nom = command.SousprefectureNom, Departement = departement };
        sousPrefectureFactoryMock.Setup(x => x.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Departement>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sousPrefecture);

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtFactoryMock.Verify(x => x.GetOrCreateAsync(command.DistrictNom, command.Population, CancellationToken.None), Times.Once);
        regionFactoryMock.Verify(x => x.GetOrCreateAsync(command.RegionNom, command.Population, district, CancellationToken.None), Times.Once);
        departementFactoryMock.Verify(x => x.GetOrCreateAsync(command.DepartementNom, command.Population, region, CancellationToken.None), Times.Once);
        sousPrefectureFactoryMock.Verify(x => x.GetOrCreateAsync(command.SousprefectureNom, command.Population, departement, CancellationToken.None), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static AjoutLigneSousPrefecture.Handler MakeSut(
        out Mock<IDistrictFactory> districtFactoryMock,
        out Mock<IRegionFactory> regionFactoryMock, 
        out Mock<IDepartementFactory>  departementFactoryMock,
        out Mock<ISousPrefectureFactory> sousPrefectureFactoryMock,
        out Mock<IUnitOfWork> unitOfWorkMock)
    {
        var logger = new Mock<ILogger<AjoutLigneSousPrefecture.Handler>>();
        districtFactoryMock = new();
        regionFactoryMock = new();
        departementFactoryMock = new();
        sousPrefectureFactoryMock = new();
        unitOfWorkMock = new();
        return new AjoutLigneSousPrefecture.Handler(
            logger.Object,
            districtFactoryMock.Object,
            regionFactoryMock.Object,
            departementFactoryMock.Object,
            sousPrefectureFactoryMock.Object,
            unitOfWorkMock.Object);
    }
}
