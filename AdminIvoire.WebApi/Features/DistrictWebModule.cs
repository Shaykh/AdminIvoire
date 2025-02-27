using AdminIvoire.Application.Query;
using Carter;
using MediatR;

namespace AdminIvoire.WebApi.Features;

public class DistrictWebModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/districts", async (IMediator mediator) =>
        {
            var districts = await mediator.Send(new GetAllDistricts.Query());

            return Results.Ok(districts);
        }).WithName("Get all districts")
            .WithOpenApi();

        app.MapGet("/api/districts/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var district = await mediator.Send(new GetDistrictById.Query(id));
            return Results.Ok(district);
        }).WithName("Get district by Id").WithOpenApi();
    }
}
