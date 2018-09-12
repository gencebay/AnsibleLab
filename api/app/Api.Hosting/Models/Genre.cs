using NetCoreStack.Contracts;

namespace Api.Hosting.Models
{
    [CollectionName("Genres")]
    public class Genre : EntityIdentityBson
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
