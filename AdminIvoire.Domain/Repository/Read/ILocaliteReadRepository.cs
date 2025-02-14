using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface ILocaliteReadRepository<T> where T : Localite
{
    Task<IList<T>> GetAll();
    Task<T> GetById(Guid id);
}
