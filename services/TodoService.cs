using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.shared;
using Microsoft.EntityFrameworkCore;

namespace api.services
{
    public class TodoService : ITodoService
    {

        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }


        //Return without deleted ones
        public async Task<List<ToDo>> GetToDosAsync()
        {
            return await _todoRepository.GetAllAsync();
        }

        //Return with deleted ones
        public async Task<List<ToDo>> GetAllAsyncWithDeletedToDos()
        {
             return await _todoRepository.GetAllAsyncWithDeleted();
        }
        
        //Return ToDo by id
        public async Task<ToDo> GetToDoByIdAsync(string id)
        {
            var getToDo = await _todoRepository.GetToDoByIdAsync(id);

            if(getToDo == null)
            {
                throw new Exception($"ToDo item with ID {id} not found");
            }
            return getToDo;
        }

        //Post ToDo (Add)
        public async Task<ToDo> PostToDoAsync(ToDo newToDo)
        {
            await _todoRepository.PostAsync(newToDo);
            return newToDo;
        }

        //Put ToDo (Update)
        public async Task<ToDo> UpdateToDoAsync(string id, ToDo updatedToDo)
        {  
            if(id != updatedToDo.ToDoID) throw new Exception($"ToDo item with ID {id} do not match the item that wants to update");

            var ExistsToDo = await _todoRepository.GetToDoByIdAsync(id);

            if(ExistsToDo == null) throw new Exception($"ToDo item with ID {id} do not exists");

            ExistsToDo.IstoDoDone = updatedToDo.IstoDoDone;
            ExistsToDo.ToDoContent = updatedToDo.ToDoContent;

            try
            {
                await _todoRepository.UpdateAsync(ExistsToDo.ToDoID,ExistsToDo);
            }
            catch (DbUpdateConcurrencyException)
            {               
                    throw;
            }
            return updatedToDo;          
        }

        //Delete
        public async Task<ToDo> DeleteToDoAsync(string id)
        {            
            var deletedTodo = await _todoRepository.GetToDoByIdAsync(id);

            if(deletedTodo == null)
            {
                throw new Exception("ToDo item not found");
            }

            deletedTodo.IsDeleted = true;

            await _todoRepository.UpdateAsync(deletedTodo.ToDoID,deletedTodo);
            
            return deletedTodo;
        }

        
        
    }
}