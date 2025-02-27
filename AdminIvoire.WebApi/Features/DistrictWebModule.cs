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
        });
    }
}
