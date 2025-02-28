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
            return regions;
        }).WithName("Get all regions")
            .WithOpenApi();

        app.MapGet("/api/regions/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var region = await mediator.Send(new GetRegionById.Query(id));
            return region;
        }).WithName("Get region by Id")
            .WithOpenApi();

        app.MapGet("/api/regions/{id:guid}/departements", async (IMediator mediator, Guid id) =>
        {
            var departements = await mediator.Send(new GetDepartementsByRegionId.Query(id));
            return departements;
        }).WithName("Get departements by region Id")
            .WithOpenApi();
    }
}
