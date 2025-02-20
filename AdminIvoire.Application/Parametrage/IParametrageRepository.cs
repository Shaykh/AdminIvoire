namespace AdminIvoire.Application.Parametrage;

public interface IParametrageRepository
{
    Task<ParametrageEntity?> GetParametrageAsync(string key);
    Task SetParametrageAsync(ParametrageEntity parametrage);
}
