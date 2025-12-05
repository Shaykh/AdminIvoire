using AdminIvoire.Application.Command;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AdminIvoire.WebApi.Features;

/// <summary>
/// Module Carter définissant les routes API d'administration
/// </summary>
public class AdminWebModule : ICarterModule
{
    /// <summary>
    /// Ajoute les routes API d'administration
    /// </summary>
    /// <param name="app">Le constructeur de routes d'endpoints</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/sousprefectures/{sousPrefectureId:guid}/villages", async (Guid sousPrefectureId, [FromBody] IEnumerable<string> villages,
            ISender sender) =>
        {
            var command = new AjoutVillagesDeSousPrefecture.Command(sousPrefectureId, [.. villages]);

            await sender.Send(command);

            return Results.Ok("Les villages ont été ajoutés avec succès.");
        })
        .WithName("AjoutVillagesDeSousPrefecture")
        .WithTags("Admin")
        .RequireAuthorization()
        .WithSummary("Ajoute des villages à une sous-préfecture")
        .WithDescription("Ajoute une liste de villages à une sous-préfecture existante. Les coordonnées géographiques seront automatiquement récupérées pour chaque village.")
        .Accepts<IEnumerable<string>>("application/json")
        .Produces<string>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Ajoute des villages à une sous-préfecture",
            Description = "Permet d'ajouter une liste de villages à une sous-préfecture existante. Chaque village sera créé avec ses coordonnées géographiques récupérées automatiquement via l'API de géocodage. L'opération nécessite une authentification.",
            Tags = [new() { Name = "Admin" }],
            Parameters =
            [
                new()
                {
                    Name = "sousPrefectureId",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "L'identifiant unique (GUID) de la sous-préfecture à laquelle ajouter les villages",
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
                }
            ],
            RequestBody = new OpenApiRequestBody
            {
                Required = true,
                Description = "La liste des noms de villages à ajouter",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "array",
                            Items = new OpenApiSchema { Type = "string" },
                            Example = new Microsoft.OpenApi.Any.OpenApiArray
                            {
                                new Microsoft.OpenApi.Any.OpenApiString("Village 1"),
                                new Microsoft.OpenApi.Any.OpenApiString("Village 2")
                            }
                        }
                    }
                }
            },
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Les villages ont été ajoutés avec succès",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema { Type = "string" },
                            Example = new Microsoft.OpenApi.Any.OpenApiString("Les villages ont été ajoutés avec succès.")
                        }
                    }
                },
                ["400"] = new OpenApiResponse { Description = "Requête invalide (validation échouée ou liste de villages vide)" },
                ["401"] = new OpenApiResponse { Description = "Non authentifié - Un token JWT valide est requis" },
                ["404"] = new OpenApiResponse { Description = "Sous-préfecture non trouvée" }
            }
        });
    }
}
