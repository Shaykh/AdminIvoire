using MediatR;

namespace AdminIvoire.Application.Query;

/// <summary>
/// Interface presentant une query (CQRS)
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IQuery<out TResult> : IRequest<TResult>
{
}

