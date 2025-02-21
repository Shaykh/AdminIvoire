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

        await RecupererDonneesLocaliteAsync(stoppingToken);

        logger.LogInformation("Fin exécution du service de lecture des données de localité");
    }

    public async Task RecupererDonneesLocaliteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var parametrageRepository = scope.ServiceProvider.GetRequiredService<IParametrageRepository>();
        if ((await parametrageRepository.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService))) is not null)
        {
            logger.LogInformation("La recuperation des coordonnées des localités a déja été effectuée.");
            return;
        }
        var recuperationReussie = await sender.Send(new RecupererCoordonneesGeographiquesDeSousPrefectures.Command(), stoppingToken);
        if (recuperationReussie)
        {
            await parametrageRepository.SetParametrageAsync(
            new ParametrageEntity { Key = nameof(RecuperationDonneesGeographiqueService), Value = DateTime.Now.ToString() }
            );
        }
    }
}