using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TheatricalEventChargerRepository.Abstracts
{
    public interface IRepository<TKey, TEntity>
    {
        IList<TEntity> Query(Expression<Func<TEntity, bool>> expression);

        TEntity FindByKey(TKey key);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Remove(TEntity entity);
    }
}
