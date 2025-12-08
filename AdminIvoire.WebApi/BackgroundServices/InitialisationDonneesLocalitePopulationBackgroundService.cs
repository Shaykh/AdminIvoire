using AdminIvoire.Application.Parametrage;
using AdminIvoire.Application.Services;
using AdminIvoire.Infrastructure.Configuration;

namespace AdminIvoire.WebApi.BackgroundServices;

/// <summary>
/// Service en arrière-plan pour initialiser les données de localité et de population depuis un fichier CSV
/// </summary>
public class InitialisationDonneesLocalitePopulationBackgroundService(ILogger<InitialisationDonneesLocalitePopulationBackgroundService> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IWebHostEnvironment webHostEnvironment) : BackgroundService
{
    /// <summary>
    /// Exécute le service d'initialisation des données de localité
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter le service</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Début exécution du service de lecture des données de localité depuis le fichier csv");

        await LireDonneesLocaliteAsync(stoppingToken);

        logger.LogInformation("Fin exécution du service de lecture des données de localité depuis le fichier csv");
    }

    /// <summary>
    /// Lit les données de localité depuis le fichier CSV de population
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter l'opération</param>
    /// <exception cref="ConfigurationException">Lancée si le chemin du fichier n'est pas configuré</exception>
    public async Task LireDonneesLocaliteAsync(CancellationToken stoppingToken)
    {
        var cheminFichier = configuration["FichierPopulation"];
        if (string.IsNullOrWhiteSpace(cheminFichier))
        {
            throw new ConfigurationException("Aucune valeur de chemin du fichier de population n'est configurée");
        }
        var cheminFichierPhysique = GetPhysicalFullPath(cheminFichier);

        // Utilisation d'un scope pour cette opération
        // Le contexte créé par le scope utilise le DbContextFactory en arrière-plan
        using var scope = serviceProvider.CreateScope();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();

        if ((await parametrageRepository.GetParametrageAsync(nameof(InitialisationDonneesLocalitePopulationBackgroundService))) is not null)
        {
            logger.LogInformation("La lecture des données de localité depuis le fichier csv a déja  été effectuée.");
            return;
        }

        var lectureFichierCsvPopulationService = scope.ServiceProvider.GetRequiredService<ILectureFichierCsvPopulationService>();
        await lectureFichierCsvPopulationService.LireFichierCsvPopulationAsync(cheminFichierPhysique, stoppingToken);
        await parametrageRepository.SetParametrageAsync(
            new ParametrageEntity { Key = nameof(InitialisationDonneesLocalitePopulationBackgroundService), Value = DateTime.Now.ToString() }
        );
    }

    /// <summary>
    /// Convertit un chemin relatif en chemin physique complet
    /// </summary>
    /// <param name="relativePath">Le chemin relatif du fichier</param>
    /// <returns>Le chemin physique complet du fichier</returns>
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
