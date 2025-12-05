using MediatR;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Interface présentant une query (CQRS)
/// </summary>
/// <typeparam name="TResult">Le type du résultat retourné par la query</typeparam>
public interface IQuery<out TResult> : IRequest<TResult>
{
}

