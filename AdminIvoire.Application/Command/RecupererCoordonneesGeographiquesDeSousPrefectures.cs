using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

/// <summary>
/// Commande pour récupérer les coordonnées géographiques de toutes les sous-préfectures
/// </summary>
public static class RecupererCoordonneesGeographiquesDeSousPrefectures
{
    /// <summary>
    /// Commande pour récupérer les coordonnées géographiques de toutes les sous-préfectures
    /// </summary>
    public record Command : ICommand<bool>
    { 
    }

    /// <summary>
    /// Gestionnaire de la commande de récupération des coordonnées géographiques des sous-préfectures
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository,
        ISousPrefectureWriteRepository sousPrefectureWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : ICommandHandler<Command, bool>
    {
        /// <summary>
        /// Traite la commande en récupérant les coordonnées géographiques de toutes les sous-préfectures
        /// </summary>
        /// <param name="request">La commande</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>True si des sous-préfectures ont été traitées, False sinon</returns>
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des coordonnées géographiques de sous-préfectures en lot");

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

        /// <summary>
        /// Récupère les coordonnées géographiques d'une sous-préfecture spécifique et met à jour la base de données
        /// </summary>
        /// <param name="nomLocalite">Le nom de la sous-préfecture</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        private async Task RecupererCoordonnneesGeographiquesDUneSousPrefectureAsync(string nomLocalite, CancellationToken cancellationToken)
        {
            var coordonnees = await geocodingApiClient.GetCoordonneesGeographiquesAsync(nomLocalite, cancellationToken);
            await sousPrefectureWriteRepository.UpdateCoordonneesGeographiquesAsync(nomLocalite, coordonnees, cancellationToken);
        }
    }
}
