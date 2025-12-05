using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Query;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;

namespace AdminIvoire.WebApi.Features;

/// <summary>
/// Module Carter définissant les routes API pour les districts
/// </summary>
public class DistrictWebModule : ICarterModule
{
    /// <summary>
    /// Ajoute les routes API pour les districts
    /// </summary>
    /// <param name="app">Le constructeur de routes d'endpoints</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/districts", async (IMediator mediator) =>
        {
            var districts = await mediator.Send(new GetAllDistricts.Query());

            return districts;
        })
        .WithName("GetAllDistricts")
        .WithTags("Districts")
        .WithSummary("Récupère tous les districts")
        .WithDescription("Retourne la liste de tous les districts avec leurs régions associées")
        .Produces<IEnumerable<GetDistrictResponse>>(StatusCodes.Status200OK, "application/json")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère tous les districts",
            Description = "Retourne la liste complète de tous les districts de la Côte d'Ivoire avec leurs régions associées. Chaque district contient ses informations de base (nom, superficie, population) ainsi que la liste de ses régions.",
            Tags = [new() { Name = "Districts" }],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Liste des districts récupérée avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetDistrictResponse" } }
                            }
                        }
                    }
                }
            }
        });

        app.MapGet("/api/districts/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var district = await mediator.Send(new GetDistrictById.Query(id));
            return district;
        })
        .WithName("GetDistrictById")
        .WithTags("Districts")
        .WithSummary("Récupère un district par son identifiant")
        .WithDescription("Retourne les informations détaillées d'un district spécifique avec toutes ses régions")
        .Produces<GetDistrictResponse>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère un district par son identifiant",
            Description = "Retourne les informations complètes d'un district spécifique identifié par son GUID, incluant toutes ses régions associées avec leurs informations de base.",
            Tags = [new() { Name = "Districts" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) du district",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "District trouvé et retourné avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetDistrictResponse" } }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "District non trouvé" }
            }
        });

        app.MapGet("/api/districts/{id:guid}/regions", async (IMediator mediator, Guid id) =>
        {
            var regions = await mediator.Send(new GetRegionsByDistrictId.Query(id));
            return regions;
        })
        .WithName("GetRegionsByDistrictId")
        .WithTags("Districts", "Regions")
        .WithSummary("Récupère les régions d'un district")
        .WithDescription("Retourne la liste de toutes les régions appartenant à un district spécifique")
        .Produces<IEnumerable<GetRegionResponse>>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère les régions d'un district",
            Description = "Retourne la liste complète de toutes les régions appartenant au district spécifié, avec leurs informations détaillées incluant leurs départements.",
            Tags = [new() { Name = "Districts" }, new() { Name = "Regions" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) du district",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
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
                },
                ["404"] = new OpenApiResponse { Description = "District non trouvé" }
            }
        });
    }
}
