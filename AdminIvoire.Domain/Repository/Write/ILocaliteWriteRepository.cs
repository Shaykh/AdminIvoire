﻿using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Write;

public interface ILocaliteWriteRepository<T> where T : Localite
{
    Task<T> Create(T localite);
    Task<T> Update(T localite);
    Task Delete(Guid id);
}