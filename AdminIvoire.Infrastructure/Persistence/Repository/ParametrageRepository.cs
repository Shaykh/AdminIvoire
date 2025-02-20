using AdminIvoire.Application.Parametrage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class ParametrageRepository(ILogger<ParametrageRepository> logger,
    LocaliteContext dbContext) : IParametrageRepository
{
    readonly DbSet<ParametrageEntity> _dbSet = dbContext.Set<ParametrageEntity>();

    public async Task<ParametrageEntity?> GetParametrageAsync(string key)
    {
        logger.LogInformation("Recherche du parametrage {Key}", key);
        return await _dbSet.FirstOrDefaultAsync(p => p.Key == key);
    }

    public async Task SetParametrageAsync(ParametrageEntity parametrage)
    {
        logger.LogInformation("Enregistrement du parametrage {Key} {Value}", parametrage.Key, parametrage.Value);
        await _dbSet.AddAsync(parametrage);
        await dbContext.SaveChangesAsync();
    }
}
