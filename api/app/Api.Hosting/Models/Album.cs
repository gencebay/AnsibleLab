using MongoDB.Bson.Serialization.Attributes;
using NetCoreStack.Contracts;
using System;

namespace Api.Hosting.Models
{
    [CollectionName("Albums")]
    public class Album : EntityIdentityBson
    {
        public long AlbumId { get; set; }

        public long ArtistId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string AlbumArtUrl { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Created { get; set; }
    }
}
