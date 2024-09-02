using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Entities
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("Token")]
        public string Token { get; set; } = null!;

        [BsonElement("UserId")]
        public string UserId { get; set; } = null!;

        [BsonElement("Created")]
        public DateTime Created { get; set; }

        [BsonElement("Expires")]
        public DateTime Expires { get; set; }

        [BsonElement("Revoked")]
        public DateTime? Revoked { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}