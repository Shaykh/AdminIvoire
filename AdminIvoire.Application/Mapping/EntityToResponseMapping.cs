using AdminIvoire.Application.Dto;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Application.Mapping;

/// <summary>
/// Classe statique contenant les méthodes d'extension pour mapper les entités du domaine vers les DTOs de réponse
/// </summary>
public static class EntityToResponseMapping
{
    /// <summary>
    /// Mappe une entité CoordonneesGeographiques vers un DTO
    /// </summary>
    /// <param name="entity">L'entité CoordonneesGeographiques à mapper</param>
    /// <returns>Le DTO correspondant</returns>
    public static CoordonneesGeographiquesDto MapToDto(this CoordonneesGeographiques entity)
    {
        return new CoordonneesGeographiquesDto(entity.Latitude, entity.Longitude);
    }

    /// <summary>
    /// Mappe une entité Localite vers un DTO
    /// </summary>
    /// <param name="entity">L'entité Localite à mapper</param>
    /// <returns>Le DTO correspondant</returns>
    public static LocaliteDto MapToDto(this Localite entity)
    {
        return new LocaliteDto(entity.Id,
            entity.Nom,
            entity.Superficie,
            entity.Population,
            entity.CoordonneesGeographiques?.MapToDto());
    }

    /// <summary>
    /// Mappe une entité District vers un DTO de réponse avec ses régions
    /// </summary>
    /// <param name="entity">L'entité District à mapper</param>
    /// <returns>Le DTO de réponse correspondant</returns>
    public static GetDistrictResponse MapToResponse(this District entity)
    {
        var regions = entity.Regions.Select(region => region.MapToDto());

        return new GetDistrictResponse(entity.Id, entity.Nom, entity.Superficie, entity.Population, regions);
    }

    /// <summary>
    /// Mappe une entité Region vers un DTO de réponse avec ses départements
    /// </summary>
    /// <param name="entity">L'entité Region à mapper</param>
    /// <returns>Le DTO de réponse correspondant</returns>
    public static GetRegionResponse MapToResponse(this Region entity)
    {
        var departements = entity.Departements.Select(departement => departement.MapToDto());

        return new GetRegionResponse(entity.Id,
            entity.Nom,
            entity.Superficie,
            entity.Population,
            entity.DistrictId,
            entity.District.Nom,
            departements);
    }

    /// <summary>
    /// Mappe une entité Departement vers un DTO de réponse avec ses sous-préfectures
    /// </summary>
    /// <param name="entity">L'entité Departement à mapper</param>
    /// <returns>Le DTO de réponse correspondant</returns>
    public static GetDepartementResponse MapToResponse(this Departement entity)
    {
        var sousPrefectures = entity.SousPrefectures.Select(commune => commune.MapToDto());

        return new GetDepartementResponse(entity.Id,
            entity.Nom,
            entity.Superficie,
            entity.Population,
            entity.RegionId,
            entity.Region.Nom,
            entity.Region.District.Nom,
            sousPrefectures,
            entity.CoordonneesGeographiques!.MapToDto());
    }

    /// <summary>
    /// Mappe une entité SousPrefecture vers un DTO de réponse avec ses villages
    /// </summary>
    /// <param name="entity">L'entité SousPrefecture à mapper</param>
    /// <returns>Le DTO de réponse correspondant</returns>
    public static GetSousPrefectureResponse MapToResponse(this SousPrefecture entity)
    {
        var villages = entity.Villages.Select(village => village.MapToDto());

        return new GetSousPrefectureResponse(entity.Id,
            entity.Nom,
            entity.Superficie,
            entity.Population,
            entity.DepartementId,
            entity.Departement.Nom,
            entity.Departement.Region.Nom,
            entity.Departement.Region.District.Nom,
            villages,
            entity.CoordonneesGeographiques!.MapToDto());
    }

    /// <summary>
    /// Mappe une entité Village vers un DTO de réponse
    /// </summary>
    /// <param name="entity">L'entité Village à mapper</param>
    /// <returns>Le DTO de réponse correspondant</returns>
    public static GetVillageResponse MapToResponse(this Village entity)
    {
        return new GetVillageResponse(entity.Id,
            entity.Nom,
            entity.Superficie,
            entity.Population,
            entity.SousPrefectureId,
            entity.SousPrefecture!.Nom,
            entity.SousPrefecture.Departement.Nom,
            entity.SousPrefecture.Departement.Region.Nom,
            entity.SousPrefecture.Departement.Region.District.Nom,
            entity.CoordonneesGeographiques!.MapToDto());
    }
}
