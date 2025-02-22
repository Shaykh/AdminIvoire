using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

public static class RecupererCoordonneesGeographiquesDeVillages
{
    public record Command : ICommand<bool>
    {
    }

    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository,
        IVillageWriteRepository villageWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : ICommandHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des coordonnées géographiques de villages en lot");
            var listeNomVillages = await villageReadRepository.GetAllNomsAsync(cancellationToken);
            if (listeNomVillages.Count == 0)
            {
                logger.LogInformation("Aucun village à traiter");
                return false;
            }
            foreach (var nomLocalite in listeNomVillages)
            {
                await RecupererCoordonnneesGeographiquesDUnVillageAsync(nomLocalite, cancellationToken);
            }
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }

        private async Task RecupererCoordonnneesGeographiquesDUnVillageAsync(string nomLocalite, CancellationToken cancellationToken)
        {
            var coordonnees = await geocodingApiClient.GetCoordonneesGeographiquesAsync(nomLocalite, cancellationToken);
            await villageWriteRepository.UpdateCoordonneesGeographiquesAsync(nomLocalite, coordonnees, cancellationToken);
        }
    }
}
