using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using TheatricalEventChargerDomain.Entities;

namespace TheatricalEventChargerRepository.DocumentStore
{
    public class DocumentStoreHolder : IDocumentStoreHolder
    {
        private ILogger<IDocumentStoreHolder> _logger;
        private IOptions<RavenSettings> _options;
        public IDocumentStore Store { get; }

        public DocumentStoreHolder(IOptions<RavenSettings> options, IDocumentStore store, ILogger<IDocumentStoreHolder> logger)
        {
            _logger = logger;
            _options = options;
            var settings = options.Value;
            Store = store;

            Store.Initialize();

            _logger.LogInformation($"Initialized RavenDB document store for {settings.Database} at {settings.Url}");

            EnsureDatabaseExists(this.Store, this.Store.Database);
        }

        public void EnsureDatabaseExists(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                this._logger.LogInformation("RavenDB database does not exist");
                if (createDatabaseIfNotExists == false)
                {
                    this._logger.LogInformation("Database creation disabled");
                    throw;
                }

                try
                {
                    this._logger.LogInformation("Creating and seeding with initial data.");
                    var createResult = Store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));

                    if (createResult.Name != null)
                    {
                        var hamlet = new TheatricalPlayCatalogItem
                        {
                            Play = "hamlet",
                            KindOfPlay = "tragedy",
                            Price = 40000
                        };

                        var asLike = new TheatricalPlayCatalogItem
                        {
                            Play = "as-like",
                            KindOfPlay = "comedy",
                            Price = 30000
                        };

                        var othelo = new TheatricalPlayCatalogItem
                        {
                            Play = "othelo",
                            KindOfPlay = "tragedy",
                            Price = 40000
                        };

                        var performances = new List<TheatricalPlayCatalogItem>() { hamlet, asLike, othelo };

                        using (var session = Store.OpenSession())
                        {
                            foreach (var performance in performances)
                            {
                                session.Store(performance);
                            }
                            session.SaveChanges();
                        }
                    }
                }
                catch (ConcurrencyException)
                {
                    // The database was already created before calling CreateDatabaseOperation
                }

            }
        }
    }
}
