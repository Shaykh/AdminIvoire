using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Query;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;

namespace AdminIvoire.WebApi.Features;

/// <summary>
/// Module Carter définissant les routes API pour les régions
/// </summary>
public class RegionWebModule : ICarterModule
{
    /// <summary>
    /// Ajoute les routes API pour les régions
    /// </summary>
    /// <param name="app">Le constructeur de routes d'endpoints</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/regions", async (IMediator mediator) =>
        {
            var regions = await mediator.Send(new GetAllRegions.Query());
            return regions;
        })
        .WithName("GetAllRegions")
        .WithTags("Regions")
        .WithSummary("Récupère toutes les régions")
        .WithDescription("Retourne la liste de toutes les régions avec leurs départements associés")
        .Produces<IEnumerable<GetRegionResponse>>(StatusCodes.Status200OK, "application/json")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère toutes les régions",
            Description = "Retourne la liste complète de toutes les régions de la Côte d'Ivoire avec leurs départements associés. Chaque région contient ses informations de base ainsi que la liste de ses départements.",
            Tags = [new() { Name = "Regions" }],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Liste des régions récupérée avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetRegionResponse" } }
                            }
                        }
                    }
                }
            }
        });

        app.MapGet("/api/regions/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var region = await mediator.Send(new GetRegionById.Query(id));
            return region;
        })
        .WithName("GetRegionById")
        .WithTags("Regions")
        .WithSummary("Récupère une région par son identifiant")
        .WithDescription("Retourne les informations détaillées d'une région spécifique avec tous ses départements")
        .Produces<GetRegionResponse>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère une région par son identifiant",
            Description = "Retourne les informations complètes d'une région spécifique identifiée par son GUID, incluant tous ses départements associés avec leurs informations de base.",
            Tags = [new() { Name = "Regions" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) de la région",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Région trouvée et retournée avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetRegionResponse" } }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Région non trouvée" }
            }
        });

        app.MapGet("/api/regions/{id:guid}/departements", async (IMediator mediator, Guid id) =>
        {
            var departements = await mediator.Send(new GetDepartementsByRegionId.Query(id));
            return departements;
        })
        .WithName("GetDepartementsByRegionId")
        .WithTags("Regions", "Departements")
        .WithSummary("Récupère les départements d'une région")
        .WithDescription("Retourne la liste de tous les départements appartenant à une région spécifique")
        .Produces<IEnumerable<GetDepartementResponse>>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère les départements d'une région",
            Description = "Retourne la liste complète de tous les départements appartenant à la région spécifiée, avec leurs informations détaillées incluant leurs sous-préfectures et coordonnées géographiques.",
            Tags = [new() { Name = "Regions" }, new() { Name = "Departements" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) de la région",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Liste des départements récupérée avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetDepartementResponse" } }
                            }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Région non trouvée" }
            }
        });
    }
}
