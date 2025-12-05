namespace AdminIvoire.Application.Parametrage;

/// <summary>
/// Entité représentant un paramètre de configuration de l'application
/// </summary>
public class ParametrageEntity
{
    /// <summary>
    /// La clé du paramètre
    /// </summary>
    public required string Key { get; set; }
    /// <summary>
    /// La valeur du paramètre
    /// </summary>
    public required string Value { get; set; }
}
