using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.shared
{
    public interface ITodoRepository
    {
        Task<List<ToDo>> GetAllAsync();
        Task<List<ToDo>> GetAllAsyncWithDeleted();
        Task<ToDo> GetToDoByIdAsync(string id);
        Task PostAsync(ToDo todo);
        Task UpdateAsync(string id, ToDo todo);
        Task DeleteAsync(string id);
    }
}