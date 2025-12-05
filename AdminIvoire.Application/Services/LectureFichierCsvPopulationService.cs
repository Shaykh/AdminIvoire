using AdminIvoire.Application.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Services;

/// <summary>
/// Interface pour lire et traiter un fichier CSV contenant des données de population
/// </summary>
public interface ILectureFichierCsvPopulationService
{
    /// <summary>
    /// Lit un fichier CSV de population ligne par ligne et envoie des commandes pour traiter chaque ligne
    /// </summary>
    /// <param name="cheminFichier">Le chemin vers le fichier CSV à lire</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Une tâche représentant l'opération asynchrone</returns>
    Task LireFichierCsvPopulationAsync(string cheminFichier, CancellationToken cancellationToken);
}

/// <summary>
/// Service pour lire un fichier CSV contenant des données de population et créer les entités de localité correspondantes
/// </summary>
public sealed class LectureFichierCsvPopulationService(ILogger<LectureFichierCsvPopulationService> logger,
    ISender sender) : ILectureFichierCsvPopulationService
{
    readonly string[] ValidPopulationGroup = ["HOMME", "FEMME"];

    /// <summary>
    /// Lit un fichier CSV de population ligne par ligne et envoie des commandes pour traiter chaque ligne
    /// </summary>
    /// <param name="cheminFichier">Le chemin vers le fichier CSV à lire</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <exception cref="ArgumentNullException">Lancée lorsque le chemin du fichier est null ou vide</exception>
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

    /// <summary>
    /// Parse une ligne CSV et crée une commande pour ajouter une sous-préfecture si la ligne est valide
    /// </summary>
    /// <param name="ligne">La ligne CSV à parser (format: District,Region,Département,Sous-préfecture,Groupe,Population)</param>
    /// <returns>Une commande pour ajouter la sous-préfecture, ou null si la ligne n'est pas valide</returns>
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
