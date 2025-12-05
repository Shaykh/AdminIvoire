using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer tous les districts avec leurs régions
/// </summary>
public static class GetAllDistricts
{
    /// <summary>
    /// Query pour récupérer tous les districts
    /// </summary>
    public record Query : IQuery<IEnumerable<GetDistrictResponse>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer tous les districts
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDistrictReadRepository districtReadRepository) : IQueryHandler<Query, IEnumerable<GetDistrictResponse>>
    {
        /// <summary>
        /// Traite la query en récupérant tous les districts depuis le repository
        /// </summary>
        /// <param name="request">La query</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste de tous les districts avec leurs régions</returns>
        public async Task<IEnumerable<GetDistrictResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de tous les districts");

            var districts = await districtReadRepository.GetAllAsync(cancellationToken);

            var result = districts.Select(d => d.MapToResponse());
            logger.LogInformation("Récupération de {Count} districts", result.Count());
            return result;
        }
    }
}
