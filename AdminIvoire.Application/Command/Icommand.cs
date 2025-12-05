using MediatR;

namespace AdminIvoire.Application.Command;

/// <summary>
/// Interface présentant une commande (CQRS)
/// </summary>
public interface ICommand : IRequest
{
}
/// <summary>
/// Interface générique présentant une commande (CQRS) avec résultat
/// </summary>
/// <typeparam name="TResult">Le type du résultat retourné par la commande</typeparam>
public interface ICommand<out TResult> : IRequest<TResult>
{
}
