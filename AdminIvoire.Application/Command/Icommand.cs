using MediatR;

namespace AdminIvoire.Application.Command;

/// <summary>
/// Interface presentant une command (CQRS)
/// </summary>
public interface ICommand : IRequest
{
}
/// <summary>
/// Interface generique presentant une command (CQRS)
/// </summary>
public interface ICommand<out TResult> : IRequest<TResult>
{
}
