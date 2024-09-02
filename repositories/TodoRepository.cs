using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.shared;
using MongoDB.Driver;

namespace api.repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<ToDo> _todoCollection;

        public TodoRepository(IMongoDatabase database)
        {
            _todoCollection = database.GetCollection<ToDo>("TodoItems");
        }
        
        //Get all ToDos that IsDeleted is false
        public async Task<List<ToDo>> GetAllAsync()
        {
            return await _todoCollection.Find(x => x.IsDeleted == false).ToListAsync();
        }

        //Get all ToDos including Deleted ones
        public async Task<List<ToDo>> GetAllAsyncWithDeleted()
        {
             return await _todoCollection.Find(_ => true).ToListAsync();    
        }

        //Get ToDo by id
        public async Task<ToDo> GetToDoByIdAsync(string id)
        {
            return await _todoCollection.Find(x => x.ToDoID == id).FirstOrDefaultAsync();
        }

        //Post ToDo (Add ToDo to the DB)
        public async Task PostAsync(ToDo todo) 
        {
            await _todoCollection.InsertOneAsync(todo);
        }

        //Put ToDo (Update ToDo)
        public async Task UpdateAsync(string id, ToDo todo)
        {
            await _todoCollection.ReplaceOneAsync(i => i.ToDoID == id, todo);    
        }

        //Delete ToDo
        public async Task DeleteAsync(string id)
        {
            await _todoCollection.DeleteOneAsync(item => item.ToDoID == id);
        }




    }
}