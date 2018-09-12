using MongoDB.Driver;

namespace Api.Hosting.Data
{
    public interface IMongoDatabaseAccessor
    {
        IMongoDatabase Database { get; }
    }
}
