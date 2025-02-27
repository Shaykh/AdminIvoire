using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetRegionById
{
    public record Query(Guid Id) : IQuery<GetRegionResponse>;

    public class Handler(ILogger<Handler> logger,
        IRegionReadRepository regionReadRepository) : IQueryHandler<Query, GetRegionResponse>
    {
        public async Task<GetRegionResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de la région {Id}", request.Id);
            var region = await regionReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = region.MapToResponse();
            logger.LogInformation("Récupération de la région {Id} réussie", request.Id);
            return result;
        }
    }
}
