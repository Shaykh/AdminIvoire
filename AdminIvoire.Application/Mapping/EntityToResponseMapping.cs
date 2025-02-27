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
}
