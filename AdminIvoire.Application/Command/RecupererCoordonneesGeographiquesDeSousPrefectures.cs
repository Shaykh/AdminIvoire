using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

public static class RecupererCoordonneesGeographiquesDeSousPrefectures
{
    public record Command : ICommand
    { 
    }

    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository,
        ISousPrefectureWriteRepository sousPrefectureWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : ICommandHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des coordonnées géographiques en lot");

            var listeNomSousPrefectures = await sousPrefectureReadRepository.GetAllNomsAsync(cancellationToken);

            foreach (var nomLocalite in listeNomSousPrefectures)
            {
                await RecupererCoordonnneesGeographiquesDUneSousPrefectureAsync(nomLocalite, cancellationToken);
            }

            await unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task RecupererCoordonnneesGeographiquesDUneSousPrefectureAsync(string nomLocalite, CancellationToken cancellationToken)
        {
            var coordonnees = await geocodingApiClient.GetCoordonneesGeographiquesAsync(nomLocalite, cancellationToken);
            await sousPrefectureWriteRepository.UpdateCoordonneesGeographiquesAsync(nomLocalite, coordonnees, cancellationToken);
        }
    }
}
