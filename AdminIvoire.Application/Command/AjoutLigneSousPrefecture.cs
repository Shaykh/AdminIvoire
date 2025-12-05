using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

/// <summary>
/// Commande pour ajouter une ligne de sous-préfecture depuis un fichier CSV
/// </summary>
public static class AjoutLigneSousPrefecture
{
    /// <summary>
    /// Commande représentant une ligne CSV de sous-préfecture avec sa hiérarchie administrative
    /// </summary>
    public record Command : ICommand
    {
        /// <summary>
        /// Le nom du district
        /// </summary>
        public required string DistrictNom { get; init; }
        /// <summary>
        /// Le nom de la région
        /// </summary>
        public required string RegionNom { get; init; }
        /// <summary>
        /// Le nom du département
        /// </summary>
        public required string DepartementNom { get; init; }
        /// <summary>
        /// Le nom de la sous-préfecture
        /// </summary>
        public required string SousprefectureNom { get; init; }
        /// <summary>
        /// La population de la localité
        /// </summary>
        public int Population { get; init; }
    }

    /// <summary>
    /// Gestionnaire de la commande d'ajout d'une ligne de sous-préfecture
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDistrictFactory districtFactory,
        IRegionFactory regionFactory,
        IDepartementFactory departementFactory,
        ISousPrefectureFactory sousPrefectureFactory,
        IUnitOfWork unitOfWork) : ICommandHandler<Command>
    {
        /// <summary>
        /// Traite la commande d'ajout d'une ligne de sous-préfecture en créant ou récupérant la hiérarchie administrative complète
        /// </summary>
        /// <param name="request">La commande contenant les informations de la sous-préfecture</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Ajout d'une ligne de sous-préfecture {District}, {Region}, {Departement}, {SousPrefecture}",
                request.DistrictNom, request.RegionNom, request.DepartementNom, request.SousprefectureNom);

            var district = await districtFactory.GetOrCreateAsync(request.DistrictNom, request.Population, cancellationToken);
            var region = await regionFactory.GetOrCreateAsync(request.RegionNom, request.Population, district, cancellationToken);
            var departement = await departementFactory.GetOrCreateAsync(request.DepartementNom, request.Population, region, cancellationToken);
            await sousPrefectureFactory.GetOrCreateAsync(request.SousprefectureNom, request.Population, departement, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
