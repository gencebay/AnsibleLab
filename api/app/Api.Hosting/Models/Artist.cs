using NetCoreStack.Contracts;

namespace Api.Hosting.Models
{
    [CollectionName("Artists")]
    public class Artist : EntityIdentityBson
    {
        public string Name { get; set; }
    }
}
