using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

/// <summary>
/// Commande pour récupérer les coordonnées géographiques de tous les départements
/// </summary>
public static class RecupererCoordonneesGeographiquesDeDepartements
{
    /// <summary>
    /// Commande pour récupérer les coordonnées géographiques de tous les départements
    /// </summary>
    public record Command : ICommand<bool>
    {
    }

    /// <summary>
    /// Gestionnaire de la commande de récupération des coordonnées géographiques des départements
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository,
        IDepartementWriteRepository departementWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : ICommandHandler<Command, bool>
    {
        /// <summary>
        /// Traite la commande en récupérant les coordonnées géographiques de tous les départements
        /// </summary>
        /// <param name="request">La commande</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>True si des départements ont été traités, False sinon</returns>
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

        /// <summary>
        /// Récupère les coordonnées géographiques d'un département spécifique et met à jour la base de données
        /// </summary>
        /// <param name="nomLocalite">Le nom du département</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        private async Task RecupererCoordonnneesGeographiquesDUnDepartementAsync(string nomLocalite, CancellationToken cancellationToken)
        {
            var coordonnees = await geocodingApiClient.GetCoordonneesGeographiquesAsync(nomLocalite, cancellationToken);
            await departementWriteRepository.UpdateCoordonneesGeographiquesAsync(nomLocalite, coordonnees, cancellationToken);
        }
    }
}
