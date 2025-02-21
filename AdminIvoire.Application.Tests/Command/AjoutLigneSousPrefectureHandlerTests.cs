using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Command;

public class AjoutLigneSousPrefectureHandlerTests
{
    [Fact]
    public async Task GivenHandler_WhenDistrictAndRegionAndDepartementAndSouSprefectureDontExist_ThenAddDistrictRegionDepartementAndSousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock, 
            out var regionReadRepository, out var regionWriteRepositoryMock, 
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureReadRepositoryMock, out var sousPrefectureWriteRepositoryMock, 
            out var unitOfWorkMock);
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = 100
        };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(District));
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Region));
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Departement));
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(SousPrefecture));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByNomAsync(command.SousprefectureNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()), Times.Once);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()), Times.Once);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictExistsButRegionAndDepartementAndSouSprefectureDontExist_ThenAddOnlyRegionAndDepartementAndSousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureReadRepositoryMock, out var sousPrefectureWriteRepositoryMock, 
            out var unitOfWorkMock);
        const int initialPopulation = 100;
        const int linePopulation = 100;
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = linePopulation
        };
        var district = new District { Nom = command.DistrictNom, Population = initialPopulation };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Region));
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Departement));
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(SousPrefecture));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByNomAsync(command.SousprefectureNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()), 
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()), Times.Once);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(initialPopulation + linePopulation, district.Population);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictAndRegionExistButDepartementAndSouSprefectureDontExist_ThenAddOnlyDepartementAndSousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureReadRepositoryMock, out var sousPrefectureWriteRepositoryMock, 
            out var unitOfWorkMock);
        const int initialPopulation = 100;
        const int linePopulation = 100;
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = linePopulation
        };
        var district = new District { Nom = command.DistrictNom, Population = initialPopulation };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        var region = new Region { Nom = command.RegionNom, District = district, Population = initialPopulation };
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Departement));
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(SousPrefecture));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByNomAsync(command.SousprefectureNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()),
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()), 
            Times.Never);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(initialPopulation + linePopulation, district.Population);
        Assert.Equal(initialPopulation + linePopulation, region.Population);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictAndRegionAndDepartementExistButSouSprefectureDoesNotExist_ThenAddOnlySousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureReadRepositoryMock, out var sousPrefectureWriteRepositoryMock, 
            out var unitOfWorkMock);
        const int initialPopulation = 100;
        const int linePopulation = 100;
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = linePopulation
        };
        var district = new District { Nom = command.DistrictNom, Population = initialPopulation };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        var region = new Region { Nom = command.RegionNom, District = district, Population = initialPopulation };
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        var departement = new Departement { Nom = command.DepartementNom, Region = region, Population = initialPopulation };
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(departement);
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(SousPrefecture));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByNomAsync(command.SousprefectureNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()),
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()),
            Times.Never);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()),
            Times.Never);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(initialPopulation + linePopulation, district.Population);
        Assert.Equal(initialPopulation + linePopulation, region.Population);
        Assert.Equal(initialPopulation + linePopulation, departement.Population);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictAndRegionAndDepartementAndSouSprefectureDoNotExist_ThenAddPopulationToLocalites()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureReadRepositoryMock, out var sousPrefectureWriteRepositoryMock,
            out var unitOfWorkMock);
        const int initialPopulation = 100;
        const int linePopulation = 100;
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = linePopulation
        };
        var district = new District { Nom = command.DistrictNom, Population = initialPopulation };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        var region = new Region { Nom = command.RegionNom, District = district, Population = initialPopulation };
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        var departement = new Departement { Nom = command.DepartementNom, Region = region, Population = initialPopulation };
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(departement);
        var sousPrefecture = new SousPrefecture { Nom = command.SousprefectureNom, Departement = departement, Population = initialPopulation };
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sousPrefecture);

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByNomAsync(command.SousprefectureNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()),
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()),
            Times.Never);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()),
            Times.Never);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()),
            Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(initialPopulation+linePopulation, district.Population);
        Assert.Equal(initialPopulation + linePopulation, region.Population);
        Assert.Equal(initialPopulation + linePopulation, departement.Population);
        Assert.Equal(initialPopulation + linePopulation, sousPrefecture.Population);
    }

    private static AjoutLigneSousPrefecture.Handler MakeSut(
        out Mock<IDistrictReadRepository> districtReadRepositoryMock,
        out Mock<IDistrictWriteRepository> districtWriteRepositoryMock,
        out Mock<IRegionReadRepository> regionReadRepositoryMock, 
        out Mock<IRegionWriteRepository> regionWriteRepositoryMock,
        out Mock<IDepartementReadRepository>  departementReadRepositoryMock,
        out Mock<IDepartementWriteRepository> departementWriteRepositoryMock,
        out Mock<ISousPrefectureReadRepository> sousPrefectureReadRepositoryMock,
        out Mock<ISousPrefectureWriteRepository> sousPrefectureWriteRepositoryMock,
        out Mock<IUnitOfWork> unitOfWorkMock)
    {
        districtReadRepositoryMock = new Mock<IDistrictReadRepository>();
        districtWriteRepositoryMock = new Mock<IDistrictWriteRepository>();
        regionReadRepositoryMock = new Mock<IRegionReadRepository>();
        departementReadRepositoryMock = new Mock<IDepartementReadRepository>();
        var logger = new Mock<ILogger<AjoutLigneSousPrefecture.Handler>>();
        regionWriteRepositoryMock = new Mock<IRegionWriteRepository>();
        departementWriteRepositoryMock = new Mock<IDepartementWriteRepository>();
        sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureWriteRepositoryMock = new Mock<ISousPrefectureWriteRepository>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        return new AjoutLigneSousPrefecture.Handler(
            logger.Object,
            districtReadRepositoryMock.Object,
            districtWriteRepositoryMock.Object,
            regionReadRepositoryMock.Object,
            regionWriteRepositoryMock.Object,
            departementReadRepositoryMock.Object,
            departementWriteRepositoryMock.Object,
            sousPrefectureReadRepositoryMock.Object,
            sousPrefectureWriteRepositoryMock.Object,
            unitOfWorkMock.Object);
    }
}
