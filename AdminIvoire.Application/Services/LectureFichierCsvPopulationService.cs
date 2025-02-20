using AdminIvoire.Application.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Services;

public interface ILectureFichierCsvPopulationService
{
    Task LireFichierCsvPopulationAsync(string cheminFichier, CancellationToken cancellationToken);
}

public class LectureFichierCsvPopulationService(ILogger<LectureFichierCsvPopulationService> logger,
    ISender sender) : ILectureFichierCsvPopulationService
{
    readonly string[] ValidPopulationGroup = ["HOMME", "FEMME"];

    public async Task LireFichierCsvPopulationAsync(string cheminFichier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cheminFichier))
        {
            throw new ArgumentNullException(nameof(cheminFichier));
        }
        logger.LogInformation("Lecture du fichier Csv {Fichier}", cheminFichier);
        using var reader = new StreamReader(cheminFichier);

        while (!reader.EndOfStream)
        {
            var ligne = await reader.ReadLineAsync(cancellationToken);
            logger.LogDebug("Ligne lue {Ligne}", ligne);
            var command = GetCommandFromLine(ligne);
            if (command is not null)
                await sender.Send(command, cancellationToken);
        }
    }

    public AjoutLigneSousPrefecture.Command? GetCommandFromLine(string? ligne)
    {
        if (string.IsNullOrWhiteSpace(ligne))
        {
            logger.LogDebug("La ligne {Ligne} n'a pas le bon format", ligne);
            return null;
        }
        var valeurs = ligne!.Split(',');
        if (valeurs.Length <= 5)
        {
            logger.LogDebug("La ligne {Ligne} n'a pas le bon format", ligne);
            return null;
        }
        if (!int.TryParse(valeurs[5], out var population))
        {
            logger.LogDebug("La population {Population} n'est pas un nombre valide", valeurs[5]);
            return null;
        }
        if (!ValidPopulationGroup.Contains(valeurs[4]))
        {
            logger.LogDebug("Le groupe de population {PopulationGroup} n'est pas valide", valeurs[4]);
            return null;
        }
        var command = new AjoutLigneSousPrefecture.Command
        {
            DistrictNom = valeurs[0],
            RegionNom = valeurs[1],
            DepartementNom = valeurs[2],
            SousprefectureNom = valeurs[3],
            Population = population
        };
        logger.LogInformation("Commande {Command}", command);
        return command;
    }
}
