using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetAllSousPrefectures
{
    public record Query : IQuery<IEnumerable<LocaliteDto>>;

    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository) : IQueryHandler<Query, IEnumerable<LocaliteDto>>
    {
        public async Task<IEnumerable<LocaliteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de toutes les sous-préfectures");
            var sousPrefectures = await sousPrefectureReadRepository.GetAllAsync(cancellationToken);

            var result = sousPrefectures.Select(sp => sp.MapToDto());
            logger.LogInformation("Récupération de {Count} sous-préfectures", result.Count());
            return result;
        }
    }
}
