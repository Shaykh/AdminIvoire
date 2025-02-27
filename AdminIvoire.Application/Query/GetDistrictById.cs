using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetDistrictById
{
    public record Query(Guid Id) : IQuery<GetDistrictResponse>;

    public class Handler(ILogger<Handler> logger,
        IDistrictReadRepository districtReadRepository) : IQueryHandler<Query, GetDistrictResponse>
    {
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
