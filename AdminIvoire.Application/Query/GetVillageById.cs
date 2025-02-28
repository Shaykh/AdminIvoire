using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetVillageById
{
    public record Query(Guid Id) : IQuery<GetVillageResponse>;

    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository) : IQueryHandler<Query, GetVillageResponse>
    {
        public async Task<GetVillageResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération du village {Id}", request.Id);
            var village = await villageReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = village.MapToResponse();
            logger.LogInformation("Récupération du village {Id} réussie", request.Id);
            return result;
        }
    }
}
