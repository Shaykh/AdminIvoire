using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

public static class AjoutVillagesDeSousPrefecture
{
    public record Command(Guid SousPrefectureId, string[] Villages) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.SousPrefectureId).NotEmpty();
            RuleFor(x => x.Villages).NotEmpty();
            RuleForEach(x => x.Villages).NotEmpty();
        }
    }

    public class Handler(ILogger<Handler> logger,
        IValidator<Command> validator,
        ISousPrefectureReadRepository sousPrefectureReadRepository,
        IVillageWriteRepository villageWriteRepository,
        IGeocodingApiClient geocodingApiClient,
        IUnitOfWork unitOfWork) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            logger.LogInformation("Ajout de villages à la sous-préfecture {SousPrefectureId}", request.SousPrefectureId);
            var sousPrefecture = await sousPrefectureReadRepository.GetByIdAsync(request.SousPrefectureId, cancellationToken);
            
            var villages = await SetVillagesAsync(request.Villages, sousPrefecture, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            logger.LogInformation("{NombreVillages} villages ajoutés à la sous-préfecture {SousPrefectureId}", villages.Count, sousPrefecture.Nom);
        }

        private async Task<IList<Village>> SetVillagesAsync(string[] villages, SousPrefecture sousPrefecture, CancellationToken cancellationToken)
        {
            logger.LogDebug("Ajout des villages");
            var villagesList = new List<Village>();
            foreach (var villageName in villages)
            {
                var village = new Village
                {
                    Nom = villageName,
                    SousPrefectureId = sousPrefecture.Id,
                    SousPrefecture = sousPrefecture
                };
                if (sousPrefecture.AddVillage(village))
                {
                    var coordonneesGeographiques = await geocodingApiClient.GetCoordonneesGeographiquesAsync(villageName, cancellationToken);
                    village.CoordonneesGeographiques = coordonneesGeographiques;

                    logger.LogDebug("Ajout du village {VillageName}", villageName);
                    villagesList.Add(village);
                    await villageWriteRepository.AddAsync(village, cancellationToken);
                }
            }
            return villagesList;
        }
    }
}
