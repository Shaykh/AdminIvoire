using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Command;

/// <summary>
/// Commande pour ajouter des villages à une sous-préfecture
/// </summary>
public static class AjoutVillagesDeSousPrefecture
{
    /// <summary>
    /// Commande représentant l'ajout de villages à une sous-préfecture
    /// </summary>
    /// <param name="SousPrefectureId">L'identifiant de la sous-préfecture à laquelle ajouter les villages</param>
    /// <param name="Villages">Le tableau des noms de villages à ajouter</param>
    public record Command(Guid SousPrefectureId, string[] Villages) : ICommand;

    /// <summary>
    /// Validateur FluentValidation pour la commande d'ajout de villages
    /// </summary>
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
        IUnitOfWork unitOfWork) : ICommandHandler<Command>
    {
        /// <summary>
        /// Traite la commande d'ajout de villages à une sous-préfecture en récupérant leurs coordonnées géographiques
        /// </summary>
        /// <param name="request">La commande contenant l'identifiant de la sous-préfecture et la liste des villages</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            logger.LogInformation("Ajout de villages à la sous-préfecture {SousPrefectureId}", request.SousPrefectureId);
            var sousPrefecture = await sousPrefectureReadRepository.GetByIdAsync(request.SousPrefectureId, cancellationToken);
            
            var villages = await SetVillagesAsync(request.Villages, sousPrefecture, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            logger.LogInformation("{NombreVillages} villages ajoutés à la sous-préfecture {SousPrefectureId}", villages.Count, sousPrefecture.Nom);
        }

        /// <summary>
        /// Crée et ajoute les villages à la sous-préfecture en récupérant leurs coordonnées géographiques
        /// </summary>
        /// <param name="villages">Le tableau des noms de villages à ajouter</param>
        /// <param name="sousPrefecture">La sous-préfecture à laquelle ajouter les villages</param>
        /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
        /// <returns>La liste des villages créés</returns>
        private async Task<IList<Village>> SetVillagesAsync(string[] villages, SousPrefecture sousPrefecture, CancellationToken cancellationToken)
        {
            logger.LogDebug("Ajout des villages");
            var villagesList = new List<Village>();
            foreach (var villageName in villages)
            {
                var village = new Village
                {
                    Nom = villageName,
                    SousPrefectureId = sousPrefecture.Id
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
