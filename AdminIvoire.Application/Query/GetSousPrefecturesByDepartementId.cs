using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer toutes les sous-préfectures d'un département
/// </summary>
public static class GetSousPrefecturesByDepartementId
{
    /// <summary>
    /// Query pour récupérer toutes les sous-préfectures d'un département
    /// </summary>
    /// <param name="DepartementId">L'identifiant unique du département</param>
    public record Query(Guid DepartementId) : IQuery<IEnumerable<GetSousPrefectureResponse>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer toutes les sous-préfectures d'un département
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository) : IQueryHandler<Query, IEnumerable<GetSousPrefectureResponse>>
    {
        /// <summary>
        /// Traite la query en récupérant toutes les sous-préfectures d'un département depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant du département</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste des sous-préfectures du département</returns>
        public async Task<IEnumerable<GetSousPrefectureResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des sous-préfectures du département {DepartementId}", request.DepartementId);
            var sousPrefectures = await sousPrefectureReadRepository.GetAllByDepartementIdAsync(request.DepartementId, cancellationToken);

            var result = sousPrefectures.Select(d => d.MapToResponse());
            logger.LogInformation("Récupération de {Count} sous-préfectures du département {DepartementId}", result.Count(), request.DepartementId);
            return result;
        }
    }
}
