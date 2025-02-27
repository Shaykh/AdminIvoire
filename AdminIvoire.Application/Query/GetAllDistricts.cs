using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetAllDistricts
{
    public record Query : IQuery<IEnumerable<GetDistrictResponse>>;

    public class Handler(ILogger<Handler> logger,
        IDistrictReadRepository districtReadRepository) : IQueryHandler<Query, IEnumerable<GetDistrictResponse>>
    {
        public async Task<IEnumerable<GetDistrictResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de tous les districts");

            var districts = await districtReadRepository.GetAllAsync(cancellationToken);

            var result = districts.Select(d => d.MapToResponse());
            logger.LogInformation("Récupération de {Count} districts", result.Count());
            return result;
        }
    }
}
