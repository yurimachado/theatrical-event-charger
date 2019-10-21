using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheatricalEventChargerDomain.Entities;
using TheatricalEventChargerRepository.Abstracts;

namespace TheatricalEventChargerRepository.DocumentStore.Stores
{
    public class TheatricalPlayCatalogDocumentStore : DocumentStoreBase, IReadOnlyRepository<string, TheatricalPlayCatalogItem>
    {
        public TheatricalPlayCatalogDocumentStore(IDocumentStoreHolder context) : base(context)
        {
        }

        public TheatricalPlayCatalogItem FindByKey(string key) => Session.Query<TheatricalPlayCatalogItem>().Where(o => o.Play == key).FirstOrDefault();

        public IList<TheatricalPlayCatalogItem> Query(Expression<Func<TheatricalPlayCatalogItem, bool>> expression) => Session.Query<TheatricalPlayCatalogItem>().Where(expression).ToList();

        public TheatricalPlayCatalogItem Remove(TheatricalPlayCatalogItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Session.Delete(entity.Play);

            SaveChanges();

            return entity;
        }

        public TheatricalPlayCatalogItem Update(TheatricalPlayCatalogItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var stored = Session.Load<TheatricalPlayCatalogItem>(entity.Play);
            var etag = Session.Advanced.GetChangeVectorFor(stored);

            Session.Store(entity, etag);

            SaveChanges();

            return entity;
        }
    }
}
