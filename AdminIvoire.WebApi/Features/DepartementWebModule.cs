using AdminIvoire.Application.Query;
using Carter;
using MediatR;

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
        }).WithName("Get all departements")
            .WithOpenApi();

        app.MapGet("/api/departements/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var departement = await mediator.Send(new GetDepartementById.Query(id));
            return departement;
        }).WithName("Get departement by Id")
            .WithOpenApi();

        app.MapGet("/api/departements/{id:guid}/sousprefectures", async (IMediator mediator, Guid id) =>
        {
            var sousPrefectures = await mediator.Send(new GetSousPrefecturesByDepartementId.Query(id));
            return sousPrefectures;
        }).WithName("Get sous-prefecture by departement Id")
            .WithOpenApi();
    }
}
