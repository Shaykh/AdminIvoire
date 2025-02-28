using AdminIvoire.Application.Dto;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Application.Mapping;

public static class EntityToResponseMapping
{
    public static CoordonneesGeographiquesDto MapToDto(this CoordonneesGeographiques entity)
    {
        return new CoordonneesGeographiquesDto(entity.Latitude, entity.Longitude);
    }

    public static LocaliteDto MapToDto(this Localite entity)
    {
        return new LocaliteDto(entity.Id, 
            entity.Nom, 
            entity.Superficie, 
            entity.Population, 
            entity.CoordonneesGeographiques?.MapToDto());
    }

    public static GetDistrictResponse MapToResponse(this District entity)
    {
        var regions = entity.Regions.Select(region => region.MapToDto());

        return new GetDistrictResponse(entity.Id, entity.Nom, entity.Superficie, entity.Population, regions);
    }

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
