using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetRegionsByDistrictId
{
    public record Query(Guid DistrictId) : IQuery<IEnumerable<GetRegionResponse>>;
    public class Handler(ILogger<Handler> logger,
        IRegionReadRepository regionReadRepository) : IQueryHandler<Query, IEnumerable<GetRegionResponse>>
    {
        public async Task<IEnumerable<GetRegionResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des régions du district {DistrictId}", request.DistrictId);
            var regions = await regionReadRepository.GetAllByDistrictIdAsync(request.DistrictId, cancellationToken);

            var result = regions.Select(r => r.MapToResponse());
            logger.LogInformation("Récupération de {Count} régions du district {DistrictId}", result.Count(), request.DistrictId);
            return result;
        }
    }
}
