using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Domain.Entite;

/// <summary>
/// Classe abstraite de base représentant une localité administrative de la Côte d'Ivoire
/// </summary>
public abstract class Localite
{
    /// <summary>
    /// L'identifiant unique de la localité
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Le nom de la localité
    /// </summary>
    public required string Nom { get; set; }
    /// <summary>
    /// Le code administratif de la localité (optionnel)
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// La superficie en kilomètres carrés
    /// </summary>
    public decimal Superficie { get; set; }
    /// <summary>
    /// Le nombre d'habitants
    /// </summary>
    public int Population { get; set; }
    /// <summary>
    /// Les coordonnées géographiques (latitude, longitude) de la localité
    /// </summary>
    public CoordonneesGeographiques? CoordonneesGeographiques { get; set; }
}
