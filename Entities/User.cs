using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Entities
{
    public class User
    {   
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; } = null!;

        [BsonElement("UserEmail")]
        public string UserEmail { get; set; } = null!;

        [BsonElement("UserPassword")]
        [JsonIgnore]
        public string UserPassword { get; set; } = null!;
        
    }
}