using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetAllVillages
{
    public record Query : IQuery<IEnumerable<LocaliteDto>>;

    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository) : IQueryHandler<Query, IEnumerable<LocaliteDto>>
    {
        public async Task<IEnumerable<LocaliteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de tous les villages");
            var villages = await villageReadRepository.GetAllAsync(cancellationToken);

            var result = villages.Select(v => v.MapToDto());
            logger.LogInformation("Récupération de {Count} villages", result.Count());
            return result;
        }
    }
}
