using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.shared;
using api.shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("api/[controller]")]  //Burda "api/[controller]" derken controllerdan kastı controller sınıfının
    [ApiController] //controller yazısı atılmış hali yani sınıfım TodoController o yüzden
//    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDoList ()
        {
           var ToDoList = await _todoService.GetToDosAsync();
           return Ok(ToDoList);
        }

        //Get api/todo/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDobyId(string id)
        {
            
            try
            {
                var _ToDo = await _todoService.GetToDoByIdAsync(id);
                return Ok(_ToDo);
            }
            catch (Exception ex)
            {
                
                return NotFound(new {message = ex.Message});
            }
        }

        

        // POST: api/todo
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostTodoItem(TodoDto todoDto)
        {
            var todoItem = new ToDo 
            {
                ToDoContent = todoDto.context,
                IstoDoDone = todoDto.IstoDoDone,
                IsDeleted = todoDto.IsDeleted
            };
            
           var addedToDo = await _todoService.PostToDoAsync(todoItem);
           return Created("success", addedToDo);

        }

        //Update api/todo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> UpdateToDoList(string id, ToDo Updatedtodo)
        {
            try
            {
                var updatedTodo = await _todoService.UpdateToDoAsync(id , Updatedtodo);
                return Ok(updatedTodo);
            }
            catch (Exception ex)
            {
                
                return NotFound(new {message = ex.Message});
            }

        }

        //Delete api/todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(string id)
        {

            try
            {
                var deletedTodo = await _todoService.DeleteToDoAsync(id);
                return Ok(deletedTodo);
            }
            catch (Exception ex)
            {
                
                return NotFound(new {message = ex.Message});
            }
        }
        
    }
}