namespace AdminIvoire.Infrastructure.Configuration;

/// <summary>
/// Exception levée lorsqu'une erreur de configuration est détectée
/// </summary>
public class ConfigurationException(string message) : Exception(message)
{
}
