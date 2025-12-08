using AdminIvoire.Application.Command;
using AdminIvoire.Application.Parametrage;
using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        // Vérifier si le service a déjà été exécuté
        // Utilisation d'un scope pour cette vérification rapide
        using (var checkScope = serviceProvider.CreateScope())
        {
            var parametrageRepository = checkScope.ServiceProvider.GetRequiredService<IParametrageRepository>();
            var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);

            if (await parametrageRepository.GetParametrageAsync(parametrageKey) is not null)
            {
                logger.LogInformation("L'ajout automatique des villages a déjà été effectué.");
                return;
            }
        }

        // Récupérer toutes les sous-préfectures dans un scope dédié
        // Le contexte créé par le scope sera automatiquement disposé après cette opération
        IList<Domain.Entite.SousPrefecture> sousPrefectures;
        using (var readScope = serviceProvider.CreateScope())
        {
            var sousPrefectureReadRepository = readScope.ServiceProvider.GetRequiredService<ISousPrefectureReadRepository>();
            sousPrefectures = await sousPrefectureReadRepository.GetAllAsync(stoppingToken);
        }

        logger.LogInformation("Traitement de {Count} sous-préfectures", sousPrefectures.Count);

        var totalVillagesAjoutes = 0;
        var sousPrefecturesTraitees = 0;
        var erreurs = 0;

        // Créer un nouveau scope (et donc un nouveau DbContext) pour chaque itération
        // Cela évite de garder un DbContext ouvert trop longtemps et réduit les problèmes de tracking
        // Chaque scope utilise le DbContextFactory en arrière-plan pour créer un contexte frais
        foreach (var sousPrefecture in sousPrefectures)
        {
            using var iterationScope = serviceProvider.CreateScope();
            try
            {
                logger.LogInformation("Traitement de la sous-préfecture {SousPrefectureNom} (ID: {Id})",
                    sousPrefecture.Nom, sousPrefecture.Id);

                var villageWebSourceService = iterationScope.ServiceProvider.GetRequiredService<IVillageWebSourceService>();
                var sender = iterationScope.ServiceProvider.GetRequiredService<ISender>();

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
                // Le handler MediatR créera son propre scope avec un contexte frais depuis la factory
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

        // Marquer le service comme exécuté dans un scope séparé
        using (var finalScope = serviceProvider.CreateScope())
        {
            var parametrageRepository = finalScope.ServiceProvider.GetRequiredService<IParametrageRepository>();
            var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);
            await parametrageRepository.SetParametrageAsync(
                new ParametrageEntity
                {
                    Key = parametrageKey,
                    Value = DateTime.Now.ToString()
                });
        }
    }
}

