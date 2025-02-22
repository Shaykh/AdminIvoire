using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

public static class RecupererCoordonneesGeographiquesDeDepartements
{
    public record Command : ICommand<bool>
    {
    }

    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository,
        IDepartementWriteRepository departementWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : ICommandHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des coordonnées géographiques de départements en lot");
            var listeNomDepartements = await departementReadRepository.GetAllNomsAsync(cancellationToken);
            if (listeNomDepartements.Count == 0)
            {
                logger.LogInformation("Aucun département à traiter");
                return false;
            }
            foreach (var nomLocalite in listeNomDepartements)
            {
                await RecupererCoordonnneesGeographiquesDUnDepartementAsync(nomLocalite, cancellationToken);
            }
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }

        private async Task RecupererCoordonnneesGeographiquesDUnDepartementAsync(string nomLocalite, CancellationToken cancellationToken)
        {
            var coordonnees = await geocodingApiClient.GetCoordonneesGeographiquesAsync(nomLocalite, cancellationToken);
            await departementWriteRepository.UpdateCoordonneesGeographiquesAsync(nomLocalite, coordonnees, cancellationToken);
        }
    }
}
