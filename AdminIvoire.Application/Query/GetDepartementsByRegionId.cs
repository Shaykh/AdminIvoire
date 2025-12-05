using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer tous les départements d'une région
/// </summary>
public static class GetDepartementsByRegionId
{
    /// <summary>
    /// Query pour récupérer tous les départements d'une région
    /// </summary>
    /// <param name="RegionId">L'identifiant unique de la région</param>
    public record Query(Guid RegionId) : IQuery<IEnumerable<GetDepartementResponse>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer tous les départements d'une région
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository) : IQueryHandler<Query, IEnumerable<GetDepartementResponse>>
    {
        /// <summary>
        /// Traite la query en récupérant tous les départements d'une région depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant de la région</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste des départements de la région</returns>
        public async Task<IEnumerable<GetDepartementResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des départements de la région {RegionId}", request.RegionId);
            var departements = await departementReadRepository.GetAllByRegionIdAsync(request.RegionId, cancellationToken);

            var result = departements.Select(d => d.MapToResponse());
            logger.LogInformation("Récupération de {Count} départements de la région {RegionId}", result.Count(), request.RegionId);
            return result;
        }
    }
}
