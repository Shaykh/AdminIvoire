using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Mapping;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Query;

public static class GetDepartementById
{
    public record Query(Guid Id) : IQuery<GetDepartementResponse>;
    public class Handler(ILogger<Handler> logger,
        IDepartementReadRepository departementReadRepository) : IQueryHandler<Query, GetDepartementResponse>
    {
        public async Task<GetDepartementResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Récupération du département {Id}", request.Id);
            var departement = await departementReadRepository.GetByIdAsync(request.Id, cancellationToken);

            var result = departement.MapToResponse();
            logger.LogInformation("Récupération du département {Id} réussie", request.Id);
            return result;
        }
    }
}
