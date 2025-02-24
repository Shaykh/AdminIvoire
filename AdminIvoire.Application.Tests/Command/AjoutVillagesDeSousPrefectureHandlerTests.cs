using AdminIvoire.Application.ApiClient;
using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Command;

public class AjoutVillagesDeSousPrefectureHandlerTests
{
    private static AjoutVillagesDeSousPrefecture.Handler MakeSut(out Mock<ISousPrefectureReadRepository> sousPrefectureReadRepositoryMock,
        out Mock<IVillageWriteRepository> villageWriteRepositoryMock,
        out Mock<IGeocodingApiClient> geocodingApiClientMock,
        out Mock<IUnitOfWork> unitOfWorkMock)
    {
        var loggerMock = new Mock<ILogger<AjoutVillagesDeSousPrefecture.Handler>>();
        var validator = new AjoutVillagesDeSousPrefecture.Validator();
        sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        villageWriteRepositoryMock = new Mock<IVillageWriteRepository>();
        geocodingApiClientMock = new Mock<IGeocodingApiClient>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        return new AjoutVillagesDeSousPrefecture.Handler(loggerMock.Object,
            validator,
            sousPrefectureReadRepositoryMock.Object,
            villageWriteRepositoryMock.Object,
            geocodingApiClientMock.Object,
            unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GivenHandle_WhenCommandIsInvalid_ThenThrowException()
    {
        // Arrange
        var sut = MakeSut(out var sousPrefectureReadRepositoryMock, out var villageWriteRepositoryMock, out var geocodingApiClientMock, out var unitOfWorkMock);
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.Empty, []);

        // Act
        async Task act() => await sut.Handle(command, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(act);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),  
            Times.Never); 
        villageWriteRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Village>(), It.IsAny<CancellationToken>()),
            Times.Never);
        geocodingApiClientMock.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task GivenHandle_WhenValidCommand_ThenAddVillages()
    {
        // Arrange
        var sut = MakeSut(out var sousPrefectureReadRepositoryMock, out var villageWriteRepositoryMock, out var geocodingApiClientMock, out var unitOfWorkMock);
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.NewGuid(), ["Village1", "Village2"]);
        var sousPrefecture = new SousPrefecture
        {
            Id = command.SousPrefectureId,
            Nom = "SousPrefecture",
           Departement = new Departement
           {
               Id = Guid.NewGuid(),
               Nom = "Departement",
               Region = new Region
               {
                   Id = Guid.NewGuid(),
                   Nom = "Region",
                   District = new District
                   {
                       Id = Guid.NewGuid(),
                       Nom = "District"
                   }
               }
           }
        };
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sousPrefecture);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(2, sousPrefecture.Villages.Count);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
        villageWriteRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Village>(), It.IsAny<CancellationToken>()),
            Times.Exactly(command.Villages.Length));
        geocodingApiClientMock.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(command.Villages.Length));
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task GivenHandle_WhenVillageAlreadyInSousPrefecture_ThenDoNotAddVillage()
    {
        // Arrange
        var sut = MakeSut(out var sousPrefectureReadRepositoryMock, out var villageWriteRepositoryMock, out var geocodingApiClientMock, out var unitOfWorkMock);
        var command = new AjoutVillagesDeSousPrefecture.Command(Guid.NewGuid(), ["Village1"]);
        var sousPrefecture = new SousPrefecture
        {
            Id = command.SousPrefectureId,
            Nom = "SousPrefecture",
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Departement",
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Region",
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "District"
                    }
                }
            }
        };
        sousPrefecture.AddVillage(new Village
        {
            Nom = "Village1",
            SousPrefectureId = sousPrefecture.Id,
            SousPrefecture = sousPrefecture
        });
        sousPrefectureReadRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sousPrefecture);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.Single(sousPrefecture.Villages);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
        villageWriteRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Village>(), It.IsAny<CancellationToken>()),
            Times.Never);
        geocodingApiClientMock.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
