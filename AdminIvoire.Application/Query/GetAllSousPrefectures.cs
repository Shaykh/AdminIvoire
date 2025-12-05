using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer toutes les sous-préfectures
/// </summary>
public static class GetAllSousPrefectures
{
    /// <summary>
    /// Query pour récupérer toutes les sous-préfectures
    /// </summary>
    public record Query : IQuery<IEnumerable<LocaliteDto>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer toutes les sous-préfectures
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository) : IQueryHandler<Query, IEnumerable<LocaliteDto>>
    {
        /// <summary>
        /// Traite la query en récupérant toutes les sous-préfectures depuis le repository
        /// </summary>
        /// <param name="request">La query</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste de toutes les sous-préfectures</returns>
        public async Task<IEnumerable<LocaliteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de toutes les sous-préfectures");
            var sousPrefectures = await sousPrefectureReadRepository.GetAllAsync(cancellationToken);

            var result = sousPrefectures.Select(sp => sp.MapToDto());
            logger.LogInformation("Récupération de {Count} sous-préfectures", result.Count());
            return result;
        }
    }
}
