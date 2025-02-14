using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
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
        IDistrictReadRepository districtReadRepository,
        IDistrictWriteRepository districtWriteRepository,
        IRegionReadRepository regionReadRepository,
        IRegionWriteRepository regionWriteRepository,
        IDepartementReadRepository departementReadRepository,
        IDepartementWriteRepository departementWriteRepository,
        ISousPrefectureWriteRepository sousPrefectureWriteRepository, 
        IUnitOfWork unitOfWork) : ICommandHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Ajout d'une ligne de sous-préfecture {District}, {Region}, {Departement}, {SousPrefecture}",
                request.DistrictNom, request.RegionNom, request.DepartementNom, request.SousprefectureNom);

            var district = await GetDistrictAsync(request, cancellationToken);
            var region = await GetRegionAsync(request, district, cancellationToken);
            var departement = await GetDepartementAsync(request, region, cancellationToken);
            var sousPrefecture = new SousPrefecture
            {
                Nom = request.SousprefectureNom,
                Departement = departement,
                Population = request.Population
            };
            await sousPrefectureWriteRepository.AddAsync(sousPrefecture, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task<District> GetDistrictAsync(Command request, CancellationToken cancellationToken)
        {
            var district = await districtReadRepository.GetByNomAsync(request.DistrictNom, cancellationToken);
            if (district == null)
            {
                district = new District { Nom = request.DistrictNom, Population = request.Population };
            }
            else
            {
                district.Population += request.Population;
            }
            await districtWriteRepository.AddAsync(district, cancellationToken);
            return district;
        }

        private async Task<Region> GetRegionAsync(Command request, District district, CancellationToken cancellationToken)
        {
            var region = await regionReadRepository.GetByNomAsync(request.RegionNom, cancellationToken);
            if (region == null)
            {
                region = new Region { Nom = request.RegionNom, District = district, Population = request.Population };
            }
            else
            {
                region.Population += request.Population;
            }
            await regionWriteRepository.AddAsync(region, cancellationToken);
            return region;
        }

        private async Task<Departement> GetDepartementAsync(Command request, Region region, CancellationToken cancellationToken)
        {
            var departement = await departementReadRepository.GetByNomAsync(request.DepartementNom, cancellationToken);
            if (departement == null)
            {
                departement = new Departement { Nom = request.DepartementNom, Region = region, Population = request.Population };
            }
            else
            {
                departement.Population += request.Population;
            }
            await departementWriteRepository.AddAsync(departement, cancellationToken);
            return departement;
        }

    }
}
