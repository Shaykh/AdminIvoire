using AdminIvoire.Application.Query;
using Carter;
using MediatR;

namespace AdminIvoire.WebApi.Features;

public class SousPrefectureWebModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/sousprefectures", async (IMediator mediator) =>
        {
            var sousPrefectures = await mediator.Send(new GetAllSousPrefectures.Query());
            return sousPrefectures;
        }).WithName("Get all sous-prefectures")
            .WithOpenApi();

        app.MapGet("/api/sousprefectures/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var sousPrefecture = await mediator.Send(new GetSousPrefectureById.Query(id));
            return sousPrefecture;
        }).WithName("Get sous-prefecture by Id")
            .WithOpenApi();

        app.MapGet("/api/sousprefectures/{id:guid}/villages", async (IMediator mediator, Guid id) =>
        {
            var villages = await mediator.Send(new GetVillagesBySousPrefectureId.Query(id));
            return villages;
        }).WithName("Get villages by sous-prefecture Id")
            .WithOpenApi();
    }
}
