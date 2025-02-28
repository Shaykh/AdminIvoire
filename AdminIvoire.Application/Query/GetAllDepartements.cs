using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetAllDepartements
{
    public record Query : IQuery<IEnumerable<LocaliteDto>>;

    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository) : IQueryHandler<Query, IEnumerable<LocaliteDto>>
    {
        public async Task<IEnumerable<LocaliteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération de tous les départements");
            var departements = await departementReadRepository.GetAllAsync(cancellationToken);

            var result = departements.Select(d => d.MapToDto());
            logger.LogInformation("Récupération de {Count} départements", result.Count());
            return result;
        }
    }
}
