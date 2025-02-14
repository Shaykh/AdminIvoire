using MediatR;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Interface d'execution d'une query
/// </summary>
/// <typeparam name="TQuery">Generique implementant IQuery</typeparam>
/// <typeparam name="TResult">Generique en sortie d'execution de la query</typeparam>
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
{
}