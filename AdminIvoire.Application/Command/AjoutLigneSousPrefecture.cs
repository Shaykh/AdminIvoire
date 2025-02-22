using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

public static class AjoutLigneSousPrefecture
{
    public record Command : ICommand
    {
        public required string DistrictNom { get; init; }
        public required string RegionNom { get; init; }
        public required string DepartementNom { get; init; }
        public required string SousprefectureNom { get; init; }
        public int Population { get; init; }
    }

    public class Handler(ILogger<Handler> logger,
        IDistrictFactory districtFactory,
        IRegionFactory regionFactory,
        IDepartementFactory departementFactory,
        ISousPrefectureFactory sousPrefectureFactory,
        IUnitOfWork unitOfWork) : ICommandHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Ajout d'une ligne de sous-préfecture {District}, {Region}, {Departement}, {SousPrefecture}",
                request.DistrictNom, request.RegionNom, request.DepartementNom, request.SousprefectureNom);

            var district = await districtFactory.GetOrCreateAsync(request.DistrictNom, request.Population, cancellationToken);
            var region = await regionFactory.GetOrCreateAsync(request.RegionNom, request.Population, district, cancellationToken);
            var departement = await departementFactory.GetOrCreateAsync(request.DepartementNom, request.Population, region, cancellationToken);
            await sousPrefectureFactory.GetOrCreateAsync(request.SousprefectureNom, request.Population, departement, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
