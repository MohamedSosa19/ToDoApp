using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Enums;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            var todo = await GetTodoByIdAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(Guid id)
        {
            var todo= await _context.Todos.FindAsync(id);
            return todo;
        }

        public async Task<IEnumerable<Todo>> GetTodosByStatusAsync(TodoStatus status)
        {
            return await _context.Todos.Where(t=>t.Status==status).ToListAsync();
        }

        public async Task MarkAsCompleteAsync(Guid id)
        {
            var todo = await GetTodoByIdAsync(id);
            if (todo != null)
            {
                todo.Status = TodoStatus.Completed;
                todo.LastModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        

        public async Task UpdateTodoAsync(Todo todo)
        {
            //todo.LastModifiedDate = DateTime.UtcNow;
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
