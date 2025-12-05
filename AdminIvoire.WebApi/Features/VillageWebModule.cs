using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Query;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;

namespace AdminIvoire.WebApi.Features;

/// <summary>
/// Module Carter définissant les routes API pour les villages
/// </summary>
public class VillageWebModule : ICarterModule
{
    /// <summary>
    /// Ajoute les routes API pour les villages
    /// </summary>
    /// <param name="app">Le constructeur de routes d'endpoints</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/villages", async (IMediator mediator) =>
        {
            var villages = await mediator.Send(new GetAllVillages.Query());
            return villages;
        })
        .WithName("GetAllVillages")
        .WithTags("Villages")
        .WithSummary("Récupère tous les villages")
        .WithDescription("Retourne la liste de tous les villages")
        .Produces<IEnumerable<LocaliteDto>>(StatusCodes.Status200OK, "application/json")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère tous les villages",
            Description = "Retourne la liste complète de tous les villages de la Côte d'Ivoire avec leurs informations de base (nom, superficie, population, coordonnées géographiques).",
            Tags = [new() { Name = "Villages" }],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Liste des villages récupérée avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "LocaliteDto" } }
                            }
                        }
                    }
                }
            }
        });

        app.MapGet("/api/villages/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var village = await mediator.Send(new GetVillageById.Query(id));
            return village;
        })
        .WithName("GetVillageById")
        .WithTags("Villages")
        .WithSummary("Récupère un village par son identifiant")
        .WithDescription("Retourne les informations détaillées d'un village spécifique")
        .Produces<GetVillageResponse>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère un village par son identifiant",
            Description = "Retourne les informations complètes d'un village spécifique identifié par son GUID, incluant ses coordonnées géographiques et toutes ses informations hiérarchiques (sous-préfecture, département, région, district).",
            Tags = [new() { Name = "Villages" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) du village",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Village trouvé et retourné avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetVillageResponse" } }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Village non trouvé" }
            }
        });
    }
}
