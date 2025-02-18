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
    public async Task GivenHandler_WhenDistrictAndRegionAndDepartementDontExist_ThenAddDistrictRegionDepartementAndSousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock, 
            out var regionReadRepository, out var regionWriteRepositoryMock, 
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureWriteRepositoryMock, out var unitOfWorkMock);
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

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()), Times.Once);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()), Times.Once);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictExistsButRegionAndDepartementDontExist_ThenAddOnlyRegionAndDepartementAndSousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureWriteRepositoryMock, out var unitOfWorkMock);
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = 100
        };
        var district = new District { Nom = command.DistrictNom, Population = 100 };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Region));
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Departement));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()), 
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()), Times.Once);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictAndRegionExistButDepartementDoesntExist_ThenAddOnlyDepartementAndSousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureWriteRepositoryMock, out var unitOfWorkMock);
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = 100
        };
        var district = new District { Nom = command.DistrictNom, Population = 100 };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        var region = new Region { Nom = command.RegionNom, District = district, Population = 100 };
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Departement));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()),
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()), 
            Times.Never);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()), Times.Once);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenHandler_WhenDistrictAndRegionAndDepartementExist_ThenAddOnlySousPrefecture()
    {
        //Arrange
        var sut = MakeSut(out var districtReadRepository, out var districtWriteRepositoryMock,
            out var regionReadRepository, out var regionWriteRepositoryMock,
            out var departementReadRepository, out var departementWriteRepositoryMock,
            out var sousPrefectureWriteRepositoryMock, out var unitOfWorkMock);
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = "Abidjan",
            RegionNom = "Abidjan",
            DepartementNom = "Abidjan",
            SousprefectureNom = "Abobo",
            Population = 100
        };
        var district = new District { Nom = command.DistrictNom, Population = 100 };
        districtReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(district);
        var region = new Region { Nom = command.RegionNom, District = district, Population = 100 };
        regionReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        var departement = new Departement { Nom = command.DepartementNom, Region = region, Population = 100 };
        departementReadRepository.Setup(x => x.GetByNomAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(departement);

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        districtReadRepository.Verify(x => x.GetByNomAsync(command.DistrictNom, It.IsAny<CancellationToken>()), Times.Once);
        regionReadRepository.Verify(x => x.GetByNomAsync(command.RegionNom, It.IsAny<CancellationToken>()), Times.Once);
        departementReadRepository.Verify(x => x.GetByNomAsync(command.DepartementNom, It.IsAny<CancellationToken>()), Times.Once);
        districtWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<District>(d => d.Nom == command.DistrictNom), It.IsAny<CancellationToken>()),
            Times.Never);
        regionWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Region>(r => r.Nom == command.RegionNom), It.IsAny<CancellationToken>()),
            Times.Never);
        departementWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<Departement>(d => d.Nom == command.DepartementNom), It.IsAny<CancellationToken>()),
            Times.Never);
        sousPrefectureWriteRepositoryMock.Verify(x => x.AddAsync(It.Is<SousPrefecture>(s => s.Nom == command.SousprefectureNom), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static AjoutLigneSousPrefecture.Handler MakeSut(
        out Mock<IDistrictReadRepository> districtReadRepositoryMock,
        out Mock<IDistrictWriteRepository> districtWriteRepositoryMock,
        out Mock<IRegionReadRepository> regionReadRepositoryMock, 
        out Mock<IRegionWriteRepository> regionWriteRepositoryMock,
        out Mock<IDepartementReadRepository>  departementReadRepositoryMock,
        out Mock<IDepartementWriteRepository> departementWriteRepositoryMock,
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
            sousPrefectureWriteRepositoryMock.Object,
            unitOfWorkMock.Object);
    }
}
