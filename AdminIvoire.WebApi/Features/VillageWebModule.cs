using AdminIvoire.Application.Query;
using Carter;
using MediatR;

namespace AdminIvoire.WebApi.Features;

public class VillageWebModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    { 
        app.MapGet("/api/villages", async (IMediator mediator) =>
        {
            var villages = await mediator.Send(new GetAllVillages.Query());
            return villages;
        }).WithName("Get all villages")
            .WithOpenApi();

        app.MapGet("/api/villages/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var village = await mediator.Send(new GetVillageById.Query(id));
            return village;
        }).WithName("Get village by Id")
            .WithOpenApi();
    }
}
