using AdminIvoire.Application.Command;
using AdminIvoire.Application.Parametrage;
using MediatR;

namespace AdminIvoire.WebApi.BackgroundServices;

public class RecuperationDonneesGeographiqueService(ILogger<RecuperationDonneesGeographiqueService> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Début exécution du service de lecture des données de localité");

        await RecupererDonneesGeoLocaliteAsync(stoppingToken);

        logger.LogInformation("Fin exécution du service de lecture des données de localité");
    }

    public async Task RecupererDonneesGeoLocaliteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        await RecupererDonneesGeoDeDepartementsAsync(scope, stoppingToken);
        await RecupererDonneesGeoDeSousPrefecturesAsync(scope, stoppingToken);
    }

    private async Task RecupererDonneesGeoDeSousPrefecturesAsync(IServiceScope scope, CancellationToken stoppingToken)
    {
        var parametrageKey = nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures);
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();
        if ((await parametrageRepository.GetParametrageAsync(parametrageKey)) is not null)
        {
            logger.LogInformation("La recuperation des coordonnées des sous-préfectures a déja été effectuée.");
            return;
        }
        var recuperationReussie = await sender.Send(new RecupererCoordonneesGeographiquesDeSousPrefectures.Command(), stoppingToken);
        if (recuperationReussie)
        {
            await parametrageRepository.SetParametrageAsync(
            new ParametrageEntity { Key = parametrageKey, Value = DateTime.Now.ToString() }
            );
        }
    }

    private async Task RecupererDonneesGeoDeDepartementsAsync(IServiceScope scope, CancellationToken stoppingToken)
    {
        var parametrageKey = nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements);
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();
        if ((await parametrageRepository.GetParametrageAsync(parametrageKey)) is not null)
        {
            logger.LogInformation("La recuperation des coordonnées des départements a déja été effectuée.");
            return;
        }
        var recuperationReussie = await sender.Send(new RecupererCoordonneesGeographiquesDeDepartements.Command(), stoppingToken);
        if (recuperationReussie)
        {
            await parametrageRepository.SetParametrageAsync(
            new ParametrageEntity { Key = parametrageKey, Value = DateTime.Now.ToString() }
            );
        }
    }

}