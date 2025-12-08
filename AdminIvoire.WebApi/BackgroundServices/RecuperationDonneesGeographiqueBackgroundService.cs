using AdminIvoire.Application.Command;
using AdminIvoire.Application.Parametrage;
using MediatR;

namespace AdminIvoire.WebApi.BackgroundServices;

/// <summary>
/// Service en arrière-plan pour récupérer les coordonnées géographiques des départements et sous-préfectures
/// </summary>
public class RecuperationDonneesGeographiqueBackgroundService(ILogger<RecuperationDonneesGeographiqueBackgroundService> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    /// <summary>
    /// Exécute le service de récupération des coordonnées géographiques
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter le service</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Début exécution du service de lecture des données de localité");

        await RecupererDonneesGeoLocaliteAsync(stoppingToken);

        logger.LogInformation("Fin exécution du service de lecture des données de localité");
    }

    /// <summary>
    /// Récupère les coordonnées géographiques pour toutes les localités (départements et sous-préfectures)
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter l'opération</param>
    public async Task RecupererDonneesGeoLocaliteAsync(CancellationToken stoppingToken)
    {
        // Créer un scope séparé pour chaque opération afin d'avoir des DbContext indépendants
        // Chaque scope utilise le DbContextFactory en arrière-plan pour créer un contexte frais
        await RecupererDonneesGeoDeDepartementsAsync(stoppingToken);
        await RecupererDonneesGeoDeSousPrefecturesAsync(stoppingToken);
    }

    /// <summary>
    /// Récupère les coordonnées géographiques de toutes les sous-préfectures
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter l'opération</param>
    private async Task RecupererDonneesGeoDeSousPrefecturesAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var parametrageKey = nameof(RecuperationDonneesGeographiqueBackgroundService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures);
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();

        if ((await parametrageRepository.GetParametrageAsync(parametrageKey)) is not null)
        {
            logger.LogInformation("La recuperation des coordonnées des sous-préfectures a déja été effectuée.");
            return;
        }

        // Le handler MediatR créera son propre scope avec un contexte frais depuis la factory
        var recuperationReussie = await sender.Send(new RecupererCoordonneesGeographiquesDeSousPrefectures.Command(), stoppingToken);
        if (recuperationReussie)
        {
            await parametrageRepository.SetParametrageAsync(
                new ParametrageEntity { Key = parametrageKey, Value = DateTime.Now.ToString() }
            );
        }
    }

    /// <summary>
    /// Récupère les coordonnées géographiques de tous les départements
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter l'opération</param>
    private async Task RecupererDonneesGeoDeDepartementsAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var parametrageKey = nameof(RecuperationDonneesGeographiqueBackgroundService) + nameof(RecupererCoordonneesGeographiquesDeDepartements);
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();

        if ((await parametrageRepository.GetParametrageAsync(parametrageKey)) is not null)
        {
            logger.LogInformation("La recuperation des coordonnées des départements a déja été effectuée.");
            return;
        }

        // Le handler MediatR créera son propre scope avec un contexte frais depuis la factory
        var recuperationReussie = await sender.Send(new RecupererCoordonneesGeographiquesDeDepartements.Command(), stoppingToken);
        if (recuperationReussie)
        {
            await parametrageRepository.SetParametrageAsync(
                new ParametrageEntity { Key = parametrageKey, Value = DateTime.Now.ToString() }
            );
        }
    }

}