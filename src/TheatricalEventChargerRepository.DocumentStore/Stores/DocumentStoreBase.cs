using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace TheatricalEventChargerRepository.DocumentStore.Stores
{
    public abstract class DocumentStoreBase
    {
        public IDocumentStore Context { get; }

        private readonly Lazy<IDocumentSession> _session;

        public IDocumentSession Session => _session.Value;

        public DocumentStoreBase(IDocumentStoreHolder context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Context = context.Store;

            _session = new Lazy<IDocumentSession>(() =>
            {
                var session = Context.OpenSession();
                session.Advanced.UseOptimisticConcurrency = true;
                return session;
            }, true);
        }

        public void SaveChanges() => Session.SaveChanges();
    }
}
