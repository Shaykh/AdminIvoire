namespace AdminIvoire.Infrastructure.Persistence;

/// <summary>
/// Exception levée lorsqu'une erreur survient lors d'un accès à la base de données
/// </summary>
public class DataAccessException(string message) : Exception(message)
{
}
