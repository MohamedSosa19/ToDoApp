using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Enums;
using TodoApp.Core.Models;

namespace TodoApp.Core.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<IEnumerable<Todo>> GetTodosByStatusAsync(TodoStatus status);
        Task<Todo?> GetTodoByIdAsync(Guid id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task UpdateTodoAsync(Todo todo);
        Task DeleteTodoAsync(Guid id);
        Task  MarkAsCompleteAsync(Guid id);
    }
}
