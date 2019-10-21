using Raven.Client.Documents;

namespace TheatricalEventChargerRepository.DocumentStore
{
    public interface IDocumentStoreHolder
    {
        IDocumentStore Store { get; }
    }
}
