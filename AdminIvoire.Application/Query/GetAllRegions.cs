using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetAllRegions
{
    public record Query : IQuery<IEnumerable<GetRegionResponse>>;

    public class Handler(ILogger<Handler> logger,
        IRegionReadRepository regionReadRepository) : IQueryHandler<Query, IEnumerable<GetRegionResponse>>
    {
        public async Task<IEnumerable<GetRegionResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de toutes les régions");
            var regions = await regionReadRepository.GetAllAsync(cancellationToken);

            var result = regions.Select(r => r.MapToResponse());
            logger.LogInformation("Récupération de {Count} régions", result.Count());
            return result;
        }
    }
}
