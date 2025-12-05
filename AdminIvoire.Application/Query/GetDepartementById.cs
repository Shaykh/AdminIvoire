using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer un département par son identifiant
/// </summary>
public static class GetDepartementById
{
    /// <summary>
    /// Query pour récupérer un département par son identifiant
    /// </summary>
    /// <param name="Id">L'identifiant unique du département</param>
    public record Query(Guid Id) : IQuery<GetDepartementResponse>;
    /// <summary>
    /// Gestionnaire de la query pour récupérer un département par son identifiant
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository) : IQueryHandler<Query, GetDepartementResponse>
    {
        /// <summary>
        /// Traite la query en récupérant un département par son identifiant depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant du département</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>Le département avec ses sous-préfectures</returns>
        public async Task<GetDepartementResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération du département {Id}", request.Id);
            var departement = await departementReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = departement.MapToResponse();
            logger.LogInformation("Récupération du département {Id} réussie", request.Id);
            return result;
        }
    }
}
