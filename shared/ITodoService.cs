using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.shared
{
    public interface ITodoService
    {
        Task<List<ToDo>> GetToDosAsync();
        Task<List<ToDo>> GetAllAsyncWithDeletedToDos();
        Task<ToDo> GetToDoByIdAsync(string id);
        Task<ToDo> PostToDoAsync(ToDo newToDo);
        Task<ToDo> UpdateToDoAsync(string id, ToDo updatedToDo);
        Task<ToDo> DeleteToDoAsync(string id);
    }
}