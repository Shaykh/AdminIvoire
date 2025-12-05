using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer toutes les régions avec leurs départements
/// </summary>
public static class GetAllRegions
{
    /// <summary>
    /// Query pour récupérer toutes les régions
    /// </summary>
    public record Query : IQuery<IEnumerable<GetRegionResponse>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer toutes les régions
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IRegionReadRepository regionReadRepository) : IQueryHandler<Query, IEnumerable<GetRegionResponse>>
    {
        /// <summary>
        /// Traite la query en récupérant toutes les régions depuis le repository
        /// </summary>
        /// <param name="request">La query</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste de toutes les régions avec leurs départements</returns>
        public async Task<IEnumerable<GetRegionResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de toutes les régions");
            var regions = await regionReadRepository.GetAllAsync(cancellationToken);

            var result = regions.Select(r => r.MapToResponse());
            logger.LogInformation("Récupération de {Count} régions", result.Count());
            return result;
        }
    }
}
