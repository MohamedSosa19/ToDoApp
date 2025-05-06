using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Enums;
using TodoApp.Core.Models;

namespace TodoApp.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext()
        {
            // This constructor is used for design-time operations like migrations
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<Todo> Todos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);

            modelBuilder.Entity<Todo>()
           .Property(t => t.Status)
           .HasDefaultValue(TodoStatus.Pending);

            modelBuilder.Entity<Todo>()
            .Property(t => t.Priority)
            .HasDefaultValue(PriorityLevel.Medium);

            modelBuilder.Entity<Todo>()
                .Property(t => t.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Todo>()
                .Property(t => t.LastModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
        }
}
