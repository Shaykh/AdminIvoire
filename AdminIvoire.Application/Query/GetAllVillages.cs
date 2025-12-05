using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer tous les villages
/// </summary>
public static class GetAllVillages
{
    /// <summary>
    /// Query pour récupérer tous les villages
    /// </summary>
    public record Query : IQuery<IEnumerable<LocaliteDto>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer tous les villages
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository) : IQueryHandler<Query, IEnumerable<LocaliteDto>>
    {
        /// <summary>
        /// Traite la query en récupérant tous les villages depuis le repository
        /// </summary>
        /// <param name="request">La query</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste de tous les villages</returns>
        public async Task<IEnumerable<LocaliteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de tous les villages");
            var villages = await villageReadRepository.GetAllAsync(cancellationToken);

            var result = villages.Select(v => v.MapToDto());
            logger.LogInformation("Récupération de {Count} villages", result.Count());
            return result;
        }
    }
}
