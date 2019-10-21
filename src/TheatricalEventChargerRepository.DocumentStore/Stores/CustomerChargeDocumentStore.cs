using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TheatricalEventChargerDomain.Entities;
using TheatricalEventChargerRepository.Abstracts;

namespace TheatricalEventChargerRepository.DocumentStore.Stores
{
    public class CustomerChargeDocumentStore : DocumentStoreBase, IRepository<string, CustomerCharge>
    {
        public CustomerChargeDocumentStore(IDocumentStoreHolder context) : base(context)
        {
        }

        public CustomerCharge Add(CustomerCharge entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Session.Store(entity);
            SaveChanges();

            return entity;
        }

        public CustomerCharge FindByKey(string key) => Session.Query<CustomerCharge>().Where(o => o.ChargeId == key).FirstOrDefault();

        public IList<CustomerCharge> Query(Expression<Func<CustomerCharge, bool>> expression) => Session.Query<CustomerCharge>().Where(expression).ToList();

        public CustomerCharge Remove(CustomerCharge entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Session.Delete(entity.ChargeId);

            SaveChanges();

            return entity;
        }

        public CustomerCharge Update(CustomerCharge entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var stored = Session.Load<CustomerCharge>(entity.ChargeId);
            var etag = Session.Advanced.GetChangeVectorFor(stored);

            Session.Store(entity, etag);

            SaveChanges();

            return entity;
        }
    }
}
