using MediatR;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Interface d'exécution d'une query
/// </summary>
/// <typeparam name="TQuery">Type générique implémentant IQuery</typeparam>
/// <typeparam name="TResult">Type générique en sortie d'exécution de la query</typeparam>
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
{
}