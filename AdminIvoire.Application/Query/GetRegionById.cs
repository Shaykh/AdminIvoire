using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer une région par son identifiant
/// </summary>
public static class GetRegionById
{
    /// <summary>
    /// Query pour récupérer une région par son identifiant
    /// </summary>
    /// <param name="Id">L'identifiant unique de la région</param>
    public record Query(Guid Id) : IQuery<GetRegionResponse>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer une région par son identifiant
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IRegionReadRepository regionReadRepository) : IQueryHandler<Query, GetRegionResponse>
    {
        /// <summary>
        /// Traite la query en récupérant une région par son identifiant depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant de la région</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La région avec ses départements</returns>
        public async Task<GetRegionResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de la région {Id}", request.Id);
            var region = await regionReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = region.MapToResponse();
            logger.LogInformation("Récupération de la région {Id} réussie", request.Id);
            return result;
        }
    }
}
