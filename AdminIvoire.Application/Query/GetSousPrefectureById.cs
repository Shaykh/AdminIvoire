using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetSousPrefectureById
{
    public record Query(Guid Id) : IQuery<GetSousPrefectureResponse>;

    public class Handler(ILogger<Handler> logger,
        ISousPrefectureReadRepository sousPrefectureReadRepository) : IQueryHandler<Query, GetSousPrefectureResponse>
    {
        public async Task<GetSousPrefectureResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de la sous-préfecture {Id}", request.Id);
            var sousPrefecture = await sousPrefectureReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = sousPrefecture.MapToResponse();
            logger.LogInformation("Récupération de la sous-préfecture {Id} réussie", request.Id);
            return result;
        }
    }
}
