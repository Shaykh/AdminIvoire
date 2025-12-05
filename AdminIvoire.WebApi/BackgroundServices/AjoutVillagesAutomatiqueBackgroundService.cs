using AdminIvoire.Application.Command;
using AdminIvoire.Application.Parametrage;
using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Repository.Read;
using MediatR;

namespace AdminIvoire.WebApi.BackgroundServices;

/// <summary>
/// Service en arrière-plan pour ajouter automatiquement les villages de toutes les sous-préfectures
/// depuis des sources web valides
/// </summary>
public class AjoutVillagesAutomatiqueBackgroundService(
    ILogger<AjoutVillagesAutomatiqueBackgroundService> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Début exécution du service d'ajout automatique des villages");

        await AjouterVillagesPourToutesSousPrefecturesAsync(stoppingToken);

        logger.LogInformation("Fin exécution du service d'ajout automatique des villages");
    }

    /// <summary>
    /// Ajoute automatiquement les villages pour toutes les sous-préfectures depuis des sources web
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation pour arrêter l'opération</param>
    public async Task AjouterVillagesPourToutesSousPrefecturesAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();
        var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);

        // Vérifier si le service a déjà été exécuté
        if (await parametrageRepository.GetParametrageAsync(parametrageKey) is not null)
        {
            logger.LogInformation("L'ajout automatique des villages a déjà été effectué.");
            return;
        }

        var sousPrefectureReadRepository = scope.ServiceProvider.GetRequiredService<ISousPrefectureReadRepository>();
        var villageWebSourceService = scope.ServiceProvider.GetRequiredService<IVillageWebSourceService>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        // Récupérer toutes les sous-préfectures
        var sousPrefectures = await sousPrefectureReadRepository.GetAllAsync(stoppingToken);
        logger.LogInformation("Traitement de {Count} sous-préfectures", sousPrefectures.Count);

        var totalVillagesAjoutes = 0;
        var sousPrefecturesTraitees = 0;
        var erreurs = 0;

        foreach (var sousPrefecture in sousPrefectures)
        {
            try
            {
                logger.LogInformation("Traitement de la sous-préfecture {SousPrefectureNom} (ID: {Id})",
                    sousPrefecture.Nom, sousPrefecture.Id);

                // Récupérer les villages depuis la source web
                var villages = await villageWebSourceService.GetVillagesAsync(
                    sousPrefecture.Nom,
                    sousPrefecture.Departement?.Nom,
                    sousPrefecture.Departement?.Region?.Nom,
                    stoppingToken);

                if (villages.Count == 0)
                {
                    logger.LogWarning("Aucun village trouvé pour la sous-préfecture {SousPrefectureNom}", sousPrefecture.Nom);
                    continue;
                }

                logger.LogInformation("Récupération de {Count} villages pour {SousPrefectureNom}",
                    villages.Count, sousPrefecture.Nom);

                // Ajouter les villages en utilisant la commande existante
                var command = new AjoutVillagesDeSousPrefecture.Command(sousPrefecture.Id, villages.ToArray());
                await sender.Send(command, stoppingToken);

                totalVillagesAjoutes += villages.Count;
                sousPrefecturesTraitees++;

                logger.LogInformation("Ajout réussi de {Count} villages pour {SousPrefectureNom}",
                    villages.Count, sousPrefecture.Nom);

                // Ajouter un délai pour éviter de surcharger les APIs externes
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
            catch (Exception ex)
            {
                erreurs++;
                logger.LogError(ex, "Erreur lors du traitement de la sous-préfecture {SousPrefectureNom} (ID: {Id})",
                    sousPrefecture.Nom, sousPrefecture.Id);
                // Continuer avec la prochaine sous-préfecture même en cas d'erreur
            }
        }

        logger.LogInformation(
            "Traitement terminé : {SousPrefecturesTraitees} sous-préfectures traitées, {TotalVillagesAjoutes} villages ajoutés, {Erreurs} erreurs",
            sousPrefecturesTraitees, totalVillagesAjoutes, erreurs);

        // Marquer le service comme exécuté
        await parametrageRepository.SetParametrageAsync(
            new ParametrageEntity
            {
                Key = parametrageKey,
                Value = DateTime.Now.ToString()
            });
    }
}

