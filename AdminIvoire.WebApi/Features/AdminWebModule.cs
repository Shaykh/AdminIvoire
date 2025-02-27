using AdminIvoire.Application.Command;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminIvoire.WebApi.Features;

public class AdminWebModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/sousprefectures/{sousPrefectureId:guid}/villages", async (Guid sousPrefectureId, [FromBody]IEnumerable<string> villages,
            ISender sender) =>
        {
            var command = new AjoutVillagesDeSousPrefecture.Command(sousPrefectureId, [.. villages]);
            
            await sender.Send(command);

            return Results.Ok("Les villages ont été ajoutés avec succès.");
        })
            .WithName("AjoutVillagesDeSousPrefecture")
            .WithOpenApi();
    }
}
