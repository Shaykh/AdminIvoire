using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer tous les départements
/// </summary>
public static class GetAllDepartements
{
    /// <summary>
    /// Query pour récupérer tous les départements
    /// </summary>
    public record Query : IQuery<IEnumerable<LocaliteDto>>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer tous les départements
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository) : IQueryHandler<Query, IEnumerable<LocaliteDto>>
    {
        /// <summary>
        /// Traite la query en récupérant tous les départements depuis le repository
        /// </summary>
        /// <param name="request">La query</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste de tous les départements</returns>
        public async Task<IEnumerable<LocaliteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de tous les départements");
            var departements = await departementReadRepository.GetAllAsync(cancellationToken);

            var result = departements.Select(d => d.MapToDto());
            logger.LogInformation("Récupération de {Count} départements", result.Count());
            return result;
        }
    }
}
