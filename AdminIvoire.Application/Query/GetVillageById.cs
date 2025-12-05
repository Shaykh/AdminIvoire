using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer un village par son identifiant
/// </summary>
public static class GetVillageById
{
    /// <summary>
    /// Query pour récupérer un village par son identifiant
    /// </summary>
    /// <param name="Id">L'identifiant unique du village</param>
    public record Query(Guid Id) : IQuery<GetVillageResponse>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer un village par son identifiant
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository) : IQueryHandler<Query, GetVillageResponse>
    {
        /// <summary>
        /// Traite la query en récupérant un village par son identifiant depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant du village</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>Le village avec ses informations</returns>
        public async Task<GetVillageResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération du village {Id}", request.Id);
            var village = await villageReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = village.MapToResponse();
            logger.LogInformation("Récupération du village {Id} réussie", request.Id);
            return result;
        }
    }
}
