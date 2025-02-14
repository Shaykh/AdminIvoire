using MediatR;

namespace AdminIvoire.Application.Command;
/// <summary>
/// Interface d'execution d'une command
/// </summary>
/// <typeparam name="TCommand">generique implementant ICommand</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
  where TCommand : ICommand
{ }
/// <summary>
/// Interface generique d'execution d'une command
/// </summary>
/// <typeparam name="TResult">Resultat de la command</typeparam>
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
  where TCommand : ICommand<TResult>
{ }
