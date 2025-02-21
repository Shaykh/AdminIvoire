using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

public static class RecupererCoordonneesGeographiquesDeSousPrefectures
{
    public record Command : ICommand<bool>
    { 
    }

    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository,
        ISousPrefectureWriteRepository sousPrefectureWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : ICommandHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des coordonnées géographiques en lot");

            var listeNomSousPrefectures = await sousPrefectureReadRepository.GetAllNomsAsync(cancellationToken);
            if (listeNomSousPrefectures.Count == 0)
            {
                logger.LogInformation("Aucune sous-préfecture à traiter");
                return false;
            }
            foreach (var nomLocalite in listeNomSousPrefectures)
            {
                await RecupererCoordonnneesGeographiquesDUneSousPrefectureAsync(nomLocalite, cancellationToken);
            }

            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }

        private async Task RecupererCoordonnneesGeographiquesDUneSousPrefectureAsync(string nomLocalite, CancellationToken cancellationToken)
        {
            var coordonnees = await geocodingApiClient.GetCoordonneesGeographiquesAsync(nomLocalite, cancellationToken);
            await sousPrefectureWriteRepository.UpdateCoordonneesGeographiquesAsync(nomLocalite, coordonnees, cancellationToken);
        }
    }
}
