using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer tous les villages d'une sous-préfecture
/// </summary>
public static class GetVillagesBySousPrefectureId
{
    /// <summary>
    /// Query pour récupérer tous les villages d'une sous-préfecture
    /// </summary>
    /// <param name="SousPrefectureId">L'identifiant unique de la sous-préfecture</param>
    public record Query(Guid SousPrefectureId) : IQuery<IEnumerable<GetVillageResponse>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer tous les villages d'une sous-préfecture
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository) : IQueryHandler<Query, IEnumerable<GetVillageResponse>>
    {
        /// <summary>
        /// Traite la query en récupérant tous les villages d'une sous-préfecture depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant de la sous-préfecture</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste des villages de la sous-préfecture</returns>
        public async Task<IEnumerable<GetVillageResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des villages de la sous-préfecture {SousPrefectureId}", request.SousPrefectureId);
            var villages = await villageReadRepository.GetAllBySousPrefectureIdAsync(request.SousPrefectureId, cancellationToken);

            var result = villages.Select(v => v.MapToResponse());
            logger.LogInformation("Récupération de {Count} villages de la sous-préfecture {SousPrefectureId}", result.Count(), request.SousPrefectureId);
            return result;
        }
    }
}
