using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetVillagesBySousPrefectureId
{
    public record Query(Guid SousPrefectureId) : IQuery<IEnumerable<GetVillageResponse>>;

    public class Handler(ILogger<Handler> logger,
        IVillageReadRepository villageReadRepository) : IQueryHandler<Query, IEnumerable<GetVillageResponse>>
    {
        public async Task<IEnumerable<GetVillageResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des villages de la sous-préfecture {SousPrefectureId}", request.SousPrefectureId);
            var villages = await villageReadRepository.GetAllBySousPrefectureIdAsync(request.SousPrefectureId, cancellationToken);

            var result = villages.Select(v => v.MapToResponse());
            logger.LogInformation("Récupération de {Count} villages de la sous-préfecture {SousPrefectureId}", result.Count(), request.SousPrefectureId);
            return result;
        }
    }
}
