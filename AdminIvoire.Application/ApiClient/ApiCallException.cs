namespace AdminIvoire.Application.ApiClient;

public class ApiCallException(Exception exception) : Exception(exception.Message)
{
}
