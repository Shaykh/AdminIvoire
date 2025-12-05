using MediatR;

namespace AdminIvoire.Application.Command;
/// <summary>
/// Interface d'exécution d'une commande
/// </summary>
/// <typeparam name="TCommand">Type générique implémentant ICommand</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
  where TCommand : ICommand
{ }
/// <summary>
/// Interface générique d'exécution d'une commande avec résultat
/// </summary>
/// <typeparam name="TCommand">Type générique implémentant ICommand</typeparam>
/// <typeparam name="TResult">Type du résultat de la commande</typeparam>
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
  where TCommand : ICommand<TResult>
{ }
