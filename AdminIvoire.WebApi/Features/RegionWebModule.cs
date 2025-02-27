using AdminIvoire.Application.Query;
using Carter;
using MediatR;

namespace AdminIvoire.WebApi.Features;

public class RegionWebModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/regions", async (IMediator mediator) =>
        {
            var regions = await mediator.Send(new GetAllRegions.Query());
            return Results.Ok(regions);
        }).WithName("Get all regions")
            .WithOpenApi();

        app.MapGet("/api/regions/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var region = await mediator.Send(new GetRegionById.Query(id));
            return Results.Ok(region);
        }).WithName("Get region by Id").WithOpenApi();
    }
}
