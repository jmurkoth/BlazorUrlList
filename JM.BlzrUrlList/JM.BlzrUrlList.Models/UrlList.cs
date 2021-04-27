using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;

namespace JM.BlzrUrlList.Models
{
    public record  UrlList
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; init; }
        public string UserId { get; init; }
        public string UrlId { get; set; }
        public string Description { get; set; }
        public IList<CustomUrl>   Urls { get; set; }
    }
}
