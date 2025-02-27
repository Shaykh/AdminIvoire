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
        return new LocaliteDto(entity.Id, entity.Nom, entity.Superficie, entity.Population, entity.CoordonneesGeographiques?.MapToDto());
    }

    public static GetDistrictResponse MapToResponse(this District entity)
    {
        var regions = entity.Regions.Select(region => region.MapToDto());

        return new GetDistrictResponse(entity.Id, entity.Nom, entity.Superficie, entity.Population, regions);
    }

    public static GetRegionResponse MapToResponse(this Region entity)
    {
        var departements = entity.Departements.Select(departement => departement.MapToDto());

        return new GetRegionResponse(entity.Id, entity.Nom, entity.Superficie, entity.Population, departements);
    }

    public static GetDepartementResponse MapToResponse(this Departement entity)
    {
        var sousPrefectures = entity.SousPrefectures.Select(commune => commune.MapToDto());

        return new GetDepartementResponse(entity.Id, entity.Nom, entity.Superficie, entity.Population, sousPrefectures, entity.CoordonneesGeographiques!.MapToDto());
    }

    public static GetSousPrefectureResponse MapToResponse(this SousPrefecture entity)
    {
        var villages = entity.Villages.Select(village => village.MapToDto());

        return new GetSousPrefectureResponse(entity.Id, entity.Nom, entity.Superficie, entity.Population, villages, entity.CoordonneesGeographiques!.MapToDto());
    }
}
