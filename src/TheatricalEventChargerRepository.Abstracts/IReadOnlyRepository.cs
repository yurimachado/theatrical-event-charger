using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TheatricalEventChargerRepository.Abstracts
{
    public interface IReadOnlyRepository<TKey, TEntity>
    {
        IList<TEntity> Query(Expression<Func<TEntity, bool>> expression);

        TEntity FindByKey(TKey key);
    }
}
