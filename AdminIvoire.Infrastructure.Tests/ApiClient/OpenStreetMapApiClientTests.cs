using AdminIvoire.Infrastructure.ApiClient;

namespace AdminIvoire.Infrastructure.Tests.ApiClient;

public class OpenStreetMapApiClientTests
{
    [Fact]
    public void GivenBuildOverpassQuery_WhenOnlySousPrefectureNom_ThenReturnValidQuery()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("[out:json][timeout:25];", result);
        Assert.Contains("node[\"place\"~\"^(village|hamlet|town)$\"]", result);
        Assert.Contains("way[\"place\"~\"^(village|hamlet|town)$\"]", result);
        Assert.Contains($"[\"addr:subdistrict\"~\"^{sousPrefectureNom}$\",i]", result);
        Assert.Contains($"[\"is_in\"~\"{sousPrefectureNom}\",i]", result);
        Assert.Contains("(4.3,-8.6,10.7,-2.5)", result);
        Assert.Contains("out body;", result);
        Assert.Contains("out skel qt;", result);
        Assert.DoesNotContain("addr:district", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenSousPrefectureNomAndDepartementNom_ThenReturnValidQueryWithDepartement()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";
        const string departementNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, departementNom, null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("[out:json][timeout:25];", result);
        Assert.Contains($"[\"addr:subdistrict\"~\"^{sousPrefectureNom}$\",i]", result);
        Assert.Contains($"[\"addr:district\"~\"^{departementNom}$\",i]", result);
        Assert.Contains("(4.3,-8.6,10.7,-2.5)", result);
        Assert.Contains("out body;", result);
        Assert.Contains("out skel qt;", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenAllParametersProvided_ThenReturnValidQuery()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";
        const string departementNom = "Abidjan";
        const string regionNom = "Lagunes";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, departementNom, regionNom);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("[out:json][timeout:25];", result);
        Assert.Contains($"[\"addr:subdistrict\"~\"^{sousPrefectureNom}$\",i]", result);
        Assert.Contains($"[\"addr:district\"~\"^{departementNom}$\",i]", result);
        Assert.Contains("(4.3,-8.6,10.7,-2.5)", result);
        Assert.Contains("out body;", result);
        Assert.Contains("out skel qt;", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenEmptyDepartementNom_ThenDoNotIncludeDepartementFilter()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";
        const string emptyDepartementNom = "";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, emptyDepartementNom, null);

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("addr:district", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenWhitespaceDepartementNom_ThenDoNotIncludeDepartementFilter()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";
        const string whitespaceDepartementNom = "   ";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, whitespaceDepartementNom, null);

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("addr:district", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenSousPrefectureNomWithSpecialCharacters_ThenEscapeSpecialCharacters()
    {
        // Arrange
        const string sousPrefectureNom = "Test\"With\\Special\nChars\rAnd\tTabs";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Vérifier que les caractères spéciaux sont échappés
        Assert.DoesNotContain("Test\"With", result);
        Assert.Contains("Test\\\"With", result);
        Assert.Contains("\\\\", result); // Backslash échappé
        Assert.Contains("\\n", result); // Newline échappé
        Assert.Contains("\\r", result); // Carriage return échappé
        Assert.Contains("\\t", result); // Tab échappé
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenDepartementNomWithSpecialCharacters_ThenEscapeSpecialCharacters()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";
        const string departementNom = "Test\"With\\Special\nChars";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, departementNom, null);

        // Assert
        Assert.NotNull(result);
        // Vérifier que les caractères spéciaux du département sont échappés
        Assert.DoesNotContain("Test\"With", result);
        Assert.Contains("Test\\\"With", result);
        Assert.Contains("\\\\", result);
        Assert.Contains("\\n", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenSousPrefectureNomContainsBackslash_ThenEscapeBackslash()
    {
        // Arrange
        const string sousPrefectureNom = "Test\\Backslash";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Le backslash doit être échappé en double backslash
        Assert.Contains("Test\\\\Backslash", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenSousPrefectureNomContainsQuotes_ThenEscapeQuotes()
    {
        // Arrange
        const string sousPrefectureNom = "Test\"Quotes\"";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Les guillemets doivent être échappés
        Assert.Contains("Test\\\"Quotes\\\"", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenQueryStructure_ThenContainsAllRequiredParts()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Vérifier la structure de base
        Assert.Contains(lines, line => line.Contains("[out:json][timeout:25];"));
        Assert.Contains(lines, line => line.Trim() == "(");
        Assert.Contains(lines, line => line.Trim() == ");");
        Assert.Contains(lines, line => line.Trim() == "out body;");
        Assert.Contains(lines, line => line.Trim() == ">;");
        Assert.Contains(lines, line => line.Trim() == "out skel qt;");
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenOnlySousPrefectureNom_ThenContainsFourSearchLines()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Devrait contenir 4 lignes de recherche (2 node + 2 way) pour addr:subdistrict et is_in
        var searchLines = lines.Where(line =>
            line.Contains("node[\"place\"") || line.Contains("way[\"place\"")).ToArray();
        Assert.Equal(4, searchLines.Length);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenDepartementNomProvided_ThenContainsSixSearchLines()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";
        const string departementNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, departementNom, null);

        // Assert
        Assert.NotNull(result);
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Devrait contenir 6 lignes de recherche (4 de base + 2 avec département)
        var searchLines = lines.Where(line =>
            line.Contains("node[\"place\"") || line.Contains("way[\"place\"")).ToArray();
        Assert.Equal(6, searchLines.Length);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenSousPrefectureNomWithAccents_ThenPreserveAccents()
    {
        // Arrange
        const string sousPrefectureNom = "Bouaké";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Les accents doivent être préservés (pas échappés)
        Assert.Contains("Bouaké", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenSousPrefectureNomIsEmpty_ThenReturnValidQuery()
    {
        // Arrange
        const string emptySousPrefectureNom = "";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(emptySousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("[out:json][timeout:25];", result);
        Assert.Contains("out body;", result);
        Assert.Contains("out skel qt;", result);
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenBoundingBox_ThenContainsCoteIvoireBbox()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Le bounding box de la Côte d'Ivoire doit être présent dans toutes les recherches
        var searchLines = result.Split('\n')
            .Where(line => line.Contains("node[\"place\"") || line.Contains("way[\"place\""))
            .ToArray();

        foreach (var line in searchLines)
        {
            Assert.Contains("(4.3,-8.6,10.7,-2.5)", line);
        }
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenPlaceFilter_ThenContainsVillageHamletTown()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Toutes les recherches doivent filtrer sur place=village|hamlet|town
        var searchLines = result.Split('\n')
            .Where(line => line.Contains("node[\"place\"") || line.Contains("way[\"place\""))
            .ToArray();

        foreach (var line in searchLines)
        {
            Assert.Contains("[\"place\"~\"^(village|hamlet|town)$\"]", line);
        }
    }

    [Fact]
    public void GivenBuildOverpassQuery_WhenCaseInsensitiveFlag_ThenContainsCaseInsensitiveFlag()
    {
        // Arrange
        const string sousPrefectureNom = "Abidjan";

        // Act
        var result = OpenStreetMapApiClient.BuildOverpassQuery(sousPrefectureNom, null, null);

        // Assert
        Assert.NotNull(result);
        // Toutes les recherches avec sous-préfecture doivent avoir le flag case-insensitive
        var searchLines = result.Split('\n')
            .Where(line => (line.Contains("addr:subdistrict") || line.Contains("is_in")) &&
                          (line.Contains("node[\"place\"") || line.Contains("way[\"place\"")))
            .ToArray();

        foreach (var line in searchLines)
        {
            Assert.Contains(",i]", line);
        }
    }
}

