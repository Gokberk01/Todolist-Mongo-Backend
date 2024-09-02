using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using MongoDB.Driver;

namespace api.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration _configuration)
        {
            var client = new MongoClient(_configuration.GetConnectionString("MongoDbConnection"));
            _database = client.GetDatabase("mongoTodoDb");
        }

        public IMongoCollection<ToDo> ToDoList => _database.GetCollection<ToDo>("todoCollection");
    }
}