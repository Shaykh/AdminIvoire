using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Query pour récupérer un district par son identifiant
/// </summary>
public static class GetDistrictById
{
    /// <summary>
    /// Query pour récupérer un district par son identifiant
    /// </summary>
    /// <param name="Id">L'identifiant unique du district</param>
    public record Query(Guid Id) : IQuery<GetDistrictResponse>;

    /// <summary>
    /// Gestionnaire de la query pour récupérer un district par son identifiant
    /// </summary>
    public class Handler(ILogger<Handler> logger,
        IDistrictReadRepository districtReadRepository) : IQueryHandler<Query, GetDistrictResponse>
    {
        /// <summary>
        /// Traite la query en récupérant un district par son identifiant depuis le repository
        /// </summary>
        /// <param name="request">La query contenant l'identifiant du district</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>Le district avec ses régions</returns>
        public async Task<GetDistrictResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération du district {Id}", request.Id);
            var district = await districtReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = district.MapToResponse();
            logger.LogInformation("Récupération du district {Id} réussie", request.Id);
            return result;
        }
    }
}
