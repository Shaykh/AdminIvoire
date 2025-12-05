namespace AdminIvoire.Application.Parametrage;

/// <summary>
/// Interface pour gérer les paramètres de configuration de l'application
/// </summary>
public interface IParametrageRepository
{
    /// <summary>
    /// Récupère un paramètre par sa clé
    /// </summary>
    /// <param name="key">La clé du paramètre à récupérer</param>
    /// <returns>L'entité ParametrageEntity correspondante, ou null si non trouvée</returns>
    Task<ParametrageEntity?> GetParametrageAsync(string key);
    /// <summary>
    /// Définit ou met à jour un paramètre
    /// </summary>
    /// <param name="parametrage">L'entité ParametrageEntity contenant la clé et la valeur</param>
    Task SetParametrageAsync(ParametrageEntity parametrage);
}
