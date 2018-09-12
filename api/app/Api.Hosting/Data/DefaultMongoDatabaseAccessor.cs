using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Api.Hosting.Data
{
    public class DefaultMongoDatabaseAccessor : IMongoDatabaseAccessor
    {
        private readonly IOptions<DbSettings> _options;

        public IMongoDatabase Database { get; }

        public DefaultMongoDatabaseAccessor(IOptions<DbSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            var mongoUrl = new MongoUrl(_options.Value.MongoDbConnectionString);
            var client = new MongoClient(mongoUrl);
            Database = client.GetDatabase(mongoUrl.DatabaseName);            
        }
    }
}
