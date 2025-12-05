using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer toutes les régions d'un district
/// </summary>
public static class GetRegionsByDistrictId
{
    /// <summary>
    /// Query pour récupérer toutes les régions d'un district
    /// </summary>
    /// <param name="DistrictId">L'identifiant unique du district</param>
    public record Query(Guid DistrictId) : IQuery<IEnumerable<GetRegionResponse>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer toutes les régions d'un district
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IRegionReadRepository regionReadRepository) : IQueryHandler<Query, IEnumerable<GetRegionResponse>>
    {
        /// <summary>
        /// Traite la query en récupérant toutes les régions d'un district depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant du district</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste des régions du district</returns>
        public async Task<IEnumerable<GetRegionResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des régions du district {DistrictId}", request.DistrictId);
            var regions = await regionReadRepository.GetAllByDistrictIdAsync(request.DistrictId, cancellationToken);

            var result = regions.Select(r => r.MapToResponse());
            logger.LogInformation("Récupération de {Count} régions du district {DistrictId}", result.Count(), request.DistrictId);
            return result;
        }
    }
}
