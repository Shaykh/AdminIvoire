using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetDepartementsByRegionId
{
    public record Query(Guid RegionId) : IQuery<IEnumerable<GetDepartementResponse>>;

    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository) : IQueryHandler<Query, IEnumerable<GetDepartementResponse>>
    {
        public async Task<IEnumerable<GetDepartementResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des départements de la région {RegionId}", request.RegionId);
            var departements = await departementReadRepository.GetAllByRegionIdAsync(request.RegionId, cancellationToken);

            var result = departements.Select(d => d.MapToResponse());
            logger.LogInformation("Récupération de {Count} départements de la région {RegionId}", result.Count(), request.RegionId);
            return result;
        }
    }
}
