using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Query;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;

namespace AdminIvoire.WebApi.Features;

/// <summary>
/// Module Carter définissant les routes API pour les sous-préfectures
/// </summary>
public class SousPrefectureWebModule : ICarterModule
{
    /// <summary>
    /// Ajoute les routes API pour les sous-préfectures
    /// </summary>
    /// <param name="app">Le constructeur de routes d'endpoints</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/sousprefectures", async (IMediator mediator) =>
        {
            var sousPrefectures = await mediator.Send(new GetAllSousPrefectures.Query());
            return sousPrefectures;
        })
        .WithName("GetAllSousPrefectures")
        .WithTags("SousPrefectures")
        .WithSummary("Récupère toutes les sous-préfectures")
        .WithDescription("Retourne la liste de toutes les sous-préfectures")
        .Produces<IEnumerable<LocaliteDto>>(StatusCodes.Status200OK, "application/json")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère toutes les sous-préfectures",
            Description = "Retourne la liste complète de toutes les sous-préfectures de la Côte d'Ivoire avec leurs informations de base (nom, superficie, population, coordonnées géographiques).",
            Tags = [new() { Name = "SousPrefectures" }],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Liste des sous-préfectures récupérée avec succès",
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

        app.MapGet("/api/sousprefectures/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var sousPrefecture = await mediator.Send(new GetSousPrefectureById.Query(id));
            return sousPrefecture;
        })
        .WithName("GetSousPrefectureById")
        .WithTags("SousPrefectures")
        .WithSummary("Récupère une sous-préfecture par son identifiant")
        .WithDescription("Retourne les informations détaillées d'une sous-préfecture spécifique avec tous ses villages")
        .Produces<GetSousPrefectureResponse>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère une sous-préfecture par son identifiant",
            Description = "Retourne les informations complètes d'une sous-préfecture spécifique identifiée par son GUID, incluant tous ses villages associés, ses coordonnées géographiques et ses informations hiérarchiques (département, région, district).",
            Tags = [new() { Name = "SousPrefectures" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) de la sous-préfecture",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Sous-préfecture trouvée et retournée avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetSousPrefectureResponse" } }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Sous-préfecture non trouvée" }
            }
        });

        app.MapGet("/api/sousprefectures/{id:guid}/villages", async (IMediator mediator, Guid id) =>
        {
            var villages = await mediator.Send(new GetVillagesBySousPrefectureId.Query(id));
            return villages;
        })
        .WithName("GetVillagesBySousPrefectureId")
        .WithTags("SousPrefectures", "Villages")
        .WithSummary("Récupère les villages d'une sous-préfecture")
        .WithDescription("Retourne la liste de tous les villages appartenant à une sous-préfecture spécifique")
        .Produces<IEnumerable<GetVillageResponse>>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère les villages d'une sous-préfecture",
            Description = "Retourne la liste complète de tous les villages appartenant à la sous-préfecture spécifiée, avec leurs informations détaillées incluant leurs coordonnées géographiques et informations hiérarchiques.",
            Tags = [new() { Name = "SousPrefectures" }, new() { Name = "Villages" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) de la sous-préfecture",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
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
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetVillageResponse" } }
                            }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Sous-préfecture non trouvée" }
            }
        });
    }
}
