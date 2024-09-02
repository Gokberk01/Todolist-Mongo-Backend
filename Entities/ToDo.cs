using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Entities
{
    public class ToDo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ToDoID { get; set; }

        [BsonElement("ToDoContent")]
        public string ToDoContent { get; set; }

        [BsonElement("IstoDoDone")]
        public bool IstoDoDone { get; set; }

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}