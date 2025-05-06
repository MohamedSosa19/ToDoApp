using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOS;
using TodoApp.Core.Enums;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<TodosController> _logger;
        private readonly IMapper _mapper;
        public TodosController(ITodoRepository todoRepository, ILogger<TodosController> logger, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("GetTodos")]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
        {
            try
            {
                var todos = await _todoRepository.GetAllTodosAsync();
                return Ok(_mapper.Map<IEnumerable<TodoDto>>(todos));
            }
            catch (Exception)
            {
                return BadRequest("Error getting all todos");
            }
        }

        [HttpGet("GetTodosByStatus/{status}")]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodosByStatus(TodoStatus status)
        {
            try
            {
                var todos = await _todoRepository.GetTodosByStatusAsync(status);
                return Ok(_mapper.Map<IEnumerable<TodoDto>>(todos));
            }
            catch (Exception )
            {
          
                return BadRequest($"\"Error getting todos by status {{status}}");
            }
        }

        [HttpGet("GetTodoById/{id}")]
        public async Task<ActionResult<TodoDto>> GetTodoById(Guid id)
        {
            try
            {
                var todo = await _todoRepository.GetTodoByIdAsync(id);
                return todo == null ? NotFound() : Ok(_mapper.Map<TodoDto>(todo));
            }
            catch (Exception )
            {
                return BadRequest($"Error getting todo with id {id}");
            }
        }
        [HttpPost("CreateTodo")]
        public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoDto createTodo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var todo = _mapper.Map<Todo>(createTodo);
                var createdTodo = await _todoRepository.CreateTodoAsync(todo);

                return CreatedAtAction(
                    nameof(createTodo),
                    new { id = createdTodo.Id },
                    _mapper.Map<TodoDto>(createdTodo));
            }
            catch (Exception ex)
            {
             
                return BadRequest($"Error Error creating new todo");
            }

        }

        [HttpPut("UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(Guid id,UpdateTodoDto updateTodoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingTodo = await _todoRepository.GetTodoByIdAsync(id);
                if (existingTodo == null)
                    return NotFound();

                _mapper.Map(updateTodoDto, existingTodo);
                await _todoRepository.UpdateTodoAsync(existingTodo);

                return NoContent();
            }
            catch (Exception ex)
            {
             
                return BadRequest($"Error updating todo with id {id}");
            }
        }

        [HttpDelete("DeleteTodo/{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await _todoRepository.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoRepository.DeleteTodoAsync(id);
            return NoContent();
        }

        [HttpPatch("Complete/{id}")]
        public async Task<IActionResult> MarkAsComplete(Guid id)
        {
            var todo = await _todoRepository.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoRepository.MarkAsCompleteAsync(id);
            return BadRequest($"Error marking todo with id {id} as complete");
        }




    }
}
