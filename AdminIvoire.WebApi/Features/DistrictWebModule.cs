using AdminIvoire.Application.Query;
using Carter;
using MediatR;

namespace AdminIvoire.WebApi.Features;

public class DistrictWebModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/districts", async (IMediator mediator) =>
        {
            var districts = await mediator.Send(new GetAllDistricts.Query());

            return districts;
        }).WithName("Get all districts")
            .WithOpenApi();

        app.MapGet("/api/districts/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var district = await mediator.Send(new GetDistrictById.Query(id));
            return district;
        }).WithName("Get district by Id")
            .WithOpenApi();

        app.MapGet("/api/districts/{id:guid}/regions", async (IMediator mediator, Guid id) =>
        {
            var regions = await mediator.Send(new GetRegionsByDistrictId.Query(id));
            return regions;
        }).WithName("Get regions by district Id")
            .WithOpenApi();
    }
}
