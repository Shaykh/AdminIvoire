using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Tests.Entite;

public class SousPrefectureTests
{
    private static SousPrefecture MakeSut()
    {
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        return new SousPrefecture { Nom = "SousPrefecture", Departement = departement };
    }

    [Fact]
    public void GivenAddVillage_WhenVillageAlreadyExists_ThenReturnFalse()
    {
        // Arrange
        var sut = MakeSut();
        var village = new Village { Nom = "Village", SousPrefecture = sut };
        sut.Villages.Add(village);

        // Act
        var result = sut.AddVillage(village);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GivenAddVillage_WhenVillageIsAdded_ThenReturnTrue()
    {
        // Arrange
        var sut = MakeSut();
        var village = new Village { Nom = "Village", SousPrefecture = sut };

        // Act
        var result = sut.AddVillage(village);

        // Assert
        Assert.True(result);
    }
}