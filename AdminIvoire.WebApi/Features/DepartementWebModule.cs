using AdminIvoire.Application.Dto;
using AdminIvoire.Application.Query;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;

namespace AdminIvoire.WebApi.Features;

/// <summary>
/// Module Carter définissant les routes API pour les départements
/// </summary>
public class DepartementWebModule : ICarterModule
{
    /// <summary>
    /// Ajoute les routes API pour les départements
    /// </summary>
    /// <param name="app">Le constructeur de routes d'endpoints</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/departements", async (IMediator mediator) =>
        {
            var departements = await mediator.Send(new GetAllDepartements.Query());
            return departements;
        })
        .WithName("GetAllDepartements")
        .WithTags("Departements")
        .WithSummary("Récupère tous les départements")
        .WithDescription("Retourne la liste de tous les départements")
        .Produces<IEnumerable<LocaliteDto>>(StatusCodes.Status200OK, "application/json")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère tous les départements",
            Description = "Retourne la liste complète de tous les départements de la Côte d'Ivoire avec leurs informations de base (nom, superficie, population, coordonnées géographiques).",
            Tags = [new() { Name = "Departements" }],
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
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "LocaliteDto" } }
                            }
                        }
                    }
                }
            }
        });

        app.MapGet("/api/departements/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var departement = await mediator.Send(new GetDepartementById.Query(id));
            return departement;
        })
        .WithName("GetDepartementById")
        .WithTags("Departements")
        .WithSummary("Récupère un département par son identifiant")
        .WithDescription("Retourne les informations détaillées d'un département spécifique avec toutes ses sous-préfectures")
        .Produces<GetDepartementResponse>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère un département par son identifiant",
            Description = "Retourne les informations complètes d'un département spécifique identifié par son GUID, incluant toutes ses sous-préfectures associées, ses coordonnées géographiques et ses informations hiérarchiques (région, district).",
            Tags = [new() { Name = "Departements" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) du département",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Département trouvé et retourné avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetDepartementResponse" } }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Département non trouvé" }
            }
        });

        app.MapGet("/api/departements/{id:guid}/sousprefectures", async (IMediator mediator, Guid id) =>
        {
            var sousPrefectures = await mediator.Send(new GetSousPrefecturesByDepartementId.Query(id));
            return sousPrefectures;
        })
        .WithName("GetSousPrefecturesByDepartementId")
        .WithTags("Departements", "SousPrefectures")
        .WithSummary("Récupère les sous-préfectures d'un département")
        .WithDescription("Retourne la liste de toutes les sous-préfectures appartenant à un département spécifique")
        .Produces<IEnumerable<GetSousPrefectureResponse>>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Récupère les sous-préfectures d'un département",
            Description = "Retourne la liste complète de toutes les sous-préfectures appartenant au département spécifié, avec leurs informations détaillées incluant leurs villages et coordonnées géographiques.",
            Tags = [new() { Name = "Departements" }, new() { Name = "SousPrefectures" }],
            Parameters =
            [
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) du département",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
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
                                Items = new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "GetSousPrefectureResponse" } }
                            }
                        }
                    }
                },
                ["404"] = new OpenApiResponse { Description = "Département non trouvé" }
            }
        });
    }
}
