namespace AdminIvoire.Application.ApiClient;

/// <summary>
/// Exception levée lorsqu'une erreur survient lors d'un appel à une API externe
/// </summary>
public class ApiCallException(Exception exception) : Exception(exception.Message)
{
}
