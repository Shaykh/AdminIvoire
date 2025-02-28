using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetSousPrefecturesByDepartementId
{
    public record Query(Guid DepartementId) : IQuery<IEnumerable<GetSousPrefectureResponse>>;

    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository) : IQueryHandler<Query, IEnumerable<GetSousPrefectureResponse>>
    {
        public async Task<IEnumerable<GetSousPrefectureResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération des sous-préfectures du département {DepartementId}", request.DepartementId);
            var sousPrefectures = await sousPrefectureReadRepository.GetAllByDepartementIdAsync(request.DepartementId, cancellationToken);

            var result = sousPrefectures.Select(d => d.MapToResponse());
            logger.LogInformation("Récupération de {Count} sous-préfectures du département {DepartementId}", result.Count(), request.DepartementId);
            return result;
        }
    }
}
