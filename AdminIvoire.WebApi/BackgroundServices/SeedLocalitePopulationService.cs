using AdminIvoire.Application.Parametrage;
using AdminIvoire.Application.Services;
using AdminIvoire.Infrastructure.Configuration;

namespace AdminIvoire.WebApi.BackgroundServices;

public class SeedLocalitePopulationService(ILogger<SeedLocalitePopulationService> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IWebHostEnvironment webHostEnvironment) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Début exécution du service de lecture des données de localité depuis le fichier csv");

        await ReadLocaliteDataAsync(stoppingToken);

        logger.LogInformation("Fin exécution du service de lecture des données de localité depuis le fichier csv");
    }

    public async Task ReadLocaliteDataAsync(CancellationToken stoppingToken)
    {
        var cheminFichier = configuration["FichierPopulation"];
        if (string.IsNullOrWhiteSpace(cheminFichier))
        {
            throw new ConfigurationException("Aucune valeur de chemin du fichier de population n'est configurée");
        }
        var cheminFichierPhysique = GetPhysicalFullPath(cheminFichier);
        using var scope = serviceProvider.CreateScope();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();
        if ((await parametrageRepository.GetParametrageAsync(nameof(SeedLocalitePopulationService))) is not null)
        {
            logger.LogInformation("La lecture des données de localité depuis le fichier csv a déja  été effectuée.");
            return;
        }
        var lectureFichierCsvPopulationService = scope.ServiceProvider.GetRequiredService<ILectureFichierCsvPopulationService>();
        await lectureFichierCsvPopulationService.LireFichierCsvPopulationAsync(cheminFichierPhysique, stoppingToken);
        await parametrageRepository.SetParametrageAsync(
            new ParametrageEntity { Key = nameof(SeedLocalitePopulationService), Value = DateTime.Now.ToString() }
            );
    }

    private string GetPhysicalFullPath(string relativePath)
    {
        logger.LogInformation("Début récuperation chemin physique {RelativePath}", relativePath);
        relativePath = relativePath.Replace(@"/", Path.DirectorySeparatorChar.ToString());
        relativePath = relativePath.Replace(@"\", Path.DirectorySeparatorChar.ToString());
        var webRootPath = webHostEnvironment.WebRootPath;
        var finalPath = Path.Combine(webRootPath, relativePath);
        logger.LogInformation("Fin récuperation chemin physique {FinalPath}", finalPath);
        return finalPath;
    }

}
