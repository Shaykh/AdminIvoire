using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer une sous-préfecture par son identifiant
/// </summary>
public static class GetSousPrefectureById
{
    /// <summary>
    /// Query pour récupérer une sous-préfecture par son identifiant
    /// </summary>
    /// <param name="Id">L'identifiant unique de la sous-préfecture</param>
    public record Query(Guid Id) : IQuery<GetSousPrefectureResponse>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer une sous-préfecture par son identifiant
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository) : IQueryHandler<Query, GetSousPrefectureResponse>
    {
        /// <summary>
        /// Traite la query en récupérant une sous-préfecture par son identifiant depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant de la sous-préfecture</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La sous-préfecture avec ses villages</returns>
        public async Task<GetSousPrefectureResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de la sous-préfecture {Id}", request.Id);
            var sousPrefecture = await sousPrefectureReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = sousPrefecture.MapToResponse();
            logger.LogInformation("Récupération de la sous-préfecture {Id} réussie", request.Id);
            return result;
        }
    }
}
